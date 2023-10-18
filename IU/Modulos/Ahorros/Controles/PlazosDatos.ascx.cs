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
using Ahorros.Entidades;
using Comunes.Entidades;
using Ahorros;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Collections.Generic;

namespace IU.Modulos.Ahorros.Controles
{
    public partial class PlazosDatos : ControlesSeguros
    {
        private AhoPlazos MiAhoPlazos
        {
            get { return (AhoPlazos)Session[this.MiSessionPagina + "AhorroMiAhoPlazos"]; }
            set { Session[this.MiSessionPagina + "AhorroMiAhoPlazos"] = value; }
        }

        private List<TGEMonedas> MisMonedas
        {
            get { return (List<TGEMonedas>)Session[this.MiSessionPagina + "AhorroMisMonedas"]; }
            set { Session[this.MiSessionPagina + "AhorroMisMonedas"] = value; }
        }

        public delegate void PlazosDatosAceptarEventHandler(object sender, AhoPlazos e);
        public event PlazosDatosAceptarEventHandler PlazosDatosAceptar;

        public delegate void PlazosDatosCancelarEventHandler();
        public event PlazosDatosCancelarEventHandler PlazosDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MiAhoPlazos == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(AhoPlazos pPlazos, Gestion pGestion)
        {
            this.MiAhoPlazos = pPlazos;
            this.GestionControl = pGestion;
            this.DeshabilitarControles();
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    //this.MiAhoPlazos = pPlazos;
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazos, new Objeto(), this.GestionControl);
                    //this.ddlEstado.SelectedIndex = 1;
                    //this.txtFechaAlta.Text = DateTime.Today.ToShortDateString();
                    break;
                case Gestion.Modificar:
                    this.MiAhoPlazos = AhorroF.PlazosObtenerDatosCompletos(pPlazos);
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazos, new Objeto(), this.GestionControl);
                    this.MapearObjetoAControles(this.MiAhoPlazos);
                    break;
                case Gestion.Consultar:
                    this.MiAhoPlazos = AhorroF.PlazosObtenerDatosCompletos(pPlazos);
                    this.ctrCamposValores.IniciarControl(this.MiAhoPlazos, new Objeto(), this.GestionControl);
                    this.MapearObjetoAControles(this.MiAhoPlazos);
                    //this.txtPlazoDias.Enabled = false;
                    //this.txtTasaInteres.Enabled = false;
                    //this.ddlEstado.Enabled = false;
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
            this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();

            MisMonedas = TGEGeneralesF.MonedasCotizacionesObtenerCombo();
            this.ddlMoneda.DataSource = MisMonedas;
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "MonedaDescripcionCotizacion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            else
                ddlMoneda_SelectedIndexChanged(ddlMoneda, EventArgs.Empty);

            if (GestionControl == Gestion.Agregar)
            {
                AhoPlazos filtroPlazos = new AhoPlazos();
                filtroPlazos.IdPlazos = MiAhoPlazos.IdPlazos;
                //filtroPlazos.Moneda.IdMoneda = Convert.ToInt32(ddlMoneda.SelectedValue);//MiAhoPlazos.Moneda.IdMoneda;
                
                List<AhoPlazos> plazo = new List<AhoPlazos>();
                plazo = AhorroF.PlazosObtenerAnterior(MiAhoPlazos);

                if (plazo.Count > 0)
                {
                    ddlPlazoAnterior.DataSource = plazo;

                    ddlPlazoAnterior.DataValueField = "IdPlazoAnterior";
                    ddlPlazoAnterior.DataTextField = "Descripcion";
                    ddlPlazoAnterior.DataBind();
                }
                if (ddlPlazoAnterior.Items.Count != 1)
                {
                    AyudaProgramacion.InsertarItemSeleccione(ddlPlazoAnterior, ObtenerMensajeSistema("SeleccioneOpcion"));
                }
                
            }
            //this.ddlMoneda.DataSource = TGEGeneralesF.MonedasObtenerListaActiva();
            //this.ddlMoneda.DataValueField = "IdMoneda";
            //this.ddlMoneda.DataTextField = "Descripcion";
            //this.ddlMoneda.DataBind();
            //if (this.ddlMoneda.Items.Count != 1)
            //    AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        private void DeshabilitarControles()
        {
            this.txtFechaAlta.Enabled = false;
        }

        private void MapearObjetoAControles(AhoPlazos pPlazos)
        {
            this.txtPlazoDias.Text = pPlazos.PlazoDias.ToString();
            this.txtTasaInteres.Text = pPlazos.TasaInteres.ToString();
            this.txtFechaDesde.Text = pPlazos.FechaDesde.ToShortDateString();
            this.ddlEstado.SelectedValue = pPlazos.Estado.IdEstado.ToString();
            this.txtFechaAlta.Text = pPlazos.FechaAlta.ToShortDateString();
            txtImporteDesde.Text = pPlazos.ImporteDesde.ToString("C2");
            txtImporteHasta.Text = pPlazos.ImporteHasta.ToString("C2");
            
            ListItem item;
            item = this.ddlMoneda.Items.FindByValue(pPlazos.Moneda.IdMoneda.ToString());
            if (item == null && pPlazos.Moneda.IdMoneda > 0)
                this.ddlMoneda.Items.Add(new ListItem(pPlazos.Moneda.Descripcion, pPlazos.Moneda.IdMoneda.ToString()));
            this.ddlMoneda.SelectedValue = pPlazos.Moneda.IdMoneda > 0 ? pPlazos.Moneda.IdMoneda.ToString() : string.Empty;

            
            if (pPlazos.Moneda.IdMoneda > 0)
            {
                ddlMoneda_SelectedIndexChanged(null, EventArgs.Empty);
            
                
            }

                AhoPlazos filtroPlazos = new AhoPlazos();
                filtroPlazos.IdPlazos = MiAhoPlazos.IdPlazos;
                //filtroPlazos.Moneda.IdMoneda = Convert.ToInt32(ddlMoneda.SelectedValue);//MiAhoPlazos.Moneda.IdMoneda;
                List<AhoPlazos> plazo = new List<AhoPlazos>();
                plazo = AhorroF.PlazosObtenerAnterior(MiAhoPlazos);

                if (plazo.Count > 0)
                {
                    ddlPlazoAnterior.Items.Clear();
                    ddlPlazoAnterior.SelectedIndex = -1;
                    ddlPlazoAnterior.SelectedValue = null;
                    ddlPlazoAnterior.ClearSelection();
                    ddlPlazoAnterior.DataSource = plazo;
                    ddlPlazoAnterior.DataValueField = "IdPlazoAnterior";
                    ddlPlazoAnterior.DataTextField = "Descripcion";
                    ddlPlazoAnterior.DataBind();
                }
          
            item = ddlPlazoAnterior.Items.FindByValue(pPlazos.IdPlazoAnterior.ToString());
            if (item == null && pPlazos.IdPlazoAnterior > 0)
            {
                ddlPlazoAnterior.Items.Add(new ListItem(pPlazos.Descripcion, pPlazos.IdPlazoAnterior.ToString()));
            }
            ddlPlazoAnterior.SelectedValue = pPlazos.IdPlazoAnterior > 0 ? pPlazos.IdPlazoAnterior.ToString() : string.Empty;
            //this.ctrParametrosValores.IniciarControl(pPlazos, Gestion.Agregar, Generales.Entidades.EnumCamposTipos.NumericTextBox);
            if (ddlPlazoAnterior.Items.Count != 1)
            {
                AyudaProgramacion.InsertarItemSeleccione(ddlPlazoAnterior, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            this.ctrComentarios.IniciarControl(pPlazos, this.GestionControl);
            this.ctrArchivos.IniciarControl(pPlazos, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pPlazos);
            this.ctrCamposValores.IniciarControl(pPlazos, new Objeto(), this.GestionControl);
        }

        private void MapearControlesAObjeto(AhoPlazos pPlazos)
        {
            pPlazos.PlazoDias = Convert.ToInt32(this.txtPlazoDias.Text);
            pPlazos.TasaInteres = Convert.ToDecimal(this.txtTasaInteres.Text);
            pPlazos.FechaDesde = Convert.ToDateTime(this.txtFechaDesde.Text);
            pPlazos.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pPlazos.Moneda.IdMoneda = Convert.ToInt32(this.ddlMoneda.SelectedValue);
            pPlazos.ImporteDesde = txtImporteDesde.Text == "" ? 0 : txtImporteDesde.Decimal;// Convert.ToDecimal(txtImporteHasta.Text);
            pPlazos.ImporteHasta = txtImporteHasta.Text == "" ? 0 : txtImporteHasta.Decimal;// Convert.ToDecimal(txtImporteDesde.Text);
            pPlazos.IdPlazoAnterior = ddlPlazoAnterior.SelectedValue == "" ? 0 : Convert.ToInt32(ddlPlazoAnterior.SelectedValue);

            pPlazos.Comentarios = ctrComentarios.ObtenerLista();
            pPlazos.Archivos = ctrArchivos.ObtenerLista();
            pPlazos.Campos = this.ctrCamposValores.ObtenerLista();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiAhoPlazos);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAhoPlazos.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = AhorroF.PlazosAgregar(this.MiAhoPlazos);
                    break;
                case Gestion.Modificar:
                    guardo = AhorroF.PlazosModificar(this.MiAhoPlazos);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAhoPlazos.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiAhoPlazos.CodigoMensaje, true, this.MiAhoPlazos.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.PlazosDatosCancelar != null)
                this.PlazosDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.PlazosDatosAceptar != null)
                this.PlazosDatosAceptar(null, this.MiAhoPlazos);
        }

        protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlMoneda.SelectedValue))
            {
                AhoPlazos ahoPlazos = new AhoPlazos();
                ahoPlazos.Estado.IdEstado = (int)Estados.Activo;
                ahoPlazos.Moneda = MisMonedas.First(x => x.IdMoneda == Convert.ToInt32(ddlMoneda.SelectedValue));
                ahoPlazos.IdPlazos = MiAhoPlazos.IdPlazos;
                List<AhoPlazos> plazo = new List<AhoPlazos>();
                plazo = AhorroF.PlazosObtenerAnterior(MiAhoPlazos);

                if (plazo.Count > 0)
                {
                    ddlPlazoAnterior.DataSource = plazo;

                    ddlPlazoAnterior.DataValueField = "IdPlazoAnterior";
                    ddlPlazoAnterior.DataTextField = "Descripcion";
                    ddlPlazoAnterior.DataBind();
                }
                if(ddlPlazoAnterior.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(ddlPlazoAnterior, ObtenerMensajeSistema("SeleccioneOpcion"));

                SetInitializeCulture(ahoPlazos.Moneda.Moneda);
            }
        }
    }
}