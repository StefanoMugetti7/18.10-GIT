using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Compras.Entidades;
using System.Collections;
//using Proveedores.Entidades;
using Compras;
using Generales.FachadaNegocio;
using Generales.Entidades;


namespace IU.Modulos.Compras
{
    public partial class SolicitudesComprasListar : PaginaSegura
    {
        private List<CmpSolicitudesCompras> MisSolicitudes
        {
            get { return (List<CmpSolicitudesCompras>)Session[this.MiSessionPagina + "SolicitudesComprasListarMisSolicitudes"]; }
            set { Session[this.MiSessionPagina + "SolicitudesComprasListarMisSolicitudes"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrBuscarProveedorPopUp.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar);
            if (!this.IsPostBack)
            {

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSolicitud, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroFactura, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPrefijoNumeroFactura, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("SolicitudesComprasAgregar.aspx");
                this.CargarCombos();

                CmpSolicitudesCompras parametros = this.BusquedaParametrosObtenerValor<CmpSolicitudesCompras>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSolicitud.Text = parametros.IdSolicitudCompra == 0 ? String.Empty : parametros.IdSolicitudCompra.ToString();
                    //this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    
                    this.ddlTipoSolicitud.SelectedValue = parametros.TipoSolicitudCompra.IdTipoSolicitudCompra.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    //this.txtCodigo.Text = parametros.Proveedor.IdProveedor.ToString();
                    //this.txtProveedor.Text = parametros.Proveedor.RazonSocial;

                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CmpSolicitudesCompras parametros = this.BusquedaParametrosObtenerValor<CmpSolicitudesCompras>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasAgregar.aspx"), true);
        }

        private void CargarCombos()
        {
            this.ddlTipoSolicitud.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposSolicitudesCompras);
            this.ddlTipoSolicitud.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoSolicitud.DataTextField = "Descripcion";
            this.ddlTipoSolicitud.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoSolicitud, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosSolicitudesCompras));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();

        }

        #region "Grilla"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()
                ||e.CommandName == Gestion.Modificar.ToString()
                ||e.CommandName == Gestion.Impresion.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CmpSolicitudesCompras pSolicitud = this.MisSolicitudes[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdSolicitudCompra", pSolicitud.IdSolicitudCompra);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Autorizar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasAutorizar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/SolicitudesComprasCotizar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                this.ctrPopUpComprobantes.CargarReporte(pSolicitud, EnumTGEComprobantes.CmpSolicitudesCompras);
                this.UpdatePanel1.Update();
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CmpSolicitudesCompras solicitud = (CmpSolicitudesCompras)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton cotizar = (ImageButton)e.Row.FindControl("btnCotizarSolicitud");
                ibtnConsultar.Visible = this.ValidarPermiso("SolicitudesComprasConsultar.aspx");

                switch (solicitud.Estado.IdEstado)
                {
                    case (int)EstadosSolicitudesCompras.Activo:
                        autorizar.Visible = this.ValidarPermiso("SolicitudesComprasAutorizar.aspx");
                        anular.Visible = this.ValidarPermiso("SolicitudesComprasAnular.aspx");
                        //cotizar.Visible = this.ValidarPermiso("SolicitudesComprasCotizar.aspx");
                        break;
                    case (int)EstadosSolicitudesCompras.Autorizado:
                        cotizar.Visible = this.ValidarPermiso("SolicitudesComprasCotizar.aspx");
                        anular.Visible = this.ValidarPermiso("SolicitudesComprasAnular.aspx");
                        break;

                    case (int)EstadosSolicitudesCompras.Cotizado:
                        anular.Visible = this.ValidarPermiso("SolicitudesComprasAnular.aspx");
                        break;
                    
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSolicitudes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CmpSolicitudesCompras parametros = this.BusquedaParametrosObtenerValor<CmpSolicitudesCompras>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CmpSolicitudesCompras>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisSolicitudes;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSolicitudes = this.OrdenarGrillaDatos<CmpSolicitudesCompras>(this.MisSolicitudes, e);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        #endregion

        //#region "Proveedores PopUp"

        //protected void btnLimpiar_Click(object sender, EventArgs e)
        //{
        //    CmpSolicitudesCompras parametros = this.BusquedaParametrosObtenerValor<CmpSolicitudesCompras>();
        //    parametros.Proveedor = new CapProveedores();
        //    this.txtCodigo.Text = string.Empty;
        //    this.txtProveedor.Text = string.Empty;
        //    this.BusquedaParametrosGuardarValor<CmpSolicitudesCompras>(parametros);
        //    this.UpdatePanel1.Update();
        //}

        //protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarProveedorPopUp.IniciarControl();
        //}

        //void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        //{
        //    CmpSolicitudesCompras parametros = this.BusquedaParametrosObtenerValor<CmpSolicitudesCompras>();
        //    parametros.Proveedor.RazonSocial = pProveedor.RazonSocial;
        //    parametros.Proveedor.IdProveedor = pProveedor.IdProveedor.Value;
        //    this.BusquedaParametrosGuardarValor<CmpSolicitudesCompras>(parametros);
        //    this.MapearObjetoAControlesProveedor(pProveedor);
        //    this.UpdatePanel1.Update();
        //}

        //private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        //{
        //    this.txtCodigo.Text = pProveedor.IdProveedor.Value.ToString();
        //    this.txtProveedor.Text = pProveedor.RazonSocial;
        //}

        ////protected void txtCodigo_TextChanged(object sender, EventArgs e)
        ////{
        ////    string txtCodigo = ((TextBox)sender).Text;
        ////    CapProveedores parametro = new CapProveedores();
        ////    parametro.IdProveedor = Convert.ToInt32(txtCodigo);
        ////    this.MisSolicitudes.Proveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);
        ////    if (this.MiSolicitud.Proveedor.IdProveedor != 0)
        ////    {
        ////        this.MapearObjetoAControlesProveedor(this.MiSolicitud.Proveedor);
        ////    }
        ////    else
        ////    {
        ////        parametro.CodigoMensaje = "ProveedorCodigoNoExiste";
        ////        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(parametro.CodigoMensaje));
        ////    }
        ////}

        //#endregion

        private void CargarLista(CmpSolicitudesCompras pSolicitud)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pSolicitud.IdSolicitudCompra = this.txtNumeroSolicitud.Text == String.Empty ? 0 : Convert.ToInt32(this.txtNumeroSolicitud.Text);
            //pSolicitud.NumeroFactura = this.txtNumeroFactura.Text == string.Empty ? "" : this.txtNumeroFactura.Text;
            //pSolicitud.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text == String.Empty ? "" : this.txtPrefijoNumeroFactura.Text;
            pSolicitud.TipoSolicitudCompra.IdTipoSolicitudCompra = this.ddlTipoSolicitud.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlTipoSolicitud.SelectedValue);
            pSolicitud.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //VER QUE BUSQUE CON EL POP UP

            //pSolicitud.Proveedor.IdProveedor = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);
            pSolicitud.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pSolicitud.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pSolicitud.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CmpSolicitudesCompras>(pSolicitud);
            this.MisSolicitudes = ComprasF.ComprasObtenerListaFiltro(pSolicitud);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.PageIndex = pSolicitud.IndiceColeccion;
            this.gvDatos.DataBind();

            //if (this.MisSolicitudes.Count > 0)
            //    btnExportarExcel.Visible = true;
            //else
            //    btnExportarExcel.Visible = false;
        }
    }
}