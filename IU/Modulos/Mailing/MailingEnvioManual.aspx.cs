using Comunes.Entidades;
using IU.Modulos.ProcesosDatos.Controles;
using Mailing;
using Mailing.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace IU.Modulos.Mailing
{
    public partial class MailingEnvioManual : PaginaSegura
    {
        private DataTable MisDatosMailing
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MailingEnvioManualMisDatosMailing"]; }
            set { Session[this.MiSessionPagina + "MailingEnvioManualMisDatosMailing"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!IsPostBack)
            {
                TGEMailing pParametro = new TGEMailing();
                pParametro.MailingProcesamiento.IdMailingProcesamiento = Convert.ToInt32(MisParametrosUrl["IdMailingProcesamiento"]);
                this.CargarLista(pParametro);
            }
        }

        protected void gvDatos_RowCreated(object sender, GridViewRowEventArgs e)
        {

            GridViewRow row = e.Row;
            // Intitialize TableCell list
            List<TableCell> columns = new List<TableCell>();
            foreach (DataControlField column in gvDatos.Columns)
            {
                //Get the first Cell /Column
                TableCell cell = row.Cells[0];
                // Then Remove it after
                row.Cells.Remove(cell);
                //And Add it to the List Collections
                columns.Add(cell);
            }

            // Add cells
            row.Cells.AddRange(columns.ToArray());
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkIncluir = (CheckBox)e.Row.FindControl("chkIncluir");
                chkIncluir.Checked = true;
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            AudMailsEnvios mailsEnvios = new AudMailsEnvios();
            mailsEnvios.IdMailEnvio = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdMailEnvio"].ToString());

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                ctrVistaPrevia.IniciarControl(mailsEnvios);
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //if (!this.Page.IsValid)
            //    return;
            this.btnAceptar.Visible = false;

            TGEMailing mailingEnvioManual = new TGEMailing();

            mailingEnvioManual.MailingProcesamiento.IdMailingProcesamiento = Convert.ToInt32(MisParametrosUrl["IdMailingProcesamiento"]);

            //mailingEnvioManual.LoteMailingEnvioManual = new XmlDocument();
            //XmlNode mailingNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("MailingEnvioManual");
            //mailingEnvioManual.LoteMailingEnvioManual.AppendChild(mailingNode);
            //DataTable enviar = MisDatosMailing.Copy();
            
            int idTable;
            DataRow dr;
            foreach (GridViewRow pre in this.gvDatos.Rows)
            {
                if (pre.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkIncluir = (CheckBox)pre.FindControl("chkIncluir");
                    if (!chkIncluir.Checked)
                    {
                        idTable = Convert.ToInt32(this.gvDatos.DataKeys[pre.DataItemIndex]["IdMailEnvio"].ToString());
                        dr = this.MisDatosMailing.AsEnumerable().First(x => x.Field<Int32>("IdMailEnvio") == idTable);
                        //dr.Delete();
                        MisDatosMailing.Rows.Remove(dr);
                        MisDatosMailing.AcceptChanges();
                    }
                }
            }

            #region Grilla
            //XmlNode mNode;
            //XmlNode ValorNode;
            //int cantidad = 0;
            //            cantidad++;
            //            idTable = Convert.ToInt32(this.gvDatos.DataKeys[pre.DataItemIndex]["IdMailEnvio"].ToString());
            //            dr = this.MisDatosMailing.AsEnumerable().First(x => x.Field<Int32>("IdMailEnvio") == idTable);
            //            mNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("EnvioManual");

            //            ValorNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("Para");
            //            ValorNode.InnerText = dr["Para"].ToString();
            //            mNode.AppendChild(ValorNode);
            //            ValorNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("Nombre");
            //            ValorNode.InnerText = dr["Nombre"].ToString();
            //            mNode.AppendChild(ValorNode);
            //            ValorNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("Asunto");
            //            ValorNode.InnerText = dr["Asunto"].ToString();
            //            mNode.AppendChild(ValorNode);
            //            ValorNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("Cuerpo");
            //            ValorNode.InnerText = dr["Cuerpo"].ToString();
            //            mNode.AppendChild(ValorNode);

            //            mailingNode.AppendChild(mNode);
            //        }
            //    }
            //}
            //if (cantidad == 0)
            //{
            //    this.btnAceptar.Visible = true;
            //    this.MostrarMensaje("ValidarCantidadItems", true);
            //    return;
            //}
            #endregion

            guardo = MailingF.TGEMailingEnviarMailsSeleccionados(MisDatosMailing, mailingEnvioManual);

            if (guardo)
            {
                this.MostrarMensaje(mailingEnvioManual.CodigoMensaje, false, mailingEnvioManual.CodigoMensajeArgs);
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(mailingEnvioManual.CodigoMensaje, true, mailingEnvioManual.CodigoMensajeArgs);

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingModificar.aspx"), true);
        }

        private void CargarLista(TGEMailing pParametro)
        {
            MisDatosMailing = MailingF.TGEMailingObtenerMailsAEnviar(pParametro);
            this.gvDatos.DataSource = this.MisDatosMailing;
            this.gvDatos.DataBind();
        }
      
    }
}