using Afiliados;
using Afiliados.Entidades;
using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Contabilidad;
using Contabilidad.Entidades;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Reportes.FachadaNegocio;
using Servicio.AccesoDatos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Xml;

namespace Facturas.LogicaNegocio
{
    class VTAFacturasLN : BaseLN<VTAFacturas>
    {
        public DataTable ObtenerGrilla(VTAFacturas pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTAFacturasSeleccionarDescripcionPorFiltro", pFactura);
        }

        public override List<VTAFacturas> ObtenerListaFiltro(VTAFacturas pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturas>("VTAFacturasSeleccionarDescripcionPorFiltro", pFactura);
        }

        public List<VTAFacturas> ObtenerListaFiltroComboAsociados(VTAFacturas pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturas>("VTAFacturasSeleccionarComboAsociadosPorFiltro", pFactura);
        }

        public VTAFacturas ValidacionesCuentasCorrientesCargosFacturados(VTAFacturas pFactura)
        {
            pFactura.EsFacturaCargos = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pFactura, "VTAFacturasValidacionesCuentasCorrientesCargosFacturados");
            pFactura.FacturasDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasDetalles>("VTAFacturasDetallesSeleccionarPorIdFacturaCargos", pFactura);
            if (pFactura.EsFacturaCargos)
            {
                pFactura.FacturasDetalles.ForEach(x => x.EsFacturaCargos = true);
            }
            else
            {
                pFactura.FacturasDetalles.ForEach(x => x.EsFacturaCargos = false);
            }
            return pFactura;
        }

        public override VTAFacturas ObtenerDatosCompletos(VTAFacturas pFactura)
        {
            VTAFacturas factura = BaseDatos.ObtenerBaseDatos().Obtener<VTAFacturas>("VTAFacturasSeleccionar", pFactura);
            factura.FacturasDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasDetalles>("VTAFacturasDetallesSeleccionarPorIdFactura", pFactura);
            factura.FacturasAsociadas = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturas>("VTAFacturasNotasCreditosSeleccionarPorIdFactura", pFactura);
            if (factura.ImportePercepciones > 0)
                factura.FacturasTiposPercepciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasTiposPercepciones>("VTAFacturasTiposPercepcionesSeleccionarPorIdFactura", pFactura);
            return factura;
        }

        public VTAFacturas ObtenerDatosPreCargados(VTAFacturas pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTAFacturas>("VTAFacturasSeleccionarPreCargados", pFactura);
        }
        public VTAFacturas ObtenerDatosPreCargadosSimple(VTAFacturas pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTAFacturas>("VTAFacturasSimpleSeleccionarPreCargados", pFactura);
        }
        public VTAFacturas ObtenerArchivo(VTAFacturas pFactura)
        {
            this.GenerarActualizarPDF(pFactura);
            return pFactura;
        }


        public bool ObtenerProximoNumeroFacturaTmp(VTAFacturas pParametro)
        {
            bool resultado = true;
            switch (pParametro.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta)
            {
                //case (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea:
                //    break;
                case (int)EnumAFIPTiposPuntosVentas.ComprobanteManual:
                    //pParametro.NumeroFactura = this.ObtenerProximoNumeroFactura(pParametro);
                    //if (pParametro.NumeroFactura == string.Empty || pParametro.NumeroFactura == "0" || pParametro.NumeroFactura == "00000000")
                    //{
                    //    pParametro.CodigoMensaje = "ErrorValidarProximoNumeroComprobante";
                    //    resultado = false;
                    //}
                    break;
                //case (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica:
                //    FacturaElectronica.FacturaElectronica feLN = new FacturaElectronica.FacturaElectronica();
                //    if (!feLN.ValidarProximoNumeroComprobante(pParametro))
                //        resultado = false;
                //    break;
                default:
                    break;
            }

            pParametro.NumeroFactura = this.ObtenerProximoNumeroFactura(pParametro);
            if (pParametro.NumeroFactura == string.Empty || pParametro.NumeroFactura == "0" || pParametro.NumeroFactura == "00000000")
            {
                pParametro.CodigoMensaje = "ErrorValidarProximoNumeroComprobante";
                resultado = false;
            }
            pParametro.PrefijoNumeroFactura = pParametro.PrefijoNumeroFactura.PadLeft(4, '0');
            pParametro.NumeroFactura = pParametro.NumeroFactura.PadLeft(8, '0');

            return resultado;
        }

        private string ObtenerProximoNumeroFactura(VTAFacturas pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTAFacturas>("VTAFacturasSeleccionarProximoNumero", pFactura).NumeroFactura;
        }

        private string ObtenerProximoNumeroFactura(VTAFacturas pFactura, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<VTAFacturas>("VTAFacturasSeleccionarProximoNumero", pFactura, bd, tran).NumeroFactura;
        }

        public override bool Agregar(VTAFacturas pParametro)
        {
            throw new NotImplementedException();
        }

        public bool Agregar(VTAFacturas pParametro, VTARemitos pRemito)
        {
            //Control por Duplicacion. Si tengo el Id es porque ya se grabo
            //Si hay error en el Finally del try/catch bloq lo pongo en 0
            if (pParametro.IdFactura > 0)
                return true;

            FacturaElectronica.FacturaElectronica feLN = new FacturaElectronica.FacturaElectronica();
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)EstadosFacturas.Activo;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.FechaContable = pParametro.FechaFactura;
            pParametro.ObtenerListaFacturasAsociadas();

            //pParametro.DataTableFacturas = pParametro.FacturasDetalles.ToDataTable();//.ToXmlDocument();
            //pParametro.DataTableFacturas.TableName = "ListasPreciosDetalles";
            //pParametro.LoteFacturas = pParametro.DataTableFacturas.ToXmlDocument();

            if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasA
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasB
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasC
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesA
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesB
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesC)
            {
                pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.FacturaVenta;
            }
            else if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesA
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesB
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesC)
            {
                pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.NotaCreditoVenta;
            }
            else if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoA
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoB
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoC
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesA
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesB
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesC
                )
            {
                pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.NotaDebitoVenta;
            }
            else if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.CbteInterno
                || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.cbteInternoC)
                pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.CbteInterno;
            else if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.CbteInternoCredito)
                pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.CbteInternoCredito;

            if (!this.Validar(pParametro, new VTAFacturas()))
                return false;

            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.FacturaVenta
                        || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaCreditoVenta)
            {
                CtbPeriodosIvas periodoFiltro = new CtbPeriodosIvas();
                periodoFiltro.Periodo = AyudaProgramacionLN.ObtenerPeriodo(pParametro.FechaFactura);
                if (ContabilidadF.PeriodosIvasValidarCierre(periodoFiltro))
                {
                    pParametro.CodigoMensaje = "ValidarFechaContablePeriodoIvaCerrado";
                    pParametro.CodigoMensajeArgs.Add(periodoFiltro.Periodo.ToString());
                    return false;
                }
            }

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.PrefijoNumeroFactura = pParametro.PrefijoNumeroFactura.PadLeft(4, '0');
                    pParametro.NumeroFactura = pParametro.NumeroFactura.PadLeft(8, '0');

                    switch (pParametro.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta)
                    {
                        case (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea:
                            pParametro.CodigoMensaje = "ErrorValidarComprobanteEnLinea";
                            pParametro.CodigoMensajeArgs.Add(EnumAFIPTiposPuntosVentas.ComprobanteEnLinea.ToString());
                            resultado = false;
                            break;
                        case (int)EnumAFIPTiposPuntosVentas.ComprobanteManual:
                            if(pParametro.NumeroFactura.PadLeft(8, '0') == "00000000")
                            {
                                pParametro.NumeroFactura = this.ObtenerProximoNumeroFactura(pParametro, bd, tran);
                                pParametro.NumeroFactura = pParametro.NumeroFactura.PadLeft(8, '0');
                            }
                            break;
                        case (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica:
                            pParametro.Estado.IdEstado = (int)EstadosFacturas.FESinValidadaAfip;
                            pParametro.NumeroFactura = this.ObtenerProximoNumeroFactura(pParametro, bd, tran);
                            pParametro.NumeroFactura = pParametro.NumeroFactura.PadLeft(8, '0');
                            break;
                        default:
                            break;
                    }
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "VTAFacturasValidaciones"))
                    {
                        resultado = false;
                    }

                    if (resultado && !this.Agregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ItemsActualizar(pParametro, new VTAFacturas(), bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarTiposPercepciones(pParametro, new VTAFacturas(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.FacturaVenta
                        || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaCreditoVenta
                        || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaDebitoVenta)
                    {
                        if (resultado && !new InterfazContableLN().AgregarComprobante(pParametro, bd, tran))
                            resultado = false;
                    }

                    if (resultado && !this.ActualizarNotaCredito(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && pParametro.RemitoVentaAutomatico)
                    {
                        bool generaRemito = false;
                        VTARemitosLN remitoLN = new VTARemitosLN();
                        pRemito.NumeroRemitoPrefijo = pRemito.NumeroRemitoPrefijo.PadLeft(4, '0');
                        pRemito.NumeroRemitoSuFijo = pRemito.NumeroRemitoSuFijo.PadLeft(8, '0');
                        pRemito.UsuarioLogueado = pParametro.UsuarioLogueado;
                        pRemito.Afiliado = pParametro.Afiliado;
                        pRemito.Factura.IdFactura = pParametro.IdFactura;
                        pRemito.FechaRemito = pParametro.FechaFactura;
                        pRemito.Filial.IdFilial = pParametro.Filial.IdFilial;
                        pRemito.DirPath = pParametro.DirPath;
                        pRemito.AppPath = pParametro.AppPath;
                        pRemito.RemitosDetalles = new List<VTARemitosDetalles>();
                        VTARemitosDetalles remDet;
                        foreach (VTAFacturasDetalles factDet in pParametro.FacturasDetalles)
                        {
                            if (factDet.ListaPrecioDetalle.Producto.Familia.Stockeable)
                            {
                                generaRemito = true;
                                remDet = new VTARemitosDetalles();
                                remDet.EstadoColeccion = EstadoColecciones.Agregado;
                                remDet.IdFacturaDetalle = factDet.IdFacturaDetalle;
                                remDet.Producto.IdProducto = factDet.ListaPrecioDetalle.Producto.IdProducto;
                                remDet.Producto.Descripcion = factDet.DescripcionProducto;
                                remDet.Cantidad = factDet.Cantidad.Value;
                                remDet.Estado.IdEstado = (int)Estados.Activo;
                                remDet.Descripcion = factDet.Descripcion;
                                pRemito.RemitosDetalles.Add(remDet);
                            }
                        }
                        if (resultado && generaRemito && !remitoLN.Agregar(pRemito, bd, tran))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(pRemito, pParametro);
                        }
                    }

                    if (resultado && pParametro.EsFacturaCargos)
                    {
                        if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "VTAFacturasNotasCreditoCargoInsertarProceso"))
                        {
                            resultado = false;
                        }

                    }

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
                    resultado = false;
                }
                finally
                {
                    if (!resultado)
                        pParametro.IdFactura = 0;
                }

                try
                {
                    bool resultadoFE = true;
                    if (resultado &&
                        pParametro.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta == (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica)
                    {
                        resultadoFE = this.ValidarFE(pParametro);
                        if (!resultadoFE)
                            pParametro = this.ObtenerDatosCompletos(pParametro);
                        else
                            pParametro.Estado.IdEstado = (int)EstadosFacturas.Activo;
                    }

                    bool resultadoOC = true;
                    if (resultado && resultadoFE && pParametro.FacturaContado)
                    {
                        tran = con.BeginTransaction();
                        pParametro.PrefijoNumeroRecibo = pParametro.PrefijoNumeroRecibo.PadLeft(4, '0');
                        pParametro.NumeroRecibo = pParametro.NumeroRecibo.PadLeft(8, '0');

                        if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "VTAFacturasCobroContadoInsertarProceso"))
                        {
                            resultadoOC = false;
                            pParametro.FacturaContado = false;
                        }
                        else
                        {
                            pParametro.Estado.IdEstado = (int)EstadosFacturas.Cobrada;
                        }

                        if (resultadoOC)
                        {
                            tran.Commit();
                            pParametro.CodigoMensaje = "ResultadoTransaccion";
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }

                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                }
                finally
                {

                }

                if (resultado)
                {
                    try
                    {
                        BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, "VTAFacturasAgregarActualizarFinalizado");
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    }
                    finally
                    {
                    }
                }

            }
            return resultado;
        }

        public bool ArmarMailFactura(VTAFacturas pParametro, MailMessage mail)
        {
            bool resultado = true;

            List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            TGEArchivos archivo = new TGEArchivos();
            VTAFacturas facturaPdf = this.ObtenerArchivo(pParametro);

            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            string nombreArchivo = string.Concat(empresa.CUIT, "_", pParametro.TipoFactura.CodigoValor, "_", pParametro.PrefijoNumeroFactura, "_", pParametro.NumeroFactura, ".pdf");

            archivo.Archivo = facturaPdf.FacturaPDF;
            archivo.NombreArchivo = nombreArchivo;
            listaArchivos.Add(archivo);

            VTARemitosLN remitosLN = new VTARemitosLN();
            VTARemitos remito = remitosLN.ObtenerPorFactura(pParametro);
            if (remito.IdRemito > 0)
            {
                remito = remitosLN.ObtenerArchivo(remito);
                if (remito.RemitoPDF != null)
                {
                    archivo = new TGEArchivos();
                    archivo.Archivo = remito.RemitoPDF;
                    nombreArchivo = string.Concat(empresa.CUIT, "_", remito.NumeroRemitoPrefijo, "_", remito.NumeroRemitoSuFijo, ".pdf");
                    archivo.NombreArchivo = nombreArchivo;
                    listaArchivos.Add(archivo);
                }
            }
            AfiAfiliados cliente = new AfiAfiliados();
            cliente.IdAfiliado = pParametro.Afiliado.IdAfiliado;
            cliente = AfiliadosF.AfiliadosObtenerDatos(cliente);

            string template = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Templates\\MailEnviarFactura.htm");
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "VentasMail";
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
                mail.Subject = "Comprobante de Venta";
                mail.Body = mail.Body.Replace("%ApellidoNombre%", cliente.ApellidoNombre);
                mail.Body = mail.Body.Replace("%TipoFacturaNumeroCompleto%", string.Concat(pParametro.TipoFactura.Descripcion, " ", pParametro.NumeroFacturaCompleto));
                mail.Body = mail.Body.Replace("%Empresa%", empresa.Empresa);
            }

            foreach (TGEArchivos attach in listaArchivos)
                mail.Attachments.Add(new Attachment(new MemoryStream(attach.Archivo), attach.NombreArchivo));

            return resultado;
        }

        public bool ValidarFE(Objeto objResultado)
        {
            VTAFacturas filtro = new VTAFacturas();
            filtro.Estado.IdEstado = (int)EstadosFacturas.FESinValidadaAfip;
            filtro.UsuarioLogueado = objResultado.UsuarioLogueado;
            List<VTAFacturas> listaFacturas = BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturas>("VTAFacturasSeleccionarValidarAfip", filtro);
            //listaFacturas = listaFacturas.OrderBy(x => x.IdFactura).ToList();
            List<VTAFacturas> listaValidar = new List<VTAFacturas>();
            VTAFacturas factu;

            foreach (VTAFacturas item in listaFacturas)
            {
                item.UsuarioLogueado = objResultado.UsuarioLogueado;
                factu = FacturasF.FacturasObtenerDatosCompletos(item);
                factu.Campos = TGEGeneralesF.CamposObtenerListaFiltro(factu, factu.TipoFactura);
                factu.DirPath = System.AppDomain.CurrentDomain.BaseDirectory;
                if (factu.Estado.IdEstado == (int)EstadosFacturas.FESinValidadaAfip)
                    listaValidar.Add(factu);
            }

            bool resultado = true;
            FacturaElectronica.FacturaElectronica feLN = new FacturaElectronica.FacturaElectronica();
            bool resuladoFE = feLN.ValidarFacturaAFIP(objResultado, listaValidar);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    foreach (VTAFacturas factura in listaValidar.Where(x => x.EstadoColeccion == EstadoColecciones.Modificado).ToList())
                    {
                        if (resultado && !this.ActualizarCAE(factura, bd, tran))
                            resultado = false;

                        //if (resultado 
                        //    && factura.Estado.IdEstado==(int)EstadosFacturas.Activo
                        //    && !this.GenerarActualizarPDF(factura, bd, tran))
                        //    resultado = false;

                        if (!resultado)
                            break;
                    }
                    if (resultado)
                    {
                        tran.Commit();
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
                    objResultado.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    objResultado.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resuladoFE;
        }

        /// <summary>
        /// Genera el PDF y lo Guarda en la BD
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private bool GenerarActualizarPDF(VTAFacturas pParametro)//, Database bd, DbTransaction tran)
        {
            #region Generao y Guardo el Comprobante
            bool resultadoPDF = true;
            pParametro.FacturaPDF = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.VTAComprobantes, "VTAComprobantes", pParametro, pParametro.UsuarioLogueado);
            return resultadoPDF;
            #endregion
        }
        private bool GenerarActualizarPDF(VTARemitos pParametro)//, Database bd, DbTransaction tran)
        {
            #region Generao y Guardo el Comprobante
            bool resultadoPDF = true;
            pParametro.RemitoPDF = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.VTARemitos, "VTARemitos", pParametro, pParametro.UsuarioLogueado);
            return resultadoPDF;
            #endregion
        }
        /// <summary>
        /// Actualiza el CAE, FechaVtoCAE, Estado Comprobante, ObservacionesErrores(Afip)
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private bool ActualizarCAE(VTAFacturas pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAFacturasActualizarCAE");
        }

        internal bool Agregar(VTAFacturas pParametro, Database bd, DbTransaction tran)
        {
            pParametro.IdFactura = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "VTAFacturasInsertar");
            if (pParametro.IdFactura == 0)
                return false;

            return true;
        }

        private bool Validar(VTAFacturas pParametro, VTAFacturas pValorViejo)
        {
            VTAFacturas factura = new VTAFacturas();

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.SinCambio:
                    break;
                case EstadoColecciones.Agregado:
                    if (pParametro.Afiliado.IdAfiliado == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarNumeroCliente";
                        return false;
                    }

                    if (pParametro.FacturasDetalles.Count == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarItemsFactura";
                        return false;
                    }
                    if (pParametro.FacturasDetalles.Exists(x => x.Cantidad <= 0 || x.Cantidad == null))
                    {
                        pParametro.CodigoMensaje = "ValidarItemsFacturaCantidad";
                        return false;
                    }
                    if (pParametro.FacturasDetalles.Exists(x => x.PrecioUnitarioSinIva == 0 || x.PrecioUnitarioSinIva == null))
                    {
                        pParametro.CodigoMensaje = "ValidarItemsFacturaPrecio";
                        return false;
                    }

                    if (pParametro.FacturasTiposPercepciones.Exists(x => x.TipoPercepcion.IdTipoPercepcion == 0
                            || x.Importe == 0))
                    {
                        pParametro.CodigoMensaje = "ValidarItemTipoPercepcion";
                        return false;
                    }
                    //if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasA
                    //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
                    //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoA)
                    //{
                    //    if (pParametro.FacturasDetalles.Exists(x => !x.ImporteIVA.HasValue || x.ImporteIVA.Value <= 0))
                    //    {
                    //        pParametro.CodigoMensaje = "ValidarItemsFacturaIVA";
                    //        pParametro.CodigoMensajeArgs.Add(pParametro.TipoFactura.Descripcion);
                    //        return false;
                    //    }
                    //}
                    if (pParametro.TipoFactura.Signo < 0 && pParametro.FacturasDetalles.Exists(x => x.SubTotal < 0))
                    {
                        pParametro.CodigoMensaje = "ValidarItemImporteNegativo";
                        return false;
                    }
                    if (pParametro.ImporteTotal < 0)
                    {
                        pParametro.CodigoMensaje = "ValidarImporteTotalNegativo";
                        return false;
                    }
                    if (pParametro.FacturaContado && pParametro.TipoFactura.Signo < 0)
                    {
                        pParametro.CodigoMensaje = "ValidarFacturaContadoSignoNegativo";
                        return false;
                    }
                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:
                    break;
                case EstadoColecciones.Modificado:

                    break;
                default:
                    break;
            }
            return true;
        }

        //public bool ActualizarPDF(VTAFacturas pParametro)
        //{
        //    bool resultado = true;
        //    DbTransaction tran;
        //    DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

        //    using (DbConnection con = bd.CreateConnection())
        //    {
        //        con.Open();
        //        tran = con.BeginTransaction();

        //        try
        //        {
        //            resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAFacturasInsertarPDF");
        //            if (resultado)
        //            {
        //                tran.Commit();
        //                pParametro.CodigoMensaje = "ResultadoTransaccion";
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
        //            pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
        //            pParametro.CodigoMensajeArgs.Add(ex.Message);
        //            return false;
        //        }
        //    }
        //    return resultado;
        //}

        public override bool Modificar(VTAFacturas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.ObtenerListaFacturasAsociadas();

            if (!this.Validar(pParametro, new VTAFacturas()))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            VTAFacturas valorViejo = new VTAFacturas();
            valorViejo.IdFactura = pParametro.IdFactura;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            //if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasA
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasB
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasC
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesA
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesB
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesC)
            //{
            //    pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.FacturaVenta;
            //}
            //else if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesA
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesB
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesC)
            //{
            //    pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.NotaCreditoVenta;
            //}
            //else if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoA
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoB
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoC
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesA
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesB
            //    || pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesC
            //    )
            //{
            //    pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.NotaDebitoVenta;
            //}
            //else if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.CbteInterno)
            //    pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.CbteInterno;
            //else if (pParametro.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.CbteInternoCredito)
            //    pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.CbteInternoCredito;

            if (!this.Validar(pParametro, new VTAFacturas()))
                return false;

            //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.FacturaVenta
            //            || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaCreditoVenta)
            //{
            //    CtbPeriodosIvas periodoFiltro = new CtbPeriodosIvas();
            //    periodoFiltro.Periodo = AyudaProgramacionLN.ObtenerPeriodo(pParametro.FechaFactura);
            //    if (ContabilidadF.PeriodosIvasValidarCierre(periodoFiltro))
            //    {
            //        pParametro.CodigoMensaje = "ValidarFechaContablePeriodoIvaCerrado";
            //        pParametro.CodigoMensajeArgs.Add(periodoFiltro.Periodo.ToString());
            //        return false;
            //    }
            //}

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //pParametro.PrefijoNumeroFactura = pParametro.PrefijoNumeroFactura.PadLeft(4, '0');
                    //pParametro.NumeroFactura = pParametro.NumeroFactura.PadLeft(8, '0');

                    //switch (pParametro.FilialPuntoVenta.TipoPuntoVenta.IdTipoPuntoVenta)
                    //{
                    //    case (int)EnumAFIPTiposPuntosVentas.ComprobanteEnLinea:
                    //        pParametro.CodigoMensaje = "ErrorValidarComprobanteEnLinea";
                    //        pParametro.CodigoMensajeArgs.Add(EnumAFIPTiposPuntosVentas.ComprobanteEnLinea.ToString());
                    //        resultado = false;
                    //        break;
                    //    case (int)EnumAFIPTiposPuntosVentas.ComprobanteManual:

                    //        break;
                    //    case (int)EnumAFIPTiposPuntosVentas.WebServiceFacturaElectronica:
                    //        pParametro.Estado.IdEstado = (int)EstadosFacturas.FESinValidadaAfip;
                    //        //pParametro.NumeroFactura = this.ObtenerProximoNumeroFactura(pParametro, bd, tran);
                    //        pParametro.NumeroFactura = pParametro.NumeroFactura.PadLeft(8, '0');
                    //        break;
                    //    default:
                    //        break;
                    //}
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "VTAFacturasValidaciones"))
                    {
                        resultado = false;
                    }

                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAFacturasActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.FacturaVenta
                        || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaCreditoVenta
                        || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaDebitoVenta)
                    {
                        if (pParametro.FechaFactura.Date != valorViejo.FechaFactura.Date)
                        {
                            if (resultado && !new InterfazContableLN().AnularComprobante(pParametro, bd, tran))
                                resultado = false;
                            if (resultado && !new InterfazContableLN().AgregarComprobante(pParametro, bd, tran))
                                resultado = false;
                        }
                    }

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

        public bool Anular(VTAFacturas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            CtbPeriodosIvas periodoFiltro = new CtbPeriodosIvas();
            periodoFiltro.Periodo = AyudaProgramacionLN.ObtenerPeriodo(pParametro.FechaFactura);
            if (ContabilidadF.PeriodosIvasValidarCierre(periodoFiltro))
            {
                pParametro.CodigoMensaje = "ValidarFechaContablePeriodoIvaCerrado";
                pParametro.CodigoMensajeArgs.Add(periodoFiltro.Periodo.ToString());
                return false;
            }
            //CtbPeriodosContables perConta = new CtbPeriodosContables();
            //perConta.Periodo = AyudaProgramacionLN.ObtenerPeriodo(DateTime.Now);
            //if (ContabilidadF.PeriodosContablesValidarCierre(perConta))
            //{
            //    AyudaProgramacionLN.MapearError(pParametro, perConta);
            //    return false;
            //}            

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            VTAFacturas valorViejo = new VTAFacturas();
            valorViejo.IdFactura = pParametro.IdFactura;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "VTAFacturasActualizarEstado");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.ItemsActualizar(pParametro, valorViejo, bd, tran))
                    //    resultado = false;

                    if (resultado && pParametro.TipoOperacion.Contabiliza && !new InterfazContableLN().AnularComprobante(pParametro, bd, tran))
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

        public bool ModificarEstado(VTAFacturas pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "VTAFacturasActualizarEstado"))
                return false;

            return true;
        }

        #region "Items Factura"

        private bool ItemsActualizar(VTAFacturas pParametro, VTAFacturas pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (VTAFacturasDetalles item in pParametro.FacturasDetalles)
            {
                switch (item.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        item.IdFactura = pParametro.IdFactura;

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

                        if (item.RemitosDetalles.Count > 0)
                        {
                            XmlDocument xml = new XmlDocument();
                            XmlNode items = xml.CreateElement("RemitosDetalles");
                            xml.AppendChild(items);

                            XmlNode itemNodo;
                            XmlNode ValorNode;

                            //int CantidadSinRemitar = Convert.ToInt32(item.Cantidad);
                            decimal? cantidadItemFactura = item.Cantidad.HasValue ? item.Cantidad : 0;

                            foreach (VTARemitosDetalles detalleR in item.RemitosDetalles)
                            {

                                itemNodo = xml.CreateElement("RemitoDetalle");

                                ValorNode = xml.CreateElement("IdFacturaDetalle");
                                ValorNode.InnerText = item.IdFacturaDetalle.ToString();
                                itemNodo.AppendChild(ValorNode);

                                ValorNode = xml.CreateElement("IdRemitoDetalle");
                                ValorNode.InnerText = detalleR.IdRemitoDetalle.ToString();
                                itemNodo.AppendChild(ValorNode);


                                if ((detalleR.Cantidad - cantidadItemFactura) <= 0)
                                {

                                    ValorNode = xml.CreateElement("Cantidad");
                                    ValorNode.InnerText = detalleR.Cantidad.ToString();
                                    itemNodo.AppendChild(ValorNode);

                                    //CantidadSinRemitar = CantidadSinRemitar - Convert.ToInt32(detalleR.Cantidad);
                                    cantidadItemFactura = cantidadItemFactura - detalleR.Cantidad;

                                }
                                else if ((detalleR.Cantidad - cantidadItemFactura) > 0)
                                {
                                    ValorNode = xml.CreateElement("Cantidad");
                                    ValorNode.InnerText = cantidadItemFactura.ToString();
                                    itemNodo.AppendChild(ValorNode);

                                    cantidadItemFactura = 0;
                                }
                                items.AppendChild(itemNodo);
                            }
                            item.LoteRemitosDetalles = xml;
                            if (cantidadItemFactura != 0)
                            {
                                pParametro.CodigoMensaje = "No se puede facturar una cantidad mayor a la de los remitos importados.";
                                return false;
                            }
                        }
                        // XML FOR EACH
                        item.IdFacturaDetalle = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "VTAFacturasDetallesInsertar");
                        if (item.IdFacturaDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        //ACTUALIZACION DE REMITO DETALLE
                        break;
                    #endregion
                    case EstadoColecciones.Borrado:
                        break;
                    #region Modificado
                    case EstadoColecciones.Modificado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "VTAFacturasDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.FacturasDetalles.Find(x => x.IdFacturaDetalle == item.IdFacturaDetalle), Acciones.Update, item, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }

                        //ACTUALIZACION DE REMITO DETALLE
                        if (item.RemitosDetalles.Count > 0)
                        {
                            foreach (VTARemitosDetalles detalleR in item.RemitosDetalles)
                            {
                                //seteo IdFacturaDetalle
                                detalleR.IdFacturaDetalle = null; // seteo como NULL para que el item del remito quede libre para ser facturado nuevamente
                                detalleR.UsuarioLogueado = pParametro.UsuarioLogueado;
                                if (!BaseDatos.ObtenerBaseDatos().Actualizar(detalleR, bd, tran, "VTARemitosDetallesActualizar"))
                                {
                                    AyudaProgramacionLN.MapearError(detalleR, pParametro);
                                    return false;
                                }
                                //if (!AuditoriaF.AuditoriaAgregar(
                                //    pValorViejo.RemitosDetalles.Find(x => x.IdRemitoDetalle == item.IdRemitoDetalle), Acciones.Update, item, bd, tran))
                                //{
                                //    AyudaProgramacionLN.MapearError(item, pParametro);
                                //    return false;
                                //}
                            }
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            return true;
        }

        private bool ActualizarTiposPercepciones(VTAFacturas pParametro, VTAFacturas pValorViejo, Database db, DbTransaction tran)
        {
            foreach (VTAFacturasTiposPercepciones Detalle in pParametro.FacturasTiposPercepciones)
            {
                switch (Detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        Detalle.Estado.IdEstado = (int)Estados.Activo;
                        Detalle.IdFactura = pParametro.IdFactura;
                        Detalle.idFacturaTipoPercepcion = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, db, tran, "VTAFacturasTiposPercepcionesInsertar");
                        if (Detalle.idFacturaTipoPercepcion == 0)
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;           //CREAR STORE  ¬
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "VTAFacturasTiposPercepcionesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.FacturasTiposPercepciones.Find(x => x.idFacturaTipoPercepcion == Detalle.idFacturaTipoPercepcion), Acciones.Update, Detalle, db, tran))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }

                        break;
                        #endregion
                }
            }

            return true;
        }

        #endregion

        internal bool ActualizarNotaCredito(VTAFacturas pParametro, Database bd, DbTransaction tran)
        {
            BaseDatos datos = BaseDatos.ObtenerBaseDatos();
            Hashtable param;

            foreach (VTAFacturas factAsoc in pParametro.FacturasAsociadas)
            {
                param = new Hashtable();
                param.Add("IdFactura", pParametro.IdFactura);
                param.Add("IdFacturaNotaCredito", factAsoc.IdFactura);

                if (!datos.Actualizar(param, bd, tran, "VTAFacturasNotasCreditosInsertar"))
                {
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    return false;
                }
            }
            return true;
        }

        //private bool ValidarRemitoImportado(VTAFacturas pParametro, Database bd, DbTransaction tran)
        //{
        //    //si la factura tiene items importados desde un remito valido los items
        //    if (pParametro.FacturasDetalles.Exists(x => x.DetalleImportado))
        //    {
        //        pParametro.RemitoVentaAutomatico = false;
        //        VTARemitos remitoImportado = new VTARemitos();
        //        //obtengo los datos completos del remito que importe
        //        remitoImportado.IdRemito = Convert.ToInt32(pParametro.IdRemitoImportado);
        //        remitoImportado = new VTARemitosLN().ObtenerDatosCompletos(remitoImportado);
        //        if (remitoImportado.Afiliado.IdAfiliado != pParametro.Afiliado.IdAfiliado)
        //        {
        //            pParametro.CodigoMensaje = "ValidarClienteRemito";
        //            return false;
        //        }
        //        //valido que los items de las facturas tengan la misma cantidad o mayor a los ya remitados
        //        if (!this.ValidarCantidadDetalle(pParametro, remitoImportado))
        //        {
        //            return false;
        //        }
        //        //Si pasa las validaciones de Cantidad... 
        //        //Actualizo el Remito con el IdFactura, para que quede Facturado
        //        remitoImportado.Factura.IdFactura = pParametro.IdFactura;
        //        if (!BaseDatos.ObtenerBaseDatos().Actualizar(remitoImportado, bd, tran, "VTARemitosActualizar"))
        //        {
        //            AyudaProgramacionLN.MapearError(remitoImportado, pParametro);
        //            return false;
        //        }
        //        //Actualizo en la base de datos el detalle del remito, seteandole el IdFacturaDetalle.
        //        foreach (VTARemitosDetalles detalle in remitoImportado.RemitosDetalles)
        //        {
        //            detalle.IdFacturaDetalle = pParametro.FacturasDetalles.Find(x => x.ListaPrecioDetalle.Producto.IdProducto == detalle.Producto.IdProducto && x.DetalleImportado).IdFacturaDetalle;

        //            if (!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, bd, tran, "VTARemitosDetallesActualizar"))
        //            {
        //                AyudaProgramacionLN.MapearError(detalle, pParametro);
        //                return false;
        //            }
        //        }
        //    }
        //    return true;
        //}

        //private bool ValidarCantidadDetalle(VTAFacturas pParametro, VTARemitos remitoImportado)
        //{
        //    bool result = true;
        //    List<VTAFacturasDetalles> listaImportada = new List<VTAFacturasDetalles>();
        //    //de todos los items de la factura obtengo los que fueron importados desde un remito
        //    foreach (VTAFacturasDetalles item in pParametro.FacturasDetalles)
        //    {
        //        if (item.DetalleImportado == true)
        //        {
        //            listaImportada.Add(item);
        //        }
        //    }
        //    //Valido cantidades
        //    foreach (VTAFacturasDetalles itemImport in listaImportada)
        //    {
        //        if (itemImport.Cantidad < remitoImportado.RemitosDetalles.Find(x => x.Producto.IdProducto == itemImport.ListaPrecioDetalle.Producto.IdProducto).Cantidad)
        //        {
        //            int cantidadRemitada = remitoImportado.RemitosDetalles.Find(x => x.Producto.IdProducto == itemImport.ListaPrecioDetalle.Producto.IdProducto).Cantidad;
        //            int codigoProducto = itemImport.ListaPrecioDetalle.Producto.IdProducto;
        //            pParametro.CodigoMensaje = "ValidarCantidadItemFacturaImportado";
        //            pParametro.CodigoMensajeArgs.Add(codigoProducto.ToString()); //codigo {0}
        //            pParametro.CodigoMensajeArgs.Add(cantidadRemitada.ToString()); //CANTIDAD REMITADA {1}
        //            return false;
        //        }
        //    }

        //    //si la cantidad es mayor o igual devuelvo TRUE
        //    return result;
        //}

        public List<TGETiposFacturas> ObtenerPorCondicionFiscal(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGETiposFacturas>("VTATiposFacturasSeleccionarPorCondicionFiscal", pParametro);
        }

        public List<TGETiposFacturas> ObtenerActivosPorIdTipoFactura(TGETiposFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGETiposFacturas>("VTATiposFacturasSeleccionarPorIdTipoFactura", pParametro);
        }
        public List<VTAFacturasTiposPercepciones> ObtenerPercepcionesPorIdRefTabla(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasTiposPercepciones>("VTAFacturasPercepcionesPorIdRefTabla", pParametro);
        }

        public DataTable ObtenerDetallesPorIdFactura(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTAFacturasDetallesPopUpPorIdFactura", pParametro);
        }

        public DataTable ObtenerDetallesPendienteEntrega(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTAFacturasDetallesPendienteEntregaListar", pParametro);
        }

        public DataTable ObtenerDetallesPendienteFacturar(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTARemitosSeleccionarListaFiltroPopUp", pParametro);
        }

        public DataTable ObtenerDetallesAcopiosPendienteEntrega(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("VTAFacturasDetallesAcopiosPendienteEntrega", pParametro);
        }

        public List<VTAFacturasDetalles> ObtenerDetallesPendientesPorRefTabla(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasDetalles>("VTAFacturasDetallesSeleccionarPendientePorRefTabla", pParametro);
        }
        public List<VTAFacturasDetalles> ObtenerDetallesServiciosPendientesPorRefTabla(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasDetalles>("VTAFacturasDetallesSeleccionarServiciosPendientePorRefTabla", pParametro);
        }

        /*Para notas de credito modulo hotel*/
        public List<VTAFacturasDetalles> ObtenerDetallesPorIdFacturaRefTabla(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturasDetalles>("VTAFacturasDetallesSeleccionarPorIdFacturaRefTabla", pParametro);
        }
        /************************************/

        public List<VTAFacturas> ObtenerComboAsociados(VTAFacturas pFactura)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<VTAFacturas>("VTAFacturasSeleccionarComboAsociados", pFactura);
        }
    }
}

