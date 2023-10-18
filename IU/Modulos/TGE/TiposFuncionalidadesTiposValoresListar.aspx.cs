using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class TiposFuncionalidadesTiposValoresListar : PaginaSegura
    {
        private DataTable MisTiposFuncionalidadesTiposValores
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisTiposFuncionalidadesTiposValoresListar"]; }
            set { Session[this.MiSessionPagina + "MisTiposFuncionalidadesTiposValoresListar"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                this.btnAgregar.Visible = this.ValidarPermiso("TiposOperacionesModulosFuncionalidadesAgregar.aspx");
                TGETiposFuncionalidadesTiposValores parametros = this.BusquedaParametrosObtenerValor<TGETiposFuncionalidadesTiposValores>();
                int idTipoValor = new ListaParametros(this.MiSessionPagina).ObtenerValor("IdTipoValor");
                if (idTipoValor > 0)
                {
                    parametros.IdTipoValor = idTipoValor;
                    parametros.BusquedaParametros = true;
                }
                if (parametros.BusquedaParametros)
                {
                    this.ddlTipoValor.SelectedValue = parametros.IdTipoValor.ToString();
                    this.ddlTipoFuncionalidad.SelectedValue = parametros.IdTipoFuncionalidad.ToString();
                    this.CargarLista();
                }
                if (this.MisParametrosUrl.Contains("IdTipoValor"))
                {
                    this.ddlTipoValor.SelectedValue = parametros.IdTipoValor.ToString();
                    this.ddlTipoFuncionalidad.SelectedValue = parametros.IdTipoFuncionalidad.ToString();
                    this.CargarLista();
                    this.MisParametrosUrl = new Hashtable();
                }
                else
                {
                    this.CargarLista();
                }
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposFuncionalidadesTiposValoresAgregar.aspx"), true);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarLista();
        }
        private void CargarCombos()
        {
            this.ddlTipoFuncionalidad.DataSource = TGEGeneralesF.TGETiposFuncionalidadesObtenerLista();
            this.ddlTipoFuncionalidad.DataValueField = "IdTipoFuncionalidad";
            this.ddlTipoFuncionalidad.DataTextField = "TipoFuncionalidad";
            this.ddlTipoFuncionalidad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFuncionalidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoValor.DataSource = TGEGeneralesF.TiposValoresObtenerLista();
            this.ddlTipoValor.DataValueField = "IdTipoValor";
            this.ddlTipoValor.DataTextField = "TipoValor";
            this.ddlTipoValor.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoValor, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista()
        {
            TGETiposFuncionalidadesTiposValores tipoOp = new TGETiposFuncionalidadesTiposValores();
            tipoOp.IdTipoValor = this.ddlTipoValor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoValor.SelectedValue);
            tipoOp.IdTipoFuncionalidad = this.ddlTipoFuncionalidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFuncionalidad.SelectedValue);
            this.MisTiposFuncionalidadesTiposValores = TGEGeneralesF.TiposFuncionalidadesTiposValoresSeleccionarFilttro(tipoOp);
            this.gvDatos.DataSource = this.MisTiposFuncionalidadesTiposValores;
            this.gvDatos.DataBind();
            this.UpdatePanel1.Update();
        }
        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Anular"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdTipoValor = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoValor"].ToString());
            int IdTipoFuncionalidad = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoFuncionalidad"].ToString());

            switch (e.CommandName)
            {
                case "Anular":
                    this.MisParametrosUrl = new Hashtable
                    {
                        { "IdTipoValor", IdTipoValor },
                        { "IdTipoFuncionalidad", IdTipoFuncionalidad }
                    };
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/TiposFuncionalidadesTiposValoresEliminar.aspx"), true);
                    break;
                default:
                    break;
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //int index = Convert.ToInt32(e.Row.RowIndex);
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ibtnAnular.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                object suma;
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                suma = this.MisTiposFuncionalidadesTiposValores.Rows.Count;
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), suma);
            }
        }
        #endregion
    }
}