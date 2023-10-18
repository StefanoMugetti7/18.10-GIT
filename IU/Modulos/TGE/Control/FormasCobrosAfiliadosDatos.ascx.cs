using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE.Control
{
    public partial class FormasCobrosAfiliadosDatos : ControlesSeguros
    {
        private TGEFormasCobrosAfiliados MiFormaCobroAfiliado
        {
            get { return (TGEFormasCobrosAfiliados)Session[this.MiSessionPagina + "FormasCobrosAfiliadosDatosMiFormaCobroAfiliado"]; }
            set { Session[this.MiSessionPagina + "FormasCobrosAfiliadosDatosMiFormaCobroAfiliado"] = value; }
        }
        public delegate void FormasCobrosAfiliadosAceptarEventHandler(object sender, TGEFormasCobrosAfiliados e);
        public event FormasCobrosAfiliadosAceptarEventHandler FormasCobrosAfiliadosModificarDatosAceptar;
        public delegate void FormasCobrosAfiliadosCancelarEventHandler();
        public event FormasCobrosAfiliadosCancelarEventHandler FormasCobrosAfiliadosModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
        }
        public void IniciarControl(TGEFormasCobrosAfiliados pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiFormaCobroAfiliado = pParametro;
            this.CargarCombos();
            this.ddlFormasCobros.Enabled = false;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ddlFormasCobros.Enabled = true;
                    TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.EstadoFormasCobrosAfiliadoPorDefecto);
                    ListItem item = this.ddlEstados.Items.FindByValue(paramValor.ParametroValor);
                    if (item != null)
                        this.ddlEstados.SelectedValue = item.Value;
                    else
                        this.ddlEstados.SelectedValue = ((int)EstadosFormasCobrosAfiliados.Activo).ToString();
                    this.ctrCamposValores.IniciarControl(this.MiFormaCobroAfiliado, new Objeto(), this.MiFormaCobroAfiliado.IdAfiliado, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiFormaCobroAfiliado = TGEGeneralesF.FormasCobrosAfiliadosObtenerDatosCompletos(this.MiFormaCobroAfiliado);
                    this.MapearObjetoAControles(this.MiFormaCobroAfiliado);
                    break;
                case Gestion.Consultar:
                    this.MiFormaCobroAfiliado = TGEGeneralesF.FormasCobrosAfiliadosObtenerDatosCompletos(this.MiFormaCobroAfiliado);
                    this.MapearObjetoAControles(this.MiFormaCobroAfiliado);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosFormasCobrosAfiliados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            this.ddlFormasCobros.DataSource = TGEGeneralesF.FormasCobrosObtenerPendientes(this.MiFormaCobroAfiliado);
            this.ddlFormasCobros.DataValueField = "IdFormaCobro";
            this.ddlFormasCobros.DataTextField = "FormaCobro";
            this.ddlFormasCobros.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormasCobros, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearControlesAObjeto(TGEFormasCobrosAfiliados pFormaCobroAfiliado)
        {
            pFormaCobroAfiliado.FormaCobro.IdFormaCobro = Convert.ToInt32(this.ddlFormasCobros.SelectedValue);
            pFormaCobroAfiliado.FormaCobro.FormaCobro = this.ddlFormasCobros.SelectedItem.Text;
            pFormaCobroAfiliado.Predeterminado = this.chkPredeterminado.Checked;
            pFormaCobroAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pFormaCobroAfiliado.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pFormaCobroAfiliado.Campos = this.ctrCamposValores.ObtenerLista();
        }
        private void MapearObjetoAControles(TGEFormasCobrosAfiliados pFormaCobroAfiliado)
        {
            ListItem item = this.ddlFormasCobros.Items.FindByValue(pFormaCobroAfiliado.FormaCobro.IdFormaCobro.ToString());
            if (item == null)
                this.ddlFormasCobros.Items.Add(new ListItem(pFormaCobroAfiliado.FormaCobro.FormaCobro,
                    pFormaCobroAfiliado.FormaCobro.IdFormaCobro.ToString()));
            this.ddlFormasCobros.SelectedValue = pFormaCobroAfiliado.FormaCobro.IdFormaCobro.ToString();
            this.chkPredeterminado.Checked = pFormaCobroAfiliado.Predeterminado;
            this.ddlEstados.SelectedValue = pFormaCobroAfiliado.Estado.IdEstado.ToString();
            this.ctrCamposValores.IniciarControl(this.MiFormaCobroAfiliado, new Objeto(), this.MiFormaCobroAfiliado.IdAfiliado, this.GestionControl);
            this.ctrCamposValores.IniciarControl(pFormaCobroAfiliado, pFormaCobroAfiliado.FormaCobro, pFormaCobroAfiliado.IdAfiliado, this.GestionControl);
            this.pnlCamposValores.Visible = this.ctrCamposValores.MostrarControl;
        }
        protected void ddlFormasCobros_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFormasCobros.SelectedValue))
            {
                this.MiFormaCobroAfiliado.FormaCobro.IdFormaCobro = Convert.ToInt32(this.ddlFormasCobros.SelectedValue);
                this.ctrCamposValores.BorrarControlesParametros(this.MiFormaCobroAfiliado.FormaCobro);
            }
            else
                this.MiFormaCobroAfiliado.FormaCobro.IdFormaCobro = 0;

            this.ctrCamposValores.IniciarControl(this.MiFormaCobroAfiliado, new Objeto(), this.MiFormaCobroAfiliado.IdAfiliado, this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiFormaCobroAfiliado, this.MiFormaCobroAfiliado.FormaCobro, this.MiFormaCobroAfiliado.IdAfiliado, this.GestionControl);
            this.pnlCamposValores.Visible = this.ctrCamposValores.MostrarControl;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiFormaCobroAfiliado);
            this.MiFormaCobroAfiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, this.MiFormaCobroAfiliado.UsuarioAlta);
                    this.MiFormaCobroAfiliado.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.FormasCobrosAfiliadosAgregar(this.MiFormaCobroAfiliado);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.FormasCobrosAfiliadosModificar(this.MiFormaCobroAfiliado);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiFormaCobroAfiliado.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiFormaCobroAfiliado.CodigoMensaje, true, this.MiFormaCobroAfiliado.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.FormasCobrosAfiliadosModificarDatosCancelar?.Invoke();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            this.FormasCobrosAfiliadosModificarDatosAceptar?.Invoke(null, this.MiFormaCobroAfiliado);
        }
    }
}