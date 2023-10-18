using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Proveedores.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using CuentasPagar.Entidades;
using Proveedores;
using System.Globalization;
using Generales.Entidades;
using CuentasPagar.FachadaNegocio;

namespace IU.Modulos.CuentasPagar.Controles
{
    public partial class SolicitudesPagosAnticipos : ControlesSeguros
    {
        private CapSolicitudPago MiSolicitud
        {
            get { return (CapSolicitudPago)Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"]; }
            set { Session[this.MiSessionPagina + "SolicitudDatosMiSolicitud"] = value; }
        }

        public delegate void SolicitudPagoAnticiposAceptarEventHandler(object sender, CapSolicitudPago e);
        public event SolicitudPagoAnticiposAceptarEventHandler SolicitudPagoAnticipoModificarDatosAceptar;

        public delegate void SolicitudPagoAnticiposCancelarEventHandler();
        public event SolicitudPagoAnticiposCancelarEventHandler SolicitudPagoAnticipoModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(CtrBuscarProveedor_BuscarProveedor);
            if (!this.IsPostBack)
            {
                if (this.MiSolicitud == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        
        public void IniciarControl(CapSolicitudPago pSolicitud, Gestion pGestion)
        {
            CapProveedores prov = new CapProveedores();
            this.GestionControl = pGestion;
            this.MiSolicitud = pSolicitud;
            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:

                    if (this.MisParametrosUrl.Contains("IdProveedor"))
                    {
                        //this.hdfIdAfiliado.Value = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //MiProveedor.IdProveedor = 0;
                        ctrBuscarProveedor.BuscarProveedor += new Proveedores.Controles.ProveedoresCabecerasDatos.ProveedoresDatosCabeceraAjaxEventHandler(CtrBuscarProveedor_BuscarProveedor);
                        prov.IdProveedor = Convert.ToInt32(this.MisParametrosUrl["IdProveedor"].ToString());
                        ctrBuscarProveedor.IniciarControl(prov, this.GestionControl);
                        //button_Click(null, EventArgs.Empty);
                        //this.txtNumeroSocio.Text = this.MisParametrosUrl["IdAfiliado"].ToString();
                        //this.txtNumeroSocio_TextChanged(this.txtNumeroSocio, EventArgs.Empty);
                    }

                    this.txtDescripcion.Enabled = true;
                    this.txtImporte.Enabled = true;
                    this.ctrComentarios.IniciarControl(this.MiSolicitud, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiSolicitud, this.GestionControl);
                    break;
                case Gestion.Consultar:
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoAnticipoObtenerDatosCompletos(pSolicitud);
                    prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;

                    //MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    MapearObjetoAControles(this.MiSolicitud);
                    this.btnAceptar.Visible = false;
                    
                    break;
                case Gestion.Autorizar:
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoAnticipoObtenerDatosCompletos(pSolicitud);
                    prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;

                    //MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    MapearObjetoAControles(this.MiSolicitud);
                    
                    break;
                case Gestion.Anular:
                    this.MiSolicitud = CuentasPagarF.SolicitudPagoAnticipoObtenerDatosCompletos(pSolicitud);
                    prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;

                    //MapearObjetoAControlesProveedor(ProveedoresF.ProveedoresObtenerDatosCompletos(prov));
                    MapearObjetoAControles(this.MiSolicitud);
                    break;
                default:
                    break;
            }
        }

        #region "Proveedores PopUp"
        void CtrBuscarProveedor_BuscarProveedor(CapProveedores e)
        {
            this.MapearObjetoAControlesProveedor(e);
            //this.UpdatePanelProovedor.Update();
        }

        private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        {
            this.MiSolicitud.Entidad.IdCuentaContable = pProveedor.CuentaContable.IdCuentaContable;
            //en caso de cambiar de proveedor limpia la grilla
            if (this.GestionControl == Gestion.Agregar)
            {
              
            }
            //this.btnLimpiar.Visible = true;
            //Luego de agregar el Proveedor, dejo el foco en la fecha de factura

            ListaParametros listaParam = new ListaParametros(this.MiSessionPagina);
            listaParam.Agregar("IdProveedor", pProveedor.IdProveedor);
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            string txtCodigo = ((TextBox)sender).Text;
            CapProveedores parametro = new CapProveedores();
            parametro.IdProveedor = Convert.ToInt32(txtCodigo);
            parametro = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);
            this.MiSolicitud.Entidad.IdRefEntidad = parametro.IdProveedor == null ? 0 : Convert.ToInt32(parametro.IdProveedor);
            //mapear todo prov a Entidad
            if (this.MiSolicitud.Entidad.IdRefEntidad != 0)
            {
                this.MapearObjetoAControlesProveedor(parametro);
            }
            else
            {
                parametro.CodigoMensaje = "ProveedorCodigoNoExiste";
                this.MostrarMensaje(parametro.CodigoMensaje, true);
            }
        }

        #endregion

        private void CargarCombos()
        {

            TGETiposOperaciones operaciones = new TGETiposOperaciones();
            operaciones.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            operaciones.Estado.IdEstado = (int)Estados.Activo;
            this.ddlTipoOperacion.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operaciones);
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (ddlTipoOperacion.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

       

        void MapearControlesAObjeto(CapSolicitudPago pSolicitud)
        {
            pSolicitud.ImporteTotal = decimal.Parse(this.txtImporte.Text, NumberStyles.Currency);
            pSolicitud.Observacion = this.txtDescripcion.Text;
            pSolicitud.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            pSolicitud.Comentarios = ctrComentarios.ObtenerLista();
            
            pSolicitud.FechaFactura = txtFechaFactura.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(txtFechaFactura.Text);
            pSolicitud.FechaContable = pSolicitud.FechaFactura;
            pSolicitud.Archivos = ctrArchivos.ObtenerLista();
            pSolicitud.Entidad.IdRefEntidad = ctrBuscarProveedor.MiProveedor.IdProveedor.Value;
        }

        private void MapearObjetoAControles(CapSolicitudPago pSolicitud)
        {
            this.txtDescripcion.Text = pSolicitud.Observacion;
            this.txtImporte.Text = pSolicitud.ImporteTotal.ToString("C2");
            this.ddlTipoOperacion.SelectedValue = pSolicitud.TipoOperacion.IdTipoOperacion.ToString(); //REVISAR
            this.ctrComentarios.IniciarControl(pSolicitud, this.GestionControl);
            this.ctrArchivos.IniciarControl(pSolicitud, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pSolicitud);
            this.txtFechaFactura.Text = pSolicitud.FechaFactura.ToString();
            this.ctrAsientoMostrar.IniciarControl(pSolicitud);
            CapProveedores prov = new CapProveedores();
            prov.IdProveedor = this.MiSolicitud.Entidad.IdRefEntidad;
            this.ctrBuscarProveedor.IniciarControl(prov, this.GestionControl);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;

            this.Page.Validate("Aceptar");

            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiSolicitud);
            this.MiSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiSolicitud.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiSolicitud.Estado.IdEstado = (int)Estados.Activo;
                    guardo = CuentasPagarF.SolicitudPagoAnticipoAgregar(this.MiSolicitud);
                    break;
                case Gestion.Autorizar:
                    this.MiSolicitud.IdUsuarioAutorizacion = this.UsuarioActivo.IdUsuario;
                    this.MiSolicitud.FechaAutorizacion = DateTime.Now;
                    this.MiSolicitud.Estado.IdEstado = (int)EstadosSolicitudesPagos.Autorizado;

                    guardo = CuentasPagarF.SolicitudPagoAutorizar(this.MiSolicitud);
                    break;
                case Gestion.Anular:
                    this.MiSolicitud.Estado.IdEstado = (int)Estados.Baja;
                    this.MiSolicitud.FechaAnulacion = DateTime.Now;
                    this.MiSolicitud.IdUsuarioAnulacion = this.UsuarioActivo.IdUsuario;
                    
                    guardo = CuentasPagarF.SolicitudPagoAnular(this.MiSolicitud);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiSolicitud.CodigoMensaje));
                this.ctrAsientoMostrar.IniciarControl(this.MiSolicitud);
            }
            else
            {
                this.MostrarMensaje(this.MiSolicitud.CodigoMensaje, true, this.MiSolicitud.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            if (this.SolicitudPagoAnticipoModificarDatosCancelar != null)
                this.SolicitudPagoAnticipoModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.SolicitudPagoAnticipoModificarDatosAceptar != null)
                this.SolicitudPagoAnticipoModificarDatosAceptar(null, this.MiSolicitud);
        }
    }
}