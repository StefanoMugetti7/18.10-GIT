using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;

namespace IU.Modulos.TGE.Control
{
    public partial class ListasValoresDetallesDatosPopUp : ControlesSeguros
    {
        private TGEListasValoresDetalles MiListaValorDetalle
        {
            get { return (TGEListasValoresDetalles)Session[this.MiSessionPagina + "ListasValoresDetallesDatosPopUpMiListaValorDetalle"]; }
            set { Session[this.MiSessionPagina + "ListasValoresDetallesDatosPopUpMiListaValorDetalle"] = value; }
        }

        private TGEListasValores MiListaValor
        {
            get { return (TGEListasValores)Session[this.MiSessionPagina + "ListasValoresDetallesDatosPopUpMiListaValor"]; }
            set { Session[this.MiSessionPagina + "ListasValoresDetallesDatosPopUpMiListaValor"] = value; }
        }

        public delegate void ListasValoresDetallesDatosPopUpAceptarEventHandler(TGEListasValoresDetalles e, Gestion pGestion);
        public event ListasValoresDetallesDatosPopUpAceptarEventHandler ListasValoresDetallesPopUpAceptar;

        public delegate void ListasValoresDetallesDatosPopUpCancelarEventHandler();
        public event ListasValoresDetallesDatosPopUpCancelarEventHandler ListasValoresDetallesPopUpCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
                this.MiListaValorDetalle = new TGEListasValoresDetalles();
        }

        public void IniciarControl(TGEListasValoresDetalles pParametro, TGEListasValores lista, Gestion pGestion)
        {
            this.LimpiarControles();
            this.CargarCombos(lista);
            this.MiListaValorDetalle = pParametro;
            this.MiListaValor = lista;
            this.GestionControl = pGestion;
            //this.txtListaValor.Text = pParametro.ListaValor.ListaValor;
            int indice = MiListaValorDetalle.IndiceColeccion;
            this.ctrCamposValores.BorrarControlesParametros();
            switch (pGestion)
            {
                case Gestion.Agregar:
                    if (UsuarioActivo.EsAdministradorGeneral)
                    {
                        lblConsultaDinamicaCombos.Visible = true;
                        txtConsultaDinamicaCombos.Visible = true;
                    }
                    this.ddlEstados.Enabled = false;
                    this.ctrCamposValores.IniciarControl(this.MiListaValorDetalle, MiListaValor, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    if (UsuarioActivo.EsAdministradorGeneral)
                    {
                        lblConsultaDinamicaCombos.Visible = true;
                        txtConsultaDinamicaCombos.Visible = true;
                    }
                    this.MiListaValorDetalle = TGEGeneralesF.ListasValoresItemObtenerDatosCompletos(pParametro);
                    MiListaValorDetalle.IndiceColeccion = indice;
                    this.MapearObjetoAControles(MiListaValorDetalle);
                    this.HabilitarControles(true);
                    this.ctrCamposValores.IniciarControl(this.MiListaValorDetalle, MiListaValor, this.GestionControl);
                    break;
                case Gestion.Consultar:
                    this.MiListaValorDetalle = TGEGeneralesF.ListasValoresItemObtenerDatosCompletos(MiListaValorDetalle);
                    MiListaValorDetalle.IndiceColeccion = indice;
                    this.MapearObjetoAControles(MiListaValorDetalle);
                    this.HabilitarControles(false);
                    break;
                default:
                    break;
            }

            //this.mpePopUp.Show();
        }

        private void MapearObjetoAControles(TGEListasValoresDetalles MiListaValorDetalle)
        {
            this.txtCodigoValor.Text = MiListaValorDetalle.CodigoValor;
            this.txtDescripcion.Text = MiListaValorDetalle.Descripcion;
            this.txtConsultaDinamicaCombos.Text = MiListaValorDetalle.ConsultaDinamicaCombo;
            this.ddlEstados.SelectedValue = MiListaValorDetalle.Estado.IdEstado.ToString();
            if (MiListaValor.IdRefListaValor > 0)
            {
                //phDepende.Visible = true;
                ListItem item = this.ddlListaDepende.Items.FindByValue(MiListaValorDetalle.IdRefListaValorDetalle.ToString());
                if (item == null)
                    this.ddlListaDepende.Items.Add(new ListItem(MiListaValorDetalle.DescripcionRef, MiListaValorDetalle.IdRefListaValorDetalle.ToString()));
                this.ddlListaDepende.SelectedValue = MiListaValorDetalle.IdRefListaValorDetalle.ToString();
            }
        }

        private void MapearControlesAObjeto(TGEListasValoresDetalles MiListaValorDetalle)
        {
            MiListaValorDetalle.CodigoValor = this.txtCodigoValor.Text;
            MiListaValorDetalle.Descripcion = this.txtDescripcion.Text;
            MiListaValorDetalle.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            MiListaValorDetalle.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            MiListaValorDetalle.ConsultaDinamicaCombo = this.txtConsultaDinamicaCombos.Text;
            if (MiListaValor.IdRefListaValor > 0)
            {
                MiListaValorDetalle.IdRefListaValorDetalle = ddlListaDepende.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlListaDepende.SelectedValue);
                MiListaValorDetalle.DescripcionRef = ddlListaDepende.SelectedValue == string.Empty ? string.Empty : this.ddlListaDepende.SelectedItem.Text;
            }
             this.MiListaValorDetalle.Campos = this.ctrCamposValores.ObtenerLista();
        }

        private void LimpiarControles()
        {
            this.txtCodigoValor.Text = string.Empty;
            this.txtDescripcion.Text = string.Empty;
            this.ddlListaDepende.Items.Clear();
            this.ddlListaDepende.SelectedIndex = -1;
            this.ddlListaDepende.SelectedValue = null;
            this.ddlListaDepende.ClearSelection();
        }

        private void HabilitarControles(bool pEstado)
        {
            this.txtCodigoValor.Enabled = pEstado;
            this.txtDescripcion.Enabled = pEstado;
            this.ddlEstados.Enabled = pEstado;
            this.btnAceptar.Visible = pEstado;
        }

        private void CargarCombos(TGEListasValores MiListaValor)
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();


            this.ddlListaDepende.Items.Clear();
            this.ddlListaDepende.SelectedIndex = -1;
            this.ddlListaDepende.SelectedValue = null;
            this.ddlListaDepende.ClearSelection();

            if (MiListaValor.IdRefListaValor.HasValue)
            {
                //this.rfvListaDepende.Visible = true;
                //phDepende.Visible = true;
                this.ddlListaDepende.DataSource = TGEGeneralesF.ListasValoresObtenerListaRecursivaItem(MiListaValor);
                this.ddlListaDepende.DataValueField = "IdListaValorDetalle";
                this.ddlListaDepende.DataTextField = "Descripcion";
                this.ddlListaDepende.DataBind();
                if (this.ddlListaDepende.Items.Count != 1)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlListaDepende, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.ListasValoresDetallesPopUpCancelar();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("AfiliadosDatosDomicilios");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiListaValorDetalle);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiListaValorDetalle.Estado.IdEstado = (int)Estados.Activo;
                    this.MiListaValorDetalle.EstadoColeccion = EstadoColecciones.Agregado;
                    break;
                case Gestion.Modificar:
                    this.MiListaValorDetalle.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiListaValorDetalle, this.GestionControl);
                    break;
                default:
                    break;
            }
            //this.mpePopUp.Hide();
            if (this.ListasValoresDetallesPopUpAceptar != null)
                this.ListasValoresDetallesPopUpAceptar(this.MiListaValorDetalle, this.GestionControl);
        }
    }
}