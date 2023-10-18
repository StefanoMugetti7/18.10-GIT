using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Proveedores;
using Proveedores.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Afiliados.Entidades;
using System.Data;
using Seguridad.FachadaNegocio;
using Reportes.Entidades;
using System.Net.Mail;
using Comunes.LogicaNegocio;
using System.IO;
using Contabilidad;
using Compras.Entidades;
using Compras;
using CuentasPagar.Entidades;
using Facturas;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Proveedores.Controles
{
    public partial class ProveedoresDatos : ControlesSeguros
    {
        private CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "ProveedoresDatosMiProveedor"]; }
            set { Session[this.MiSessionPagina + "ProveedoresDatosMiProveedor"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "ProveedorModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "ProveedorModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        //private List<CapCuentasCorrientes> MiCuentaCorriente
        //{
        //    get { return (List<CapCuentasCorrientes>)Session[this.MiSessionPagina + "ProveedoresDatosMiCuentaCorriente"]; }
        //    set { Session[this.MiSessionPagina + "ProveedoresDatosMiCuentaCorriente"] = value; }
        //}

        private DataTable MiCuentaCorriente
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProveedoresDatosMiCuentaCorriente"]; }
            set { Session[this.MiSessionPagina + "ProveedoresDatosMiCuentaCorriente"] = value; }
        }

        private DataTable MisInformes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProveedoresDatosMisInformes"]; }
            set { Session[this.MiSessionPagina + "ProveedoresDatosMisInformes"] = value; }
        }

        private DataTable MisInformesPendientes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProveedoresDatosMisInformesPendientes"]; }
            set { Session[this.MiSessionPagina + "ProveedoresDatosMisInformesPendientes"] = value; }
        }

        private DataTable MisRecepcionesAcopios
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProveedoresDatosMisRecepcionesAcopios"]; }
            set { Session[this.MiSessionPagina + "ProveedoresDatosMisRecepcionesAcopios"] = value; }
        }

        public delegate void ProveedoresDatosAceptarEventHandler(object sender, CapProveedores e);
        public event ProveedoresDatosAceptarEventHandler ProveedorModificarDatosAceptar;

        public delegate void ProveedoresDatosCancelarEventHandler();
        public event ProveedoresDatosCancelarEventHandler ProveedorModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrDomicilios.ProveedoresModificarDatosAceptar += new ProveedorModificarDatosDomicilioPopUp.ProveedorModificarDatosDomicilioEventHandler(ctrDomicilios_ProveedoresModificarDatosAceptar);
            this.ctrTelefonos.ProveedoresModificarDatosAceptar += new ProveedorModificarDatosTelefonoPopUp.ProveedorModificarDatosTelefonoEventHandler(ctrTelefonos_ProveedoresModificarDatosAceptar);
            this.ctrCuentasContables.CuentasContablesBuscarSeleccionar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(ctrCuentasContables_CuentasContablesBuscarSeleccionar);
            this.ctrCuentasContables.CuentasContablesBuscarIniciar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(ctrCuentasContables_CuentasContablesBuscarIniciar);
            if (this.IsPostBack)
            {
                if (this.MiProveedor == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        void ctrCuentasContables_CuentasContablesBuscarIniciar(global::Contabilidad.Entidades.CtbEjerciciosContables ejercicio)
        {
            AyudaProgramacion.MatchObjectProperties(ContabilidadF.EjerciciosContablesObtenerUltimoActivo(), ejercicio);
        }

        void ctrCuentasContables_CuentasContablesBuscarSeleccionar(global::Contabilidad.Entidades.CtbCuentasContables e, int indiceColeccion)
        {
            this.MiProveedor.CuentaContable = e;
            this.upCuentasContables.Update();
        }

        public void IniciarControl(CapProveedores pProveedor, Gestion pGestion)
        {
            //OJO CON ESTO!
            CapProveedores parametros = this.BusquedaParametrosObtenerValor<CapProveedores>();
            if (pProveedor.IdProveedor != parametros.IdProveedor)
            {
                parametros = new CapProveedores();
                this.BusquedaParametrosGuardarValor<CapProveedores>(parametros);
            }
            else if (parametros.BusquedaParametros)
            {
                tcDatos.ActiveTabIndex = parametros.HashTransaccion;
                if (tcDatos.ActiveTab.ID == "tpCuentaCorriente")
                {
                    gvDatos.PageIndex = parametros.IndiceColeccion;
                }
                else if (tcDatos.ActiveTab.ID == "tpInformesRecepcion")
                {
                    gvRemitos.PageIndex = parametros.IndiceColeccion;
                }
                //else if (tcDatos.ActiveTab.ID == "tpPresupuestos")
                //{
                //    gvPresupuestos.PageIndex = parametros.IndiceColeccion;
                //}
                BusquedaParametrosGuardarValor<CapProveedores>(parametros);
            }

            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiProveedor = pProveedor;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlEstados.SelectedValue =((int)Estados.Activo).ToString();
                    this.ctrArchivos.IniciarControl(pProveedor, this.GestionControl);
                    this.tpHistorial.Visible = false;
                    this.ctrCamposValores.IniciarControl(this.MiProveedor, new Objeto(), this.GestionControl);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.UpdatePanel2.GetType(), "InitControlsScript", "InitControls();", true);
                    this.phBotones.Visible = false;
                    this.txtRazonSocial.Enabled = true;
                    break;
                case Gestion.Modificar:
                    this.MiProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(pProveedor);
                    this.MapearObjetoAControles(this.MiProveedor);
                    this.btnAgregarComprobante.Visible = this.ValidarPermiso("SolicitudesPagosAgregar.aspx");
                    this.btnAgregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                    this.txtRazonSocial.Enabled = true;
                    break;
                case Gestion.Consultar:
                    this.MiProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(pProveedor);
                    this.MapearObjetoAControles(this.MiProveedor);
                    this.btnAceptar.Visible = false;
                    this.btnAgregarDomicilio.Visible = false;
                    this.btnAgregarTelefono.Visible = false;
                    this.ddlCondicionesFiscales.Enabled = false;
                    this.txtRazonSocial.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.ddlEstados.Enabled = false;
                    this.txtCuit.Enabled = false;
                    this.txtPaginaWeb.Enabled = false;
                    this.txtBeneficiarioDelCheque.Enabled = false;
                    this.txtCBU.Enabled = false;
                    this.txtFechaVencimiento.Enabled = false;
                    this.btnTxtCuitBlur.Visible = false;
                    this.txtDetalle.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(CapProveedores pProveedor)
        {
            this.txtIdProveedor.Text = pProveedor.IdProveedor.ToString();
            this.txtRazonSocial.Text = pProveedor.RazonSocial;
            this.txtDetalle.Text = pProveedor.Detalle;
            this.txtFechaVencimiento.Text = AyudaProgramacion.MostrarFechaPantalla(pProveedor.CUITVto);
            this.ddlEstados.SelectedValue = pProveedor.Estado.IdEstado.ToString();
            this.ddlCondicionesFiscales.SelectedValue = pProveedor.CondicionFiscal.IdCondicionFiscal.ToString();
            this.txtBeneficiarioDelCheque.Text = pProveedor.BeneficiarioDelCheque;
            this.txtCBU.Text = string.Concat(pProveedor.CBU8Digitos, pProveedor.CBU14Digitos);
            if (!string.IsNullOrEmpty(pProveedor.CBU))
            {
                this.lblDatosCbu.Text = ProveedoresF.ProveedoresObtenerDatosCbu(pProveedor).DatosCBU;
            }
            this.txtCuit.Text = pProveedor.CUIT;
            this.txtPaginaWeb.Text = pProveedor.PaginaWeb;

            this.MiCuentaCorriente = ProveedoresF.CuentasCorrientesObtenerListaFiltroFechasDT(this.MiProveedor);
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            this.upCtaCte.Update();

            CmpInformesRecepciones filtro = new CmpInformesRecepciones();
            filtro.Proveedor = this.MiProveedor;
            filtro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MisInformesPendientes = ComprasF.InformesRecepcionesObtenerDetallesPendientesRecibirFiltroPorProveedor(filtro);
            this.gvInformesRecepcionPendientes.DataSource = MisInformesPendientes;
            this.gvInformesRecepcionPendientes.DataBind();

            this.MisRecepcionesAcopios = ComprasF.InformesRecepcionesObtenerAcopiosPendientesRecibirFiltroPorProveedor(filtro);
            this.phDetalleAcopio.Visible = this.MisRecepcionesAcopios.Rows.Count > 0;
            this.gvAcopios.DataSource = this.MisRecepcionesAcopios;
            this.gvAcopios.DataBind();

            this.MisInformes = ComprasF.InformesRecepcionesObtenerListaGrilla(filtro);
            this.gvRemitos.DataSource = this.MisInformes;
            this.gvRemitos.DataBind();

            AyudaProgramacion.CargarGrillaListas(pProveedor.ProveedoresDomicilios, true, this.gvDomicilios, true);
            AyudaProgramacion.CargarGrillaListas(this.MiProveedor.Telefonos, true, this.gvTelefonos, true);
            
            if (this.MiCuentaCorriente.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
            
            this.ctrComentarios.IniciarControl(pProveedor, this.GestionControl);
            this.ctrArchivos.IniciarControl(pProveedor, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pProveedor);
            this.ctrCamposValores.IniciarControl(this.MiProveedor, new Objeto(), this.GestionControl);
            this.ctrCuentasContables.MapearObjetoControles(pProveedor.CuentaContable, this.GestionControl, 0);
        }

        private void MapearControlesAObjeto(CapProveedores pProveedor)
        {
            pProveedor.RazonSocial = this.txtRazonSocial.Text;
            pProveedor.Detalle = this.txtDetalle.Text;
            pProveedor.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pProveedor.CondicionFiscal.IdCondicionFiscal = Convert.ToInt32(this.ddlCondicionesFiscales.SelectedValue);
            pProveedor.BeneficiarioDelCheque = this.txtBeneficiarioDelCheque.Text;
            pProveedor.CUIT = this.txtCuit.Text;
            pProveedor.PaginaWeb = this.txtPaginaWeb.Text;
            if (this.txtFechaVencimiento.Text != String.Empty)
            {
                pProveedor.CUITVto = Convert.ToDateTime(this.txtFechaVencimiento.Text);
            }

            if (this.txtCBU.Text.Length > 8)
            {
                pProveedor.CBU8Digitos = this.txtCBU.Text.Substring(0, 8);
                pProveedor.CBU14Digitos = this.txtCBU.Text.Substring(8, this.txtCBU.Text.Length - 8);
            }
            else
            {
                pProveedor.CBU8Digitos = this.txtCBU.Text.Substring(0, this.txtCBU.Text.Length);
                pProveedor.CBU14Digitos = String.Empty;
            }

            pProveedor.Comentarios = ctrComentarios.ObtenerLista();
            pProveedor.Archivos = ctrArchivos.ObtenerLista();
            pProveedor.Campos = this.ctrCamposValores.ObtenerLista();
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            //this.ddlCondicionesFiscales.DataSource = TGEGeneralesF.TGECondicionesFiscalesObtenerLista();
            this.ddlCondicionesFiscales.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionesFiscales.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionesFiscales.DataTextField = "Descripcion";
            this.ddlCondicionesFiscales.DataBind();
        }

        protected void btnTxtCuitBlur_Click(object sender, EventArgs e)
        {
            MapearControlesAObjeto(MiProveedor);
            if (txtCuit.Decimal > 0)
            {
                this.txtRazonSocial.Enabled = true;
                if (!ProveedoresF.ProveedoresObtenerDatosAFIP(MiProveedor))
                {
                    //this.txtCuit.Decimal = 0;
                    MostrarMensaje(MiProveedor.CodigoMensaje, true);
                }
            }
            MapearObjetoAControles(MiProveedor);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiProveedor);
            this.MiProveedor.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ProveedoresF.ProveedoresAgregar(this.MiProveedor);
                    break;
                case Gestion.Modificar:
                    guardo = ProveedoresF.ProveedoresModificar(this.MiProveedor);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiProveedor.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiProveedor.CodigoMensaje, true, this.MiProveedor.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ProveedorModificarDatosCancelar != null)
                this.ProveedorModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ProveedorModificarDatosAceptar != null)
                this.ProveedorModificarDatosAceptar(null, this.MiProveedor);
        }

        protected void txtCBU_TextChanged(object sender, EventArgs e)
        {
            if (this.txtCBU.Text.Length != 22)
            {
                this.lblDatosCbu.Text = "CBU Invalido.";
                return;
            }
            CapProveedores aux = new CapProveedores();
            aux.CBU = this.txtCBU.Text;
            aux = ProveedoresF.ProveedoresObtenerDatosCbu(aux);
            this.lblDatosCbu.Text = aux.DatosCBU;
        }


        #region Cuuenta Corriente
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            CapProveedores parametros = this.BusquedaParametrosObtenerValor<CapProveedores>();


            parametros.IndiceColeccion = e.NewPageIndex;
            parametros.HashTransaccion = tpCuentaCorriente.TabIndex;
            parametros.IdProveedor = MiProveedor.IdProveedor.Value;
            parametros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CapProveedores>(parametros);
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            //AyudaProgramacion.CargarGrillaListas(this.MiCuentaCorriente, false, this.gvDatos, true);
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            // GridViewExportUtil.Export("Datos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MiCuentaCorriente);
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            
            this.MiProveedor.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiProveedor.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            this.MiProveedor.Filtro = this.txtDetalleFiltro.Text;
            this.MiCuentaCorriente = ProveedoresF.CuentasCorrientesObtenerListaFiltroFechasDT(this.MiProveedor);
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            this.upCtaCte.Update();
        }

        protected void btnAgregarComprobante_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEntidad", (int)EnumTGEEntidades.Proveedores);
            this.MisParametrosUrl.Add("IdRefEntidad", this.MiProveedor.IdProveedor);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAgregar.aspx"), true);
        }

        protected void btnAgregarOP_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEntidad", (int)EnumTGEEntidades.Proveedores);
            this.MisParametrosUrl.Add("IdRefEntidad", this.MiProveedor.IdProveedor);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAgregar.aspx"), true);
        }

        protected void btnAgregarRemito_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdProveedor", this.MiProveedor.IdProveedor);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAbiertaAgregar.aspx"), true);
        }
        protected void btnAgregarAnticipo_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdProveedor", this.MiProveedor.IdProveedor);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAnticiposAgregar.aspx"), true);
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdProveedor";
            param.ValorParametro = this.MiProveedor.IdProveedor;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaDesde";
            param.ValorParametro = this.txtFechaDesde.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesde.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = this.txtFechaHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaHasta.Text);
            reporte.Parametros.Add(param);

            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.CapResumenCuenta);

            reporte.StoredProcedure = comprobante.NombreSP;
            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "CapResumenCuenta";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(comprobante, plantilla, ds, "IdProveedor", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.upCtaCte, "CapResumenCuenta", this.UsuarioActivo);

            //this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.VTAResumenCuenta);
            this.upCtaCte.Update();

        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdProveedor";
            param.ValorParametro = this.MiProveedor.IdProveedor;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaDesde";
            param.ValorParametro = this.txtFechaDesde.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesde.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = this.txtFechaHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaHasta.Text);
            reporte.Parametros.Add(param);

            MailMessage mail = new MailMessage();
            TGEArchivos archivo = new TGEArchivos();
            archivo.Archivo = AyudaProgramacionLN.StreamToByteArray(this.ctrPopUpComprobantes.GenerarReporteArchivo(reporte, EnumTGEComprobantes.CapResumenCuenta));
            archivo.NombreArchivo = string.Concat(this.MiProveedor.IdProveedor, ".pdf");

            mail.Attachments.Add(new Attachment(new MemoryStream(archivo.Archivo), archivo.NombreArchivo));
            if (ProveedoresF.ProveedoresArmarMailResumenCuenta(this.MiProveedor, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiProveedor);
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.AnularConfirmar.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
                || e.CommandName == "AgregarOP"
            ))
                return;

            CapProveedores parametros = this.BusquedaParametrosObtenerValor<CapProveedores>();
            parametros.HashTransaccion = tpCuentaCorriente.TabIndex;
            parametros.IdProveedor = MiProveedor.IdProveedor;
            parametros.BusquedaParametros = true;



            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            int IdEstado = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfEstado")).Value);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)

            if (e.CommandName == Gestion.Impresion.ToString())
            {
                if (IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagos
                    || IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesPagosInterno)
                {
                    CapOrdenesPagos op = new CapOrdenesPagos();
                    op.IdOrdenPago = IdRefTipoOperacion;
                    op.Estado.IdEstado = IdEstado;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapOrdenesPagos, "OrdenesPagos", op, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.tpCuentaCorriente, "OrdenesPagos", this.UsuarioActivo);
                }
                else
                {
                    CapSolicitudPago sp = new CapSolicitudPago();
                    sp.IdSolicitudPago = IdRefTipoOperacion;
                    sp.Estado.IdEstado = IdEstado;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapSolicitudPagos, "SolicitudPagoCompras", sp, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.tpCuentaCorriente, "SolicitudPagoCompras", this.UsuarioActivo);
                }
                    this.UpdatePanel2.Update();
            }
            else
            {

                if (e.CommandName == Gestion.Consultar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;
                else if (e.CommandName == Gestion.Anular.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;
                else if (e.CommandName == Gestion.AnularConfirmar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_AnularConfirmar;
                else if (e.CommandName == Gestion.Autorizar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Autorizar;
                else if (e.CommandName == Gestion.Modificar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Modificar;
                else if (e.CommandName == "AgregarOP")
                {
                    this.MisParametrosUrl.Add("IdEntidad", 187);
                   this.MisParametrosUrl.Add("IdRefEntidad", MiProveedor.IdProveedor);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAgregar.aspx"), true);
                }

                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
                this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
                this.MisParametrosUrl.Add("IdProveedor", this.MiProveedor.IdProveedor);
                //Si devuelve una URL Redirecciona si no muestra mensaje error
                if (filtroMenu.URL.Length != 0)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
                }
                else
                {
                    this.MiProveedor.CodigoMensaje = "ErrorURLNoValida";
                    this.MostrarMensaje(this.MiProveedor.CodigoMensaje, true);
                }
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
                ImageButton anularConfirmar = (ImageButton)e.Row.FindControl("btnAnularConfirmar");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                ImageButton agregarOP = (ImageButton)e.Row.FindControl("btnAgregarOP");

                ibtnConsultar.Visible = this.ValidarPermiso("SolicitudesPagosConsultar.aspx");
                modificar.Visible = this.ValidarPermiso("SolicitudesPagosModificar.aspx");
                imprimir.Visible = false;

                switch (Convert.ToInt32(dr["TipoOperacionIdTipoOperacion"]))
                {
                    case (int)EnumTGETiposOperaciones.SolicitudesPagosInternos:
                    case (int)EnumTGETiposOperaciones.SolicitudesPagosCompras:
                    case (int)EnumTGETiposOperaciones.SolicitudesPagosAFIP:
                        switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                        {
                            case (int)EstadosSolicitudesPagos.Activo:
                                autorizar.Visible = this.ValidarPermiso("SolicitudesPagosAutorizar.aspx");
                                anular.Visible = this.ValidarPermiso("SolicitudesPagosAnular.aspx");
                                imprimir.Visible = true;

                                break;
                            case (int)EstadosSolicitudesPagos.Autorizado:
                                anular.Visible = this.ValidarPermiso("SolicitudesPagosAnular.aspx");
                                agregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                                imprimir.Visible = true;

                                break;
                            case (int)EstadosSolicitudesPagos.EnOrdenPagoParcial:
                            case (int)EstadosSolicitudesPagos.PagadoParcial:
                                agregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                                imprimir.Visible = true;

                                break;
                        }
                        break;
                    case (int)EnumTGETiposOperaciones.OrdenesPagos:
                    case (int)EnumTGETiposOperaciones.OrdenesPagosInterno:      
                        switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                        {
                            case (int)EstadosOrdenesPago.Activo:
                                autorizar.Visible = this.ValidarPermiso("OrdenesPagosAutorizar.aspx");
                                anular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                                modificar.Visible = false;
                                break;
                            case (int)EstadosOrdenesPago.Autorizado:
                                anular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                                modificar.Visible = false;
                                break;
                            case (int)EstadosOrdenesPago.Pagado:
                                anularConfirmar.Visible = this.ValidarPermiso("OrdenesPagosAnularPagada.aspx");
                                modificar.Visible = this.ValidarPermiso("OrdenesPagosModificar.aspx");
                                break; 
                        }
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        #endregion

        #region "Remitos"
        protected void gvInformesRecepcionPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = Convert.ToDecimal(this.MisInformesPendientes.Compute("Sum(SubTotalItem)", string.Empty)).ToString("C2");
            }
        }
        protected void gvRemitos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                //|| e.CommandName == Gestion.Autorizar.ToString()
                ))
                return;
            CapProveedores parametros = this.BusquedaParametrosObtenerValor<CapProveedores>();
            parametros.HashTransaccion = tpInformesRecepcion.TabIndex;
            parametros.IdProveedor = MiProveedor.IdProveedor;
            parametros.BusquedaParametros = true;

            int index = Convert.ToInt32(e.CommandArgument);
            int idInformeRecepcion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdInformeRecepcion", idInformeRecepcion);

            //if (pInforme.OrdenCompra.IdOrdenCompra > 0)
            //{
            //    if (e.CommandName == Gestion.Anular.ToString())
            //    {
            //        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAnular.aspx"), true);
            //    }
            //    else if (e.CommandName == Gestion.Consultar.ToString())
            //    {
            //        this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesConsultar.aspx"), true);
            //    }
            //}
            //else
            //{
                if (e.CommandName == Gestion.Anular.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAbiertaAnular.aspx"), true);
                }
                else if (e.CommandName == Gestion.Consultar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAbiertaConsultar.aspx"), true);
                }
            //}
        }

        protected void gvRemitos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DataRowView dr = (DataRowView)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnConsultarAbierta = (ImageButton)e.Row.FindControl("btnConsultarAbierta");
                ImageButton anularAbierta = (ImageButton)e.Row.FindControl("btnAnularAbierta");

                //if (informe.OrdenCompra.IdOrdenCompra > 0)
                //{
                //    ibtnConsultar.Visible = this.ValidarPermiso("InformesRecepcionesConsultar.aspx");
                //    switch (informe.Estado.IdEstado)
                //    {
                //        case (int)EstadosInformesRecepciones.Activo:
                //            anular.Visible = this.ValidarPermiso("InformesRecepcionesAnular.aspx");
                //            break;
                //    }
                //}
                //else
                //{
                    ibtnConsultarAbierta.Visible = this.ValidarPermiso("InformesRecepcionesAbiertaConsultar.aspx");
                    switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                    {
                        case (int)EstadosInformesRecepciones.Activo:
                            anularAbierta.Visible = this.ValidarPermiso("InformesRecepcionesAbiertaAnular.aspx");
                            break;
                    }
                //}
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisInformes.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvRemitos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CmpInformesRecepciones parametros = this.BusquedaParametrosObtenerValor<CmpInformesRecepciones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CmpInformesRecepciones>(parametros);

            CapProveedores parametro = this.BusquedaParametrosObtenerValor<CapProveedores>();
            parametro.IndiceColeccion = e.NewPageIndex;
            parametro.HashTransaccion = tpInformesRecepcion.TabIndex;
            parametro.IdProveedor = MiProveedor.IdProveedor;
            parametro.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<CapProveedores>(parametro);


            gvRemitos.PageIndex = e.NewPageIndex;
            gvRemitos.DataSource = this.MisInformes;
            gvRemitos.DataBind();
        }

        protected void gvRemitos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MisInformes = this.OrdenarGrillaDatos(this.MisInformes, e);
            this.gvDatos.DataSource = this.MisInformes;
            this.gvDatos.DataBind();
        }
        #endregion

        #region Acopio
        protected void btnAgregarAcopio_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdRefTabla", this.MiProveedor.IdProveedor);
            this.MisParametrosUrl.Add("Tabla", "CapProveedores");
            this.MisParametrosUrl.Add("RazonSocial", this.MiProveedor.RazonSocial);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Acopios/AcopiosAgregar.aspx"), true);
        }
        #endregion

        #region Domicilios

        protected void btnAgregarDomicilio_Click(object sender, EventArgs e)
        {
            this.ctrDomicilios.IniciarControl(new CapProveedoresDomicilios(), Gestion.Agregar);
        }

        void ctrDomicilios_ProveedoresModificarDatosAceptar(object sender, CapProveedoresDomicilios e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiProveedor.ProveedoresDomicilios.Add(e);
                    e.IndiceColeccion = this.MiProveedor.ProveedoresDomicilios.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiProveedor.ProveedoresDomicilios[this.MiIndiceDetalleModificar] = e;

                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiProveedor.ProveedoresDomicilios, true, this.gvDomicilios, true);
            this.upDomicilios.Update();
        }

        protected void gvDomicilios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrDomicilios.IniciarControl(this.MiProveedor.ProveedoresDomicilios[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrDomicilios.IniciarControl(this.MiProveedor.ProveedoresDomicilios[indiceColeccion], Gestion.Consultar);
            }
        }

        protected void gvDomicilios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapProveedoresDomicilios item = (CapProveedoresDomicilios)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("DomiciliosConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Telefonos

        protected void gvTelefonos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrTelefonos.IniciarControl(this.MiProveedor.Telefonos[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrTelefonos.IniciarControl(this.MiProveedor.Telefonos[indiceColeccion], Gestion.Consultar);
            }
        }

        protected void gvTelefonos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapProveedoresTelefonos item = (CapProveedoresTelefonos)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.Cells[4].FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("TelefonosConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        void ctrTelefonos_ProveedoresModificarDatosAceptar(object sender, CapProveedoresTelefonos e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiProveedor.Telefonos.Add(e);
                    e.IndiceColeccion = this.MiProveedor.Telefonos.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiProveedor.Telefonos[this.MiIndiceDetalleModificar] = e;
                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiProveedor.Telefonos, true, this.gvTelefonos, true);
            this.upTelefonos.Update();
        }

        protected void btnAgregarTelefono_Click(object sender, EventArgs e)
        {
            CapProveedoresTelefonos telefono = new CapProveedoresTelefonos();
            telefono.IdProveedor = Convert.ToInt32(this.MiProveedor.IdProveedor);
            this.ctrTelefonos.IniciarControl(telefono, Gestion.Agregar);
        }

        #endregion

    }
}