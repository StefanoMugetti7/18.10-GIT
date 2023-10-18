using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using Contabilidad;
using Contabilidad.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;

namespace IU.Modulos.Bancos.Controles
{
    public partial class BancosCuentasMovimientosDatos : ControlesSeguros
    {
        private TESBancosCuentasMovimientos MiMovimiento
        {
            get { return (TESBancosCuentasMovimientos)Session[this.MiSessionPagina + "BancosCuentasMovimientosDatosMiMovimiento"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosDatosMiMovimiento"] = value; }
        }

        private List<TGETiposOperaciones> MisTiposOperaciones
        {
            get { return (List<TGETiposOperaciones>)Session[this.MiSessionPagina + "BancosCuentasMovimientosDatosMisTiposOperaciones"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosDatosMisTiposOperaciones"] = value; }
        }

        private List<CtbConceptosContables> MisConceptosContables
        {
            get { return (List<CtbConceptosContables>)Session[this.MiSessionPagina + "BancosCuentasMovimientosDatosMisConceptosContables"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasMovimientosDatosMisConceptosContables"] = value; }
        }

        //public delegate void BancosCuentasMovimientosDatosAceptarEventHandler(object sender, TESBancosCuentasMovimientos e);
        //public event BancosCuentasMovimientosDatosAceptarEventHandler BancosCuentasMovimientosDatosAceptar;

        public delegate void BancosCuentasMovimientosDatosCancelarEventHandler();
        public event BancosCuentasMovimientosDatosCancelarEventHandler BancosCuentasMovimientosDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
        }

        /// <summary>
        /// Inicializa el control de Movimientos de Cuentas de Ahorros
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TESBancosCuentasMovimientos pMovimiento, Gestion pGestion)
        {
            this.MiMovimiento = pMovimiento;
            this.GestionControl = pGestion;
            this.CargarCombo();

            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiMovimiento.BancoCuenta = BancosF.BancosCuentasObtenerDatosCompletos(this.MiMovimiento.BancoCuenta);
                    SetInitializeCulture(MiMovimiento.BancoCuenta.Moneda.Moneda);
                    this.MapearBancoAControles();
                    this.txtFechaMovimiento.Text = DateTime.Now.ToShortDateString();
                    this.txtFechaConciliacionBanco.Enabled = true;
                    this.rfvFechaConciliacionBanco.Enabled = true;
                    this.ctrArchivos.IniciarControl(this.MiMovimiento, this.GestionControl);
                    this.ctrComentarios.IniciarControl(this.MiMovimiento, this.GestionControl);
                    break;
                case Gestion.Anular:
                    this.MiMovimiento = BancosF.BancosCuentasMovimientosObtenerDatosCompletos(this.MiMovimiento);
                    this.SetInitializeCulture(this.MiMovimiento.BancoCuenta.Moneda.Moneda);
                    this.MapearBancoAControles();
                    this.MapearObjetoAControles();
                    this.DeshabilitarControles();
                    break;
                case Gestion.Consultar:
                    this.MiMovimiento = BancosF.BancosCuentasMovimientosObtenerDatosCompletos(this.MiMovimiento);
                    this.SetInitializeCulture(this.MiMovimiento.BancoCuenta.Moneda.Moneda);
                    this.MapearObjetoAControles();
                    this.MapearBancoAControles();
                    this.DeshabilitarControles();
                    this.btnAceptar.Visible = false;
                    this.btnImprimir.Visible = true;
                    break;
                default:
                    break;
            }
        }

        private void DeshabilitarControles()
        {
            this.txtImporte.Enabled = false;
            this.txtNumeroTipoOperacion.Enabled = false;
            this.txtDetalle.Enabled = false;
            this.ddlTipoOperacion.Enabled = false;
            this.ddlConceptosContables.Enabled = false;
            this.ddlBancos.Enabled = false;
            this.ddlBancosCuentas.Enabled = false;
            this.ddlFiliales.Enabled = false;
        }

        private void CargarCombo()
        {
            TGETiposOperaciones opFiltro = new TGETiposOperaciones();
            opFiltro.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            this.MisTiposOperaciones = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(opFiltro);
            this.ddlTipoOperacion.DataSource = this.MisTiposOperaciones;
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            if (this.ddlTipoOperacion.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoOperacion_OnSelectedIndexChanged(null, EventArgs.Empty);

            AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptosContables, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlTipoOperacion_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            this.pnlFilialDestino.Visible = false;
            this.rfvFiliales.Enabled = false;
            this.pnlCuentaInternaDestino.Visible = false;
            this.rfvBancos.Enabled = false;
            this.rfvBancosCuentas.Enabled = false;

            if (string.IsNullOrEmpty(this.ddlTipoOperacion.SelectedValue))
                return;

            TGETiposOperaciones opeFiltro = new TGETiposOperaciones();
            opeFiltro.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);// this.paginaSegura.paginaActual.IdTipoFuncionalidad;//(int)EnumTGETiposFuncionalidades.Cobros;
            opeFiltro.Estado.IdEstado = (int)Estados.Activo;

            this.ddlConceptosContables.Items.Clear();
            this.ddlConceptosContables.SelectedIndex = -1;
            this.ddlConceptosContables.SelectedValue = null;
            this.ddlConceptosContables.ClearSelection();

            this.MisConceptosContables = ContabilidadF.ConceptosContablesObtenerListaFiltro(opeFiltro);
            this.ddlConceptosContables.DataSource = this.MisConceptosContables;
            this.ddlConceptosContables.DataValueField = "IdConceptoContable";
            this.ddlConceptosContables.DataTextField = "ConceptoContable";
            this.ddlConceptosContables.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptosContables, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            ListItem item = this.ddlConceptosContables.Items.FindByValue(this.MiMovimiento.ConceptoContable.IdConceptoContable.ToString());
            if (item != null)
                this.ddlConceptosContables.SelectedValue = this.MiMovimiento.ConceptoContable.IdConceptoContable.ToString();


            if (opeFiltro.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                || opeFiltro.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito)
            {
                this.pnlFilialDestino.Visible = true;
                this.rfvFiliales.Enabled = true;
                this.ddlFiliales.DataSource = BancosF.BancosFilialesObtenerListaTransferenciaDestino(this.MiMovimiento.BancoCuenta.Filial);
                this.ddlFiliales.DataValueField = "IdFilial";
                this.ddlFiliales.DataTextField = "Filial";
                this.ddlFiliales.DataBind();
            }
            else if (opeFiltro.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito)
            {
                this.pnlFilialDestino.Visible = true;
                this.rfvFiliales.Enabled = true;
                List<TGEFiliales> filiales = new List<TGEFiliales>();
                filiales.Add(this.MiMovimiento.BancoCuenta.Filial);
                this.ddlFiliales.DataSource = filiales;// BancosF.BancosFilialesObtenerListaTransferenciaDestino(this.MiMovimiento.BancoCuenta.Filial);
                this.ddlFiliales.DataValueField = "IdFilial";
                this.ddlFiliales.DataTextField = "Filial";
                this.ddlFiliales.DataBind();
            }

            if (opeFiltro.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                || opeFiltro.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
                )
            {
                this.pnlCuentaInternaDestino.Visible = true;
                this.rfvBancos.Enabled = true;
                this.rfvBancosCuentas.Enabled = true;

                if (this.ddlBancos.Items.Count > 1)
                {
                    this.ddlBancos.Items.Clear();
                    this.ddlBancos.ClearSelection();
                    this.ddlBancos.SelectedValue = null;
                    this.ddlBancos.SelectedIndex = -1;
                }

                if (this.ddlBancosCuentas.Items.Count > 1)
                {
                    this.ddlBancosCuentas.Items.Clear();
                    this.ddlBancosCuentas.ClearSelection();
                    this.ddlBancosCuentas.SelectedValue = null;
                    this.ddlBancosCuentas.SelectedIndex = -1;
                }

                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.ddlFiliales_OnSelectedIndexChanged(null, EventArgs.Empty);

            }
            this.ddlConceptosContables_OnSelectedIndexChanged(null, EventArgs.Empty);
        }
        protected void ddlConceptosContables_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlConceptosContables.SelectedValue))
            {
                MiMovimiento.ConceptoContable.IdConceptoContable = Convert.ToInt32(ddlConceptosContables.SelectedValue);
                this.ctrCamposValores.IniciarControl(this.MiMovimiento, MiMovimiento.ConceptoContable, this.GestionControl);
            }
            else
            {

            }
        }
        protected void ddlFiliales_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFiliales.SelectedValue))
            {
                TGEFiliales filial = new TGEFiliales();
                filial.IdFilial = Convert.ToInt32(this.ddlFiliales.SelectedValue);
                filial.Estado.IdEstado = (int)Estados.Activo;

                List<TESBancos> lista = BancosF.BancosObtenerListaFilialFiltro(filial);
                if (lista.Count > 0)
                {
                    this.ddlBancos.Items.Clear();
                    this.ddlBancos.ClearSelection();
                    this.ddlBancos.SelectedValue = null;
                    this.ddlBancos.SelectedIndex = -1;
                    this.ddlBancos.DataSource = lista;
                    this.ddlBancos.DataValueField = "IdBanco";
                    this.ddlBancos.DataTextField = "Descripcion";
                    this.ddlBancos.DataBind();
                }
                if (this.ddlBancos.Items.Count > 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.ddlBancos_OnSelectedIndexChanged(null, EventArgs.Empty);
            }
        }

        protected void ddlBancos_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlBancos.SelectedValue))
            {
                TESBancosCuentas bancoCuenta = new TESBancosCuentas();
                bancoCuenta.Banco.IdBanco = Convert.ToInt32(this.ddlBancos.SelectedValue);
                bancoCuenta.Filial.IdFilial = Convert.ToInt32(this.ddlFiliales.SelectedValue);
                bancoCuenta.Moneda.IdMoneda = this.MiMovimiento.BancoCuenta.Moneda.IdMoneda;
                bancoCuenta.Estado.IdEstado = (int)Estados.Activo;
                bancoCuenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                List<TESBancosCuentas> lista = BancosF.BancosCuentasObtenerListaFiltro(bancoCuenta);
                lista = lista.Where(x => x.IdBancoCuenta != MiMovimiento.BancoCuenta.IdBancoCuenta).ToList();
                this.ddlBancosCuentas.Items.Clear();
                this.ddlBancosCuentas.ClearSelection();
                this.ddlBancosCuentas.SelectedValue = null;
                this.ddlBancosCuentas.SelectedIndex = -1;

                if (lista.Count > 0)
                {
                    this.ddlBancosCuentas.DataSource = lista;
                    this.ddlBancosCuentas.DataValueField = "IdBancoCuenta";
                    this.ddlBancosCuentas.DataTextField = "DescripcionFilialBancoTipoCuentaNumero";
                    this.ddlBancosCuentas.DataBind();
                }

                if (this.ddlBancosCuentas.Items.Count == 0 || this.ddlBancosCuentas.Items.Count > 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }

        private void MapearBancoAControles()
        {
            this.txtBanco.Text = this.MiMovimiento.BancoCuenta.Banco.Descripcion;
            this.txtNumeroCuenta.Text = this.MiMovimiento.BancoCuenta.NumeroCuenta;
            this.txtEstado.Text = this.MiMovimiento.BancoCuenta.Banco.Estado.Descripcion;
            this.txtDenominacion.Text = this.MiMovimiento.BancoCuenta.Denominacion;
            this.txtNumeroBancoSucursal.Text = this.MiMovimiento.BancoCuenta.NumeroBancoSucursal;
            this.txtTipoCuenta.Text = this.MiMovimiento.BancoCuenta.BancoCuentaTipo.Descripcion;
        }
        private void MapearObjetoAControles()
        {
            this.txtFechaMovimiento.Text = this.MiMovimiento.FechaMovimiento.ToShortDateString();
            this.txtImporte.Text = this.MiMovimiento.Importe.ToString("C2");
            this.txtNumeroTipoOperacion.Text = this.MiMovimiento.NumeroTipoOperacion;
            this.txtDetalle.Text = this.MiMovimiento.Detalle;
            this.txtEstado.Text = this.MiMovimiento.Estado.Descripcion;

            ListItem item = this.ddlConceptosContables.Items.FindByValue(this.MiMovimiento.ConceptoContable.IdConceptoContable.ToString());
            if (item == null)
                this.ddlConceptosContables.Items.Add(new ListItem(this.MiMovimiento.ConceptoContable.ConceptoContable, this.MiMovimiento.ConceptoContable.IdConceptoContable.ToString()));
            this.ddlConceptosContables.SelectedValue = this.MiMovimiento.ConceptoContable.IdConceptoContable.ToString();
            this.ctrArchivos.IniciarControl(this.MiMovimiento, this.GestionControl);
            this.ctrComentarios.IniciarControl(this.MiMovimiento, this.GestionControl);

            this.ddlTipoOperacion.Items.Add(new ListItem(this.MiMovimiento.TipoOperacion.TipoOperacion, this.MiMovimiento.TipoOperacion.IdTipoOperacion.ToString()));
            this.ddlTipoOperacion.SelectedValue = this.MiMovimiento.TipoOperacion.IdTipoOperacion.ToString();
            if (Convert.ToInt32(ddlTipoOperacion.SelectedValue) > 0)
            {
                this.ddlTipoOperacion_OnSelectedIndexChanged(null, EventArgs.Empty);
            }
            //this.ddlConceptosContables.Items.Add(new ListItem(this.MiMovimiento.ConceptoContable.ConceptoContable, this.MiMovimiento.ConceptoContable.IdConceptoContable.ToString()));
            //this.ddlConceptosContables.SelectedValue = this.MiMovimiento.ConceptoContable.IdConceptoContable.ToString();

            if (this.MiMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaDebito)
            {
                this.pnlFilialDestino.Visible = true;
                this.ddlFiliales.Items.Add(new ListItem(this.MiMovimiento.BancoCuenta.Filial.Filial, this.MiMovimiento.BancoCuenta.Filial.IdFilial.ToString()));
                this.ddlFiliales.SelectedValue = this.MiMovimiento.BancoCuenta.Filial.IdFilial.ToString();
            }

            if (this.MiMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasDebito
                || this.MiMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito)
            {
                this.pnlFilialDestino.Visible = true;
                this.ddlFiliales.Items.Add(new ListItem(this.MiMovimiento.BancoCuentaDestino.Filial.Filial, this.MiMovimiento.BancoCuentaDestino.Filial.IdFilial.ToString()));
                this.ddlFiliales.SelectedValue = this.MiMovimiento.BancoCuentaDestino.Filial.IdFilial.ToString();

                this.pnlCuentaInternaDestino.Visible = true;
                this.ddlBancos.Items.Add(new ListItem(this.MiMovimiento.BancoCuentaDestino.Banco.Descripcion, this.MiMovimiento.BancoCuentaDestino.Banco.IdBanco.ToString()));
                this.ddlBancos.SelectedValue = this.MiMovimiento.BancoCuentaDestino.Banco.IdBanco.ToString();

                this.ddlBancosCuentas.Items.Add(new ListItem(this.MiMovimiento.BancoCuentaDestino.DescripcionFilialBancoTipoCuentaNumero, this.MiMovimiento.BancoCuenta.IdBancoCuenta.ToString()));
                this.ddlBancosCuentas.SelectedValue = this.MiMovimiento.BancoCuentaDestino.IdBancoCuenta.ToString();
            }
            this.ctrAsientoMostrar.IniciarControl(this.MiMovimiento);
            this.ctrCamposValores.IniciarControl(this.MiMovimiento, new Objeto(), this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiMovimiento, this.MiMovimiento.ConceptoContable, this.GestionControl);
        }
        private void MapearControlesAObjeto()
        {
            //this.MiMovimiento.ConceptoContable.IdConceptoContable = Convert.ToInt32(this.ddlConceptosContables.SelectedValue);
            //this.MiMovimiento.ConceptoContable.ConceptoContable = this.ddlConceptosContables.SelectedItem.Text;
            this.MiMovimiento.Importe = this.txtImporte.Decimal; //Convert.ToDecimal(this.txtImporte.Text);
            this.MiMovimiento.NumeroTipoOperacion = this.txtNumeroTipoOperacion.Text;
            this.MiMovimiento.Detalle = this.txtDetalle.Text;
            this.MiMovimiento.TipoOperacion = this.MisTiposOperaciones[this.ddlTipoOperacion.SelectedIndex];
            this.MiMovimiento.ConceptoContable = this.MisConceptosContables[this.ddlConceptosContables.SelectedIndex];
            //Se usa para transferencias de movimientos a Tesorerias
            this.MiMovimiento.IdRefTipoOperacion = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            this.MiMovimiento.FechaConfirmacionBanco = Convert.ToDateTime(this.txtFechaConciliacionBanco.Text);
            this.MiMovimiento.BancoCuentaDestino.IdBancoCuenta = this.ddlBancosCuentas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancosCuentas.SelectedValue);
            this.MiMovimiento.Comentarios = this.ctrComentarios.ObtenerLista();
            this.MiMovimiento.Archivos = this.ctrArchivos.ObtenerLista();

            this.MiMovimiento.Campos = this.ctrCamposValores.ObtenerLista();
            this.MiMovimiento.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.Page.Validate("BancosCuentasMovimientosDatos");
            //if (!this.Page.IsValid)
            //    return;
            bool resultado = false;

            this.MapearControlesAObjeto();
            this.MiMovimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    TGEMonedasCotizaciones monedas = new TGEMonedasCotizaciones();
                    monedas.IdMoneda = MiMovimiento.BancoCuenta.Moneda.IdMoneda;
                    monedas = TGEGeneralesF.MonedasCotizacionesObtenerCotizacion(monedas);
                    this.MiMovimiento.BancoCuenta.Moneda.MonedeaCotizacion.MonedaCotizacion = monedas.MonedaCotizacion;
                    this.MiMovimiento.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    resultado = BancosF.BancosCuentasMovimientosAgregar(this.MiMovimiento);
                    break;
                case Gestion.Anular:
                    resultado = BancosF.BancosCuentasMovimientosAnular(this.MiMovimiento);
                    break;
                default:
                    break;
            }

            if (resultado)
            {
                //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiMovimiento.CodigoMensaje));
                this.MostrarMensaje(this.MiMovimiento.CodigoMensaje, false);
                this.btnAceptar.Visible = false;
                this.btnImprimir.Visible = true;
                this.ctrAsientoMostrar.IniciarControl(this.MiMovimiento);
            }
            else
            {
                this.MostrarMensaje(this.MiMovimiento.CodigoMensaje, true, this.MiMovimiento.CodigoMensajeArgs);
                if (this.MiMovimiento.dsResultado != null)
                {
                    this.ctrPopUpGrilla.IniciarControl(this.MiMovimiento);
                    this.MiMovimiento.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.BancosCuentasMovimientosDatosCancelar != null)
                this.BancosCuentasMovimientosDatosCancelar();
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {

            RepReportes reporte = new RepReportes();
            string parametro = "";
            if (MiMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaCuentasInternasCredito
                || MiMovimiento.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.TransferenciaTesoreriaCredito)
                parametro = MiMovimiento.IdRefTipoOperacion.ToString();
            else
                parametro = MiMovimiento.IdBancoCuentaMovimiento.ToString();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
            param.Parametro = "IdBancoCuentaMovimiento";
            param.ValorParametro = parametro;
            reporte.Parametros.Add(param);
            //param = new RepParametros();
            //param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            //param.Parametro = "FechaDesde";
            //param.ValorParametro = Convert.ToDateTime(this.txtFechaDesde.Text);
            //reporte.Parametros.Add(param);
            //param = new RepParametros();
            //param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
            //param.Parametro = "FechaHasta";
            //param.ValorParametro = Convert.ToDateTime(this.txtFechaHasta.Text);
            //reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdEstado";
            param.ValorParametro = MiMovimiento.Estado.IdEstado;
            reporte.Parametros.Add(param);

            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "TESBancosCuentasMovimientos";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            reporte.StoredProcedure = plantilla.NombreSP;


            DataSet ds = ReportesF.ReportesObtenerDatos(reporte);


            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdBancoCuentaMovimiento", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.Page, "BancoCuentaMovimiento_" + MiMovimiento.IdBancoCuentaMovimiento.ToString() + "_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);
            this.UpdatePanel2.Update();
        }

        //protected void popUpMensajes_popUpMensajesPostBackAceptar()
        //{
        //    if (this.BancosCuentasMovimientosDatosAceptar != null)
        //        this.BancosCuentasMovimientosDatosAceptar(null, this.MiMovimiento);
        //}
    }
}