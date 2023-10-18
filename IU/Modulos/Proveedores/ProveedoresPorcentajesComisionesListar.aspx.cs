using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Proveedores;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Proveedores.Entidades;

namespace IU.Modulos.Proveedores
{
    public partial class ProveedoresPorcentajesComisionesListar : PaginaSegura
    {
        private List<CapProveedoresPorcentajesComisiones> MisProveedoresPorcentajesComisiones
        {
            get { return (List<CapProveedoresPorcentajesComisiones>)Session[this.MiSessionPagina + "ProveedoresListarMisProveedoresPorcentajesComisiones"]; }
            set { Session[this.MiSessionPagina + "ProveedoresListarMisProveedoresPorcentajesComisiones"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrModificarDatos.ModificarDatosAceptar += new Controles.ProveedoresPorcentajesComisionesDatos.ModificarDatosAceptarEventHandler(ctrModificarDatos_ModificarDatosAceptar);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("ProveedoresPorcentajesComisionesAgregar.aspx");
                this.CargarCombos();

                CapProveedoresPorcentajesComisiones parametros = this.BusquedaParametrosObtenerValor<CapProveedoresPorcentajesComisiones>();
                if (parametros.BusquedaParametros)
                {
                    ddlNumeroProveedor.SelectedValue = parametros.Proveedor.IdProveedor > 0 ? parametros.Proveedor.IdProveedor.ToString() : string.Empty;
                    ddlTiposOperaciones.SelectedValue = parametros.TipoOperacion.IdTipoOperacion > 0 ? parametros.TipoOperacion.IdTipoOperacion.ToString() : string.Empty;
                    
                    ddlFormaCobro.SelectedValue = parametros.FormaCobro.IdFormaCobro > 0 ? parametros.FormaCobro.IdFormaCobro.ToString() : string.Empty;
                    this.CargarLista(parametros);
                }

            }
        }

        void ctrModificarDatos_ModificarDatosAceptar(object sender, CapProveedoresPorcentajesComisiones e)
        {
            this.btnBuscar_Click(null, EventArgs.Empty);
        }

        private void CargarCombos()
        {

            this.ddlFormaCobro.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
            this.ddlFormaCobro.DataTextField = "FormaCobro";
            this.ddlFormaCobro.DataValueField = "IdFormaCobro";
            this.ddlFormaCobro.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlNumeroProveedor.DataSource = ProveedoresF.ProveedoresObtenerEsVendedor();
            this.ddlNumeroProveedor.DataValueField = "IdProveedor";
            this.ddlNumeroProveedor.DataTextField = "RazonSocial";
            this.ddlNumeroProveedor.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroProveedor, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TGETiposOperaciones tipoOp = new TGETiposOperaciones();
            tipoOp.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaActual.IdTipoFuncionalidad;
            this.ddlTiposOperaciones.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(tipoOp);
            this.ddlTiposOperaciones.DataValueField = "IdTipoOperacion";
            this.ddlTiposOperaciones.DataTextField = "TipoOperacion";
            this.ddlTiposOperaciones.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposOperaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CapProveedoresPorcentajesComisiones parametros = this.BusquedaParametrosObtenerValor<CapProveedoresPorcentajesComisiones>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            //  this.ctrModificarDatos.IniciarControl(new CapProveedoresPorcentajesComisiones());
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresPorcentajesComisionesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CapProveedoresPorcentajesComisiones proveedor = this.MisProveedoresPorcentajesComisiones[indiceColeccion];

            ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            parametros.Agregar("IdProveedorPorcentajeComision", proveedor.IdProveedorPorcentajeComision);//proveedor.IdProveedor);

            if (e.CommandName == "Baja")
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresPorcentajesComisionesAnular.aspx"), true);
            }
            else if (e.CommandName == "Consultar")
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Proveedores/ProveedoresPorcentajesComisionesConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapProveedoresPorcentajesComisiones item = (CapProveedoresPorcentajesComisiones)e.Row.DataItem;
                if (item.Estado.IdEstado == (int)Estados.Activo
                    && this.ValidarPermiso("ProveedoresPorcentajesComisionesAnular.aspx"))
                {
                    ImageButton ibtnBaja = (ImageButton)e.Row.FindControl("btnBaja");
                    //string msg = string.Format(this.ObtenerMensajeSistema("ConfirmarBajaProveedorComision"), item.Porcentaje.ToString(), item.TipoOperacion.TipoOperacion, item.Proveedor.RazonSocial);
                    //string funcion = string.Format("showConfirm(this,'{0}'); return false;", msg);
                    //ibtnBaja.Attributes.Add("OnClick", funcion);
                    ibtnBaja.Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CapProveedoresPorcentajesComisiones parametros = this.BusquedaParametrosObtenerValor<CapProveedoresPorcentajesComisiones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CapProveedoresPorcentajesComisiones>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisProveedoresPorcentajesComisiones;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisProveedoresPorcentajesComisiones = this.OrdenarGrillaDatos<CapProveedoresPorcentajesComisiones>(this.MisProveedoresPorcentajesComisiones, e);
            this.gvDatos.DataSource = this.MisProveedoresPorcentajesComisiones;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisProveedoresPorcentajesComisiones;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        private void CargarLista(CapProveedoresPorcentajesComisiones pProveedorPorcComi)
        {
            pProveedorPorcComi.Proveedor.IdProveedor = this.ddlNumeroProveedor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlNumeroProveedor.SelectedValue);
            pProveedorPorcComi.TipoOperacion.IdTipoOperacion = this.ddlTiposOperaciones.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTiposOperaciones.SelectedValue);
            pProveedorPorcComi.FormaCobro.IdFormaCobro = ddlFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlFormaCobro.SelectedValue);
            pProveedorPorcComi.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CapProveedoresPorcentajesComisiones>(pProveedorPorcComi);
            this.MisProveedoresPorcentajesComisiones = ProveedoresF.CapProveedoresPorcentajesComisionesObtenerListaFiltro(pProveedorPorcComi);
            this.gvDatos.DataSource = this.MisProveedoresPorcentajesComisiones;
            this.gvDatos.PageIndex = pProveedorPorcComi.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisProveedoresPorcentajesComisiones.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}