using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;

namespace IU.Modulos.TGE
{
    public partial class EmpresasListar : PaginaSegura
    {
        private List<TGEEmpresas> MiEmpresa
        {
            get { return (List<TGEEmpresas>)Session[this.MiSessionPagina + "EmpresasListarMiEmpresa"]; }
            set { Session[this.MiSessionPagina + "EmpresasListarMiEmpresa"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                TGEEmpresas parametros = this.BusquedaParametrosObtenerValor<TGEEmpresas>();
                this.btnAgregar.Visible = this.ValidarPermiso("EmpresasAgregar.aspx");
                if (parametros.BusquedaParametros)
                {
                    this.txtEmpresa.Text = parametros.Empresa;
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
            TGEEmpresas parametros = this.BusquedaParametrosObtenerValor<TGEEmpresas>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/EmpresasAgregar.aspx"), true);
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEEmpresas empresa= this.MiEmpresa[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEmpresa", empresa.IdEmpresa);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/EmpresasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/EmpresasConsultar.aspx"), true);
            }

        }


        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGEEmpresas empresa = (TGEEmpresas)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("EmpresasModificar.aspx");
                consultar.Visible = this.ValidarPermiso("EmpresasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiEmpresa.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEEmpresas parametros = this.BusquedaParametrosObtenerValor<TGEEmpresas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEEmpresas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<TGEEmpresas>(this.MiEmpresa, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiEmpresa = this.OrdenarGrillaDatos<TGEEmpresas>(this.MiEmpresa, e);
            AyudaProgramacion.CargarGrillaListas<TGEEmpresas>(this.MiEmpresa, false, this.gvDatos, true);
        }
        #endregion

        private void CargarLista(TGEEmpresas pParametro)
        {
            pParametro.Empresa = this.txtEmpresa.Text;
            pParametro.Descripcion = this.txtDescripcion.Text;
            //pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            //pParametro.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TGEEmpresas>(pParametro);
            this.MiEmpresa = TGEGeneralesF.EmpresaObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<TGEEmpresas>(this.MiEmpresa, false, this.gvDatos, true);
        }
    }
}