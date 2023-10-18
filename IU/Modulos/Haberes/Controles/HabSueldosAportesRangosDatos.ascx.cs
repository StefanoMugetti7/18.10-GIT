using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Haberes;
using Haberes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Haberes.Controles
{
    public partial class HabSueldosAportesRangosDatos : ControlesSeguros
    {
        private HabSueldosAportesRangos MiSueldoAporteRango
        {
            get { return (HabSueldosAportesRangos)Session[MiSessionPagina + "HabSueldosAportesRangosMiSueldoAporteRango"]; }
            set { Session[MiSessionPagina + "HabSueldosAportesRangosMiSueldoAporteRango"] = value; }
        }

        public delegate void HabSueldosAportesRangosAceptarEventHandler();
        public event HabSueldosAportesRangosAceptarEventHandler HabSueldosAportesRangosAceptar;

        public delegate void HabSueldosAportesRangosCancelarEventHandler();
        public event HabSueldosAportesRangosCancelarEventHandler HabSueldosAportesRangosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!IsPostBack)
            {


            }
        }

        public void IniciarControl(HabSueldosAportesRangos pParametro, Gestion pGestion)
        {
            GestionControl = pGestion;
            MiSueldoAporteRango = pParametro;
            CargarCombos();
            
            switch (GestionControl)
            {
                case Gestion.Agregar:

                    break;

                case Gestion.Modificar:

                    MiSueldoAporteRango = HaberesF.AportesRangosObtenerDatosCompletos(MiSueldoAporteRango);
                    MapearObjetoAControles(MiSueldoAporteRango);



                    break;


                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
        }

        private void MapearObjetoAControles(HabSueldosAportesRangos pParametro)
        {
            txtAnioMaximo.Text = pParametro.AniosMaximos.ToString();
            txtAnioMinimo.Text = pParametro.AniosMinimos.ToString();
            txtIngresoDesde.Text = pParametro.FechaIngresoDesde.ToString();
            txtIngresoHasta.Text = pParametro.FechaIngresoHasta.ToString();
            txtPorcentajeMaximo.Decimal = Convert.ToDecimal(pParametro.PorcentajeAporteMaximo.ToString("N" + txtPorcentajeMaximo.NumberOfDecimals));
            txtPorcentajeMinimo.Decimal = Convert.ToDecimal(pParametro.PorcentajeAporteMinimo.ToString("N" + txtPorcentajeMaximo.NumberOfDecimals));
            ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();


        }

        private void MapearControlesAObjeto(HabSueldosAportesRangos pParametro)
        {
            pParametro.AniosMaximos = txtAnioMaximo.Text == string.Empty ? 0 : Convert.ToInt32(txtAnioMaximo.Text);
            pParametro.AniosMinimos = txtAnioMinimo.Text == string.Empty ? 0 : Convert.ToInt32(txtAnioMinimo.Text);
            pParametro.FechaIngresoDesde = Convert.ToDateTime(txtIngresoDesde.Text);
            pParametro.FechaIngresoHasta = Convert.ToDateTime(txtIngresoHasta.Text);
            pParametro.PorcentajeAporteMaximo = txtPorcentajeMaximo.Decimal;
            pParametro.PorcentajeAporteMinimo = txtPorcentajeMinimo.Decimal;
            pParametro.Estado.IdEstado = Convert.ToInt32(ddlEstados.SelectedValue);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //if (!Page.IsValid)
            //    return;

            MapearControlesAObjeto(MiSueldoAporteRango);


            // MiPlazoFijoPropio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            switch (GestionControl)
            {
                case Gestion.Agregar:

                    guardo = HaberesF.AportesRangosAgregar(MiSueldoAporteRango);
                    break;
                case Gestion.Modificar:
                    MiSueldoAporteRango.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = HaberesF.AportesRangosModificar(MiSueldoAporteRango);
                    break;
            }
            if (guardo)
            {
                btnAceptar.Visible = false;
                MostrarMensaje(MiSueldoAporteRango.CodigoMensaje, false);
            }
            else
            {
                if (MiSueldoAporteRango.ConfirmarAccion) { }
                // popUpMensajes.MostrarMensaje(ObtenerMensajeSistema(MiPlazoFijoPropio.CodigoMensaje, MiPlazoFijoPropio.CodigoMensajeArgs), true);
                else
                    MostrarMensaje(MiSueldoAporteRango.CodigoMensaje, false, MiSueldoAporteRango.CodigoMensajeArgs);
            }


        }

        //Agregar btn y después descomentar:
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            HabSueldosAportesRangosCancelar?.Invoke();
        }
    }
}