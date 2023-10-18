using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using LavaYa.Entidades;

namespace IU.Modulos.LavaYa.Controles
{
    public partial class DiasHorasModificarDatosPopUp : ControlesSeguros
    {
        private LavPuntosVentasDetalle MiDiasHoras
        {
            get { return (LavPuntosVentasDetalle)Session[this.MiSessionPagina + "DiasHorasModificarDatosPopUpMiDiasHoras"]; }
            set { Session[this.MiSessionPagina + "DiasHorasModificarDatosPopUpMiDiasHoras"] = value; }
        }



        public delegate void DiasHorasModificarDatosEventHandler(LavPuntosVentasDetalle e, Gestion pGestion);
        public event DiasHorasModificarDatosEventHandler DiasHorasModificarDatosAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //this.paginaSegura.scriptManager.Scripts.Add(new ScriptReference("~/Recursos/jquery.ptTimeSelect.js"));
            }
        }

        public void IniciarControl(LavPuntosVentasDetalle pParametro, Gestion pGestion)
        {

            this.MiDiasHoras = pParametro;
            this.GestionControl = pGestion;
            this.ddlEstados.Enabled = true;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.txtFe.Text = DateTime.Now.ToShortDateString();
                    pParametro.Estado.IdEstado = (int)Estados.Activo;
                    this.ddlEstados.Enabled = false;
                    //pAfiliado.Estado = TGEGeneralesF.TGEEstadosObtener(pAfiliado.Estado);
                    this.CargarCombos();
                    break;
                case Gestion.Modificar:
                    this.MapearObjetoAControles(this.MiDiasHoras);
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MapearObjetoAControles(this.MiDiasHoras);
                    this.txtHoraDesde.Enabled = false;
                    this.txtHoraHasta.Enabled = false;
                    this.ddlDia.Enabled = false;
                    this.ddlEstados.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalDiasHoras();", true);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("DiasHorasModificarDatos");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiDiasHoras);

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiDiasHoras.Estado.IdEstado = (int)Estados.Activo;
                    this.MiDiasHoras.EstadoColeccion = EstadoColecciones.Agregado;
                    break;
                case Gestion.Modificar:
                    this.MiDiasHoras.Estado.IdEstado = Convert.ToInt32(ddlEstados.SelectedValue);
                    this.MiDiasHoras.Estado.Descripcion = ddlEstados.SelectedItem.ToString();
                    this.MiDiasHoras.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiDiasHoras, this.GestionControl);
                    break;
                default:
                    break;
            }

            if (this.DiasHorasModificarDatosAceptar != null)
                this.DiasHorasModificarDatosAceptar(this.MiDiasHoras, this.GestionControl);
            //AyudaProgramacion.LimpiarControles(this.pnlPopUp, true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalDiasHoras();", true);
        }

        private void MapearControlesAObjeto(LavPuntosVentasDetalle pParametro)
        {
            pParametro.IdDia = this.ddlDia.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlDia.SelectedValue);
            pParametro.Dia = this.ddlDia.SelectedItem.Text;
            DateTime horaDesde = DateTime.Parse(this.txtHoraDesde.Text);
            pParametro.HoraDesde = new TimeSpan(horaDesde.Hour, horaDesde.Minute, horaDesde.Second);
            DateTime horaHasta = DateTime.Parse(this.txtHoraHasta.Text);
            pParametro.HoraHasta = new TimeSpan(horaHasta.Hour, horaHasta.Minute, horaHasta.Second);
            pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEstados.SelectedValue);
        }

        private void MapearObjetoAControles(LavPuntosVentasDetalle pParametro)
        {
            this.CargarCombos();
            DateTime horaDesde = DateTime.Parse(pParametro.HoraDesde.ToString());
            this.txtHoraDesde.Text = horaDesde.ToString("hh:mm tt");
            DateTime horaHasta = DateTime.Parse(pParametro.HoraHasta.ToString());
            this.txtHoraHasta.Text = horaHasta.ToString("hh:mm tt");

            ListItem item = this.ddlDia.Items.FindByValue(pParametro.IdDia.ToString());
            if (item == null)
                this.ddlDia.Items.Add(new ListItem(pParametro.IdDia.ToString(), pParametro.Dia));
            this.ddlDia.SelectedValue = pParametro.IdDia.ToString();

            this.ddlEstados.SelectedValue = pParametro.Estado.IdEstado.ToString();


        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //AyudaProgramacion.LimpiarControles(this.pnlPopUp, true);
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "HideModalDiasHoras();", true);
        }

        private void CargarCombos()
        {

            List<TGEListasValoresSistemasDetalles> lista = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.Dias);
            lista = lista.OrderBy(X => X.CodigoValor).ToList();

            this.ddlDia.DataSource = lista;
            this.ddlDia.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlDia.DataTextField = "Descripcion";
            this.ddlDia.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlDia, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }







    }
}