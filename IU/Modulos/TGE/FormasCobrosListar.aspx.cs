using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class FormasCobrosListar : PaginaSegura
    {
        private List<TGEFormasCobros> MiFormaCobro
        {
            get { return (List<TGEFormasCobros>)Session[this.MiSessionPagina + "FormasCobrosListarMiFormaCobro"]; }
            set { Session[this.MiSessionPagina + "FormasCobrosListarMiFormaCobro"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoFormaCobro, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFormaCobro, this.btnBuscar);
                this.CargarCombo();
                TGEFormasCobros parametros = this.BusquedaParametrosObtenerValor<TGEFormasCobros>();
                this.btnAgregar.Visible = this.ValidarPermiso("FormasCobrosAgregar.aspx");
                if (parametros.BusquedaParametros)
                {
                    this.txtFormaCobro.Text = parametros.FormaCobro;
                    this.txtCodigoFormaCobro.Text = parametros.CodigoFormaCobro;
                    this.CargarLista(parametros);
                }
            }
        }
        private void CargarCombo()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlEstado.SelectedValue = 1.ToString();
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGEFormasCobros parametros = this.BusquedaParametrosObtenerValor<TGEFormasCobros>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosAgregar.aspx"), true);
        }
        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEFormasCobros formaCobro = this.MiFormaCobro[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdFormaCobro", formaCobro.IdFormaCobro);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("FormasCobrosModificar.aspx");
                consultar.Visible = this.ValidarPermiso("FormasCobrosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiFormaCobro.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEFormasCobros parametros = this.BusquedaParametrosObtenerValor<TGEFormasCobros>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEFormasCobros>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<TGEFormasCobros>(this.MiFormaCobro, false, this.gvDatos, true);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiFormaCobro = this.OrdenarGrillaDatos<TGEFormasCobros>(this.MiFormaCobro, e);
            AyudaProgramacion.CargarGrillaListas<TGEFormasCobros>(this.MiFormaCobro, false, this.gvDatos, true);
        }
        #endregion
        private void CargarLista(TGEFormasCobros pParametro)
        {
            pParametro.FormaCobro = this.txtFormaCobro.Text;
            pParametro.CodigoFormaCobro = this.txtCodigoFormaCobro.Text;

            pParametro.Estado.IdEstado = ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlEstado.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<TGEFormasCobros>(pParametro);
            this.MiFormaCobro = TGEGeneralesF.FormasCobrosObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            AyudaProgramacion.CargarGrillaListas<TGEFormasCobros>(this.MiFormaCobro, false, this.gvDatos, true);
        }
    }
}