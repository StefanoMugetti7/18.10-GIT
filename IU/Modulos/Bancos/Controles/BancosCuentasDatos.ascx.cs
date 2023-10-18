using Bancos;
using Bancos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Web.UI.WebControls;

namespace IU.Modulos.Tesoreria.Controles
{
    public partial class BancosCuentasDatos : ControlesSeguros
    {
        private TESBancosCuentas MiBancoCuenta
        {
            get { return (TESBancosCuentas)Session[this.MiSessionPagina + "BancosCuentasDatosMiBancoCuenta"]; }
            set { Session[this.MiSessionPagina + "BancosCuentasDatosMiBancoCuenta"] = value; }
        }

        public delegate void BancosCuentasDatosAceptarEventHandler(object sender, TESBancosCuentas e);
        public event BancosCuentasDatosAceptarEventHandler BancoCuentaModificarDatosAceptar;

        public delegate void BancosCuentasDatosCancelarEventHandler();
        public event BancosCuentasDatosCancelarEventHandler BancoCuentaModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                //if (this.MiBancoCuenta == null && this.GestionControl != Gestion.Agregar)
                //{
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                //}
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Cuenta Bancaria
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TESBancosCuentas pBancoCuenta, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiBancoCuenta = pBancoCuenta;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    pBancoCuenta.FechaAlta = DateTime.Now;
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, pBancoCuenta.UsuarioAlta);
                    this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
                    this.txtUsuarioAlta.Text = this.UsuarioActivo.ApellidoNombre;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ctrComentarios.IniciarControl(this.MiBancoCuenta, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiBancoCuenta, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiBancoCuenta, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiBancoCuenta = BancosF.BancosCuentasObtenerDatosCompletos(pBancoCuenta);
                    this.MapearObjetoAControles(this.MiBancoCuenta);
                    break;
                case Gestion.Consultar:
                    this.MiBancoCuenta = BancosF.BancosCuentasObtenerDatosCompletos(pBancoCuenta);
                    this.MapearObjetoAControles(this.MiBancoCuenta);
                    this.txtCbu.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(TESBancosCuentas pBancoCuenta)
        {
            this.txtDenominacion.Text = pBancoCuenta.Denominacion;
            this.txtFechaAlta.Text = pBancoCuenta.FechaAlta.ToShortDateString();
            this.txtImporteDescubierto.Text = pBancoCuenta.ImporteDescubierto.ToString();
            this.txtNumeroBancoSucursal.Text = pBancoCuenta.NumeroBancoSucursal;
            this.txtNumeroCuenta.Text = pBancoCuenta.NumeroCuenta;
            this.txtUsuarioAlta.Text = pBancoCuenta.UsuarioAlta.ApellidoNombre;
            this.txtCbu.Text = pBancoCuenta.Cbu;

            this.hdfIdCuentaContable.Value = pBancoCuenta.IdCuentaContable.ToString();

            this.ddlCuentaContable.Items.Clear();
            this.ddlCuentaContable.Items.Add(new ListItem(pBancoCuenta.CuentaContable, pBancoCuenta.CuentaContable));
            this.ddlCuentaContable.SelectedValue = pBancoCuenta.CuentaContable;

            ListItem item = this.ddlBancos.Items.FindByValue(pBancoCuenta.Banco.IdBanco.ToString());
            if (item == null)
                this.ddlBancos.Items.Add(new ListItem(pBancoCuenta.Banco.Descripcion, pBancoCuenta.Banco.IdBanco.ToString()));
            this.ddlBancos.SelectedValue = pBancoCuenta.Banco.IdBanco.ToString();

            item = this.ddlBancosCuentasTipos.Items.FindByValue(pBancoCuenta.BancoCuentaTipo.IdBancoCuentaTipo.ToString());
            if (item == null)
                this.ddlBancosCuentasTipos.Items.Add(new ListItem(pBancoCuenta.BancoCuentaTipo.Descripcion, pBancoCuenta.BancoCuentaTipo.IdBancoCuentaTipo.ToString()));
            this.ddlBancosCuentasTipos.SelectedValue = pBancoCuenta.BancoCuentaTipo.IdBancoCuentaTipo.ToString();

            item = this.ddlFiliales.Items.FindByValue(pBancoCuenta.Filial.IdFilial.ToString());
            if (item == null)
                this.ddlFiliales.Items.Add(new ListItem(pBancoCuenta.Filial.Filial, pBancoCuenta.Filial.IdFilial.ToString()));
            this.ddlFiliales.SelectedValue = pBancoCuenta.Filial.IdFilial.ToString();

            this.ddlEstado.SelectedValue = pBancoCuenta.Estado.IdEstado.ToString();
            this.ddlMonedas.SelectedValue = pBancoCuenta.Moneda.IdMoneda.ToString();

            this.ctrComentarios.IniciarControl(pBancoCuenta, this.GestionControl);
            this.ctrArchivos.IniciarControl(pBancoCuenta, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pBancoCuenta);
            this.ctrCamposValores.IniciarControl(this.MiBancoCuenta, new Objeto(), this.GestionControl);
        }
        private void MapearControlesAObjeto(TESBancosCuentas pBancoCuenta)
        {
            pBancoCuenta.Denominacion = this.txtDenominacion.Text;
            pBancoCuenta.ImporteDescubierto = this.txtImporteDescubierto.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtImporteDescubierto.Text);
            pBancoCuenta.NumeroBancoSucursal = this.txtNumeroBancoSucursal.Text;
            pBancoCuenta.NumeroCuenta = this.txtNumeroCuenta.Text;
            pBancoCuenta.Banco.IdBanco = Convert.ToInt32(this.ddlBancos.SelectedValue);
            pBancoCuenta.BancoCuentaTipo.IdBancoCuentaTipo = Convert.ToInt32(this.ddlBancosCuentasTipos.SelectedValue);
            pBancoCuenta.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pBancoCuenta.Moneda.IdMoneda = Convert.ToInt32(this.ddlMonedas.SelectedValue);
            pBancoCuenta.Filial.IdFilial = Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pBancoCuenta.Comentarios = ctrComentarios.ObtenerLista();
            pBancoCuenta.Archivos = ctrArchivos.ObtenerLista();
            pBancoCuenta.Cbu = this.txtCbu.Text;
            pBancoCuenta.IdCuentaContable = this.hdfIdCuentaContable.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdCuentaContable.Value);

            pBancoCuenta.Comentarios = this.ctrComentarios.ObtenerLista();
            pBancoCuenta.Archivos = this.ctrArchivos.ObtenerLista();
            pBancoCuenta.Campos = this.ctrCamposValores.ObtenerLista();
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlMonedas.DataSource = TGEGeneralesF.MonedasObtenerLista();
            this.ddlMonedas.DataValueField = "IdMoneda";
            this.ddlMonedas.DataTextField = "miMonedaDescripcion";
            this.ddlMonedas.DataBind();
            if (this.ddlMonedas.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMonedas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlBancos.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.Bancos);
            this.ddlBancos.DataValueField = "IdListaValorDetalle";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();
            if (this.ddlBancos.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlBancosCuentasTipos.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.BancosCuentasTipos);
            this.ddlBancosCuentasTipos.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlBancosCuentasTipos.DataTextField = "Descripcion";
            this.ddlBancosCuentasTipos.DataBind();
            if (this.ddlBancosCuentasTipos.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlBancosCuentasTipos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiBancoCuenta);
            this.MiBancoCuenta.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiBancoCuenta.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = BancosF.BancosCuentasAgregar(this.MiBancoCuenta);
                    break;
                case Gestion.Modificar:
                    guardo = BancosF.BancosCuentasModificar(this.MiBancoCuenta);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiBancoCuenta.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiBancoCuenta.CodigoMensaje, true, this.MiBancoCuenta.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.BancoCuentaModificarDatosCancelar != null)
                this.BancoCuentaModificarDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.BancoCuentaModificarDatosAceptar != null)
                this.BancoCuentaModificarDatosAceptar(null, this.MiBancoCuenta);
        }
    }
}