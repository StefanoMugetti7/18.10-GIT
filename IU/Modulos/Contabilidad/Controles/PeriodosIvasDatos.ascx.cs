using Comunes.Entidades;
using Contabilidad;
using Contabilidad.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class PeriodosIvasDatos : ControlesSeguros
    {
        private CtbPeriodosIvas MiPeriodoIva
        {
            get { return (CtbPeriodosIvas)Session[this.MiSessionPagina + "PeriodoIVAMiPeriodoIVA"]; }
            set { Session[this.MiSessionPagina + "PeriodoIVAMiPeriodoIVA"] = value; }
        }
        public delegate void PeriodoIVADatosAceptarEventHandler(object sender, CtbPeriodosIvas e);
        public event PeriodoIVADatosAceptarEventHandler PeriodoIVADatosAceptar;
        public delegate void PeriodoIVADatosCancelarEventHandler();
        public event PeriodoIVADatosCancelarEventHandler PeriodoIVADatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
                if (this.MiPeriodoIva == null && this.GestionControl != Gestion.Agregar)
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }
        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbPeriodosIvas pPeriodoIVA, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPeriodoIva = pPeriodoIVA;
                    this.txtPeriodo.Enabled = true;
                    this.ddlEstado.SelectedValue = ((int)EstadosPeriodosIVAS.Bloqueado).ToString();
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
                    //if (this.MiPeriodoIva.Periodo != ContabilidadF.PeriodosIvasObtenerUltimoCerrado(this.MiPeriodoIva).Periodo)
                    //{
                    //    this.ddlEstado.Enabled = false;
                    //    this.btnAceptar.Visible = false;
                    //} else {
                    //    this.ddlEstado.SelectedValue = ((int)Estados.Baja).ToString();
                    //    this.ddlEstado.Enabled = false;
                    //}
                    this.MiPeriodoIva = ContabilidadF.PeriodosIvasObtenerDatosCompletos(pPeriodoIVA);
                    this.MapearObjetoAControles(this.MiPeriodoIva);
                    this.txtPeriodo.Enabled = false;
                    this.txtFechaCierre.Enabled = false;
                    this.ddlEjercicioContable.Enabled = false;
                    this.btnArmarLiquidacion.Visible = false;
                    this.txtIVAVentas.Enabled = true;
                    this.txtIVACompras.Enabled = true;
                    this.txtPercepciones.Enabled = true;
                    this.txtRetenciones.Enabled = true;
                    break;
                case Gestion.Consultar:
                    this.MiPeriodoIva = ContabilidadF.PeriodosIvasObtenerDatosCompletos(pPeriodoIVA);
                    this.MapearObjetoAControles(this.MiPeriodoIva);
                    this.txtFechaCierre.Enabled = false;
                    this.txtFechaContable.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.ddlEjercicioContable.Enabled = false;
                    this.btnArmarLiquidacion.Visible = false;
                    this.txtIVAVentas.Enabled = false;
                    this.txtIVACompras.Enabled = false;
                    this.txtIVAPosicion.Enabled= false;
                    this.txtPercepciones.Enabled= false;
                    this.txtRetenciones.Enabled= false;
                    this.txtIVAAPagar.Enabled= false;
                    break;
                default:
                    break;
            }
        }
        private void MapearObjetoAControles(CtbPeriodosIvas pPeriodoIVA)
        {
            this.txtPeriodo.Text = pPeriodoIVA.Periodo.ToString();
            this.txtFechaCierre.Text = pPeriodoIVA.FechaCierre.ToShortDateString();
            this.txtFechaContable.Text = pPeriodoIVA.FechaContable.ToShortDateString();
            this.ddlEstado.SelectedValue = pPeriodoIVA.Estado.IdEstado.ToString();
            this.ddlEjercicioContable.SelectedValue = pPeriodoIVA.EjercicioContable.IdEjercicioContable.ToString();
            this.txtIVAVentas.Text = pPeriodoIVA.IVAVentas.ToString("C2");
            this.txtIVACompras.Text = pPeriodoIVA.IVACompras.ToString("C2");
            this.txtIVAPosicion.Text = pPeriodoIVA.IVAPosicion.ToString("C2");
            this.txtPercepciones.Text = pPeriodoIVA.Percepciones.ToString("C2");
            this.txtRetenciones.Text = pPeriodoIVA.Retenciones.ToString("C2");
            this.txtIVAAPagar.Text = pPeriodoIVA.IVAAPagar.ToString("C2");
            this.txtSaldoTecnico.Text = pPeriodoIVA.SaldoTecnico.ToString("C2");
        }
        protected void ddlEjerciciosContables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
                return;
            this.MiPeriodoIva.EjercicioContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            //VER QUE NO SEA EL PRIMERO
            this.MiPeriodoIva.EjercicioContable = ContabilidadF.EjerciciosContablesObtenerDatosCompletos(this.MiPeriodoIva.EjercicioContable);
            //obtengo el ultimo período cerrado
            int ultimoCerrado = ContabilidadF.PeriodosIvasObtenerUltimoCerrado(this.MiPeriodoIva).Periodo;
            if (ultimoCerrado == 0)
            {
                string primerPeriodo = this.MiPeriodoIva.EjercicioContable.FechaInicio.Year.ToString();
                primerPeriodo = string.Concat(primerPeriodo, this.MiPeriodoIva.EjercicioContable.FechaInicio.Month.ToString().PadLeft(2, '0'));
                ultimoCerrado = Convert.ToInt32(primerPeriodo);
            }
            else
                ultimoCerrado = this.IncrementarPeriodo(ultimoCerrado);
            this.txtPeriodo.Text = ultimoCerrado.ToString();
            this.txtFechaCierre.Text = DateTime.Now.ToShortDateString();
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
        private void MapearControlesAObjeto(CtbPeriodosIvas pPeriodoIVA)
        {
            pPeriodoIVA.FechaCierre = Convert.ToDateTime(this.txtFechaCierre.Text);
            pPeriodoIVA.Periodo = Convert.ToInt32(this.txtPeriodo.Text);
            pPeriodoIVA.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pPeriodoIVA.FechaContable = Convert.ToDateTime(this.txtFechaContable.Text);
            pPeriodoIVA.IVAVentas = this.txtIVAVentas.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtIVAVentas.Decimal);
            pPeriodoIVA.IVACompras = this.txtIVACompras.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtIVACompras.Decimal);
            pPeriodoIVA.IVAPosicion = this.txtIVAPosicion.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtIVAPosicion.Decimal);
            pPeriodoIVA.Percepciones =  this.txtPercepciones.Text == string.Empty ? 0 : Convert.ToDecimal(txtPercepciones.Decimal);
            pPeriodoIVA.Retenciones = this.txtRetenciones.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtRetenciones.Decimal);
            pPeriodoIVA.IVAAPagar = this.txtIVAAPagar.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtIVAAPagar.Decimal);
            pPeriodoIVA.SaldoTecnico = this.txtSaldoTecnico.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtSaldoTecnico.Decimal);
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPeriodosIVAS));
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
        }
        protected void btnArmarLiquidacion_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.txtPeriodo.Text))
            {
                this.txtIVAVentas.Visible = true;
                this.txtIVACompras.Visible = true;
                this.txtIVAPosicion.Visible = true;
                this.txtPercepciones.Visible = true;
                this.txtRetenciones.Visible = true;
                this.txtIVAAPagar.Visible = true;
                CtbPeriodosIvas obj = new CtbPeriodosIvas();
                obj.Periodo = Convert.ToInt32(this.txtPeriodo.Text);
                obj = ContabilidadF.PeriodosIvasObtenerArmarLiquidacionIVA(obj);
                this.txtIVAVentas.Text = obj.IVAVentas.ToString("C2");
                this.txtIVACompras.Text = obj.IVACompras.ToString("C2");
                this.txtIVAPosicion.Text = obj.IVAPosicion.ToString("C2");
                this.txtPercepciones.Text = obj.Percepciones.ToString("C2");
                this.txtRetenciones.Text = obj.Retenciones.ToString("C2");
                this.txtIVAAPagar.Text = obj.IVAAPagar.ToString("C2");
                this.txtSaldoTecnico.Text = obj.SaldoTecnico.ToString("C2");
                this.upIvas.Update();
            }
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiPeriodoIva);
            this.MiPeriodoIva.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.PeriodosIvasAgregar(this.MiPeriodoIva);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.PeriodosIvasModificar(this.MiPeriodoIva);
                    break;
                default:
                    break;
            }
            if (guardo)
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiPeriodoIva.CodigoMensaje));
            else
                this.MostrarMensaje(this.MiPeriodoIva.CodigoMensaje, true, this.MiPeriodoIva.CodigoMensajeArgs);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.PeriodoIVADatosCancelar != null)
                this.PeriodoIVADatosCancelar();
        }
        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.PeriodoIVADatosAceptar != null)
                this.PeriodoIVADatosAceptar(null, this.MiPeriodoIva);
        }
    }
}