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
    public partial class MailingProcesamientosListar : PaginaSegura
    {

        private TGEMailing MiMailing
        {
            get { return (TGEMailing)Session[this.MiSessionPagina + "TGEMailingDatosMiMailing"]; }
            set { Session[this.MiSessionPagina + "TGEMailingDatosMiMailing"] = value; }
        }
        private DataTable DetalleEnvioProcesamiento
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarArchivoDatos"]; }
            set { Session[this.MiSessionPagina + "ImportarArchivoDatos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
           
                this.ddlProceso.DataSource = MailingF.TGEMailingObtenerListaMailingProceso();
                this.ddlProceso.DataValueField = "IdMailingProceso";
                this.ddlProceso.DataTextField = "Descripcion";
                this.ddlProceso.DataBind();
                if (this.ddlProceso.Items.Count != 1)
                    AyudaProgramacion.InsertarItemSeleccione(this.ddlProceso, this.ObtenerMensajeSistema("SeleccioneOpcion"));


                ddlProceso_OnSelectedIndexChanged(null, EventArgs.Empty);

                TGEMailing parametros = this.BusquedaParametrosObtenerValor<TGEMailing>();
                
                if (this.MisParametrosUrl.Contains("IdMailing"))
                {
                    this.gvDetalleEnvio.PageIndex = parametros.IndiceColeccion;
                    gvDetalleEnvio.DataSource = MiMailing.DetalleEnvioProcesamiento;
                    gvDetalleEnvio.DataBind();
                    gvDetalleEnvio.Visible = true;
                }
                
               

            }
        }

        protected void ddlProceso_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(ddlProceso.SelectedValue))
            {
                MiMailing = new TGEMailing();
                MiMailing.IdMailing = Convert.ToInt32(ddlProceso.SelectedValue);
                MiMailing = MailingF.TGEMailingObtenerDatosCompletos(MiMailing);
                gvDetalleEnvio.DataSource = MiMailing.DetalleEnvioProcesamiento;
                gvDetalleEnvio.DataBind();
                gvDetalleEnvio.Visible = true;

            }
            else
            {
                MiMailing = new TGEMailing();
              
                MiMailing = MailingF.TGEMailingObtenerDatosCompletos(MiMailing);
                gvDetalleEnvio.DataSource = MiMailing.DetalleEnvioProcesamiento;
                gvDetalleEnvio.DataBind();
                gvDetalleEnvio.Visible = true;
            }



        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {

            if (this.MisParametrosUrl.Contains("IdMailing"))
            {
                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdMailingProcesamiento", MiMailing.MailingProcesamiento.IdMailingProcesamiento);
            }

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingProcesamientosEnvios.aspx"), true);


        }
      

        protected void gvDetalleEnvio_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdMailing = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //VTAPresupuestos factura = this.MisDatosGrillas[id];



            if (e.CommandName == Gestion.Consultar.ToString())
            {
                int index1 = Convert.ToInt32(e.CommandArgument);
                int IdMailingProcesamientoFiltro = Convert.ToInt32(((GridView)sender).DataKeys[index1].Value.ToString());
         
                MiMailing.MailingProcesamiento.IdMailingProcesamiento = IdMailingProcesamientoFiltro;
                this.MisParametrosUrl = new Hashtable();
               
                    this.MisParametrosUrl.Add("IdMailingProcesamiento", MiMailing.MailingProcesamiento.IdMailingProcesamiento);

                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingConsultarDetalle.aspx"), true);
      

            }


        }
    }
}