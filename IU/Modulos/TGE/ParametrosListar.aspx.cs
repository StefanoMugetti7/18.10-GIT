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
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.TGE
{
    public partial class ParametrosListar : PaginaSegura
    {
        private List<TGEParametros> MisParametros
        {
            get { return (List<TGEParametros>)Session[this.MiSessionPagina + "ParametrosListarMisParametros"]; }
            set { Session[this.MiSessionPagina + "ParametrosListarMisParametros"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.ddlModulos.DataSource = TGEGeneralesF.ModulosSistemaObtenerLista();
                this.ddlModulos.DataTextField = "ModuloSistema";
                this.ddlModulos.DataValueField = "IdModuloSistema";
                this.ddlModulos.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlModulos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                //this.btnAgregar.Visible = this.ValidarPermiso("RequisicionesAgregar.aspx");
                TGEParametros parametros = this.BusquedaParametrosObtenerValor<TGEParametros>();
                if (parametros.BusquedaParametros)
                {
                    this.txtParametro.Text = parametros.NombreParametro;
                    this.ddlModulos.SelectedValue = parametros.ModuloSistema.IdModuloSistema.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGEParametros parametros = this.BusquedaParametrosObtenerValor<TGEParametros>();
            this.CargarLista(parametros);
        }

        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ParametrosAgregar.aspx"), true);
        //}

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName==Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEParametros param = this.MisParametros[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdParametro", param.IdParametro);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                string url = "~/Modulos/TGE/ParametrosModificar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                string url = "~/Modulos/TGE/ParametrosConsultar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Permisos btnConsultar
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = this.ValidarPermiso("ParametrosConsultar.aspx");
                //Permisos btnConsultar
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnModificar.Visible = this.ValidarPermiso("ParametrosModificar.aspx");  
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisParametros.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEParametros parametros = this.BusquedaParametrosObtenerValor<TGEParametros>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEParametros>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisParametros;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisParametros = this.OrdenarGrillaDatos<TGEParametros>(this.MisParametros, e);
            this.gvDatos.DataSource = this.MisParametros;
            this.gvDatos.DataBind();
        }

        private void CargarLista(TGEParametros pParametro)
        {

            pParametro.BusquedaParametros = true;

            pParametro.NombreParametro = this.txtParametro.Text;
            pParametro.ModuloSistema.IdModuloSistema = this.ddlModulos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlModulos.SelectedValue);
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.BusquedaParametrosGuardarValor<TGEParametros>(pParametro);
            this.MisParametros = TGEGeneralesF.ParametrosObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisParametros;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
