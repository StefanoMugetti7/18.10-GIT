using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Facturas;
using SKP.ASP.Controls;
using System.Data;

namespace IU.Modulos.Facturas.Controles
{
    public partial class NotasPedidosBuscarPopUp : ControlesSeguros
    {
        private DataTable MisNotasPedidos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "NotasPedidosBuscarPopUpMisNotasPedidos"]; }
            set { Session[this.MiSessionPagina + "NotasPedidosBuscarPopUpMisNotasPedidos"] = value; }
        }

        private VTANotasPedidos MiNotaPedido
        {
            get { return (VTANotasPedidos)Session[this.MiSessionPagina + "NotasPedidosBuscarPopUpMiNotaPedido"]; }
            set { Session[this.MiSessionPagina + "NotasPedidosBuscarPopUpMiNotaPedido"] = value; }
        }

        private List<VTANotasPedidosDetalles> MisDetalles
        {
            get { return (List<VTANotasPedidosDetalles>)Session[this.MiSessionPagina + "NotasPedidosListarMisDetalles"]; }
            set { Session[this.MiSessionPagina + "NotasPedidosListarMisDetalles"] = value; }
        }

        public delegate void NotasPedidosBuscarEventHandler(VTANotasPedidos e);
        public event NotasPedidosBuscarEventHandler NotasPedidosBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }

        public void IniciarControl(VTANotasPedidos pParametro)
        {
            MiNotaPedido = pParametro;
            CargarLista();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarNotaPedido();", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CargarLista();
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTANotasPedidos NotaPedido = new VTANotasPedidos();
            NotaPedido.IdNotaPedido = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            NotaPedido = FacturasF.NotasPedidosObtenerDatosCompletos(NotaPedido);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.NotasPedidosBuscarSeleccionar != null)
                {
                    this.NotasPedidosBuscarSeleccionar(NotaPedido);
                }
            }
        }

        protected void gvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer) 
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDetalles.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProductos.PageIndex = e.NewPageIndex;
            gvProductos.DataSource = this.MisDetalles;
            gvProductos.DataBind();
        }

        protected void gvProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisDetalles = this.OrdenarGrillaDatos<VTANotasPedidosDetalles>(this.MisDetalles, e);
            this.gvProductos.DataSource = this.MisDetalles;
            this.gvProductos.DataBind();
        }

        private void CargarLista()
        {
            MiNotaPedido.Estado.IdEstado = (int)Estados.Activo;
            MiNotaPedido.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MiNotaPedido.Descripcion = txtDescripcion.Text;
            this.MisDetalles = FacturasF.NotasPedidosObtenerListaFiltroPopUp(MiNotaPedido);
            this.gvProductos.DataSource = this.MisDetalles;
            this.gvProductos.DataBind();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.MapearChecked();
            if (this.MiNotaPedido.NotasPedidosDetalles.Count > 0)
            {
                if (this.NotasPedidosBuscarSeleccionar != null)
                {
                    this.NotasPedidosBuscarSeleccionar(this.MiNotaPedido);
                }
            }
        }

        private void MapearChecked()
        {
            this.MiNotaPedido = new VTANotasPedidos();
            this.MiNotaPedido.NotasPedidosDetalles.AddRange(this.MisDetalles.Where(x => x.Incluir).ToList());
            this.MiNotaPedido.NotasPedidosDetalles = AyudaProgramacion.AcomodarIndices<VTANotasPedidosDetalles>(this.MiNotaPedido.NotasPedidosDetalles);
        }

        private void PersistirDatosGrilla()
        {
            foreach (GridViewRow fila in this.gvProductos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkIncluir = ((CheckBox)fila.FindControl("chkIncluir"));
                    this.MisDetalles[fila.DataItemIndex].Incluir = chkIncluir.Checked;
                }
            }
        }
    }
}
