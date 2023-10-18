using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Compras;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using ProcesosDatos;

namespace IU.Modulos.Compras.Controles
{
    public partial class OrdenesComprasTercerosBuscarPopUp : ControlesSeguros
    {
        private List<CmpOrdenesCompras> MisOrdenesCompras
        {
            get { return (List<CmpOrdenesCompras>)Session[this.MiSessionPagina + "OrdenesComprasBuscarPopUpMisOrdenesCompras"]; }
            set { Session[this.MiSessionPagina + "OrdenesComprasBuscarPopUpMisOrdenesCompras"] = value; }
        }

        private CmpOrdenesCompras MiOrdenCompra
        {
            get { return (CmpOrdenesCompras)Session[this.MiSessionPagina + "OrdenesComprasBuscarPopUpMiOrdenCompra"]; }
            set { Session[this.MiSessionPagina + "OrdenesComprasBuscarPopUpMiOrdenCompra"] = value; }
        }

        private List<CmpOrdenesCompras> MiListaIncluida
        {
            get { return (List<CmpOrdenesCompras>)Session[this.MiSessionPagina + "OrdenesComprasBuscarPopUpMiListaIncluida"]; }
            set { Session[this.MiSessionPagina + "OrdenesComprasBuscarPopUpMiListaIncluida"] = value; }
        }


        public delegate void OrdenesComprasBuscarEventHandler(List<CmpOrdenesCompras> e);
        public event OrdenesComprasBuscarEventHandler OrdenesComprasBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.MiOrdenCompra = new CmpOrdenesCompras();
                this.MiListaIncluida = new List<CmpOrdenesCompras>();
                this.MisOrdenesCompras = new List<CmpOrdenesCompras>();
            }
            else
                this.PersistirDatos();
        }

        public void IniciarControl(CmpOrdenesCompras pOrden, List<CmpOrdenesCompras> listaIncluida)
        {
            List<int> periodos = ProcesosDatosF.ProcesosProcesamientoObtenerUltimosPeriodosCargos(10);
            foreach (int per in periodos)
                this.ddlPeriodo.Items.Add(per.ToString());

            this.MiOrdenCompra = pOrden;
            this.MiListaIncluida = listaIncluida;

            this.CargarLista();
            this.mpePopUp.Show();
        }

        protected void ddlPeriodo_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.CargarLista();
            this.mpePopUp.Show();
        }

        private void CargarLista()
        {
            this.MiOrdenCompra.PeriodoPrimerVencimiento = Convert.ToInt32(this.ddlPeriodo.SelectedValue);
            this.MisOrdenesCompras = ComprasF.OrdenCompraObtenerTercerosPendientes(this.MiOrdenCompra);
            this.MisOrdenesCompras = this.MisOrdenesCompras.Where(p => !this.MiListaIncluida.Any(x => p.IdOrdenCompra == x.IdOrdenCompra)).ToList();
            AyudaProgramacion.CargarGrillaListas<CmpOrdenesCompras>(this.MisOrdenesCompras, false, this.gvDatos, false);
        }

        private void PersistirDatos()
        {
            if (this.MisOrdenesCompras.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                    this.MisOrdenesCompras[fila.DataItemIndex].Check=chkIncluir.Checked;
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.OrdenesComprasBuscarSeleccionar != null)
            {
                this.OrdenesComprasBuscarSeleccionar(AyudaProgramacion.AcomodarIndices<CmpOrdenesCompras>(this.MisOrdenesCompras.Where(x=>x.Check).ToList()));
                this.mpePopUp.Hide();
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (!(e.CommandName == Gestion.Consultar.ToString()
            //    ))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //CmpOrdenesCompras orden = new CmpOrdenesCompras();
            // orden = this.MisOrdenesCompras[indiceColeccion];
            // orden = ComprasF.OrdenCompraObtenerDatosCompletos(orden);
            //if (e.CommandName == Gestion.Consultar.ToString())
            //{

            //    if (this.OrdenesComprasBuscarSeleccionar != null)
            //    {
            //        this.OrdenesComprasBuscarSeleccionar(orden);
            //        this.mpePopUp.Hide();
            //    }
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");

            //    //Permisos btnEliminar
            //    ibtnConsultar.Visible = this.ValidarPermiso("AfiliadosConsultar.aspx");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisOrdenesCompras.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisOrdenesCompras;
            gvDatos.DataBind();

            this.mpePopUp.Show();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisOrdenesCompras = this.OrdenarGrillaDatos<CmpOrdenesCompras>(this.MisOrdenesCompras, e);
            this.gvDatos.DataSource = this.MisOrdenesCompras;
            this.gvDatos.DataBind();

            this.mpePopUp.Show();
        }
    }
}