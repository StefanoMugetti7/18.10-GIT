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
using Afiliados.Entidades;
using System.Collections.Generic;
using Generales.FachadaNegocio;
using Afiliados;
using Comunes.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class CategoriasListar : PaginaSegura
    {
        private List<AfiCategorias> MisCategorias
        {
            get { return (List<AfiCategorias>)Session[this.MiSessionPagina + "CategoriasListarMisCategorias"]; }
            set { Session[this.MiSessionPagina + "CategoriasListarMisCategorias"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("CategoriasAgregar.aspx");
                this.CargarCombos();
                AfiCategorias parametros = this.BusquedaParametrosObtenerValor<AfiCategorias>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCategoria.Text = parametros.Categoria;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiCategorias parametros = this.BusquedaParametrosObtenerValor<AfiCategorias>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CategoriasAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AfiCategorias categoria = this.MisCategorias[indiceColeccion];
            
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdCategoria", categoria.IdCategoria);
            //string parametros = string.Format("?IdCategoria={0}", categoria.IdCategoria);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CategoriasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CategoriasConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCategorias.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiCategorias parametros = this.BusquedaParametrosObtenerValor<AfiCategorias>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiCategorias>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCategorias;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCategorias = this.OrdenarGrillaDatos<AfiCategorias>(this.MisCategorias, e);
            this.gvDatos.DataSource = this.MisCategorias;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";

            this.ddlTipoCategoria.DataSource = AfiliadosF.CategoriasObtenerTiposCategoria();
            this.ddlTipoCategoria.DataValueField = "IdTipoCategoria";
            this.ddlTipoCategoria.DataTextField = "TipoCategoria";
            this.ddlTipoCategoria.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlTipoCategoria, ObtenerMensajeSistema("SeleccioneOpcion"));
            //DEBO LLENAR EL COMBO
        }

        private void CargarLista(AfiCategorias pCategoria)
        {
            pCategoria.Categoria = this.txtCategoria.Text.Trim();
            pCategoria.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pCategoria.BusquedaParametros = true;
            pCategoria.IdTipoCategoria = this.ddlTipoCategoria.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlTipoCategoria.SelectedValue);
            this.BusquedaParametrosGuardarValor<AfiCategorias>(pCategoria);
            this.MisCategorias = AfiliadosF.CategoriasObtenerListaFiltro(pCategoria);
            this.gvDatos.DataSource = this.MisCategorias;
            this.gvDatos.PageIndex = pCategoria.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
