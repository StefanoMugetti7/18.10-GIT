using Comunes.Entidades;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace IU.Modulos.ProcesosDatos
{
    public partial class ProcesosDatosListar : PaginaSegura
    {
        private List<SisProcesosProcesamiento> MisProcesosProcesamiento
        {
            get { return this.PropiedadObtenerValor<List<SisProcesosProcesamiento>>("ProcesosDatosListarMisProcesosProcesamiento"); }
            set { this.PropiedadGuardarValor("ProcesosDatosListarMisProcesosProcesamiento", value); }
        }
        private DataTable MisProcesosProcesamientoDT
        {
            get { return this.PropiedadObtenerValor<DataTable>("ProcesosDatosListarMisProcesosProcesamientoDT"); }
            set { this.PropiedadGuardarValor("ProcesosDatosListarMisProcesosProcesamientoDT", value); }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                this.CargarCombo();
                SisProcesos parametros = this.BusquedaParametrosObtenerValor<SisProcesos>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlFiltro.SelectedValue = parametros.IdProceso.ToString();
                    this.txtDescripcion.Text = parametros.Filtro;
                    this.CargarLista(parametros);
                }
            }
        }
        private void CargarCombo()
        {
            SisProcesos filtro = new SisProcesos();
            filtro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlFiltro.DataSource = ProcesosDatosF.ProcesosObtenerListaFiltro(filtro);
            this.ddlFiltro.DataTextField = "Descripcion";
            this.ddlFiltro.DataValueField = "IdProceso";
            this.ddlFiltro.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlFiltro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlFiltro.SelectedValue))
            {
                SisProcesos proceso = new SisProcesos();
                proceso.IdProceso = Convert.ToInt32(this.ddlFiltro.SelectedValue);
                this.btnBuscar.Visible = true;
                this.lblFiltro.Visible = true;
                this.txtDescripcion.Visible = true;
                this.btnAgregar.Visible = true;
                this.CargarLista(proceso);
            }
            else
            {
                this.btnAgregar.Visible = false;
                this.btnBuscar.Visible = false;
                this.txtDescripcion.Visible = false;
                this.lblFiltro.Visible = false;
                this.MisProcesosProcesamiento = new List<SisProcesosProcesamiento>();
                this.MisProcesosProcesamientoDT = new DataTable();
                this.gvDatos.DataSource = this.MisProcesosProcesamientoDT;
                this.gvDatos.DataBind();
            }
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFiltro.SelectedValue))
            {
                string proceso = this.ddlFiltro.SelectedValue;
                this.MisParametrosUrl = new Hashtable
                {
                    { "IdProceso", proceso }
                };
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/ProcesosDatosAgregar.aspx"), true);
            }
            else
            {
                this.MostrarMensaje(this.ObtenerMensajeSistema("SeleccioneProceso"), true);
            }
        }
        private void CargarLista(SisProcesos pParametro)
        {
            pParametro.Filtro = this.txtDescripcion.Text;
            pParametro.IdProceso = Convert.ToInt32(this.ddlFiltro.SelectedValue);
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;

            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            this.gvDatos.PageSize = pParametro.PageSize;

            this.BusquedaParametrosGuardarValor<SisProcesos>(pParametro);
            this.MisProcesosProcesamientoDT = ProcesosDatosF.ProcesosObtenerListaDT(pParametro);
            this.gvDatos.DataSource = this.MisProcesosProcesamientoDT;
            this.gvDatos.VirtualItemCount = MisProcesosProcesamientoDT.Rows.Count > 0 ? Convert.ToInt32(MisProcesosProcesamientoDT.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatos);
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            SisProcesos parametros = this.BusquedaParametrosObtenerValor<SisProcesos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SisProcesos parametros = this.BusquedaParametrosObtenerValor<SisProcesos>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                consultar.Visible = this.ValidarPermiso("ProcesosDatosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                //tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisProcesosProcesamiento.Count);
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisProcesosProcesamientoDT.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idprocesamiento = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdProcesoProcesamiento", idprocesamiento }
            };

            if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/ProcesosDatos/ProcesosDatosConsultar.aspx"), true);

        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFiltro.SelectedValue))
            {
                SisProcesos proceso = new SisProcesos();
                proceso.IdProceso = Convert.ToInt32(this.ddlFiltro.SelectedValue);
                proceso.PageIndex = 0;
                this.gvDatos.PageIndex = 0;
                this.CargarLista(proceso);
            }
            else
            {
                return;
            }
        }
    }
}