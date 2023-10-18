using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class EjerciciosContablesDatos : ControlesSeguros
    {
        private CtbEjerciciosContables MiEjercicioContable
        {
            get { return (CtbEjerciciosContables)Session[this.MiSessionPagina + "MiEjercicioContable"]; }
            set { Session[this.MiSessionPagina + "MiEjercicioContable"] = value; }
        }

        public delegate void EjercicioContableDatosAceptarEventHandler(object sender, CtbEjerciciosContables e);
        public event EjercicioContableDatosAceptarEventHandler EjercicioContableDatosAceptar;

        public delegate void EjercicioContableDatosCancelarEventHandler();
        public event EjercicioContableDatosCancelarEventHandler EjercicioContableDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiEjercicioContable == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbEjerciciosContables pEjercicioContable, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiEjercicioContable = pEjercicioContable;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.txtFechaCierre.Enabled = false;
                    this.txtFechaCopiativo.Enabled = false;
                    this.btnActualizarPlan.Visible = false;
                    break;
                case Gestion.Modificar:

                    this.MiEjercicioContable = ContabilidadF.EjerciciosContablesObtenerDatosCompletos(pEjercicioContable);
                
                    this.txtFechaCierre.Enabled = false;
                    this.txtFechaCopiativo.Enabled = false;
                    this.MapearObjetoAControles(this.MiEjercicioContable);
                    break;
                case Gestion.Consultar:
                    this.MiEjercicioContable = ContabilidadF.EjerciciosContablesObtenerDatosCompletos(pEjercicioContable);
                    this.MapearObjetoAControles(this.MiEjercicioContable);
                    this.txtDescripcion.Enabled = false;
                    this.txtFechaInicio.Enabled = false;
                    this.txtFechaFin.Enabled = false;
                    this.txtFechaCierre.Enabled = false;
                    this.txtFechaCopiativo.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(CtbEjerciciosContables pEjercicioContable)
        {
            this.txtDescripcion.Text = pEjercicioContable.Descripcion;
            this.txtFechaInicio.Text = pEjercicioContable.FechaInicio.ToShortDateString();
            this.txtFechaFin.Text = pEjercicioContable.FechaFin.ToShortDateString();
            this.txtFechaCierre.Text = pEjercicioContable.FechaCierre == null ? string.Empty : Convert.ToDateTime(pEjercicioContable.FechaCierre).ToShortDateString();
            this.txtFechaCopiativo.Text = pEjercicioContable.FechaCopiativo == null ? string.Empty : Convert.ToDateTime(pEjercicioContable.FechaCopiativo).ToShortDateString();
            this.ddlEstado.SelectedValue = pEjercicioContable.Estado.IdEstado.ToString();
            this.ddlEjercicioContable.Enabled = false;
            this.ddlEjercicioContable.SelectedValue = pEjercicioContable.IdEjercicioContableOrigen.ToString();

        }

        private void MapearControlesAObjeto(CtbEjerciciosContables pEjercicioContable)
        {
            pEjercicioContable.Descripcion = this.txtDescripcion.Text;
            pEjercicioContable.FechaInicio = Convert.ToDateTime(this.txtFechaInicio.Text);
            pEjercicioContable.FechaFin = Convert.ToDateTime(this.txtFechaFin.Text);
            pEjercicioContable.FechaCierre = this.txtFechaCierre.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaCierre.Text);
            pEjercicioContable.FechaCopiativo = this.txtFechaCopiativo.Text == string.Empty ? (Nullable<DateTime>)null :Convert.ToDateTime(this.txtFechaCopiativo.Text);
            pEjercicioContable.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            if(this.GestionControl == Gestion.Agregar)
                pEjercicioContable.IdEjercicioContableOrigen = this.ddlEjercicioContable.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            //Cargar Ejercicios contables
            CtbEjerciciosContables filtro = new CtbEjerciciosContables();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            List<CtbEjerciciosContables> lista = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtro);
            this.ddlEjercicioContable.DataSource = lista;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();
            if (this.ddlEjercicioContable.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEjercicioContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            if (ddlEjercicioContable.Items.Count > 0)
                rfvEjercicioContable.Enabled = true;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiEjercicioContable);
            this.MiEjercicioContable.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.EjerciciosContablesAgregar(this.MiEjercicioContable);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.EjerciciosContablesModificar(this.MiEjercicioContable);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiEjercicioContable.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiEjercicioContable.CodigoMensaje, true, this.MiEjercicioContable.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.EjercicioContableDatosCancelar != null)
                this.EjercicioContableDatosCancelar();
        }
        protected void btnActualizarPlan_Click(object sender, EventArgs e)
        {
            bool Actualizar = true;
             MiEjercicioContable.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
        
            MiEjercicioContable.IdEjercicioContableOrigen = this.ddlEjercicioContable.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            Actualizar = ContabilidadF.EjerciciosContablesActualizarPlanCuentas(this.MiEjercicioContable);
            if (Actualizar)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiEjercicioContable.CodigoMensaje));
            }
        }


        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.EjercicioContableDatosAceptar != null)
                this.EjercicioContableDatosAceptar(null, this.MiEjercicioContable);
        }
    }
}