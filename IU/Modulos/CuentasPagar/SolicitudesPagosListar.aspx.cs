using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using CuentasPagar.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;
using Proveedores.Entidades;
using Proveedores;
using System.Data;
using Reportes.FachadaNegocio;

namespace IU.Modulos.CuentasPagar
{
    public partial class SolicitudesPagosListar : PaginaSegura
    {
        private DataTable MisSolicitudes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "SolicitudesPagosListarMisSolicitudes"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosListarMisSolicitudes"] = value; }
        }

        public CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "ProveedorCabeceraDatosMiProveedor"]; }
            set { Session[this.MiSessionPagina + "ProveedorCabeceraDatosMiProveedor"] = value; }
        }

        public delegate void ProveedoresDatosCabeceraAjaxEventHandler(CapProveedores e);

        public event ProveedoresDatosCabeceraAjaxEventHandler BuscarProveedor;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSolicitud, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroFactura, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPrefijoNumeroFactura, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("SolicitudesPagosAgregar.aspx");
                this.CargarCombos();


                CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
             

                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSolicitud.Text = parametros.IdSolicitudPago == 0 ? String.Empty : parametros.IdSolicitudPago.ToString();
                    this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    this.txtNumeroFactura.Text = parametros.NumeroFactura;
                    if (parametros.Entidad.IdRefEntidad > 0)
                    {

                        
                            CapProveedores proveedor = new CapProveedores();
                            proveedor.IdProveedor = parametros.Entidad.IdRefEntidad;
                            proveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(proveedor);
                            this.ddlNumeroProveedor.Items.Add(new ListItem(proveedor.RazonSocial.ToString(), proveedor.IdProveedor.ToString()));
                            this.ddlNumeroProveedor.SelectedValue = proveedor.IdProveedor.ToString();
                        
                     
                    }
                    this.ddlTipoSolicitud.SelectedValue = parametros.TipoSolicitudPago.IdTipoSolicitudPago.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlFilial.SelectedValue = parametros.IdFilial > 0 ? parametros.IdFilial.ToString() : string.Empty;
                    this.CargarLista(parametros);
                }
            }
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }

        protected void button_Click(object sender, EventArgs e)
        {
            string prueba = hdfRazonSocial.Value;

            string txtNumeroProveedor = this.hdfIdProveedor.Value;
            MiProveedor = new CapProveedores();
            MiProveedor.IdProveedor = txtNumeroProveedor == string.Empty ? 0 : Convert.ToInt32(txtNumeroProveedor);
            MiProveedor.RazonSocial = prueba;
            MiProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(MiProveedor);

            if (MiProveedor.IdProveedor != 0)
            {
                this.MapearObjetoAControlesProveedor(MiProveedor);
            }
            else
            {
                this.txtCUIT.Text = string.Empty;
                this.ddlEstados.SelectedValue = string.Empty;
                MiProveedor.CodigoMensaje = "El Proveedor no existe";
                //this.UpdatePanel2.Update();
                this.MostrarMensaje(MiProveedor.CodigoMensaje, true);
            }
            BuscarProveedor?.Invoke(MiProveedor);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            CapSolicitudPago pSolicitud = new CapSolicitudPago();
            this.MisParametrosUrl = new Hashtable();
            pSolicitud.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
            //pSolicitud.Entidad.IdRefEntidad = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);
            this.MisParametrosUrl.Add("IdEntidad", pSolicitud.Entidad.IdEntidad);
            this.MisParametrosUrl.Add("IdRefEntidad", pSolicitud.Entidad.IdRefEntidad);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAgregar.aspx"), true);
        }

        private void CargarCombos()
        {
            this.ddlTipoSolicitud.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposSolicitudesPago);
            this.ddlTipoSolicitud.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoSolicitud.DataTextField = "Descripcion";
            this.ddlTipoSolicitud.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoSolicitud, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosSolicitudesPagos));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        #region "Grilla"
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
                || e.CommandName == "AgregarOP"
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            CapSolicitudPago pSolicitud = new CapSolicitudPago();
            pSolicitud.IdSolicitudPago = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdSolicitudPago"].ToString());
            pSolicitud.Entidad.IdEntidad = Convert.ToInt32(((GridView)sender).DataKeys[index]["EntidadIdEntidad"].ToString());
            pSolicitud.Entidad.IdRefEntidad = Convert.ToInt32(((GridView)sender).DataKeys[index]["EntidadIdRefEntidad"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdSolicitudPago", pSolicitud.IdSolicitudPago);

            if (pSolicitud.Entidad.IdEntidad == (int)EnumTGEEntidades.Proveedores)
            {
                ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
                parametros.Agregar("IdProveedor", pSolicitud.Entidad.IdRefEntidad);
            }

            if (pSolicitud.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosAnticipos)
            {
                if (e.CommandName == Gestion.Anular.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAnticiposAnular.aspx"), true);
                }
                else if (e.CommandName == Gestion.Consultar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAnticiposConsultar.aspx"), true);
                }
                else if (e.CommandName == Gestion.Autorizar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAnticiposAutorizar.aspx"), true);
                }
                else if (e.CommandName == Gestion.Impresion.ToString())
                {
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapSolicitudPagos, "SolicitudPagoCompras", pSolicitud, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "SolicitudPagoCompras", this.UsuarioActivo);
                    this.UpdatePanel1.Update();
                }
            }
            else
            {
                if (e.CommandName == Gestion.Anular.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAnular.aspx"), true);
                }
                else if (e.CommandName == Gestion.Consultar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosConsultar.aspx"), true);
                }
                else if (e.CommandName == Gestion.Autorizar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAutorizar.aspx"), true);
                }
                else if (e.CommandName == Gestion.Modificar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosModificar.aspx"), true);
                }
                else if (e.CommandName == "AgregarOP")
                {
                    this.MisParametrosUrl.Add("IdEntidad", pSolicitud.Entidad.IdEntidad);
                    this.MisParametrosUrl.Add("IdRefEntidad", pSolicitud.Entidad.IdRefEntidad);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAgregar.aspx"), true);
                }
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapSolicitudPagos, "SolicitudPagoCompras", pSolicitud, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.Page, "SolicitudPagoCompras", this.UsuarioActivo);
                this.UpdatePanel1.Update();
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataRowView dr = (DataRowView)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton agregarOP = (ImageButton)e.Row.FindControl("btnAgregarOP");

                ibtnConsultar.Visible = this.ValidarPermiso("SolicitudesPagosConsultar.aspx");
                modificar.Visible = this.ValidarPermiso("SolicitudesPagosModificar.aspx");
                switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                {
                    case (int)EstadosSolicitudesPagos.Activo:
                        autorizar.Visible = this.ValidarPermiso("SolicitudesPagosAutorizar.aspx");
                        anular.Visible = this.ValidarPermiso("SolicitudesPagosAnular.aspx");
                        break;
                    case (int)EstadosSolicitudesPagos.Autorizado:
                        anular.Visible = this.ValidarPermiso("SolicitudesPagosAnular.aspx");
                        agregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                        break;
                    case (int)EstadosSolicitudesPagos.EnOrdenPagoParcial:
                    case (int)EstadosSolicitudesPagos.PagadoParcial:
                        agregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                        break;
                    case (int)EstadosSolicitudesPagos.Baja:
                        modificar.Visible = false ;
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteSinIVA = (Label)e.Row.FindControl("lblImporteSinIVA");
                lblImporteSinIVA.Text = Convert.ToDecimal(this.MisSolicitudes.Compute("Sum(ImporteSinIVA)", "")).ToString("C2");

                Label lblIvaTotal = (Label)e.Row.FindControl("lblIvaTotal");
                lblIvaTotal.Text = Convert.ToDecimal(this.MisSolicitudes.Compute("Sum(IvaTotal)", "")).ToString("C2");

                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = Convert.ToDecimal(this.MisSolicitudes.Compute("Sum(ImporteTotal)", "")).ToString("C2"); 

                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSolicitudes.Rows.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;

            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSolicitudes = this.OrdenarGrillaDatos<DataTable>(this.MisSolicitudes, e);
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

        #region "Proveedores PopUp"
        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.Entidad = new TGEEntidades();
            //this.txtCodigo.Text = string.Empty;
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(parametros);
            this.UpdatePanel1.Update();
            ListaParametros listaParam = new ListaParametros(this.MiSessionPagina);
            listaParam.Limpiar("IdProveedor");
        }

        private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        {
            //this.txtCodigo.Text = pProveedor.IdProveedor.Value.ToString();
            ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            parametros.Agregar("IdProveedor", pProveedor.IdProveedor);
            string proovedor = hdfRazonSocial.Value.ToString();
            this.ddlNumeroProveedor.Items.Add(new ListItem(pProveedor.RazonSocial, pProveedor.IdProveedor.ToString()));
            this.ddlNumeroProveedor.SelectedValue = hdfIdProveedor.Value;
            this.txtCUIT.Text = pProveedor.CUIT.ToString();
        }
    
        #endregion

        private void CargarLista(CapSolicitudPago pSolicitud)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pSolicitud.IdSolicitudPago = this.txtNumeroSolicitud.Text == String.Empty ? 0 : Convert.ToInt32(this.txtNumeroSolicitud.Text);
            pSolicitud.NumeroFactura = this.txtNumeroFactura.Text == string.Empty ? "" : this.txtNumeroFactura.Text;
            pSolicitud.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text == String.Empty ? "" : this.txtPrefijoNumeroFactura.Text;
            pSolicitud.TipoSolicitudPago.IdTipoSolicitudPago = this.ddlTipoSolicitud.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlTipoSolicitud.SelectedValue);
            pSolicitud.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.Text);
            //pSolicitud.Entidad.Nombre = this.ddlNumeroProveedor.SelectedValue == string.Empty ? "" : this.ddlNumeroProveedor.SelectedValue;
            //VER QUE BUSQUE CON EL POP UP
            if (!string.IsNullOrEmpty(hdfIdProveedor.Value))
            {
                pSolicitud.Entidad.IdRefEntidad = hdfIdProveedor.Value == string.Empty ? 0 : Convert.ToInt32(hdfIdProveedor.Value);
                if (pSolicitud.Entidad.IdRefEntidad != 0)
                {
                    ddlNumeroProveedor.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdProveedor.Value));
                    ddlNumeroProveedor.SelectedValue = hdfIdProveedor.Value;
                }
            }

            pSolicitud.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
            //pSolicitud.Entidad.IdRefEntidad = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);
            pSolicitud.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pSolicitud.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pSolicitud.FechaVencimientoDesde = this.txtFechaVencimientoDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaVencimientoDesde.Text);
            pSolicitud.FechaVencimientoHasta = this.txtFechaVencimientoHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaVencimientoHasta.Text);
            pSolicitud.IdFilial = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pSolicitud.BusquedaParametros = true;
            pSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pSolicitud.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pSolicitud.PageSize;
            gvDatos.PageIndex = pSolicitud.PageIndex;
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(pSolicitud);
            this.MisSolicitudes = CuentasPagarF.SolicitudPagoObtenerGrilla(pSolicitud);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.VirtualItemCount = MisSolicitudes.Rows.Count > 0 ? Convert.ToInt32(MisSolicitudes.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();

            if (this.MisSolicitudes.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }

        //protected void btnModificar_Click(object sender, EventArgs e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/ModificarComprobantes.aspx"), true);
        //}
    }
}