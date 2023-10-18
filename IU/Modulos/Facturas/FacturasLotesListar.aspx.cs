using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Facturas.Entidades;
using System.Collections;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Facturas;

namespace IU.Modulos.Facturas
{
    public partial class FacturasLotesListar : PaginaSegura
    {
        private DataTable MisFacturasLotes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "FacturasListarMisFacturasLotes"]; }
            set { Session[this.MiSessionPagina + "FacturasListarMisFacturasLotes"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPeriodo, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("FacturasLotesAgregar.aspx");
                this.CargarCombos();

                VTAFacturasLotesEnviados parametros = this.BusquedaParametrosObtenerValor<VTAFacturasLotesEnviados>();
                if (parametros.BusquedaParametros)
                {
                    this.txtPeriodo.Text = parametros.Periodo.ToString();
                    this.ddlTipoLote.SelectedValue = parametros.TiposLotesEnviados.IdTipoLoteEnviado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            VTAFacturasLotesEnviados parametros = this.BusquedaParametrosObtenerValor<VTAFacturasLotesEnviados>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasLotesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTAFacturasLotesEnviados factura = new VTAFacturasLotesEnviados();
            factura.IdFacturaLoteEnviado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdFacturaLoteEnviado"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdFacturaLoteEnviado", factura.IdFacturaLoteEnviado);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasLotesAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                DataRowView dr = (DataRowView)e.Row.DataItem;

                if (Convert.ToBoolean(dr["PuedeAnular"]))
                {
                    ibtnAnular.Visible = this.ValidarPermiso("FacturasLotesAnular.aspx");
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = Convert.ToDecimal(this.MisFacturasLotes.Compute("Sum(ImporteTotal)", string.Empty)).ToString("C2");

                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisFacturasLotes.Rows.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTAFacturasLotesEnviados parametros = this.BusquedaParametrosObtenerValor<VTAFacturasLotesEnviados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTAFacturasLotesEnviados>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisFacturasLotes;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisFacturasLotes = this.OrdenarGrillaDatos<DataTable>(this.MisFacturasLotes, e);
            this.gvDatos.DataSource = this.MisFacturasLotes;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisFacturasLotes;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        private void CargarCombos()
        {
            this.ddlTipoLote.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposLotesEnviados);
            this.ddlTipoLote.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoLote.DataTextField = "Descripcion";
            this.ddlTipoLote.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoLote, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarLista(VTAFacturasLotesEnviados pFactura)
        {
            pFactura.Periodo = this.txtPeriodo.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPeriodo.Text);
            pFactura.TiposLotesEnviados.IdTipoLoteEnviado = this.ddlTipoLote.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoLote.SelectedValue);
            
            pFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pFactura.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<VTAFacturasLotesEnviados>(pFactura);
            this.MisFacturasLotes = FacturasF.FacturasLotesObtenerGrilla(pFactura);
            this.gvDatos.DataSource = this.MisFacturasLotes;
            this.gvDatos.PageIndex = pFactura.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
