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
    public partial class PresupuestosBuscarPopUp : ControlesSeguros
    {
        private DataTable MisPresupuestos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PresupuestosBuscarPopUpMisPresupuestos"]; }
            set { Session[this.MiSessionPagina + "PresupuestosBuscarPopUpMisPresupuestos"] = value; }
        }

        private VTAPresupuestos MiPresupuesto
        {
            get { return (VTAPresupuestos)Session[this.MiSessionPagina + "PresupuestosBuscarPopUpMiPresupuesto"]; }
            set { Session[this.MiSessionPagina + "PresupuestosBuscarPopUpMiPresupuesto"] = value; }
        }

        public delegate void PresupuestosBuscarEventHandler(VTAPresupuestos e);
        public event PresupuestosBuscarEventHandler PresupuestosBuscarSeleccionar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroProducto, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
            }
        }

        public void IniciarControl(VTAPresupuestos pParametro)
        {
            MiPresupuesto = pParametro;
            CargarLista();
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModal", "ShowModalBuscarPresupuesto();", true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            CargarLista();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModaBuscarProducto();", true);
        }

        protected void gvProductos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTAPresupuestos presupuesto = new VTAPresupuestos();
            presupuesto.IdPresupuesto = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            presupuesto = FacturasF.PresupuestosObtenerDatosCompletos(presupuesto);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.PresupuestosBuscarSeleccionar != null)
                {
                    this.PresupuestosBuscarSeleccionar(presupuesto);
                }
            }
        }

        protected void gvProductos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");

            //    //Permisos btnEliminar
            //    ibtnConsultar.Visible = this.ValidarPermiso("ProductosConsultar.aspx");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPresupuestos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvProductos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //CMPProductos parametros = this.BusquedaParametrosObtenerValor<CMPProductos>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<CMPProductos>(parametros);
            gvProductos.PageIndex = e.NewPageIndex;
            gvProductos.DataSource = this.MisPresupuestos;
            gvProductos.DataBind();

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalBuscarProducto();", true);
        }

        protected void gvProductos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPresupuestos = this.OrdenarGrillaDatos<DataTable>(this.MisPresupuestos, e);
            this.gvProductos.DataSource = this.MisPresupuestos;
            this.gvProductos.DataBind();
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModal", "ShowModalBuscarProducto();", true);
        }

        private void CargarLista()
        {
            MiPresupuesto.Estado.IdEstado = (int)Estados.Activo;
            MiPresupuesto.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MiPresupuesto.IdPresupuesto = txtNumeroProducto.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumeroProducto.Text);
            MiPresupuesto.Descripcion = txtDescripcion.Text;
            this.MisPresupuestos = FacturasF.PresupuestosObtenerListaGrilla(MiPresupuesto);
            //this.gvProductos.PageIndex = MiPresupuesto.IndiceColeccion;
            this.gvProductos.DataSource = this.MisPresupuestos;
            this.gvProductos.DataBind();
        }
    }
}
