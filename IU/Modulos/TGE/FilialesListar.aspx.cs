using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using System.Collections;

namespace IU.Modulos.TGE
{
    public partial class FilialesListar : PaginaSegura
    {
        private List<TGEFiliales> MiFilial
        {
            get { return (List<TGEFiliales>)Session[this.MiSessionPagina + "FilialesListarMiFilial"]; }
            set { Session[this.MiSessionPagina + "FilialesListarMiFilial"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                TGEFiliales parametros = this.BusquedaParametrosObtenerValor<TGEFiliales>();
                if (parametros.BusquedaParametros)
                {
                    this.txtFilial.Text = parametros.Filial;
                    this.txtDescripcion.Text = parametros.Descripcion;
                    //this.ddlFiliales.SelectedValue = parametros.IdFilial == 0 ? string.Empty : parametros.IdFilial.ToString();
                    //this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        private void CargarCombos()
        {
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGEFiliales parametros = this.BusquedaParametrosObtenerValor<TGEFiliales>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FilialesAgregar.aspx"), true);
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEFiliales filial = this.MiFilial[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdFilial", filial.IdFilial);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FilialesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FilialesConsultar.aspx"), true);
            }

        }


        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGEFiliales filial = (TGEFiliales)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("FilialesModificar.aspx");
                consultar.Visible = this.ValidarPermiso("FilialesConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiFilial.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEFiliales parametros = this.BusquedaParametrosObtenerValor<TGEFiliales>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEFiliales>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<TGEFiliales>(this.MiFilial, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiFilial = this.OrdenarGrillaDatos<TGEFiliales>(this.MiFilial, e);
            AyudaProgramacion.CargarGrillaListas<TGEFiliales>(this.MiFilial, false, this.gvDatos, true);
        }
        #endregion

        private void CargarLista(TGEFiliales pParametro)
        {
            pParametro.Filial = this.txtFilial.Text;
            pParametro.Descripcion = this.txtDescripcion.Text;
            //pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            //pParametro.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TGEFiliales>(pParametro);
            this.MiFilial = TGEGeneralesF.FilialesObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<TGEFiliales>(this.MiFilial, false, this.gvDatos, true);
        }
    }
}
