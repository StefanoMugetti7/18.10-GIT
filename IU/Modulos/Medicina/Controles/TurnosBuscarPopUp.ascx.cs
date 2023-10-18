using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using Afiliados.Entidades;
using Medicina;

namespace IU.Modulos.Medicina.Controles
{
    public partial class TurnosBuscarPopUp : ControlesSeguros
    {
        private List<MedTurnos> MisTurnos
        {
            get { return (List<MedTurnos>)Session[this.MiSessionPagina + "TurnosBuscarPopUpMisTurnos"]; }
            set { Session[this.MiSessionPagina + "TurnosBuscarPopUpMisTurnos"] = value; }
        }

        public delegate void BuscarEventHandler(MedTurnos e);
        public event BuscarEventHandler BuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(MedTurnos pParametro)
        {
            this.MisTurnos = MedicinaF.TurnosObtenerListaFiltro(pParametro);
            AyudaProgramacion.CargarGrillaListas<MedTurnos>(this.MisTurnos, false, this.gvDatos, true);
            this.mpePopUp.Show();
        }

        //protected void btnCancelar_Click(object sender, EventArgs e)
        //{
        //    AyudaProgramacion.LimpiarControles(this.pnlPopUp, true);
        //    this.mpePopUp.Hide();
        //}

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            MedTurnos turno = this.MisTurnos[indiceColeccion];

            if (this.BuscarSeleccionar != null)
                this.BuscarSeleccionar(turno);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisTurnos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<MedTurnos>(this.MisTurnos, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisTurnos = this.OrdenarGrillaDatos<MedTurnos>(this.MisTurnos, e);
            AyudaProgramacion.CargarGrillaListas<MedTurnos>(this.MisTurnos, false, this.gvDatos, true);
        }
    }
}
