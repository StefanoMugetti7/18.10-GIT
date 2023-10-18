using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subsidios.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.Subsidios.Controles
{
    public partial class SubEscalasDatosPopUp : ControlesSeguros
    {
        private SubEscalas MiEscala
        {
            get { return (SubEscalas)Session[this.MiSessionPagina + "SubEscalasDatosPopUpMiEscala"]; }
            set { Session[this.MiSessionPagina + "SubEscalasDatosPopUpMiEscala"] = value; }
        }

        public delegate void SubEscalasDatosEventHandler(SubEscalas e, Gestion pGestion);
        public event SubEscalasDatosEventHandler EscalasModificarDatosAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //this.paginaSegura.scriptManager.Scripts.Add(new ScriptReference("~/Recursos/jquery.ptTimeSelect.js"));
            }
        }

        public void IniciarControl(SubEscalas pParametro, Gestion pGestion)
        {
            this.CargarCombos();
            this.MiEscala = pParametro;
            this.GestionControl = pGestion;
            this.ddlEstados.Enabled = true;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.txtFe.Text = DateTime.Now.ToShortDateString();
                    pParametro.Estado.IdEstado = (int)Estados.Activo;
                    this.ddlEstados.Enabled = false;
                    //pAfiliado.Estado = TGEGeneralesF.TGEEstadosObtener(pAfiliado.Estado);

                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiEscala);
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MapearObjetoAControles(this.MiEscala);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpEscalasModal", "ShowModalEscalasPopUp();", true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("DiasHorasModificarDatos");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiEscala);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiEscala.Estado.IdEstado = (int)Estados.Activo;
                    this.MiEscala.EstadoColeccion = EstadoColecciones.Agregado;
                    break;
                case Gestion.Modificar:
                    this.MiEscala.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiEscala, this.GestionControl);
                    break;
                default:
                    break;
            }

            if (this.EscalasModificarDatosAceptar != null)
                this.EscalasModificarDatosAceptar(this.MiEscala, this.GestionControl);
            //AyudaProgramacion.LimpiarControles(this.pnlPopUp, true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpEscalasModal", "HideModalEscalasPopUp();", true);
        }

        private void MapearControlesAObjeto(SubEscalas pParametro)
        {
            pParametro.FechaIngresoDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaIngresoHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.EdadDesde = this.txtEdadDesde.Text == string.Empty ? default(int?) : Convert.ToInt32(this.txtEdadDesde.Text);
            pParametro.EdadHasta = this.txtEdadHasta.Text == string.Empty ? default(int?) : Convert.ToInt32(this.txtEdadHasta.Text);
            pParametro.AntiguedadDesde = this.txtAntiguedadDesde.Text == string.Empty ? default(int?) : Convert.ToInt32(this.txtAntiguedadDesde.Text);
            pParametro.AntiguedadHasta = this.txtAntiguedadHasta.Text == string.Empty ? default(int?) : Convert.ToInt32(this.txtAntiguedadHasta.Text);
            pParametro.FechaInicioVigencia = this.txtFechaInicioVigencia.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaInicioVigencia.Text);
            pParametro.FechaFinVigencia = this.txtFechaFinVigencia.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaFinVigencia.Text);
            pParametro.ImporteBeneficio = this.txtImporteBeneficio.Decimal;
            pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Estado.Descripcion = this.ddlEstados.Text;
        }

        private void MapearObjetoAControles(SubEscalas pParametro)
        {
            this.txtFechaDesde.Text  = pParametro.FechaIngresoDesde.HasValue? pParametro.FechaIngresoDesde.Value.ToShortDateString() : string.Empty;
            this.txtFechaHasta.Text = pParametro.FechaIngresoHasta.HasValue ? pParametro.FechaIngresoHasta.Value.ToShortDateString() : string.Empty;
            this.txtEdadDesde.Text = pParametro.EdadDesde.HasValue ? pParametro.EdadDesde.ToString() : string.Empty;
            this.txtEdadHasta.Text = pParametro.EdadHasta.HasValue ? pParametro.EdadHasta.ToString() : string.Empty;
            this.txtAntiguedadDesde.Text = pParametro.AntiguedadDesde.HasValue ? pParametro.AntiguedadDesde.ToString() : string.Empty;
            this.txtAntiguedadHasta.Text = pParametro.AntiguedadHasta.HasValue ? pParametro.AntiguedadHasta.ToString() : string.Empty;
            this.txtFechaInicioVigencia.Text = pParametro.FechaInicioVigencia.HasValue ? pParametro.FechaInicioVigencia.Value.ToShortDateString() : string.Empty;
            this.txtFechaFinVigencia.Text = pParametro.FechaFinVigencia.HasValue ? pParametro.FechaFinVigencia.Value.ToShortDateString() : string.Empty;
            this.txtImporteBeneficio.Text = pParametro.ImporteBeneficio.ToString("N2");
            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //AyudaProgramacion.LimpiarControles(this.pnlPopUp, true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpEscalasModal", "HideModalEscalasPopUp();", true);
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }

    }
}