using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Contabilidad;
using System.Collections;
using Generales.Entidades;
using System.Data;

namespace IU.Modulos.Contabilidad
{
    public partial class AsientosListar : PaginaSegura
    {
        private DataTable MisAsientosContables
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisAsientosContables"]; }
            set { Session[this.MiSessionPagina + "MisAsientosContables"] = value; }
        }
        private List<CtbEjerciciosContables> MisEjerciciosContables
        {
            get { return (List<CtbEjerciciosContables>)Session[this.MiSessionPagina + "MisEjerciciosContables"]; }
            set { Session[this.MiSessionPagina + "MisEjerciciosContables"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)   
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDetalle, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroAsiento, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("AsientosAgregar.aspx");
                this.txtFechaAsientoDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                this.txtFechaAsientoHasta.Text = DateTime.Now.ToShortDateString();
                this.CargarCombos();
                CtbAsientosContables parametros = this.BusquedaParametrosObtenerValor<CtbAsientosContables>();
                if (parametros.BusquedaParametros)
                {
                    this.txtDetalle.Text = parametros.DetalleGeneral;
                    this.txtNumeroAsiento.Text = parametros.NumeroAsiento;
                    this.txtFechaAsientoDesde.Text = parametros.FechaAsientoDesde.HasValue ? parametros.FechaAsientoDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaAsientoHasta.Text = parametros.FechaAsientoHasta.HasValue ? parametros.FechaAsientoHasta.Value.ToShortDateString() : string.Empty;
                    this.txtNumeroCopiativo.Text = parametros.NumeroAsientoCopiativo;
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtFechaRealizado.Text = parametros.FechaRealizado.HasValue ? parametros.FechaRealizado.Value.ToShortDateString() : string.Empty;
                    this.ddlTipoOperacion.SelectedValue = parametros.IdTipoOperacion.ToString();
                    this.ddlRefTipoOperacion.SelectedValue = parametros.IdRefTipoOperacion.ToString();
                    this.ddlEjercicioContable.SelectedValue = parametros.IdEjercicioContable.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CtbAsientosContables parametros = this.BusquedaParametrosObtenerValor<CtbAsientosContables>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbAsientosContables parametros = this.BusquedaParametrosObtenerValor<CtbAsientosContables>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idAsientoContable = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdAsientoContable", idAsientoContable);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosConsultar.aspx"), true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                modificar.Visible = this.ValidarPermiso("AsientosModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisAsientosContables.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbAsientosContables parametros = this.BusquedaParametrosObtenerValor<CtbAsientosContables>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisAsientosContables = this.OrdenarGrillaDatos<CtbAsientosContables>(this.MisAsientosContables, e);
            this.gvDatos.DataSource = this.MisAsientosContables;
            this.gvDatos.DataBind();
        }

        protected void ddlEjercicioContable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
            {
                CtbEjerciciosContables ejerCon = this.MisEjerciciosContables[this.ddlEjercicioContable.SelectedIndex];
                double desde = ejerCon.FechaInicio.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                double hasta = ejerCon.FechaFin.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                string script = string.Format("InitControlFecha('{0}', '{1}');", desde, hasta);
                ScriptManager.RegisterStartupScript(this.upEjercicioContable, this.upEjercicioContable.GetType(), "InitControlFechaScript", script, true);
                this.txtFechaAsientoDesde.Text = ejerCon.FechaInicio.ToShortDateString(); //fechaDesde < ejerCon.FechaInicio ? ejerCon.FechaInicio.ToShortDateString() : fechaDesde.ToShortDateString();
                this.txtFechaAsientoHasta.Text = ejerCon.FechaFin.ToShortDateString(); //fechaHasta > ejerCon.FechaFin ? ejerCon.FechaFin.ToShortDateString() : fechaHasta.ToShortDateString();
                this.upEjercicioContable.Update();
            }
        }
        private void CargarCombos()
        {
            CtbEjerciciosContables filtroAsiento = new CtbEjerciciosContables();
            filtroAsiento.Estado.IdEstado = (int)EstadosEjerciosContables.Activo;
            this.MisEjerciciosContables = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtroAsiento);
            this.ddlEjercicioContable.DataSource = this.MisEjerciciosContables;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();
            this.ddlEjercicioContable_SelectedIndexChanged(this.ddlEjercicioContable, EventArgs.Empty);
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
            //TO DO: Completar con la lista correspondiente, aun no esta definida
            TGETiposOperaciones filtro = new TGETiposOperaciones();
            filtro.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaActual.IdTipoFuncionalidad;
            this.ddlTipoOperacion.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(filtro);
            this.ddlTipoOperacion.DataValueField = "IdTipoOperacion";
            this.ddlTipoOperacion.DataTextField = "TipoOperacion";
            this.ddlTipoOperacion.DataBind();
            this.ddlTipoOperacion.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlTipoOperacion.SelectedValue = ((int)EstadosTodos.Todos).ToString();
            //TO DO: Completar con la lista correspondiente, aun no esta definida
            this.ddlRefTipoOperacion.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposAsientos);
            this.ddlRefTipoOperacion.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlRefTipoOperacion.DataTextField = "Descripcion";
            this.ddlRefTipoOperacion.DataBind();
            this.ddlRefTipoOperacion.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlRefTipoOperacion.SelectedValue = ((int)EstadosTodos.Todos).ToString();

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            this.ddlFilial.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlFilial.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        private void CargarLista(CtbAsientosContables pAseintoContable)
        {
            pAseintoContable.Filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
            pAseintoContable.DetalleGeneral = this.txtDetalle.Text.Trim();
            pAseintoContable.NumeroAsiento = this.txtNumeroAsiento.Text;
            pAseintoContable.FechaAsientoDesde = Convert.ToDateTime(this.txtFechaAsientoDesde.Text.Trim());
            pAseintoContable.FechaAsientoHasta = Convert.ToDateTime(this.txtFechaAsientoHasta.Text.Trim());
            pAseintoContable.NumeroAsientoCopiativo = this.txtNumeroCopiativo.Text.Trim();
            pAseintoContable.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            if (this.txtFechaRealizado.Text != string.Empty)
                pAseintoContable.FechaRealizado = Convert.ToDateTime(this.txtFechaRealizado.Text.Trim());
            pAseintoContable.IdTipoOperacion = Convert.ToInt32(this.ddlTipoOperacion.SelectedValue);
            pAseintoContable.AsientoContableTipo.IdAsientoContableTipo = Convert.ToInt32(this.ddlRefTipoOperacion.SelectedValue);
            pAseintoContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            pAseintoContable.BusquedaParametros = true;
            pAseintoContable.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize); //UsuarioActivo.PageSize > 0 ? UsuarioActivo.PageSize : 25;
            gvDatos.PageSize = pAseintoContable.PageSize;
            this.BusquedaParametrosGuardarValor<CtbAsientosContables>(pAseintoContable);
            this.MisAsientosContables = ContabilidadF.AsientosContablesObtenerListaFiltroGrilla(pAseintoContable);
            this.gvDatos.DataSource = this.MisAsientosContables;
            this.gvDatos.VirtualItemCount = MisAsientosContables.Rows.Count > 0 ? Convert.ToInt32(MisAsientosContables.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
        }
    }
}