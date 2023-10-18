using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using Generales.Entidades;
using CuentasPagar.FachadaNegocio;
using Proveedores.Entidades;
using Comunes.Entidades;
using System.Collections;
using Generales.FachadaNegocio;
using Proveedores;

namespace IU.Modulos.CuentasPagar
{
    public partial class SolicitudesPagosAfiliadosListar : PaginaAfiliados
    {
        private List<CapSolicitudPago> MisSolicitudes
        {
            get { return (List<CapSolicitudPago>)Session[this.MiSessionPagina + "SolicitudesPagosAfiliadosListarMisSolicitudes"]; }
            set { Session[this.MiSessionPagina + "SolicitudesPagosAfiliadosListarMisSolicitudes"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrBuscarProveedorPopUp.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar);
            if (!this.IsPostBack)
            {

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSolicitud, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroFactura, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPrefijoNumeroFactura, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("SolicitudesPagosAfiliadosAgregar.aspx");
                this.CargarCombos();

                CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSolicitud.Text = parametros.IdSolicitudPago == 0 ? String.Empty : parametros.IdSolicitudPago.ToString();
                    //this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    this.txtNumeroFactura.Text = parametros.NumeroFactura;
                    this.ddlTipoSolicitud.SelectedValue = parametros.TipoSolicitudPago.IdTipoSolicitudPago.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtCodigo.Text = parametros.Entidad.IdRefEntidad.ToString();
                    this.txtProveedor.Text = parametros.Entidad.Beneficiario;
                    this.btnLimpiar.Visible = parametros.Entidad.IdRefEntidad > 0;
                    this.btnBuscarProveedor.Visible = parametros.Entidad.IdRefEntidad == 0;

                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAfiliadosAgregar.aspx"), true);
        }

        private void CargarCombos()
        {
            this.ddlTipoSolicitud.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposSolicitudesPago);
            this.ddlTipoSolicitud.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoSolicitud.DataTextField = "Descripcion";
            this.ddlTipoSolicitud.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoSolicitud, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosSolicitudesPagos));
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
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CapSolicitudPago pSolicitud = this.MisSolicitudes[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdSolicitudPago", pSolicitud.IdSolicitudPago);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAfiliadosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAfiliadosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAfiliadosAutorizar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CapSolicitudPago solicitud = (CapSolicitudPago)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");

                ibtnConsultar.Visible = this.ValidarPermiso("SolicitudesPagosAfiliadosConsultar.aspx");

                switch (solicitud.Estado.IdEstado)
                {
                    case (int)EstadosSolicitudesPagos.Activo:
                        autorizar.Visible = this.ValidarPermiso("SolicitudesPagosAfiliadosAutorizar.aspx");
                        modificar.Visible = this.ValidarPermiso("SolicitudesPagosAfiliadosAnular.aspx");
                        break;
                    case (int)EstadosSolicitudesPagos.Autorizado:
                        modificar.Visible = this.ValidarPermiso("SolicitudesPagosAfiliadosAnular.aspx");
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSolicitudes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisSolicitudes;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSolicitudes = this.OrdenarGrillaDatos<CapSolicitudPago>(this.MisSolicitudes, e);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        #endregion

        #region "Proveedores PopUp"

        protected void btnLimpiar_Click(object sender, EventArgs e)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.Entidad = new TGEEntidades();
            this.txtCodigo.Text = string.Empty;
            this.txtProveedor.Text = string.Empty;
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(parametros);
            this.btnLimpiar.Visible = false;
            this.btnBuscarProveedor.Visible = true;
            this.UpdatePanel1.Update();
        }

        protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        {
            this.ctrBuscarProveedorPopUp.IniciarControl();
        }

        void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        {
            CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
            parametros.Entidad.Beneficiario = pProveedor.RazonSocial;
            parametros.Entidad.IdRefEntidad = pProveedor.IdProveedor.Value;
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(parametros);
            this.MapearObjetoAControlesProveedor(pProveedor);
            this.btnLimpiar.Visible = true;
            this.btnBuscarProveedor.Visible = false;
            this.UpdatePanel1.Update();
        }

        private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        {
            this.txtCodigo.Text = pProveedor.IdProveedor.Value.ToString();
            this.txtProveedor.Text = pProveedor.RazonSocial;
        }

        protected void txtCodigo_TextChanged(object sender, EventArgs e)
        {
            string txtCodigo = ((TextBox)sender).Text;
            CapProveedores parametro = new CapProveedores();
            parametro.IdProveedor = Convert.ToInt32(txtCodigo);
            parametro = ProveedoresF.ProveedoresObtenerDatosCompletos(parametro);

            if (parametro.IdProveedor > 0)
                this.ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(parametro);
            else
                this.ctrBuscarProveedorPopUp.IniciarControl();

        }

        #endregion

        private void CargarLista(CapSolicitudPago pSolicitud)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pSolicitud.IdSolicitudPago = this.txtNumeroSolicitud.Text == String.Empty ? 0 : Convert.ToInt32(this.txtNumeroSolicitud.Text);
            pSolicitud.NumeroFactura = this.txtNumeroFactura.Text == string.Empty ? "" : this.txtNumeroFactura.Text;
            //pSolicitud.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text == String.Empty ? "" : this.txtPrefijoNumeroFactura.Text;
            pSolicitud.TipoSolicitudPago.IdTipoSolicitudPago = this.ddlTipoSolicitud.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlTipoSolicitud.SelectedValue);
            pSolicitud.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //VER QUE BUSQUE CON EL POP UP
            pSolicitud.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
            pSolicitud.Entidad.IdRefEntidad = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);
            pSolicitud.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pSolicitud.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pSolicitud.Afiliado.IdAfiliado = this.MiAfiliado.IdAfiliado;
            pSolicitud.BusquedaParametros = true;
            pSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(pSolicitud);
            this.MisSolicitudes = CuentasPagarF.SolicitudPagoObtenerListaFiltro(pSolicitud);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.PageIndex = pSolicitud.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisSolicitudes.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }

    }
}