using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Servicio.AccesoDatos;
using Tesorerias.Entidades;
using Generales.Entidades;
using System.Xml;
using Bancos.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.LogicaNegocio;
using Comunes.Entidades;

namespace Tesorerias.LogicaNegocio
{
    public class TESCajasMovimientosLN
    {
        public DataTable ObtenerGrilla(TESCajasMovimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TESCajasMovimientosSeleccionarDescripcionPorFiltroGrilla", pParametro);
        }
        public DataTable ObtenerGrillaFlujoEfectivo(TESCajasMovimientos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TesTesoreriaMovimientosFlujoTesoreriaCaja", pParametro);
        }

        public TESCajasMovimientos ObtenerMovmientoAConfirmarPorIdRefTipoOperacion(TESCajasMovimientos pParametro)
        {
            TESCajasMovimientos mov = new TESCajasMovimientos();
            DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet("TESCajasMovimientosAConfirmarPorIdRefTipoOperacion", pParametro);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    Mapeador.SetearEntidadPorFila(ds.Tables[0].Rows[0], mov);
                }
                
            }
            if (ds.Tables.Count > 1)
            {
                if (ds.Tables[1].Rows.Count > 0)
                {
                    TESCajasMovimientosValores pc;
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        pc = new TESCajasMovimientosValores();
                        pc.EstadoColeccion = EstadoColecciones.Agregado;                        
                        Mapeador.SetearEntidadPorfila(dr, pc, pc.EstadoColeccion);
                        mov.CajasMovimientosValores.Add(pc);
                        pc.IndiceColeccion = mov.CajasMovimientosValores.IndexOf(pc);
                    }
                }
            }

            if (ds.Tables.Count > 2)
            {
                if (ds.Tables[2].Rows.Count > 0)
                {
                    TESCheques cheques;
                    TESCajasMovimientosValores movValor;
                    foreach (DataRow dr in ds.Tables[2].Rows)
                    {
                        cheques = new TESCheques();
                        cheques.EstadoColeccion = EstadoColecciones.Agregado;
                        Mapeador.SetearEntidadPorfila(dr, cheques, cheques.EstadoColeccion);
                        movValor = mov.CajasMovimientosValores.FirstOrDefault(x => x.TipoValor.IdTipoValor == (int)dr["IdTipoValor"]);
                        if (movValor != null)
                        {
                            if (movValor.TipoValor.IdTipoValor == (int)EnumTiposValores.ChequeTercero)
                            {
                                movValor.Cheques.Add(cheques);
                                cheques.IndiceColeccion = movValor.Cheques.IndexOf(cheques);
                            }
                        }
                    }
                }
            }

            return mov;
        }

        public TESCajasMovimientos ObtenerDatosCompletos(TESCajasMovimientos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESCajasMovimientos>("TESCajasMovimientosSeleccionar", pParametro);
            pParametro.CajasMovimientosConceptosContables = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajasMovimientosConceptosContables>("TESCajasMovimientosConceptosContablesSeleccionar", pParametro);
            pParametro.CajasMovimientosValores = BaseDatos.ObtenerBaseDatos().ObtenerLista<TESCajasMovimientosValores>("TESCajasMovimientosValoresSeleccionar", pParametro);
            return pParametro;
        }

        public TESCajasMovimientos ObtenerMovimientoValoresXML(TESCajasMovimientos pParametro)
        {
            pParametro.LoteCajasMovimientosValores = new XmlDocument();
            //XmlNode docNode = pParametro.LotePrestamos.CreateXmlDeclaration("1.0", "UTF-8", null);
            //pParametro.LotePrestamos.AppendChild(docNode);

            XmlNode cajasMovimientosValores = pParametro.LoteCajasMovimientosValores.CreateElement("CajasMovimientosValores");
            pParametro.LoteCajasMovimientosValores.AppendChild(cajasMovimientosValores);

            XmlNode cajaMovimientoValor;
            XmlAttribute attribute;
            XmlNode camposValores;
            foreach (TESCajasMovimientosValores movValores in pParametro.CajasMovimientosValores)
            {
                switch (movValores.TipoValor.IdTipoValor)
                {
                    case (int)EnumTiposValores.AfipTiposPercepciones:
                    case (int)EnumTiposValores.AfipTiposRetenciones:
                    case (int)EnumTiposValores.Efectivo:
                        #region Efectivo
                        cajaMovimientoValor = pParametro.LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Importe");
                        attribute.Value = movValores.Importe.ToString().Replace(',','.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    case (int)EnumTiposValores.Cargos:
                        break;
                    case (int)EnumTiposValores.Cheque:
                        #region Cheque
                        foreach (TESCheques cheque in movValores.Cheques)
                        {
                            cajaMovimientoValor = pParametro.LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                            attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = cheque.Importe.ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdBancoCuenta");
                            attribute.Value = cheque.IdBancoCuenta.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Fecha");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(cheque.Fecha);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("FechaDiferido");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(cheque.FechaDiferido.Value);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("NumeroCheque");
                            attribute.Value = cheque.NumeroCheque;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        }
                        #endregion
                        break;
                    case (int)EnumTiposValores.ChequeTercero:
                        #region Cheque Terceros
                        foreach (TESCheques cheque in movValores.Cheques)
                        {
                            cajaMovimientoValor = pParametro.LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                            attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = cheque.Importe.ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdBanco");
                            attribute.Value = cheque.Banco.IdBanco.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Fecha");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(cheque.Fecha);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("FechaDiferido");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(cheque.FechaDiferido.Value);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdCheque");
                            attribute.Value = cheque.IdCheque.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("NumeroCheque");
                            attribute.Value = cheque.NumeroCheque;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("TitularCheque");
                            attribute.Value = cheque.TitularCheque;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("CUIT");
                            attribute.Value = cheque.CUIT;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Concepto");
                            attribute.Value = cheque.Concepto;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        }
                        #endregion
                        break;
                    case (int)EnumTiposValores.Prestamos:
                        break;
                    case (int)EnumTiposValores.SinMovimientosFondos:
                        break;
                    case (int)EnumTiposValores.TarjetaCredito:
                        #region TarjetaCredito
                        /*Para Ordenes de COBROS */
                        foreach (TESTarjetasTransacciones tarjeta in movValores.TarjetasTransacciones)
                        {
                            cajaMovimientoValor = pParametro.LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                            attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = tarjeta.Importe.ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("FechaTransaccion");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(tarjeta.FechaTransaccion);
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdTarjetaCredito");
                            attribute.Value = tarjeta.IdTarjetaCredito.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Titular");
                            attribute.Value = tarjeta.Titular.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("NumeroTarjetaCredito");
                            attribute.Value = tarjeta.NumeroTarjetaCredito;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("VencimientoAnio");
                            attribute.Value = tarjeta.VencimientoAnio.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("VencimientoMes");
                            attribute.Value = tarjeta.VencimientoMes.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("NumeroTransaccionPosnet");
                            attribute.Value = tarjeta.NumeroTransaccionPosnet;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("CantidadCuotas");
                            attribute.Value = tarjeta.CantidadCuotas.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("NumeroLote");
                            attribute.Value = tarjeta.NumeroLote;
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        }
                        /*Para Ordenes de PAGO*/
                        foreach (TESBancosCuentasMovimientos bcoCtaMov in movValores.BancosCuentasMovimientos)
                        {
                            cajaMovimientoValor = pParametro.LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                            attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = bcoCtaMov.Importe.ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdBancoCuenta");
                            attribute.Value = bcoCtaMov.BancoCuenta.IdBancoCuenta.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("CantidadCuotas");
                            attribute.Value = bcoCtaMov.NumeroTipoOperacion.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("ImporteCuota");
                            attribute.Value = (bcoCtaMov.Importe / Convert.ToInt32(bcoCtaMov.NumeroTipoOperacion)).ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdOrdenPagoValor");
                            attribute.Value = bcoCtaMov.IdRefTipoOperacion.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        }
                        #endregion
                        break;
                    case (int)EnumTiposValores.Transferencia:
                    case (int)EnumTiposValores.EfectivoFondoFijo:
                        #region Transferencia / Fondo Fijo
                        foreach (TESBancosCuentasMovimientos bcoCtaMov in movValores.BancosCuentasMovimientos)
                        {
                            cajaMovimientoValor = pParametro.LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                            attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            //cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Importe");
                            attribute.Value = bcoCtaMov.Importe.ToString().Replace(',', '.');
                            cajaMovimientoValor.Attributes.Append(attribute);
                            //cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdBancoCuenta");
                            attribute.Value = bcoCtaMov.BancoCuenta.IdBancoCuenta.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);
                            //cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("FechaConfirmacionBanco");
                            attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(bcoCtaMov.FechaConfirmacionBanco);
                            cajaMovimientoValor.Attributes.Append(attribute);

                            attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("NumeroTipoOperacion");
                            Guid guid= Guid.NewGuid();
                            attribute.Value = guid.ToString();
                            cajaMovimientoValor.Attributes.Append(attribute);

                            if (bcoCtaMov.LoteCamposValores != null)
                            {
                                cajaMovimientoValor.InnerXml = bcoCtaMov.LoteCamposValores.InnerXml;
                            }
                            cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        }
                        #endregion
                        break;
                    case (int)EnumTiposValores.CajaAhorro:
                        #region Cajas de Ahorros
                        cajaMovimientoValor = pParametro.LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("Importe");
                        attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = pParametro.LoteCajasMovimientosValores.CreateAttribute("IdCuenta");
                        attribute.Value = movValores.IdCuenta.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    default:
                        break;
                }

            }
            return pParametro;
        }

        internal bool ConfirmarMovimientoXml(TESCajasMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = false;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TesCajasMovimientosConfirmacionAutomaticaProceso");
                    
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }

                    return resultado;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return resultado;
                }
            }
        }

        internal bool AnularMovimientoIngresosEgreso(TESCajasMovimientos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = false;
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.IngresosCajas
                    //    || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.IngresosCajasInternos)
                    //    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TesCajasMovimientosAnularIngreso");
                    //else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.EgresosCajas
                    //    || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.EgresosCajasInternos)
                    //    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TesCajasMovimientosAnularEgreso");
                    //else
                    //{
                    //resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TESCajasMovimientosAnulacionProceso");
                    //}
                    /* Se comentó todo porque se unificó todo en un solo sp. */
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "TESCajasMovimientosAnulacionProceso");
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                    }

                    return resultado;
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return resultado;
                }
            }
        }

        public bool CircuitoDiarioCajasAutomatico(TESCajasMovimientos pParametro)
        {
            

            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "TesCajasMovimientosCircuitoDiarioCajasAutomatico");
          
        }
    }
}