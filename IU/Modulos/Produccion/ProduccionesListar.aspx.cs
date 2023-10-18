using Comunes.Entidades;
using Producciones;
using Producciones.Entidades;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Produccion
{
    public partial class ProduccionesListar : PaginaSegura
    {
        private DataTable MisProducciones
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProduccionesListarMisProducciones"]; }
            set { Session[this.MiSessionPagina + "ProduccionesListarMisProducciones"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroProduccion, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDescripcion, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaIngreso, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtFechaIngresoHasta, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("ProduccionesAgregar.aspx");
                this.CargarCombos();
                PrdProducciones parametros = this.BusquedaParametrosObtenerValor<PrdProducciones>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroProduccion.Text = parametros.IdProduccion.ToString();
                    this.txtDescripcion.Text = parametros.Descripcion;
                    this.txtFechaIngreso.Text = parametros.FechaInicio.HasValue ? parametros.FechaInicio.Value.ToShortDateString() : string.Empty;
                    this.txtFechaIngresoHasta.Text = parametros.FechaInicioHasta.HasValue ? parametros.FechaInicioHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlFiliales.SelectedValue = parametros.Filial.IdFilial.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        private void CargarCombos()
        {
            //this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            //this.ddlEstado.DataValueField = "IdEstado";
            //this.ddlEstado.DataTextField = "Descripcion";
            //this.ddlEstado.DataBind();

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PrdProducciones parametros = this.BusquedaParametrosObtenerValor<PrdProducciones>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Produccion/ProduccionesAgregar.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            PrdProducciones reserva = new PrdProducciones();
            reserva.IdProduccion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdProduccion"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdProduccion", reserva.IdProduccion }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Produccion/ProduccionesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Produccion/ProduccionesConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnConsultar.Visible = this.ValidarPermiso("ProduccionesConsultar.aspx");
                modificar.Visible = this.ValidarPermiso("ProduccionesModificar.aspx");

                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (Convert.ToInt32(dr["IdEstado"]) == (int)EstadosProducciones.Finalizado
                    && !this.UsuarioActivo.EsAdministradorGeneral)
                    modificar.Visible = false;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrdProducciones parametros = this.BusquedaParametrosObtenerValor<PrdProducciones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<PrdProducciones>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisProducciones;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisProducciones = this.OrdenarGrillaDatos<DataTable>(this.MisProducciones, e);
            this.gvDatos.DataSource = this.MisProducciones;
            this.gvDatos.DataBind();
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisProducciones;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        private void CargarLista(PrdProducciones pProduccion)
        {
            pProduccion.IdProduccion = this.txtNumeroProduccion.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroProduccion.Text);
            pProduccion.Descripcion = this.txtDescripcion.Text;
            pProduccion.FechaInicio = this.txtFechaIngreso.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaIngreso.Text);
            pProduccion.FechaInicioHasta = this.txtFechaIngresoHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaIngresoHasta.Text);
            pProduccion.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pProduccion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pProduccion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<PrdProducciones>(pProduccion);
            this.MisProducciones = ProduccionesF.ProduccionesObtenerListaGrilla(pProduccion);
            this.gvDatos.DataSource = this.MisProducciones;
            this.gvDatos.PageIndex = pProduccion.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisProducciones.Rows.Count > 0)
            {
                this.btnExportarExcel.Visible = true;
            }
            else
            {
                this.btnExportarExcel.Visible = false;
            }
        }
    }
}
