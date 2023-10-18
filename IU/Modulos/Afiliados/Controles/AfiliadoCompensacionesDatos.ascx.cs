using Afiliados;
using Afiliados.Entidades;
using Ahorros;
using Ahorros.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfiliadoCompensaciones : ControlesSeguros
    {
        private AfiCompensaciones MiCompensacion
        {
            get { return (AfiCompensaciones)Session[this.MiSessionPagina + "CompensacionesModificarDatosMiCompensacion"]; }
            set { Session[this.MiSessionPagina + "CompensacionesModificarDatosMiCompensacion"] = value; }
        }

        public delegate void AfiliadoCompensacionesDatosAceptarEventHandler(object sender, AfiCompensaciones e);
        public event AfiliadoCompensacionesDatosAceptarEventHandler CompensacionesModificarDatosAceptar;
        public delegate void AfiliadoCompensacionesDatosCancelarEventHandler();
        public event AfiliadoCompensacionesDatosCancelarEventHandler CompensacionesModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            //    this.ctrAfiliados.AfiliadosBuscarSeleccionar += new AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            base.PageLoadEvent(sender, e);
            this.txtCompensacion.Attributes.Add("onchange", "CalcularItem();");
            this.txtFondoRepresentacion.Attributes.Add("onchange", "CalcularItem();");
            this.txtGastosRepresentacion.Attributes.Add("onchange", "CalcularItem();");
            if (!this.IsPostBack)
            {
                if (this.MiCompensacion == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlSesion.aspx"), true);
                }
            }
        }
        public void IniciarControl(AfiCompensaciones pCompensacion, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiCompensacion = pCompensacion;
            this.CargarEstados();

            if (pGestion != Gestion.Agregar)
            {
                this.MiCompensacion = AfiliadosF.CompensacionesObtenerDatosCompletos(this.MiCompensacion);
                this.CargarCuentas();
            }

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrComentarios.IniciarControl(this.MiCompensacion, this.GestionControl);
                    this.ctrArchivos.IniciarControl(this.MiCompensacion, this.GestionControl);
                    //para no dejar vacio el ddl
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlCuenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    break;

                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiCompensacion);
                    break;
                case Gestion.Anular:
                    this.MapearObjetoAControles(this.MiCompensacion);
                    this.txtCompensacion.Enabled = false;
                    this.txtFondoRepresentacion.Enabled = false;
                    this.txtGastosRepresentacion.Enabled = false;
                    this.txtTotalAAcreditar.Enabled = false;
                    //    this.txtNumeroSocio.ReadOnly = true;
                    //    this.txtRazonSocial.ReadOnly = true;
                    //this.txtTotalAAcreditar.ReadOnly = true;
                    //  this.btnBuscar.Enabled = false;
                    this.ddlCuenta.Enabled = false;
                    break;

                case Gestion.Consultar:
                    this.MapearObjetoAControles(this.MiCompensacion);
                    this.btnAceptar.Enabled = false;
                    this.txtCompensacion.Enabled = false;
                    this.txtFondoRepresentacion.Enabled = false;
                    this.txtGastosRepresentacion.Enabled = false;
                    this.txtTotalAAcreditar.Enabled = false;
                    //  this.txtNumeroSocio.ReadOnly = true;
                    //  this.txtRazonSocial.ReadOnly = true;
                    //this.txtTotalAAcreditar.ReadOnly = true;
                    //    this.btnBuscar.Enabled = false;
                    this.ddlCuenta.Enabled = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarEstados()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.CompensacionesModificarDatosAceptar != null)
                this.CompensacionesModificarDatosAceptar(null, this.MiCompensacion);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {

            if (this.CompensacionesModificarDatosCancelar != null)
                this.CompensacionesModificarDatosCancelar();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto();
            this.MiCompensacion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:
                    guardo = AfiliadosF.CompensacionesAgregar(this.MiCompensacion);
                    break;
                case Gestion.Modificar:
                    guardo = AfiliadosF.CompensacionesModificar(this.MiCompensacion);
                    break;

                case Gestion.Anular:
                    guardo = AfiliadosF.CompensacionesAnular(this.MiCompensacion);
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCompensacion.CodigoMensaje));
            }
            else
            {
                if (this.MiCompensacion.ErrorException.Contains("UNIQUE"))
                {
                    this.MiCompensacion.CodigoMensaje = "ValidarNumeroSocio";
                    this.MostrarMensaje(this.MiCompensacion.CodigoMensaje, true);
                }
                else
                    this.MostrarMensaje(this.MiCompensacion.CodigoMensaje, true, this.MiCompensacion.CodigoMensajeArgs);
            }
        }
        void MapearControlesAObjeto()
        {
            this.MiCompensacion.FondoRepresentacion = this.txtFondoRepresentacion.Text == string.Empty ? 0 : this.txtFondoRepresentacion.Decimal;
            this.MiCompensacion.GastosRepresentacion = this.txtGastosRepresentacion.Text == string.Empty ? 0 : this.txtGastosRepresentacion.Decimal;
            this.MiCompensacion.Compensacion = this.txtCompensacion.Text == string.Empty ? 0 : this.txtCompensacion.Decimal;
            //CALCULANDO TOTAL
            this.MiCompensacion.TotalAAcreditar = this.MiCompensacion.Compensacion + this.MiCompensacion.GastosRepresentacion - this.MiCompensacion.FondoRepresentacion;
            //CUENTA
            this.MiCompensacion.IdCuenta = Convert.ToInt32(this.ddlCuenta.SelectedValue);
            this.MiCompensacion.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);

            this.MiCompensacion.Comentarios = ctrComentarios.ObtenerLista();
            this.MiCompensacion.Archivos = ctrArchivos.ObtenerLista();
        }
        void MapearObjetoAControles(AfiCompensaciones pCompensacion)
        {
            this.txtFondoRepresentacion.Text = pCompensacion.FondoRepresentacion.ToString();
            this.txtGastosRepresentacion.Text = pCompensacion.GastosRepresentacion.ToString();
            this.txtCompensacion.Text = pCompensacion.Compensacion.ToString();
            this.txtTotalAAcreditar.Text = pCompensacion.TotalAAcreditar.ToString();
            //Datos Afiliado
            // this.txtNumeroSocio.Text = pCompensacion.NumeroSocio;
            //  this.txtRazonSocial.Text = pCompensacion.ApellidoNombre;

            this.ddlCuenta.SelectedValue = pCompensacion.IdCuenta.ToString();
            this.ddlEstado.SelectedValue = pCompensacion.Estado.IdEstado.ToString();

            this.ctrComentarios.IniciarControl(pCompensacion, this.GestionControl);
            this.ctrArchivos.IniciarControl(pCompensacion, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pCompensacion);
        }
        protected void button_Click(object sender, EventArgs e)
        {
            this.MiCompensacion.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            this.MiCompensacion.ApellidoNombre = this.hdfRazonSocial.Value;
            this.ddlNumeroSocio.Items.Add(new ListItem(this.MiCompensacion.ApellidoNombre, this.MiCompensacion.IdAfiliado.ToString()));
            this.ddlNumeroSocio.SelectedValue = this.MiCompensacion.IdAfiliado.ToString();
            this.CargarCuentas();
        }
        private void CargarCuentas()
        {
            AhoCuentas cuentaFiltro = new AhoCuentas();
            cuentaFiltro.Afiliado.IdAfiliado = this.MiCompensacion.IdAfiliado;
            cuentaFiltro.Estado.IdEstado = (int)EstadosAhorrosCuentas.CuentaAbierta;
            cuentaFiltro.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;
            cuentaFiltro.CuentaTipo.IdCuentaTipo = (int)EnumAhorrosCuentasTipos.CajaAhorro;
            this.ddlCuenta.DataSource = AhorroF.CuentasObtenerListaFiltro(cuentaFiltro);
            this.ddlCuenta.DataValueField = "IdCuenta";
            this.ddlCuenta.DataTextField = "NumeroCuenta";
            this.ddlCuenta.DataBind();

            if (this.ddlCuenta.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCuenta, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.upAfiliado.Update();
        }
    }
}