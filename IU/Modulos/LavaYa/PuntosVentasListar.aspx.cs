using Comunes.Entidades;
using LavaYa;
using LavaYa.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.LavaYa
{
    public partial class PuntosVentasListar : PaginaSegura
    {
        private DataTable MisPuntosVentas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PuntosVentasListarMisPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "PuntosVentasListarMisPuntosVentas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;

            if (!this.IsPostBack)
            {
                LavPuntosVentas parametros = this.BusquedaParametrosObtenerValor<LavPuntosVentas>();
                if (parametros.BusquedaParametros)
                {
                    txtDescripcion.Text = parametros.Descripcion;
                    CargarLista(parametros);
                }
            }

        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            LavPuntosVentas parametros = this.BusquedaParametrosObtenerValor<LavPuntosVentas>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            LavPuntosVentas parametros = this.BusquedaParametrosObtenerValor<LavPuntosVentas>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/PuntosVentasAgregar.aspx"), true);
        }

        #region GV

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idPuntoVenta = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPuntoVenta", idPuntoVenta);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/PuntosVentasModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/PuntosVentasConsultar.aspx"), true);

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("PuntosVentasModificar.aspx");
                consultar.Visible = this.ValidarPermiso("PuntosVentasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPuntosVentas.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LavPuntosVentas parametros = BusquedaParametrosObtenerValor<LavPuntosVentas>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisPuntosVentas = OrdenarGrillaDatos<DataTable>(MisPuntosVentas, e);
            gvDatos.DataSource = MisPuntosVentas;
            gvDatos.DataBind();
        }

        #endregion
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            gvDatos.AllowPaging = false;
            gvDatos.DataSource = MisPuntosVentas;
            gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", gvDatos);
        }
        private void CargarLista(LavPuntosVentas pParametro)
        {
            pParametro.Descripcion = txtDescripcion.Text;
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<LavPuntosVentas>(pParametro);

            this.MisPuntosVentas = PuntosVentasF.PuntosVentasObtenerListaGrilla(pParametro);
            this.gvDatos.DataSource = this.MisPuntosVentas;
            this.gvDatos.VirtualItemCount = MisPuntosVentas.Rows.Count > 0 ? Convert.ToInt32(MisPuntosVentas.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisPuntosVentas.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}