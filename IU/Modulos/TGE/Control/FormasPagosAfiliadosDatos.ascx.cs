using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE.Control
{
    public partial class FormasPagosAfiliadosDatos : ControlesSeguros
    {
        private TGEFormasPagosAfiliados MiFormaPagoAfiliado
        {
            get { return (TGEFormasPagosAfiliados)Session[this.MiSessionPagina + "FormasPagosAfiliadosDatosMiFormaPagoAfiliado"]; }
            set { Session[this.MiSessionPagina + "FormasPagosAfiliadosDatosMiFormaPagoAfiliado"] = value; }
        }
        public delegate void FormasPagosAfiliadosAceptarEventHandler(object sender, TGEFormasPagosAfiliados e);
        public event FormasPagosAfiliadosAceptarEventHandler FormasPagosAfiliadosModificarDatosAceptar;
        public delegate void FormasPagosAfiliadosCancelarEventHandler();
        public event FormasPagosAfiliadosCancelarEventHandler FormasPagosAfiliadosModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
        }
        public void IniciarControl(TGEFormasPagosAfiliados pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiFormaPagoAfiliado = pParametro;
            this.CargarCombos();
            // this.ddlFormasPagos.Enabled = false;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.ddlFormasPagos.Enabled = true;
                    TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.EstadoFormasPagosAfiliadoPorDefecto);
                    ListItem item = this.ddlEstados.Items.FindByValue(paramValor.ParametroValor);
                    if (item != null)
                        this.ddlEstados.SelectedValue = item.Value;
                    else
                        this.ddlEstados.SelectedValue = ((int)EstadosFormasCobrosAfiliados.Activo).ToString();

                    this.ddlEstados.Enabled = false;
                    this.ctrComentarios.IniciarControl(pParametro, pGestion);
                    this.ctrArchivos.IniciarControl(pParametro, pGestion);
                    this.ctrAuditoria.IniciarControl(pParametro);
                    break;
                case Gestion.Modificar:
                    this.MiFormaPagoAfiliado = TGEGeneralesF.FormasPagosAfiliadosObtenerDatosCompletos(this.MiFormaPagoAfiliado);
                    this.MapearObjetoAControles(this.MiFormaPagoAfiliado);
                    break;
                case Gestion.Consultar:
                    this.MiFormaPagoAfiliado = TGEGeneralesF.FormasPagosAfiliadosObtenerDatosCompletos(this.MiFormaPagoAfiliado);
                    this.MapearObjetoAControles(this.MiFormaPagoAfiliado);
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlFormasPagos.DataSource = TGEGeneralesF.FormasPagosObtenerListaActiva();
            this.ddlFormasPagos.DataValueField = "IdFormaPago";
            this.ddlFormasPagos.DataTextField = "FormaPago";
            this.ddlFormasPagos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormasPagos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFiliales.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosFormasPagosAfiliados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
        }
        private void MapearControlesAObjeto(TGEFormasPagosAfiliados pFormaPagoAfiliado)
        {
            pFormaPagoAfiliado.FormaPago.IdFormaPago = Convert.ToInt32(this.ddlFormasPagos.SelectedValue);
            pFormaPagoAfiliado.FormaPago.FormaPago = this.ddlFormasPagos.SelectedItem.Text;
            pFormaPagoAfiliado.Filial.IdFilial = Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pFormaPagoAfiliado.Filial.Filial = this.ddlFiliales.SelectedItem.Text;
            //pFormaPagoAfiliado.Predeterminado = this.chkPredeterminado.Checked;
            //pFormaPagoAfiliado.Estado.IdEstado = (int)Estados.Activo;
            pFormaPagoAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pFormaPagoAfiliado.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pFormaPagoAfiliado.Campos = this.ctrCamposValores.ObtenerLista();
            pFormaPagoAfiliado.Comentarios = this.ctrComentarios.ObtenerLista();
            pFormaPagoAfiliado.Archivos = this.ctrArchivos.ObtenerLista();
        }
        private void MapearObjetoAControles(TGEFormasPagosAfiliados pFormaPagoAfiliado)
        {
            ListItem item = this.ddlFormasPagos.Items.FindByValue(pFormaPagoAfiliado.FormaPago.IdFormaPago.ToString());
            if (item == null)
                this.ddlFormasPagos.Items.Add(new ListItem(pFormaPagoAfiliado.FormaPago.IdFormaPago.ToString(), pFormaPagoAfiliado.FormaPago.FormaPago));
            this.ddlFormasPagos.SelectedValue = pFormaPagoAfiliado.FormaPago.IdFormaPago.ToString();
            this.ddlFiliales.SelectedValue = pFormaPagoAfiliado.Filial.IdFilial.ToString();
            this.txtZG.Text = pFormaPagoAfiliado.ZG;
            //this.chkPredeterminado.Checked = pFormaPagoAfiliado.Predeterminado;
            this.ddlEstados.SelectedValue = pFormaPagoAfiliado.Estado.IdEstado.ToString();
            this.ctrCamposValores.IniciarControl(pFormaPagoAfiliado, pFormaPagoAfiliado.FormaPago, pFormaPagoAfiliado.IdAfiliado, this.GestionControl);
            this.ctrComentarios.IniciarControl(pFormaPagoAfiliado, this.GestionControl);
            this.ctrArchivos.IniciarControl(pFormaPagoAfiliado, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pFormaPagoAfiliado);
        }
        protected void ddlFormasPagos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFormasPagos.SelectedValue))
                this.MiFormaPagoAfiliado.FormaPago.IdFormaPago = Convert.ToInt32(this.ddlFormasPagos.SelectedValue);
            else
                this.MiFormaPagoAfiliado.FormaPago.IdFormaPago = 0;

            this.ctrCamposValores.IniciarControl(this.MiFormaPagoAfiliado, this.MiFormaPagoAfiliado.FormaPago, this.MiFormaPagoAfiliado.IdAfiliado, this.GestionControl);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.Page.Validate("Aceptar");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiFormaPagoAfiliado);
            this.MiFormaPagoAfiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, this.MiFormaPagoAfiliado.UsuarioAlta);
                    this.MiFormaPagoAfiliado.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.FormasPagosAfiliadosAgregar(this.MiFormaPagoAfiliado);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.FormasPagosAfiliadosModificar(this.MiFormaPagoAfiliado);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFormaPagoAfiliado.CodigoMensaje));
            }
            else
            {
                if (!this.MiFormaPagoAfiliado.ErrorAccesoDatos && this.MiFormaPagoAfiliado.ConfirmarAccion)
                {
                    this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFormaPagoAfiliado.MostrarValidarCodigoMensaje()), true);
                }
                else
                    this.MostrarMensaje(this.MiFormaPagoAfiliado.CodigoMensaje, true, this.MiFormaPagoAfiliado.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.FormasPagosAfiliadosModificarDatosCancelar != null)
                this.FormasPagosAfiliadosModificarDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.MiFormaPagoAfiliado.ConfirmarAccion)
            {
                this.MiFormaPagoAfiliado.ConfirmarMensaje();
                this.btnAceptar_Click(null, EventArgs.Empty);
            }
            else
            {
                if (this.FormasPagosAfiliadosModificarDatosAceptar != null)
                    this.FormasPagosAfiliadosModificarDatosAceptar(null, this.MiFormaPagoAfiliado);
            }
        }
    }
}