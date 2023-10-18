using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cobros.Entidades;
using Generales.FachadaNegocio;
using CuentasPagar.Entidades;
using Comunes.Entidades;
using Comunes;
using Cobros;
using System.Collections;
using Afiliados.Entidades;
using Afiliados;
using Generales.Entidades;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System.Data;
using CrystalDecisions.Web;
using System.IO;
using Comunes.LogicaNegocio;
using CrystalDecisions.Shared;
using Prestamos;
using Prestamos.Entidades;
using System.Net.Mail;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosFacturasListar : PaginaSegura
    {
   
        private DataTable MisOrdenesCobros
        {
            get { return (DataTable)Session[this.MiSessionPagina + "OrdenesCobrosListarCobOrdenesCobros"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosListarCobOrdenesCobros"] = value; }
        }
        private DataTable MisOrdenesCobrosExcel
        {
            get { return (DataTable)Session[this.MiSessionPagina + "OrdenesCobrosListarCobOrdenesCobrosExcel"]; }
            set { Session[this.MiSessionPagina + "OrdenesCobrosListarCobOrdenesCobrosExcel"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            //this.ctrAfiliados.AfiliadosBuscarSeleccionar += new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAgregar.aspx");
                this.CargarCombos();
                this.txtFechaDesde.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
                this.txtFechaHasta.Text = DateTime.Now.ToShortDateString();

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumero, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigoSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPrefijoNumeroFactura, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroFactura, this.btnBuscar);

                CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumero.Text = parametros.IdOrdenCobro == 0 ? String.Empty : parametros.IdOrdenCobro.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    if(parametros.Afiliado.IdAfiliado > 0)
                    {
                        AfiAfiliados afi = new AfiAfiliados();
                        afi.IdAfiliado = parametros.Afiliado.IdAfiliado;
                        afi = AfiliadosF.AfiliadosObtenerDatos(afi);
                        this.ddlNumeroSocio.Items.Add(new ListItem(afi.DescripcionAfiliado.ToString(), afi.IdAfiliado.ToString()));
                        this.hdfIdAfiliado.Value = afi.IdAfiliado > 0 ? afi.IdAfiliado.ToString() : string.Empty;
                        this.hdfRazonSocial.Value =  afi.DescripcionAfiliado.ToString();
                        this.hdfCuit.Value = afi.CUIL.ToString();

                        this.ddlNumeroSocio.SelectedValue = parametros.Afiliado.IdAfiliado.ToString();
                    }
                    this.txtCuit.Text = parametros.Afiliado.CUILFormateado;
                    this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    this.txtNumeroFactura.Text = parametros.NumeroFactura;
                    this.CargarLista(parametros);
                }
            }
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAgregar.aspx"), true);
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            CobOrdenesCobros pOrdenCobro = new CobOrdenesCobros();
            pOrdenCobro.Afiliado.IdAfiliado = hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(hdfIdAfiliado.Value); //this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pOrdenCobro.Afiliado.IdAfiliado != 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = hdfIdAfiliado.Value;
                txtCuit.Text = hdfCuit.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                txtCuit.Text = string.Empty;
            }
            pOrdenCobro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pOrdenCobro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pOrdenCobro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pOrdenCobro.IdOrdenCobro = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            pOrdenCobro.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text;
            pOrdenCobro.NumeroFactura = this.txtNumeroFactura.Text;
            pOrdenCobro.BusquedaParametros = true;
            pOrdenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pOrdenCobro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            pOrdenCobro.PageSize = gvDatos.VirtualItemCount;
            gvDatos.PageIndex = pOrdenCobro.PageIndex;
            this.MisOrdenesCobrosExcel = CobrosF.OrdenesCobrosFacturaListaFiltro(pOrdenCobro);
            //GridViewExportUtil.Export("Datos.xls", this.gvDatos);
            ExportData exportData = new ExportData();
            Dictionary<string, string> exportColumns = new Dictionary<string, string>();
            exportColumns.Add("IdOrdenCobro", "Número");
            exportColumns.Add("RazonSocial", "Razon Social");
            exportColumns.Add("AfiliadoCUIL", "CUIL");
            exportColumns.Add("TipoOperacionTipoOperacion", "Tipo operacion");
            exportColumns.Add("Detalle", "Detalle");
            exportColumns.Add("NumeroReciboCompleto", "Nro. Recibo");
            exportColumns.Add("FechaEmision", "Fecha Emision");
            exportColumns.Add("FechaConfirmacion", "Fecha Cobro");
            exportColumns.Add("ImporteBruto", "Importe Total");
            exportColumns.Add("EstadoDescripcion", "Estado");
            exportData.ExportExcel(this, this.MisOrdenesCobrosExcel, true, "Ordenes de Cobro", "Ordenes de Cobro", exportColumns);
        }

        #region "Grilla"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
            ordenCobro.IdOrdenCobro = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdOrdenCobro"].ToString());
            ordenCobro.Estado.IdEstado= Convert.ToInt32(((GridView)sender).DataKeys[index]["EstadoIdEstado"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdOrdenCobro", ordenCobro.IdOrdenCobro);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                if(ordenCobro.Estado.IdEstado==(int)EstadosOrdenesCobro.Cobrado)
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAnularConfirmada.aspx"), true);
                else
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.EnviarMail.ToString())
            {
                MailMessage mail = new MailMessage();
                ordenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                ordenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(ordenCobro);
                if (CobrosF.OrdenesCobroArmarMail(ordenCobro, mail))
                {
                    this.popUpMail.IniciarControl(mail, ordenCobro);
                }
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                //TGEArchivos archivo = new TGEArchivos();
                //archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, "CobOrdenesCobros", ordenCobro, AyudaProgramacion.ObtenerDatosUsuario( this.UsuarioActivo));

                //TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                //string nombreArchivo = string.Concat(empresa.CUIT, "_", ordenCobro.TipoOperacion.TipoOperacion.Replace(" ", ""), "_", ordenCobro.IdOrdenCobro, ".pdf");
                //archivo.NombreArchivo = nombreArchivo;
                //ExportPDF.ConvertirArchivoEnPdf(this.upGrilla, archivo);
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, "OrdenesCobros", ordenCobro, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this, "OrdenesCobros", this.UsuarioActivo);
                this.upEntidades.Update();



                //if (ordenCobro.Prestamo.IdPrestamo != 0)
                //{
                //    #region COMPROBANTE ORDEN/PRESTAMO
                //    List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                //    TGEArchivos archivo = new TGEArchivos();
                //    TGEArchivos archivoPre = new TGEArchivos();
                //    //CobOrdenesCobros ordenCobroPdf = FacturasF.FacturasObtenerArchivo(factura);
                //    byte[] pdfOrden;
                //    byte[] pdfPrestamo;

                //    TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.CobOrdenesCobros);
                //    string archivoReporteLeer = string.Concat(this.ObtenerAppPath(), comprobante.NombreRPT);

                //    RepReportes pReporte = new RepReportes();
                //    pReporte.StoredProcedure = comprobante.NombreSP;
                //    RepParametros param = new RepParametros();
                //    param.NombreParametro = "IdOrdenCobro";
                //    param.Parametro = "IdOrdenCobro";
                //    param.ValorParametro = ordenCobro.IdOrdenCobro.ToString();
                //    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                //    pReporte.Parametros.Add(param);
                //    DataSet dataSet = ReportesF.ReportesObtenerDatos(pReporte);

                //    CrystalReportSource CryReportSource = new CrystalReportSource();
                //    CryReportSource.CacheDuration = 1;
                //    CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer);
                //    CryReportSource.ReportDocument.SetDataSource(dataSet);
                //    Stream reciboCom = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                //    CryReportSource.ReportDocument.Close();
                //    CryReportSource.Dispose();
                //    pdfOrden = AyudaProgramacionLN.StreamToByteArray(reciboCom);
                //    //ordenCobroPdf.IdOrdenCobro = ordenCobro.IdOrdenCobro;
                //    archivo.Archivo = pdfOrden;
                //    listaArchivos.Add(archivo);

                //    //// AHORA CARGO EL PRESTAMO
                //    TGEComprobantes comprobantePrestamo = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.PrePrestamos);
                //    string archivoReporteLeer2 = string.Concat(this.ObtenerAppPath(), comprobantePrestamo.NombreRPT);

                //    RepReportes pReportePres = new RepReportes();
                //    pReportePres.StoredProcedure = comprobantePrestamo.NombreSP;
                //    RepParametros paramPre = new RepParametros();
                //    paramPre.NombreParametro = "IdPrestamo";
                //    paramPre.Parametro = "IdPrestamo";
                //    paramPre.ValorParametro = ordenCobro.Prestamo.IdPrestamo.ToString();
                //    paramPre.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                //    pReportePres.Parametros.Add(paramPre);
                //    DataSet dataSetPre = ReportesF.ReportesObtenerDatos(pReportePres);

                //    CryReportSource = new CrystalReportSource();
                //    CryReportSource.CacheDuration = 1;
                //    CryReportSource.Report.FileName = Server.MapPath(archivoReporteLeer2);
                //    CryReportSource.ReportDocument.SetDataSource(dataSetPre);
                //    Stream reciboPre = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                //    CryReportSource.ReportDocument.Close();
                //    CryReportSource.Dispose();
                //    pdfPrestamo = AyudaProgramacionLN.StreamToByteArray(reciboPre);
                //    //ordenCobroPdf.IdOrdenCobro = ordenCobro.IdOrdenCobro;
                //    archivoPre.Archivo = pdfPrestamo;
                //    listaArchivos.Add(archivoPre);

                //    //Levanto la Factura y el Remito
                //    listaArchivos.AddRange(CobrosF.OrdenesCobrosObtenerArchivos(ordenCobro));

                //    string nombre = string.Concat(ordenCobro.GetType().Name, "_", ordenCobro.IdOrdenCobro.ToString().PadLeft(10, '0'));
                //    nombre = string.Format("{0}.pdf", nombre);

                //    this.ctrPopUpComprobantes.CargarArchivo(listaArchivos, nombre);
                //    #endregion
                //}
                //else
                //{
                //    this.ctrPopUpComprobantes.CargarReporte(ordenCobro, EnumTGEComprobantes.CobOrdenesCobros);
                //}
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                ibtnConsultar.Visible = this.ValidarPermiso("OrdenesCobrosFacturasConsultar.aspx");
                ibtnAnular.Visible = false;

                //CobOrdenesCobros ordenCobro = (CobOrdenesCobros)e.Row.DataItem;

                DataRowView dr = (DataRowView)e.Row.DataItem;
                if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosOrdenesCobro.Activo)
                {
                    ibtnAnular.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAnular.aspx");
                }
                else if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosOrdenesCobro.Cobrado)
                {
                    ibtnAnular.Visible = this.ValidarPermiso("OrdenesCobrosFacturasAnularConfirmada.aspx");
                    ibtnModificar.Visible = false;
                }
                else
                {
                    ibtnModificar.Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //int cellCount = e.Row.Cells.Count;
                //e.Row.Cells.Clear();
                //TableCell tableCell = new TableCell();
                //tableCell.ColumnSpan = cellCount;
                //tableCell.HorizontalAlign = HorizontalAlign.Right;
                //tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisOrdenesCobros.Count);
                //e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;

            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisOrdenesCobros = this.OrdenarGrillaDatos<CobOrdenesCobros>(this.MisOrdenesCobros, e);
            this.gvDatos.DataSource = this.MisOrdenesCobros;
            this.gvDatos.DataBind();
        }

        #endregion

        //protected void btnBuscarCliente_Click(object sender, EventArgs e)
        //{
        //    this.ctrAfiliados.IniciarControl(true);
        //}

        //protected void ctrAfiliados_AfiliadosBuscarSeleccionar(global::Afiliados.Entidades.AfiAfiliados e)
        //{
        //    //this.PropiedadGuardarValor("gblIdAfiliado", e.IdAfiliado);
        //    CobOrdenesCobros parametros = this.BusquedaParametrosObtenerValor<CobOrdenesCobros>();
        //    parametros.BusquedaParametros = true;
        //    parametros.Afiliado.IdAfiliado = e.IdAfiliado;
        //    this.BusquedaParametrosGuardarValor<CobOrdenesCobros>(parametros);

        //    this.txtCodigoSocio.Text = e.IdAfiliado.ToString();
        //    this.txtRazonSocial.Text = e.ApellidoNombre;
        //    this.txtCuit.Text = e.CUILFormateado;
        //    this.upEntidades.Update();
        //}

        //protected void txtCodigoSocio_TextChanged(object sender, EventArgs e)
        //{
        //    AfiAfiliados afiliado = new AfiAfiliados();
        //    afiliado.IdAfiliado = this.txtCodigoSocio.Text==string.Empty? 0 : Convert.ToInt32(this.txtCodigoSocio.Text);
        //    afiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(afiliado);
        //    if (afiliado.IdAfiliado > 0)
        //        this.ctrAfiliados_AfiliadosBuscarSeleccionar(afiliado);
        //    else
        //        this.ctrAfiliados.IniciarControl(true);
        //}

        //protected void button_Click(object sender, EventArgs e)
        //{
        //    string txtNumeroSocio = this.hdfIdAfiliado.Value;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = txtNumeroSocio == string.Empty ? 0 : Convert.ToInt32(txtNumeroSocio);
        //    //parametro = AfiliadosF.AfiliadosObtenerDatos(parametro);
        //    if (parametro.IdAfiliado != 0)
        //    {

        //        this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, parametro.IdAfiliado.ToString()));
        //        this.ddlNumeroSocio.SelectedValue = parametro.IdAfiliado.ToString();
        //        this.txtCuit.Text = hdfCuit.Value;
        //        //this.ddlNumeroSocio.SelectedIndex = parametro.DescripcionAfiliado;
        //        //this.MapearObjetoAControlesAfiliado(parametroFacturas.Afiliado);
        //    }
        //    else
        //    {
        //        AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        //        this.txtCuit.Text = string.Empty;
        //        //this.txtSocio.Text = string.Empty;
        //        //parametro.CodigoMensaje = "NumeroSocioNoExiste";
        //        //this.upEntidades.Update();
        //        //this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosOrdenesCobro));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();
        }

        private void CargarLista(CobOrdenesCobros pOrdenCobro)
        {
            
            //if (hdfIdAfiliado.Value == string.Empty)
            //{
            //    if (pOrdenCobro.Afiliado.IdAfiliado != 0)
            //        hdfIdAfiliado.Value = pOrdenCobro.Afiliado.IdAfiliado.ToString();
            //}
            pOrdenCobro.Afiliado.IdAfiliado = hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(hdfIdAfiliado.Value); //this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pOrdenCobro.Afiliado.IdAfiliado != 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = hdfIdAfiliado.Value;
                txtCuit.Text = hdfCuit.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                txtCuit.Text = string.Empty;
            }
            pOrdenCobro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pOrdenCobro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pOrdenCobro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pOrdenCobro.IdOrdenCobro = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            pOrdenCobro.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text;
            pOrdenCobro.NumeroFactura = this.txtNumeroFactura.Text;
            pOrdenCobro.BusquedaParametros = true;
            pOrdenCobro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pOrdenCobro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pOrdenCobro.PageSize;
            gvDatos.PageIndex = pOrdenCobro.PageIndex;
            this.BusquedaParametrosGuardarValor<CobOrdenesCobros>(pOrdenCobro);
            this.MisOrdenesCobros = CobrosF.OrdenesCobrosFacturaListaFiltro(pOrdenCobro);

            if (this.MisOrdenesCobros.Rows.Count > 0)
            {
                btnExportarExcel.Visible = true;
            }
            else
            {
                btnExportarExcel.Visible = false;

            }
            this.gvDatos.DataSource = this.MisOrdenesCobros;
            this.gvDatos.VirtualItemCount = MisOrdenesCobros.Rows.Count > 0 ? Convert.ToInt32(MisOrdenesCobros.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
            this.upGrilla.Update();
        }
    }
}