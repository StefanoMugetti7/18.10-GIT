using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Prestamos
{
    public partial class PrestamosCotizacionesListar : PaginaSegura
    {
        private DataTable MisCotizaciones
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CotizacionesListarMisCotizaciones"]; }
            set { Session[this.MiSessionPagina + "CotizacionesListarMisCotizaciones"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();

                PrePrestamosCotizacionesUnidades parametros = this.BusquedaParametrosObtenerValor<PrePrestamosCotizacionesUnidades>();
                if (parametros.BusquedaParametros)
                {
                    this.CargarLista(parametros);
                }
            }
        }

        private void CargarCombos()
        {
            this.ddlTipoUnidad.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposUnidades);
            this.ddlTipoUnidad.DataValueField = "IdListaValorDetalle";
            this.ddlTipoUnidad.DataTextField = "Descripcion";
            this.ddlTipoUnidad.DataBind();
            if (this.ddlTipoUnidad.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoUnidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PrePrestamosCotizacionesUnidades parametros = this.BusquedaParametrosObtenerValor<PrePrestamosCotizacionesUnidades>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosCotizacionesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrePrestamosCotizacionesUnidades parametros = this.BusquedaParametrosObtenerValor<PrePrestamosCotizacionesUnidades>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<PrePrestamosCotizacionesUnidades>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisCotizaciones;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCotizaciones = this.OrdenarGrillaDatos<DataTable>(this.MisCotizaciones, e);
            this.gvDatos.DataSource = this.MisCotizaciones;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisCotizaciones;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }



        private void CargarLista(PrePrestamosCotizacionesUnidades pCotizaciones)
        {
            pCotizaciones.TipoUnidad.IdTiposUnidades = ddlTipoUnidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoUnidad.SelectedValue);
            pCotizaciones.BusquedaParametros = true;

            this.BusquedaParametrosGuardarValor<PrePrestamosCotizacionesUnidades>(pCotizaciones);
            MisCotizaciones = PrePrestamosF.PrestamosCotizacionesObtenerListaGrilla(pCotizaciones);

            this.gvDatos.DataSource = this.MisCotizaciones;
            this.gvDatos.PageIndex = pCotizaciones.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisCotizaciones.Rows.Count > 0)
            {
                //btnExportarExcel.Visible = true;
            }
            else
            {
                //btnExportarExcel.Visible = false;
            }
        }
    }
}
