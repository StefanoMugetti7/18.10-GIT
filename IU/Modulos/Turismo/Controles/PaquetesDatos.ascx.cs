using Afiliados;
using Afiliados.Entidades;
using Cargos;
using Cargos.Entidades;
using Compras;
using Compras.Entidades;
using Comunes.Entidades;
using CuentasPagar.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Proveedores.Entidades;
using Reportes.FachadaNegocio;
using Seguridad.FachadaNegocio;
using Servicio.AccesoDatos;
using Servicio.Encriptacion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Turismo;
using Turismo.Entidades;
namespace IU.Modulos.Turismo.Controles
{
    public partial class PaquetesDatos : ControlesSeguros
    {
        #region Variables de Sesion
        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "PaquetesMisMonedas"]; }
            set { Session[this.MiSessionPagina + "PaquetesMisMonedas"] = value; }
        }
        private List<TurPaquetesDetalles> MisDetalles
        {
            get { return (List<TurPaquetesDetalles>)Session[this.MiSessionPagina + "PaquetesMisDetalles"]; }
            set { Session[this.MiSessionPagina + "PaquetesMisDetalles"] = value; }
        }
        private List<CarTiposCargos> MisCargos
        {
            get { return (List<CarTiposCargos>)Session[this.MiSessionPagina + "PaquetesMisCargos"]; }
            set { Session[this.MiSessionPagina + "PaquetesMisCargos"] = value; }
        }
        private TurPaquetes MiPaquete
        {
            get { return (TurPaquetes)Session[this.MiSessionPagina + "PaqueteModificarDatosMiPaquete"]; }
            set { Session[this.MiSessionPagina + "PaqueteModificarDatosMiPaquete"] = value; }
        }
        private DataSet MiTurismoDetalles
        {
            get { return (DataSet)Session[this.MiSessionPagina + "TurismoMiTurismoDetalles"]; }
            set { Session[this.MiSessionPagina + "TurismoMiTurismoDetalles"] = value; }
        }
        private bool HabilitarControles
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoHabilitarControles"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoHabilitarControles"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoHabilitarControles"] = value; }
        }
        private bool MiArmarParametros
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametros"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametros"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametros"] = value; }
        }
        private bool MiArmarParametrosTurismo
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametrosTurismo"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametrosTurismo"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiArmarParametrosTurismo"] = value; }
        }
        private List<TGECampos> MisCamposServicios
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMisCamposServicios"] == null ?
                    (List<TGECampos>)(Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMisCamposServicios"] = new List<TGECampos>()) : (List<TGECampos>)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMisCamposServicios"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMisCamposServicios"] = value; }
        }
        private List<TGECampos> MisCamposTurismo
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "ControlMisCamposTurismo"] == null ?
                    (List<TGECampos>)(Session[this.MiSessionPagina + this.ClientID + "ControlMisCamposTurismo"] = new List<TGECampos>()) : (List<TGECampos>)Session[this.MiSessionPagina + this.ClientID + "ControlMisCamposTurismo"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlMisCamposTurismo"] = value; }
        }
        private CarTiposCargosAfiliadosFormasCobros MiTablaValor
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiTablaValor"] == null ? new CarTiposCargosAfiliadosFormasCobros() : (CarTiposCargosAfiliadosFormasCobros)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiTablaValor"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMiTablaValor"] = value; }
        }
        #endregion
        #region css
        const string _cssLabel = "col-sm-3 col-form-label";
        const string _cssLabelCol6 = "col-lg-12 col-form-label";
        const string _cssRow = "row";
        const string _cssCol3 = "col-sm-9";
        const string _cssCol2 = "col-sm-12";
        const string _cssContainer = "col-12 col-md-8 col-lg-4";
        const string preid = "EvolCvId";
        public bool MostrarControl
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMostrarControl"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMostrarControl"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoMostrarControl"] = value; }
        }
        public string cssLabel
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismoccsLabel"] == null ? _cssLabel : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoccsLabel"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismoccsLabel"] = value; }
        }
        public string cssLabelCol6
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssLabelCol6"] == null ? _cssLabelCol6 : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismoccsLabelCol6"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssLabelCol6"] = value; }
        }
        public string cssRow
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssRow"] == null ? _cssRow : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssRow"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssRow"] = value; }
        }
        public string cssCol
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssCol"] == null ? _cssCol3 : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssCol"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssCol"] = value; }
        }
        public string ccsContainer
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssContainer"] == null ? _cssContainer : (string)Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssContainer"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "ControlTurismocssContainer"] = value; }
        }
        #endregion
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.IsPostBack)
            {
                this.MiArmarParametrosTurismo = false;
                this.MisCamposTurismo = new List<TGECampos>();
            }
            if (this.MiArmarParametros)
            {
                this.ObtenerValoresParametros(this.MisCamposServicios);
                this.ArmarTablaParametros(this.pnlCamposDinamicosTurismoServicios, this.MisCamposServicios, this.HabilitarControles);
            }
            if (this.MiArmarParametrosTurismo)
            {
                this.ObtenerValoresParametros(this.MisCamposTurismo);
                this.ArmarTablaParametros(this.pnlCamposDinamicosTurismoServicios, this.MisCamposTurismo, this.HabilitarControles);
                this.pnlPrinc.Update();
            }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvReservasTurismo.PageSizeEvent += this.GvReservas_PageSizeEvent;
            if (this.IsPostBack)
            {
                if (this.MiPaquete == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            else
            {
                this.CargarCargosReservasTurismo(new CarTiposCargosAfiliadosFormasCobros());
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>("filtroMultitab");
                if (parametrosFiltros.BusquedaParametros)
                {
                    this.tcDatos.ActiveTabIndex = parametrosFiltros.HashTransaccion;
                    if (this.tcDatos.ActiveTab.ID == "tpTurismo")
                    {
                        this.gvReservasTurismo.PageIndex = parametrosFiltros.HashTransaccion;
                    }
                }
            }
        }
        public void IniciarControl(TurPaquetes pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MisMonedas = TGEGeneralesF.MonedasObtenerListaActiva();
            this.MisCargos = TurismoF.PaquetesObtenerTiposCargos(new TurPaquetes());
            this.CargarCombos();
            this.HabilitarControles = true;
            this.MiArmarParametros = false;
            this.MiArmarParametrosTurismo = false;
            this.MisCamposServicios = new List<TGECampos>();
            this.MiTablaValor = new CarTiposCargosAfiliadosFormasCobros();
            TurPaquetesDetalles turCampos = new TurPaquetesDetalles();
            this.MisCamposTurismo = TGEGeneralesF.CamposObtenerListaFiltro(turCampos, new Objeto());
            this.ArmarTablaParametros(this.pnlCamposDinamicosTurismoServicios, this.MisCamposTurismo, this.HabilitarControles);
            this.MiArmarParametrosTurismo = this.MisCamposTurismo.Count > 0;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPaquete = pParametro;
                    this.ddlEstado.SelectedValue = ((int)EstadosPaquetesTurismo.Pendiente).ToString();
                    this.ddlEstado.Enabled = false;
                    #region inicializo currency
                    this.txtImpDetalle.Text = "$0,00";
                    this.txtCostoDetalle.Text = "$0,00";
                    this.txtImporte.Text = "$0,00";
                    this.txtCostoTotal.Text = "$0,00";
                    #endregion
                    this.ddlMoneda_SelectedIndexChanged(null, new EventArgs());
                    this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);
                    this.MisDetalles = new List<TurPaquetesDetalles>();
                    this.txtCantidad.Attributes.Add("onchange", "CalcularTotalImporte();");
                    break;
                case Gestion.Modificar:
                    this.MiPaquete = TurismoF.PaquetesObtenerDatosCompletos(pParametro);
                    this.MisDetalles = this.MiPaquete.Detalles;
                    this.ddlEstado.Items.Remove(this.ddlEstado.Items.FindByValue(((int)EstadosPaquetesTurismo.Autorizado).ToString()));
                    this.MapearObjetoAControles(this.MiPaquete);
                    this.SetInitializeCulture(this.MiPaquete.Moneda.Moneda);
                    this.txtCantidad.Attributes.Add("onchange", "CalcularTotalImporte();");
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularTotalImporte", "CalcularTotalImporte();", true);
                    break;
                case Gestion.Consultar:
                    this.MisDetalles = this.MiPaquete.Detalles;
                    this.MiPaquete = TurismoF.PaquetesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiPaquete);
                    this.SetInitializeCulture(this.MiPaquete.Moneda.Moneda);
                    this.btnAceptar.Visible = false;
                    this.ddlEstado.Enabled = false;
                    this.ddlMoneda.Enabled = false;
                    this.ddlTipoCargo.Enabled = false;
                    this.txtCantidad.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.txtNombre.Enabled = false;
                    #region OCULTO CONTROLES  
                    this.btnCargar.Visible = false;
                    this.ddlProveedor.Visible = false;
                    this.txtImpDetalle.Visible = false;
                    this.txtCostoDetalle.Visible = false;
                    this.ddlServicio.Visible = false;
                    this.lblCostoDetalle.Visible = false;
                    this.lblImporteDetalle.Visible = false;
                    this.lblServicio.Visible = false;
                    this.lblProveedor.Visible = false;
                    this.gvPaquetes.Columns[this.gvPaquetes.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    #endregion
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularTotalImporte", "CalcularTotalImporte();", true);
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("PaquetesTurismo");
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlServicio.DataSource = ComprasF.ObtenerProductosServiciosTurismo(new CMPProductos());
            this.ddlServicio.DataValueField = "IdProducto";
            this.ddlServicio.DataTextField = "Descripcion";
            this.ddlServicio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlServicio, ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlMoneda.DataSource = this.MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoCargo.DataSource = this.MisCargos;
            this.ddlTipoCargo.DataValueField = "IdTipoCargo";
            this.ddlTipoCargo.DataTextField = "TipoCargo";
            this.ddlTipoCargo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoCargo, ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.InsertarItemSeleccione(this.ddlProveedoresPagos, ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(TurPaquetes pParametro)
        {
            this.txtNombre.Text = pParametro.Nombre;
            this.txtCantidad.Text = pParametro.Cantidad.ToString();
            this.txtNroPaquete.Text = pParametro.IdPaquete == 0 ? "" : pParametro.IdPaquete.ToString();
            this.txtCostoTotal.Text = pParametro.Costo.ToString();
            this.txtImporte.Text = pParametro.Importe.ToString();
            this.ddlTipoCargo.SelectedValue = pParametro.IdTipoCargo.ToString();
            this.txtFechaRegreso.Text = pParametro.FechaRegreso.Value.ToShortDateString();
            this.txtFechaSalida.Text = pParametro.FechaSalida.Value.ToShortDateString();

            ListItem item3 = this.ddlMoneda.Items.FindByValue(pParametro.Moneda.IdMoneda.ToString());
            if (item3 == null)
                this.ddlMoneda.Items.Add(new ListItem(pParametro.Moneda.Descripcion, pParametro.Moneda.IdMoneda.ToString()));
            this.ddlMoneda.SelectedValue = pParametro.Moneda.IdMoneda.ToString();

            ListItem item2 = ddlEstado.Items.FindByValue(pParametro.Estado.IdEstado.ToString());
            if (item2 == null)
                this.ddlEstado.Items.Add(new ListItem(pParametro.Estado.Descripcion, pParametro.Estado.IdEstado.ToString()));
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();

            string xml;
            xml = this.ObtenerListaCamposValores(this.MiPaquete.Detalles, this.MisCamposTurismo).InnerXml;
            this.MapearDatos(xml);
            if (this.MiPaquete.Detalles.Count > 0)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularTotalImporte", "CalcularTotalImporte();", true);
            }
            this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);
        }
        private void MapearControlesAObjeto(TurPaquetes pParametro)
        {
            pParametro.Nombre = this.txtNombre.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.IdTipoCargo = Convert.ToInt32(this.ddlTipoCargo.SelectedValue);
            pParametro.Importe = decimal.Parse(this.txtImporte.Text, NumberStyles.Currency);
            pParametro.Cantidad = Convert.ToInt32(this.txtCantidad.Text);
            pParametro.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            pParametro.FechaSalida = Convert.ToDateTime(this.txtFechaSalida.Text);
            pParametro.FechaRegreso = Convert.ToDateTime(this.txtFechaRegreso.Text);
            pParametro.Costo = decimal.Parse(this.txtCostoTotal.Text, NumberStyles.Currency);
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
        }
        private List<TurPaquetesDetalles> ObtenerEliminados(List<TurPaquetesDetalles> listConBorrados, List<TurPaquetesDetalles> listA)
        {
            List<TurPaquetesDetalles> retorno = new List<TurPaquetesDetalles>(listA);
            foreach (TurPaquetesDetalles item2 in listConBorrados)
            {
                var aux = listA.Find(x => x.IdPaqueteDetalle == item2.IdPaqueteDetalle);
                if (aux == null)//noexiste
                {
                    item2.EstadoColeccion = EstadoColecciones.Borrado;
                    retorno.Add(item2);
                }
            }
            return retorno;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiPaquete);
            this.MiPaquete.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            string xml = this.ObtenerListaCamposValores(this.MiPaquete.Detalles, this.MisCamposTurismo).InnerXml;
            string xml2 = this.ObtenerListaCamposValores(this.MisDetalles, this.MisCamposTurismo).InnerXml;
            List<TurPaquetesDetalles> lista = this.ObtenerListaServiciosDesdeXML(xml);//contiene los borrados
            List<TurPaquetesDetalles> listaConBorrados = this.ObtenerListaServiciosDesdeXML(xml2);//no contiene los borrados
            this.MiPaquete.Detalles = this.ObtenerEliminados(lista, listaConBorrados);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPaquete.IdPaquete = 0;
                    guardo = TurismoF.PaquetesAgregar(this.MiPaquete);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiPaquete.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = TurismoF.PaquetesModificar(this.MiPaquete);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiPaquete.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiPaquete.CodigoMensaje, true, this.MiPaquete.CodigoMensajeArgs);
                if (this.MiPaquete.dsResultado != null)
                {
                    this.MiPaquete.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.ControlModificarDatosCancelar?.Invoke();
        }
        #region DETALLES
        protected void gvPaquetes_RowCreated(object sender, GridViewRowEventArgs e)
        { }
        protected void gvPaquetes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                  || e.CommandName == Gestion.Modificar.ToString()))
                return;

            this.hdfIdPaqueteDetalle.Value = string.Empty;
            int index = Convert.ToInt32(e.CommandArgument);
            int idTurismoDetalle = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.hdfIdPaqueteDetalle.Value = idTurismoDetalle.ToString();
                TurPaquetesDetalles miServicio = new TurPaquetesDetalles();
                miServicio = MisDetalles.Find(x => x.IdPaqueteDetalle == idTurismoDetalle);
                if (miServicio == null)
                    return;

                this.ddlServicio.SelectedValue = miServicio.Producto.IdProducto.ToString();
                this.txtImpDetalle.Text = "$" + miServicio.Importe.ToString("N2");
                this.txtCostoDetalle.Text = "$" + miServicio.Costo.ToString("N2");
                this.hdfIdProveedor.Value = miServicio.IdProveedor.ToString();
                this.hdfProveedor.Value = miServicio.Proveedor.ToString();
                this.ddlProveedor.Items.Add(new ListItem(miServicio.Proveedor, miServicio.IdProveedor.ToString()));
                this.ddlProveedor.SelectedValue = miServicio.IdProveedor.ToString();
                this.ddlProveedor.Enabled = false;
                this.ddlServicio.Enabled = false;
                this.pnlCamposDinamicosTurismoServicios.Controls.Clear();
                this.MisCamposServicios = new List<TGECampos>();
                this.HabilitarControles = true;

                CMPProductos turParam = new CMPProductos();
                turParam.IdProducto = Convert.ToInt32(this.ddlServicio.SelectedValue);
                this.MisCamposServicios = TGEGeneralesF.CamposObtenerListaFiltro(miServicio, turParam);
                foreach (TGECampos c in this.MisCamposServicios)
                {
                    TGECampos val = miServicio.Campos.FirstOrDefault(x => x.IdCampo == c.IdCampo);
                    if (val != null)
                        c.CampoValor = val.CampoValor;
                }
                this.ArmarTablaParametros(this.pnlCamposDinamicosTurismoServicios, this.MisCamposServicios, this.HabilitarControles);
                this.MiArmarParametros = this.MisCamposServicios.Count > 0;
                this.MostrarControl = this.MisCamposServicios.Count > 0;
                this.pnlCamposDinamicosTurismoServicios.Visible = this.MisCamposServicios.Count > 0;
                this.pnlPrinc.Update();
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                TurPaquetesDetalles aux2 = this.MisDetalles.Find(x => x.IdPaqueteDetalle == idTurismoDetalle);
                if (this.MisDetalles.Contains(aux2))
                {
                    this.MisDetalles.Remove(aux2);
                }
                //this.MiPaquete.Detalles = MisSericios;
                string xml = this.ObtenerListaCamposValores(this.MisDetalles, this.MisCamposTurismo).InnerXml;
                this.hdfIdPaquete.Value = Encriptar.EncriptarTexto(xml);
                this.MapearDatos(xml);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularTotalImporte", "CalcularTotalImporte();", true);
                this.pnlCamposDinamicosTurismoServicios.Controls.Clear();
                this.LimpiarControles();
                ListItem item = this.ddlProveedoresPagos.Items.FindByValue(aux2.IdProveedor.ToString());
                if (item != null)
                {
                    this.ddlProveedoresPagos.Items.Remove(item);
                }
                this.upPagos.Update();
            }
        }
        protected void gvPaquetes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton cancelar = (ImageButton)e.Row.FindControl("btnCancelar");
                HiddenField hdfImporte = (HiddenField)e.Row.FindControl("hdfImporte");
                string idTurismoDetalle = this.gvPaquetes.DataKeys[e.Row.DataItemIndex].Value.ToString();
                if (this.MiTurismoDetalles.Tables[1].AsEnumerable()
                            .Where(row => row.Field<int>("IdPaqueteDetalle") == Convert.ToInt32(idTurismoDetalle)).Count() > 0)
                {
                    Literal detalles = e.Row.FindControl("ltlDetalleCampos") as Literal;
                    DataTable tblFiltered = this.MiTurismoDetalles.Tables[1].AsEnumerable()
                            .Where(row => row.Field<int>("IdPaqueteDetalle") == Convert.ToInt32(idTurismoDetalle))
                            .CopyToDataTable();
                    string codigo = string.Empty;
                    codigo = string.Concat(codigo, "<table>");
                    foreach (DataRow r in tblFiltered.Rows)
                    {
                        codigo = string.Concat(codigo, string.Format("<tr><td>{0}</td><td>{1}</td></tr>", r["Titulo"].ToString(), r["Valor"].ToString()));
                    }
                    codigo = string.Concat(codigo, "</table>");
                    detalles.Text = codigo;
                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lbl = (Label)e.Row.FindControl("lblCostoTotal");
                lbl.Text = this.MiTurismoDetalles.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("Costo")).ToString("C2");
                lbl = (Label)e.Row.FindControl("lblImporteTotal");
                lbl.Text = this.MiTurismoDetalles.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("Importe")).ToString("C2");
            }
        }
        protected void ddlServicio_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.hdfIdPaqueteDetalle.Value = "";
            this.pnlCamposDinamicosTurismoServicios.Controls.Clear();
            this.MisCamposServicios = new List<TGECampos>();
            this.HabilitarControles = true;

            if (!string.IsNullOrEmpty(ddlServicio.SelectedValue))
            {
                TurPaquetesDetalles turCampos = new TurPaquetesDetalles();
                CMPProductos turParam = new CMPProductos();
                turParam.IdProducto = Convert.ToInt32(this.ddlServicio.SelectedValue);
                this.MisCamposServicios = TGEGeneralesF.CamposObtenerListaFiltro(turCampos, turParam);
                this.ArmarTablaParametros(this.pnlCamposDinamicosTurismoServicios, this.MisCamposServicios, this.HabilitarControles);
                this.MiArmarParametros = this.MisCamposServicios.Count > 0;
            }
            this.MostrarControl = this.MisCamposServicios.Count > 0;
            this.pnlCamposDinamicosTurismoServicios.Visible = this.MisCamposServicios.Count > 0;
        }
        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlMoneda.SelectedValue))
            {
                TGEMonedas moneda = this.MisMonedas.First(x => x.IdMoneda == Convert.ToInt32(this.ddlMoneda.SelectedValue));
                this.SetInitializeCulture(moneda.Moneda);
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            TurPaquetesDetalles reserva = new TurPaquetesDetalles();
            string xml = this.ObtenerListaCamposValores(MisDetalles, this.MisCamposTurismo).InnerXml;
            List<TurPaquetesDetalles> MisSericios = this.MisDetalles;//ObtenerListaServiciosDesdeXML(xml);
            reserva.IdPaqueteDetalle = (MisSericios.Count > 0 ? MisSericios.Max(x => x.IdPaqueteDetalle) : 0) + 1;
            reserva.IdProveedor = Convert.ToInt32(this.hdfIdProveedor.Value);
            reserva.Proveedor = this.hdfProveedor.Value;
            reserva.Producto.IdProducto = Convert.ToInt32(this.ddlServicio.SelectedValue);
            reserva.Producto.Descripcion = this.ddlServicio.SelectedItem.Text;
            reserva.Costo = decimal.Parse(txtCostoDetalle.Text, NumberStyles.Currency);
            reserva.Importe = decimal.Parse(txtImpDetalle.Text, NumberStyles.Currency);
            int indice = 0;
            if (!string.IsNullOrEmpty(this.hdfIdPaqueteDetalle.Value)
                && MisSericios.Count > 0)
            {
                reserva.IdPaqueteDetalle = Convert.ToInt32(this.hdfIdPaqueteDetalle.Value);
                indice = MisSericios.IndexOf(MisSericios.Find(x => x.IdPaqueteDetalle == Convert.ToInt32(this.hdfIdPaqueteDetalle.Value)));
                MisSericios.Remove(MisSericios.Find(x => x.IdPaqueteDetalle == Convert.ToInt32(this.hdfIdPaqueteDetalle.Value)));
            }
            reserva.Campos = this.MisCamposServicios;
            reserva.EstadoColeccion = EstadoColecciones.Agregado;
            MisSericios.Insert(indice, reserva);
            //MisDetalles.Add(reserva);
            this.MiPaquete.Detalles = this.MisDetalles;
            xml = this.ObtenerListaCamposValores(MisDetalles, this.MisCamposTurismo).InnerXml;
            this.hdfIdPaquete.Value = Encriptar.EncriptarTexto(xml);
            this.MapearDatos(xml);
            decimal total = MisSericios.Sum(x => x.Importe);
            this.hdfTotalServicios.Value = total.ToString("N2").Replace(".", "").Replace(",", ".");
            string stotal = total.ToString().Replace(",", ".");
            this.ddlProveedor.Items.Clear();
            this.hdfIdProveedor.Value = "";
            this.hdfProveedor.Value = "";
            this.ddlServicio.SelectedValue = "";
            this.pnlCamposDinamicosTurismoServicios.Controls.Clear();
            this.ddlProveedor.Enabled = true;
            this.ddlServicio.Enabled = true;
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "CalcularTotalImporte", "CalcularTotalImporte();", true);
            this.LimpiarControles();
        }
        protected void LimpiarControles()
        {
            this.txtCostoDetalle.Decimal = 0;
            this.txtImpDetalle.Decimal = 0;
        }
        private void MapearDatos(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                this.gvPaquetes.DataSource = null;
                this.gvPaquetes.DataBind();
                AyudaProgramacion.FixGridView(this.gvPaquetes);
                return;
            }
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode nodo = doc.SelectSingleNode("TurismoDetalles");
            if (nodo is null)
                return;

            this.MiTurismoDetalles = new DataSet();
            TGECampos MiCampo = new TGECampos();
            MiCampo.CampoValor.Valor = xml;
            this.MiTurismoDetalles = TurismoF.PaquetesObtenerReservasDetallesDesdeXML(MiCampo);
            this.gvPaquetes.DataSource = this.MiTurismoDetalles.Tables[0];
            this.gvPaquetes.DataBind();
            AyudaProgramacion.FixGridView(this.gvPaquetes);
            this.hdfTotalServicios.Value = this.MiTurismoDetalles.Tables[0].AsEnumerable().Sum(x => x.Field<decimal>("Importe")).ToString("N2").Replace(".", "").Replace(",", ".");

            if (nodo.HasChildNodes)
            {
                List<CapProveedores> proveedores = new List<CapProveedores>();
                CapProveedores prov;
                foreach (XmlNode item in nodo.ChildNodes)
                {
                    if (!proveedores.Exists(x => x.IdProveedor == Convert.ToInt32(item.Attributes["IdProveedor"].Value)))
                    {
                        prov = new CapProveedores
                        {
                            IdProveedor = Convert.ToInt32(item.Attributes["IdProveedor"].Value),
                            RazonSocial = item.Attributes["Proveedor"].Value
                        };
                        proveedores.Add(prov);
                    }
                }
                this.ddlProveedor.Items.Clear();
                this.ddlProveedor.SelectedIndex = -1;
                this.ddlProveedor.SelectedValue = null;
                this.ddlProveedor.ClearSelection();

                this.ddlProveedor.DataSource = proveedores;
                this.ddlProveedor.DataValueField = "IdProveedor";
                this.ddlProveedor.DataTextField = "RazonSocial";
                this.ddlProveedor.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(this.ddlProveedor, ObtenerMensajeSistema("SeleccioneOpcion"));

                /*AGREGO LOS MISMOS PROVEEDORES AL COMBO EN SOLAPA PAGOS*/
                this.ddlProveedoresPagos.Items.Clear();
                this.ddlProveedoresPagos.SelectedIndex = -1;
                this.ddlProveedoresPagos.SelectedValue = null;
                this.ddlProveedoresPagos.ClearSelection();

                this.ddlProveedoresPagos.DataSource = proveedores;
                this.ddlProveedoresPagos.DataValueField = "IdProveedor";
                this.ddlProveedoresPagos.DataTextField = "RazonSocial";
                this.ddlProveedoresPagos.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(this.ddlProveedoresPagos, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        private List<TurPaquetesDetalles> ObtenerListaServiciosDesdeXML(string xml)
        {
            List<TurPaquetesDetalles> lista = new List<TurPaquetesDetalles>();
            if (string.IsNullOrEmpty(xml))
                return lista;

            TurPaquetesDetalles servicio;
            TGECampos campo;
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            XmlNode nodo = doc.SelectSingleNode("TurismoDetalles");
            if (nodo is null)
                return lista;

            XmlNodeList nodos = doc.SelectNodes("TurismoDetalles/TurismoDetalle");
            foreach (XmlNode item in nodos)
            {
                servicio = new TurPaquetesDetalles();
                servicio.IdPaqueteDetalle = Convert.ToInt32(item.Attributes["IdPaqueteDetalle"].Value);
                servicio.Producto.IdProducto = Convert.ToInt32(item.Attributes["IdProducto"].Value);
                servicio.Producto.Descripcion = item.Attributes["ProductoDescripcion"].Value;
                servicio.IdProveedor = Convert.ToInt32(item.Attributes["IdProveedor"].Value);
                servicio.Proveedor = item.Attributes["Proveedor"].Value;
                servicio.Costo = Convert.ToDecimal(item.Attributes["Costo"].Value.Replace(".", ","));
                servicio.Importe = Convert.ToDecimal(item.Attributes["Importe"].Value.Replace(".", ","));
                if (item.Attributes["EstadoColeccion"].Value.ToString() == "Borrado")
                {
                    servicio.EstadoColeccion = EstadoColecciones.Borrado;
                }
                if (item.HasChildNodes)
                {
                    foreach (XmlNode cv in item.ChildNodes)
                    {
                        campo = new TGECampos();
                        campo.IdCampo = Convert.ToInt32(cv.Attributes["IdCampo"].Value);
                        campo.Nombre = cv.Attributes["Nombre"].Value;
                        campo.CampoValor.Valor = cv.Attributes["Valor"].Value;
                        campo.CampoValor.ListaValor = cv.Attributes["ListaValor"].Value;
                        servicio.Campos.Add(campo);
                    }
                }
                if (servicio.EstadoColeccion != EstadoColecciones.Borrado)
                {
                    lista.Add(servicio);
                }
            }
            return lista;
        }
        private XmlDocument ObtenerListaCamposValores(List<TurPaquetesDetalles> listaServicios, List<TGECampos> listaCamposTurismo)
        {
            XmlNode itemNodo;
            XmlNode itemCampos;
            XmlAttribute itemAttribute;
            XmlDocument loteCampos = new XmlDocument();
            XmlNode nodos = loteCampos.CreateElement("TurismoDetalles");

            if (listaServicios.Count > 0)
            {
                itemAttribute = loteCampos.CreateAttribute("TotalServicios");
                itemAttribute.Value = listaServicios.Sum(x => x.Importe).ToString("N2").Replace(".", "").Replace(",", "."); ;
                nodos.Attributes.Append(itemAttribute);
            }
            loteCampos.AppendChild(nodos);

            foreach (TurPaquetesDetalles item in listaServicios)
            {
                itemNodo = loteCampos.CreateElement("TurismoDetalle");

                itemAttribute = loteCampos.CreateAttribute("IdPaqueteDetalle");
                itemAttribute.Value = item.IdPaqueteDetalle.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("IdProducto");
                itemAttribute.Value = item.Producto.IdProducto.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("ProductoDescripcion");
                itemAttribute.Value = item.Producto.Descripcion;
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("IdProveedor");
                itemAttribute.Value = item.IdProveedor.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Proveedor");
                itemAttribute.Value = item.Proveedor.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Costo");
                itemAttribute.Value = item.Costo.ToString("N2").Replace(".", "").Replace(",", ".");
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Importe");
                itemAttribute.Value = item.Importe.ToString("N2").Replace(".", "").Replace(",", ".");
                itemNodo.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("EstadoColeccion");
                itemAttribute.Value = item.EstadoColeccion.ToString();
                itemNodo.Attributes.Append(itemAttribute);

                foreach (TGECampos servicio in item.Campos)
                {
                    itemCampos = loteCampos.CreateElement("ServiciosCampos");

                    itemAttribute = loteCampos.CreateAttribute("IdPaqueteDetalle");
                    itemAttribute.Value = item.IdPaqueteDetalle.ToString();
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("IdCampo");
                    itemAttribute.Value = servicio.IdCampo.ToString();
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("Nombre");
                    itemAttribute.Value = servicio.Nombre;
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("Valor");
                    itemAttribute.Value = servicio.CampoValor.Valor;
                    itemCampos.Attributes.Append(itemAttribute);

                    itemAttribute = loteCampos.CreateAttribute("ListaValor");
                    itemAttribute.Value = servicio.CampoValor.ListaValor;
                    itemCampos.Attributes.Append(itemAttribute);
                    itemNodo.AppendChild(itemCampos);
                }
                nodos.AppendChild(itemNodo);
            }

            XmlNode nodoTur = loteCampos.CreateElement("TurismoCampos");
            foreach (TGECampos item in listaCamposTurismo)
            {
                itemCampos = loteCampos.CreateElement("TurismoCampo");

                itemAttribute = loteCampos.CreateAttribute("IdCampo");
                itemAttribute.Value = item.IdCampo.ToString();
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Nombre");
                itemAttribute.Value = item.Nombre;
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("Valor");
                itemAttribute.Value = item.CampoValor.Valor;
                itemCampos.Attributes.Append(itemAttribute);

                itemAttribute = loteCampos.CreateAttribute("ListaValor");
                itemAttribute.Value = item.CampoValor.ListaValor;
                itemCampos.Attributes.Append(itemAttribute);

                nodoTur.AppendChild(itemCampos);
            }
            return loteCampos;
        }
        private void ArmarTablaParametros(PlaceHolder phCamposDinamicos, List<TGECampos> pCampos, bool pHabilitar)
        {
            List<Control> controlesEliminar = new List<Control>();
            foreach (Control ctr in phCamposDinamicos.Controls)// this.pnlCamposDinamicosTurismoServicios.Controls)
            {
                if (ctr is PlaceHolder)
                {
                    if (!pCampos.Exists(x => "panel" + x.IdCampo.ToString() == ctr.ID))
                        controlesEliminar.Add(ctr);
                }
            }
            foreach (Control ctr in controlesEliminar)
                phCamposDinamicos.Controls.Remove(ctr);
            //pCampos = pCampos.OrderBy(x => x.Orden).ToList();
            List<TGEListasValoresDetalles> listaValoresDetalles;
            Control ctrExiste;
            foreach (TGECampos parametro in pCampos)
            {
                ctrExiste = phCamposDinamicos.FindControl("panel" + parametro.IdCampo.ToString());
                if (ctrExiste != null)
                {
                    foreach (Control c in ctrExiste.Controls)
                        if (c is WebControl)
                        {
                            if (c is PlaceHolder)
                                foreach (Control d in c.Controls)
                                    if (d is WebControl)
                                        ((WebControl)d).Enabled = pHabilitar;
                        }
                    continue;
                }
                if (parametro.SaltoLinea)
                {
                    Panel pnlWrap = new Panel();
                    pnlWrap.CssClass = "w-100";
                    phCamposDinamicos.Controls.Add(pnlWrap);
                }
                listaValoresDetalles = new List<TGEListasValoresDetalles>();
                switch (parametro.CampoTipo.IdCampoTipo)
                {
                    case (int)EnumCamposTipos.DropDownList:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresObtenerListaDetalle(parametro.ListaValor);
                        phCamposDinamicos.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListSP:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(parametro, this.MiTablaValor);
                        phCamposDinamicos.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
                        phCamposDinamicos.Controls.Add(AddListBoxAutocompleteRow(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListQuery:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerConsultaDinamica(parametro, this.MiTablaValor);
                        phCamposDinamicos.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.TextBox:
                        PlaceHolder textboxEnPlaceHolder = AddTextBoxRow(parametro, pHabilitar);
                        phCamposDinamicos.Controls.Add(textboxEnPlaceHolder);
                        phCamposDinamicos.Controls.Add(AddLabelRow(parametro, textboxEnPlaceHolder));
                        break;
                    case (int)EnumCamposTipos.DateTime:
                        phCamposDinamicos.Controls.Add(AddDateTimeRow(parametro, pHabilitar));
                        break;
                    default:
                        break;
                }
            }
            //pnlCamposDinamicosTurismoServicios.Controls.Add(pnlRow);
        }
        private void ObtenerValoresParametros(List<TGECampos> pCampos)
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;
            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("$" + this.ID + "$")).ToList();
            string k, l;
            foreach (TGECampos parametro in pCampos)
            {
                switch (parametro.CampoTipo.IdCampoTipo)
                {
                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        k = keys.Find(z => z.EndsWith(preid + "select2HdfText" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.ListaValor = this.Request.Form[k];
                        break;
                    case (int)EnumCamposTipos.ComboBoxSP:
                        k = keys.Find(x => x.EndsWith(preid + "ListComboBox" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        break;
                    case (int)EnumCamposTipos.CheckBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = string.IsNullOrEmpty(this.Request.Form[k]) ? false.ToString() : this.Request.Form[k] == "on" ? true.ToString() : false.ToString();
                        else
                            parametro.CampoValor.Valor = false.ToString();
                        break;
                    case (int)EnumCamposTipos.DropDownList:
                    case (int)EnumCamposTipos.DropDownListSP:
                    case (int)EnumCamposTipos.DropDownListQuery:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            parametro.CampoValor.Valor = this.Request.Form[k];
                            if (parametro.CampoValor.Valor.Trim() == string.Empty)
                                parametro.CampoValor.ListaValor = string.Empty;
                            else
                            {
                                l = keys.Find(z => z.EndsWith(preid + "HdfText" + parametro.IdCampo.ToString()));
                                parametro.CampoValor.ListaValor = string.IsNullOrEmpty(l) ? string.Empty : this.Request.Form[l];
                            }
                        }
                        break;
                    case (int)EnumCamposTipos.DropDownListMultiple:
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        }
                        else
                        {
                            parametro.CampoValor.Valor = "";
                        }
                        break;
                    case (int)EnumCamposTipos.TextBox:
                    case (int)EnumCamposTipos.DateTime:
                    case (int)EnumCamposTipos.IntegerTextBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];

                        break;
                    case (int)EnumCamposTipos.CurrencyTextBox:
                    case (int)EnumCamposTipos.NumericTextBox:
                    case (int)EnumCamposTipos.PercentTextBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = decimal.Parse(this.Request.Form[k].Replace(culture.NumberFormat.CurrencySymbol, string.Empty)).ToString();
                        break;
                    case (int)EnumCamposTipos.GrillaDinamicaAB:
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            string value = this.Request.Form[k];
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                List<string> values = parametro.CampoValor.Valor.Split(';').ToList();
                                if (!values.Exists(x => x == value))
                                {
                                    if (parametro.CampoValor.Valor.Trim().Length > 0)
                                        parametro.CampoValor.Valor += ";" + value;
                                    else
                                        parametro.CampoValor.Valor = value;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
                //parametro.CampoValor.IdRefTablaValor = this.MiIdRefTablaValor;
                parametro.CampoValor.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(parametro.CampoValor, this.GestionControl);

                if (parametro.CampoValor.IdCampoValor == 0 && parametro.CampoValor.Valor.Trim() == string.Empty)
                {
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.SinCambio;
                }
                else if (parametro.CampoValor.IdCampoValor == 0)
                {
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.Agregado;
                    //parametro.CampoValor.IdRefTablaValor = this.MiIdRefTablaValor;
                }
                else
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.Modificado;

            }
        }
        private PlaceHolder AddListBoxRow(TGECampos pCampo, List<TGEListasValoresDetalles> dsDatos, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = cssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol;

            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.CssClass = "form-control select2";

            ddlListaOpciones.DataValueField = "IdListaValorDetalle";
            ddlListaOpciones.DataTextField = "Descripcion";
            ddlListaOpciones.DataSource = dsDatos;
            ddlListaOpciones.DataBind();
            ListItem item = new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), string.Empty);
            ddlListaOpciones.Items.Add(item);
            ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
            item = ddlListaOpciones.Items.FindByValue(pCampo.CampoValor.Valor);
            if (item != null)
                ddlListaOpciones.SelectedValue = pCampo.CampoValor.Valor;
            else
                ddlListaOpciones.SelectedValue = string.Empty;
            ddlListaOpciones.Enabled = pHabilitar;

            if (this.MisCamposServicios.Exists(x => x.IdCampoDependiente.HasValue && x.IdCampoDependiente == pCampo.IdCampo))
            {
                //ddlListaOpciones.AutoPostBack = true;
                //ddlListaOpciones.SelectedIndexChanged += DdlListaOpciones_SelectedIndexChanged;
            }

            HiddenField hdfText = new HiddenField();
            hdfText.ID = preid + "HdfText" + pCampo.IdCampo.ToString();
            hdfText.Value = pCampo.CampoValor.ListaValor;

            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfText);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "TurismoCamposValores";
                pnlRow.Controls.Add(validador);
            }
            panel.Controls.Add(pnlRow);
            if (pHabilitar)
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControl|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);
                //Select Change
                script.AppendLine("control.change(function() { ");
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendFormat("hdfText.val( $('select[name$={0}] option:selected').text() );", ddlListaOpciones.ID);

                if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
                {
                    script.AppendLine(pCampo.EventoJavaScript);
                }

                script.AppendLine("});");
                //end Select Change

                script.AppendLine("};");
                //end function
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControl|CTRLID|);");
                script.AppendLine("InitControl|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = cssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = ccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);
            return panel;
        }
        private PlaceHolder AddListBoxAutocompleteRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = cssLabel;
            lblParametro.Text = pCampo.Titulo;
            //panel.Controls.Add(lblParametro);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol;
            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.Enabled = pHabilitar;

            if (pCampo.CampoValor.Valor.Trim().Length > 0)
            {
                TGECamposValores parametro = new TGECamposValores();
                parametro.Valor = pCampo.CampoValor.Valor.Trim();
                parametro.IdRefTablaValor = 0;
                DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet(pCampo.StoredProcedure, parametro);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pCampo.CampoValor.ListaValor = ds.Tables[0].Rows[0]["Descripcion"].ToString();

                ListItem item = new ListItem(pCampo.CampoValor.ListaValor, pCampo.CampoValor.Valor);
                ddlListaOpciones.Items.Add(item);
            }

            HiddenField hdfValue = new HiddenField();
            hdfValue.ID = preid + "select2HdfValue" + pCampo.IdCampo.ToString();
            hdfValue.Value = pCampo.CampoValor.Valor;
            HiddenField hdfText = new HiddenField();
            hdfText.ID = preid + "select2HdfText" + pCampo.IdCampo.ToString();
            hdfText.Value = pCampo.CampoValor.ListaValor;
            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfValue);
            pnlRow.Controls.Add(hdfText);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "TurismoCamposValores";
                pnlRow.Controls.Add(validador);
            }


            if (pHabilitar)
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControlSelect2|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);

                //select2 start
                script.AppendLine("control.select2({");
                script.AppendFormat("placeholder: '{0}',", pCampo.Titulo);
                script.AppendLine("minimumInputLength: 4,");
                script.AppendLine("theme: 'bootstrap4',");
                script.AppendLine("language: 'es',");
                script.AppendLine("allowClear: true,");
                script.AppendLine("ajax: {");
                script.AppendLine("type: 'POST',");
                script.AppendLine("contentType: 'application/json; charset=utf-8',");
                script.AppendLine("dataType: 'json',");
                script.AppendFormat("url: '{0}',", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica2"));
                script.AppendLine("delay: 250,");
                script.AppendLine("data: function (params) {");
                script.AppendLine("return JSON.stringify( {");
                script.AppendLine("value: control.val(), // search term");
                script.AppendLine("filtro: params.term, // search term");
                script.AppendFormat("sp: '{0}',", pCampo.StoredProcedure);
                script.AppendFormat("idRefTablaValor: '{0}',", 0);// this.MiIdRefTablaValor);
                script.AppendFormat("idRefValor: '{0}'", 0); // MiIdRefValor.HasValue ? MiIdRefValor.Value.ToString() : string.Empty);
                script.AppendLine(" });");
                script.AppendLine("},");
                script.AppendLine("processResults: function (data, params) {");
                script.AppendLine("return {");
                script.AppendLine("results: data.d,");
                script.AppendLine("};");
                script.AppendLine(" cache: true");
                script.AppendLine("},");
                script.AppendLine("}");
                script.AppendLine("});");
                //end select2
                //select2 ON select
                script.AppendLine("control.on('select2:select', function(e) { ");
                script.AppendLine("var newOption = new Option(e.params.data.text, e.params.data.id, false, true);");
                script.AppendFormat("$('select[id$={0}]').append(newOption).trigger('change');", ddlListaOpciones.ID);
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendLine("hdfValue.val(e.params.data.id);");
                script.AppendLine("hdfText.val(e.params.data.text);");
                if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
                {
                    script.AppendLine("ControlSelect2|CTRLID|On(e.params.data.id);");
                }
                script.AppendLine("});");
                //end select2 ON

                //select2 ON unselect
                script.AppendLine("control.on('select2:unselect', function(e) { ");
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendLine("hdfValue.val('');");
                script.AppendLine("hdfText.val('');");
                script.AppendLine("});");
                //end select2 ON unselect

                script.AppendLine("};");
                //end function
                //start scriptEvent
                script.AppendLine("function ControlSelect2|CTRLID|On(e) {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent

                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlSelect2|CTRLID|);");
                script.AppendLine("InitControlSelect2|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = cssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = ccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);
            return panel;
        }
        private PlaceHolder AddLabelRow(TGECampos pCampo, PlaceHolder txtPlaceHolder)
        {
            PlaceHolder placeHolder = new PlaceHolder();
            if (!string.IsNullOrWhiteSpace(pCampo.StoredProcedureLeyenda))
            {
                placeHolder.ID = "panel" + "Leyenda" + pCampo.IdCampo.ToString();

                Panel divContainer = new Panel();
                divContainer.ID = string.Concat("dv", preid, "Leyenda", pCampo.IdCampo.ToString());
                divContainer.CssClass = ccsContainer;
                placeHolder.Controls.Add(divContainer);

                Panel divRowLeyenda = new Panel();
                divRowLeyenda.CssClass = cssRow;

                TextBox Text = (TextBox)BuscarControlRecursivo(txtPlaceHolder, preid + pCampo.IdCampo.ToString());
                Text.AutoPostBack = true;
                //Text.TextChanged += Text_TextChanged;
                pCampo.Filtro = Text.Text;

                Label lblLeyenda = new Label();
                lblLeyenda.ID = preid + "Leyenda" + pCampo.IdCampo.ToString();
                lblLeyenda.CssClass = cssLabelCol6;

                divRowLeyenda.Controls.Add(lblLeyenda);
                if (pCampo.Filtro.Length > 0)
                {
                    TGEListasValoresDetalles valorDetalle = TGEGeneralesF.ListasValoresDetallesObtenerLeyenda(pCampo, this.MiTablaValor);

                    if (!string.IsNullOrEmpty(valorDetalle.Descripcion))
                        lblLeyenda.Text = valorDetalle.Descripcion;
                    else
                        lblLeyenda.Text = string.Empty;
                }
                divContainer.Controls.Add(divRowLeyenda);
            }
            return placeHolder;
        }
        private PlaceHolder AddTextBoxRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder placeHolder = new PlaceHolder();
            placeHolder.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = cssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlInputCtrl = new Panel();
            pnlInputCtrl.CssClass = cssCol;

            TextBox Text = new TextBox();
            Text.CssClass = "form-control";
            Text.ID = preid + pCampo.IdCampo.ToString();
            Text.Text = pCampo.CampoValor.Valor;
            Text.Enabled = pHabilitar;
            Text.MaxLength = pCampo.TamanioMaximo;
            pnlInputCtrl.Controls.Add(Text);

            //if (!string.IsNullOrWhiteSpace(pCampo.StoredProcedure))
            //{
            //    List<TGEListasValoresDetalles> listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(pCampo, this.MiTablaValor);
            //    if (listaValoresDetalles.Count > 0)
            //        Text.Text = listaValoresDetalles[0].Descripcion;
            //}

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = Text.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "TurismoCamposValores";
                pnlInputCtrl.Controls.Add(validador);
            }

            Panel divRow = new Panel();
            divRow.CssClass = cssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlInputCtrl);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = ccsContainer;
            divContainer.Controls.Add(divRow);
            placeHolder.Controls.Add(divContainer);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start scriptEvent
                script.AppendLine("function ControlTextBox|CTRLID|() {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlTextBox|CTRLID|);");
                script.AppendLine("ControlTextBox|CTRLID|();");
                script.AppendLine("});");
                script.Replace("|CTRLID|", Text.ID);
                ScriptManager.RegisterClientScriptBlock(this.pnlPrinc, this.pnlPrinc.GetType(), "Script" + Text.ID, script.ToString(), true);
            }
            return placeHolder;
        }
        private PlaceHolder AddDateTimeRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = cssLabel;
            lblParametro.Text = pCampo.Titulo;
            //panel.Controls.Add(lblParametro);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol;
            TextBox Text = new TextBox();
            Text.CssClass = "form-control datepicker";
            Text.ID = preid + pCampo.IdCampo.ToString();
            Text.Text = pCampo.CampoValor.Valor;
            Text.Enabled = pHabilitar;
            pnlRow.Controls.Add(Text);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = Text.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "TurismoCamposValores";
                pnlRow.Controls.Add(validador);
            }
            Panel divRow = new Panel();
            divRow.CssClass = cssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = ccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start scriptEvent
                script.AppendLine("function ControlTextBox|CTRLID|() {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlTextBox|CTRLID|);");
                script.AppendLine("ControlTextBox|CTRLID|();");
                script.AppendLine("});");
                script.Replace("|CTRLID|", Text.ID);
                ScriptManager.RegisterClientScriptBlock(this.upPaquetes, this.upPaquetes.GetType(), "Script" + Text.ID, script.ToString(), true);
            }
            return panel;
        }
        #endregion
        #region PAGOS
        protected void btnAgregarPago_Click(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "SetMultitabActive", "SetMultitabProfileActive();", true);
            //if (string.IsNullOrEmpty(ddlProveedoresPagos.SelectedValue))
            //    return;

            //if (this.MiTurismoDetalles.Tables[0].Rows.Count == 0)
            //{
            //    this.MostrarMensaje("Los anticipos ya han sido generados", true);
            //    return;
            //}

            //if (txtImporteAnticipo.Decimal > Convert.ToDecimal(this.MiTurismoDetalles.Tables[0].Rows[0]["Importe"]))
            //{
            //    this.MostrarMensaje(string.Format("El importe no puede ser mayor a {0}", this.MiTurismoDetalles.Tables[0].Rows[0]["Importe"]), true);
            //    return;
            //}

            //this.MiTurismoDetalles.Tables[0].Rows[0]["Incluir"] = true;
            //this.MiTurismoDetalles.Tables[0].Rows[0]["Importe"] = txtImporteAnticipo.Decimal;
            //int IdTipoCargoAfiliadoFormaCobro = this.Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"] != null ? Convert.ToInt32(this.Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"]) : 0;// (int)this.MiTurismoDetalles.Tables[0].Rows[0]["IdTipoCargoAfiliadoFormaCobro"];
            //Objeto resultado = new Objeto();
            //resultado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            //if (CuentasPagarF.SolicitudPagoAgregarAnticiposTurismo(resultado, this.MiTurismoDetalles.Tables[0]))
            //{
            //    this.MiTurismoDetalles = new DataSet();
            //    this.MostrarMensaje(resultado.CodigoMensaje, false);
            //    CapOrdenesPagos pOrdenPago = new CapOrdenesPagos();
            //    pOrdenPago.IdRefTipoOperacion = IdTipoCargoAfiliadoFormaCobro;
            //    pOrdenPago.Filtro = typeof(CarTiposCargosAfiliadosFormasCobros).Name;
            //    this.gvPagos.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroGrillaTurismo(pOrdenPago);
            //    this.gvPagos.DataBind();
            //    this.gvPagosValores.DataSource = CuentasPagarF.OrdenesPagosObtenerListaFiltroPagoTurismo(pOrdenPago);
            //    this.gvPagosValores.DataBind();
            //    txtImporteAnticipo.Text = "";
            //    ddlProveedoresPagos.SelectedValue = "";
            //}
            //else
            //{
            //    this.MostrarMensaje(resultado.CodigoMensaje, true);
            //}
            //PaginaAfiliados paginaAfiliados = new PaginaAfiliados();
            //AfiAfiliados afi = paginaAfiliados.Obtener(MiSessionPagina);
            //MisParametrosUrl = new Hashtable();
            //MisParametrosUrl.Add("IdRefEntidad", ddlProveedoresPagos.SelectedValue);
            //MisParametrosUrl.Add("IdEntidad", (int)EnumTGEEntidades.Proveedores);
            //MisParametrosUrl.Add("AnticipoTurismo", "true");
            //this.MisParametrosUrl.Add("IdAfiliado", afi.IdAfiliado);
            //this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", idTipoCargoAfiliadoFormaCobro);
            //Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAgregar.aspx"), true);
        }
        protected void ddlProveedoresPagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "SetMultitabActive", "SetMultitabProfileActive();", true);
        }
        protected void gvPagos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvPagos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdSolicitudPago"].ToString());

            if (IdRefTipoOperacion <= 0)
                IdRefTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdOrdenPago"].ToString());

            this.MisParametrosUrl = new Hashtable();
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
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapOrdenesPagos, "OrdenesPagos", op, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "OrdenesPagos", this.UsuarioActivo);
                }
                else
                {
                    CapSolicitudPago sp = new CapSolicitudPago();
                    sp.IdSolicitudPago = IdRefTipoOperacion;
                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapSolicitudPagos, "SolicitudPagoCompras", sp, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.Page, "SolicitudPagoCompras", this.UsuarioActivo);
                }
            }
            else
            {
                if (e.CommandName == Gestion.Consultar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;
                else if (e.CommandName == Gestion.Anular.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;
                else if (e.CommandName == Gestion.Autorizar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Autorizar;

                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
                this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
                string IdTipoCargoAfiliadoFormaCobro = Request.Form["ctl00$ctl00$ContentPlaceHolder1$cphPrincipal$ModificarDatos$hdfIdTipoCargoAfiliadoFormaCobro"];
                this.MisParametrosUrl.Add("IdTipoCargoAfiliadoFormaCobro", IdTipoCargoAfiliadoFormaCobro);
                //Si devuelve una URL Redirecciona si no muestra mensaje error
                if (filtroMenu.URL.Length != 0)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
                }
                else
                {
                    this.MostrarMensaje("La URL no es valida.", true);
                }
            }
        }
        protected void gvPagos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("OrdenesPagosConsultar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;

                switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                {
                    case (int)EstadosOrdenesPago.Activo:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                        ibtnAutorizar.Visible = this.ValidarPermiso("OrdenesPagosAutorizar.aspx");
                        break;
                    case (int)EstadosOrdenesPago.Autorizado:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                        break;
                    case (int)EstadosOrdenesPago.Pagado:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnularPagada.aspx");
                        ibtnModificar.Visible = this.ValidarPermiso("OrdenesPagosModificar.aspx");
                        break;
                    default:
                        break;
                }
            }
        }
        protected void gvPagosValores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
        #endregion
        #region Reservas de Turismo
        protected void gvReservasTurismo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                   || e.CommandName == Gestion.Modificar.ToString()
                   || e.CommandName == Gestion.Impresion.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idTipoCargoAdministrable = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoCargoAfiliadoFormaCobro"].ToString());
            int idAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdTipoCargoAfiliadoFormaCobro", idTipoCargoAdministrable },
                { "IdAfiliado", idAfiliado}
            };
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
                parametrosFiltros.BusquedaParametros = true;
                parametrosFiltros.HashTransaccion = this.tcDatos.ActiveTabIndex;
                parametrosFiltros.Filtro = this.tcDatos.ActiveTab.ID;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros, "filtroMultitab");

                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cargos/CargosAfiliadosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros parametrosFiltros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
                parametrosFiltros.BusquedaParametros = true;
                parametrosFiltros.HashTransaccion = this.tcDatos.ActiveTabIndex;
                parametrosFiltros.Filtro = this.tcDatos.ActiveTab.ID;
                this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametrosFiltros, "filtroMultitab");
                if (idAfiliado > 0)
                {
                    AfiAfiliados afi = new AfiAfiliados();
                    afi.IdAfiliado = idAfiliado;
                    PaginaAfiliados paginaAfi = new PaginaAfiliados();
                    paginaAfi.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatos(afi));
                }
                string url = "~/Modulos/Cargos/CargosAfiliadosConsultar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                CarTiposCargosAfiliadosFormasCobros cargoTurismo = new CarTiposCargosAfiliadosFormasCobros();
                cargoTurismo.IdTipoCargoAfiliadoFormaCobro = idTipoCargoAdministrable;
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, "CarTiposCargosAfiliadosFormasCobrosReservaTurismo", cargoTurismo, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.upPaquetes, "CarTiposCargosAfiliadosFormasCobrosReservaTurismo", this.UsuarioActivo);
            }
        }
        protected void gvReservasTurismo_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int idEstado = Convert.ToInt32(((HiddenField)e.Row.FindControl("hdfIdEstado")).Value);
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                if (idEstado == (int)EstadosCargos.Facturado || idEstado == (int)EstadosCargos.Activo)
                    ibtnModificar.Visible = true;
            }
        }
        protected void gvReservasTurismo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            this.gvReservasTurismo.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            parametros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametros);
            this.CargarCargosReservasTurismo(parametros);
        }
        private void CargarCargosReservasTurismo(CarTiposCargosAfiliadosFormasCobros parametro)
        {
            parametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            this.gvReservasTurismo.PageSize = parametro.PageSize;
            parametro.IdAfiliado = 0;
            parametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CarTiposCargosAfiliadosFormasCobros>(parametro);
            DataTable MisCargosReservasTurismo = CargosF.TiposCargosAfiliadosObtenerReservasTurismo(parametro);
            this.gvReservasTurismo.DataSource = MisCargosReservasTurismo;
            this.gvReservasTurismo.VirtualItemCount = MisCargosReservasTurismo.Rows.Count > 0 ? Convert.ToInt32(MisCargosReservasTurismo.Rows[0]["Cantidad"]) : 0;
            this.gvReservasTurismo.deTantos.Text = "de " + this.gvReservasTurismo.VirtualItemCount.ToString();
            this.gvReservasTurismo.DataBind();
        }
        private void GvReservas_PageSizeEvent(int pageSize)
        {
            CarTiposCargosAfiliadosFormasCobros parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosAfiliadosFormasCobros>();
            parametros.PageIndex = 0;
            this.gvReservasTurismo.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarCargosReservasTurismo(parametros);
        }
        #endregion
    }
}