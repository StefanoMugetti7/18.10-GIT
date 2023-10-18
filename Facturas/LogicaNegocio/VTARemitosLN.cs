using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Facturas.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;
using Compras.Entidades;
using Compras;
using CrystalDecisions.Web;
using Generales.Entidades;
using System.Data;
using CrystalDecisions.Shared;
using System.IO;
using Reportes.FachadaNegocio;
using System.Xml;
using System.Net.Mail;
using Afiliados.Entidades;
using Afiliados;
using System.Web;

namespace Facturas.LogicaNegocio
{
    class VTARemitosLN : BaseLN<VTARemitos>
    {
        public override VTARemitos ObtenerDatosCompletos(VTARemitos pParametro)
        {
            VTARemitos remito = BaseDatos.ObtenerBaseDatos().Obtener<VTARemitos>("VTARemitosSeleccionar",pParametro);
            remito.RemitosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTARemitosDetalles>("VTARemitosDetallesSeleccionarPorIdRemito",pParametro);
            remito.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            remito.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            return remito;
        }

        public VTARemitos ObtenerDatosPreCargados(VTARemitos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTARemitos>("VTARemitosSeleccionarPreCargados", pParametro);
        }

        public VTARemitos ObtenerPorFactura(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTARemitos>("VTARemitosSeleccionarPorFactura", pParametro);
        }

        public VTARemitos ObtenerArchivo(VTARemitos pRemito)
        {
            this.GeneroGuardoPDF(pRemito);
            return pRemito;
            //VTARemitos remito = BaseDatos.ObtenerBaseDatos().Obtener<VTARemitos>("VTARemitosSeleccionarArchivo", pRemito);
            //if (remito.RemitoPDF == null)
            //{
            //    DbTransaction tran;
            //    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            //    using (DbConnection con = bd.CreateConnection())
            //    {
            //        con.Open();
            //        tran = con.BeginTransaction();
            //        try
            //        {
            //            bool resultado = this.GeneroGuardoPDF(pRemito, bd, tran);
            //            if (resultado)
            //            {
            //                remito.RemitoPDF = pRemito.RemitoPDF;
            //                tran.Commit();
            //            }
            //            else
            //            {
            //                tran.Rollback();
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            ExceptionHandler.HandleException(ex, "LogicaNegocio");
            //            tran.Rollback();
            //            pRemito.CodigoMensaje = "ResultadoTransaccionIncorrecto";
            //            pRemito.CodigoMensajeArgs.Add(ex.Message);
            //        }
            //    }
            //}
            //return remito;
        }

        public bool ObtenerProximoNumeroRemitoTmp(VTARemitos pParametro)
        {
            bool resultado = true;
            switch (pParametro.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta)
            {
                //case (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea:
                //    break;
                case (int)EnumAFIPTiposPuntosVentas.ComprobanteManual:
                case (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica:
                    pParametro.NumeroRemitoSuFijo = this.ObtenerProximoNumeroRemito(pParametro);
                    if (pParametro.NumeroRemitoSuFijo == string.Empty || pParametro.NumeroRemitoSuFijo == "0" || pParametro.NumeroRemitoSuFijo == "00000000")
                    {
                        pParametro.CodigoMensaje = "ErrorValidarProximoNumeroComprobante";
                        resultado = false;
                    }
                    break;
                //case (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica:
                //    FacturaElectronica.FacturaElectronica feLN = new FacturaElectronica.FacturaElectronica();
                //    //if (!feLN.ValidarProximoNumeroComprobante(pParametro))
                //    //    resultado = false;
                //    break;
                default:
                    break;
            }

            pParametro.NumeroRemitoPrefijo = pParametro.NumeroRemitoPrefijo.PadLeft(4, '0');
            pParametro.NumeroRemitoSuFijo = pParametro.NumeroRemitoSuFijo.PadLeft(8, '0');

            return resultado;
        }

        public DataTable ObtenerGrilla(VTARemitos pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTARemitosSeleccionarListaFiltro", pFactura);
        }

        public DataTable ObtenerAfiliadoGrilla(VTARemitos pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTARemitosDetallesSeleccionarPorAfiliadoGrilla", pFactura);
        }

        public override List<VTARemitos> ObtenerListaFiltro(VTARemitos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTARemitos>("VTARemitosSeleccionarListaFiltro", pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Remitos por Afiliados para asociar a una orden de cobro
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<VTARemitos> ObtenerListaOrdenesCobrosPendientes(VTARemitos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTARemitos>("VTARemitosSeleccionarOrdenesCobrosPendientes", pParametro);
        }

        /// <summary>
        /// Obtiene una lista filtrada de remitos, SIN FACTURAR.
        /// </summary>
        /// <param name="pParametro">IdRemito, FechaDesde, FechaHasta, IdAfiliado, IdFactura, NumeroRemitoPrefijo, NumeroRemitoSuFijo</param>
        /// <returns>List(Remitos)</returns>
        public List<VTARemitosDetalles> ObtenerListaFiltroPopUp(VTARemitos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTARemitosDetalles>("VTARemitosSeleccionarListaFiltroPopUp", pParametro);
        }

        public override bool Agregar(VTARemitos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            //pParametro.Estado.IdEstado = (int)EstadosRemitos.Entregado;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;


            if (!this.Validar(pParametro, new VTARemitos()))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Agregar(pParametro, bd, tran);
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public bool Agregar(VTARemitos pParametro, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            //pParametro.NumeroRemitoSuFijo = this.ObtenerProximoNumeroRemito(pParametro, bd, tran);

            //if (pParametro.NumeroRemitoSuFijo == "0")
            //{
            //    pParametro.CodigoMensaje = "ErrorValidarProximoNumeroComprobante";
            //    return false;
            //}

            pParametro.NumeroRemitoPrefijo = pParametro.NumeroRemitoPrefijo.PadLeft(4, '0');
            pParametro.NumeroRemitoSuFijo = pParametro.NumeroRemitoSuFijo.PadLeft(8, '0');

            switch (pParametro.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta)
            {
                case (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea:
                    pParametro.CodigoMensaje = "ErrorValidarComprobanteEnLinea";
                    pParametro.CodigoMensajeArgs.Add(EnumAFIPTiposPuntosVentas.ComprobanteEnLinea.ToString());
                    resultado = false;
                    break;
                case (int)EnumAFIPTiposPuntosVentas.ComprobanteManual:
                case (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica:
                    //pParametro.NumeroFactura = this.ObtenerProximoNumeroFactura(pParametro, bd, tran);
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "[VTARemitosValidarNumeroComprobante]"))
                    {
                        pParametro.CodigoMensaje = "ErrorValidarNumeroComprobanteManualRemito";
                        resultado = false;
                    }

                    if (resultado && (pParametro.NumeroRemitoSuFijo == string.Empty || pParametro.NumeroRemitoSuFijo == "0" || pParametro.NumeroRemitoSuFijo == "00000000"))
                    {
                        pParametro.CodigoMensaje = "ErrorValidarNumeroComprobanteRemito";
                        resultado = false;
                    }
                    break;
                //case (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica:
                //    pParametro.Estado.IdEstado = (int)EstadosFacturas.FESinValidadaAfip;
                //    //if (resultado && !feLN.ValidarProximoNumeroComprobante(pParametro))
                //    //    resultado = false;
                //    break;
                default:
                    break;
            }

            if (resultado)
            {
                pParametro.IdRemito = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "VTARemitosInsertar");
                if (pParametro.IdRemito == 0)
                    resultado = false;
            }
            if (resultado && !this.ItemsActualizar(pParametro, new VTARemitos(), bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                resultado = false;

            
            return resultado;
        }


        /// <summary>
        /// Metodo que Genera y Guarda el PDF del Remito
        /// </summary>
        /// <param name="pParametro">IdRemito</param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public bool GeneroGuardoPDF(VTARemitos pParametro)//, Database bd, DbTransaction tran)
        {
            bool resultadoPDF = true;
            pParametro.RemitoPDF = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.VTARemitos, "VTARemitos", pParametro, pParametro.UsuarioLogueado);
            return resultadoPDF;
        }
        public bool ArmarMailRemito(VTARemitos pParametro, MailMessage mail)
        {
            bool resultado = true;

            List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            TGEArchivos archivo = new TGEArchivos();
            VTARemitos facturaPdf = this.ObtenerArchivo(pParametro);

            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            string nombreArchivo = string.Concat(empresa.CUIT, "_", pParametro.TipoFactura.CodigoValor, "_", pParametro.NumeroRemitoPrefijo, "_", pParametro.NumeroRemitoSuFijo, ".pdf");

            archivo.Archivo = facturaPdf.RemitoPDF;
            archivo.NombreArchivo = nombreArchivo;
            listaArchivos.Add(archivo);

            VTARemitosLN remitosLN = new VTARemitosLN();
            if (pParametro.IdRemito > 0)
            {
                pParametro = remitosLN.ObtenerArchivo(pParametro);
                if (pParametro.RemitoPDF != null)
                {
                    archivo = new TGEArchivos();
                    archivo.Archivo = pParametro.RemitoPDF;
                    nombreArchivo = string.Concat(empresa.CUIT, "_", pParametro.NumeroRemitoPrefijo, "_", pParametro.NumeroRemitoSuFijo, ".pdf");
                    archivo.NombreArchivo = nombreArchivo;
                    listaArchivos.Add(archivo);
                }
            }
            AfiAfiliados cliente = new AfiAfiliados();
            cliente.IdAfiliado = pParametro.Afiliado.IdAfiliado;
            cliente = AfiliadosF.AfiliadosObtenerDatos(cliente);

            string template = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Templates\\MailEnviarRemito.htm");
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "RemitosMail";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            if (cliente.CorreoElectronico.Trim() != string.Empty)
            {
                if (cliente.CorreoElectronico.Trim().Contains(";"))
                {
                    List<string> lista = cliente.CorreoElectronico.Trim().Split(';').ToList();
                    foreach (string item in lista)
                        mail.To.Add(new MailAddress(item.Trim(), cliente.ApellidoNombre.Trim()));
                }
                else
                    mail.To.Add(new MailAddress(cliente.CorreoElectronico.Trim(), cliente.ApellidoNombre.Trim()));
            }

            mail.IsBodyHtml = true;
            if (plantilla.HtmlPlantilla.Trim().Length > 0)
            {
                string htmlPlantilla = plantilla.HtmlPlantilla;
                List<StringStartEnd> posiciones = AyudaProgramacionLN.FindAllString(htmlPlantilla, "{", "}");
                List<string> campos = new List<string>();
                string campo;
                foreach (StringStartEnd pos in posiciones)
                {
                    campo = htmlPlantilla.Substring(pos.start, pos.end - pos.start).Replace("{", "").Replace("}", "");
                    if (campo.Length > 0)
                        campos.Add(campo);
                }
                AyudaProgramacionLN.MapearEntidad(ref htmlPlantilla, campos, pParametro);
                mail.Subject = HttpUtility.HtmlDecode(AyudaProgramacionLN.StripHtml(AyudaProgramacionLN.getBetween(htmlPlantilla, "[Evol:Asunto]", "[/Evol:Asunto]")
                    .Replace("[Evol:Asunto]", "").Replace("[/Evol:Asunto]", "")).Trim());
                mail.Body = AyudaProgramacionLN.getBetween(htmlPlantilla, "[Evol:Cuerpo]", "[/Evol:Cuerpo]")
                    .Replace("[Evol:Cuerpo]", "").Replace("[/Evol:Cuerpo]", "").Trim();
            }
            else
            {
                mail.Body = new StreamReader(template).ReadToEnd();
                mail.Subject = "Remitos de Venta";
                mail.Body = mail.Body.Replace("%ApellidoNombre%", cliente.ApellidoNombre);
                mail.Body = mail.Body.Replace("%TipoFacturaNumeroCompleto%", string.Concat(pParametro.TipoFactura.Descripcion, " ", pParametro.NumeroRemitoCompleto));
                mail.Body = mail.Body.Replace("%Empresa%", empresa.Empresa);
            }

            foreach (TGEArchivos attach in listaArchivos)
                mail.Attachments.Add(new Attachment(new MemoryStream(attach.Archivo), attach.NombreArchivo));

            return resultado;
        }
        public bool ModificarPDF(VTARemitos pParametro)
        {
            bool resultado = true;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTARemitosInsertarPDF"))
                        resultado = false;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        private string ObtenerProximoNumeroRemito(VTARemitos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTARemitos>("VTARemitosSeleccionarProximoNumero", pParametro).NumeroRemitoSuFijo;
        }

        private string ObtenerProximoNumeroRemito(VTARemitos pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTARemitos>("VTARemitosSeleccionarProximoNumero", pParametro, bd, tran).NumeroRemitoSuFijo;
        }
        
        public override bool Modificar(VTARemitos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            VTARemitos valorViejo = new VTARemitos();
            valorViejo.IdRemito = pParametro.IdRemito;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaEvento = DateTime.Now;

            if (!this.Validar(pParametro, valorViejo))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTARemitosActualizar");

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;

        }

        public bool Anular(VTARemitos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            VTARemitos valorViejo = new VTARemitos();
            valorViejo.IdRemito = pParametro.IdRemito;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            pParametro.EstadoColeccion = EstadoColecciones.Borrado;
            pParametro.Estado.IdEstado = (int)EstadosRemitos.Baja;
            pParametro.Estado = TGEGeneralesF.TGEEstadosObtener(pParametro.Estado);
            pParametro.FechaEvento = DateTime.Now;


            //if (!this.Validar(pParametro, new VTARemitos()))
            //    return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTARemitosActualizar");
                        

                    if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;

        }

        #region "Items Remitos"

        private bool ItemsActualizar(VTARemitos pParametro, VTARemitos pValorViejo, Database bd, DbTransaction tran)
        {
            TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ValidarStockNegativo, bd, tran);
            bool validarStock = paramValor.ParametroValor.Trim() == string.Empty ? false : Convert.ToBoolean(paramValor.ParametroValor);
            foreach (VTARemitosDetalles item in pParametro.RemitosDetalles)
            {
                CMPStock stock = new CMPStock();
                //int stockViejo = 0;
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.IdRemito = pParametro.IdRemito;
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;

                        if (item.NotasPedidosDetalles.Count > 0)
                        {
                            item.LoteNotasPedidos = new XmlDocument();

                            XmlNode notas = item.LoteNotasPedidos.CreateElement("Notas");
                            item.LoteNotasPedidos.AppendChild(notas);

                            XmlNode notaDetalle;
                            XmlNode atributoNotaDetalle;

                            foreach (VTANotasPedidosDetalles x in item.NotasPedidosDetalles)
                            {
                                notaDetalle = item.LoteNotasPedidos.CreateElement("Nota");

                                atributoNotaDetalle = item.LoteNotasPedidos.CreateElement("IdNotaPedidoDetalle");
                                atributoNotaDetalle.InnerText = x.IdNotaPedidoDetalle.ToString();
                                notaDetalle.AppendChild(atributoNotaDetalle);

                                notas.AppendChild(notaDetalle);
                            }
                        }

                        item.IdRemitoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "VTARemitosDetallesInsertar");
                        if (item.IdRemitoDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        #region stock
                        stock.ValidarStock = validarStock;
                        stock.IdFilial = pParametro.FilialEntrega.IdFilialEntrega;
                        stock.Producto = item.Producto;
                        if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.DevolucionRemitosVentas)
                            stock.StockActual = item.Cantidad;
                        else
                            stock.StockActual = - item.Cantidad;
                        if (!ComprasF.StockAgregarModificar(stock, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }
                        #endregion
                        break;
                    #endregion

                    #region Borrado
                    case EstadoColecciones.Borrado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "VTARemitosDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.RemitosDetalles.Find(x => x.IdRemitoDetalle == item.IdRemitoDetalle), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        #region stock
                        stock.IdFilial = pParametro.FilialEntrega.IdFilialEntrega;
                        stock.Producto = item.Producto;
                        if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.DevolucionRemitosVentas)
                            stock.StockActual = - item.Cantidad;
                        else
                            stock.StockActual = item.Cantidad;
                        if (!ComprasF.StockAgregarModificar(stock, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(stock, pParametro);
                            return false;
                        }
                        #endregion                        
                        break;
                    #endregion

                    #region Modificado
                    case EstadoColecciones.Modificado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "VTARemitosDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.RemitosDetalles.Find(x => x.IdRemitoDetalle == item.IdRemitoDetalle), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }


            }
            

            return true;
        }
        #endregion

        #region Validaciones

        private bool Validar(VTARemitos pRemito, VTARemitos valorViejo)
        {
            bool result = true;

            switch (pRemito.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    if (pRemito.NumeroRemitoSuFijo == "0" || pRemito.NumeroRemitoSuFijo == "00000000" || pRemito.NumeroRemitoSuFijo == string.Empty
                        || pRemito.NumeroRemitoPrefijo == "0" || pRemito.NumeroRemitoPrefijo == "0000" || pRemito.NumeroRemitoPrefijo == string.Empty)
                    {
                        pRemito.CodigoMensaje = "ErrorValidarNumeroComprobante";
                        return false;
                    }

                    List<VTARemitosDetalles> lista = pRemito.RemitosDetalles.Where(x => x.EstadoColeccion == EstadoColecciones.Agregado).ToList();
                    if (lista.Count == 0)
                    {
                        pRemito.CodigoMensaje = "RemitosValidarItems";
                        return false;
                    }

                    if (lista.Exists(x => x.Cantidad == 0))
                    {
                        pRemito.CodigoMensaje = "RemitosValidarItemsCantidad";
                        return false;
                    }
                    //Remitos con Items Importados de Facturas
                    /* Modificacion: Punto Obra --> no traía la cantidad restante correctamente y cuando se modifico la validacion no tenia sentido. 
                    * Fecha: 02/06/2021 | Programador: Ornella Leiva  | Se cambio: x.Cantidad + x.CantidadEntregada > x.CantidadRestante); */
                    if (lista.Exists(x => x.IdFacturaDetalle.HasValue
                                 && x.Cantidad > x.CantidadRestante))
                    {
                        VTARemitosDetalles det = lista.FirstOrDefault(x => x.IdFacturaDetalle.HasValue
                                 && x.Cantidad > x.CantidadRestante); 
                        pRemito.CodigoMensaje = "RemitosValidarCantidad";
                        pRemito.CodigoMensajeArgs.Add(det.Producto.Descripcion);
                        pRemito.CodigoMensajeArgs.Add(det.CantidadRestante.HasValue ? det.CantidadRestante.Value.ToString() : "0");
                        return false;

                    }
                    //Remitos con Items asociados a Acopio
                    if (pRemito.Factura.IdFactura > 0)
                    {
                        if (lista.Exists(x => !x.PrecioUnitario.HasValue || x.PrecioUnitario.Value == 0))
                        {
                            pRemito.CodigoMensaje = "RemitosDetallesValidarPrecioUnitario";
                            return false;
                        }
                        decimal importeEntregar = lista.Sum(x => x.Cantidad * x.PrecioUnitario.Value);
                        if (pRemito.ImportePrevioEntregado.Value + importeEntregar > pRemito.Factura.ImporteSinIVA)
                        {
                            pRemito.CodigoMensaje = "RemitosValidarAcopioImporteEntregar";
                            pRemito.CodigoMensajeArgs.Add(pRemito.Factura.ImporteSinIVA.ToString("C2"));
                            pRemito.CodigoMensajeArgs.Add(pRemito.ImportePrevioEntregado.Value.ToString("C2"));
                            pRemito.CodigoMensajeArgs.Add(importeEntregar.ToString("C2"));
                            return false;
                        }
                    }
                        //VTAFacturasDetalles facturaDetalle;
                        //if (!(pRemito.Factura.IdFactura == 0))
                        //{
                        //    foreach (VTARemitosDetalles rDetalle in pRemito.RemitosDetalles)
                        //    {
                        //        facturaDetalle = pRemito.Factura.FacturasDetalles.Find(x => x.IdFacturaDetalle == rDetalle.IdFacturaDetalle);
                        //        decimal fCantRestante = facturaDetalle.CantidadRestante.HasValue ? facturaDetalle.CantidadRestante.Value : 0;

                        //        if (rDetalle.Cantidad > fCantRestante || rDetalle.Cantidad == 0)
                        //        {
                        //            pRemito.CodigoMensaje = "RemitosValidarCantidad";
                        //            pRemito.CodigoMensajeArgs.Add(rDetalle.Producto.Descripcion);
                        //            pRemito.CodigoMensajeArgs.Add(fCantRestante.ToString());//IdProducto {0}
                        //            result = false;
                        //        }
                        //    }
                        //}

                        if (pRemito.Campos.Exists(x => x.Nombre == "RemitosBloqueoPrestamosCuotas" && x.CampoTipo.IdCampoTipo==(int)EnumCamposTipos.CheckBox))
                    {
                        TGECampos campo = pRemito.Campos.Find(x => x.Nombre == "RemitosBloqueoPrestamosCuotas" && x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.CheckBox);
                        if (campo.CampoValor.Valor != string.Empty && Convert.ToBoolean(campo.CampoValor.Valor) &&
                            (pRemito.Estado.IdEstado != (int)EstadosRemitos.PendienteEntrega && pRemito.Estado.IdEstado != (int)EstadosRemitos.Baja)
                            )
                        {
                            pRemito.CodigoMensaje = "RemitosValidarBloqueoPrestamosCuotas";
                            return false;
                        }
                    }

                    break;
                case EstadoColecciones.Modificado:
                    if (pRemito.Campos.Exists(x => x.Nombre == "RemitosBloqueoPrestamosCuotas" && x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.CheckBox))
                    {
                        TGECampos campo = pRemito.Campos.Find(x => x.Nombre == "RemitosBloqueoPrestamosCuotas" && x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.CheckBox);
                        if (campo.CampoValor.Valor != string.Empty && Convert.ToBoolean(campo.CampoValor.Valor) &&
                            (pRemito.Estado.IdEstado != (int)EstadosRemitos.PendienteEntrega && pRemito.Estado.IdEstado != (int)EstadosRemitos.Baja)
                            )
                        {
                            pRemito.CodigoMensaje = "RemitosValidarBloqueoPrestamosCuotas";
                            return false;
                        }
                    }
                    break;
                case EstadoColecciones.SinCambio:
                    break;
                default:
                    break;
            }

            //if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pRemito, "VTARemitosValidaciones"))
            //    return false;

            return result;
        }

        #endregion

        public List<VTAFacturas> ObtenerFacturasPendientesARemitarPorIdAfiliado(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturas>("VTAFacturasObtenerPendientesARemitarPorIdAfiliado", pParametro);
        }

        public List<TGETiposFacturas> ObtenerPorRemitos(VTARemitos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGETiposFacturas>("[VTATiposFacturasSeleccionarRemitos]", pParametro);
        }
    }
}
