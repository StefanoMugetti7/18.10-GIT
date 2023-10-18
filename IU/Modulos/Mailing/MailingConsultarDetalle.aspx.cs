using Comunes.Entidades;
using Mailing;
using Mailing.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Mailing
{
    public partial class MailingConsultarDetalle : PaginaSegura
    {
        private DataTable DetalleEnvio
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MailingEnvioManualMisDatosMailing"]; }
            set { Session[this.MiSessionPagina + "MailingEnvioManualMisDatosMailing"] = value; }
        }
        private TGEMailing MiMailing
        {
            get { return (TGEMailing)Session[this.MiSessionPagina + "TGEMailingDatosMiMailing"]; }
            set { Session[this.MiSessionPagina + "TGEMailingDatosMiMailing"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (this.MisParametrosUrl.Contains("UrlReferrer"))
                this.viewStatePaginaSegura = this.MisParametrosUrl["UrlReferrer"].ToString();
            if (!IsPostBack)
            {
                TGEMailing pParametro = new TGEMailing();
                pParametro.MailingProcesamiento.IdMailingProcesamiento = Convert.ToInt32(MisParametrosUrl["IdMailingProcesamiento"]);
                pParametro.IdMailing = Convert.ToInt32(MisParametrosUrl["IdMailing"]);
                this.CargarLista(pParametro);
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGEMailing parametros = this.BusquedaParametrosObtenerValor<TGEMailing>();
            this.CargarLista(parametros);
        }
         protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdMailing", this.MiMailing.IdMailing);
            this.MisParametrosUrl.Add("IdMailingProcesamiento", MiMailing.MailingProcesamiento.IdMailingProcesamiento);
            if (!string.IsNullOrEmpty(this.viewStatePaginaSegura))
                this.MisParametrosUrl.Add("UrlReferrer", this.viewStatePaginaSegura.ToString());
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingListar.aspx"), true);

        }
        private void CargarLista(TGEMailing pParametro)
        {
            pParametro.MailingProcesamiento.IdMailingProcesamiento = MiMailing.MailingProcesamiento.IdMailingProcesamiento;
            pParametro.Filtro = txtFiltro.Text;
            
            this.DetalleEnvio = MailingF.TGEMailingObtenerDatosCompletosFiltroDT(pParametro);
            this.gvDetalleEnvio.DataSource = DetalleEnvio;
            this.gvDetalleEnvio.DataBind();
          

        }




        protected void gvDetalleEnvio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;


            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdMailingProcesamiento", MiMailing.MailingProcesamiento.IdMailingProcesamiento);
            this.MisParametrosUrl.Add("IdMailing", MiMailing.IdMailing);

        }
    }
}