using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using System.Collections;
using Comunes.Entidades;
using Facturas;
using Generales.FachadaNegocio;
using Afiliados.Entidades;
using Afiliados;
using Generales.Entidades;

namespace IU.Modulos.Facturas
{
    public partial class RemitosListar : PaginaSegura
    {
        private DataTable MisRemitos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "RemitosListarMisRemitos"]; }
            set { Session[this.MiSessionPagina + "RemitosListarMisRemitos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrBuscarClientePopUp.AfiliadosBuscarSeleccionar +=new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar);
            if (!this.IsPostBack)
            {

                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoRemito, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("RemitosAgregar.aspx");
                this.CargarCombos();

                VTARemitos parametros = this.BusquedaParametrosObtenerValor<VTARemitos>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlNumeroSocio.SelectedValue = parametros.Afiliado.IdAfiliado.ToString();
                    this.txtNumeroRemitoPrefijo.Text = parametros.NumeroRemitoPrefijo;
                    this.txtNumeroRemitoSuFijo.Text = parametros.NumeroRemitoSuFijo;
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                   
                    
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            VTARemitos parametros = this.BusquedaParametrosObtenerValor<VTARemitos>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTARemitos remito = new VTARemitos();
            remito.IdRemito = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdRemito"].ToString());
            remito.TipoFactura.CodigoValor = ((GridView)sender).DataKeys[index]["TipoFacturaCodigoValor"].ToString();
            remito.NumeroRemitoPrefijo = ((GridView)sender).DataKeys[index]["NumeroRemitoPrefijo"].ToString();
            remito.NumeroRemitoSuFijo = ((GridView)sender).DataKeys[index]["NumeroRemitoSuFijo"].ToString();

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdRemito", remito.IdRemito);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                VTARemitos remitoPDF = new VTARemitos();
                remitoPDF.IdRemito = remito.IdRemito;
                remitoPDF.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                remitoPDF = FacturasF.RemitosObtenerArchivo(remitoPDF);
                List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                TGEArchivos archivo = new TGEArchivos();
                archivo.Archivo = remitoPDF.RemitoPDF;
                if (archivo.Archivo != null)
                    listaArchivos.Add(archivo);
                TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                string nombreArchivo = string.Concat(empresa.CUIT, "_", remito.TipoFactura.CodigoValor, "_", remito.NumeroRemitoPrefijo, "_", remito.NumeroRemitoSuFijo, ".pdf");
                ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel1, listaArchivos, nombreArchivo);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton baja = (ImageButton)e.Row.FindControl("btnBaja");
               
                //Permisos btnEliminar

                ibtnConsultar.Visible = this.ValidarPermiso("RemitosConsultar.aspx");
                DataRowView dr = (DataRowView)e.Row.DataItem;

                switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                {
                    case (int)EstadosRemitos.PendienteEntrega:
                    case (int)EstadosRemitos.EnDistribucion:
                    case (int)EstadosRemitos.EnDespacho:
                        baja.Visible = this.ValidarPermiso("RemitosAnular.aspx");
                        modificar.Visible = this.ValidarPermiso("RemitosModificar.aspx");
                        break;
                    case (int)EstadosRemitos.Entregado:
                        baja.Visible = this.ValidarPermiso("RemitosAnular.aspx");
                        modificar.Visible = this.ValidarPermiso("RemitosModificar.aspx");
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisRemitos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTARemitos parametros = this.BusquedaParametrosObtenerValor<VTARemitos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTARemitos>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisRemitos;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisRemitos = this.OrdenarGrillaDatos<DataTable>(this.MisRemitos, e);
            this.gvDatos.DataSource = this.MisRemitos;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisRemitos;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("VTARemitos");
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        #region "PopUp Afiliados"
        //void ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(AfiAfiliados pAfiliado)
        //{
        //    this.MapearObjetoAControlesAfiliado(pAfiliado);
        //    VTARemitos parametros = this.BusquedaParametrosObtenerValor<VTARemitos>();
        //    parametros.Afiliado = pAfiliado;
        //    this.BusquedaParametrosGuardarValor<VTARemitos>(parametros);
        //    this.btnLimpiar.Visible = true;
        //    this.btnBuscarSocio.Visible = false;
        //    this.pnlAfiliados.Update();
        //}

        //private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        //{
        //    this.txtNumeroSocio.Text = pAfiliado.IdAfiliado.ToString();
        //    this.txtSocio.Text = pAfiliado.Apellido;
        //}

        //protected void txtNumeroSocio_TextChanged(object sender, EventArgs e)
        //{
        //    string txtNumeroSocio = ((TextBox)sender).Text;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = Convert.ToInt32(txtNumeroSocio);
        //    parametro = AfiliadosF.AfiliadosObtenerDatosCompletos(parametro);
        //    if (parametro.IdAfiliado != 0)
        //    {
        //        this.ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(parametro);
        //    }
        //    else
        //    {
        //        parametro.CodigoMensaje = "NumeroSocioNoExiste";
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}

        //protected void btnBuscarCliente_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarClientePopUp.IniciarControl(true);
        //}

        //protected void btnLimpiar_Click(object sender, EventArgs e)
        //{
        //    VTARemitos parametros = this.BusquedaParametrosObtenerValor<VTARemitos>();
        //    parametros.Afiliado = new AfiAfiliados();
        //    this.txtNumeroSocio.Text = string.Empty;
        //    this.txtSocio.Text = string.Empty;
        //    this.BusquedaParametrosGuardarValor<VTARemitos>(parametros);
        //    this.btnLimpiar.Visible = false;
        //    this.btnBuscarSocio.Visible = true;
        //    this.pnlAfiliados.Update();
        //}
        #endregion

        #region AfiliadosAjax
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
        //    }
        //    else
        //    {
        //        //this.txtSocio.Text = string.Empty;
        //        parametro.CodigoMensaje = "NumeroSocioNoExiste";
        //        this.UpdatePanel1.Update();
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}


        //private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        //{
        //    this.ddlNumeroSocio.Items.Add(new ListItem(pAfiliado.DescripcionAfiliado, pAfiliado.IdAfiliado.ToString()));
        //    this.ddlNumeroSocio.SelectedValue = pAfiliado.IdAfiliado.ToString();

        //    //if (this.MisParametrosUrl.Contains("IdAfiliado"))
        //    //    this.MisParametrosUrl["IdAfiliado"] = pAfiliado.IdAfiliado;

        //    //this.txtSocio.Text = pAfiliado.RazonSocial;
        //}
        #endregion

        private void CargarLista(VTARemitos pRemito)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pRemito.Afiliado.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pRemito.Afiliado.IdAfiliado != 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = hdfIdAfiliado.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            pRemito.NumeroRemitoSuFijo = this.txtNumeroRemitoSuFijo.Text.ToString();
            pRemito.NumeroRemitoPrefijo = this.txtNumeroRemitoPrefijo.Text;
            pRemito.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);

            pRemito.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pRemito.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
                        
            pRemito.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<VTARemitos>(pRemito);
            this.MisRemitos = FacturasF.RemitosObtenerGrilla(pRemito);
            this.gvDatos.DataSource = this.MisRemitos;
            this.gvDatos.PageIndex = pRemito.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisRemitos.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}