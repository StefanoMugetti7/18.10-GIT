using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
//using Facturas.ar.gov.afip.wswhomo;
using Facturas.Entidades;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Facturas.ar.gov.afip.fe;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Net;

namespace Facturas.FacturaElectronica
{
    class FacturaElectronica
    {
        FEAuthRequest _aut;
        Service _servicio;

        public FacturaElectronica()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            this._servicio = new Service();
            this._servicio.Timeout = int.MaxValue;
        }

        /// <summary>
        /// Valida una Factura en AFIP y obtiene el CAE y la Fecha de Vencimiento
        /// </summary>
        /// <param name="pFactura"></param>
        /// <returns></returns>
        //public bool ValidarFacturaAFIP(VTAFacturas pFactura )
        //{
        //    if (!this.ObtenerAutenticacion(pFactura))
        //        return false;

        //    if (!this.ValidarPuntosVentas(pFactura))
        //        return false;

        //    //Objetos para la FE
        //    FECAERequest req = new FECAERequest();
        //    FECAECabRequest cab = new FECAECabRequest();
        //    FECAEDetRequest det = new FECAEDetRequest();

        //    cab.CantReg = 1;
        //    cab.PtoVta = Convert.ToInt32(pFactura.PrefijoNumeroFactura);
        //    cab.CbteTipo = Convert.ToInt32(pFactura.TipoFactura.CodigoValor);

        //    int conceptoCte = 0;
        //    if (!int.TryParse(pFactura.ConceptoComprobante.CodigoValor, out conceptoCte))
        //    {
        //        pFactura.CodigoMensaje = "ErrorValidarConceptoComprobante";
        //        return false;
        //    }

        //    //Array Detalle
        //    det.Concepto = conceptoCte;
        //    det.DocTipo = pFactura.Afiliado.TipoDocumento.AfipCodigo;
        //    det.DocNro = pFactura.Afiliado.NumeroDocumento;

        //    //SOLO PARA UN COMPROBANTE
        //    det.CbteDesde = Convert.ToInt64(pFactura.NumeroFactura);
        //    det.CbteHasta = det.CbteDesde;
        //    det.CbteFch = pFactura.FechaFactura.ToString("yyyyMMdd");
        //    //CAMBIO LOS IMPORTES NEGATIVOS A POSITIVOS. ESTO ES PORQUE LAS NOTAS DE CREDITOS LAS MUESTRO EN NEGATIVO
        //    det.ImpTotal = pFactura.ImporteTotal < 0 ? Convert.ToDouble(pFactura.ImporteTotal * -1) : Convert.ToDouble(pFactura.ImporteTotal);
        //    det.ImpTotConc = 0;
        //    det.ImpNeto = pFactura.ImporteSinIVA < 0 ? Convert.ToDouble( pFactura.ImporteSinIVA *-1) : Convert.ToDouble( pFactura.ImporteSinIVA);
        //    det.ImpOpEx = 0;
        //    det.ImpTrib = 0;
        //    //det.ImpIVA =  Convert.ToDouble( pFactura.IvaTotal);
        //    if (pFactura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.ProductosYServicios
        //        || pFactura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.Servicios)
        //    {
        //        det.FchServDesde = DateTime.Now.ToString("yyyyMMdd");
        //        det.FchServHasta = DateTime.Now.ToString("yyyyMMdd");
        //        det.FchVtoPago = DateTime.Now.ToString("yyyyMMdd");
        //    }
        //    det.MonId = pFactura.Moneda.AfipCodigo;
        //    det.MonCotiz = 1;

        //    if (pFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasA
        //        || pFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
        //        || pFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasB
        //        || pFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
        //        || pFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoA
        //        || pFactura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoA)
        //    {
        //        AlicIva iva;
        //        foreach (VTAFacturasDetalles detalle in pFactura.FacturasDetalles)
        //        {
        //            if (detalle.IVA.AfipCodigo == 2)
        //                continue;

        //            if (det.Iva == null || !det.Iva.ToList().Exists(x => x.Id == detalle.IVA.AfipCodigo))
        //            {
        //                iva = new AlicIva();
        //                iva.Id = detalle.IVA.AfipCodigo;
        //                if (det.Iva == null)
        //                    det.Iva = new AlicIva[] { iva };
        //                else
        //                    det.Iva.ToList().Add(iva);
        //            }
        //            else
        //            {
        //                iva = det.Iva.ToList().Find(x => x.Id == detalle.IVA.AfipCodigo);
        //            }

        //                iva.BaseImp += Convert.ToDouble(detalle.SubTotal);
        //                iva.Importe += Convert.ToDouble(detalle.ImporteIVA);
        //        }
        //    }
        //    //Agregado por error en AFIP con Decimales
        //    if (det.Iva != null)
        //    {
        //        for (int i = 0; i < det.Iva.Length; i++)
        //        {
        //            det.Iva[i].BaseImp = Math.Round(det.Iva[i].BaseImp, 2, MidpointRounding.AwayFromZero);
        //            det.Iva[i].Importe = Math.Round(det.Iva[i].Importe, 2, MidpointRounding.AwayFromZero);
        //        }
        //    }
        //    det.ImpIVA = pFactura.IvaTotal < 0 ? Convert.ToDouble(pFactura.IvaTotal * -1) : Convert.ToDouble(pFactura.IvaTotal); //det.Iva == null ? 0 : det.Iva.ToList().Sum(x => x.Importe);

        //    //SOLO MANDO UN COMPROBANTE POR VEZ
        //    req.FeCabReq = cab;
        //    req.FeDetReq = new FECAEDetRequest[] { det };

        //    FECAEResponse res = _servicio.FECAESolicitar(_aut, req);

        //    //A=APROBADO, R=RECHAZADO, P=PARCIAL
        //    if (res.FeCabResp.Resultado == "A")
        //    {
        //        //SOLO MANDO UN COMPROBANTE POR VEZ
        //        pFactura.CAE = res.FeDetResp[0].CAE;
        //        pFactura.CAEFechaVencimiento = res.FeDetResp[0].CAEFchVto;
        //        return true;
        //    }

        //    //TRATAMIENTO DE ERRORES
        //    if (res.Errors != null)
        //        for (int i = 0; i < res.Errors.Length; i++)
        //        {
        //            pFactura.CodigoMensaje = "ErrorAfipFECAESolicitarGeneral";
        //            pFactura.CodigoMensajeArgs.Add(res.Errors[i].Code.ToString());
        //            pFactura.CodigoMensajeArgs.Add(res.Errors[i].Msg);
        //            return false;
        //        }

        //    if (res.FeDetResp != null)
        //    {
        //        for (int i = 0; i < res.FeDetResp.Length; i++)
        //        {
        //            if (res.FeDetResp[i].Observaciones != null)
        //            {
        //                for (int j = 0; j < res.FeDetResp[i].Observaciones.Length; j++)
        //                {
        //                    pFactura.CodigoMensaje = "ErrorAfipFECAESolicitarDetalle";
        //                    pFactura.CodigoMensajeArgs.Add(res.FeDetResp[i].Observaciones[j].Code.ToString());
        //                    pFactura.CodigoMensajeArgs.Add(res.FeDetResp[i].Observaciones[j].Msg);
        //                    return false;
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        /// <summary>
        /// Valida un lote de Facturas en AFIP y obtiene el CAE y la Fecha de Vencimiento
        /// </summary>
        /// <param name="pFactura"></param>
        /// <returns></returns>
        public bool ValidarFacturaAFIP(Objeto resultado, List<VTAFacturas> pFacturas)
        {
            try
            {
                if (!this.ObtenerAutenticacion(resultado))
                    return false;

                DummyResponse resVal = _servicio.FEDummy();
                if (resVal == null)
                {
                    resultado.CodigoMensaje = "ErrorAfipValidarServicio";
                    return false;
                }
                /*
                 * FeCabReq: La cabecera del comprobante o lote de comprobantes de ingreso está compuesta por los siguientes campos
                 * CantReg Int (4)  Cantidad de registros del detalle del comprobante o lote de comprobantes de ingreso
                 * CbteTipo Int (3) Tipo de comprobante que se está informando.  Si se informa más de un comprobante, todos deben ser del mismo tipo
                 * PtoVta Int (4)   Punto de Venta del comprobante que se está informando. Si se informa más de un comprobante, todos deben corresponder al mismo punto de venta
                 */
                decimal ultimoNumeroFactura;
                List<TGEIVA> lstIVA = TGEGeneralesF.TGEIVAAlicuotaObtenerLista();
                TGEIVA ivaEvol;
                List<VTAFacturas> ultimasFacturasAutorizadasAfip = new List<VTAFacturas>();
                VTAFacturas facturaAutAfip;
                List<FECAERequest> listaValidar = new List<FECAERequest>();
                //Objetos para la FE
                FECAERequest req;
                FECAEDetRequest det; ;
                List<FECAEDetRequest> listaDetalles;
                foreach (VTAFacturas factura in pFacturas)
                {
                    //Valido las ultimas facturas autorizadas en AFIP
                    int puntoVenta = Convert.ToInt32(factura.PrefijoNumeroFactura);
                    int tipoCbte = Convert.ToInt32(factura.TipoFactura.CodigoValor);
                    FERecuperaLastCbteResponse ultCbteAut = _servicio.FECompUltimoAutorizado(_aut, puntoVenta, tipoCbte);

                    if (ultCbteAut == null)
                    {
                        resultado.CodigoMensaje = "ErrorAfipValidarObtenerUltimoComprobante";
                        return false;
                    }
                    else if (ultCbteAut.Errors != null && ultCbteAut.Errors.Length > 0)
                    {
                        foreach (Err error in ultCbteAut.Errors.ToList())
                        {
                            factura.ObservacionesErrores = string.Concat(factura.ObservacionesErrores, " Codigo: ", error.Code.ToString(), " - Descripcion: ", error.Msg);
                        }
                        factura.EstadoColeccion = EstadoColecciones.Modificado;
                        resultado.CodigoMensaje = factura.ObservacionesErrores;
                        return false;
                    }

                    if (!ultimasFacturasAutorizadasAfip.Exists(x => x.PrefijoNumeroFactura == factura.PrefijoNumeroFactura
                                                                && x.TipoFactura.CodigoValor == factura.TipoFactura.CodigoValor))
                    {
                        facturaAutAfip = new VTAFacturas();
                        facturaAutAfip.TipoFactura.CodigoValor = factura.TipoFactura.CodigoValor;
                        facturaAutAfip.PrefijoNumeroFactura = factura.PrefijoNumeroFactura;
                        facturaAutAfip.NumeroFactura = ultCbteAut.CbteNro.ToString().PadLeft(8, '0');
                        ultimasFacturasAutorizadasAfip.Add(facturaAutAfip);
                    }

                    ultimoNumeroFactura = ultimasFacturasAutorizadasAfip.Where(x => x.TipoFactura.CodigoValor == factura.TipoFactura.CodigoValor
                                             && x.PrefijoNumeroFactura == factura.PrefijoNumeroFactura).Max(x => Convert.ToDecimal(x.NumeroFactura));

                    if (Convert.ToDecimal(factura.NumeroFactura) <= ultimoNumeroFactura)
                    {
                        if (!this.ConsultarDatosAutorizacion(factura))
                        {
                            AyudaProgramacionLN.MapearError(factura, resultado);
                            return false;
                        }
                        else
                        {
                            factura.Estado.IdEstado = (int)EstadosFacturas.Activo;
                            factura.EstadoColeccion = EstadoColecciones.Modificado;
                            continue;
                        }
                    }

                    //Fin valido facturas ya autorizadas

                    if (!listaValidar.Exists(x => x.FeCabReq != null
                        && x.FeCabReq.PtoVta == Convert.ToInt32(factura.PrefijoNumeroFactura)
                        && x.FeCabReq.CbteTipo == Convert.ToInt32(factura.TipoFactura.CodigoValor)))
                    {
                        req = new FECAERequest();
                        req.FeCabReq = new FECAECabRequest();
                        req.FeCabReq.PtoVta = Convert.ToInt32(factura.PrefijoNumeroFactura);
                        req.FeCabReq.CbteTipo = Convert.ToInt32(factura.TipoFactura.CodigoValor);
                        if (!this.ValidarPuntosVentas(factura))
                        {
                            resultado.CodigoMensaje = factura.CodigoMensaje;
                            resultado.CodigoMensajeArgs.Add(factura.ObservacionesErrores);
                            return false;
                        }
                        listaValidar.Add(req);
                    }

                    req = listaValidar.Find(x => x.FeCabReq.PtoVta == Convert.ToInt32(factura.PrefijoNumeroFactura)
                        && x.FeCabReq.CbteTipo == Convert.ToInt32(factura.TipoFactura.CodigoValor));

                    //Array Detalle (por cada factura que quiero validar)
                    det = new FECAEDetRequest();

                    int conceptoCte = 0;
                    if (!int.TryParse(factura.ConceptoComprobante.CodigoValor, out conceptoCte))
                    {
                        resultado.CodigoMensaje = "ErrorValidarConceptoComprobante";
                        resultado.CodigoMensajeArgs.Add(factura.TipoFactura.Descripcion);
                        resultado.CodigoMensajeArgs.Add(factura.NumeroFacturaCompleto);
                        return false;
                    }

                    det.Concepto = conceptoCte;
                    det.DocTipo = Convert.ToInt32( factura.ClienteTipoDocumentoAfipCodigo);
                    det.DocNro = Convert.ToInt64( factura.ClienteCuit );

                    det.CbteDesde = Convert.ToInt64(factura.NumeroFactura);
                    det.CbteHasta = det.CbteDesde;
                    det.CbteFch = factura.FechaFactura.ToString("yyyyMMdd");
                    //det.FchVtoPago = factura.FechaVencimiento.ToString("yyyyMMdd");
                    //CAMBIO LOS IMPORTES NEGATIVOS A POSITIVOS. ESTO ES PORQUE LAS NOTAS DE CREDITOS LAS MUESTRO EN NEGATIVO
                    det.ImpTotal = factura.ImporteTotal < 0 ? Convert.ToDouble(factura.ImporteTotal * -1) : Convert.ToDouble(factura.ImporteTotal);
                    //Importe No GRAVADO
                    det.ImpTotConc = 0;
                    det.ImpNeto = factura.ImporteSinIVA < 0 ? Convert.ToDouble(factura.ImporteSinIVA * -1) : Convert.ToDouble(factura.ImporteSinIVA);
                    det.ImpOpEx = 0;
                    det.ImpTrib = Convert.ToDouble(factura.ImportePercepciones);
                    if (factura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.ProductosYServicios
                        || factura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.Servicios)
                    {
                        det.FchServDesde = factura.PeriodoFacturadoDesde.HasValue ? factura.PeriodoFacturadoDesde.Value.ToString("yyyyMMdd") : DateTime.Now.ToString("yyyyMMdd");
                        det.FchServHasta = factura.PeriodoFacturadoHasta.HasValue ? factura.PeriodoFacturadoHasta.Value.ToString("yyyyMMdd") : DateTime.Now.ToString("yyyyMMdd");
                        det.FchVtoPago = factura.FechaVencimiento.HasValue ? factura.FechaVencimiento.Value.ToString("yyyyMMdd") : string.Empty;
                    }
                    if (factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesC)
                    {
                        det.FchVtoPago = factura.FechaVencimiento.HasValue ? factura.FechaVencimiento.Value.ToString("yyyyMMdd") : string.Empty;
                    }

                    //if(factura.TipoOperacion.IdTipoOperacion==(int)EnumTGETiposOperaciones.NotaCreditoVenta
                    //    && (factura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.ProductosYServicios
                    //    || factura.ConceptoComprobante.IdConceptoComprobante == (int)EnumConceptosComprobantes.Servicios))
                    //    det.FchVtoPago = factura.FechaVencimiento.HasValue ? factura.FechaVencimiento.Value.ToString("yyyyMMdd") : string.Empty;
                    
                    det.MonId = factura.Moneda.AfipCodigo;
                    det.MonCotiz = Convert.ToDouble(factura.MonedaCotizacion);

                    if (factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturasB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesB
                        )
                    {
                        AlicIva iva;
                        foreach (VTAFacturasDetalles detalle in factura.FacturasDetalles)
                        {
                            //if (detalle.IVA.AfipCodigo == 2)
                            //    continue;

                            //Sumo los importes no gravados
                            if (detalle.IVA.IdIVA == (int)EnumIVA.IVANoGrabado
                                || detalle.IVA.IdIVA == (int)EnumIVA.NoCorresponde)
                            {
                                det.ImpTotConc += Convert.ToDouble(detalle.SubTotal);
                                continue;
                            }
                            else if (detalle.IVA.IdIVA == (int)EnumIVA.IVAExcento)
                            {
                                det.ImpOpEx += Convert.ToDouble(detalle.SubTotal);
                                continue;
                            }

                            if (det.Iva == null || !det.Iva.ToList().Exists(x => x.Id == detalle.IVA.AfipCodigo))
                            {
                                iva = new AlicIva();
                                iva.Id = detalle.IVA.AfipCodigo;
                                if (det.Iva == null)
                                    det.Iva = new AlicIva[] { iva };
                                else
                                    det.Iva.ToList().Add(iva);
                            }
                            else
                            {
                                iva = det.Iva.ToList().Find(x => x.Id == detalle.IVA.AfipCodigo);
                            }
                            iva.Importe += Convert.ToDouble(detalle.ImporteIVA);
                            iva.BaseImp += detalle.ImporteIVA == 0 ? 0 : Convert.ToDouble(detalle.SubTotal);
                        }
                    }
                    det.ImpNeto -= (det.ImpTotConc + det.ImpOpEx);
                    det.ImpNeto = Math.Round(det.ImpNeto, 2, MidpointRounding.AwayFromZero);
                    det.ImpTotConc = Math.Round(det.ImpTotConc, 2, MidpointRounding.AwayFromZero);
                    det.ImpOpEx = Math.Round(det.ImpOpEx, 2, MidpointRounding.AwayFromZero);
                    //Agregado por error en AFIP con Decimales
                    double sumIva = 0;
                    if (det.Iva != null)
                    {
                        for (int i = 0; i < det.Iva.Length; i++)
                        {
                            det.Iva[i].BaseImp = Math.Round(det.Iva[i].BaseImp, 2, MidpointRounding.AwayFromZero);
                            det.Iva[i].Importe = Math.Round(det.Iva[i].Importe, 2, MidpointRounding.AwayFromZero);
                            ivaEvol = lstIVA.FirstOrDefault(x => x.AfipCodigo == det.Iva[i].Id);
                            double ivaCal = Math.Round(det.Iva[i].BaseImp * Convert.ToDouble(ivaEvol.Alicuota) / 100, 2, MidpointRounding.AwayFromZero);
                            if (ivaCal != det.Iva[i].Importe)
                            {
                                det.Iva[i].Importe = ivaCal;
                                det.ImpOpEx += ivaCal > det.Iva[i].Importe ? det.Iva[i].Importe - ivaCal : ivaCal - det.Iva[i].Importe;
                            }
                            sumIva += det.Iva[i].Importe;
                        }
                    }
                    det.ImpIVA = sumIva; // factura.IvaTotal < 0 ? Convert.ToDouble(factura.IvaTotal * -1) : Convert.ToDouble(factura.IvaTotal); //det.Iva == null ? 0 : det.Iva.ToList().Sum(x => x.Importe);

                    /* Otros Tributos */
                    det.ImpTrib = Convert.ToDouble( factura.FacturasTiposPercepciones.Sum(x => x.Importe));
                    Tributo tributo;
                    List<Tributo> lstTributos = new List<Tributo>();
                    foreach (VTAFacturasTiposPercepciones detalle in factura.FacturasTiposPercepciones)
                    {
                        tributo = new Tributo();
                        //switch (detalle.TipoPercepcion.CodigoValor)
                        //{
                        //    case "01":
                        //        tributo.Id = 01;
                        //        break;
                        //    case "02":
                        //        tributo.Id = 01;
                        //        break;
                        //    case "901":
                        //        tributo.Id = 02;
                        //        break;
                        //    case "04":
                        //        tributo.Id = 03;
                        //        break;
                        //    case "902":
                        //        tributo.Id = 02;
                        //        break;
                        //    case "05":
                        //        tributo.Id = 04;
                        //        break;
                        //    default:
                        //        tributo.Id = 99;
                        //        break;
                        //}

                        //if (det.Tributos == null)
                        //    det.Tributos = new Tributo[factura.FacturasTiposPercepciones.Count] { tributo };
                        //else
                        //    det.Tributos = det.Tributos.Append(tributo); // det.Tributos.ToList().Add(tributo);
                        tributo.Id = Convert.ToInt16(detalle.TributoId);
                        tributo.BaseImp = Convert.ToDouble(factura.ImporteSinIVA);
                        tributo.Desc = detalle.TipoPercepcion.Descripcion;
                        tributo.Alic = Convert.ToDouble(detalle.Porcentaje);
                        tributo.Importe = Convert.ToDouble(detalle.Importe);
                        lstTributos.Add(tributo);
                    }
                    if (lstTributos.Count > 0)
                        det.Tributos = lstTributos.ToArray();

                    /*Comprobantes Asociados*/
                    if (factura.FacturasAsociadas.Count > 0)
                    {
                        CbteAsoc ca;
                        foreach (VTAFacturas cbteAsoc in factura.FacturasAsociadas)
                        {
                            ca = new CbteAsoc();
                            ca.Tipo = Convert.ToInt32(cbteAsoc.TipoFactura.CodigoValor);
                            ca.PtoVta = Convert.ToInt32(cbteAsoc.PrefijoNumeroFactura);
                            ca.Nro = Convert.ToInt64(cbteAsoc.NumeroFactura);
                            ca.Cuit = _aut.Cuit.ToString();
                            ca.CbteFch = cbteAsoc.FechaFactura.ToString("yyyyMMdd");
                            if (det.CbtesAsoc == null)
                                det.CbtesAsoc = new CbteAsoc[] { ca };
                            else
                                det.CbtesAsoc.ToList().Add(ca);
                        }
                    }
                    /*Notas de Crédito sin comprobante asociado tiene que tener Periodo asociado*/
                    if (factura.TipoFactura.Signo == -1
                        && factura.FacturasAsociadas.Count == 0)
                    {
                        det.PeriodoAsoc = new Periodo();
                        DateTime fechaDesde = factura.PeriodoFacturadoDesde.HasValue ? factura.PeriodoFacturadoDesde.Value : DateTime.Now;
                        fechaDesde = fechaDesde.Date > factura.FechaFactura.Date ? factura.FechaFactura : fechaDesde;
                        det.PeriodoAsoc.FchDesde = fechaDesde.ToString("yyyyMMdd");
                        DateTime fechaHasta = factura.PeriodoFacturadoHasta.HasValue ? factura.PeriodoFacturadoHasta.Value : DateTime.Now;
                        fechaHasta = fechaHasta.Date > factura.FechaFactura.Date ? factura.FechaFactura : fechaHasta;
                        det.PeriodoAsoc.FchHasta = fechaHasta.ToString("yyyyMMdd");
                    }

                    if (factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoA
                    //    || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoB
                      //  || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoC
                      //  || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesC
                      )
                    {
                        det.PeriodoAsoc = new Periodo();
                        DateTime fechaDesde = factura.PeriodoFacturadoDesde.HasValue ? factura.PeriodoFacturadoDesde.Value : DateTime.Now;
                        fechaDesde = fechaDesde.Date > factura.FechaFactura.Date ? factura.FechaFactura : fechaDesde;
                        det.PeriodoAsoc.FchDesde = fechaDesde.ToString("yyyyMMdd");
                        DateTime fechaHasta = factura.PeriodoFacturadoHasta.HasValue ? factura.PeriodoFacturadoHasta.Value : DateTime.Now;
                        fechaHasta = fechaHasta.Date > factura.FechaFactura.Date ? factura.FechaFactura : fechaHasta;
                        det.PeriodoAsoc.FchHasta = fechaHasta.ToString("yyyyMMdd");
                    }
                    /* OPCIONALES */
                    if (factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesA
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesB
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesC
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesC
                        || factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaDebitoElectronicaMyPymesC
                        )
                    {
                        if (factura.Campos.Exists(x => x.Nombre == "AfipCbuEmisor"))
                        {
                            Opcional opCBU = new Opcional();
                            opCBU.Id = "2101";
                            opCBU.Valor = factura.Campos.Find(x => x.Nombre == "AfipCbuEmisor").CampoValor.Valor;
                            if (det.Opcionales == null)
                                det.Opcionales = new Opcional[] { opCBU };
                            else
                            {
                                List<Opcional> lst = det.Opcionales.ToList();
                                lst.Add(opCBU);
                                det.Opcionales = lst.ToArray();
                            }
                        }
                        if (factura.Campos.Exists(x => x.Nombre == "RG"))
                        {
                            Opcional opRG = new Opcional();
                            opRG.Id = "27";
                            TGECamposValores cv = factura.Campos.Find(x => x.Nombre == "RG").CampoValor;
                            string[] vals = cv.ListaValor.Trim().Length > 0 ? cv.ListaValor.Trim().Split('-') : null;
                            opRG.Valor = vals != null && vals.Length > 0 ? vals[0] : string.Empty;
                            if (det.Opcionales == null)
                                det.Opcionales = new Opcional[] { opRG };
                            else
                            {
                                List<Opcional> lst = det.Opcionales.ToList();
                                lst.Add(opRG);
                                det.Opcionales = lst.ToArray();
                            }
                        }
                        if (factura.Campos.Exists(x => x.Nombre == "AfipAnulacion"))
                        {
                            Opcional opAnulacion = new Opcional();
                            opAnulacion.Id = "22";
                            string valor = factura.Campos.Find(x => x.Nombre == "AfipAnulacion").CampoValor.Valor;
                            opAnulacion.Valor = string.IsNullOrEmpty(valor) ? "N" : valor == "206" ? "S" : "N";
                            if (det.Opcionales == null)
                                det.Opcionales = new Opcional[] { opAnulacion };
                            else
                            {
                                List<Opcional> lst = det.Opcionales.ToList();
                                lst.Add(opAnulacion);
                                det.Opcionales = lst.ToArray();
                            }
                        }
                    }

                    listaDetalles = new List<FECAEDetRequest>();
                    if (req.FeDetReq != null)
                        listaDetalles.AddRange(req.FeDetReq.ToList());
                    listaDetalles.Add(det);

                    req.FeDetReq = listaDetalles.ToArray();
                }

                FECAEResponse res;
                string erroresAfip = string.Empty;
                FERegXReqResponse regCantidad = _servicio.FECompTotXRequest(_aut);
                int cantidadMaxima = regCantidad.RegXReq;
                int contadorFactura;
                int contadorTotal;
                FECAERequest fecaeRequestVal;
                List<FECAEDetRequest> fecaeDetRequestVal;
                foreach (FECAERequest item in listaValidar)
                {
                    contadorFactura = 0;
                    contadorTotal = 0;
                    fecaeDetRequestVal = new List<FECAEDetRequest>();
                    foreach (FECAEDetRequest feDetReq in item.FeDetReq.ToList())
                    {
                        contadorFactura++;
                        contadorTotal++;
                        fecaeDetRequestVal.Add(feDetReq);
                        if (contadorFactura == cantidadMaxima
                            || contadorTotal == item.FeDetReq.ToList().Count)
                        {
                            fecaeRequestVal = new FECAERequest();
                            fecaeRequestVal.FeCabReq = item.FeCabReq;
                            fecaeRequestVal.FeDetReq = fecaeDetRequestVal.ToArray();
                            fecaeRequestVal.FeCabReq.CantReg = fecaeRequestVal.FeDetReq.ToList().Count;
                            res = _servicio.FECAESolicitar(_aut, fecaeRequestVal);

                            contadorFactura = 0;
                            fecaeDetRequestVal = new List<FECAEDetRequest>();
                            //item.FeCabReq.CantReg = item.FeDetReq.ToList().Count;
                            //res = _servicio.FECAESolicitar(_aut, item);

                            //A=APROBADO, R=RECHAZADO, P=PARCIAL
                            VTAFacturas facturaValida;
                            if (res.FeCabResp.Resultado == "A" || res.FeCabResp.Resultado == "P" || res.FeCabResp.Resultado == "R")
                            {
                                foreach (FECAEDetResponse resDet in res.FeDetResp.ToList())
                                {
                                    facturaValida = pFacturas.Find(x => Convert.ToInt32(x.ClienteTipoDocumentoAfipCodigo) == resDet.DocTipo
                                        && Convert.ToInt64(x.ClienteCuit)== resDet.DocNro
                                        && Convert.ToInt32(x.TipoFactura.CodigoValor) == res.FeCabResp.CbteTipo
                                        && Convert.ToInt32(x.PrefijoNumeroFactura) == res.FeCabResp.PtoVta
                                        && Convert.ToInt64(x.NumeroFactura) == resDet.CbteDesde
                                        && resDet.CbteDesde == resDet.CbteHasta);

                                    if (resDet.Resultado == "A")
                                    {
                                        facturaValida.CAE = resDet.CAE;
                                        facturaValida.CAEFechaVencimiento = resDet.CAEFchVto;
                                        facturaValida.Estado.IdEstado = (int)EstadosFacturas.Activo;
                                        facturaValida.EstadoColeccion = EstadoColecciones.Modificado;
                                    }
                                    else
                                    //RECHAZADO
                                    {
                                        if (resDet.Observaciones != null)
                                        {
                                            foreach (Obs observacion in resDet.Observaciones.ToList())
                                            {
                                                facturaValida.ObservacionesErrores = string.Concat(facturaValida.ObservacionesErrores, " Codigo: ", observacion.Code.ToString(), " - Descripcion: ", observacion.Msg);
                                            }
                                            facturaValida.EstadoColeccion = EstadoColecciones.Modificado;
                                        }
                                        else if (res.Errors != null)
                                        {
                                            foreach (Err error in res.Errors.ToList())
                                            {
                                                facturaValida.ObservacionesErrores = string.Concat(facturaValida.ObservacionesErrores, " Codigo: ", error.Code.ToString(), " - Descripcion: ", error.Msg);
                                            }
                                            facturaValida.EstadoColeccion = EstadoColecciones.Modificado;
                                        }
                                    }
                                }
                            }
                            //else
                            ////TRATAMIENTO DE ERRORES POR RECHAZO TOTAL "R"
                            //{
                            //    if (res.Errors != null)
                            //    {
                            //        for (int i = 0; i < res.Errors.Length; i++)
                            //        {
                            //            erroresAfip = string.Concat(resultado.CodigoMensaje, "Codigo: ", res.Errors[i].Code.ToString(), " - Descripcion: ", res.Errors[i].Msg);
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                int facturasValidadas = pFacturas.Count(x => x.Estado.IdEstado == (int)EstadosFacturas.Activo);
                if (facturasValidadas == pFacturas.Count)
                {
                    resultado.CodigoMensaje = "FacturaElectronicaAprobado";
                    resultado.CodigoMensajeArgs.Add(facturasValidadas.ToString());
                    resultado.CodigoMensajeArgs.Add(pFacturas.Count.ToString());
                    return true;
                }
                else if (facturasValidadas > 0)
                {
                    resultado.CodigoMensaje = "FactruaElectronicaParcial";
                    resultado.CodigoMensajeArgs.Add(facturasValidadas.ToString());
                    resultado.CodigoMensajeArgs.Add(pFacturas.Count.ToString());
                    return false;
                }
                else
                {
                    resultado.CodigoMensaje = "FacturaElectronicaRechazo";
                    resultado.CodigoMensajeArgs.Add(erroresAfip);
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Service Unavailable"))
                    resultado.CodigoMensaje = "El Servicio de AFIP no se encuentra disonible. Vuelva a intentar en unos minutos. " + ex.Message;
                else
                    resultado.CodigoMensaje = ex.Message;
                return false;
            }

        }

        /// <summary>
        /// Valida que el Web Service este activo
        /// </summary>
        /// <param name="pFactura"></param>
        /// <returns></returns>
        public bool ValidarServicio(VTAFacturas pFactura)
        {
            if (!this.ObtenerAutenticacion(pFactura))
                return false;
            try
            {
                DummyResponse res = _servicio.FEDummy();
                if (res == null)
                {
                    pFactura.CodigoMensaje = "ErrorAfipValidarServicio";
                    return false;
                }

                FERegXReqResponse respuesta = _servicio.FECompTotXRequest(_aut);
                if (respuesta == null)
                {
                    pFactura.CodigoMensaje = "ErrorAfipObtenerCantidadMaximaValidar";
                    return false;
                }
                pFactura.IdFactura = respuesta.RegXReq;
                return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Service Unavailable"))
                    pFactura.CodigoMensaje = "El Servicio de AFIP no se encuentra disonible. Vuelva a intentar en unos minutos. " + ex.Message;
                else
                    pFactura.CodigoMensaje = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Valida y obtiene el Proximo Numero de Comprobante
        /// </summary>
        /// <param name="pFactura"></param>
        /// <returns></returns>
        public bool ValidarProximoNumeroComprobante(VTAFacturas pFactura)
        {
            if (!this.ObtenerAutenticacion(pFactura))
                return false;

            if (!this.ValidarPuntosVentas(pFactura))
                return false;

            if (!this.ObtenerProximoNumeroComprobante(pFactura))
                return false;

            return true;
        }

        private bool ValidarPuntosVentas(VTAFacturas pFactura)
        {
            //Obtener Puntos de Ventas
            FEPtoVentaResponse ptoVta = _servicio.FEParamGetPtosVenta(_aut);
            if (ptoVta.ResultGet == null)
            {
                //Console.WriteLine("No existen Puntos de Ventas asociados a WebService de FE");
                pFactura.CodigoMensaje = "ErrorAfipValidarPuntoVentaAsociadosWebService";
                string msg = string.Empty;
                string sep = string.Empty;
                foreach (Err error in ptoVta.Errors)
                {
                    msg = string.Concat(msg, sep, error.Code, ": ", error.Msg);
                    sep = " - ";
                }
                pFactura.CodigoMensajeArgs.Add(msg);
                return false;
            }
            int puntoVenta = Convert.ToInt32(pFactura.FilialPuntoVenta.AfipPuntoVenta);
            if (!ptoVta.ResultGet.ToList().Exists(x => x.Nro == puntoVenta))
            {
                pFactura.CodigoMensaje = "ErrorAfipValidarPuntoVentaNumeroParametro";
                return false;
            }

            //if (!ptoVta.ResultGet.ToList().Exists(x => x.Bloqueado))
            //{ }

            return true;
        }

        private bool ObtenerProximoNumeroComprobante(VTAFacturas pFactura)
        {
            int puntoVenta = Convert.ToInt32(pFactura.PrefijoNumeroFactura);
            int tipoCbte = Convert.ToInt32(pFactura.TipoFactura.CodigoValor);
            FERecuperaLastCbteResponse ultCbteAut = _servicio.FECompUltimoAutorizado(_aut, puntoVenta, tipoCbte);
            if (ultCbteAut == null)
            {
                pFactura.CodigoMensaje = "ErrorAfipValidarObtenerUltimoComprobante";
                return false;
            }
            //pFactura.NumeroFactura = ultCbteAut.CbteNro.ToString().PadLeft(8, '0');
            pFactura.NumeroFactura = (ultCbteAut.CbteNro + 1).ToString().PadLeft(8, '0');
            return true;
        }

        private bool ObtenerAutenticacion(Objeto pParametro)
        {
            try { 
            if (this._aut != null && !string.IsNullOrEmpty(this._aut.Token))
                return true;

            ar.com.evol.erp.Objeto parametro = new ar.com.evol.erp.Objeto();
            parametro.CodigoMensaje = "ERP.EVOL.COM.AR.WS.INTERNAL.CODE";

            ar.com.evol.erp.AfipServiciosWebTickets ticket = new ar.com.evol.erp.AfipServiciosWebTickets();
            ticket = new ar.com.evol.erp.WSEvol().ObtenerAutenticacion(parametro);
            AyudaProgramacionLN.MatchObjectProperties(parametro, pParametro);

            if (ticket.IdLoginTicket == 0)
                return false;
            else
                pParametro.CodigoMensaje = string.Empty;

            DummyResponse res = _servicio.FEDummy();
            if (res == null)
            {
                pParametro.CodigoMensaje = "ErrorAfipValidarServicio";
                return false;
            }

            this._aut = new FEAuthRequest();
            this._aut.Token = ticket.Token;
            this._aut.Sign = ticket.Sign;

            string cuit = TGEGeneralesF.EmpresasSeleccionar().CUIT; // TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSCuit).ParametroValor;
            if (string.IsNullOrEmpty(cuit))
            {
                pParametro.CodigoMensaje = "ErrorValidarWSCuit";
                return false;
            }
            long cuitNumero;
            if (!long.TryParse(cuit, out cuitNumero))
            {
                pParametro.CodigoMensaje = "ErrorValidarWSCuit";
                return false;
            }
            this._aut.Cuit = cuitNumero;

            return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Service Unavailable"))
                    pParametro.CodigoMensaje = "El Servicio de AFIP no se encuentra disonible. Vuelva a intentar en unos minutos. " + ex.Message;
                else
                    pParametro.CodigoMensaje = ex.Message;
                return false;
            }
        }

        private bool ObtenerAutenticacion_Back(Objeto pParametro)
        {
            if (this._aut != null && !string.IsNullOrEmpty(this._aut.Token))
                return true;

            bool resultado = true;

            AfipServiciosWebTickets ticket = new AfipServiciosWebTickets();
            //Obtengo el Login Ticket
            LoginTicket objTicketRespuesta = new LoginTicket();
            //Objeto para Autenticacion
            this._aut = new FEAuthRequest();

            ticket.FechaEvento = DateTime.Now;
            ticket = BaseDatos.ObtenerBaseDatos().Obtener<AfipServiciosWebTickets>("AfipServiciosWebTicketsSeleccionar", ticket, BaseDatos.conexionErpComun);
            if (ticket.IdLoginTicket > 0)
            {
                this._aut.Token = ticket.Token;
                this._aut.Sign = ticket.Sign;
            }
            else
            {
                string path = "Modulos\\Facturas\\FacturaElectronica\\EvolCert.p12";
                if (!objTicketRespuesta.ObtenerLoginTicketResponse(pParametro, path))
                    return false;

                ticket = new AfipServiciosWebTickets();
                //ticket.UniqueId = objTicketRespuesta.UniqueId;
                ticket.GenerationTime = objTicketRespuesta.GenerationTime;
                ticket.ExpirationTime = objTicketRespuesta.ExpirationTime;
                ticket.Sign = objTicketRespuesta.Sign;
                ticket.Token = objTicketRespuesta.Token;
                ticket.LoginTicketResponse = objTicketRespuesta.LoginTicketResponse;

                //Guardo el Ticke en la BD
                DbTransaction tran;
                DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.Create(BaseDatos.conexionErpComun);

                using (DbConnection con = bd.CreateConnection())
                {
                    con.Open();
                    tran = con.BeginTransaction();

                    try
                    {
                        ticket.IdLoginTicket = BaseDatos.ObtenerBaseDatos().Agregar(ticket, bd, tran, "AfipServiciosWebTicketsInsertar");
                        if (ticket.IdLoginTicket == 0)
                            resultado = false;

                        if (resultado)
                        {
                            tran.Commit();
                            pParametro.CodigoMensaje = "ResultadoTransaccion";
                        }
                        else
                        {
                            tran.Rollback();
                            AyudaProgramacionLN.MapearError(ticket, pParametro);
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

                //Objeto para Autenticacion
                this._aut.Token = objTicketRespuesta.Token;
                this._aut.Sign = objTicketRespuesta.Sign;
            }

            string cuit = TGEGeneralesF.EmpresasSeleccionar().CUIT; // TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSCuit).ParametroValor;
            if (string.IsNullOrEmpty(cuit))
            {
                pParametro.CodigoMensaje = "ErrorValidarWSCuit";
                return false;
            }
            long cuitNumero;
            if (!long.TryParse(cuit, out cuitNumero))
            {
                pParametro.CodigoMensaje = "ErrorValidarWSCuit";
                return false;
            }
            this._aut.Cuit = cuitNumero;

            return true;
        }

        public bool ConsultarDatosAutorizacion(VTAFacturas pFactura)
        {
            if (!this.ObtenerAutenticacion(pFactura))
                return false;

            FECompConsultaReq compConsulta = new FECompConsultaReq();
            compConsulta.CbteTipo = Convert.ToInt32(pFactura.TipoFactura.CodigoValor);
            compConsulta.CbteNro = Convert.ToInt64(pFactura.NumeroFactura);
            compConsulta.PtoVta = Convert.ToInt32(pFactura.PrefijoNumeroFactura);

            FECompConsultaResponse respuesta = _servicio.FECompConsultar(this._aut, compConsulta);

            if (respuesta.ResultGet.Resultado == "A")
            {
                /* Validacion de Datos del Comprobante */
                DateTime fechaFactura = new DateTime(Convert.ToInt32(respuesta.ResultGet.CbteFch.Substring(0, 4)), Convert.ToInt32(respuesta.ResultGet.CbteFch.Substring(4, 2)), Convert.ToInt32(respuesta.ResultGet.CbteFch.Substring(6, 2)));
                
                if (Convert.ToInt64(pFactura.ClienteCuit) != respuesta.ResultGet.DocNro
                    || Math.Abs( pFactura.ImporteTotal) != Convert.ToDecimal(respuesta.ResultGet.ImpTotal)
                    || pFactura.FechaFactura.Date != fechaFactura.Date
                    )
                {
                    pFactura.CodigoMensaje = "Los datos del comprobante a Validar/Consultar no se corresponden con los datos ingresados en AFIP.";
                    return false;
                }
                pFactura.CAE = respuesta.ResultGet.CodAutorizacion;
                pFactura.CAEFechaVencimiento = respuesta.ResultGet.FchVto;
                return true;
            }
            else
            {
                //TRATAMIENTO DE ERRORES
                if (respuesta.Errors != null)
                    for (int i = 0; i < respuesta.Errors.Length; i++)
                    {
                        pFactura.CodigoMensaje = "ErrorAfipFECAEConsultarDatosAutorizacion";
                        pFactura.CodigoMensajeArgs.Add(respuesta.Errors[i].Code.ToString());
                        pFactura.CodigoMensajeArgs.Add(respuesta.Errors[i].Msg);
                    }
                return false;
            }
        }

        public bool ConsultarDatosAutorizacion2(VTAFacturas pFactura)
        {
            if (!this.ObtenerAutenticacion(pFactura))
                return false;

            //FETributoResponse tributos = _servicio.FEParamGetTiposTributos(this._aut);
            OpcionalTipoResponse opcionales = _servicio.FEParamGetTiposOpcional(this._aut);
            //OpcionalTipoResponse opcionales2 = _servicio.FEParamGetTiposOpcional(this._aut);

            FECompConsultaReq compConsulta = new FECompConsultaReq();
            compConsulta.CbteTipo = Convert.ToInt32(pFactura.TipoFactura.CodigoValor);
            compConsulta.CbteNro = Convert.ToInt64(pFactura.NumeroFactura);
            compConsulta.PtoVta = Convert.ToInt32(pFactura.PrefijoNumeroFactura);

            FECompConsultaResponse respuesta = _servicio.FECompConsultar(this._aut, compConsulta);


            if (respuesta.ResultGet != null && respuesta.ResultGet.Resultado == "A")
            {
                DateTime fechaFactura = new DateTime(Convert.ToInt32(respuesta.ResultGet.CbteFch.Substring(0,4)),Convert.ToInt32(respuesta.ResultGet.CbteFch.Substring(4, 2)), Convert.ToInt32(respuesta.ResultGet.CbteFch.Substring(6, 2)));
                pFactura.FechaFactura = fechaFactura;
                pFactura.Afiliado.TipoDocumento.AfipCodigo = respuesta.ResultGet.DocTipo;
                pFactura.Afiliado.NumeroDocumento = respuesta.ResultGet.DocNro;
                pFactura.CAE = respuesta.ResultGet.CodAutorizacion;
                pFactura.CAEFechaVencimiento = respuesta.ResultGet.FchVto;
                pFactura.ImporteSinIVA = Convert.ToDecimal( respuesta.ResultGet.ImpNeto);
                pFactura.IvaTotal = Convert.ToDecimal(respuesta.ResultGet.ImpIVA);
                pFactura.ImporteTotal = Convert.ToDecimal( respuesta.ResultGet.ImpTotal);
                return true;
            }
            else
            {
                //TRATAMIENTO DE ERRORES
                if (respuesta.Errors != null)
                    for (int i = 0; i < respuesta.Errors.Length; i++)
                    {
                        pFactura.CodigoMensaje = "ErrorAfipFECAEConsultarDatosAutorizacion";
                        pFactura.CodigoMensajeArgs.Add(respuesta.Errors[i].Code.ToString());
                        pFactura.CodigoMensajeArgs.Add(respuesta.Errors[i].Msg);
                    }
                return false;
            }
        }
    }
}
