using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class FormasCobrosCodigosConceptosListar : PaginaSegura
    {
        private DataTable MiFormaCobroCodigosConceptos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "FormasCobrosCodigosConceptosListarMiFormaCobroCodigosConceptos"]; }
            set { Session[this.MiSessionPagina + "FormasCobrosCodigosConceptosListarMiFormaCobroCodigosConceptos"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("FormasCobrosCodigosConceptosAgregar.aspx");

                TGEFormasCobros fc = new TGEFormasCobros();
                fc.Estado.IdEstado = (int)Estados.Activo;
                this.ddlFormaCobro.DataSource = TGEGeneralesF.FormasCobrosObtenerListaFiltro(fc);
                this.ddlFormaCobro.DataValueField = "IdFormaCobro";
                this.ddlFormaCobro.DataTextField = "FormaCobro";
                this.ddlFormaCobro.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTipoCargo, this.btnBuscar);
                TGEFormasCobrosCodigosConceptosTiposCargosCategorias parametros = this.BusquedaParametrosObtenerValor<TGEFormasCobrosCodigosConceptosTiposCargosCategorias>();
                if (parametros.BusquedaParametros)
                {
                    this.txtTipoCargo.Text = parametros.TipoCargo.ToString();
                    this.ddlFormaCobro.SelectedValue = parametros.FormaCobro.IdFormaCobro == 0 ? string.Empty : parametros.FormaCobro.IdFormaCobro.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            TGEFormasCobrosCodigosConceptosTiposCargosCategorias parametros = this.BusquedaParametrosObtenerValor<TGEFormasCobrosCodigosConceptosTiposCargosCategorias>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosCodigosConceptosAgregar.aspx"), true);
        }
        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdFormaCobroCodigoConceptoTipoCargoCategoria = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdFormaCobroCodigoConceptoTipoCargoCategoria", IdFormaCobroCodigoConceptoTipoCargoCategoria }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosCodigosConceptosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosCodigosConceptosConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("FormasCobrosCodigosConceptosModificar.aspx");
                consultar.Visible = this.ValidarPermiso("FormasCobrosCodigosConceptosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiFormaCobroCodigosConceptos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEFormasCobrosCodigosConceptosTiposCargosCategorias parametros = this.BusquedaParametrosObtenerValor<TGEFormasCobrosCodigosConceptosTiposCargosCategorias>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEFormasCobrosCodigosConceptosTiposCargosCategorias>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiFormaCobroCodigosConceptos;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiFormaCobroCodigosConceptos = this.OrdenarGrillaDatos<TGEFormasCobrosCodigosConceptosTiposCargosCategorias>(this.MiFormaCobroCodigosConceptos, e);
            this.gvDatos.DataSource = this.MiFormaCobroCodigosConceptos;
            this.gvDatos.DataBind();
        }
        #endregion
        private void CargarLista(TGEFormasCobrosCodigosConceptosTiposCargosCategorias pParametro)
        {
            pParametro.BusquedaParametros = true;
            pParametro.FormaCobro.IdFormaCobro = this.ddlFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            pParametro.TipoCargo = this.txtTipoCargo.Text;
            this.BusquedaParametrosGuardarValor<TGEFormasCobrosCodigosConceptosTiposCargosCategorias>(pParametro);
            this.MiFormaCobroCodigosConceptos = TGEGeneralesF.FormasCobrosCodigosConceptosTiposCargosCategoriasObtenerListaFiltro(pParametro);
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataSource = this.MiFormaCobroCodigosConceptos;
            this.gvDatos.DataBind();
        }
    }
}