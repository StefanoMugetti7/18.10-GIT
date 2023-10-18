using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Ahorros.Entidades;
using Comunes.Entidades;
using Ahorros;
using Generales.FachadaNegocio;
using Afiliados.Entidades;
using Generales.Entidades;
using System.Collections.Generic;
using System.Globalization;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Ahorros.Controles
{
    public partial class PlazosFijosDatos : ControlesSeguros
    {
        private AhoPlazosFijos MiAhoPlazosFijos
        {
            get { return (AhoPlazosFijos)Session[this.MiSessionPagina + "AhorroMiAhoPlazosFijos"]; }
            set { Session[this.MiSessionPagina + "AhorroMiAhoPlazosFijos"] = value; }
        }

        private List<AhoPlazos> MisAhoPlazos
        {
            get { return (List<AhoPlazos>)Session[this.MiSessionPagina + "PlazosFijosDatosMisAhoPlazos"]; }
            set { Session[this.MiSessionPagina + "PlazosFijosDatosMisAhoPlazos"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "PlazosFijosDatosMisMonedas"]; }
            set { Session[this.MiSessionPagina + "PlazosFijosDatosMisMonedas"] = value; }
        }

        public delegate void PlazosFijosDatosAceptarEventHandler(object sender, AhoPlazosFijos e);
        public event PlazosFijosDatosAceptarEventHandler PlazosFijosDatosAceptar;

        public delegate void PlazosFijosDatosCancelarEventHandler();
        public event PlazosFijosDatosCancelarEventHandler PlazosFijosDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrAfiliados.AfiliadosBuscarSeleccionar += new IU.Modulos.Afiliados.Controles.AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
          
            if (!this.IsPostBack)
            {
                //this.txtImporte.Attributes.Add("onchange", " CalcularImporte(); CalcularTotal();");
                //this.txtTasaInteres.Attributes.Add("onchange", " CalcularTotal(); ");
            
                if (this.MiAhoPlazosFijos == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(AhoPlazosFijos pPlazosFijos, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiAhoPlazosFijos = pPlazosFijos;
            this.DeshabilitarControles();
            this.CargarCombos();
            this.ddlEstado.Enabled = false;
            TGEParametrosValores diasPorAnio = new TGEParametrosValores();

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlEstado.SelectedValue = ((int)EstadosPlazosFijos.PendienteConfirmacion).ToString();
                    this.txtFechaInicioVigencia.Text = DateTime.Today.ToShortDateString();
                    //this.txtFechaInicioVigencia.Enabled = false;
                    //this.rfvFechaInicioVigencia.Enabled = false;
                    //this.dvFechaInicioVigencia.Visible = false;
                    //this.dvFechaInicioVigenciaEspacio.Visible = true;
                    this.txtFechaInicioVigencia.Enabled = true;
                    this.txtFechaInicioVigencia.Enabled = true;
                    ddlCuenta.Enabled = true;
                    ddlMoneda.Enabled = true;
                    //this.rfvFechaInicioVigencia.Enabled = true;
                    //this.dvFechaInicioVigencia.Visible = true;
                    //this.dvFechaInicioVigenciaEspacio.Visible = false;
                    this.btnAgregarCotitular.Visible = true;
                    this.ctrComentarios.IniciarControl(this.MiAhoPlazosFijos, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiAhoPlazosFijos, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
                    diasPorAnio = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DiasPorAnio);
                    hdfDiasPorAnio.Value = diasPorAnio.ParametroValor;
                    break;
                case Gestion.AnularCancelar:
                    diasPorAnio = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DiasPorAnio);
                    hdfDiasPorAnio.Value = diasPorAnio.ParametroValor;
                    this.MiAhoPlazosFijos = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazosFijos);
                    this.MapearObjetoAControles(this.MiAhoPlazosFijos);
                    ScriptManager.RegisterStartupScript(this.upSaldoActual, this.upSaldoActual.GetType(), "CalcularTotal", "CalcularTotal();", true);
                    if (!string.IsNullOrEmpty(ddlCuenta.SelectedValue))
                    {
                        this.dvCancelarCajaAhorros.Visible = true;
                        this.lblCancelarCajaAhorros.Visible = true;
                        this.chkCancelarCajaAhorros.Visible = true;
                    }
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
                    string mensajeBajaAnular = this.ObtenerMensajeSistema("ConfirmarCancelarPlazoFijoDesdeCajaAhorro");
                    string funcionBajaAnular = string.Format("ValidarShowConfirmBaja(this,'{0}'); return false;", mensajeBajaAnular);
                    this.btnAceptar.Attributes.Add("OnClick", funcionBajaAnular);
                    this.ddlCuenta.Enabled = false;
                    this.ddlPlazo.Enabled = false;
                    this.cbTasaEspecial.Enabled = false;
                    this.txtTasaInteres.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = false;
                    this.pnlPago.Visible = true;
                    this.ddlMoneda.Enabled = false;
                    this.txtDias.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    //this.ddlTipoValorPago.Enabled = false;
                    this.CargarCombosPagos();

                    break;
                case Gestion.Anular:
               
                case Gestion.Cancelar:
                    this.MiAhoPlazosFijos = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazosFijos);
                    this.MapearObjetoAControles(this.MiAhoPlazosFijos);
                    if (!string.IsNullOrEmpty(ddlCuenta.SelectedValue))
                    {
                        this.dvCancelarCajaAhorros.Visible = true;
                        this.lblCancelarCajaAhorros.Visible = true;
                        this.chkCancelarCajaAhorros.Visible = true;
                        this.chkCancelarCajaAhorros.Enabled = true;
                    }
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
                    string mensajeBaja = this.ObtenerMensajeSistema("ConfirmarCancelarPlazoFijoDesdeCajaAhorro");
                    string funcionBaja = string.Format("ValidarShowConfirmBaja(this,'{0}'); return false;", mensajeBaja);
                    this.btnAceptar.Attributes.Add("OnClick", funcionBaja);
                    this.ddlCuenta.Enabled = false;
                    this.ddlPlazo.Enabled = false;
                    this.cbTasaEspecial.Enabled = false;
                    this.txtTasaInteres.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = false;
                    this.pnlPago.Visible = true;
                    this.ddlMoneda.Enabled = false;
                    this.txtDias.Enabled = false;
                    this.ddlFilialPago.Enabled = false;
                    //this.ddlTipoValorPago.Enabled = false;
                    this.CargarCombosPagos();
                    break;
                case Gestion.RenovacionAnticipada:
                 
                    this.MiAhoPlazosFijos = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazosFijos);
                    hdfImporteRenovacion.Value = MiAhoPlazosFijos.ImporteTotal.ToString();
                    int dias = MiAhoPlazosFijos.PlazoDias;
                    MiAhoPlazosFijos = AhorroF.PlazosFijosRecalcularRenovacionAnticipada(MiAhoPlazosFijos);
                    MiAhoPlazosFijos.PlazoDias = dias;
                    MiAhoPlazosFijos.ImporteCapital += MiAhoPlazosFijos.ImporteInteres;
                    //MiAhoPlazosFijos.TasaInteres = 
                    if (MiAhoPlazosFijos.Cuenta.IdCuenta > 0)
                    {
                        ddlCuenta.SelectedValue = MiAhoPlazosFijos.Cuenta.IdCuenta.ToString();
                        ddlCuenta_SelectedIndexChanged(ddlCuenta, EventArgs.Empty);
                    }
                    else
                    {
                        ddlMoneda.SelectedValue = MiAhoPlazosFijos.Moneda.IdMoneda.ToString();
                        ddlMoneda_SelectedIndexChanged(ddlMoneda, EventArgs.Empty);
                    }
                   
                    if (!this.MiAhoPlazosFijos.TasaEspecial)
                    {
                        AhoPlazos plazo = this.MisAhoPlazos.Find(x => x.IdPlazos == this.MiAhoPlazosFijos.Plazo.IdPlazos);
                        if (plazo != null && plazo.TasaInteres != this.MiAhoPlazosFijos.TasaInteres)
                            this.MiAhoPlazosFijos.TasaInteres = plazo.TasaInteres;
                    }
                    if (!string.IsNullOrEmpty(ddlCuenta.SelectedValue))
                        chkRegistrarCaja.Enabled = true;
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
                    this.MapearObjetoAControles(this.MiAhoPlazosFijos);
                    this.ddlMoneda.Enabled = false;
                    this.ddlCuenta.Enabled = false;
                    this.ddlPlazo.Enabled = true;
                    this.cbTasaEspecial.Enabled = true;
                    //this.txtTasaInteres.Enabled = true;

                    this.txtImporte.Enabled = true;
                    this.ddlTipoRenovacion.Enabled = true;
         
                    this.txtFechaInicioVigencia.Enabled = false;
               
                    //this.rfvFechaInicioVigencia.Enabled = true;
                    //this.dvFechaInicioVigencia.Visible = true;
                    //this.dvFechaInicioVigenciaEspacio.Visible = false;
                    this.btnAgregarCotitular.Visible = true;
                    foreach (AhoCotitulares coti in this.MiAhoPlazosFijos.Cotitulares)
                        coti.EstadoColeccion = EstadoColecciones.Agregado;
                    string mensajeBajaRenovacion = this.ObtenerMensajeSistema("ConfirmarRenovacionAnticipadaPlazoFijo");
                    string funcionBajaRenovacion = string.Format("ValidarShowConfirmBaja(this,'{0}'); return false;", mensajeBajaRenovacion);
                    this.btnAceptar.Attributes.Add("OnClick", funcionBajaRenovacion);
                    break;
                case Gestion.Consultar:
                    this.MiAhoPlazosFijos = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazosFijos);
                    this.MapearObjetoAControles(this.MiAhoPlazosFijos);
                    this.ddlCuenta.Enabled = false;
                    this.ddlPlazo.Enabled = false;
                    this.cbTasaEspecial.Enabled = false;
                    this.txtTasaInteres.Enabled = false;
                    this.txtDias.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.ddlMoneda.Enabled = false;
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
                    if (!string.IsNullOrEmpty(ddlCuenta.SelectedValue) && MiAhoPlazosFijos.Estado.IdEstado == (int)EstadosPlazosFijos.Pagado 
                        || MiAhoPlazosFijos.Estado.IdEstado == (int)EstadosPlazosFijos.Cancelado)
                    {
                        this.pnlPago.Visible = true;
                        this.dvCancelarCajaAhorros.Visible = true;
                        this.lblCancelarCajaAhorros.Visible = true;
                        this.chkCancelarCajaAhorros.Visible = true;
                        if (MiAhoPlazosFijos.Estado.IdEstado == (int)EstadosPlazosFijos.Pagado)
                        {
                            lblCancelarCajaAhorros.Text = "Pagar en Cuenta de Ahorro";
                        }
                    }
                    btnImprimir.Visible = true;
                    break;
                case Gestion.Modificar:
                    this.MiAhoPlazosFijos = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazosFijos);
                    this.MapearObjetoAControles(this.MiAhoPlazosFijos);
                    this.ddlCuenta.Enabled = false;
                    this.ddlPlazo.Enabled = false;
                    this.cbTasaEspecial.Enabled = false;
                    this.txtTasaInteres.Enabled = false;
                    this.txtDias.Enabled = false;
                    ddlMoneda.Enabled = false;
                    if (!string.IsNullOrEmpty(ddlCuenta.SelectedValue))
                        chkRegistrarCaja.Enabled = true;
                    this.txtImporte.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = true;
                    this.btnAceptar.Visible = true;
                    this.btnAgregarCotitular.Visible = true;
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Renovar:
                    diasPorAnio = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.DiasPorAnio);
                    hdfDiasPorAnio.Value = diasPorAnio.ParametroValor;
                    this.MiAhoPlazosFijos = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazosFijos);
                    hdfImporteRenovacion.Value = MiAhoPlazosFijos.ImporteTotal.ToString();
                    if (MiAhoPlazosFijos.Cuenta.IdCuenta > 0)
                    {
                        ddlCuenta.SelectedValue = MiAhoPlazosFijos.Cuenta.IdCuenta.ToString();
                        ddlCuenta_SelectedIndexChanged(ddlCuenta, EventArgs.Empty);
                    }
                    else
                    {
                        ddlMoneda.SelectedValue = MiAhoPlazosFijos.Moneda.IdMoneda.ToString();
                        ddlMoneda_SelectedIndexChanged(ddlMoneda, EventArgs.Empty);
                    }
                    this.MiAhoPlazosFijos.ImporteCapital = this.MiAhoPlazosFijos.ImporteTotal;
                    if (!this.MiAhoPlazosFijos.TasaEspecial)
                    {
                        AhoPlazos plazo = this.MisAhoPlazos.Find(x => x.IdPlazos == this.MiAhoPlazosFijos.Plazo.IdPlazos);
                        if (plazo!=null && plazo.TasaInteres != this.MiAhoPlazosFijos.TasaInteres)
                            this.MiAhoPlazosFijos.TasaInteres = plazo.TasaInteres;                            
                    }
                    if (!string.IsNullOrEmpty(ddlCuenta.SelectedValue))
                        chkRegistrarCaja.Enabled = true;
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
                    this.MapearObjetoAControles(this.MiAhoPlazosFijos);
                    this.ddlCuenta.Enabled = false;
                    this.ddlPlazo.Enabled = true;
                    this.cbTasaEspecial.Enabled = true;
                    //this.txtTasaInteres.Enabled = true;
                    ddlMoneda.Enabled = false;
                    this.txtImporte.Enabled = true;
                    this.ddlTipoRenovacion.Enabled = true;
                    this.txtFechaInicioVigencia.Enabled = true;
                    this.txtFechaInicioVigencia.Enabled = true;
                    //this.rfvFechaInicioVigencia.Enabled = true;
                    //this.dvFechaInicioVigencia.Visible = true;
                    //this.dvFechaInicioVigenciaEspacio.Visible = false;
                    this.btnAgregarCotitular.Visible = true;
                    foreach (AhoCotitulares coti in this.MiAhoPlazosFijos.Cotitulares)
                        coti.EstadoColeccion = EstadoColecciones.Agregado;
                    break;
                case Gestion.Pagar:
                    this.MiAhoPlazosFijos = AhorroF.PlazosFijosObtenerDatosCompletos(pPlazosFijos);
                    this.CargarCombosPagos();
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
                    this.MapearObjetoAControles(this.MiAhoPlazosFijos);
                    if (!string.IsNullOrEmpty(ddlCuenta.SelectedValue))
                    {
                        this.dvCancelarCajaAhorros.Visible = true;
                        this.lblCancelarCajaAhorros.Visible = true;
                        this.chkCancelarCajaAhorros.Visible = true;
                        this.chkCancelarCajaAhorros.Enabled = true;
                        lblCancelarCajaAhorros.Text = "Pagar en Cuenta de Ahorro";
                    }
                    this.ddlCuenta.Enabled = false;
                    ddlMoneda.Enabled = false;
                    this.ddlPlazo.Enabled = false;
                    this.cbTasaEspecial.Enabled = false;
                    this.txtTasaInteres.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.ddlTipoRenovacion.Enabled = false;
                    this.pnlPago.Visible = true;
                    break;
                default:
                    break;
            }
            /* Se dividen los check y la funcion para que quede ok y mas ordenado. */
            /* Para Agregar, Renovar y Renovacion Anticipada: utiliza el check RegistrarCajaAhorro */
            if (GestionControl == Gestion.Agregar
                    || GestionControl == Gestion.Renovar
                    || GestionControl == Gestion.RenovacionAnticipada)
            {
                string mensaje = this.ObtenerMensajeSistema("ConfirmarPlazoFijoDesdeCajaAhorro");
                string funcion = string.Format("ValidarShowConfirmRegistraCajaAhorro(this,'{0}'); return false;", mensaje);
                this.btnAceptar.Attributes.Add("OnClick", funcion);                
            }
            /* Para Pagar y Cancelacion Anticipada: utiliza el check CancelaCajaAhorro */
            if (GestionControl == Gestion.Pagar 
                || GestionControl == Gestion.Cancelar)
            {
                string mensaje = this.ObtenerMensajeSistema("ConfirmarPlazoFijoDesdeCajaAhorro");
                string funcion = string.Format("ValidarShowConfirmCancelaCajaAhorro(this,'{0}'); return false;", mensaje);
                this.btnAceptar.Attributes.Add("OnClick", funcion);
            }
        }

        private void CargarCombos()
        {
            //AfiAfiliados afiliado = new AfiAfiliados();
            //afiliado.IdAfiliado = this.MiAhoPlazosFijos.IdAfiliado;
            this.MiAhoPlazosFijos.Cuenta.Afiliado.IdAfiliado = this.MiAhoPlazosFijos.IdAfiliado;
            this.MiAhoPlazosFijos.Cuenta.CuentaTipo.IdCuentaTipo = (int)EnumAhorrosCuentasTipos.CajaAhorro;
            this.ddlCuenta.DataSource = AhorroF.CuentasObtenerListaFiltro(this.MiAhoPlazosFijos.Cuenta);
            this.ddlCuenta.DataValueField = "IdCuenta";
            this.ddlCuenta.DataTextField = "CuentaDatos";
            this.ddlCuenta.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCuenta, this.ObtenerMensajeSistema("PlazosFijosSinCuenta"));

            AhoTiposRenovaciones filtro = new AhoTiposRenovaciones();
            //filtro.IdTipoRenovacion = (int)EnumAhorrosTiposRenovaciones.SinRenovacion;
            this.ddlTipoRenovacion.DataSource = AhorroF.TiposRenovacionesObtenerLista(filtro);
            this.ddlTipoRenovacion.DataValueField = "IdTipoRenovacion";
            this.ddlTipoRenovacion.DataTextField = "TipoRenovacion";
            this.ddlTipoRenovacion.DataBind();

            //AhoPlazos plazo = new AhoPlazos();
            //plazo.Estado.IdEstado = (int)Estados.Activo;
            //this.MisAhoPlazos = AhorroF.PlazosObtenerListaFiltro(plazo);
            //this.ddlPlazo.DataSource = this.MisAhoPlazos;
            //this.ddlPlazo.DataValueField = "IdPlazos";
            //this.ddlPlazo.DataTextField = "PlazoDiasInteres";
            //this.ddlPlazo.DataBind();
            //if (this.ddlPlazo.Items.Count == 1)
            //    this.ddlPlazo_SelectedIndexChanged(null, EventArgs.Empty);
            //else
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPlazo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPlazosFijos));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerVentaCombo();
            this.ddlMoneda.DataSource = MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            else
                ddlMoneda_SelectedIndexChanged(ddlMoneda, EventArgs.Empty);

        }

        private void CargarCombosPagos()
        {
            this.ddlFilialPago.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilialPago.DataValueField = "IdFilial";
            this.ddlFilialPago.DataTextField = "Filial";
            this.ddlFilialPago.DataBind();
            this.ddlFilialPago.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            this.ddlTipoValorPago.DataSource = TGEGeneralesF.TiposValoresObtenerLista();
            this.ddlTipoValorPago.DataValueField = "IdTipoValor"; // "IdTipoValor";
            this.ddlTipoValorPago.DataTextField = "TipoValor";
            this.ddlTipoValorPago.DataBind();
            //ListItem item = this.ddlTipoValorPago.Items.FindByValue(this.MiAhoPlazosFijos.TipoValor.IdTipoValor.ToString());
            //if (item!=null)
            //    this.ddlTipoValorPago.SelectedValue = this.MiAhoPlazosFijos.TipoValor.IdTipoValor.ToString();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoValorPago, "SeleccioneOpcion");
        }

        private void DeshabilitarControles()
        {
            this.txtSaldoActual.Enabled = false;
            this.txtTasaInteres.Enabled = false;
            this.txtFechaInicioVigencia.Enabled = false;
            //this.rfvFechaInicioVigencia.Enabled = false;
            //this.dvFechaInicioVigencia.Visible = false;
            //this.dvFechaInicioVigenciaEspacio.Visible = true;
        }

        private void MapearObjetoAControles(AhoPlazosFijos pPlazosFijos)
        {
            SetInitializeCulture(pPlazosFijos.Moneda.Moneda);
            this.ddlCuenta.SelectedValue = pPlazosFijos.Cuenta.IdCuenta.ToString();
            ListItem item = this.ddlPlazo.Items.FindByValue(pPlazosFijos.Plazo.IdPlazos.ToString());
            if (item == null && GestionControl!=Gestion.Renovar)
                this.ddlPlazo.Items.Add(new ListItem(pPlazosFijos.Plazo.Descripcion, pPlazosFijos.Plazo.IdPlazos.ToString()));

            this.ddlPlazo.SelectedValue = pPlazosFijos.Plazo.IdPlazos.ToString();
            this.txtDias.Text = pPlazosFijos.PlazoDias.ToString();
            this.cbTasaEspecial.Checked = pPlazosFijos.TasaEspecial;
            this.txtTasaInteres.Text = pPlazosFijos.TasaInteres.ToString();
            this.chkRegistrarCaja.Checked = pPlazosFijos.RegistrarCajaAhorros;
            this.chkCancelarCajaAhorros.Checked = pPlazosFijos.CancelaCajaAhorros;
            //this.txtImporte.Decimal = pPlazosFijos.ImporteCapital;
            this.txtImporte.Text = pPlazosFijos.ImporteCapital.ToString("C2");
            if (GestionControl == Gestion.Cancelar)
            {
                this.txtImporteTotal.Text = pPlazosFijos.ImporteCapital.ToString("C2"); ;
                this.txtImporteInteres.Text = 0.ToString("C2");
            }
            else
            {
                this.txtImporteTotal.Text = pPlazosFijos.ImporteTotal.ToString("C2");
                this.txtImporteInteres.Text = pPlazosFijos.ImporteInteres.ToString("C2");
            }
            this.ddlTipoRenovacion.SelectedValue = pPlazosFijos.TipoRenovacion.IdTipoRenovacion.ToString();
            this.ddlEstado.SelectedValue  = pPlazosFijos.Estado.IdEstado.ToString();
            this.txtFechaInicioVigencia.Text = pPlazosFijos.FechaInicioVigencia.ToShortDateString();
            this.txtFechaVencimiento.Text = pPlazosFijos.FechaVencimiento.ToShortDateString();
            this.txtSaldoActual.Text = pPlazosFijos.Cuenta.SaldoActual.ToString("C2");
            this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, new Objeto(), this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, this.MiAhoPlazosFijos.TipoOperacion, this.GestionControl);
            TGEFiliales filial = new TGEFiliales();
            filial.IdFilial = MiAhoPlazosFijos.IdFilial;
            this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, filial, this.GestionControl);

            AyudaProgramacion.CargarGrillaListas<AhoCotitulares>(this.MiAhoPlazosFijos.Cotitulares, true, this.gvDatos, true);

            if (GestionControl == Gestion.Renovar)
            {
                this.ddlEstado.SelectedValue = ((int)EstadosPlazosFijos.PendienteConfirmacion).ToString();
                this.txtFechaInicioVigencia.Text = DateTime.Today.ToShortDateString();

                if (item == null)
                {
                    this.txtDias.Text = string.Empty;
                    this.txtImporteInteres.Text = string.Empty;
                    this.txtFechaVencimiento.Text = string.Empty;
                    this.txtTasaInteres.Text = (0).ToString();
                    this.txtImporteTotal.Text = (0).ToString("C2");
                }
                else
                {
                    DateTime fechaVto = Convert.ToDateTime(this.txtFechaInicioVigencia.Text);
                    fechaVto = fechaVto.AddDays(Convert.ToInt32(this.txtDias.Text));
                    this.txtFechaVencimiento.Text = fechaVto.ToShortDateString();
                    this.CalcularTotal();
                }
            }

            item = this.ddlTipoValorPago.Items.FindByValue(pPlazosFijos.TipoValorPago.IdTipoValorPago.ToString());
            if (item == null && pPlazosFijos.TipoValorPago.IdTipoValorPago > 0)
            {
                this.ddlTipoValorPago.Items.Add(new ListItem(pPlazosFijos.TipoValorPago.TipoValor, pPlazosFijos.TipoValorPago.IdTipoValorPago.ToString()));
                this.ddlTipoValorPago.SelectedValue = pPlazosFijos.TipoValorPago.IdTipoValorPago.ToString();
            }

            item = this.ddlFilialPago.Items.FindByValue(pPlazosFijos.FilialPago.IdFilialPago.ToString());
            if (item == null && pPlazosFijos.FilialPago.IdFilialPago > 0)
            {
                this.ddlFilialPago.Items.Add(new ListItem(pPlazosFijos.FilialPago.Filial, pPlazosFijos.FilialPago.IdFilialPago.ToString()));
                this.ddlFilialPago.SelectedValue = pPlazosFijos.FilialPago.IdFilialPago.ToString();
            }

            if (MisMonedas.Exists(x => x.IdMoneda == pPlazosFijos.Moneda.IdMoneda))
                MisMonedas.First(x => x.IdMoneda == pPlazosFijos.Moneda.IdMoneda).MonedeaCotizacion.MonedaCotizacion = pPlazosFijos.MonedaCotizacion;
            else if (pPlazosFijos.Moneda.IdMoneda > 0)
                MisMonedas.Add(pPlazosFijos.Moneda);
            this.ddlMoneda.Items.Clear();
            this.ddlMoneda.SelectedIndex = -1;
            this.ddlMoneda.SelectedValue = null;
            this.ddlMoneda.ClearSelection();
            this.ddlMoneda.DataSource = MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlMoneda.SelectedValue = pPlazosFijos.Moneda.IdMoneda > 0 ? pPlazosFijos.Moneda.IdMoneda.ToString() : string.Empty;

            this.ctrComentarios.IniciarControl(pPlazosFijos, this.GestionControl);
            this.ctrArchivos.IniciarControl(pPlazosFijos, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pPlazosFijos);
        }

        private void MapearControlesAObjeto(AhoPlazosFijos pPlazosFijos)
        {
            pPlazosFijos.Cuenta.IdCuenta = this.ddlCuenta.SelectedValue== "" ? 0 : Convert.ToInt32(this.ddlCuenta.SelectedValue);
            pPlazosFijos.PlazoDias = Convert.ToInt32(this.txtDias.Text);//this.MisAhoPlazos[this.ddlPlazo.SelectedIndex].PlazoDias : Convert.ToInt32(this.ddlPlazo.SelectedItem.Text);
            pPlazosFijos.TasaEspecial = this.cbTasaEspecial.Checked;
            pPlazosFijos.RegistrarCajaAhorros = this.chkRegistrarCaja.Checked;
            pPlazosFijos.CancelaCajaAhorros = this.chkCancelarCajaAhorros.Checked;
            pPlazosFijos.TasaInteres = Convert.ToDecimal(this.txtTasaInteres.Text);
            pPlazosFijos.ImporteCapital = this.txtImporte.Decimal;
            pPlazosFijos.TipoRenovacion.IdTipoRenovacion = Convert.ToInt32(this.ddlTipoRenovacion.SelectedValue);
            pPlazosFijos.TipoRenovacion.TipoRenovacion = this.ddlTipoRenovacion.SelectedItem.Text;
            pPlazosFijos.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pPlazosFijos.FechaInicioVigencia = Convert.ToDateTime(this.txtFechaInicioVigencia.Text);
            pPlazosFijos.Plazo.IdPlazos = this.ddlPlazo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPlazo.SelectedValue);
            pPlazosFijos.TipoValorPago.IdTipoValorPago = string.IsNullOrEmpty(this.ddlTipoValorPago.SelectedValue) ? 0 : Convert.ToInt32(this.ddlTipoValorPago.SelectedValue);
            pPlazosFijos.FilialPago.IdFilialPago = string.IsNullOrEmpty(this.ddlFilialPago.SelectedValue) ? 0 : Convert.ToInt32(this.ddlFilialPago.SelectedValue);
            pPlazosFijos.Comentarios = this.ctrComentarios.ObtenerLista();
            pPlazosFijos.Archivos = this.ctrArchivos.ObtenerLista();


            pPlazosFijos.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            TGEMonedas moneda = MisMonedas.FirstOrDefault(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
            if (moneda.IdMoneda > 0)
            {
                pPlazosFijos.Moneda = moneda;
                pPlazosFijos.MonedaCotizacion = moneda.MonedeaCotizacion.MonedaCotizacion;
            }

            pPlazosFijos.Campos = this.ctrCamposValores.ObtenerLista();
            pPlazosFijos.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            Page.Validate("Aceptar");
            if (!this.Page.IsValid)
            {
                upTasaInteres.Update();
                return;
            }
       

            this.MiAhoPlazosFijos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MapearControlesAObjeto(this.MiAhoPlazosFijos);
                    //Validacion de dias
                    this.MiAhoPlazosFijos.Plazo = this.MisAhoPlazos[this.ddlPlazo.SelectedIndex];
                    //if (this.MiAhoPlazosFijos.ImporteCapital <= this.MiAhoPlazosFijos.Cuenta.SaldoActual)
                    //{
                    this.MiAhoPlazosFijos.FechaAlta = DateTime.Now;
                        this.MiAhoPlazosFijos.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    this.MiAhoPlazosFijos.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                        guardo = AhorroF.PlazosFijosAgregar(this.MiAhoPlazosFijos);
                    //}
                    //else
                    //{
                    //    this.MiAhoPlazosFijos.CodigoMensaje = "AhoPlazosFijos";
                    //    this.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAhoPlazosFijos.CodigoMensaje), true);
                    //    return;
                    //}
                    break;
                case Gestion.Modificar:
                    this.MapearControlesAObjeto(this.MiAhoPlazosFijos);
                    this.MiAhoPlazosFijos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = AhorroF.PlazosFijosModificar(this.MiAhoPlazosFijos);
                    break;
                case Gestion.Cancelar:
                    this.MapearControlesAObjeto(this.MiAhoPlazosFijos);
                    //this.MiAhoPlazosFijos.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    this.MiAhoPlazosFijos.UsuarioCancelacion.IdUsuarioCancelacion = this.UsuarioActivo.IdUsuario;
                    this.MiAhoPlazosFijos.FechaCancelacion = DateTime.Now;
                    guardo = AhorroF.PlazosFijosCancelar(this.MiAhoPlazosFijos);
                    break;
                case Gestion.Anular:
                    this.MapearControlesAObjeto(this.MiAhoPlazosFijos);
                    //this.MiAhoPlazosFijos.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    //this.MiAhoPlazosFijos.FechaCancelacion = DateTime.Now;
                    //this.MiAhoPlazosFijos.UsuarioCancelacion.IdUsuarioCancelacion = this.UsuarioActivo.IdUsuario;
                    this.MiAhoPlazosFijos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = AhorroF.PlazosFijosBorrar(this.MiAhoPlazosFijos);
                    break;
                case Gestion.AnularCancelar:
                    this.MapearControlesAObjeto(this.MiAhoPlazosFijos);
                    //this.MiAhoPlazosFijos.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    //this.MiAhoPlazosFijos.FechaCancelacion = DateTime.Now;
                    //this.MiAhoPlazosFijos.UsuarioCancelacion.IdUsuarioCancelacion = this.UsuarioActivo.IdUsuario;
                    this.MiAhoPlazosFijos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = AhorroF.PlazosFijosAnularCancelacion(this.MiAhoPlazosFijos);
                    break;
                case Gestion.RenovacionAnticipada:
                    this.MapearControlesAObjeto(this.MiAhoPlazosFijos);
                    this.MiAhoPlazosFijos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    //this.MiAhoPlazosFijos.UsuarioCancelacion.IdUsuarioCancelacion = this.UsuarioActivo.IdUsuario;
                    this.MiAhoPlazosFijos.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    guardo = AhorroF.PlazosFijosRenovacionAnticipada(this.MiAhoPlazosFijos);
                    break;
                case Gestion.Renovar:
                    this.MapearControlesAObjeto(this.MiAhoPlazosFijos);
                    this.MiAhoPlazosFijos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    //this.MiAhoPlazosFijos.UsuarioCancelacion.IdUsuarioCancelacion = this.UsuarioActivo.IdUsuario;
                    this.MiAhoPlazosFijos.IdFilial = this.UsuarioActivo.FilialPredeterminada.IdFilial;
                    guardo = AhorroF.PlazosFijosRenovar(this.MiAhoPlazosFijos);
                    break;
                case Gestion.Pagar:
                    this.MapearControlesAObjeto(this.MiAhoPlazosFijos);
                    this.MiAhoPlazosFijos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    //this.MiAhoPlazosFijos.UsuarioCancelacion.IdUsuarioCancelacion = this.UsuarioActivo.IdUsuario;
                    guardo = AhorroF.PlazosFijosPagar(this.MiAhoPlazosFijos);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.btnAgregarCotitular.Visible = false;
                this.GestionControl = Gestion.Consultar;
                this.btnImprimir.Visible = true;
                this.MostrarMensaje(this.MiAhoPlazosFijos.CodigoMensaje, false);
            }
            else
            {
                //if (this.MiAhoPlazosFijos.ConfirmarAccion)
                //    this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAhoPlazosFijos.CodigoMensaje, this.MiAhoPlazosFijos.CodigoMensajeArgs), true);
                //else
                    this.MostrarMensaje(this.MiAhoPlazosFijos.CodigoMensaje, true, this.MiAhoPlazosFijos.CodigoMensajeArgs);
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.AhoPlazosFijos, "AhorroPlazoFijo", MiAhoPlazosFijos, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, UpdatePanel2, "AhorroPlazoFijo", this.UsuarioActivo);
            }
            catch (Exception ex)
            {
                this.MostrarMensaje(ex.Message, true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.PlazosFijosDatosCancelar != null)
                this.PlazosFijosDatosCancelar();
        }

        //protected void popUpMensajes_popUpMensajesPostBackAceptar()
        //{
        //    if (this.MiAhoPlazosFijos.ConfirmarAccion)
        //    {
        //        this.MiAhoPlazosFijos.ConfirmarRenovar = true;
        //        this.btnAceptar_Click(null, EventArgs.Empty);
        //    }
        //    else
        //    {
        //        if (this.PlazosFijosDatosAceptar != null)
        //            this.PlazosFijosDatosAceptar(null, this.MiAhoPlazosFijos);
        //    }
        //}

        //protected void popUpMensajes_popUpMensajesPostBackAceptar()
        //{
        //    if (this.PlazosFijosDatosAceptar != null && this.MiAhoPlazosFijos.CodigoMensaje != "AhoPlazosFijos")
        //        this.PlazosFijosDatosAceptar(null, this.MiAhoPlazosFijos);
        //}

        protected void ddlCuenta_SelectedIndexChanged(object sender, EventArgs e)
        {
            AhoTiposRenovaciones filtro = new AhoTiposRenovaciones();
            if (!string.IsNullOrEmpty(this.ddlCuenta.SelectedValue))
            {
                this.MiAhoPlazosFijos.Cuenta.IdCuenta = Convert.ToInt32(this.ddlCuenta.SelectedValue);
                this.MiAhoPlazosFijos.Cuenta = AhorroF.CuentasObtenerDatosCompletos(this.MiAhoPlazosFijos.Cuenta);
                this.txtSaldoActual.Decimal = this.MiAhoPlazosFijos.Cuenta.SaldoActual;
                ListItem item = ddlMoneda.Items.FindByValue(MiAhoPlazosFijos.Cuenta.Moneda.IdMoneda.ToString());
                if (item == null)
                    ddlMoneda.Items.Add(new ListItem(MiAhoPlazosFijos.Cuenta.Moneda.Moneda, MiAhoPlazosFijos.Cuenta.Moneda.IdMoneda.ToString()));
                this.ddlMoneda.SelectedValue = MiAhoPlazosFijos.Cuenta.Moneda.IdMoneda.ToString();
                this.ddlMoneda.Enabled = false;
                chkRegistrarCaja.Enabled = true;
                this.ctrCamposValores.IniciarControl(this.MiAhoPlazosFijos, this.UsuarioActivo.FilialPredeterminada, this.GestionControl);
                ddlMoneda_SelectedIndexChanged(ddlMoneda, EventArgs.Empty);
                this.upTasaInteres.Update();
                ScriptManager.RegisterStartupScript(this.upSaldoActual, this.upSaldoActual.GetType(), "CalcularImporteScript", "CalcularImporte();", true);
            }
            else
            {
                chkRegistrarCaja.Enabled = false;
                chkRegistrarCaja.Checked = false;
                this.txtSaldoActual.Decimal =0;
                this.ddlMoneda.SelectedValue = string.Empty;
                this.ddlMoneda.Enabled = true;
                this.upTasaInteres.Update();
                filtro.IdTipoRenovacion = (int)EnumAhorrosTiposRenovaciones.SinRenovacion;
            }

            this.ddlTipoRenovacion.DataSource = AhorroF.TiposRenovacionesObtenerLista(filtro);
            this.ddlTipoRenovacion.DataValueField = "IdTipoRenovacion";
            this.ddlTipoRenovacion.DataTextField = "TipoRenovacion";
            this.ddlTipoRenovacion.DataBind();
        }

        protected void ddlPlazo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlPlazo.SelectedValue))
                return;

            this.MiAhoPlazosFijos.Plazo.IdPlazos = Convert.ToInt32(this.ddlPlazo.SelectedValue);
            this.MiAhoPlazosFijos.Plazo = AhorroF.PlazosObtenerDatosCompletos(this.MiAhoPlazosFijos.Plazo);
            this.txtTasaInteres.Text = this.MiAhoPlazosFijos.Plazo.TasaInteres.ToString();
            int dias = this.txtDias.Text == string.Empty ? 0 : Convert.ToInt32(this.txtDias.Text);
            //if (plazos.PlazoDias > dias)
            this.txtDias.Text = this.MiAhoPlazosFijos.Plazo.PlazoDias.ToString();
            DateTime fechaVto = this.txtFechaInicioVigencia.Text ==string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaInicioVigencia.Text);
            fechaVto = fechaVto.AddDays(this.MiAhoPlazosFijos.Plazo.PlazoDias);
            this.txtFechaVencimiento.Text = fechaVto.ToShortDateString();
            this.CalcularTotal();
        }

        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
            {
                if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue)
                    )
                {
                    AhoPlazos ahoPlazos = new AhoPlazos();
                    ahoPlazos.Estado.IdEstado = (int)Estados.Activo;
                    ahoPlazos.Moneda = MisMonedas.First(x => x.IdMoneda == Convert.ToInt32(ddlMoneda.SelectedValue));
                    //ahoPlazos.Moneda.MonedeaCotizacion.IdMonedaCotizacion = Convert.ToInt32(ddlMoneda.SelectedValue);
                    ListItem itemPlazo = this.ddlPlazo.Items.FindByValue(this.ddlPlazo.SelectedValue);

                    this.ddlPlazo.Items.Clear();
                    this.ddlPlazo.SelectedIndex = -1;
                    this.ddlPlazo.SelectedValue = null;
                    this.ddlPlazo.ClearSelection();
                    
                    this.MisAhoPlazos = AhorroF.PlazosObtenerListaFiltro(ahoPlazos);
                    this.ddlPlazo.DataSource = this.MisAhoPlazos;
                    this.ddlPlazo.DataValueField = "IdPlazos";
                    this.ddlPlazo.DataTextField = "Descripcion";//"PlazoDiasInteres";
                    this.ddlPlazo.DataBind();
                    if (this.MisAhoPlazos.Count != 1)
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlPlazo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                    if (itemPlazo != null)
                    {
                        ListItem itemPlanExiste = this.ddlPlazo.Items.FindByValue(itemPlazo.Value);
                        if (itemPlanExiste != null)
                            this.ddlPlazo.SelectedValue = itemPlazo.Value;
                    }
                    //this.ctrCamposValores.IniciarControl(this.MiPrePrestamos, planFiltro.FormaCobro, this.GestionControl);
                    SetInitializeCulture(ahoPlazos.Moneda.Moneda);
                    ScriptManager.RegisterStartupScript(this.upTasaInteres, this.upTasaInteres.GetType(), "CalcularTotalScript", "CalcularTotal();", true);
                }
            }
            ddlPlazo_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void txtDias_TextChanged(object sender, EventArgs e)
        {
            DateTime fechaVto = Convert.ToDateTime(this.txtFechaInicioVigencia.Text);
            fechaVto = fechaVto.AddDays(Convert.ToInt32(this.txtDias.Text));
            this.txtFechaVencimiento.Text = fechaVto.ToShortDateString();
            this.CalcularTotal();
        }

        protected void txtFechaInicioVigencia_TextChanged(object sender, EventArgs e)
        {
            DateTime fechaVto = Convert.ToDateTime(this.txtFechaInicioVigencia.Text);
            if (this.txtDias.Text == string.Empty)
                return;
            fechaVto = fechaVto.AddDays(Convert.ToInt32(this.txtDias.Text));
            this.txtFechaVencimiento.Text = fechaVto.ToShortDateString();
            this.CalcularTotal();
        }

        private void CalcularTotal()
        {
            ScriptManager.RegisterStartupScript(this.upTasaInteres, this.upTasaInteres.GetType(), "CalcularTotalScript", "CalcularTotal();", true);
            //decimal importe = this.txtImporte.Decimal;
            //decimal tasa = this.txtTasaInteres.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtTasaInteres.Text);
            //int dias = this.txtDias.Text == string.Empty ? 0 : Convert.ToInt32(this.txtDias.Text);
            //decimal interes = Math.Round(importe * tasa / 365 * dias) / 100;
            //this.txtImporteInteres.Text = interes.ToString("C2");
            //this.txtImporteTotal.Text = (interes + importe).ToString("C2");
        }

        protected void cbTasaEspecial_CheckedChanged(object sender, EventArgs e)
        {
            txtTasaInteres.Enabled = cbTasaEspecial.Checked;
        }
     


        #region Cotitulares

        void ctrAfiliados_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        {
            if (e.Estado.IdEstado == (int)EstadosAfiliados.Baja)
            {
                this.MostrarMensaje("ValidarAfiliadoEstado", true, new List<string>() { e.Estado.Descripcion });
                return;
            }
            if (!this.MiAhoPlazosFijos.Cotitulares.Exists(x => x.Afiliado.IdAfiliado == e.IdAfiliado && x.Estado.IdEstado !=(int)Estados.Baja))
            {
                AhoCotitulares cotitular = new AhoCotitulares();
                cotitular.EstadoColeccion = EstadoColecciones.Agregado;
                cotitular.Estado.IdEstado = (int)Estados.Activo;
                AyudaProgramacion.MatchObjectProperties(e, cotitular.Afiliado);
                this.MiAhoPlazosFijos.Cotitulares.Add(cotitular);
                AyudaProgramacion.CargarGrillaListas<AhoCotitulares>(this.MiAhoPlazosFijos.Cotitulares, false, this.gvDatos, true);
                this.upCotitulares.Update();
            }
        }

        protected void btnAgregarCotitular_Click(object sender, EventArgs e)
        {
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CotitularesCantidadMaxima);
            if (this.MiAhoPlazosFijos.Cotitulares.Count(x => x.Estado.IdEstado == (int)Estados.Activo) < Convert.ToInt32(valor.ParametroValor))
            {
                AfiAfiliados afi = new AfiAfiliados();
                afi.IdAfiliado = this.MiAhoPlazosFijos.IdAfiliado;
                this.ctrAfiliados.IniciarControl(afi, true, EnumAfiliadosTipos.Familiares, false);
                //this.ctrAfiliados.IniciarControl();
            }
            else
            {
                this.MostrarMensaje("CotitularesValidarCantidadMaxima", true, new List<string>() { valor.ParametroValor });
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Borrar"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == "Borrar")
            {
                if (this.MiAhoPlazosFijos.Cotitulares[indiceColeccion].EstadoColeccion == EstadoColecciones.Agregado)
                {
                    this.MiAhoPlazosFijos.Cotitulares.RemoveAt(indiceColeccion);
                    AyudaProgramacion.AcomodarIndices<AhoCotitulares>(this.MiAhoPlazosFijos.Cotitulares);
                }
                else
                {
                    this.MiAhoPlazosFijos.Cotitulares[indiceColeccion].Estado.IdEstado = (int)Estados.Baja;
                    this.MiAhoPlazosFijos.Cotitulares[indiceColeccion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAhoPlazosFijos.Cotitulares[indiceColeccion], Gestion.Anular);
                }
                AyudaProgramacion.CargarGrillaListas<AhoCotitulares>(this.MiAhoPlazosFijos.Cotitulares, true, this.gvDatos, true);
                this.upCotitulares.Update();
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AhoCotitulares item = (AhoCotitulares)e.Row.DataItem;

                switch (this.GestionControl)
                {
                    //case Gestion.Modificar:
                    case Gestion.Agregar:
                    case Gestion.Renovar:
                    case Gestion.Modificar:
                        ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                        //bool permisoModificar = this.ValidarPermiso("CotitularesBajas.aspx");
                        //ibtn.Visible = permisoModificar;
                        ibtn.Visible = true;

                        string mensaje = this.ObtenerMensajeSistema("CotitularesConfirmarBaja");
                        mensaje = string.Format(mensaje, string.Concat(item.Afiliado.ApellidoNombre));
                        string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                        ibtn.Attributes.Add("OnClick", funcion);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}