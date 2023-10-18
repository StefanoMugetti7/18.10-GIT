using Comunes.Entidades;
using Hoteles;
using Hoteles.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel
{
    public partial class ListaEsperaListar : PaginaSegura
    {
        private DataTable MiListaEspera
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ListaEsperaListarMiListaEspera"]; }
            set { Session[this.MiSessionPagina + "ListaEsperaListarMiListaEspera"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                HTLListaEspera parametros = this.BusquedaParametrosObtenerValor<HTLListaEspera>();
                if (parametros.BusquedaParametros)
                {
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            HTLListaEspera parametros = this.BusquedaParametrosObtenerValor<HTLListaEspera>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ListaEsperaAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            HTLListaEspera reserva = new HTLListaEspera();
            reserva.IdListaEspera = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdListaEspera"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdListaEspera", reserva.IdListaEspera);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/ListaEsperaModificar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                DataRowView dr = (DataRowView)e.Row.DataItem;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            HTLListaEspera parametros = this.BusquedaParametrosObtenerValor<HTLListaEspera>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<HTLListaEspera>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MiListaEspera;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiListaEspera = this.OrdenarGrillaDatos<DataTable>(this.MiListaEspera, e);
            this.gvDatos.DataSource = this.MiListaEspera;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MiListaEspera;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        private void CargarLista(HTLListaEspera pListaEspera)
        {
            pListaEspera.Afiliado.IdAfiliado = hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(hdfIdAfiliado.Value);
            pListaEspera.Producto.IdProducto = hdfIdProducto.Value == string.Empty ? 0 : Convert.ToInt32(hdfIdProducto.Value);
            //pListaEspera.Fecha = this.txtFechaIngreso.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaIngreso.Text);
            pListaEspera.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pListaEspera.BusquedaParametros = true;

            this.BusquedaParametrosGuardarValor<HTLListaEspera>(pListaEspera);
            this.MiListaEspera = HotelesF.ListaEsperaObtenerListaGrilla(pListaEspera);

            this.gvDatos.DataSource = this.MiListaEspera;
            this.gvDatos.PageIndex = pListaEspera.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MiListaEspera.Rows.Count > 0)
            {
                btnExportarExcel.Visible = true;
            }
            else
            {
                btnExportarExcel.Visible = false;
            }
        }
    }
}