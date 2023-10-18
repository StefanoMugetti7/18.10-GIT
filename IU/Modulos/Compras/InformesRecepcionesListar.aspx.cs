using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using System.Collections;
using Proveedores.Entidades;
using Compras;
using Proveedores;

namespace IU.Modulos.Compras
{
    public partial class InformesRecepcionesListar : PaginaSegura
    {
        private List<CmpInformesRecepciones> MisInformes
        {
            get { return (List<CmpInformesRecepciones>)Session[this.MiSessionPagina + "InformesRecepcionesListarMisInformes"]; }
            set { Session[this.MiSessionPagina + "InformesRecepcionesListarMisInformes"] = value; }
        }

        public CapProveedores MiProveedor
        {
            get { return (CapProveedores)Session[this.MiSessionPagina + "ProveedorCabeceraDatosMiProveedor"]; }
            set { Session[this.MiSessionPagina + "ProveedorCabeceraDatosMiProveedor"] = value; }
        }

        public delegate void ProveedoresDatosCabeceraAjaxEventHandler(CapProveedores e);
        public event ProveedoresDatosCabeceraAjaxEventHandler BuscarProveedor;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrBuscarProveedorPopUp.ProveedoresBuscar += new CuentasPagar.Controles.ProveedoresBuscarPopUp.ProveedoresBuscarEventHandler(ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoInforme, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoOrden, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("InformesRecepcionesAgregar.aspx");
                this.btnAgregarAbierta.Visible = this.ValidarPermiso("InformesRecepcionesAbiertaAgregar.aspx");
                this.CargarCombos();

                CmpInformesRecepciones parametros = this.BusquedaParametrosObtenerValor<CmpInformesRecepciones>();
                if (parametros.BusquedaParametros)
                {
                    this.txtCodigoInforme.Text = parametros.IdInformeRecepcion == 0 ? String.Empty : parametros.IdInformeRecepcion.ToString();
                    this.txtCodigoOrden.Text = parametros.OrdenCompra.IdOrdenCompra == 0 ? String.Empty : parametros.OrdenCompra.IdOrdenCompra.ToString();

                    //this.txtFechaRemito.Text = parametros.FechaEmision.ToShortDateString();
                    //this.txtPreNumeroRemito.Text = parametros.NumeroRemitoPrefijo;
                    //this.txtNumeroRemito.Text = parametros.NumeroRemitoSufijo;

                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    //this.txtCodigo.Text = parametros.Proveedor.IdProveedor.ToString();
                    //this.txtProveedor.Text = parametros.Proveedor.RazonSocial;

                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CmpInformesRecepciones parametros = this.BusquedaParametrosObtenerValor<CmpInformesRecepciones>();
            CargarLista(parametros);
        }

        protected void btnAgregarAbierta_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAbiertaAgregar.aspx"), true);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAgregar.aspx"), true);
        }

        //protected void button_Click(object sender, EventArgs e)
        //{


        //    string txtNumeroProveedor = this.hdfIdProveedor.Value;
        //    MiProveedor = new CapProveedores();
        //    MiProveedor.IdProveedor = txtNumeroProveedor == string.Empty ? 0 : Convert.ToInt32(txtNumeroProveedor);
        //    //MiProveedor.RazonSocial = this.hdfNumeroProveedor.Value;
        //    MiProveedor = ProveedoresF.ProveedoresObtenerDatosCompletos(MiProveedor);

        //    if (MiProveedor.IdProveedor != 0)
        //    {
        //        this.MapearObjetoAControlesProveedor(MiProveedor);
        //    }
        //    else
        //    {
        //        this.txtCUIT.Text = string.Empty;
        //        this.ddlEstados.SelectedValue = string.Empty;
        //        MiProveedor.CodigoMensaje = "El Proveedor no existe";
        //        //this.UpdatePanel2.Update();
        //        this.MostrarMensaje(MiProveedor.CodigoMensaje, true);
        //    }
        //    BuscarProveedor?.Invoke(MiProveedor);
        //}

        private void CargarCombos()
        {

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosOrdenesCompras));
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
                //|| e.CommandName == Gestion.Autorizar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CmpInformesRecepciones pInforme = this.MisInformes[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdInformeRecepcion", pInforme.IdInformeRecepcion);

            if (pInforme.OrdenCompra.IdOrdenCompra > 0)
            {
                if (e.CommandName == Gestion.Anular.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAnular.aspx"), true);
                }
                else if (e.CommandName == Gestion.Consultar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesConsultar.aspx"), true);
                }
            }
            else
            {
                if (e.CommandName == Gestion.Anular.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAbiertaAnular.aspx"), true);
                }
                else if (e.CommandName == Gestion.Consultar.ToString())
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAbiertaConsultar.aspx"), true);
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                CmpInformesRecepciones informe = (CmpInformesRecepciones)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");                
                ImageButton ibtnConsultarAbierta = (ImageButton)e.Row.FindControl("btnConsultarAbierta");
                ImageButton anularAbierta = (ImageButton)e.Row.FindControl("btnAnularAbierta");

                if (informe.OrdenCompra.IdOrdenCompra > 0)
                {
                    ibtnConsultar.Visible = this.ValidarPermiso("InformesRecepcionesConsultar.aspx");
                    switch (informe.Estado.IdEstado)
                    {
                        case (int)EstadosInformesRecepciones.Activo:
                            anular.Visible = this.ValidarPermiso("InformesRecepcionesAnular.aspx");
                            break;
                    }
                }
                else
                {
                    ibtnConsultarAbierta.Visible = this.ValidarPermiso("InformesRecepcionesAbiertaConsultar.aspx");
                    switch (informe.Estado.IdEstado)
                    {
                        case (int)EstadosInformesRecepciones.Activo:
                            anularAbierta.Visible = this.ValidarPermiso("InformesRecepcionesAbiertaAnular.aspx");
                            break;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisInformes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CmpInformesRecepciones parametros = this.BusquedaParametrosObtenerValor<CmpInformesRecepciones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CmpInformesRecepciones>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisInformes;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisInformes = this.OrdenarGrillaDatos<CmpInformesRecepciones>(this.MisInformes, e);
            this.gvDatos.DataSource = this.MisInformes;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisInformes;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        #endregion

        #region "Proveedores PopUp"

        //protected void btnLimpiar_Click(object sender, EventArgs e)
        //{
        //    CmpInformesRecepciones parametros = this.BusquedaParametrosObtenerValor<CmpInformesRecepciones>();
        //    parametros.Proveedor = new CapProveedores();
        //    //this.txtCodigo.Text = string.Empty;
        //    //this.txtProveedor.Text = string.Empty;
        //    this.BusquedaParametrosGuardarValor<CmpInformesRecepciones>(parametros);
        //    this.UpdatePanel1.Update();
        //}

        //protected void btnBuscarProveedor_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarProveedorPopUp.IniciarControl();
        //}

        //void ctrBuscarProveedorPopUp_ProveedoresBuscarSeleccionar(CapProveedores pProveedor)
        //{
        //    CmpInformesRecepciones parametros = this.BusquedaParametrosObtenerValor<CmpInformesRecepciones>();
        //    parametros.Proveedor.RazonSocial = pProveedor.RazonSocial;
        //    parametros.Proveedor.IdProveedor = pProveedor.IdProveedor.Value;
        //    this.BusquedaParametrosGuardarValor<CmpInformesRecepciones>(parametros);
        //    this.MapearObjetoAControlesProveedor(pProveedor);
        //    this.UpdatePanel1.Update();
        //}

        //private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        //{
        //    this.txtCodigo.Text = pProveedor.IdProveedor.Value.ToString();
        //    this.txtProveedor.Text = pProveedor.RazonSocial;
        //}
        //private void MapearObjetoAControlesProveedor(CapProveedores pProveedor)
        //{
        //    //this.txtCodigo.Text = pProveedor.IdProveedor.Value.ToString();
        //    ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
        //    parametros.Agregar("IdProveedor", pProveedor.IdProveedor);

        //    this.ddlNumeroProveedor.Items.Add(new ListItem(pProveedor.RazonSocial, pProveedor.IdProveedor.ToString()));
        //    this.ddlNumeroProveedor.SelectedValue = pProveedor.IdProveedor.ToString();
        //    this.txtCUIT.Text = pProveedor.CUIT.ToString();
        //    //string proovedor = hdfNumeroProveedor.Value.ToString();

        //}

        #endregion

        private void CargarLista(CmpInformesRecepciones pInforme)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pInforme.IdInformeRecepcion = this.txtCodigoInforme.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoInforme.Text);
            pInforme.OrdenCompra.IdOrdenCompra = this.txtCodigoOrden.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigoOrden.Text);
            
            //pInforme.Proveedor.IdProveedor = ddlNumeroProveedor.SelectedValue == string.Empty ? 0 : Convert.ToInt32( ddlNumeroProveedor.SelectedValue);
            //CORREGIR MAPPER Y BORRAR:
            //pInforme.OrdenCompra.Proveedor.IdProveedor = this.txtCodigo.Text == String.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text);
            //VER QUE BUSQUE CON EL POP UP

            pInforme.Proveedor.IdProveedor = hdfIdProveedor.Value == string.Empty ? 0 : Convert.ToInt32(hdfIdProveedor.Value); //this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pInforme.Proveedor.IdProveedor != 0)
            {
                ddlNumeroProveedor.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdProveedor.Value));
                ddlNumeroProveedor.SelectedValue = hdfIdProveedor.Value;
                txtCUIT.Text = hdfCuit.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(ddlNumeroProveedor, ObtenerMensajeSistema("SeleccioneOpcion"));
                txtCUIT.Text = string.Empty;
            }

            pInforme.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pInforme.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pInforme.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CmpInformesRecepciones>(pInforme);
            this.MisInformes = ComprasF.InformesRecepcionesObtenerLista(pInforme);
            this.gvDatos.DataSource = this.MisInformes;
            this.gvDatos.PageIndex = pInforme.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisInformes.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}