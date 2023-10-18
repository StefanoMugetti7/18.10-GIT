using Comunes.Entidades;
using Contabilidad;
using Contabilidad.Entidades;
using Generales.FachadaNegocio;
using System;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class DepartamentosDatos : ControlesSeguros
    {
        private CtbDepartamentos MiDepartamento
        {
            get { return (CtbDepartamentos)Session[this.MiSessionPagina + "MiDepartamento"]; }
            set { Session[this.MiSessionPagina + "MiDepartamento"] = value; }
        }
        public delegate void DepartamentoDatosAceptarEventHandler(object sender, CtbDepartamentos e);
        public event DepartamentoDatosAceptarEventHandler DepartamentoDatosAceptar;
        public delegate void DepartamentoDatosCancelarEventHandler();
        public event DepartamentoDatosCancelarEventHandler DepartamentoDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(this.popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiDepartamento == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }
        /// <summary>
        /// Inicializa el control de Alta y Modificación de un Departamento
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbDepartamentos pDepartamento, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiDepartamento = pDepartamento;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    break;
                case Gestion.Modificar:
                    this.MiDepartamento = ContabilidadF.DepartamentosObtenerDatosCompletos(pDepartamento);
                    this.MapearObjetoAControles(this.MiDepartamento);
                    break;
                case Gestion.Consultar:
                    this.MiDepartamento = ContabilidadF.DepartamentosObtenerDatosCompletos(pDepartamento);
                    this.MapearObjetoAControles(this.MiDepartamento);
                    this.txtDepartamento.Enabled = false;
                    this.txtCodigoDepartamento.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
        }
        private void MapearObjetoAControles(CtbDepartamentos pDepartamento)
        {
            this.txtDepartamento.Text = pDepartamento.Departamento;
            this.txtCodigoDepartamento.Text = pDepartamento.CodigoDepartamento;
            this.ddlEstado.SelectedValue = pDepartamento.Estado.IdEstado.ToString();
        }
        private void MapearControlesAObjeto(CtbDepartamentos pDepartamento)
        {
            pDepartamento.Departamento = this.txtDepartamento.Text;
            pDepartamento.CodigoDepartamento = this.txtCodigoDepartamento.Text;
            pDepartamento.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiDepartamento);
            this.MiDepartamento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.DepartamentosAgregar(this.MiDepartamento);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.DepartamentosModificar(this.MiDepartamento);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiDepartamento.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiDepartamento.CodigoMensaje, true, this.MiDepartamento.CodigoMensajeArgs);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.DepartamentoDatosCancelar != null)
                this.DepartamentoDatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.DepartamentoDatosAceptar != null)
                this.DepartamentoDatosAceptar(null, this.MiDepartamento);
        }
    }
}