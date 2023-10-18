using Comunes.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Facturas
{
    public partial class FilialesPuntosVentasListar : PaginaSegura
    {
        private DataTable MisDatosGrillas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "FilialesPuntosVentasListarMisDatosGrillas"]; }
            set { Session[this.MiSessionPagina + "FilialesPuntosVentasListarMisDatosGrillas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrPuntoVentaDatos.FilialPuntoVentaDatosAceptar += new Controles.FilialesPuntosVentasDatos.FilialesPuntosVentasDatosAceptarEventHandler(ctrPuntoVentaDatos_FilialPuntoVentaDatosAceptar);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                this.btnAgregar.Visible = this.ValidarPermiso("FilialesPuntosVentasAgregar.aspx");
                this.CargarGrilla();
            }
        }
        private void CargarCombos()
        {
            List<TGEListasValoresSistemasDetalles> tiposPuntosVentas = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.AFIPTiposPuntosVentas);
            this.ddlTiposPuntosVentas.DataSource = tiposPuntosVentas;
            this.ddlTiposPuntosVentas.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTiposPuntosVentas.DataTextField = "Descripcion";
            this.ddlTiposPuntosVentas.DataBind();
            if (this.ddlTiposPuntosVentas.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposPuntosVentas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFilial.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTiposFacturas.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposFacturas);
            this.ddlTiposFacturas.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTiposFacturas.DataTextField = "Descripcion";
            this.ddlTiposFacturas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposFacturas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarGrilla()
        {
            VTAFilialesPuntosVentas filtro = new VTAFilialesPuntosVentas();
            filtro.IdFilial = this.ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            filtro.IdTipoFactura = this.ddlTiposFacturas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTiposFacturas.SelectedValue);
            filtro.TipoPuntoVenta.IdTipoPuntoVenta = this.ddlTiposPuntosVentas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTiposPuntosVentas.SelectedValue);
            filtro.AfipPuntoVenta = this.txtPrefijoNumeroFactura.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPrefijoNumeroFactura.Text);
            this.MisDatosGrillas = FacturasF.VTAFilialesPuntosVentasObtenerListaGrilla(filtro);
            this.gvDatos.DataSource = this.MisDatosGrillas;
            this.gvDatos.DataBind();

            if (this.MisDatosGrillas.Rows.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }
        void ctrPuntoVentaDatos_FilialPuntoVentaDatosAceptar(object sender, global::Facturas.Entidades.VTAFilialesPuntosVentas e)
        {
            this.CargarGrilla();
            this.UpdatePanel1.Update();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarGrilla();
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.ctrPuntoVentaDatos.IniciarControl(new VTAFilialesPuntosVentas(), Gestion.Agregar);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTAFilialesPuntosVentas filialPuntoVenta = new VTAFilialesPuntosVentas();
            filialPuntoVenta.IdFilialPuntoVenta = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrPuntoVentaDatos.IniciarControl(filialPuntoVenta, Gestion.Modificar);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                modificar.Visible = this.ValidarPermiso("FilialesPuntosVentasBorrar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDatosGrillas.Rows.Count);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTAFacturas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisDatosGrillas;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataView dataView = new DataView(this.MisDatosGrillas);
            dataView.Sort = e.SortExpression + " " + e.SortDirection.ToString().Substring(0, 3).ToUpper();
            this.gvDatos.DataSource = dataView;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisDatosGrillas;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
    }
}
