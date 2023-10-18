using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class CamposListar : PaginaSegura
    {
        private List<TGECampos> MisCampos
        {
            get { return (List<TGECampos>)Session[this.MiSessionPagina + "CamposListarMisCampos"]; }
            set { Session[this.MiSessionPagina + "CamposListarMisCampos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("CamposAgregar.aspx");
                this.CargarCombo();
                TGECampos parametros = this.BusquedaParametrosObtenerValor<TGECampos>();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtValorParametro, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTitulo, this.btnBuscar);
                if (parametros.BusquedaParametros)
                {
                    this.txtTitulo.Text = parametros.Titulo;
                    this.ddlTablaAsociada.SelectedValue = parametros.IdTablaValor.ToString();
                    this.ddlTablaParametro.SelectedValue = parametros.IdTabla.ToString();
                    this.txtValorParametro.Text = parametros.IdRefTabla.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista();
                }
            }
        }
        private void CargarCombo()
        {
            this.ddlTablaAsociada.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TablasCamposDinamicos);
            this.ddlTablaAsociada.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTablaAsociada.DataTextField = "Descripcion";
            this.ddlTablaAsociada.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTablaAsociada, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTablaParametro.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TablasParametrosCamposDinamicos);
            this.ddlTablaParametro.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTablaParametro.DataTextField = "Descripcion";
            this.ddlTablaParametro.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTablaParametro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            if (ddlEstado.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CamposAgregar.aspx"), true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarLista();
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGECampos campo = this.MisCampos[indiceColeccion];
            //string parametros = string.Format("?IdCuenta={0}", cuentafiliado.IdCuenta);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCampo", campo.IdCampo);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CamposModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                //string url = string.Concat(, parametros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/CamposConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGECampos campo = (TGECampos)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                modificar.Visible = this.ValidarPermiso("CamposModificar.aspx");

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCampos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGECampos parametros = this.BusquedaParametrosObtenerValor<TGECampos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGECampos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCampos;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCampos = this.OrdenarGrillaDatos<TGECampos>(this.MisCampos, e);
            this.gvDatos.DataSource = this.MisCampos;
            this.gvDatos.DataBind();
        }
        private void CargarLista()
        {
            TGECampos campo = new TGECampos();
            campo.Titulo = this.txtTitulo.Text;
            campo.IdTablaValor = this.ddlTablaAsociada.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTablaAsociada.SelectedValue);
            campo.TablaValor = this.ddlTablaAsociada.SelectedValue == string.Empty ? string.Empty : this.ddlTablaAsociada.SelectedItem.Text;
            campo.IdTabla = this.ddlTablaParametro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTablaParametro.SelectedValue);
            campo.Tabla = this.ddlTablaParametro.SelectedValue == string.Empty ? string.Empty : this.ddlTablaParametro.SelectedItem.Text;
            campo.IdRefTabla = this.txtValorParametro.Text == string.Empty ? 0 : Convert.ToInt32(this.txtValorParametro.Text);
            campo.Estado.IdEstado = this.ddlEstado.SelectedValue == string.Empty ? (int)EstadosTodos.Todos : Convert.ToInt32(this.ddlEstado.SelectedValue);
            campo.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TGECampos>(campo);
            this.MisCampos = TGEGeneralesF.CamposObtenerListaFiltro(campo);
            this.gvDatos.DataSource = this.MisCampos;
            this.gvDatos.DataBind();
        }
    }
}
