using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;

namespace IU.Modulos.TGE
{
    public partial class SectoresListar : PaginaSegura
    {
        private List<TGESectores> MiSector
        {
            get { return (List<TGESectores>)Session[this.MiSessionPagina + "SectoresListarMiSector"]; }
            set { Session[this.MiSessionPagina + "SectoresListarMiSector"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                TGESectores parametros = this.BusquedaParametrosObtenerValor<TGESectores>();
                if (parametros.BusquedaParametros)
                {
                    
                    this.txtSector.Text = parametros.Sector;
                    this.ddlFilial.SelectedValue = parametros.Filial.IdFilial.ToString();
                    //this.ddlFiliales.SelectedValue = parametros.IdFilial == 0 ? string.Empty : parametros.IdFilial.ToString();
                    //this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        private void CargarCombos()
        {
            this.ddlFilial.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            if (this.ddlFilial.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGESectores parametros = this.BusquedaParametrosObtenerValor<TGESectores>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/SectoresAgregar.aspx"), true);
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGESectores sector = this.MiSector[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdSector", sector.IdSector);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/SectoresModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/SectoresConsultar.aspx"), true);
            }

        }


        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGESectores sector = (TGESectores)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("SectoresModificar.aspx");
                consultar.Visible = this.ValidarPermiso("SectoresConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiSector.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGESectores parametros = this.BusquedaParametrosObtenerValor<TGESectores>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGESectores>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<TGESectores>(this.MiSector, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiSector = this.OrdenarGrillaDatos<TGESectores>(this.MiSector, e);
            AyudaProgramacion.CargarGrillaListas<TGESectores>(this.MiSector, false, this.gvDatos, true);
        }
        #endregion

        private void CargarLista(TGESectores pParametro)
        {
            pParametro.Filial.IdFilial = this.ddlFilial.SelectedValue == string.Empty ? -1 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pParametro.Sector = this.txtSector.Text;
            //pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            //pParametro.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TGESectores>(pParametro);
            this.MiSector = TGEGeneralesF.SectoresObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<TGESectores>(this.MiSector, false, this.gvDatos, true);
        }
    }
}
