using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using CuentasPagar.FachadaNegocio;
using System.Collections;
using Comunes.Entidades;
using Proveedores.Entidades;
using Generales.Entidades;
using Proveedores;
using Afiliados.Entidades;
using Afiliados;
using Generales.FachadaNegocio;
using System.Data;
using Reportes.FachadaNegocio;

namespace IU.Modulos.CuentasPagar
{
    public partial class OrdenesPagosListar : PaginaSegura
    {
        private DataTable MisOrdenesPagos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "OrdenesPagosListarCapOrdenesPagos"]; }
            set { Session[this.MiSessionPagina + "OrdenesPagosListarCapOrdenesPagos"] = value; }
        }
        private DataTable MisOrdenesPagosExcel
        {
            get { return (DataTable)Session[this.MiSessionPagina + "OrdenesPagosListarCapOrdenesPagosExcel"]; }
            set { Session[this.MiSessionPagina + "OrdenesPagosListarCapOrdenesPagosExcel"] = value; }
        }

        public AfiAfiliados MiAfiliado
        {
            get { return (AfiAfiliados)Session[this.MiSessionPagina + "OrdenesPagosListarCapOrdenesPagos"]; }
            set { Session[this.MiSessionPagina + "OrdenesPagosListarCapOrdenesPagos"] = value; }
        }

        public CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "ObtenerDatosProveedor"]; }
            set { Session[this.MiSessionPagina + "ObtenerDatosProveedor"] = value; }
        }

        public delegate void AfiliadosDatosEventHandler(AfiAfiliados e);

        public event AfiliadosDatosEventHandler BuscarAfiliado;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            //button_Click(button, EventArgs.Empty);
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            //this.ctrProveedores.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrProveedores_ProveedoresBuscar);
            //this.ctrAfiliados.AfiliadosBuscarSeleccionar += new Afiliados.Controles.AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {   
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSolicitud, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroFactura, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("SolicitudesPagosAgregar.aspx");
                this.CargarCombos();

                CapOrdenesPagos parametros = this.BusquedaParametrosObtenerValor<CapOrdenesPagos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumero.Text = parametros.IdOrdenPago == 0 ? String.Empty : parametros.IdOrdenPago.ToString();
                    this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlFilial.SelectedValue = parametros.FilialPago.IdFilialPago > 0 ? parametros.FilialPago.IdFilialPago.ToString() : string.Empty;
                    this.ddlEntidades.SelectedValue = parametros.Entidad.IdEntidad > 0 ? parametros.Entidad.IdEntidad.ToString() : string.Empty;
                    this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    this.txtNumeroFactura.Text = parametros.NumeroFactura;
                    
                    this.CargarLista(parametros);

                    /*this.txtNumeroSolicitud.Text = parametros.IdSolicitudPago == 0 ? String.Empty : parametros.IdSolicitudPago.ToString();
                    this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    this.txtNumeroFactura.Text = parametros.NumeroFactura;
                    this.ddlTipoSolicitud.SelectedValue = parametros.TipoSolicitudPago.IdTipoSolicitudPago.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlFilial.SelectedValue = parametros.IdFilial > 0 ? parametros.IdFilial.ToString() : string.Empty;
                    this.CargarLista(parametros);*/
                }
            }
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CapOrdenesPagos parametros = this.BusquedaParametrosObtenerValor<CapOrdenesPagos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }

        protected void button_Click(object sender)
        {
            if (!string.IsNullOrWhiteSpace(this.hdfIdAfiliado.Value))
            {

                string txtRefEntidad = this.hdfRazonSocial.Value;
                string txtNumeroProveedor = this.hdfIdAfiliado.Value;


                if (ddlEntidades.SelectedValue == ((int)EnumTGEEntidades.Afiliados).ToString())
                {
                    MiAfiliado = new AfiAfiliados();
                    MiAfiliado.IdAfiliado = txtNumeroProveedor == string.Empty ? 0 : Convert.ToInt32(txtNumeroProveedor);
                    //MiAfiliado.RazonSocial = this.hdfNumeroProveedor.Value;
                    MiAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(MiAfiliado);
                    if (MiAfiliado.IdAfiliado != 0)
                    {

                        this.ddlNumeroSocio.Items.Add(new ListItem(MiAfiliado.DescripcionAfiliado.ToString(), MiAfiliado.IdAfiliado.ToString()));
                        this.ddlNumeroSocio.SelectedValue = MiAfiliado.IdAfiliado.ToString();
                    }
                }
                else if (ddlEntidades.SelectedValue == ((int)EnumTGEEntidades.Proveedores).ToString())
                {
                    MiProveedor = new CapProveedores();
                    MiProveedor.IdProveedor = txtNumeroProveedor == string.Empty ? 0 : Convert.ToInt32(txtNumeroProveedor);
                    MiProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(MiProveedor);

                    if (MiProveedor.IdProveedor != 0)
                    {
                        this.ddlNumeroSocio.Items.Add(new ListItem(MiProveedor.RazonSocial.ToString(), MiProveedor.IdProveedor.ToString()));
                        this.ddlNumeroSocio.SelectedValue = MiProveedor.IdProveedor.ToString();
                    }
                    else
                    {

                        MiAfiliado.CodigoMensaje = "El Proveedor no existe";
                        //this.UpdatePanel2.Update();
                        this.MostrarMensaje(MiAfiliado.CodigoMensaje, true);
                    }
                }

                
                BuscarAfiliado?.Invoke(MiAfiliado);
            }
            else
            {
                this.ddlNumeroSocio.Items.Clear();
                this.ddlNumeroSocio.SelectedValue = null;
                this.ddlNumeroSocio.ClearSelection();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                //this.txtProveedor.Text = "";
            }
        }

        //protected void button_Click(object sender)
        //{
        //    string txtNumeroSocio = this.hdfIdAfiliado.Value;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = txtNumeroSocio == string.Empty ? 0 : Convert.ToInt32(txtNumeroSocio);
        //    parametro = AfiliadosF.AfiliadosObtenerDatos(parametro);
        //    if (parametro.IdAfiliado != 0)
        //    {

        //        this.ddlNumeroSocio.Items.Add(new ListItem(parametro.DescripcionAfiliado.ToString(), parametro.IdAfiliado.ToString()));
        //        this.ddlNumeroSocio.SelectedValue = parametro.IdAfiliado.ToString();
        //        //this.ddlNumeroSocio.SelectedIndex = parametro.DescripcionAfiliado;
        //        //this.MapearObjetoAControlesAfiliado(parametroFacturas.Afiliado);
        //        this.txtProveedor.Text = parametro.RazonSocial.ToString();
        //    }
        //    else
        //    {
        //        //this.txtSocio.Text = string.Empty;
        //        parametro.CodigoMensaje = "NumeroSocioNoExiste";
        //        this.UpdatePanel1.Update();
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}

        private void CargarCombos()
        {
            this.ddlEntidades.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Entidades);
            this.ddlEntidades.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlEntidades.DataTextField = "Descripcion";
            this.ddlEntidades.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEntidades, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosOrdenesPago));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            CapOrdenesPagos pOrdenPago = new CapOrdenesPagos();
            pOrdenPago.Entidad.IdRefEntidad = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            pOrdenPago.Entidad.IdEntidad = ddlEntidades.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEntidades.SelectedValue);
            if (pOrdenPago.Entidad.IdEntidad != 0)
            {
                button_Click(hdfIdAfiliado);
            }
            else
            {
                this.ddlNumeroSocio.Items.Clear();
                this.ddlNumeroSocio.SelectedValue = null;
                this.ddlNumeroSocio.ClearSelection();
            }

            //pOrdenPago.Entidad.IdRefEntidad = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            //if (pOrdenPago.Entidad.IdRefEntidad != 0)
            //{
            //    this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdAfiliado.Value));
            //    this.ddlNumeroSocio.SelectedValue = hdfIdAfiliado.Value;
            //}
            //else
            //{
            //    AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //}

            //VER PARAMETROS QUE RECIBE EL STORE
            pOrdenPago.IdOrdenPago = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            //pOrdenPago.Entidad.IdRefEntidad = ddlNumeroSocio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlNumeroSocio.SelectedValue);
            pOrdenPago.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //VER QUE BUSQUE CON EL POP UP
            //pOrdenPago.Beneficiario = this.txtProveedor.Text;
            //pOrdenPago.Entidad.IdEntidad = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);

            pOrdenPago.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pOrdenPago.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pOrdenPago.FilialPago.IdFilialPago = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pOrdenPago.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text;
            pOrdenPago.NumeroFactura = this.txtNumeroFactura.Text;
            pOrdenPago.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pOrdenPago.PageSize = gvDatos.VirtualItemCount;
            this.MisOrdenesPagosExcel = CuentasPagarF.OrdenesPagosObtenerListaFiltroGrilla(pOrdenPago);
            //GridViewExportUtil.Export("Datos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            Dictionary<string, string> exportColumns = new Dictionary<string, string>();
            exportColumns.Add("IdOrdenPago", "Número");
            exportColumns.Add("EntidadNombre", "Razon Social");
            exportColumns.Add("EntidadCuit", "CUIL");
            exportColumns.Add("FechaAlta", "Fecha Emision");
            exportColumns.Add("FechaPago", "Fecha Pago");
            exportData.ExportExcel(this.Page, this.MisOrdenesPagosExcel, true, "Ordenes de Pago", "Ordenes de Pago", exportColumns);
       
        }


        protected void ddlEntidades_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlEntidades.SelectedValue))
            {
                this.ddlNumeroSocio.Items.Clear();
                this.ddlNumeroSocio.SelectedValue = null;
                this.ddlNumeroSocio.ClearSelection();
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CapOrdenesPagos parametros = this.BusquedaParametrosObtenerValor<CapOrdenesPagos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAgregar.aspx"), true);
        }

        #region "Grilla"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
        
            CapOrdenesPagos op = new CapOrdenesPagos();
            op.IdOrdenPago = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdOrdenPago"].ToString());
            op.Estado.IdEstado = Convert.ToInt32(((GridView)sender).DataKeys[index]["EstadoIdEstado"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdOrdenPago", op.IdOrdenPago);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                if(op.Estado.IdEstado==(int)EstadosOrdenesPago.Pagado)
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAnularPagada.aspx"), true);
                else
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Autorizar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAutorizar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosModificar.aspx"), true);
            }
            else if (e.CommandName==Gestion.Impresion.ToString())
            {
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapOrdenesPagos, "OrdenesPago", op, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this, "OrdenesPago", this.UsuarioActivo);
                this.UpdatePanel1.Update();
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("OrdenesPagosConsultar.aspx");

                


                DataRowView dr = (DataRowView)e.Row.DataItem;

                switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                {
                    case (int)EstadosOrdenesPago.Activo:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                        ibtnAutorizar.Visible = this.ValidarPermiso("OrdenesPagosAutorizar.aspx");
                        break;
                    case (int)EstadosOrdenesPago.Autorizado:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                        break;
                    case (int)EstadosOrdenesPago.Pagado:
                        ibtnAnular.Visible = this.ValidarPermiso("OrdenesPagosAnularPagada.aspx");
                        ibtnModificar.Visible = this.ValidarPermiso("OrdenesPagosModificar.aspx");
                        break;
                    default:
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisOrdenesPagos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CapOrdenesPagos parametros = BusquedaParametrosObtenerValor<CapOrdenesPagos>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;

            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisOrdenesPagos = this.OrdenarGrillaDatos<CapOrdenesPagos>(this.MisOrdenesPagos, e);
            this.gvDatos.DataSource = this.MisOrdenesPagos;
            this.gvDatos.DataBind();
        }

        #endregion

        #region "Proveedores PopUp"

        //protected void btnLimpiar_Click(object sender, EventArgs e)
        //{
        //    CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
        //    parametros.Entidad = new TGEEntidades();
        //    this.txtProveedor.Text = string.Empty;
        //    this.BusquedaParametrosGuardarValor<CapSolicitudPago>(parametros);
        //    this.UpdatePanel1.Update();
        //}

        //protected void btnBuscarEntidad_Click(object sender, EventArgs e)
        //{
        //    if (!string.IsNullOrEmpty(this.ddlEntidades.SelectedValue))
        //    {
        //        CapOrdenesPagos parametros = this.BusquedaParametrosObtenerValor<CapOrdenesPagos>();
        //        parametros.Entidad.IdEntidad = Convert.ToInt32(this.ddlEntidades.SelectedValue);
        //        switch (parametros.Entidad.IdEntidad)
        //        {
        //            case (int)EnumTGEEntidades.Proveedores:
        //                this.ctrProveedores.IniciarControl();
        //                break;
        //            case (int)EnumTGEEntidades.Afiliados:
        //                this.ctrAfiliados.IniciarControl(true);
        //                break;
        //            default:
        //                break;
        //        }
        //        this.BusquedaParametrosGuardarValor<CapOrdenesPagos>(parametros);
        //    }
        //}

        //void ctrProveedores_ProveedoresBuscar(global::Proveedores.Entidades.CapProveedores e)
        //{
        //    CapOrdenesPagos parametros = this.BusquedaParametrosObtenerValor<CapOrdenesPagos>();
        //    parametros.Entidad.Beneficiario = e.RazonSocial;
        //    parametros.Entidad.IdRefEntidad = e.IdProveedor.Value;
        //    this.BusquedaParametrosGuardarValor<CapOrdenesPagos>(parametros);

        //    this.txtProveedor.Text = e.RazonSocial;
        //    this.UpdatePanel1.Update();
        //}

        //void ctrAfiliados_AfiliadosBuscarSeleccionar(global::Afiliados.Entidades.AfiAfiliados e)
        //{
        //    CapOrdenesPagos parametros = this.BusquedaParametrosObtenerValor<CapOrdenesPagos>();
        //    parametros.Entidad.Beneficiario = e.ApellidoNombre;
        //    parametros.Entidad.IdRefEntidad = e.IdAfiliado;
        //    this.BusquedaParametrosGuardarValor<CapOrdenesPagos>(parametros);

        //    this.txtProveedor.Text = e.ApellidoNombre;
        //    this.UpdatePanel1.Update();
        //}

        protected void ddlNumeroSocio_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlEntidades.SelectedValue))
            {
                CapOrdenesPagos parametros = this.BusquedaParametrosObtenerValor<CapOrdenesPagos>();
                parametros.Entidad.IdEntidad = Convert.ToInt32(this.ddlEntidades.SelectedValue);
                
                switch (parametros.Entidad.IdEntidad)
                {
                    case (int)EnumTGEEntidades.Proveedores:
                        CapProveedores prov = new CapProveedores();
                        prov.IdProveedor = this.ddlNumeroSocio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlNumeroSocio.SelectedValue);
                        prov = ProveedoresF.ProveedoresObtenerDatosCompletos(prov);
                        if (prov.IdProveedor > 0) { }
                            //this.ctrProveedores_ProveedoresBuscar(prov);
                        else
                            this.ctrProveedores.IniciarControl();
                        break;
                    case (int)EnumTGEEntidades.Afiliados:
                        AfiAfiliados afi = new AfiAfiliados();
                        afi.NumeroSocio = this.ddlNumeroSocio.SelectedValue;
                        afi = AfiliadosF.AfiliadosObtenerDatosCompletos(afi);
                        if (afi.IdAfiliado > 0) { }
                            //this.ctrAfiliados_AfiliadosBuscarSeleccionar(afi);
                        else
                            this.ctrAfiliados.IniciarControl(true);
                        break;
                    default:
                        break;
                }
                //this.btnLimpiar.Visible = true;
                //this.btnBuscarEntidad.Visible = false;
                this.UpdatePanel1.Update();
            }
        }

        #endregion

        private void CargarLista(CapOrdenesPagos pOrdenPago)
        {
            pOrdenPago.Entidad.IdRefEntidad = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            pOrdenPago.Entidad.IdEntidad = ddlEntidades.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEntidades.SelectedValue);
            if (pOrdenPago.Entidad.IdEntidad != 0)
            {
                button_Click(hdfIdAfiliado);
            }
            else
            {
                this.ddlNumeroSocio.Items.Clear();
                this.ddlNumeroSocio.SelectedValue = null;
                this.ddlNumeroSocio.ClearSelection();
            }

            //pOrdenPago.Entidad.IdRefEntidad = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            //if (pOrdenPago.Entidad.IdRefEntidad != 0)
            //{
            //    this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdAfiliado.Value));
            //    this.ddlNumeroSocio.SelectedValue = hdfIdAfiliado.Value;
            //}
            //else
            //{
            //    AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //}

            //VER PARAMETROS QUE RECIBE EL STORE
            pOrdenPago.IdOrdenPago = this.txtNumero.Text ==string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            //pOrdenPago.Entidad.IdRefEntidad = ddlNumeroSocio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlNumeroSocio.SelectedValue);
            pOrdenPago.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //VER QUE BUSQUE CON EL POP UP
            //pOrdenPago.Beneficiario = this.txtProveedor.Text;
            //pOrdenPago.Entidad.IdEntidad = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);
       
            pOrdenPago.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pOrdenPago.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pOrdenPago.FilialPago.IdFilialPago = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pOrdenPago.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text;
            pOrdenPago.NumeroFactura = this.txtNumeroFactura.Text;
            pOrdenPago.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pOrdenPago.BusquedaParametros = true;
            pOrdenPago.PageSize = UsuarioActivo.PageSize > 0 ? UsuarioActivo.PageSize : 25;
            gvDatos.PageSize = UsuarioActivo.PageSize > 0 ? UsuarioActivo.PageSize : 25;

           
            this.BusquedaParametrosGuardarValor<CapOrdenesPagos>(pOrdenPago);
            this.MisOrdenesPagos = CuentasPagarF.OrdenesPagosObtenerListaFiltroGrilla(pOrdenPago);

            if (this.MisOrdenesPagos.Rows.Count > 0)
            {
                btnExportarExcel.Visible = true;
            }
            else
            {
                btnExportarExcel.Visible = false;

            }

            this.gvDatos.DataSource = this.MisOrdenesPagos;
            this.gvDatos.VirtualItemCount = MisOrdenesPagos.Rows.Count > 0 ? Convert.ToInt32(MisOrdenesPagos.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
        }
    }
}
