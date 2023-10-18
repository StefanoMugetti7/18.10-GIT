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
using System.Globalization;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class PeriodosContablesDatos : ControlesSeguros
    {
        private CtbPeriodosContables MiPeriodoContable
        {
            get { return (CtbPeriodosContables)Session[this.MiSessionPagina + "MiPeriodoContable"]; }
            set { Session[this.MiSessionPagina + "MiPeriodoContable"] = value; }
        }

        public delegate void PeriodoContableDatosAceptarEventHandler(object sender, CtbPeriodosContables e);
        public event PeriodoContableDatosAceptarEventHandler PeriodoContableDatosAceptar;

        public delegate void PeriodoContableDatosCancelarEventHandler();
        public event PeriodoContableDatosCancelarEventHandler PeriodoContableDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.MiPeriodoContable == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbPeriodosContables pPeriodoContable, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPeriodoContable = pPeriodoContable;
                    this.txtPeriodo.Enabled = true;
                    this.ddlEstado.SelectedValue = ((int)EstadosPeriodosContables.Bloqueado).ToString();
                    this.ddlEstado.Enabled = false;
                    this.txtFechaCierre.Enabled = false;
                    if (this.ddlEjercicioContable.Items.Count == 0 || this.ddlEjercicioContable.Items.Count > 1)
                    {
                        this.txtPeriodo.Text = string.Empty;
                        AyudaProgramacion.AgregarItemSeleccione(this.ddlEjercicioContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    }
                    else
                        this.ddlEjerciciosContables_SelectedIndexChanged(null, EventArgs.Empty);

                    break;
                case Gestion.Modificar:
                    this.MiPeriodoContable = ContabilidadF.PeriodosContablesObtenerDatosCompletos(pPeriodoContable);
                    this.MapearObjetoAControles(this.MiPeriodoContable);
                    //if (this.MiPeriodoContable.Periodo != ContabilidadF.PeriodosContablesObtenerUltimoCerrado(this.MiPeriodoContable).Periodo)
                    //{
                    //    this.ddlEstado.Enabled = false;
                    //    this.btnAceptar.Visible = false;
                    //}
                    //else
                    //{
                    //    this.ddlEstado.SelectedValue = ((int)Estados.Baja).ToString();
                    //    this.ddlEstado.Enabled = false;
                    //}
                    this.txtPeriodo.Enabled = true;
                    this.txtFechaCierre.Enabled = false;
                    this.ddlEjercicioContable.Enabled = false;
                    
                    break;
                case Gestion.Consultar:
                    this.MiPeriodoContable = ContabilidadF.PeriodosContablesObtenerDatosCompletos(pPeriodoContable);
                    this.MapearObjetoAControles(this.MiPeriodoContable);
                    //this.txtPeriodo.Enabled = false;
                    this.txtFechaCierre.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.ddlEjercicioContable.Enabled = false;

                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(CtbPeriodosContables pPeriodoContable)
        {
            this.txtPeriodo.Text = pPeriodoContable.Periodo.ToString();
            this.txtFechaCierre.Text = pPeriodoContable.FechaCierre.ToShortDateString();
            this.ddlEstado.SelectedValue = pPeriodoContable.Estado.IdEstado.ToString();
            this.ddlEjercicioContable.SelectedValue = pPeriodoContable.EjercicioContable.IdEjercicioContable.ToString();
        }

        protected void ddlEjerciciosContables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
                return;

            this.MiPeriodoContable.EjercicioContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            //VER QUE NO SEA EL PRIMERO
            this.MiPeriodoContable.EjercicioContable = ContabilidadF.EjerciciosContablesObtenerDatosCompletos(this.MiPeriodoContable.EjercicioContable);
           
           //obtengo el ultimo período cerrado
            int ultimoCerrado = ContabilidadF.PeriodosContablesObtenerUltimoCerrado(this.MiPeriodoContable).Periodo;
           
            if (ultimoCerrado == 0)
            {
                string primerPeriodo = this.MiPeriodoContable.EjercicioContable.FechaInicio.Year.ToString();
                primerPeriodo = string.Concat(primerPeriodo, this.MiPeriodoContable.EjercicioContable.FechaInicio.Month.ToString().PadLeft(2, '0'));
                ultimoCerrado = Convert.ToInt32(primerPeriodo);

            }
            else
            ultimoCerrado = this.IncrementarPeriodo(ultimoCerrado);
            
            this.txtPeriodo.Text = ultimoCerrado.ToString();
            //this.ddlEstado.SelectedValue = Convert.ToString((int)Estados.Activo);//seteo como Activo porque solo se puede dar de baja en el modificar
            this.txtFechaCierre.Text = DateTime.Now.ToString();

            this.upPeriodo.Update();

        }

        private int IncrementarPeriodo(int pPeriodo)
        {
            int mes = Convert.ToInt32(pPeriodo.ToString().Remove(0, 4));
            int anio = Convert.ToInt32(pPeriodo.ToString().Remove(4, 2));

            if (mes < 12)
                mes++;
            else
            {
                mes = 1; //Enero por el cambio de año
                anio++;
            }
            return Convert.ToInt32(String.Concat(anio.ToString(), mes.ToString().PadLeft(2, '0')));
        }

        private void MapearControlesAObjeto(CtbPeriodosContables pPeriodoContable)
        {
            pPeriodoContable.Periodo = Convert.ToInt32(this.txtPeriodo.Text);
            pPeriodoContable.FechaCierre = Convert.ToDateTime(this.txtFechaCierre.Text);
            pPeriodoContable.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPeriodosContables));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            //Cargar Ejercicios contables
            //Cargar Ejercicios contables
            CtbEjerciciosContables filtro = new CtbEjerciciosContables();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            List<CtbEjerciciosContables> lista = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtro);
            this.ddlEjercicioContable.DataSource = lista;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlEjercicioContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiPeriodoContable);
            this.MiPeriodoContable.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.PeriodosContablesAgregar(this.MiPeriodoContable);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.PeriodosContablesModificar(this.MiPeriodoContable);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiPeriodoContable.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiPeriodoContable.CodigoMensaje, true, this.MiPeriodoContable.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.PeriodoContableDatosCancelar != null)
                this.PeriodoContableDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.PeriodoContableDatosAceptar != null)
                this.PeriodoContableDatosAceptar(null, this.MiPeriodoContable);
        }
    }
}