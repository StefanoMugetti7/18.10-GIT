using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Generales.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using System.Collections.Generic;

namespace IU.Modulos.TGE
{
    public partial class ListasValoresListar : PaginaSegura
    {
        private List<TGEListasValores> MisListasValores
        {
            get { return (List<TGEListasValores>)Session[this.MiSessionPagina + "ListasValoresListarMisListasValores"]; }
            set { Session[this.MiSessionPagina + "ListasValoresListarMisListasValores"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtParametro, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("ListasValoresAgregar.aspx");
                TGEListasValores parametros = this.BusquedaParametrosObtenerValor<TGEListasValores>();
                if (parametros.BusquedaParametros)
                {
                    this.txtParametro.Text = parametros.ListaValor;
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGEListasValores parametros = this.BusquedaParametrosObtenerValor<TGEListasValores>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ListasValoresAgregar.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEListasValores param = this.MisListasValores[indiceColeccion];
            this.MisParametrosUrl = new Hashtable
            {
                { "IdListaValor", param.IdListaValor }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ListasValoresModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ListasValoresConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Permisos btnConsultar
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = this.ValidarPermiso("ListasValoresConsultar.aspx");
                //Permisos btnConsultar
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnModificar.Visible = this.ValidarPermiso("ListasValoresModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisListasValores.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEListasValores parametros = this.BusquedaParametrosObtenerValor<TGEListasValores>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEListasValores>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisListasValores;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisListasValores = this.OrdenarGrillaDatos<TGEListasValores>(this.MisListasValores, e);
            this.gvDatos.DataSource = this.MisListasValores;
            this.gvDatos.DataBind();
        }
        private void CargarLista(TGEListasValores pParametro)
        {
            pParametro.BusquedaParametros = true;
            pParametro.ListaValor = this.txtParametro.Text;
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.BusquedaParametrosGuardarValor<TGEListasValores>(pParametro);
            this.MisListasValores = TGEGeneralesF.ListasValoresObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisListasValores;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
