using Comunes.Entidades;
using LavaYa.Entidades;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using System;
using System.Collections;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.ProcesosDatos
{
    public partial class TiposCargosLotesEnviadosListar : PaginaSegura
    {
        private DataTable MisLotes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CarTiposCargosLotesEnviadosMisLotes"]; }
            set { Session[this.MiSessionPagina + "CarTiposCargosLotesEnviadosMisLotes"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                this.CargarCombo();
                CarTiposCargosLotesEnviados parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosLotesEnviados>();
                if (parametros.BusquedaParametros)
                {
                    this.txtDescripcion.Text = parametros.Filtro;
                    this.ddlPeriodo.SelectedValue = parametros.Periodo.HasValue ? parametros.Periodo.Value.ToString() : "-1";
                    this.ddlFormaCobro.SelectedValue = parametros.IdFormaCobro.HasValue ? parametros.IdFormaCobro.Value.ToString() : "-1";
                    this.CargarLista(parametros);
                }
            }
        }
        private void CargarCombo()
        {
            this.ddlFormaCobro.DataSource = LotesCobranzasF.TiposCargosLotesEnviadosObtenerFormasCobros();
            this.ddlFormaCobro.DataTextField = "FormaCobro";
            this.ddlFormaCobro.DataValueField = "IdFormaCobro";
            this.ddlFormaCobro.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlPeriodo.DataSource = LotesCobranzasF.TiposCargosLotesEnviadosObtenerPeriodos();
            this.ddlPeriodo.DataTextField = "Periodo";
            this.ddlPeriodo.DataValueField = "Periodo";
            this.ddlPeriodo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlPeriodo, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(CarTiposCargosLotesEnviados pParametro)
        {
            pParametro.Filtro = txtDescripcion.Text;
            pParametro.IdFormaCobro = string.IsNullOrEmpty(this.ddlFormaCobro.SelectedValue) ? (Nullable<int>)null : Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            pParametro.Periodo = string.IsNullOrEmpty(this.ddlPeriodo.SelectedValue) ? (Nullable<int>)null : Convert.ToInt32(this.ddlPeriodo.SelectedValue);
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            this.gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<CarTiposCargosLotesEnviados>(pParametro);

            this.MisLotes = LotesCobranzasF.TiposCargosLotesEnviadosObtenerGrilla(pParametro);
            this.gvDatos.DataSource = this.MisLotes;
            this.gvDatos.VirtualItemCount = MisLotes.Rows.Count > 0 ? Convert.ToInt32(MisLotes.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + this.gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatos);
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosLotesEnviados parametros = BusquedaParametrosObtenerValor<CarTiposCargosLotesEnviados>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisLotes = OrdenarGrillaDatos<DataTable>(MisLotes, e);
            this.gvDatos.DataSource = MisLotes;
            this.gvDatos.DataBind();
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CarTiposCargosLotesEnviados parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosLotesEnviados>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CarTiposCargosLotesEnviados parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosLotesEnviados>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/TiposCargosLotesEnviadosAgregar.aspx"), true);
        }
        #region GV
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdTipoCargoLoteEnviado = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdTipoCargoLoteEnviado", IdTipoCargoLoteEnviado }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/TiposCargosLotesEnviadosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/TiposCargosLotesEnviadosConsultar.aspx"), true);

        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                //modificar.Visible = this.ValidarPermiso("TiposCargosLotesEnviadosModificar.aspx");
                modificar.Visible = false;
                consultar.Visible = this.ValidarPermiso("TiposCargosLotesEnviadosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisLotes.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        #endregion
    }
}