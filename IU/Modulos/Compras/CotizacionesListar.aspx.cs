using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Compras;
using Proveedores.Entidades;
using Proveedores;
using Comunes.Entidades;
using System.Collections;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU.Modulos.Compras
{
    public partial class CotizacionesListar : PaginaSegura
    {
        private List<CmpCotizaciones> MisCotizaciones
        {
            get { return (List<CmpCotizaciones>)Session[this.MiSessionPagina + "CotizacionesListarMisCotizaciones"]; }
            set { Session[this.MiSessionPagina + "CotizacionesListarMisCotizaciones"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrBuscarProveedorPopUp.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar);
            if (!this.IsPostBack)
            {

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoCotizacion, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("CotizacionesAgregar.aspx");
                this.CargarCombos();

                CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCodigoCotizacion.Text = parametros.IdCotizacion == 0 ? String.Empty : parametros.IdCotizacion.ToString();
                    
                    this.ddlCondicionPago.SelectedValue = parametros.CondicionPago.IdCondicionPago.ToString();
                  
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtCodigo.Text = parametros.Proveedor.IdProveedor.ToString();
                    this.txtProveedor.Text = parametros.Proveedor.RazonSocial;

                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesAgregar.aspx"), true);
        }

        private void CargarCombos()
        {
            this.ddlCondicionPago.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CondicionesPagos);
            this.ddlCondicionPago.DataValueField = "IdListaValorDetalle";
            this.ddlCondicionPago.DataTextField = "Descripcion";
            this.ddlCondicionPago.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionPago, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosSolicitudesCompras));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();

        }

        #region "Grilla"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CmpCotizaciones pCotizacion = this.MisCotizaciones[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCotizacion", pCotizacion.IdCotizacion);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Autorizar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesAutorizar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CmpCotizaciones cotizacion = (CmpCotizaciones)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                ibtnConsultar.Visible = this.ValidarPermiso("CotizacionesConsultar.aspx");

                switch (cotizacion.Estado.IdEstado)
                {
                    case (int)EstadosCotizaciones.Activo:
                        //autorizar.Visible = this.ValidarPermiso("CotizacionesAutorizar.aspx");
                        modificar.Visible = this.ValidarPermiso("CotizacionesModificar.aspx");
                        anular.Visible = this.ValidarPermiso("CotizacionesAnular.aspx");
                        break;

                    case (int)EstadosCotizaciones.Autorizado:
                        anular.Visible = this.ValidarPermiso("CotizacionesAnular.aspx");
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCotizaciones.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CmpCotizaciones>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisCotizaciones;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCotizaciones = this.OrdenarGrillaDatos<CmpCotizaciones>(this.MisCotizaciones, e);
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
        #endregion

        #region "Proveedores PopUp"

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
            parametros.Proveedor = new CapProveedores();
            this.txtCodigo.Text = string.Empty;
            this.txtProveedor.Text = string.Empty;
            this.BusquedaParametrosGuardarValor<CmpCotizaciones>(parametros);
            this.UpdatePanel1.Update();
        }

        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            this.ctrBuscarProveedorPopUp.IniciarControl();
        }

        void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        {
            CmpCotizaciones parametros = this.BusquedaParametrosObtenerValor<CmpCotizaciones>();
            parametros.Proveedor.RazonSocial = pProveedor.RazonSocial;
            parametros.Proveedor.IdProveedor = pProveedor.IdProveedor.Value;
            this.BusquedaParametrosGuardarValor<CmpCotizaciones>(parametros);
            this.MapearObjetoAControlesProveedor(pProveedor);
            this.UpdatePanel1.Update();
        }

        private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        {
            this.txtCodigo.Text = pProveedor.IdProveedor.Value.ToString();
            this.txtProveedor.Text = pProveedor.RazonSocial;
        }

        //protected void txtCodigo_TextChanged(object sender, EventArgs e)
        //{
        //    string txtCodigo = ((TextBox)sender).Text;
        //    CapProveedores parametro = new CapProveedores();
        //    parametro.IdProveedor = Convert.ToInt32(txtCodigo);
        //    this.MisCotizaciones.Proveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);
        //    if (this.MiCotizacion.Proveedor.IdProveedor != 0)
        //    {
        //        this.MapearObjetoAControlesProveedor(this.MiCotizacion.Proveedor);
        //    }
        //    else
        //    {
        //        parametro.CodigoMensaje = "ProveedorCodigoNoExiste";
        //        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(parametro.CodigoMensaje));
        //    }
        //}

        #endregion

        private void CargarLista(CmpCotizaciones pCotizacion)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pCotizacion.IdCotizacion = this.txtCodigoCotizacion.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigoCotizacion.Text);
            
            pCotizacion.CondicionPago.IdCondicionPago= this.ddlCondicionPago.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlCondicionPago.SelectedValue);
            pCotizacion.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //VER QUE BUSQUE CON EL POP UP

            pCotizacion.Proveedor.IdProveedor = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);
            pCotizacion.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pCotizacion.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pCotizacion.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CmpCotizaciones>(pCotizacion);
            this.MisCotizaciones = ComprasF.CotizacionesObtenerListaFiltro(pCotizacion);
            this.gvDatos.DataSource = this.MisCotizaciones;
            this.gvDatos.PageIndex = pCotizacion.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisCotizaciones.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}