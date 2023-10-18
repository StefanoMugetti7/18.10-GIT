using Afiliados;
using Afiliados.Entidades;
using Afiliados.Entidades.Entidades;
using Cobros.Entidades;
using Comunes.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using Seguridad.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class ClientesModificarDatos : ControlesSeguros
    {
        private AfiAfiliados MiAfiliado
        {
            get { return (AfiAfiliados)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiAfiliado"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiAfiliado"] = value; }
        }

        public List<TGEListasValoresDetalles> MisTiposReportes
        {
            get { return this.PropiedadObtenerValor<List<TGEListasValoresDetalles>>("TGEListasValoresDetallesMisTiposReporteClientes"); }
            set { this.PropiedadGuardarValor("TGEListasValoresDetallesMisTiposReporteClientes", value); }
        }
        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        //private List<VTACuentasCorrientes> MiCuentaCorriente
        //{
        //    get { return (List<VTACuentasCorrientes>)Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorriente"]; }
        //    set { Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorriente"] = value; }
        //}

        private DataTable MiCuentaCorriente
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorriente"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorriente"] = value; }
        }

        private DataTable MiCuentaCorrienteDolar
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorrienteDolar"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMiCuentaCorrienteDolar"] = value; }
        }


        private DataTable MisRemitos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMisRemitos"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMisRemitos"] = value; }
        }
        private DataTable MisNotasPedidos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMisNotasPedidos"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMisNotasPedidos"] = value; }
        }

        private DataTable MisRemitosPendientes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMisRemitosPendientes"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMisRemitosPendientes"] = value; }
        }

        private DataTable MisFacturasPendientes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMisFacturasPendientes"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMisFacturasPendientes"] = value; }
        }

        private DataTable MisPresupuestos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMisPresupuestos"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMisPresupuestos"] = value; }
        }

        private DataTable MisAcopios
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProveedoresDatosMisAcopios"]; }
            set { Session[this.MiSessionPagina + "ProveedoresDatosMisAcopios"] = value; }
        }

        private DataTable MisFacturas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosMisFacturas"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosMisFacturas"] = value; }
        }

        private DataTable MisOrdenesCobros
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesModificarDatosCobOrdenesCobros"]; }
            set { Session[this.MiSessionPagina + "ClientesModificarDatosCobOrdenesCobros"] = value; }
        }

        public delegate void AfiliadoDatosAceptarEventHandler(object sender, AfiAfiliados e);
        public event AfiliadoDatosAceptarEventHandler AfiliadosModificarDatosAceptar;
        public delegate void AfiliadoDatosCancelarEventHandler();
        public event AfiliadoDatosCancelarEventHandler AfiliadosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrDomicilios.AfiliadosModificarDatosAceptar += new AfiliadoModificarDatosDomicilioPopUp.AfiliadoModificarDatosDomicilioEventHandler(ctrDomicilios_AfiliadosModificarDatosAceptar);
            this.ctrTelefonos.AfiliadosModificarDatosAceptar += new AfiliadoModificarDatosTelefonoPopUp.AfiliadoModificarDatosTelefonoEventHandler(ctrTelefonos_AfiliadosModificarDatosAceptar);
            //this.ctrAfiliados.AfiliadosBuscarSeleccionar += new AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            //this.ctrAfiFamiliares.AfiliadosBuscarSeleccionar += new AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiFamiliares_AfiliadosBuscarSeleccionar);
            this.ctrFacturasHabitualesListar.FacturacionesHabitualesListarAgregar += new Facturas.Controles.FacturacionesHabitualesListar.FacturacionesHabitualesListarAgregarEventHandler(ctrFacturasHabitualesListar_FacturacionesHabitualesListarAgregar);
            this.ctrFacturasHabitualesListar.FacturacionesHabitualesListarConsultar += new Facturas.Controles.FacturacionesHabitualesListar.FacturacionesHabitualesListarConsultarEventHandler(ctrFacturasHabitualesListar_FacturacionesHabitualesListarConsultar);
            this.ctrFacturasHabitualesListar.FacturacionesHabitualesListarModificar += new Facturas.Controles.FacturacionesHabitualesListar.FacturacionesHabitualesListarModificarEventHandler(ctrFacturasHabitualesListar_FacturacionesHabitualesListarModificar);
            base.PageLoadEvent(sender, e);
            this.ctrEnviarMails.ArmarMail += new Comunes.EnviarMails.ArmarMailEventHandler(ctrEnviarMails_ArmarMail);
            this.ctrEnviarMails.IniciarProceso += new Comunes.EnviarMails.IniciarProcesoEventHandler(ctrEnviarMails_IniciarProceso);
            this.ctrEnviarMails.FinalizarProceso += new Comunes.EnviarMails.FinalizarProcesoEventHandler(ctrEnviarMails_FinalizarProceso);
            if (!this.IsPostBack)
            {

            }
            else
            {
                if (this.MiAfiliado == null)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlSesion.aspx"), true);
                }
            }
        }
        void ctrEnviarMails_FinalizarProceso()
        {
            btnFiltrarRemitos_Click(null, new EventArgs());
            this.upRemitos.Update();
        }
        void ctrEnviarMails_IniciarProceso(ref List<Objeto> listaEnvio)
        {
            DataView vista = new DataView(this.MisFacturas);
            vista.RowFilter = " Incluir = 1";
            VTARemitos factura;
            foreach (DataRowView row in vista)
            {
                factura = new VTARemitos();
                factura.IdRemito = Convert.ToInt32(row["IdRemito"]);
                factura.Filtro = row["NumeroRemitoCompleto"].ToString();
                listaEnvio.Add(factura);
                factura.IndiceColeccion = listaEnvio.IndexOf(factura);
            }
        }

        bool ctrEnviarMails_ArmarMail(Objeto item, System.Net.Mail.MailMessage mail)
        {
            VTARemitos factura = (VTARemitos)item;
            factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            factura = FacturasF.RemitosObtenerDatosCompletos(factura);
            return FacturasF.RemitoArmarMail(factura, mail);
        }

        private void GuardarFacturacionHabitualTab()
        {
            AfiAfiliados filtro = new AfiAfiliados();
            filtro.BusquedaParametros = true;
            filtro.HashTransaccion = tpContratosConveniosServicios.TabIndex;
            filtro.IdAfiliado = MiAfiliado.IdAfiliado;
            BusquedaParametrosGuardarValor<AfiAfiliados>(filtro);
        }

        void ctrFacturasHabitualesListar_FacturacionesHabitualesListarModificar(VTAFacturacionesHabituales e)
        {
            this.GuardarFacturacionHabitualTab();
        }

        void ctrFacturasHabitualesListar_FacturacionesHabitualesListarConsultar(VTAFacturacionesHabituales e)
        {
            this.GuardarFacturacionHabitualTab();
        }

        void ctrFacturasHabitualesListar_FacturacionesHabitualesListarAgregar()
        {
            this.GuardarFacturacionHabitualTab();
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.AfiliadosModificarDatosAceptar != null)
                this.AfiliadosModificarDatosAceptar(null, this.MiAfiliado);
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de un Afiliado
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(AfiAfiliados pAfiliado, Gestion pGestion)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();

            if (pAfiliado.IdAfiliado != parametros.IdAfiliado)
            {
                parametros = new AfiAfiliados();
                this.BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);
            }
            else if (parametros.BusquedaParametros)
            {
                tcDatos.ActiveTabIndex = parametros.HashTransaccion;
                if (tcDatos.ActiveTab.ID == "tpCuentaCorriente")
                {
                    gvDatos.PageIndex = parametros.IndiceColeccion;
                }
                if (tcDatos.ActiveTab.ID == "tpCuentaCorrienteDolar")
                {
                    gvDatosDolar.PageIndex = parametros.IndiceColeccion;
                }
                else if (tcDatos.ActiveTab.ID == "tpRemitos")
                {
                    gvRemitos.PageIndex = parametros.IndiceColeccion;
                }
                else if (tcDatos.ActiveTab.ID == "tpPresupuestos")
                {
                    gvPresupuestos.PageIndex = parametros.IndiceColeccion;
                }
                else if (tcDatos.ActiveTab.ID == "tpNotasPedidos")
                {
                    gvNotasPedidos.PageIndex = parametros.IndiceColeccion;
                }
                BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);
            }

            this.GestionControl = pGestion;
            this.MiAfiliado = pAfiliado;
            this.btnAceptar.Visible = true;

            this.CargarCombos();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlEstados.SelectedValue = ((int)EstadosAfiliados.Normal).ToString();
                    this.txtTipoAfiliado.Text = EnumAfiliadosTipos.Clientes.ToString();
                    this.ddlEstados.Enabled = false;
                    this.txtApellido.Enabled = true;
                    this.tpContratosConveniosServicios.Enabled = false;
                    this.ctrArchivos.IniciarControl(this.MiAfiliado, this.GestionControl);
                    AfiClientes cliente = new AfiClientes();
                    cliente.IdAfiliado = this.MiAfiliado.IdAfiliado;
                    this.ctrCamposValores.IniciarControl(cliente, new Objeto(), this.GestionControl);
                    ScriptManager.RegisterStartupScript(this.UpdatePanel2, this.UpdatePanel2.GetType(), "InitControlsScript", "InitControls();", true);
                    break;
                case Gestion.Modificar:
                    this.MiAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(pAfiliado);
                    this.MapearObjetoAControles(this.MiAfiliado);
                    this.btnAgregarComprobante.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                    this.btnAgregarOC.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAgregar.aspx");
                    this.btnAgregarRemito.Visible = this.ValidarPermiso("RemitosAgregar.aspx");
                    this.btnAgregarPresupuesto.Visible = this.ValidarPermiso("PresupuestosAgregar.aspx");
                    this.btnAgregarNotaPedido.Visible = this.ValidarPermiso("NotasPedidosAgregar.aspx");
                    this.btnAgregarComprobanteDolar.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                    this.btnAgregarOCDolar.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAgregar.aspx");
                    this.btnAgregarRemitoDolar.Visible = this.ValidarPermiso("RemitosAgregar.aspx");
                    this.btnAgregarPresupuestoDolar.Visible = this.ValidarPermiso("PresupuestosAgregar.aspx");
                    if (this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo != (int)EnumAfiliadosTipos.Clientes)
                    {
                        this.txtApellido.Enabled = false;
                        this.ddlEstados.Enabled = false;
                        this.ddlTipoDocumento.Enabled = false;
                        this.txtNumeroDocumento.Enabled = false;
                    }
                    else
                    {
                        this.txtApellido.Enabled = true;
                    }
                    break;
                case Gestion.Consultar:
                    this.MiAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(pAfiliado);
                    this.MapearObjetoAControles(this.MiAfiliado);
                    //AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnAceptar.Visible = false;
                    this.btnAgregarDomicilio.Visible = false;
                    this.btnAgregarTelefono.Visible = false;
                    this.txtIdAfiliado.Enabled = false;
                    this.txtApellido.Enabled = false;
                    this.ddlEstados.Enabled = false;
                    this.txtDetalle.Enabled = false;
                    this.ddlTipoDocumento.Enabled = false;
                    this.txtNumeroDocumento.Enabled = false;
                    this.ddlCondicionFiscal.Enabled = false;
                    this.txtCorreoElectronico.Enabled = false;
                    this.btnAgregarComprobante.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                    this.btnAgregarOC.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAgregar.aspx");
                    this.btnAgregarRemito.Visible = this.ValidarPermiso("RemitosAgregar.aspx");
                    this.btnAgregarPresupuesto.Visible = this.ValidarPermiso("PresupuestosAgregar.aspx");
                    this.btnAgregarNotaPedido.Visible = this.ValidarPermiso("NotasPedidosAgregar.aspx");
                    this.btnAgregarComprobanteDolar.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                    this.btnAgregarOCDolar.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAgregar.aspx");
                    this.btnAgregarRemitoDolar.Visible = this.ValidarPermiso("RemitosAgregar.aspx");
                    this.btnAgregarPresupuestoDolar.Visible = this.ValidarPermiso("PresupuestosAgregar.aspx");
                    this.btntxtNumeroDocumentoBlur.Visible = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Mapea la Entidad SolicitudesMateriales a los controles de Pantalla
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearObjetoAControles(AfiAfiliados pAfiliado)
        {
            this.txtIdAfiliado.Text = pAfiliado.IdAfiliado.ToString();
            this.ddlEstados.SelectedValue = pAfiliado.Estado.IdEstado.ToString();
            this.txtApellido.Text = pAfiliado.Apellido;
            this.txtDetalle.Text = pAfiliado.Detalle;
            this.ddlTipoDocumento.SelectedValue = pAfiliado.TipoDocumento.IdTipoDocumento.HasValue ? pAfiliado.TipoDocumento.IdTipoDocumento.Value.ToString() : string.Empty;
            this.txtNumeroDocumento.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtCorreoElectronico.Text = pAfiliado.CorreoElectronico;
            this.txtTipoAfiliado.Text = pAfiliado.AfiliadoTipo.AfiliadoTipo;
            ListItem condicionFiscal = this.ddlCondicionFiscal.Items.FindByValue(pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString());
            if (condicionFiscal == null && pAfiliado.CondicionFiscal.IdCondicionFiscal > 0)
                this.ddlCondicionFiscal.Items.Add(new ListItem(pAfiliado.CondicionFiscal.Descripcion, pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString()));
            this.ddlCondicionFiscal.SelectedValue = pAfiliado.CondicionFiscal.IdCondicionFiscal == 0 ? string.Empty : pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString();
            this.chkComprobanteExento.Checked = pAfiliado.ComprobanteExento;
            //VTACuentasCorrientes filtro = new VTACuentasCorrientes();
            //filtro.IdAfiliado = pAfiliado.IdAfiliado;
            //this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorCliente(pAfiliado);
            /* ACA VAMOS A CARGAR LA CUENTA CORRIENTE POR MEDIO DEL DATASET*/
            CuentasCorrientesFiltro cuenta = new CuentasCorrientesFiltro();
            cuenta.IdAfiliado = MiAfiliado.IdAfiliado;
            cuenta.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            cuenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(cuenta);
            VTACuentasCorrientes parametros = this.BusquedaParametrosObtenerValor<VTACuentasCorrientes>();
            this.gvDatos.PageIndex = parametros.IndiceColeccion;
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();

            cuenta.IdMoneda = (int)EnumTGEMonedas.DolarEEUU;
            this.MiCuentaCorrienteDolar = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(cuenta);
            this.tpCuentaCorrienteDolar.Visible = this.MiCuentaCorrienteDolar.Rows.Count > 1;
            this.gvDatosDolar.PageIndex = parametros.IndiceColeccion;
            this.gvDatosDolar.DataSource = this.MiCuentaCorrienteDolar;
            this.gvDatosDolar.DataBind();


            this.upCtaCte.Update();
            this.upCtaCteDolar.Update();
            //AyudaProgramacion.CargarGrillaListas<VTACuentasCorrientes>(this.MiCuentaCorriente, false, this.gvDatos, true);
            if (this.MiCuentaCorriente.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;

            if (this.MiCuentaCorrienteDolar.Rows.Count > 0)
                btnExportarExcelDolar.Visible = true;
            else
                btnExportarExcelDolar.Visible = false;

            AyudaProgramacion.CargarGrillaListas(pAfiliado.Domicilios, true, this.gvDomicilios, true);
            AyudaProgramacion.CargarGrillaListas(pAfiliado.Telefonos, true, this.gvTelefonos, true);

            this.ctrComentarios.IniciarControl(pAfiliado, this.GestionControl);
            this.ctrArchivos.IniciarControl(pAfiliado, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pAfiliado);
            this.ctrFacturasHabitualesListar.IniciarControl(pAfiliado, this.GestionControl);
            AfiClientes cliente = new AfiClientes();
            cliente.IdAfiliado = this.MiAfiliado.IdAfiliado;
            cliente.Campos.AddRange(pAfiliado.Campos);
            this.ctrCamposValores.IniciarControl(cliente, new Objeto(), this.GestionControl);

            VTAFacturas factura = new VTAFacturas();
            factura.Afiliado = this.MiAfiliado;
            factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.MisAcopios = FacturasF.FacturasObtenerDetallesAcopiosPendienteEntrega(factura);
            this.phDetalleAcopio.Visible = this.MisAcopios.Rows.Count > 0;
            this.gvAcopios.DataSource = this.MisAcopios;
            this.gvAcopios.DataBind();

            MisRemitosPendientes = FacturasF.FacturasObtenerDetallesPendienteEntrega(factura);
            this.gvRemitosPendientes.DataSource = MisRemitosPendientes;
            this.gvRemitosPendientes.DataBind();
            this.btnMercaderiaPendienteEntrega.Visible = this.MisRemitosPendientes.Rows.Count > 0;

            MisFacturasPendientes = FacturasF.FacturasObtenerDetallesPendienteFacturar(factura);
            this.gvFacturasPendientes.DataSource = MisFacturasPendientes;
            this.gvFacturasPendientes.DataBind();
            this.upMercaderiaPendienteFacturar.Update();
            this.btnExportarExcelMercaderiaPendienteFacturar.Visible = this.MisFacturasPendientes.Rows.Count > 0;

            VTARemitos remitos = new VTARemitos();
            remitos.Afiliado = this.MiAfiliado;

            this.MisRemitos = FacturasF.RemitosObtenerAfiliadoGrilla(remitos);
            this.upRemitos.Visible = this.MisRemitos.Rows.Count > 0;
            this.btnExportarExcelRemitos.Visible = this.MisRemitos.Rows.Count > 0;
            this.gvRemitos.DataSource = this.MisRemitos;
            this.gvRemitos.DataBind();



            VTANotasPedidos notasPedidos = new VTANotasPedidos();
            notasPedidos.Afiliado = this.MiAfiliado;

            this.MisNotasPedidos = FacturasF.NotasPedidosObtenerGrilla(notasPedidos);
            this.gvNotasPedidos.DataSource = this.MisNotasPedidos;
            this.gvNotasPedidos.DataBind();


            VTAPresupuestos presupuestos = new VTAPresupuestos();
            presupuestos.Afiliado = this.MiAfiliado;
            presupuestos.Estado.IdEstado = (int)Estados.Activo;
            this.MisPresupuestos = FacturasF.PresupuestosObtenerListaGrilla(presupuestos);
            this.gvPresupuestos.DataSource = this.MisPresupuestos;
            // ? this.gvDatos.PageIndex = presupuestos.IndiceColeccion;
            this.gvPresupuestos.DataBind();
        }

        protected void btnExportarExcelMercaderiaPendienteFacturar_Click(object sender, ImageClickEventArgs e)
        {
            this.gvFacturasPendientes.AllowPaging = false;
            this.gvFacturasPendientes.DataSource = this.MisFacturasPendientes;
            this.gvFacturasPendientes.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvFacturasPendientes);
        }
        protected void btnMercaderiaPendienteEntrega_Click(object sender, ImageClickEventArgs e)
        {
            this.gvRemitosPendientes.AllowPaging = false;
            this.gvRemitosPendientes.DataSource = this.MisRemitosPendientes;
            this.gvRemitosPendientes.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvRemitosPendientes);
        }
        /// <summary>
        /// Mapea los controles de Pantalla con la Entidad SolicitudesMateriales.
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearControlesAObjeto(AfiAfiliados pAfiliado)
        {
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pAfiliado.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pAfiliado.Apellido = this.txtApellido.Text;
            pAfiliado.Detalle = this.txtDetalle.Text;
            pAfiliado.TipoDocumento.IdTipoDocumento = this.ddlTipoDocumento.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.CorreoElectronico = this.txtCorreoElectronico.Text;
            pAfiliado.CondicionFiscal.IdCondicionFiscal = this.ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscal.SelectedValue);
            pAfiliado.ComprobanteExento = this.chkComprobanteExento.Checked;

            pAfiliado.Comentarios = ctrComentarios.ObtenerLista();
            pAfiliado.Archivos = ctrArchivos.ObtenerLista();
            pAfiliado.Campos = this.ctrCamposValores.ObtenerLista();
        }

        /// <summary>
        /// Llena los DropDownList de la Pantalla con los datos
        /// </summary>
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosAfiliados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoDocumento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscal.DataTextField = "Descripcion";
            this.ddlCondicionFiscal.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //reporte.StoredProcedure = "ReportesSoloDeuda";


            TGEListasValores pParametro = new TGEListasValores();
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.CodigoValor = EnumTGEListasValoresCodigos.TiposReporteClientes.ToString();
            this.MisTiposReportes = TGEGeneralesF.ListasValoresObtenerListaDetalle(pParametro);
            this.ddlTipoReporte.DataSource = this.MisTiposReportes;
            this.ddlTipoReporte.DataValueField = "IdListaValorDetalle";
            this.ddlTipoReporte.DataTextField = "Descripcion";
            this.ddlTipoReporte.DataBind();

            //RepReportes reporte = new RepReportes();
            //reporte.StoredProcedure = "ReportesParametrosTiposFacturas";
            //this.ddlTipoComprobante.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            //this.ddlTipoComprobante.DataValueField = "IdTipoFactura";
            //this.ddlTipoComprobante.DataTextField = "Descripcion";
            //this.ddlTipoComprobante.DataBind();

            RepReportes reporte = new RepReportes();
            reporte.StoredProcedure = "ReportesSoloDeuda";
            this.ddlTipoReporteDolar.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            this.ddlTipoReporteDolar.DataValueField = "SoloDeuda";
            this.ddlTipoReporteDolar.DataTextField = "Descripcion";
            this.ddlTipoReporteDolar.DataBind();
            reporte = new RepReportes();
            reporte.StoredProcedure = "ReportesParametrosTiposFacturas";
            this.ddlTipoComprobanteDolar.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            this.ddlTipoComprobanteDolar.DataValueField = "IdTipoFactura";
            this.ddlTipoComprobanteDolar.DataTextField = "Descripcion";
            this.ddlTipoComprobanteDolar.DataBind();

            //reporte = new RepReportes();
            //reporte.StoredProcedure = "ReportesSoloDeuda";
            //this.ddlTipoReporteRemitos.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            //this.ddlTipoReporteRemitos.DataValueField = "SoloDeuda";
            //this.ddlTipoReporteRemitos.DataTextField = "Descripcion";
            //this.ddlTipoReporteRemitos.DataBind();
            //reporte = new RepReportes();
            //reporte.StoredProcedure = "ReportesParametrosTiposFacturas";
            //this.ddlTipoComprobanteRemito.DataSource = ReportesF.ReportesObtenerDatos(reporte);
            //this.ddlTipoComprobanteRemito.DataValueField = "IdTipoFactura";
            //this.ddlTipoComprobanteRemito.DataTextField = "Descripcion";
            //this.ddlTipoComprobanteRemito.DataBind();
        }

        protected void btntxtNumeroDocumentoBlur_Click(object sender, EventArgs e)
        {
            MapearControlesAObjeto(MiAfiliado);
            if (txtNumeroDocumento.Text != string.Empty
                && !string.IsNullOrEmpty(ddlTipoDocumento.SelectedValue)
                && (ddlTipoDocumento.SelectedValue == ((int)EnumTiposDocumentos.CUIL).ToString()
                    || ddlTipoDocumento.SelectedValue == ((int)EnumTiposDocumentos.CUIT).ToString()))
            {
                if (!AfiliadosF.AfiliadosObtenerDatosAFIP(MiAfiliado))
                {
                    this.txtApellido.Enabled = true;
                    MostrarMensaje(MiAfiliado.CodigoMensaje, true);
                }
                else
                {
                    this.txtApellido.Enabled = false;
                }
            }
            else
                this.txtApellido.Enabled = true;

            MapearObjetoAControles(MiAfiliado);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.PersistirDatos();
            this.Page.Validate("AfiliadosModificarDatosAceptar");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiAfiliado);
            //this.ActualizarGrilla();
            this.MiAfiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, this.MiAfiliado.UsuarioAlta);
                    this.MiAfiliado.UsuarioAlta.IdUsuarioAlta = this.MiAfiliado.UsuarioAlta.IdUsuario;
                    this.MiAfiliado.FechaAlta = DateTime.Now;
                    if (AfiliadosF.AfiliadosAgregar(this.MiAfiliado))
                    {
                        //this.MostrarMensaje(this.MiSolMat.CodigoMensaje, false, this.MiSolMat.CodigoMensajeArgs);
                        //this.RepNotaPedido.CargarReporte(this.MiAfiliado);
                        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje));
                    }
                    else
                    {
                        this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true, this.MiAfiliado.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Modificar:
                    if (AfiliadosF.AfiliadosModificar(this.MiAfiliado))
                    {
                        PaginaAfiliados pagina = new PaginaAfiliados();
                        AfiAfiliados afi = pagina.Obtener(this.MiSessionPagina);
                        if (afi.IdAfiliado == this.MiAfiliado.IdAfiliado)
                            pagina.Guardar(this.MiSessionPagina, this.MiAfiliado);
                        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje));
                    }
                    else
                    {
                        if (!this.MiAfiliado.ErrorAccesoDatos && this.MiAfiliado.ConfirmarAccion)
                        {
                            this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje, this.MiAfiliado.CodigoMensajeArgs), true);
                        }
                        else
                            this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true, this.MiAfiliado.CodigoMensajeArgs);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.AfiliadosModificarDatosCancelar != null)
                this.AfiliadosModificarDatosCancelar();
        }

        #region Cuenta Corriente Grilla
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            //GridViewExportUtil.Export("Datos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MiCuentaCorriente);
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            this.MiAfiliado.IdAfiliado = Convert.ToInt32(this.txtIdAfiliado.Text);
            this.MiAfiliado.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            this.MiAfiliado.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            //this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorCliente(this.MiAfiliado); ;
            //AyudaProgramacion.CargarGrillaListas<VTACuentasCorrientes>(this.MiCuentaCorriente, false, this.gvDatos, true);
            this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(this.MiAfiliado);
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            this.upCtaCte.Update();

        }


        protected void btnAgregarComprobante_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAgregar.aspx"), true);
        }

        protected void btnAgregarOC_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAgregar.aspx"), true);
        }

        protected void btnAgregarRemito_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosAgregar.aspx"), true);
        }

        protected void btnAgregarPresupuesto_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosAgregar.aspx"), true);
        }

        protected void btnAgregarNotaPedido_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosAgregar.aspx"), true);
        }
        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
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
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdMoneda";
            param.ValorParametro = (int)EnumTGEMonedas.PesosArgentinos;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdFilialEvento";
            param.ValorParametro = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            reporte.Parametros.Add(param);
            reporte.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);


            int idReporte = this.ddlTipoReporte.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoReporte.SelectedValue);

            TGEListasValoresDetalles aux = this.MisTiposReportes.Find(x => x.IdListaValorDetalle == idReporte);
            TGEPlantillas plantilla = new TGEPlantillas();
            if (aux != null)
            {
                plantilla.Codigo = aux.CodigoValor;
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);
                reporte.StoredProcedure = plantilla.NombreSP;
            }

            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, plantilla.KeysPDFCorte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.upImprimir, plantilla.Codigo, this.UsuarioActivo);

            this.upImprimir.Update();
        }

        protected void btnEnviarMail_Click(object sender, EventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
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
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdMoneda";
            param.ValorParametro = (int)EnumTGEMonedas.PesosArgentinos;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdFilialEvento";
            param.ValorParametro = this.UsuarioActivo.FilialPredeterminada.IdFilial;
            reporte.Parametros.Add(param);
            reporte.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            int idReporte = this.ddlTipoReporte.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoReporte.SelectedValue);


            TGEListasValoresDetalles aux = this.MisTiposReportes.Find(x => x.IdListaValorDetalle == idReporte);
            TGEPlantillas plantilla = new TGEPlantillas();
            if (aux != null)
            {
                plantilla.Codigo = aux.CodigoValor;
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);
                reporte.StoredProcedure = plantilla.NombreSP;
            }
            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

            MailMessage mail = new MailMessage();
            TGEArchivos archivo = new TGEArchivos();
            archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, plantilla.KeysPDFCorte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            //archivo.Archivo = AyudaProgramacionLN.StreamToByteArray(this.ctrPopUpComprobantes.GenerarReporteArchivo(reporte, EnumTGEComprobantes.VTAResumenCuenta));
            archivo.NombreArchivo = string.Concat(this.MiAfiliado.CUIL, ".pdf");

            mail.Attachments.Add(new Attachment(new MemoryStream(archivo.Archivo), archivo.NombreArchivo));
            if (AfiliadosF.AfiliadosArmarMailResumenCuenta(this.MiAfiliado, mail))
            {
                this.popUpMail.IniciarControl(mail, this.MiAfiliado);
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            int IdEstado = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdEstado")).Value);

            if (IdTipoOperacion == 0 || IdRefTipoOperacion == 0)
                return;

            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.HashTransaccion = tpCuentaCorriente.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;

            this.MisParametrosUrl = new Hashtable();

            if (e.CommandName == Gestion.Impresion.ToString())
            {
                if (IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                     || IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas)
                {
                    CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
                    ordenCobro.IdOrdenCobro = IdRefTipoOperacion;
                    ordenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, "OrdenesCobros", ordenCobro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.upCtaCte, "OrdenesCobros", this.UsuarioActivo);
                }
                else
                {
                    try
                    {
                        VTAFacturas factura = new VTAFacturas();
                        factura.IdFactura = IdRefTipoOperacion;
                        factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                        factura = FacturasF.FacturasObtenerDatosCompletos(factura);
                        VTAFacturas facturaPdf = FacturasF.FacturasObtenerArchivo(factura);
                        List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                        TGEArchivos archivo = new TGEArchivos();
                        archivo.Archivo = facturaPdf.FacturaPDF;
                        if (archivo.Archivo != null)
                            listaArchivos.Add(archivo);

                        VTARemitos remito = FacturasF.RemitosObtenerPorFactura(factura);
                        if (remito.IdRemito > 0)
                        {
                            archivo = new TGEArchivos();
                            remito.UsuarioLogueado = factura.UsuarioLogueado;
                            remito = FacturasF.RemitosObtenerArchivo(remito);
                            archivo.Archivo = remito.RemitoPDF;
                            if (archivo.Archivo != null)
                                listaArchivos.Add(archivo);
                        }
                        TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                        string nombreArchivo = string.Concat(empresa.CUIT, "_", factura.TipoFactura.CodigoValor, "_", factura.PrefijoNumeroFactura, "_", factura.NumeroFactura, ".pdf");
                        ExportPDF.ConvertirArchivoEnPdf(this.upCtaCte, listaArchivos, nombreArchivo);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
            }
            else
            {
                //Filtro para Obtener URL y NombreParametro
                Menues filtroMenu = new Menues();
                filtroMenu.IdTipoOperacion = IdTipoOperacion;
                //Control de Tipo de Menues (SOLO CONSULTA)
                if (e.CommandName == Gestion.Consultar.ToString())
                {
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;
                }
                //Control de Tipo de Menues (Anular)
                else if (e.CommandName == Gestion.Anular.ToString())
                {
                    if (IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                        || IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas)
                    {
                        if (IdEstado == (int)EstadosFacturas.Cobrada)
                            filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_AnularConfirmar;
                        else
                            filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;
                    }
                    else
                        filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;
                }
                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
                this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
                this.MisParametrosUrl.Add("hdfIdRefTipoOperacion", IdRefTipoOperacion);
                this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
                //Si devuelve una URL Redirecciona si no muestra mensaje error
                if (filtroMenu.URL.Length != 0)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
                }
                else
                {
                    this.MiAfiliado.CodigoMensaje = "ErrorURLNoValida";
                    this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true);
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (Convert.ToBoolean(dr["PuedeAnular"]))
                {
                    ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                    ibtnAnular.Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            parametros.HashTransaccion = tpRemitos.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();

            //VTAFacturas parametrosFacturas = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<VTAFacturas>(parametrosFacturas);

            //gvDatos.PageIndex = e.NewPageIndex;
            //gvDatos.DataSource = this.MisFacturas;
            //gvDatos.DataBind();

            //CobOrdenesCobros parametrosCob = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<CobOrdenesCobros>(parametrosCob);

            //gvDatos.PageIndex = e.NewPageIndex;
            //gvDatos.DataSource = this.MisOrdenesCobros;
            //gvDatos.DataBind();
            //AyudaProgramacion.CargarGrillaListas(this.MiCuentaCorriente, false, this.gvDatos, true);
        }
        #endregion

        #region Cuenta Corriente Grilla Dolar
        protected void btnExportarExcelDolar_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatosDolar.AllowPaging = false;
            this.gvDatosDolar.DataSource = this.MiCuentaCorrienteDolar;
            this.gvDatosDolar.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatosDolar);
        }

        protected void btnFiltrarDolar_Click(object sender, EventArgs e)
        {

            //this.MiCuentaCorriente = FacturasF.CuentasCorrientesSeleccionarPorCliente(this.MiAfiliado); ;
            //AyudaProgramacion.CargarGrillaListas<VTACuentasCorrientes>(this.MiCuentaCorriente, false, this.gvDatos, true);
            CuentasCorrientesFiltro cuenta = new CuentasCorrientesFiltro();
            cuenta.FechaDesde = this.txtFechaDesdeDolar.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesdeDolar.Text);
            cuenta.FechaHasta = this.txtFechaHastaDolar.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHastaDolar.Text);
            cuenta.IdAfiliado = MiAfiliado.IdAfiliado;
            cuenta.IdMoneda = (int)EnumTGEMonedas.DolarEEUU;
            this.MiCuentaCorrienteDolar = FacturasF.CuentasCorrientesSeleccionarPorClienteTable(cuenta);
            this.gvDatosDolar.DataSource = this.MiCuentaCorrienteDolar;
            this.gvDatosDolar.DataBind();
            this.upCtaCteDolar.Update();

        }

        protected void btnAgregarComprobanteDolar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAgregar.aspx"), true);
        }

        protected void btnAgregarOCDolar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAgregar.aspx"), true);
        }

        protected void btnAgregarRemitoDolar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosAgregar.aspx"), true);
        }

        protected void btnAgregarPresupuestoDolar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosAgregar.aspx"), true);
        }

        protected void btnImprimirDolar_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaDesde";
            param.ValorParametro = this.txtFechaDesdeDolar.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesdeDolar.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = this.txtFechaHastaDolar.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaHastaDolar.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "SoloDeuda";
            param.ValorParametro = this.ddlTipoReporteDolar.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoReporteDolar.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdTipoFactura";
            param.ValorParametro = this.ddlTipoComprobanteDolar.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoComprobanteDolar.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdMoneda";
            param.ValorParametro = (int)EnumTGEMonedas.DolarEEUU;
            reporte.Parametros.Add(param);

            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.VTAResumenCuenta);

            reporte.StoredProcedure = comprobante.NombreSP;
            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

            //  byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(comprobante, new TGEPlantillas(), ds, "IdAfiliado", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));

            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "ResumenCuenta";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);
            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(comprobante, plantilla, ds, "IdAfiliado", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));

            ExportPDF.ExportarPDF(pdf, this.UpImprimirDolar, "VTAResumenCuenta", this.UsuarioActivo);
            this.UpImprimirDolar.Update();

        }

        protected void btnEnviarMailDolar_Click(object sender, EventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado;
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaDesde";
            param.ValorParametro = this.txtFechaDesdeDolar.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesdeDolar.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            param.Parametro = "FechaHasta";
            param.ValorParametro = this.txtFechaHastaDolar.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaHastaDolar.Text);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "SoloDeuda";
            param.ValorParametro = this.ddlTipoReporteDolar.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoReporteDolar.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdTipoFactura";
            param.ValorParametro = this.ddlTipoComprobanteDolar.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlTipoComprobanteDolar.SelectedValue);
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdMoneda";
            param.ValorParametro = (int)EnumTGEMonedas.DolarEEUU;
            reporte.Parametros.Add(param);

            MailMessage mail = new MailMessage();
            TGEArchivos archivo = new TGEArchivos();
            TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.VTAResumenCuenta);

            reporte.StoredProcedure = comprobante.NombreSP;
            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);


            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "ResumenCuenta";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);
            archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(comprobante, plantilla, ds, "IdAfiliado", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            //archivo.Archivo = AyudaProgramacionLN.StreamToByteArray(this.ctrPopUpComprobantes.GenerarReporteArchivo(reporte, EnumTGEComprobantes.VTAResumenCuenta));
            archivo.NombreArchivo = string.Concat(this.MiAfiliado.CUIL, ".pdf");

            mail.Attachments.Add(new Attachment(new MemoryStream(archivo.Archivo), archivo.NombreArchivo));
            if (AfiliadosF.AfiliadosArmarMailResumenCuenta(this.MiAfiliado, mail))
            {
                this.PopUpEnviarMailDolar.IniciarControl(mail, this.MiAfiliado);

            }
        }

        protected void gvDatosDolar_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatosDolar.Rows[index].FindControl("hdfIdTipoOperacionDolar")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatosDolar.Rows[index].FindControl("hdfIdRefTipoOperacionDolar")).Value);

            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.HashTransaccion = tpCuentaCorrienteDolar.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;

            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //if (e.CommandName == Gestion.Impresion.ToString())
            //{
            //    VTAFacturas factura = new VTAFacturas();
            //    //factura.IdFactura = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdFactura"].ToString());

            //    factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //    factura = FacturasF.FacturasObtenerDatosCompletos(factura);
            //    VTAFacturas facturaPdf = FacturasF.FacturasObtenerArchivo(factura);
            //    List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            //    TGEArchivos archivo = new TGEArchivos();
            //    archivo.Archivo = facturaPdf.FacturaPDF;
            //    if (archivo.Archivo != null)
            //        listaArchivos.Add(archivo);

            //    VTARemitos remito = FacturasF.RemitosObtenerPorFactura(factura);
            //    if (remito.IdRemito > 0)
            //    {
            //        archivo = new TGEArchivos();
            //        remito.UsuarioLogueado = factura.UsuarioLogueado;
            //        remito = FacturasF.RemitosObtenerArchivo(remito);
            //        archivo.Archivo = remito.RemitoPDF;
            //        if (archivo.Archivo != null)
            //            listaArchivos.Add(archivo);
            //    }
            //    TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            //    string nombreArchivo = string.Concat(empresa.CUIT, "_", factura.TipoFactura.CodigoValor, "_", factura.PrefijoNumeroFactura, "_", factura.NumeroFactura, ".pdf");
            //    ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel1, listaArchivos, nombreArchivo);
            //}
            //else
            //{
            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)
            if (e.CommandName == Gestion.Consultar.ToString())
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;

            //Control de Tipo de Menues (Anular)
            if (e.CommandName == Gestion.Anular.ToString())
                filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;

            //Guardo Menu devuelto de la DB
            filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
            this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
            this.MisParametrosUrl.Add("hdfIdRefTipoOperacion", IdRefTipoOperacion);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            //Si devuelve una URL Redirecciona si no muestra mensaje error
            if (filtroMenu.URL.Length != 0)
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
            }
            else
            {
                this.MiAfiliado.CodigoMensaje = "ErrorURLNoValida";
                this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true);
            }
            //}


        }

        protected void gvDatosDolar_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;

                if (Convert.ToBoolean(dr["PuedeAnular"]))
                {
                    ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnularDolar");
                    ibtnAnular.Visible = true;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        protected void gvDatosDolar_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            parametros.HashTransaccion = tpRemitos.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            this.gvDatosDolar.PageIndex = e.NewPageIndex;
            this.gvDatosDolar.DataSource = this.MiCuentaCorrienteDolar;
            this.gvDatosDolar.DataBind();

            //VTAFacturas parametrosFacturas = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<VTAFacturas>(parametrosFacturas);

            //gvDatos.PageIndex = e.NewPageIndex;
            //gvDatos.DataSource = this.MisFacturas;
            //gvDatos.DataBind();

            //CobOrdenesCobros parametrosCob = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<CobOrdenesCobros>(parametrosCob);

            //gvDatos.PageIndex = e.NewPageIndex;
            //gvDatos.DataSource = this.MisOrdenesCobros;
            //gvDatos.DataBind();
            //AyudaProgramacion.CargarGrillaListas(this.MiCuentaCorriente, false, this.gvDatos, true);
        }
        #endregion

        #region Notas de pedidos

        protected void gvNotasPedidos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTANotasPedidos notaPedido = new VTANotasPedidos();
            notaPedido.IdNotaPedido = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdNotaPedido"].ToString());

            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.HashTransaccion = tpNotasPedidos.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdNotaPedido", notaPedido.IdNotaPedido);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/NotasPedidosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {

                List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                TGEArchivos archivo = new TGEArchivos();

                notaPedido = FacturasF.NotasPedidosObtenerArchivo(notaPedido);
                archivo.Archivo = notaPedido.NotaPedidoPDF;
                listaArchivos.Add(archivo);
                TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                string nombreArchivo = string.Concat(empresa.CUIT, "_NotaPedido_", notaPedido.IdNotaPedido.ToString().PadLeft(10, '0'), ".pdf");
                ExportPDF.ConvertirArchivoEnPdf(this.upNotasPedidos, listaArchivos, nombreArchivo);
            }
        }

        protected void gvNotasPedidos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton btnBaja = (ImageButton)e.Row.FindControl("btnBaja");
                ImageButton btnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                DataRowView dr = (DataRowView)e.Row.DataItem;

                if (dr["IdNotaPedido"] != DBNull.Value)
                {
                    btnImprimir.Visible = true;
                    ibtnConsultar.Visible = this.ValidarPermiso("NotasPedidosConsultar.aspx");
                    btnBaja.Visible = this.ValidarPermiso("NotasPedidosAnular.aspx");
                    modificar.Visible = this.ValidarPermiso("NotasPedidosModificar.aspx");
                    switch (Convert.ToInt32(dr["IdEstado"]))
                    {
                        case (int)EstadosRemitos.PendienteEntrega:
                            break;
                        case (int)EstadosRemitos.EnDistribucion:
                            btnBaja.Visible = this.ValidarPermiso("NotasPedidosAnular.aspx");
                            modificar.Visible = this.ValidarPermiso("NotasPedidosModificar.aspx");
                            break;
                        default:
                            break;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisRemitos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvNotasPedidos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            parametros.HashTransaccion = tpNotasPedidos.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            gvNotasPedidos.PageIndex = e.NewPageIndex;
            gvNotasPedidos.DataSource = this.MisNotasPedidos;
            gvNotasPedidos.DataBind();
        }

        protected void gvNotasPedidos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisNotasPedidos = this.OrdenarGrillaDatos<DataTable>(this.MisNotasPedidos, e);
            this.gvNotasPedidos.DataSource = this.MisNotasPedidos;
            this.gvNotasPedidos.DataBind();
        }
        #endregion

        #region Remitos
        protected void gvRemitosPendientes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = Convert.ToDecimal(this.MisRemitosPendientes.Compute("Sum(SubTotalItem)", string.Empty)).ToString("C2");
            }
        }

        protected void gvRemitos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTARemitos remito = new VTARemitos();
            remito.IdRemito = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdRemito"].ToString());
            remito.NumeroRemitoCompleto = ((GridView)sender).DataKeys[index]["NumeroRemitoCompleto"].ToString();

            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.HashTransaccion = tpRemitos.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdRemito", remito.IdRemito);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                //VTARemitos remitoPDF = new VTARemitos();
                //remitoPDF.IdRemito = remito.IdRemito;
                //remitoPDF = FacturasF.RemitosObtenerArchivo(remitoPDF);
                //TGEArchivos archivo = new TGEArchivos();
                //archivo.Archivo = remitoPDF.RemitoPDF;
                //archivo.NombreArchivo = string.Concat(remito.NumeroRemitoPrefijo, "-", remito.NumeroRemitoSuFijo, ".pdf");
                //archivo.TipoArchivo = "application/pdf";
                //this.ctrPopUpComprobantes.CargarArchivo(archivo);
                VTARemitos remitoPDF = new VTARemitos();
                remitoPDF.IdRemito = remito.IdRemito;
                remitoPDF.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                remitoPDF = FacturasF.RemitosObtenerArchivo(remitoPDF);
                List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                TGEArchivos archivo = new TGEArchivos();
                archivo.Archivo = remitoPDF.RemitoPDF;
                if (archivo.Archivo != null)
                    listaArchivos.Add(archivo);
                TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                string nombreArchivo = string.Concat(empresa.CUIT, "_", remito.NumeroRemitoCompleto, ".pdf");
                ExportPDF.ConvertirArchivoEnPdf(this.upRemitos, listaArchivos, nombreArchivo);
            }
            else if (e.CommandName == Gestion.EnviarMail.ToString())
            {
                MailMessage mail = new MailMessage();
                remito = FacturasF.RemitosObtenerDatosCompletos(remito);
                if (FacturasF.RemitoArmarMail(remito, mail))
                {
                    this.popUpMail.IniciarControl(mail, remito);
                }
            }
        }

        protected void gvRemitos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton baja = (ImageButton)e.Row.FindControl("btnBaja");
                ImageButton btnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                DataRowView dr = (DataRowView)e.Row.DataItem;

                if (dr["IdRemito"] != DBNull.Value)
                {
                    btnImprimir.Visible = true;
                    ibtnConsultar.Visible = this.ValidarPermiso("RemitosConsultar.aspx");
                    switch (Convert.ToInt32(dr["IdEstado"]))
                    {
                        case (int)EstadosRemitos.PendienteEntrega:
                        case (int)EstadosRemitos.EnDistribucion:
                        case (int)EstadosRemitos.EnDespacho:
                            baja.Visible = this.ValidarPermiso("RemitosAnular.aspx");
                            modificar.Visible = this.ValidarPermiso("RemitosModificar.aspx");
                            break;
                        case (int)EstadosRemitos.Entregado:
                            baja.Visible = this.ValidarPermiso("RemitosAnular.aspx");
                            modificar.Visible = this.ValidarPermiso("RemitosModificar.aspx");
                            break;
                        default:
                            break;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisRemitos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvRemitos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            parametros.HashTransaccion = tpRemitos.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            gvRemitos.PageIndex = e.NewPageIndex;
            gvRemitos.DataSource = this.MisRemitos;
            gvRemitos.DataBind();
        }

        protected void gvRemitos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisRemitos = this.OrdenarGrillaDatos<DataTable>(this.MisRemitos, e);
            this.gvRemitos.DataSource = this.MisRemitos;
            this.gvRemitos.DataBind();
        }

        protected void btnFiltrarRemitos_Click(object sender, EventArgs e)
        {
            VTARemitos remitos = new VTARemitos();
            remitos.Afiliado.IdAfiliado = Convert.ToInt32(this.txtIdAfiliado.Text);
            remitos.FechaDesde = this.txtFechaDesdeRemitos.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesdeRemitos.Text);
            remitos.FechaHasta = this.txtFechaHastaRemitos.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHastaRemitos.Text);
            this.MisRemitos = FacturasF.RemitosObtenerAfiliadoGrilla(remitos);
            this.gvRemitos.DataSource = this.MisRemitos;
            this.gvRemitos.DataBind();
            this.btnExportarExcelRemitos.Visible = this.MisRemitos.Rows.Count > 0;
            this.upRemitos.Update();

            //bool actualizaUpEnviarM = false;
            //if (this.MisRemitos.Rows.Count > 0)
            //{
            //    btnExportarExcelRemitos.Visible = true;
            //    actualizaUpEnviarM = !this.ctrEnviarMails.Visible;
            //    this.ctrEnviarMails.Visible = true;
            //}
            //else
            //{
            //    btnExportarExcelRemitos.Visible = false;
            //    actualizaUpEnviarM = this.ctrEnviarMails.Visible;
            //    this.ctrEnviarMails.Visible = false;
            //}
            //if (actualizaUpEnviarM)
            //    this.upEnviarMails.Update();

        }
        protected void btnExportarExcelRemitos_Click(object sender, ImageClickEventArgs e)
        {
            this.gvRemitos.AllowPaging = false;
            this.gvRemitos.DataSource = this.MisRemitos;
            this.gvRemitos.DataBind();
            //GridViewExportUtil.Export("Datos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MisRemitos);
        }
        #endregion

        #region Presupuestos

        protected void gvPresupuestos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idPresupuesto = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //VTAPresupuestos factura = this.MisDatosGrillas[id];

            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.HashTransaccion = tpPresupuestos.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            this.MisParametrosUrl.Add("IdPresupuesto", idPresupuesto);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                TGEArchivos archivo = new TGEArchivos();
                VTAPresupuestos presupuestoPDF = new VTAPresupuestos();
                presupuestoPDF.IdPresupuesto = idPresupuesto;
                presupuestoPDF.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                presupuestoPDF = FacturasF.PresupuestosObtenerArchivo(presupuestoPDF);
                archivo.Archivo = presupuestoPDF.PresupuestoPDF;
                listaArchivos.Add(archivo);
                TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                string nombreArchivo = string.Concat(empresa.CUIT, "_Presupuesto_", idPresupuesto.ToString().PadLeft(10, '0'), ".pdf");
                ExportPDF.ConvertirArchivoEnPdf(this.upPresupuestos, listaArchivos, nombreArchivo);
            }
        }

        protected void gvPresupuestos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("FacturasConsultar.aspx");
                //ibtnConsultar.Visible = true;

                //DataRow fila = (DataRow)e.Row.DataItem;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPresupuestos.Rows.Count);
            }
        }

        protected void gvPresupuestos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            parametros.HashTransaccion = tpPresupuestos.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisPresupuestos;
            gvDatos.DataBind();
        }

        protected void gvPresupuestos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //DataTable dataTable = gvDatos.DataSource as DataTable;
            DataView dataView = new DataView(this.MisPresupuestos);
            dataView.Sort = e.SortExpression + " " + e.SortDirection.ToString().Substring(0, 3).ToUpper();
            this.gvDatos.DataSource = dataView;
            this.gvDatos.DataBind();

            //this.MisDatosGrillas = this.OrdenarGrillaDatos<VTAFacturas>(this.MisDatosGrillas, e);
            //this.gvDatos.DataSource = this.MisDatosGrillas;
            //this.gvDatos.DataBind();
        }

        #endregion

        #region Domicilios

        protected void btnAgregarDomicilio_Click(object sender, EventArgs e)
        {
            this.ctrDomicilios.IniciarControl(new AfiDomicilios(), Gestion.Agregar);
        }

        void ctrDomicilios_AfiliadosModificarDatosAceptar(object sender, AfiDomicilios e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiAfiliado.Domicilios.Add(e);
                    e.IndiceColeccion = this.MiAfiliado.Domicilios.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiAfiliado.Domicilios[this.MiIndiceDetalleModificar] = e;

                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiAfiliado.Domicilios, true, this.gvDomicilios, true);
            this.upDomicilios.Update();
        }

        protected void gvDomicilios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrDomicilios.IniciarControl(this.MiAfiliado.Domicilios[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrDomicilios.IniciarControl(this.MiAfiliado.Domicilios[indiceColeccion], Gestion.Consultar);
            }
        }

        protected void gvDomicilios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiDomicilios item = (AfiDomicilios)e.Row.DataItem;

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
                this.ctrTelefonos.IniciarControl(this.MiAfiliado.Telefonos[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrTelefonos.IniciarControl(this.MiAfiliado.Telefonos[indiceColeccion], Gestion.Consultar);
            }
        }

        protected void gvTelefonos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiTelefonos item = (AfiTelefonos)e.Row.DataItem;

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

        void ctrTelefonos_AfiliadosModificarDatosAceptar(object sender, AfiTelefonos e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiAfiliado.Telefonos.Add(e);
                    e.IndiceColeccion = this.MiAfiliado.Telefonos.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiAfiliado.Telefonos[this.MiIndiceDetalleModificar] = e;
                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiAfiliado.Telefonos, true, this.gvTelefonos, true);
            this.upTelefonos.Update();
        }

        protected void btnAgregarTelefono_Click(object sender, EventArgs e)
        {
            AfiTelefonos telefono = new AfiTelefonos();
            telefono.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.ctrTelefonos.IniciarControl(telefono, Gestion.Agregar);
        }

        #endregion

    }
}