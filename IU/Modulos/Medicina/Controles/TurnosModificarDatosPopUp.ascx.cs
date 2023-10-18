using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Medicina.Entidades;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Web.Services.Description;
using System.Windows;


namespace IU.Modulos.Medicina.Controles
{
    public partial class TurnosModificarDatosPopUp : ControlesSeguros
    {
        private MedTurnos MiTurno
        {
            get { return (MedTurnos)Session[this.MiSessionPagina + "TurnosModificarDatosPopUpMiTurno"]; }
            set { Session[this.MiSessionPagina + "TurnosModificarDatosPopUpMiTurno"] = value; }
        }
        public delegate void ModificarDatosAceptarEventHandler(MedTurnos e, Gestion pGestion);
        public event ModificarDatosAceptarEventHandler ModificarDatosAceptar;
        //public delegate void ModificarDatosCancelarEventHandler();
        //public event ModificarDatosAceptarEventHandler ModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrAfiliados.AfiliadosBuscarSeleccionar += new Afiliados.Controles.AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            if (!this.IsPostBack)
                this.CargarCombos();
        }
        void ctrAfiliados_AfiliadosBuscarSeleccionar(global::Afiliados.Entidades.AfiAfiliados e)
        {
            this.MiTurno.Afiliado = e;
            this.MapearObjetoAControles(this.MiTurno);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarTurnos();", true);
        }
        public void IniciarControl(MedTurnos pParametro, Gestion pGestion)
        {
            this.ddlEspecializacion.Enabled = true;
            this.MiTurno = pParametro;
            this.CargarCombos();
            this.GestionControl = pGestion;
            if (pParametro.FechaHoraDesde.HasValue && DateTime.Now.Date > pParametro.FechaHoraDesde.Value)
                this.GestionControl = Gestion.Consultar;
            else if (MiTurno.Estado.IdEstado == (int)EstadosTurnos.Atendido || MiTurno.Estado.IdEstado == (int)EstadosTurnos.Cancelado)
                this.GestionControl = Gestion.Consultar;
            this.ddlEstados.Enabled = false;
            this.txtObservaciones.Enabled = false;
            this.ddlObraSocial.Enabled = false;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    pParametro.Estado.IdEstado = (int)EstadosTurnos.Disponible;
                    this.MapearObjetoAControles(this.MiTurno);
                    this.ddlApellido.Enabled = true;
                    this.txtObservaciones.Enabled = true;
                    this.ddlObraSocial.Enabled = true;
                    this.btnAceptar.Visible = true;
                    this.btnAtender.Visible = false;
                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiTurno);
                    this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosTurnos.Disponible).ToString()));
                    this.ddlEstados.Items.Remove(this.ddlEstados.Items.FindByValue(((int)EstadosTurnos.Atendido).ToString()));
                    this.ddlEstados.Enabled = true;
                    this.txtObservaciones.Enabled = true;
                    this.ddlObraSocial.Enabled = true;
                    this.btnAceptar.Visible = true;
                    this.ddlApellido.Enabled = false;
                    this.btnAtender.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MapearObjetoAControles(this.MiTurno);
                    this.btnAceptar.Visible = false;
                    this.btnAtender.Visible = false;
                    this.ddlApellido.Enabled = false;
                    this.ddlEspecializacion.Enabled = false;
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarTurnos();", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarTurnos();", true);
            this.ctrAfiliados.IniciarControl(true);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("DiasHorasModificarDatos");
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiTurno);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiTurno.EstadoColeccion = EstadoColecciones.Agregado;
                    this.MiTurno.Estado.IdEstado = (int)EstadosTurnos.Reservado;
                    break;
                case Gestion.Modificar:
                    this.MiTurno.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiTurno, this.GestionControl);
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarTurnos();", true);
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar(this.MiTurno, this.GestionControl);
        }
        private void MapearControlesAObjeto(MedTurnos pParametro)
        {
            pParametro.Afiliado.ApellidoNombre = this.ddlApellido.SelectedValue;
            pParametro.Observaciones = this.txtObservaciones.Text;
            pParametro.Afiliado.IdAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pParametro.Especializacion.IdEspecializacion = ddlEspecializacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlEspecializacion.SelectedValue);
            pParametro.ObraSocial.IdObraSocial = this.ddlObraSocial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlObraSocial.SelectedValue);
            pParametro.Observaciones = this.txtObservaciones.Text;
        }
        private void MapearObjetoAControles(MedTurnos pParametro)
        {
            hdfIdAfiliado.Value = "";
            ddlApellido.Items.Clear();
            if (pParametro.Afiliado.IdAfiliado > 0)
            {
                this.ddlApellido.Items.Add(new ListItem(pParametro.Afiliado.ApellidoNombre, pParametro.Afiliado.IdAfiliado.ToString()));
                this.ddlApellido.SelectedValue = pParametro.Afiliado.IdAfiliado.ToString();
                this.hdfIdAfiliado.Value = pParametro.Afiliado.IdAfiliado.ToString();
            }
            this.txtObservaciones.Text = pParametro.Observaciones.ToString();
            if (pParametro.FechaHoraDesde.HasValue)
                this.txtFecha.Text = pParametro.FechaHoraDesde.Value.ToString("dd/MM/yyyy hh:mm");
            this.txtTipoDocumento.Text = pParametro.Afiliado.TipoDocumento.TipoDocumento;
            this.txtNumeroDocumento.Text = pParametro.Afiliado.NumeroDocumento.ToString();
            this.txtPrestador.Text = pParametro.Prestador.ApellidoNombre;
            ListItem itemEspecializacion = this.ddlEspecializacion.Items.FindByValue(pParametro.Especializacion.IdEspecializacion.ToString());
            if (itemEspecializacion == null)
                this.ddlEspecializacion.Items.Add(new ListItem(pParametro.Especializacion.Descripcion, pParametro.Especializacion.IdEspecializacion.ToString()));
            if (pParametro.Especializacion.IdEspecializacion != 0)
                ddlEspecializacion.SelectedValue = pParametro.Especializacion.IdEspecializacion.ToString();
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
            ListItem item = this.ddlObraSocial.Items.FindByValue(pParametro.ObraSocial.IdObraSocial.ToString());
            if (item == null)
                this.ddlObraSocial.Items.Add(new ListItem(pParametro.ObraSocial.Descripcion, pParametro.ObraSocial.IdObraSocial.ToString()));
            this.ddlObraSocial.SelectedValue = pParametro.ObraSocial.IdObraSocial == 0 ? string.Empty : pParametro.ObraSocial.IdObraSocial.ToString();
            this.UpdatePanel1.Update();
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosAceptar != null)
                this.ModificarDatosAceptar(this.MiTurno, this.GestionControl);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalBuscarTurnos();", true);
            this.ctrAfiliados.IniciarControl(true);
        }
        protected void btnAtender_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new System.Collections.Hashtable();
            MisParametrosUrl.Add("Turno", MiTurno);
            MisParametrosUrl.Add("IdAfiliado", MiTurno.Afiliado.IdAfiliado);
            MisParametrosUrl.Add("Atender", true);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesConsultar.aspx"), true);
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosTurnos));
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)EstadosTurnos.Disponible).ToString();
            this.ddlObraSocial.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.ObrasSociales);
            this.ddlObraSocial.DataValueField = "IdListaValorDetalle";
            this.ddlObraSocial.DataTextField = "Descripcion";
            this.ddlObraSocial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlObraSocial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            if (MiTurno != null)
            {
                ddlEspecializacion.DataSource = this.MiTurno.Prestador.PrestadoresDiasHoras.Find(x => x.IdPrestadorDiaHora == MiTurno.HashTransaccion).Especializaciones;
                ddlEspecializacion.DataValueField = "IdEspecializacion";
                ddlEspecializacion.DataTextField = "Descripcion";
                ddlEspecializacion.DataBind();
                if (this.MiTurno.Prestador.PrestadoresDiasHoras.Find(x => x.IdPrestadorDiaHora == MiTurno.HashTransaccion).Especializaciones.Count < 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlEspecializacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                else
                    ddlEspecializacion.Enabled = false;
            }
            //this.ddlObraSocial.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.ObrasSociales);
            //this.ddlObraSocial.DataValueField = "IdListaValorDetalle";
            //this.ddlObraSocial.DataTextField = "Descripcion";
            //this.ddlObraSocial.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlObraSocial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
    }
}