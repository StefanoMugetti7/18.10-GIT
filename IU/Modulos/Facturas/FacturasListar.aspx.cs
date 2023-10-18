using Comunes.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Facturas
{
    public partial class FacturasListar : PaginaSegura
    {
        private DataTable MisFacturas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "FacturasListarMisFacturas"]; }
            set { Session[this.MiSessionPagina + "FacturasListarMisFacturas"] = value; }
        }
        private DataTable MisFacturasExcel
        {
            get { return (DataTable)Session[this.MiSessionPagina + "FacturasListarMisFacturasExcel"]; }
            set { Session[this.MiSessionPagina + "FacturasListarMisFacturasExcel"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrBuscarClientePopUp.AfiliadosBuscarSeleccionar += new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar);
            //this.ctrEnviarMails.ArmarMail += new Comunes.EnviarMails.ArmarMailEventHandler(this.ctrEnviarMails_ArmarMail);
            //this.ctrEnviarMails.IniciarProceso += new Comunes.EnviarMails.IniciarProcesoEventHandler(this.ctrEnviarMails_IniciarProceso);
            //this.ctrEnviarMails.FinalizarProceso += new Comunes.EnviarMails.FinalizarProcesoEventHandler(this.ctrEnviarMails_FinalizarProceso);

            this.gvDatos.PageSizeEvent += this.GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroFactura, this.btnBuscar);

                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarValidarFE"));
                this.btnValidarFacturasElectronicas.Attributes.Add("OnClick", funcion);

                this.btnAgregar.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
                this.CargarCombos();

                VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
                if (this.MisParametrosUrl.Contains("IdFacturaLoteEnviado"))
                {
                    this.ddlFacturasLotes.SelectedValue = this.MisParametrosUrl["IdFacturaLoteEnviado"].ToString();
                    parametros.IdFacturaLoteEnviado = Convert.ToInt32(this.MisParametrosUrl["IdFacturaLoteEnviado"]);
                    parametros.BusquedaParametros = true;
                    this.MisParametrosUrl = new Hashtable();
                }
                if (parametros.BusquedaParametros)
                {
                    this.ddlNumeroSocio.SelectedValue = parametros.Afiliado.IdAfiliado.ToString();
                    this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    this.txtNumeroFactura.Text = parametros.NumeroFactura;
                    this.ddlTipoFactura.SelectedValue = parametros.TipoFactura.IdTipoFactura.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.ddlFilial.SelectedValue = parametros.Filial.IdFilial == 0 ? string.Empty : parametros.Filial.IdFilial.ToString();
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlFacturasLotes.SelectedValue = parametros.IdFacturaLoteEnviado == 0 ? string.Empty : parametros.IdFacturaLoteEnviado.ToString();
                    this.CargarLista(parametros);
                }
            }
            //else
            //{
            //    this.PersistirDatos();
            //}
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        //void ctrEnviarMails_FinalizarProceso()
        //{
        //    this.btnBuscar_Click(this.btnBuscar, EventArgs.Empty);
        //    this.UpdatePanel1.Update();
        //}
        //void ctrEnviarMails_IniciarProceso(ref List<Objeto> listaEnvio)
        //{
        //    DataView vista = new DataView(this.MisFacturas);
        //    vista.RowFilter = " Incluir = 1";
        //    VTAFacturas factura;
        //    foreach (DataRowView row in vista)
        //    {
        //        factura = new VTAFacturas();
        //        factura.IdFactura = Convert.ToInt32(row["IdFactura"]);
        //        factura.Filtro = row["AfiliadoRazonSocial"].ToString();
        //        listaEnvio.Add(factura);
        //        factura.IndiceColeccion = listaEnvio.IndexOf(factura);
        //    }
        //}
        //bool ctrEnviarMails_ArmarMail(Objeto item, System.Net.Mail.MailMessage mail)
        //{
        //    VTAFacturas factura = (VTAFacturas)item;
        //    factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
        //    factura = FacturasF.FacturasObtenerDatosCompletos(factura);
        //    return FacturasF.FacturaArmarMail(factura, mail);
        //}
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.CargarLista(parametros);
        }
        protected void btnValidarFacturasElectronicas_Click(object sender, EventArgs e)
        {
            Objeto resultado = new Objeto();
            resultado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            if (FacturasF.FacturaElectronicaValidarComprobanteAfip(resultado))
            {
                this.MostrarMensaje(resultado.CodigoMensaje, false, resultado.CodigoMensajeArgs);
            }
            else
            {
                this.MostrarMensaje(resultado.CodigoMensaje, true, resultado.CodigoMensajeArgs);
            }

            this.btnBuscar_Click(null, EventArgs.Empty);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAgregar.aspx"), true);
        }
        //private void PersistirDatos()
        //{
        //    foreach (GridViewRow fila in this.gvDatos.Rows)
        //    {
        //        if (fila.RowType == DataControlRowType.DataRow)
        //        {
        //            CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
        //            this.MisFacturas.Rows[fila.DataItemIndex]["Incluir"] = chkIncluir.Checked;
        //        }
        //    }
        //}
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            VTAFacturas factura = new VTAFacturas();
            factura.IdFactura = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdFactura"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdFactura", factura.IdFactura }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.EnviarMail.ToString())
            {
                MailMessage mail = new MailMessage();
                factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                factura = FacturasF.FacturasObtenerDatosCompletos(factura);
                if (FacturasF.FacturaArmarMail(factura, mail))
                {
                    this.popUpMail.IniciarControl(mail, factura);
                }
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                try
                {
                    factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    factura = FacturasF.FacturasObtenerDatosCompletos(factura);
                    factura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    VTAFacturas facturaPdf = FacturasF.FacturasObtenerArchivo(factura);
                    List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                    TGEArchivos archivo = new TGEArchivos();
                    archivo.Archivo = facturaPdf.FacturaPDF;
                    if (archivo.Archivo != null)
                        listaArchivos.Add(archivo);

                    VTARemitos remito = FacturasF.RemitosObtenerPorFactura(factura);
                    if (remito.IdRemito > 0)
                    {
                        archivo = new TGEArchivos();
                        remito.UsuarioLogueado = factura.UsuarioLogueado;
                        remito = FacturasF.RemitosObtenerArchivo(remito);
                        archivo.Archivo = remito.RemitoPDF;
                        if (archivo.Archivo != null)
                            listaArchivos.Add(archivo);
                    }
                    archivo = new TGEArchivos();
                    RepReportes reporte = new RepReportes();
                    RepParametros param = new RepParametros
                    {
                        ValorParametro = factura.IdFactura.ToString()
                    };
                    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
                    param.Parametro = "IdFactura";
                    reporte.Parametros.Add(param);
                    if (factura.FacturaContado)
                    {
                        TGEPlantillas plantilla = new TGEPlantillas();
                        plantilla.Codigo = "OrdenesCobros";
                        plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                        reporte.StoredProcedure = plantilla.NombreSP;
                        DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

                        archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdFactura", factura.UsuarioLogueado);
                        if (archivo.Archivo != null)
                            listaArchivos.Add(archivo);

                    }
                    TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                    string nombreArchivo = string.Concat(empresa.CUIT, "_", factura.TipoFactura.CodigoValor, "_", factura.PrefijoNumeroFactura, "_", factura.NumeroFactura, ".pdf");
                    ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel1, listaArchivos, nombreArchivo);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            //    TGEPlantillas plantilla = new TGEPlantillas();
            //plantilla.Codigo = "VTAComprobantes";
            //plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            //factura = FacturasF.FacturasObtenerDatosCompletos(factura);
            //List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            //TGEArchivos archivo = new TGEArchivos();
            //TGEComprobantes comprobante;
            //DataSet ds;
            //if (plantilla.HtmlPlantilla.Trim().Length > 0)
            //{
            //    comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.VTAComprobantes);
            //    ds = ExportPDF.ObtenerDatosReporteComprobante(factura, comprobante);
            //    archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(comprobante, plantilla.Codigo, ds, string.Empty, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            //}
            //else
            //{
            //    VTAFacturas facturaPdf = FacturasF.FacturasObtenerArchivo(factura);
            //    archivo.Archivo = facturaPdf.FacturaPDF;
            //}
            //if (archivo.Archivo != null)
            //    listaArchivos.Add(archivo);

            //VTARemitos remito = FacturasF.RemitosObtenerPorFactura(factura);
            //if (remito.IdRemito > 0)
            //{
            //    archivo = new TGEArchivos();
            //    plantilla = new TGEPlantillas();
            //    plantilla.Codigo = "VTARemitos";
            //    plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);
            //    if (plantilla.HtmlPlantilla.Trim().Length > 0)
            //    {
            //        comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.VTARemitos);
            //        ds = ExportPDF.ObtenerDatosReporteComprobante(remito, comprobante);
            //        archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(comprobante, plantilla.Codigo, ds, string.Empty, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            //    }
            //    else
            //    {
            //        remito = FacturasF.RemitosObtenerArchivo(remito);
            //        if (remito.RemitoPDF != null)
            //        {
            //            archivo.Archivo = remito.RemitoPDF;
            //        }
            //        if (archivo.Archivo != null)
            //            listaArchivos.Add(archivo);
            //    }
            //}
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                //ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                //ibtnConsultar.Visible = this.ValidarPermiso("FacturasConsultar.aspx");
                //ibtnConsultar.Visible = true;
                DataRowView dr = (DataRowView)e.Row.DataItem;
                //if (factura.Estado.IdEstado == (int)EstadosFacturas.FESinValidadaAfip)
                //{
                //    ibtnAutorizar.Visible = true;
                //    string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarValidarFE"));
                //    ibtnAutorizar.Attributes.Add("OnClick", funcion);
                //}
                if (Convert.ToBoolean(dr["PuedeAnular"]) &&
                            (
                                (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosFacturas.Activo
                                && Convert.ToInt32(dr["FilialPuntoVentaTipoPuntoVentaIdTipoPuntoVenta"]) == (int)EnumAFIPTiposPuntosVentas.ComprobanteManual
                                )
                            || (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosFacturas.FESinValidadaAfip
                                && Convert.ToInt32(dr["FilialPuntoVentaTipoPuntoVentaIdTipoPuntoVenta"]) == (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica
                                )
                            )
                    )
                {
                    ibtnAnular.Visible = this.ValidarPermiso("FacturasAnular.aspx");
                }
                if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosFacturas.FESinValidadaAfip)
                {
                    System.Web.UI.WebControls.Image imgAlerta = (System.Web.UI.WebControls.Image)e.Row.FindControl("imgAlerta");
                    imgAlerta.Visible = true;
                    ibtnModificar.Visible = this.ValidarPermiso("FacturasModificar.aspx");
                }
                if (Convert.ToInt32(dr["EstadoIdEstado"]) == (int)EstadosFacturas.Cobrada)
                {
                    ibtnModificar.Visible = false;
                }
                else
                {
                    ibtnModificar.Visible = true;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = Convert.ToDecimal(this.MisFacturas.Compute("Sum(ImporteTotal)", string.Empty)).ToString("C2");

                Label lblIvaTotal = (Label)e.Row.FindControl("lblIvaTotal");
                lblIvaTotal.Text = Convert.ToDecimal(this.MisFacturas.Compute("Sum(IvaTotal)", string.Empty)).ToString("C2");

                Label lblImporte = (Label)e.Row.FindControl("lblImporte");
                lblImporte.Text = Convert.ToDecimal(this.MisFacturas.Compute("Sum(ImporteSinIVA)", string.Empty)).ToString("C2");

                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisFacturas.Rows.Count);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisFacturas = this.OrdenarGrillaDatos<DataTable>(this.MisFacturas, e);
            this.gvDatos.DataSource = this.MisFacturas;
            this.gvDatos.DataBind();
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            VTAFacturas pFactura = new VTAFacturas();
            pFactura.Afiliado.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pFactura.Afiliado.IdAfiliado != 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(this.hdfRazonSocial.Value, this.hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = this.hdfIdAfiliado.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            pFactura.NumeroFactura = this.txtNumeroFactura.Text.ToString();
            pFactura.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text;
            pFactura.TipoFactura.IdTipoFactura = this.ddlTipoFactura.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFactura.SelectedValue);
            pFactura.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pFactura.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pFactura.Filial.IdFilial = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pFactura.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pFactura.IdFacturaLoteEnviado = this.ddlFacturasLotes.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFacturasLotes.SelectedValue);
            pFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pFactura.BusquedaParametros = true;
            pFactura.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            pFactura.PageSize = this.gvDatos.VirtualItemCount;
            this.MisFacturasExcel = FacturasF.FacturasObtenerGrilla(pFactura);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this, this.MisFacturasExcel);
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
        //    }
        //    else
        //    {
        //        //this.txtSocio.Text = string.Empty;
        //        parametro.CodigoMensaje = "NumeroSocioNoExiste";
        //        this.UpdatePanel1.Update();
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}
        //protected void txtNumeroSocio_TextChanged(object sender, EventArgs e)
        //{
        //    string txtNumeroSocio = ((TextBox)sender).Text;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = this.txtNumeroSocio.Text == string.Empty ?  0 : Convert.ToInt32(txtNumeroSocio);
        //    if (parametro.IdAfiliado == 0)
        //        return;

        //    AfiAfiliados Afiliado = AfiliadosF.AfiliadosObtenerDatos(parametro);
        //    if (Afiliado.IdAfiliado != 0)
        //    {
        //        this.ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(Afiliado);
        //    }
        //    else
        //    {
        //        parametro.CodigoMensaje = "NumeroClienteNoExiste";
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}
        //protected void btnBuscarCliente_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarClientePopUp.IniciarControl(true);
        //}
        //protected void btnLimpiar_Click(object sender, EventArgs e)
        //{
        //    VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
        //    parametros.Afiliado = new AfiAfiliados();
        //    //this.txtNumeroSocio.Text = string.Empty;
        //    this.txtSocio.Text = string.Empty;
        //    this.BusquedaParametrosGuardarValor<VTAFacturas>(parametros);
        //    //this.btnLimpiar.Visible = false;
        //    //this.btnBuscarSocio.Visible = true;
        //    this.UpdatePanel1.Update();
        //}

        //private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        //{
        //    this.ddlNumeroSocio.Items.Add(new ListItem(pAfiliado.DescripcionAfiliado.ToString(), pAfiliado.IdAfiliado.ToString()));
        //    this.ddlNumeroSocio.SelectedValue = pAfiliado.IdAfiliado.ToString();

        //    //if (this.MisParametrosUrl.Contains("IdAfiliado"))
        //    //    this.MisParametrosUrl["IdAfiliado"] = pAfiliado.IdAfiliado;
        //}

        //void ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(AfiAfiliados pAfiliado)
        //{
        //    this.MapearObjetoAControlesAfiliado(pAfiliado);
        //    VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
        //    parametros.Afiliado = pAfiliado;
        //    this.BusquedaParametrosGuardarValor<VTAFacturas>(parametros);
        //    this.btnLimpiar.Visible = true;
        //    this.btnBuscarSocio.Visible = false;
        //    this.UpdatePanel1.Update();
        //}

        //private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        //{
        //    this.txtNumeroSocio.Text = pAfiliado.IdAfiliado.ToString();
        //    this.txtSocio.Text = pAfiliado.Apellido;
        //}
        private void CargarCombos()
        {
            this.ddlTipoFactura.DataSource = FacturasF.TiposFacturasActivosPorIdTipoFactura(new TGETiposFacturas());
            this.ddlTipoFactura.DataValueField = "IdTipoFactura";
            this.ddlTipoFactura.DataTextField = "Descripcion";
            this.ddlTipoFactura.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoFactura, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosFacturas));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            this.ddlFacturasLotes.DataSource = FacturasF.FacturasLotesEnviadosObtenerCombo();
            this.ddlFacturasLotes.DataValueField = "IdFacturaLoteEnviado";
            this.ddlFacturasLotes.DataTextField = "Observacion";
            this.ddlFacturasLotes.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFacturasLotes, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void CargarLista(VTAFacturas pFactura)
        {
            pFactura.Afiliado.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pFactura.Afiliado.IdAfiliado != 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(this.hdfRazonSocial.Value, this.hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = this.hdfIdAfiliado.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            pFactura.NumeroFactura = this.txtNumeroFactura.Text.ToString();
            pFactura.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text;
            pFactura.TipoFactura.IdTipoFactura = this.ddlTipoFactura.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoFactura.SelectedValue);
            pFactura.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pFactura.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pFactura.Filial.IdFilial = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pFactura.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pFactura.IdFacturaLoteEnviado = this.ddlFacturasLotes.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFacturasLotes.SelectedValue);
            pFactura.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pFactura.BusquedaParametros = true;
            pFactura.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            this.gvDatos.PageSize = pFactura.PageSize;
            this.gvDatos.PageIndex = pFactura.PageIndex;

            this.BusquedaParametrosGuardarValor<VTAFacturas>(pFactura);
            this.MisFacturas = FacturasF.FacturasObtenerGrilla(pFactura);
            this.gvDatos.DataSource = this.MisFacturas;
            this.gvDatos.VirtualItemCount = this.MisFacturas.Rows.Count > 0 ? Convert.ToInt32(this.MisFacturas.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + this.gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();

            if (this.MisFacturas.Rows.Count > 0)
            {
                this.btnExportarExcel.Visible = true;
            }
            else
            {
                this.btnExportarExcel.Visible = false;
            }

            if (this.MisFacturas.Select().ToList().Exists(row => Convert.ToInt32(row["EstadoIdEstado"]) == (int)EstadosFacturas.FESinValidadaAfip))
                this.btnValidarFacturasElectronicas.Visible = true;
            else
                this.btnValidarFacturasElectronicas.Visible = false;
        }
    }
}
