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
    public partial class MailingListar : PaginaSegura
    {
        //private DataTable MisMailing
        //{
        //    get { return (DataTable)Session[this.MiSessionPagina + "MailingListarMisMailing"]; }
        //    set { Session[this.MiSessionPagina + "MailingListarMisMailing"] = value; }
        //}

        private List<TGEMailing> MisMailing
        {
            get { return (List<TGEMailing>)Session[this.MiSessionPagina + "MailingListarMisMailing"]; }
            set { Session[this.MiSessionPagina + "MailingListarMisMailing"] = value; }
        }

        private List<TGEMailingProcesos> MisMailingProcesos
        {
            get { return (List<TGEMailingProcesos>)Session[this.MiSessionPagina + "MailingListarMisMailingProcesos"]; }
            set { Session[this.MiSessionPagina + "MailingListarMisMailingProcesos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                this.CargarLista();
            }

        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            TGEMailing mailing = new TGEMailing();
            mailing.IdMailing = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdMailing"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdMailing", mailing.IdMailing);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingModificar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ImageButton baja = (ImageButton)e.Row.FindControl("btnBaja");

                ////Permisos btnEliminar

                //ibtnConsultar.Visible = this.ValidarPermiso("MailingConsultar.aspx");
                //DataRowView dr = (DataRowView)e.Row.DataItem;

                //switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                //{
                //    case (int)EstadosRemitos.PendienteEntrega:
                //        break;
                //    case (int)EstadosRemitos.EnDistribucion:
                //        baja.Visible = this.ValidarPermiso("RemitosAnular.aspx");
                //        modificar.Visible = this.ValidarPermiso("RemitosModificar.aspx");
                //        break;
                //    default:
                //        break;
                //}
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisMailing.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        private void CargarLista()
        {
            this.MisMailing = MailingF.TGEMailingSeleccionarGrilla();
            AyudaProgramacion.CargarGrillaListas<TGEMailing>(this.MisMailing, false, this.gvDatos, true);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingAgregar.aspx"), true);
        }

    }
}