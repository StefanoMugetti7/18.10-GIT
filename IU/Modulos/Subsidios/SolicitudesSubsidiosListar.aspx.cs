using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CuentasPagar.Entidades;
using System.Collections;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using CuentasPagar.FachadaNegocio;
using Reportes.Entidades;
using System.Net.Mail;
using Comunes.LogicaNegocio;
using Afiliados;
using System.IO;
using Afiliados.Entidades;
using Reportes.FachadaNegocio;
using Seguridad.FachadaNegocio;
using System.Data;
using Subsidios;
using Subsidios.Entidades;

namespace IU.Modulos.Subsidios
{
    public partial class SolicitudesSubsidiosListar : PaginaAfiliados
    {
        private List<CapSolicitudPago> MisSolicitudes
        {
            get { return (List<CapSolicitudPago>)Session[this.MiSessionPagina + "SolicitudesSubsidiosListarMisSolicitudes"]; }
            set { Session[this.MiSessionPagina + "SolicitudesSubsidiosListarMisSolicitudes"] = value; }
        }

        private DataTable MiCuentaCorriente
        {
            get { return (DataTable)Session[this.MiSessionPagina + "SubsidiosDatosMiCuentaCorriente"]; }
            set { Session[this.MiSessionPagina + "SubsidiosDatosMiCuentaCorriente"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSolicitud, this.btnBuscar);
                //this.btnAgregarComprobante.Visible = this.ValidarPermiso("SolicitudesPagosAgregar.aspx");
                this.btnAgregarOP.Visible = this.ValidarPermiso("InformesRecepcionesAbiertaAgregar.aspx");
                //this.btnAgregarRemtio.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                this.btnAgregar.Visible = this.ValidarPermiso("SolicitudesSubsidiosAgregar.aspx");
                this.CargarCombos();
                SubSubsidios pParametro = new SubSubsidios();
                pParametro.IdRefEntidad = MiAfiliado.IdAfiliado;
                this.MiCuentaCorriente = SubsidiosF.CuentasCorrientesObtenerListaFiltroFechasDT(pParametro);
                this.gvDatosCuentaCorriente.DataSource = this.MiCuentaCorriente;
                this.gvDatosCuentaCorriente.DataBind();
                this.upCtaCte.Update();
                CapSolicitudPago parametros = this.BusquedaParametrosObtenerValor<CapSolicitudPago>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSolicitud.Text = parametros.IdSolicitudPago == 0 ? String.Empty : parametros.IdSolicitudPago.ToString();
                    //this.txtPrefijoNumeroFactura.Text = parametros.PrefijoNumeroFactura;
                    //this.txtNumeroFactura.Text = parametros.NumeroFactura;
                    //this.ddlTipoSolicitud.SelectedValue = parametros.TipoSolicitudPago.IdTipoSolicitudPago.ToString();
                    // VER TODOS LOS CAMPOS 
                 
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
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SolicitudesSubsidiosAgregar.aspx"), true);
        }

        private void CargarCombos()
        {
            //this.ddlTipoSolicitud.DataSource = TGEGeneralesF.ListasValoresObtenerDatosCompletos(EnumTGEListasValores.TiposSolicitudesPago);
            //this.ddlTipoSolicitud.DataValueField = "IdListaValorDetalle";
            //this.ddlTipoSolicitud.DataTextField = "Descripcion";
            //this.ddlTipoSolicitud.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoSolicitud, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        #region "Grilla"

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            CapSolicitudPago pSolicitud = this.MisSolicitudes[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdSolicitudPago", pSolicitud.IdSolicitudPago);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SolicitudesSubsidiosAnular.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SolicitudesSubsidiosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                TGEPlantillas plantilla = new TGEPlantillas();
                plantilla.Codigo = "SolicitudSubsidios";

                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, plantilla.Codigo, pSolicitud, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("SolicitudSubsidio_", pSolicitud.IdSolicitudPago.ToString().PadLeft(10, '0')), this.UsuarioActivo);

            
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CapSolicitudPago item = (CapSolicitudPago)e.Row.DataItem;
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("SolicitudesSubsidiosConsultar.aspx");

                if ((item.Estado.IdEstado == (int)EstadosSolicitudesPagos.Activo || item.Estado.IdEstado == (int)EstadosSolicitudesPagos.Autorizado)
                    && this.ValidarPermiso("SolicitudesSubsidiosAnular.aspx"))
                    modificar.Visible = true;
                else
                    modificar.Visible = false;

                //ibtnConsultar.Visible = true;
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

        #endregion

        private void CargarLista(CapSolicitudPago pSolicitud)
        {
            //VER PARAMETROS QUE RECIBE EL STORE
            pSolicitud.Entidad.IdRefEntidad = this.MiAfiliado.IdAfiliado;
            pSolicitud.Entidad.IdEntidad = (int)EnumTGEEntidades.Afiliados;
            pSolicitud.IdSolicitudPago = this.txtNumeroSolicitud.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumeroSolicitud.Text);
            //pSolicitud.NumeroFactura = this.txtNumeroFactura.Text.ToString();
            //pSolicitud.PrefijoNumeroFactura = this.txtPrefijoNumeroFactura.Text;
            pSolicitud.TipoSolicitudPago.IdTipoSolicitudPago = (int)EnumTiposSolicitudPago.Subsidios; // Convert.ToInt32(this.ddlTipoSolicitud.SelectedValue);
                                                                                                      //VER QUE BUSQUE CON EL POP UP
                                                                                                      //pSolicitud.IdAfiliado = this.MiAfiliado.IdAfiliado;

            pSolicitud.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CapSolicitudPago>(pSolicitud);
            pSolicitud.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            this.MisSolicitudes = CuentasPagarF.SolicitudPagoObtenerListaFiltro(pSolicitud);
            this.gvDatos.DataSource = this.MisSolicitudes;
            this.gvDatos.PageIndex = pSolicitud.IndiceColeccion;
            this.gvDatos.DataBind();

        }

        private void CargarListaCuentasCorrientes(SubSubsidios pParametro)
        {
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            this.MiCuentaCorriente = SubsidiosF.CuentasCorrientesObtenerListaFiltroFechasDT(pParametro);
            this.gvDatos.DataSource = this.MiCuentaCorriente;
            this.gvDatos.DataBind();
            this.upCtaCte.Update();
        }


        protected void btnFiltrar_Click(object sender, EventArgs e)
        {

            //CargarListaCuentasCorrientes(null);
            SubSubsidios pParametro = new SubSubsidios();
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.IdRefEntidad = MiAfiliado.IdAfiliado;
            this.MiCuentaCorriente = SubsidiosF.CuentasCorrientesObtenerListaFiltroFechasDT(pParametro);
            this.gvDatosCuentaCorriente.DataSource = this.MiCuentaCorriente;
            this.gvDatosCuentaCorriente.DataBind();
            this.upCtaCte.Update();
        }


        protected void gvDatosCuentaCorriente_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            SubSubsidios parametros = this.BusquedaParametrosObtenerValor<SubSubsidios>();


            parametros.IndiceColeccion = e.NewPageIndex;
            parametros.HashTransaccion = tpCuentaCorriente.TabIndex;
            //parametros.IdProveedor = MiAfiliado.IdAfiliado.Value;
            parametros.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<SubSubsidios>(parametros);
            this.gvDatosCuentaCorriente.PageIndex = e.NewPageIndex;
            this.gvDatosCuentaCorriente.DataSource = this.MiCuentaCorriente;
            this.gvDatosCuentaCorriente.DataBind();
            //AyudaProgramacion.CargarGrillaListas(this.MiCuentaCorriente, false, this.gvDatos, true);
        }

        //protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        //{
        //    this.gvDatos.AllowPaging = false;
        //    this.gvDatos.DataSource = this.MiCuentaCorriente;
        //    this.gvDatos.DataBind();
        //    GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        //}

        //protected void btnAgregarComprobante_Click(object sender, EventArgs e)
        //{
        //    this.MisParametrosUrl = new Hashtable();
        //    this.MisParametrosUrl.Add("IdEntidad", (int)EnumTGEEntidades.Proveedores);
        //    this.MisParametrosUrl.Add("IdRefEntidad", this.MiAfiliado.IdAfiliado);
        //    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/SolicitudesPagosAgregar.aspx"), true);
        //}

        protected void btnAgregarOP_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEntidad", (int)EnumTGEEntidades.Afiliados);
            this.MisParametrosUrl.Add("IdRefEntidad", this.MiAfiliado.IdAfiliado);
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAgregar.aspx"), true);
        }

        //protected void btnAgregarRemito_Click(object sender, EventArgs e)
        //{
        //    this.MisParametrosUrl = new Hashtable();
        //    this.MisParametrosUrl.Add("IdProveedor", this.MiAfiliado.IdAfiliado);
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesAbiertaAgregar.aspx"), true);
        //}

        //protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        //{
        //    RepReportes reporte = new RepReportes();
        //    RepParametros param = new RepParametros();
        //    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
        //    param.Parametro = "IdProveedor";
        //    param.ValorParametro = this.MiAfiliado.IdAfiliado;
        //    reporte.Parametros.Add(param);
        //    param = new RepParametros();
        //    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
        //    param.Parametro = "FechaDesde";
        //    param.ValorParametro = this.txtFechaDesde.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesde.Text);
        //    reporte.Parametros.Add(param);
        //    param = new RepParametros();
        //    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
        //    param.Parametro = "FechaHasta";
        //    param.ValorParametro = this.txtFechaHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaHasta.Text);
        //    reporte.Parametros.Add(param);


        //    this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.CapResumenCuenta);
        //    this.upCtaCte.Update();
        //}

        //protected void btnEnviarMail_Click(object sender, EventArgs e)
        //{
        //    RepReportes reporte = new RepReportes();
        //    RepParametros param = new RepParametros();
        //    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
        //    param.Parametro = "IdProveedor";
        //    param.ValorParametro = this.MiAfiliado.IdAfiliado;
        //    reporte.Parametros.Add(param);
        //    param = new RepParametros();
        //    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
        //    param.Parametro = "FechaDesde";
        //    param.ValorParametro = this.txtFechaDesde.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaDesde.Text);
        //    reporte.Parametros.Add(param);
        //    param = new RepParametros();
        //    param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.DateTime;
        //    param.Parametro = "FechaHasta";
        //    param.ValorParametro = this.txtFechaHasta.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaHasta.Text);
        //    reporte.Parametros.Add(param);

        //    MailMessage mail = new MailMessage();
        //    TGEArchivos archivo = new TGEArchivos();
        //    archivo.Archivo = AyudaProgramacionLN.StreamToByteArray(this.ctrPopUpComprobantes.GenerarReporteArchivo(reporte, EnumTGEComprobantes.CapResumenCuenta));
        //    archivo.NombreArchivo = string.Concat(this.MiAfiliado.IdAfiliado, ".pdf");

        //    mail.Attachments.Add(new Attachment(new MemoryStream(archivo.Archivo), archivo.NombreArchivo));
        //    if (AfiliadosF.AfiliadosArmarMailResumenCuenta(this.MiAfiliado, mail))
        //    {
        //        this.popUpMail.IniciarControl(mail, MiAfiliado);
        //    }
        //}

        protected void gvDatosCuentaCorriente_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.AnularConfirmar.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
            || e.CommandName == "AgregarOP"
                ))
                return;
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.HashTransaccion = tpCuentaCorriente.TabIndex;
            parametros.IdAfiliado = MiAfiliado.IdAfiliado;
            parametros.BusquedaParametros = true;



            int index = Convert.ToInt32(e.CommandArgument);

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatosCuentaCorriente.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatosCuentaCorriente.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            int IdEstado = Convert.ToInt32(((HiddenField)this.gvDatosCuentaCorriente.Rows[index].FindControl("hdfEstado")).Value);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            CapSolicitudPago sp = new CapSolicitudPago();
            sp.IdSolicitudPago = IdRefTipoOperacion;
            sp.Estado.IdEstado = IdEstado;

            this.MisParametrosUrl = new Hashtable();
            //Filtro para Obtener URL y NombreParametro
            Menues filtroMenu = new Menues();
            filtroMenu.IdTipoOperacion = IdTipoOperacion;
            //Control de Tipo de Menues (SOLO CONSULTA)

            if (e.CommandName == Gestion.Impresion.ToString())
            {
                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CapSolicitudPagos, "SolicitudPago", sp, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.tpCuentaCorriente, "SolicitudPago", this.UsuarioActivo);
                this.UpdatePanel1.Update();
            }
            else
            {

                if (e.CommandName == Gestion.Consultar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Consulta;
                else if (e.CommandName == Gestion.Anular.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Anular;
                else if (e.CommandName == Gestion.AnularConfirmar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_AnularConfirmar;
                else if (e.CommandName == Gestion.Autorizar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Autorizar;
                else if (e.CommandName == Gestion.Modificar.ToString())
                    filtroMenu.IdTipoMenu = (int)EnumTGEListasValoresSistemasDetalles.TiposMenues_Modificar;
                else if (e.CommandName == "AgregarOP")
                {
                    this.MisParametrosUrl.Add("IdEntidad", 187);
                    this.MisParametrosUrl.Add("IdRefEntidad", MiAfiliado.IdAfiliado);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CuentasPagar/OrdenesPagosAgregar.aspx"), true);
                }

                //Guardo Menu devuelto de la DB
                filtroMenu = SeguridadF.MenuesObtenerPorOperacionTipoMenu(filtroMenu);
                this.MisParametrosUrl.Add(filtroMenu.NombreParametro, IdRefTipoOperacion);
                this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
                //Si devuelve una URL Redirecciona si no muestra mensaje error
                if (filtroMenu.URL.Length != 0)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(string.Concat("~/", filtroMenu.URL)), true);
                }
                else
                {
                    this.MiAfiliado.CodigoMensaje = "ErrorURLNoValida";
                    this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true);
                }
            }
        }

        protected void gvDatosCuentaCorriente_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;

                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton anular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton anularConfirmar = (ImageButton)e.Row.FindControl("btnAnularConfirmar");
                ImageButton autorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton agregarOP = (ImageButton)e.Row.FindControl("btnAgregarOP");

                ibtnConsultar.Visible = this.ValidarPermiso("SolicitudesPagosConsultar.aspx");
                modificar.Visible = this.ValidarPermiso("SolicitudesPagosModificar.aspx");
                switch (Convert.ToInt32(dr["TipoOperacionIdTipoOperacion"]))
                {
                    case (int)EnumTGETiposOperaciones.SolicitudesPagosInternos:
                    case (int)EnumTGETiposOperaciones.SolicitudesPagosCompras:
                    case (int)EnumTGETiposOperaciones.SolicitudesPagosAFIP:
                        switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                        {
                            case (int)EstadosSolicitudesPagos.Activo:
                                autorizar.Visible = this.ValidarPermiso("SolicitudesPagosAutorizar.aspx");
                                anular.Visible = this.ValidarPermiso("SolicitudesPagosAnular.aspx");

                                break;
                            case (int)EstadosSolicitudesPagos.Autorizado:
                                anular.Visible = this.ValidarPermiso("SolicitudesPagosAnular.aspx");

                                agregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                                //agregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                                break;
                            case (int)EstadosSolicitudesPagos.EnOrdenPagoParcial:
                            case (int)EstadosSolicitudesPagos.PagadoParcial:
                                agregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");

                                //agregarOP.Visible = this.ValidarPermiso("OrdenesPagosAgregar.aspx");
                                break;
                        }

                        break;

                    case (int)EnumTGETiposOperaciones.OrdenesPagos:
                    case (int)EnumTGETiposOperaciones.OrdenesPagosInterno:
                        switch (Convert.ToInt32(dr["EstadoIdEstado"]))
                        {
                            case (int)EstadosOrdenesPago.Activo:
                                autorizar.Visible = this.ValidarPermiso("OrdenesPagosAutorizar.aspx");
                                anular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                                modificar.Visible = false;
                                break;
                            case (int)EstadosOrdenesPago.Autorizado:
                                anular.Visible = this.ValidarPermiso("OrdenesPagosAnular.aspx");
                                modificar.Visible = false;

                                break;
                            case (int)EstadosOrdenesPago.Pagado:
                                anularConfirmar.Visible = this.ValidarPermiso("OrdenesPagosAnularPagada.aspx");
                                modificar.Visible = false;
                                break;
                        }
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }


    }
}