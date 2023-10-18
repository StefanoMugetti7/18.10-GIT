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
using Comunes.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.TGE.Control
{
    public partial class TiposCambiosDatosPopUp : ControlesSeguros
    {
        //private TGETiposCambios MiTipoCambio
        //{
        //    get { return (TGETiposCambios)Session[this.MiSessionPagina + this.MiSessionPagina + "TiposCambiosDatosPopUpMiTipoCambio"]; }
        //    set { Session[this.MiSessionPagina + this.MiSessionPagina + "TiposCambiosDatosPopUpMiTipoCambio"] = value; }
        //}

        public delegate void TiposCambiosDatosAceptarEventHandler(object sender, TGETiposCambios e);
        public event TiposCambiosDatosAceptarEventHandler TiposCambiosDatosAceptar;
        public delegate void TiposCambiosCancelarEventHandler();
        public event TiposCambiosCancelarEventHandler TiposCambiosDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTipoCambio, this.btnAceptar);
            }
        }

        public void IniciarControl()
        {
            //this.ddlPaises.SelectedValue = pTipoCambio.Pais.IdPais.ToString();
            //this.ddlMonedas.SelectedValue = pTipoCambio.Moneda.IdMoneda.ToString();
            this.txtFechaDesde.Text = DateTime.Now.ToShortDateString();
            this.txtTipoCambio.Text = string.Empty;
            this.mpePopUp.Show();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("Aceptar");
            if (!this.Page.IsValid)
                return;

            TGETiposCambios tipoCambio=new TGETiposCambios();
            tipoCambio.Pais.IdPais=Convert.ToInt32(this.ddlPaises.SelectedValue);
            tipoCambio.Moneda.IdMoneda=Convert.ToInt32(this.ddlMonedas.SelectedValue);
            tipoCambio.FechaDesde = DateTime.Now;
            tipoCambio.TipoCambio = this.txtTipoCambio.GetCurrencyAmount();
            tipoCambio.Estado.IdEstado = (int)Estados.Activo;

            if (TGEGeneralesF.TGETiposCambiosAgregar(tipoCambio))
            {
                this.MostrarMensaje(tipoCambio.CodigoMensaje, false, tipoCambio.CodigoMensajeArgs);
                this.mpePopUp.Hide();
                if (this.TiposCambiosDatosAceptar != null)
                    this.TiposCambiosDatosAceptar(sender, tipoCambio);
            }
            else
            {
                this.MostrarMensaje(tipoCambio.CodigoMensaje, true, tipoCambio.CodigoMensajeArgs);
                this.mpePopUp.Show();
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.mpePopUp.Hide();
            if (this.TiposCambiosDatosCancelar != null)
                this.TiposCambiosDatosCancelar();
        }

        private void CargarCombos()
        {
            this.ddlPaises.DataSource = TGEGeneralesF.TGEPaisesObtenerLista();
            this.ddlPaises.DataValueField = "IdPais";
            this.ddlPaises.DataTextField = "Pais";
            this.ddlPaises.DataBind();
            this.ddlPaises.SelectedValue = "2";

            this.ddlMonedas.DataSource = TGEGeneralesF.MonedasObtenerLista();
            this.ddlMonedas.DataValueField = "IdMoneda";
            this.ddlMonedas.DataTextField = "miMonedaDescripcion";
            this.ddlMonedas.DataBind();
        }
    }
}