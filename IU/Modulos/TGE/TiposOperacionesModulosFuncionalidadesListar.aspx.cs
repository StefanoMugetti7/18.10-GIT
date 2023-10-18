using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class TiposOperacionesModulosFuncionalidadesListar : PaginaSegura
    {
        private DataTable MisTiposOperacionesModulosFuncionalidades
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisTiposOperacionesModulosFuncionalidadesListarMisTiposOperaciones"]; }
            set { Session[this.MiSessionPagina + "MisTiposOperacionesModulosFuncionalidadesListarMisTiposOperaciones"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                this.btnAgregar.Visible = this.ValidarPermiso("TiposOperacionesModulosFuncionalidadesAgregar.aspx");
                TGETiposOperacionesModulosFuncionalidades parametros = this.BusquedaParametrosObtenerValor<TGETiposOperacionesModulosFuncionalidades>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlModuloSistema.SelectedValue = parametros.IdModuloSistema.ToString();
                    this.ddlTipoOperacion.SelectedValue = parametros.IdTipoOperacion.ToString();
                    this.ddlTipoFuncionalidad.SelectedValue = parametros.IdTipoFuncionalidad.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesModulosFuncionalidadesAgregar.aspx"), true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGETiposOperacionesModulosFuncionalidades parametros = this.BusquedaParametrosObtenerValor<TGETiposOperacionesModulosFuncionalidades>();
            this.CargarLista(parametros);
        }
        private void CargarCombos()
        {
            this.ddlTipoFuncionalidad.DataSource = TGEGeneralesF.TGETiposFuncionalidadesObtenerLista();
            this.ddlTipoFuncionalidad.DataValueField = "IdTipoFuncionalidad";
            this.ddlTipoFuncionalidad.DataTextField = "TipoFuncionalidad";
            this.ddlTipoFuncionalidad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFuncionalidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoOperacion.DataSource = TGEGeneralesF.TGETiposOperacionesObtenerLista();
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoOperacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlModuloSistema.DataSource = TGEGeneralesF.ModulosSistemaObtenerLista();
            this.ddlModuloSistema.DataValueField = "IdModuloSistema";
            this.ddlModuloSistema.DataTextField = "ModuloSistema";
            this.ddlModuloSistema.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlModuloSistema, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(TGETiposOperacionesModulosFuncionalidades parametros)
        {
            parametros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TGETiposOperacionesModulosFuncionalidades>(parametros);

            parametros.IdTipoOperacion = this.ddlTipoOperacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            parametros.IdModuloSistema = this.ddlModuloSistema.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlModuloSistema.SelectedValue);
            parametros.IdTipoFuncionalidad = this.ddlTipoFuncionalidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFuncionalidad.SelectedValue);
            this.MisTiposOperacionesModulosFuncionalidades = TGEGeneralesF.TiposOperacionesModulosFuncionalidadesSeleccionarFilttro(parametros);
            this.gvDatos.DataSource = this.MisTiposOperacionesModulosFuncionalidades;
            this.gvDatos.PageIndex = parametros.IndiceColeccion;
            this.gvDatos.DataBind();
            this.UpdatePanel1.Update();
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);
        }
        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Anular"))
                return;
            int IdTipoFuncionalidad = 0;
            int IdTipoOperacion = 0;
            int IdModuloSistema = 0;
            int index = Convert.ToInt32(e.CommandArgument);
            if (!string.IsNullOrEmpty(((GridView)sender).DataKeys[index]["IdTipoOperacion"].ToString()))
                IdTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoOperacion"].ToString());
            if (!string.IsNullOrEmpty(((GridView)sender).DataKeys[index]["IdTipoFuncionalidad"].ToString()))
                IdTipoFuncionalidad = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoFuncionalidad"].ToString());
            if (!string.IsNullOrEmpty(((GridView)sender).DataKeys[index]["IdModuloSistema"].ToString()))
                IdModuloSistema = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdModuloSistema"].ToString());

            switch (e.CommandName)
            {
                case "Anular":
                    this.MisParametrosUrl = new Hashtable
                    {
                        { "IdTipoOperacion", IdTipoOperacion },
                        { "IdTipoFuncionalidad", IdTipoFuncionalidad },
                        { "IdModuloSistema", IdModuloSistema }
                    };
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposOperacionesModulosFuncionalidadesEliminar.aspx"), true);
                    break;
                default:
                    break;
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int index = Convert.ToInt32(e.Row.RowIndex);
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ibtnAnular.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                object suma;
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                suma = this.MisTiposOperacionesModulosFuncionalidades.Rows.Count;
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), suma);
            }
        }
        #endregion
    }
}