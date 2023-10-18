using Auditoria;
using Auditoria.Entidades;
using Cargos;
using Cargos.Entidades;
using Cobros.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Contabilidad;
using Contabilidad.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Prestamos;
using Prestamos.Entidades;
using Reportes.FachadaNegocio;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Xml;

namespace Cobros.LogicaNegocio
{
    class CobOrdenesCobrosLN : BaseLN<CobOrdenesCobros>
    {
        private byte[] ObtenerSelloTiempo(CobOrdenesCobros pParametro, Database db, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("CobOrdenesCobrosSeleccionar", pParametro, db, tran);
        }

        public override CobOrdenesCobros ObtenerDatosCompletos(CobOrdenesCobros pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CobOrdenesCobros>("CobOrdenesCobrosSeleccionar", pParametro);
            pParametro.OrdenesCobrosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobrosDetalles>("CobOrdenesCobrosDetalllesSeleccionrPorOrdenesCobros", pParametro);
            pParametro.OrdenesCobrosValores = BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobrosValores>("CobOrdenesCobrosValoresSeleccionarPorOrdenCobro", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            return pParametro;
        }

        public XmlDocument ObtenerMovimientoValoresXML(CobOrdenesCobros pParametro)
        {
            XmlDocument LoteCajasMovimientosValores = new XmlDocument();
            //XmlNode docNode = pParametro.LotePrestamos.CreateXmlDeclaration("1.0", "UTF-8", null);
            //pParametro.LotePrestamos.AppendChild(docNode);

            XmlNode cajasMovimientosValores = LoteCajasMovimientosValores.CreateElement("CajasMovimientosValores");
            LoteCajasMovimientosValores.AppendChild(cajasMovimientosValores);

            XmlNode cajaMovimientoValor;
            XmlAttribute attribute;
            foreach (CobOrdenesCobrosValores movValores in pParametro.OrdenesCobrosValores)
            {
                switch (movValores.TipoValor.IdTipoValor)
                {
                    case (int)EnumTiposValores.AfipTiposPercepciones:
                    case (int)EnumTiposValores.AfipTiposRetenciones:
                    case (int)EnumTiposValores.Efectivo:
                        #region Efectivo
                        cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                        attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    case (int)EnumTiposValores.Cargos:
                        break;
                    case (int)EnumTiposValores.Cheque:
                        #region Cheque
                        //cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        //attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        //attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        //cajaMovimientoValor.Attributes.Append(attribute);
                        //cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        //attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                        //attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                        //cajaMovimientoValor.Attributes.Append(attribute);
                        //cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        //attribute = LoteCajasMovimientosValores.CreateAttribute("IdBancoCuenta");
                        //attribute.Value = movValores.BancoCuenta.IdBancoCuenta.ToString();
                        //cajaMovimientoValor.Attributes.Append(attribute);
                        //cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        //attribute = LoteCajasMovimientosValores.CreateAttribute("Fecha");
                        //attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.Fecha.Value);
                        //cajaMovimientoValor.Attributes.Append(attribute);
                        //cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        //attribute = LoteCajasMovimientosValores.CreateAttribute("FechaDiferido");
                        //attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.FechaDiferido.Value);
                        //cajaMovimientoValor.Attributes.Append(attribute);
                        //cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        //attribute = LoteCajasMovimientosValores.CreateAttribute("NumeroCheque");
                        //attribute.Value = movValores.NumeroCheque;
                        //cajaMovimientoValor.Attributes.Append(attribute);
                        //cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    case (int)EnumTiposValores.ChequeTercero:
                        #region Cheque Terceros
                        cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                        attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdBanco");
                        attribute.Value = movValores.Cheque.Banco.IdBanco.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Fecha");
                        attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.Fecha.Value);
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("FechaDiferido");
                        attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.FechaDiferido.Value);
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("NumeroCheque");
                        attribute.Value = movValores.NumeroCheque;
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("TitularCheque");
                        attribute.Value = movValores.Cheque.TitularCheque;
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("CUIT");
                        attribute.Value = movValores.Cheque.CUIT;
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Concepto");
                        attribute.Value = movValores.Cheque.Concepto;
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    case (int)EnumTiposValores.Prestamos:
                        break;
                    case (int)EnumTiposValores.SinMovimientosFondos:
                        break;
                    case (int)EnumTiposValores.TarjetaCredito:
                        #region TarjetaCredito

                        cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                        attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("FechaTransaccion");
                        attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.Fecha.Value);
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdTarjetaCredito");
                        attribute.Value = movValores.IdTarjetaCredito.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Titular");
                        attribute.Value = movValores.Titular.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("NumeroTarjetaCredito");
                        attribute.Value = movValores.NumeroTarjetaCredito;
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("VencimientoAnio");
                        attribute.Value = movValores.VencimientoAnio.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("VencimientoMes");
                        attribute.Value = movValores.VencimientoMes.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("NumeroTransaccionPosnet");
                        attribute.Value = movValores.NumeroTransaccionPosnet;
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("CantidadCuotas");
                        attribute.Value = movValores.CantidadCuotas.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("NumeroLote");
                        attribute.Value = movValores.NumeroLote;
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        #endregion
                        break;
                    case (int)EnumTiposValores.Transferencia:
                    case (int)EnumTiposValores.EfectivoFondoFijo:
                        #region Transferencia
                        cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                        attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdBancoCuenta");
                        attribute.Value = movValores.BancoCuenta.IdBancoCuenta.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("FechaConfirmacionBanco");
                        attribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(movValores.Fecha.Value);
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);
                        #endregion
                        break;
                    case (int)EnumTiposValores.CajaAhorro:
                        #region Caja de Ahorros
                        cajaMovimientoValor = LoteCajasMovimientosValores.CreateElement("CajaMovimientoValor");

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdTipoValor");
                        attribute.Value = movValores.TipoValor.IdTipoValor.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("Importe");
                        attribute.Value = movValores.Importe.ToString().Replace(',', '.');
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        attribute = LoteCajasMovimientosValores.CreateAttribute("IdCuenta");
                        attribute.Value = movValores.IdCuenta.ToString();
                        cajaMovimientoValor.Attributes.Append(attribute);
                        cajasMovimientosValores.AppendChild(cajaMovimientoValor);

                        #endregion
                        break;
                    default:
                        break;
                }

            }
            return LoteCajasMovimientosValores;
        }

        public override List<CobOrdenesCobros> ObtenerListaFiltro(CobOrdenesCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobros>("CobOrdenesCobrosSeleccionarFiltro", pParametro);
        }
        public DataTable ObtenerListaFiltroDT(CobOrdenesCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CobOrdenesCobrosSeleccionarFiltro", pParametro);
        }
        public override bool Agregar(CobOrdenesCobros pParametro)
        {
            return this.Agregar(pParametro, null);
        }

        public bool Agregar(CobOrdenesCobros pParametro, CarTiposCargosAfiliadosFormasCobros pCargoAfiliado)
        {
            //Control por Duplicacion. Si tengo el Id es porque ya se grabo
            //Si hay error en el Finally del try/catch bloq lo pongo en 0
            if (pParametro.IdOrdenCobro > 0)
                return true;
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            bool confirmaOCxCA = false;

            if (!this.Validar(pParametro))
                return false;

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Activo;
            //pParametro.FechaEmision = DateTime.Now;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;

            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaCreditoCargos)
            {
                pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Cobrado;
                pParametro.FechaConfirmacion = pParametro.FechaConfirmacion.HasValue ? pParametro.FechaConfirmacion : pParametro.FechaEmision;
                pParametro.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas)
            {
                if (pParametro.CargoDescuentoAfiliado || pParametro.ImporteTotal == 0)
                {
                    pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Cobrado;
                    pParametro.FechaConfirmacion = pParametro.FechaConfirmacion.HasValue ? pParametro.FechaConfirmacion : pParametro.FechaEmision;
                    pParametro.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
                }

                //Si tiene prestamo armo la cuponera de acuerdo a la Fecha Periodo Inicio
                else if (pParametro.Prestamo.EstadoColeccion == EstadoColecciones.Agregado)
                {
                    //Confirmo la Orden de Cobro
                    pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Cobrado;
                    pParametro.FechaConfirmacion = pParametro.FechaConfirmacion.HasValue ? pParametro.FechaConfirmacion : pParametro.FechaEmision;
                    pParametro.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuarioEvento;

                    AyudaProgramacionLN.MatchObjectProperties(pParametro.UsuarioLogueado, pParametro.Prestamo.UsuarioLogueado);
                    pParametro.Prestamo.UsuarioPreAutorizar.IdUsuarioPreAutorizar = pParametro.UsuarioLogueado.IdUsuario;
                    pParametro.Prestamo.UsuarioAutorizar.IdUsuarioAutorizar = pParametro.UsuarioLogueado.IdUsuario;
                    pParametro.Prestamo.FilialPago.IdFilialPago = pParametro.FilialCobro.IdFilialCobro;
                    pParametro.Prestamo.FechaPreAutorizado = pParametro.FechaConfirmacion.Value;
                    pParametro.Prestamo.FechaPrestamo = pParametro.FechaConfirmacion.Value;
                    pParametro.Prestamo.FechaAutorizado = pParametro.FechaConfirmacion.Value;
                    pParametro.Prestamo.FechaValidezAutorizado = pParametro.FechaConfirmacion.Value;

                    pParametro.Prestamo.PrestamosCuotas.Clear();
                    if (!PrePrestamosF.PrestamosAgregarPrevio(pParametro.Prestamo))
                    {
                        return false;
                    }
                    pParametro.Prestamo.EstadoColeccion = EstadoColecciones.Agregado;
                    //Confirmo el Prestamo
                    pParametro.Prestamo.FechaConfirmacion = pParametro.FechaConfirmacion.Value;
                    pParametro.Prestamo.Estado.IdEstado = (int)EstadosPrestamos.Confirmado;
                    pParametro.Prestamo.UsuarioConfirmacion.IdUsuarioConfirmacion = pParametro.IdUsuarioConfirmacion.Value;
                }
            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados
                && pParametro.OrdenesCobrosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
                && pParametro.OrdenesCobrosValores.Count == pParametro.OrdenesCobrosValores.Count(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.CajaAhorro)
                && !pParametro.ModuloTesoreriaCajas
                )
            {
                confirmaOCxCA = true;
                pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Cobrado;
                pParametro.FechaConfirmacion = pParametro.FechaConfirmacion.HasValue ? pParametro.FechaConfirmacion : pParametro.FechaEmision;
                pParametro.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
            }

            //Cargo el XML
            pParametro.CargarLoteOrdenesCobrosDetallesDetalles();
            pParametro.CargarLoteOrdenesCobrosValores();

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CobOrdenesCobrosValidaciones"))
                    {
                        resultado = false;
                    }
                    //Guardo la Orden de Cobro
                    if (resultado)
                    {
                        pParametro.IdOrdenCobro = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CobOrdenesCobrosInsertar");
                        if (pParametro.IdOrdenCobro == 0)
                            resultado = false;
                    }

                    if (resultado && !this.ActualizarDetalle(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarRemitos(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    //Si es nuna Nota de Credito NO va a la Caja y se actualiza la Cuenta Corriente
                    if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.NotaCreditoCargos
                        || confirmaOCxCA)
                    {
                        if (resultado && !this.ActualizarCuentaCorriente(pParametro, bd, tran))
                            resultado = false;


                        //hago la extracción de ahorro
                        if (confirmaOCxCA)
                            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "AhoCuentasMovimientosExtraccionOrdenCobro"))
                                resultado = false;

                        if (resultado && !new InterfazContableLN().NotaCreditoCargos(pParametro, new List<InterfazValoresImportes>(), bd, tran))
                            resultado = false;

                    }
                    else
                    //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                    //    || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas)
                    {
                        //Si tiene Descuento por Cargos Genero los Cargos a Descontar
                        #region Cargos del Socio Ordenes Cobros
                        if (resultado && pParametro.CargoDescuentoAfiliado)
                        {
                            pCargoAfiliado.IdReferenciaRegistro = pParametro.IdOrdenCobro;
                            pCargoAfiliado.TablaReferenciaRegistro = pParametro.GetType().Name;
                            pCargoAfiliado.Detalle = pParametro.Detalle;

                            if (!CargosF.TiposCargosAfiliadosAgregar(pCargoAfiliado, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(pCargoAfiliado, pParametro);
                                resultado = false;
                            }
                        }
                        #endregion


                        if (resultado && !new CobOrdenesCobrosTiposRetencionesLN().AgregarOrdenesCobrosTiposRetenciones(pParametro, bd, tran))
                            resultado = false;

                        if (resultado && !ActualizarAnticipos(pParametro, bd, tran))
                            resultado = false;

                        if (resultado && !ActualizarPrestamo(pParametro, bd, tran))
                            resultado = false;

                        if (pParametro.CargoDescuentoAfiliado || pParametro.ImporteTotal == 0)
                        {
                            //Contabilizo la Orden de Cobro con el Cargo
                            CobOrdenesCobros clon = AyudaProgramacionLN.Clone(pParametro);
                            clon.OrdenesCobrosDetalles = clon.OrdenesCobrosDetalles.FindAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);
                            clon.OrdenesCobrosTiposRetenciones = clon.OrdenesCobrosTiposRetenciones.FindAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);
                            clon.OrdenesCobrosAnticipos = clon.OrdenesCobrosAnticipos.Where(x => x.IncluirEnOC).ToList();
                            clon.OrdenesCobrosAnticipos.ForEach(x => x.ImporteAplicado = x.ImporteAplicar);
                            if (resultado && !this.ActualizarDetalleFactura(clon, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(clon, pParametro);
                                resultado = false;
                            }

                            if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPDevuelveParametros(clon, bd, tran, "CobOrdenesCobrosActualizarCargosFacturasProceso"))
                            {
                                clon.CodigoMensaje = clon.Mensaje;
                                AyudaProgramacionLN.MapearError(clon, pParametro);
                                resultado = false;
                            }

                            if (resultado &&
                                pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                                && !new InterfazContableLN().OrdenCobroFacturas(clon, new List<InterfazValoresImportes>(), bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(clon, pParametro);
                                resultado = false;
                            }
                        }
                        else if (pParametro.Prestamo.EstadoColeccion == EstadoColecciones.Agregado)
                        {
                            //Contabilizo la Orden de Cobro con el Prestamo
                            CobOrdenesCobros clon = AyudaProgramacionLN.Clone(pParametro);
                            clon.OrdenesCobrosDetalles = clon.OrdenesCobrosDetalles.FindAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);
                            clon.OrdenesCobrosTiposRetenciones = clon.OrdenesCobrosTiposRetenciones.FindAll(x => x.EstadoColeccion == EstadoColecciones.Agregado);
                            clon.OrdenesCobrosAnticipos = clon.OrdenesCobrosAnticipos.Where(x => x.IncluirEnOC).ToList();
                            if (resultado && !this.ActualizarDetalleFactura(clon, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(clon, pParametro);
                                resultado = false;
                            }

                            if (resultado &&
                                pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                                && !new InterfazContableLN().OrdenCobroFacturasPrestamos(clon, new List<InterfazValoresImportes>(), bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(clon, pParametro);
                                resultado = false;
                            }
                        }
                        else
                        {


                            bool bvalor = false;
                            OperacionConfirmar paramValor = new OperacionConfirmar();
                            paramValor.IdRefTipoOperacion = pParametro.IdOrdenCobro;
                            paramValor.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                            paramValor.UsuarioLogueado = pParametro.UsuarioLogueado;
                            //TGEParametrosValores valor;
                            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
                            || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas)
                            {
                                //valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CircuitoDiarioCajasAutomatico, bd, tran);
                                //bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;

                                bvalor = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(paramValor, bd, tran, "TesCajasMovimientosCircuitoDiarioCajasAutomatico");

                            }
                            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados)
                            {
                                //valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CircuitoDiarioCajasAutomaticoOrdenesCobrosAfiliados, bd, tran);
                                //bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
                                bvalor = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(paramValor, bd, tran, "TesCajasMovimientosCircuitoDiarioCajasAutomatico");

                            }

                            if (resultado && bvalor)
                            {
                                OperacionConfirmar opConfirmar = new OperacionConfirmar();
                                opConfirmar.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                                opConfirmar.IdRefTipoOperacion = pParametro.IdOrdenCobro;
                                opConfirmar.Fecha = pParametro.FechaConfirmacion.Value;
                                opConfirmar.IdFilial = pParametro.FilialCobro.IdFilialCobro;
                                opConfirmar.Importe = pParametro.ImporteTotal;
                                opConfirmar.Descripcion = pParametro.Detalle;
                                opConfirmar.UsuarioLogueado = pParametro.UsuarioLogueado;
                                opConfirmar.LoteCajasMovimientosValores = this.ObtenerMovimientoValoresXML(pParametro);
                                opConfirmar.SelloTiempo = this.ObtenerSelloTiempo(pParametro, bd, tran);
                                resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(opConfirmar, bd, tran, "TesCajasMovimientosConfirmacionAutomaticaProceso");
                                if (!resultado)
                                {
                                    resultado = false;
                                    AyudaProgramacionLN.MapearError(opConfirmar, pParametro);
                                }
                                else
                                {
                                    pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Cobrado;
                                }
                            }
                        }
                    }


                    pParametro.SelloTiempo = this.ObtenerSelloTiempo(pParametro, bd, tran);

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
                    pParametro.CodigoMensaje = ex.Message;
                    resultado = false;
                }
                finally
                {
                    if (!resultado)
                        pParametro.IdOrdenCobro = 0;
                }
            }

            if (resultado)
            {
                try
                {
                    BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, "CobOrdenesCobrosAgregarActualizarFinalizado");
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                }
                finally
                {
                }
            }
            return resultado;
        }

        private bool ActualizarPrestamo(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        {
            CobOrdenesCobrosPrestamos relacion;
            switch (pParametro.Prestamo.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    if (!PrePrestamosF.PrestamosAgregar(pParametro.Prestamo, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(pParametro.Prestamo, pParametro);
                        return false;
                    }
                    //ACA TAMBIEN AGREGO LA RELACION PRESTAMO/ORDEN en la tabla de relacion
                    relacion = new CobOrdenesCobrosPrestamos();
                    relacion.IdOrdenCobro = pParametro.IdOrdenCobro;
                    relacion.IdPrestamo = pParametro.Prestamo.IdPrestamo;
                    relacion.IdOrdenCobroPrestamo = BaseDatos.ObtenerBaseDatos().Agregar(relacion, bd, tran, "CobOrdenesCobrosPrestamosInsertar");
                    if (relacion.IdOrdenCobroPrestamo == 0)
                    {
                        AyudaProgramacionLN.MapearError(relacion, pParametro);
                        return false;
                    }

                    ///*REGENERO EL COMPROBANTE DE REMITOS (POR AMDESS CUOTAS E IMPORTE CUOTA */
                    //VTARemitos remito = BaseDatos.ObtenerBaseDatos().Obtener<VTARemitos>("VTARemitosSeleccionarPorOrdenCobro", pParametro, bd, tran);
                    //if (remito.IdRemito > 0 && !FacturasF.RemitosGeneroGuardoPDF(remito, bd, tran))
                    //{
                    //    AyudaProgramacionLN.MapearError(remito, pParametro);
                    //    return false;
                    //}

                    break;
                case EstadoColecciones.Borrado:

                    if (!PrePrestamosF.PrestamosAnular(pParametro.Prestamo, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(pParametro.Prestamo, pParametro);
                        return false;
                    }

                    relacion = new CobOrdenesCobrosPrestamos();
                    relacion.IdOrdenCobro = pParametro.IdOrdenCobro;
                    relacion.IdPrestamo = pParametro.Prestamo.IdPrestamo;
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(relacion, bd, tran, "CobOrdenesCobrosPrestamosBorrarPorIdOrdenIdPrestamo"))
                    {
                        AyudaProgramacionLN.MapearError(relacion, pParametro);
                        return false;
                    }
                    break;
            }
            return true;
        }

        public bool Anular(CobOrdenesCobros pParametro)
        {
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Baja;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo la Orden de Cobro
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CobOrdenesCobrosActualizar");

                    if (resultado && !ActualizarPrestamo(pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }

            return resultado;
        }

        public bool AnularAfiliadoCobrada(CobOrdenesCobros pParametro)
        {
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Baja;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CobOrdenesCobros valorViejo = new CobOrdenesCobros();
            valorViejo.IdOrdenCobro = pParametro.IdOrdenCobro;
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
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CobOrdenesCobrosValidacionesAnular"))
                    {
                        resultado = false;
                    }
                    if (resultado)
                    {
                        resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CobOrdenesCobrosAnulacionProceso");
                    }
                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }

            return resultado;
        }

        public bool AnularCobrada(CobOrdenesCobros pParametro)
        {
            bool resultado = true;

            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Baja;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CobOrdenesCobros valorViejo = new CobOrdenesCobros();
            valorViejo.IdOrdenCobro = pParametro.IdOrdenCobro;
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
                    //Guardo la Orden de Cobro
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CobOrdenesCobrosAnularCobradaProceso");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }

            return resultado;
        }

        //public bool Agregar(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        //{
        //    //bool resultado = true;

        //    if (!this.Validar(pParametro))
        //        return false;

        //    pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Activo;
        //    pParametro.FechaEmision = DateTime.Now;

        //    //Guardo la Orden de Cobro
        //    pParametro.IdOrdenCobro = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CobOrdenesCobrosInsertar");
        //    if (pParametro.IdOrdenCobro == 0)
        //        return false;

        //    //Guardo el detalle de la Orden de Cobro
        //    foreach (CobOrdenesCobrosDetalles detalle in pParametro.OrdenesCobrosDetalles)
        //    {
        //        detalle.IdOrdenCobro = pParametro.IdOrdenCobro;
        //        detalle.IdOrdenCobroDetalle = BaseDatos.ObtenerBaseDatos().Agregar(detalle, bd, tran, "CobOrdenesCobrosDetallesInsertar");
        //        if (detalle.IdOrdenCobroDetalle == 0)
        //        {
        //            AyudaProgramacionLN.MapearError(detalle, pParametro); 
        //            return false;
        //        }

        //        if (detalle.CuentaCorriente.IdCuentaCorriente > 0)
        //        {
        //            CarCuentasCorrientes cobroCtaCte = new CarCuentasCorrientes();
        //            cobroCtaCte.IdRefCuentaCorriente = detalle.CuentaCorriente.IdCuentaCorriente;
        //            cobroCtaCte.FechaMovimiento = DateTime.Now;
        //            cobroCtaCte.Estado.IdEstado = (int)EstadosCuentasCorrientes.Cobrado;
        //            cobroCtaCte.IdAfiliado = pParametro.Afiliado.IdAfiliado;
        //            cobroCtaCte.IdRefTipoOperacion = pParametro.IdOrdenCobro;
        //            cobroCtaCte.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobros;
        //            //cobroCtaCte.TipoCargo.TipoOperacion.IdTipoOperacion = 
        //            cobroCtaCte.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Credito;
        //            cobroCtaCte.Importe = detalle.CuentaCorriente.Importe;
        //            //Por ahora se deja Efectivo
        //            cobroCtaCte.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
        //            cobroCtaCte.IdAfiliado = pParametro.Afiliado.IdAfiliado;

        //            if (!CargosF.CuentasCorrientesAgregar(cobroCtaCte, bd, tran))
        //            {
        //                AyudaProgramacionLN.MapearError(cobroCtaCte, pParametro);
        //                return false;
        //            }

        //            detalle.CuentaCorriente.Estado.IdEstado = (int)EstadosCuentasCorrientes.Cobrado;
        //            if (!CargosF.CuentasCorrientesActualizar(detalle.CuentaCorriente, bd, tran))
        //            {
        //                AyudaProgramacionLN.MapearError(detalle.CuentaCorriente, pParametro);
        //                return false;
        //            }
        //        }
        //    }

        //    ////Guardo el detalle de las Formas de Cobro
        //    //foreach (CobOrdenesCobrosFormasCobros detalle in pParametro.OrdenesCobrosFormasCobros)
        //    //{
        //    //    detalle.IdOrdenCobro = pParametro.IdOrdenCobro;
        //    //    detalle.IdOrdenCobroFormaCobro = BaseDatos.ObtenerBaseDatos().Agregar(detalle, bd, tran, "CobOrdenesCobrosFormasCobrosInsertar");
        //    //    if (detalle.IdOrdenCobroFormaCobro == 0)
        //    //    {
        //    //        AyudaProgramacionLN.MapearError(detalle, pParametro);
        //    //        return false;
        //    //    }
        //    //    //if (detalle.TipoValor.IdTipoValor == (int)EnumTiposValores.Cheque)
        //    //    //{
        //    //    //    //Guardeo el detalle de los Cheques Ingresados
        //    //    //    foreach (TESCheques cheque in detalle.Cheques)
        //    //    //    {
        //    //    //        cheque.IdOrdenCobroFormaCobro = detalle.IdOrdenCobroFormaCobro;
        //    //    //        cheque.Estado.IdEstado = (int)EstadosCheques.IngresoCheque;
        //    //    //        //Guardo los Cheques
        //    //    //        if (!BancosF.ChequesAgregar(cheque, bd, tran))
        //    //    //        {
        //    //    //            AyudaProgramacionLN.MapearError(cheque, pParametro);
        //    //    //            return false;
        //    //    //        }
        //    //    //    }
        //    //    //}
        //    //}

        //    return true;
        //}

        /// <summary>
        /// Modifica el Tipo de Operacion de una Orden de Cobro (Factura) y Genera o Revierte el Asiento Contable
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public override bool Modificar(CobOrdenesCobros pParametro)
        {
            bool resultado = true;

            CobOrdenesCobros valorViejo = new CobOrdenesCobros();
            valorViejo.IdOrdenCobro = pParametro.IdOrdenCobro;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            ///Si no se modifico la Operacion devuelvo TRUE.
            ///No hay que guardar nada.
            ///No descomentar porque si no se genera de nuevo el asiento contable!
            if (pParametro.TipoOperacion.IdTipoOperacion == valorViejo.TipoOperacion.IdTipoOperacion)
                return true;

            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo la Orden de Cobro
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CobOrdenesCobrosActualizarTipoOperacion");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas)
                    {

                        if (resultado && !new InterfazContableLN().OrdenCobroFacturas(pParametro, this.ObtenerValoresImporte(pParametro), bd, tran))
                            resultado = false;
                    }
                    else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas)
                    {
                        TGETiposOperaciones tipoOpeAnular = new TGETiposOperaciones();
                        tipoOpeAnular.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas;

                        CtbAsientosContables anular = new CtbAsientosContables();
                        anular.IdRefTipoOperacion = pParametro.IdOrdenCobro;
                        anular.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas;
                        anular.UsuarioLogueado = pParametro.UsuarioLogueado;

                        if (!ContabilidadF.AsientosContablesRevertirAsiento(anular, tipoOpeAnular, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(anular, pParametro);
                            return false;
                        }

                        if (!ContabilidadF.AsientosContablesAgregar(anular, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(anular, pParametro);
                            return false;
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }

            return resultado;
        }

        public bool Confirmar(CobOrdenesCobros pParametro, DateTime pFecha, List<InterfazValoresImportes> pValoresImportes, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            //if (!this.Validar(pParametro))
            //    return false;

            pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Cobrado;
            pParametro.IdUsuarioConfirmacion = pParametro.UsuarioLogueado.IdUsuario;
            pParametro.FechaConfirmacion = pFecha;// DateTime.Now;

            bool contabilizarOrdenCobroFactura = pParametro.OrdenesCobrosDetalles.Exists(
                x => x.Factura.TipoFactura.Contabilizar) ? true : false;

            if (contabilizarOrdenCobroFactura)
                pParametro.NumeroOrdenCobro = BaseDatos.ObtenerBaseDatos().Obtener<CobOrdenesCobros>("CobOrdenesCobrosSeleccionarProximoNumero", new Objeto(), bd, tran).NumeroOrdenCobro;

            //Guardo la Orden de Cobro
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CobOrdenesCobrosActualizar"))
                resultado = false;

            //if (resultado && !this.ActualizarDetalle(pParametro, bd, tran))
            //    resultado = false;

            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados)
            {
                if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CobOrdenesCobrosCuentasCorrientesCargosAfiliadosValidar"))
                    resultado = false;

                if (resultado && !this.ActualizarCuentaCorriente(pParametro, bd, tran))
                    resultado = false;

                if (resultado && !new InterfazContableLN().OrdenCobroAfiliado(pParametro, pValoresImportes, bd, tran))
                    resultado = false;
            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.CancelacionAnticipadaCuotaPrestamo)
            {
                if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "PrePrestamosCuotasActualizarCancelacionAnticipadaValidar"))
                    resultado = false;

                if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "PrePrestamosCuotasActualizarCancelacionAnticipada"))
                    resultado = false;

                if (resultado && !new InterfazContableLN().CancelacionAnticipadaCuotaPrestamo(pParametro, pValoresImportes, bd, tran))
                    resultado = false;
            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosVarios)
            {
                if (resultado && !new InterfazContableLN().OrdenCobroVarias(pParametro, pValoresImportes, bd, tran))
                    resultado = false;
            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas)
            {
                if (resultado && !this.ActualizarDetalleFactura(pParametro, bd, tran))
                    resultado = false;

                if (resultado && contabilizarOrdenCobroFactura && !new InterfazContableLN().OrdenCobroFacturas(pParametro, pValoresImportes, bd, tran))
                    resultado = false;
            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas)
            {
                if (resultado && !this.ActualizarDetalleFactura(pParametro, bd, tran))
                    resultado = false;
            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasAdelantos)
            {
                if (resultado && contabilizarOrdenCobroFactura && !new InterfazContableLN().OrdenCobroFacturasAdelanto(pParametro, pValoresImportes, bd, tran))
                    resultado = false;
            }

            return resultado;
        }

        private bool ActualizarDetalle(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        {
            //Guardo el detalle de la Orden de Cobro
            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosVarios)
            {
                #region Orden Cobro Varios
                foreach (CobOrdenesCobrosDetalles detalle in pParametro.OrdenesCobrosDetalles)
                {
                    switch (detalle.EstadoColeccion)
                    {
                        case EstadoColecciones.Agregado:
                            detalle.Estado.IdEstado = (int)EstadosOrdenesCobro.Activo;
                            detalle.IdOrdenCobro = pParametro.IdOrdenCobro;
                            detalle.IdOrdenCobroDetalle = BaseDatos.ObtenerBaseDatos().Agregar(detalle, bd, tran, "CobOrdenesCobrosDetallesInsertar");
                            if (detalle.IdOrdenCobroDetalle == 0)
                            {
                                AyudaProgramacionLN.MapearError(detalle, pParametro);
                                return false;
                            }
                            break;
                        case EstadoColecciones.Modificado:
                            //if(!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, bd, tran, "CobOrdenesCobrosDetallesActualizar"))
                            //{
                            //    AyudaProgramacionLN.MapearError(detalle, pParametro);
                            //    return false;
                            //}
                            break;
                        default:
                            break;
                    }

                }
                #endregion
            }
            else
            {
                #region Orden Cobro Facturas
                foreach (CobOrdenesCobrosDetalles detalle in pParametro.OrdenesCobrosDetalles.Where(x => x.IncluirEnOP && x.Importe != 0)) //!=0 porque hay Facturas y Notas de Creditos
                {
                    switch (detalle.EstadoColeccion)
                    {
                        case EstadoColecciones.Agregado:
                            detalle.Estado.IdEstado = (int)EstadosOrdenesCobro.Activo;
                            detalle.IdOrdenCobro = pParametro.IdOrdenCobro;
                            detalle.IdOrdenCobroDetalle = BaseDatos.ObtenerBaseDatos().Agregar(detalle, bd, tran, "CobOrdenesCobrosDetallesInsertar");
                            if (detalle.IdOrdenCobroDetalle == 0)
                            {
                                AyudaProgramacionLN.MapearError(detalle, pParametro);
                                return false;
                            }
                            break;
                        case EstadoColecciones.Modificado:
                            //if(!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, bd, tran, "CobOrdenesCobrosDetallesActualizar"))
                            //{
                            //    AyudaProgramacionLN.MapearError(detalle, pParametro);
                            //    return false;
                            //}
                            break;
                        default:
                            break;
                    }

                }
                #endregion
            }
            return true;
        }

        private bool ActualizarValores(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        {
            foreach (CobOrdenesCobrosValores valor in pParametro.OrdenesCobrosValores)
            {
                switch (valor.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        valor.Estado.IdEstado = (int)EstadosOrdenesCobro.Activo;
                        valor.IdOrdenCobro = pParametro.IdOrdenCobro;
                        valor.IdOrdenCobroValor = BaseDatos.ObtenerBaseDatos().Agregar(valor, bd, tran, "CobOrdenesCobrosValoresInsertar");
                        if (valor.IdOrdenCobroValor == 0)
                        {
                            AyudaProgramacionLN.MapearError(valor, pParametro);
                            return false;
                        }

                        if (!TGEGeneralesF.CamposActualizarCamposValores(valor, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(valor, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        //if(!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, bd, tran, "CobOrdenesCobrosDetallesActualizar"))
                        //{
                        //    AyudaProgramacionLN.MapearError(detalle, pParametro);
                        //    return false;
                        //}
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool ActualizarAnticipos(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        {
            if (pParametro.OrdenesCobrosAnticipos.Count > 0)
            {
                CobOrdenesCobrosFacturasAnticipos cobroFacturaAnticpo;
                foreach (CobOrdenesCobros anticipo in pParametro.OrdenesCobrosAnticipos)
                {
                    switch (anticipo.EstadoColeccion)
                    {
                        case EstadoColecciones.Agregado:
                            cobroFacturaAnticpo = new CobOrdenesCobrosFacturasAnticipos();
                            cobroFacturaAnticpo.IdOrdenCobroFactura = pParametro.IdOrdenCobro;
                            cobroFacturaAnticpo.IdOrdenCobroAnticipo = anticipo.IdOrdenCobro;
                            cobroFacturaAnticpo.Importe = anticipo.ImporteAplicar;
                            cobroFacturaAnticpo.IdOrdenCobroFacturaAnticipo = BaseDatos.ObtenerBaseDatos().Agregar(cobroFacturaAnticpo, bd, tran, "CobOrdenesCobrosFacturasAnticiposInsertar");
                            if (cobroFacturaAnticpo.IdOrdenCobroFacturaAnticipo == 0)
                            {
                                AyudaProgramacionLN.MapearError(cobroFacturaAnticpo, pParametro);
                                return false;
                            }
                            break;
                        case EstadoColecciones.Modificado:
                            break;
                        default:
                            break;
                    }
                }
            }
            return true;
        }

        private bool ActualizarRemitos(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        {
            CobOrdenesCobrosRemitos cobroRemito;
            foreach (VTARemitos valor in pParametro.Remitos)
            {
                switch (valor.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        cobroRemito = new CobOrdenesCobrosRemitos();
                        cobroRemito.IdOrdenCobro = pParametro.IdOrdenCobro;
                        cobroRemito.IdRemito = valor.IdRemito;
                        cobroRemito.IdOrdenCobroRemtio = BaseDatos.ObtenerBaseDatos().Agregar(cobroRemito, bd, tran, "CobOrdenesCobrosRemitosInsertar");
                        if (cobroRemito.IdOrdenCobroRemtio == 0)
                        {
                            AyudaProgramacionLN.MapearError(valor, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        //if(!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, bd, tran, "CobOrdenesCobrosDetallesActualizar"))
                        //{
                        //    AyudaProgramacionLN.MapearError(detalle, pParametro);
                        //    return false;
                        //}
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool AgregarCobroDetalleFactura(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        {
            //Guardo el detalle de la Orden de Cobro
            bool resultado = true;
            CobOrdenesCobrosFacturas item;
            foreach (CobOrdenesCobrosDetalles detalle in pParametro.OrdenesCobrosDetalles)
            {
                if (detalle.Factura.IdFactura > 0)
                {
                    item = new CobOrdenesCobrosFacturas();
                    item.IdFactura = detalle.Factura.IdFactura;
                    item.IdOrdenCobro = pParametro.IdOrdenCobro;
                    item.ImporteCobroParcial = detalle.Importe;
                    item.UsuarioLogueado = pParametro.UsuarioLogueado;
                    item.IdOrdenCobroFactura = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "CobOrdenesCobrosFacturasInsertar");
                    if (item.IdOrdenCobroFactura == 0)
                    {
                        AyudaProgramacionLN.MapearError(item, pParametro);
                        resultado = false;
                        break;
                    }
                }
            }
            return resultado;
        }

        private bool ActualizarDetalleFactura(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        {
            //Guardo el detalle de la Orden de Cobro
            bool resultado = true;

            foreach (CobOrdenesCobrosDetalles detalle in pParametro.OrdenesCobrosDetalles)
            {
                if (detalle.Factura.IdFactura > 0)
                {
                    detalle.Factura.UsuarioLogueado = pParametro.UsuarioLogueado;
                    //IF (ImporteParcialCobrado = IMPORTE YA CONFIRMADO EN CAJA DE OTRAS ORDENES (+) detalle.Importe = el importe de ESTA orden)
                    if (detalle.Factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoA ||
                       detalle.Factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoB ||
                       detalle.Factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoC ||
                       detalle.Factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesA ||
                       detalle.Factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesB ||
                       detalle.Factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.NotaCreditoElectronicaMyPymesC ||
                        detalle.Factura.TipoFactura.IdTipoFactura == (int)EnumTiposFacturas.CbteInternoCredito
                        )
                    {
                        switch (pParametro.EstadoColeccion)
                        {
                            case EstadoColecciones.Agregado:
                                if (detalle.Factura.ImporteParcialCobrado + detalle.Importe == detalle.Factura.ImporteTotal) //Cuando Nota Cred acepte imp parcial, revisar!
                                    detalle.Factura.Estado.IdEstado = (int)EstadosFacturas.Imputado;
                                else
                                    detalle.Factura.Estado.IdEstado = (int)EstadosFacturas.ImputadoParcial;
                                break;

                            case EstadoColecciones.Modificado:
                                if ((detalle.Factura.ImporteParcialCobrado + detalle.Importe) * (-1) == detalle.Factura.ImporteTotal)
                                    detalle.Factura.Estado.IdEstado = (int)EstadosFacturas.Imputado;
                                else
                                    detalle.Factura.Estado.IdEstado = (int)EstadosFacturas.ImputadoParcial;
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (detalle.Factura.ImporteParcialCobrado + detalle.Importe == detalle.Factura.ImporteTotal)
                            detalle.Factura.Estado.IdEstado = (int)EstadosFacturas.Cobrada;
                        else
                            detalle.Factura.Estado.IdEstado = (int)EstadosFacturas.CobradaParcial;
                    }
                    if (!FacturasF.FacturasModificarEstado(detalle.Factura, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(detalle.Factura, pParametro);
                        resultado = false;
                        break;
                    }
                }
            }
            return resultado;
        }

        private bool ActualizarCuentaCorriente(CobOrdenesCobros pParametro, Database bd, DbTransaction tran)
        {
            bool actualizarPrestamos = false;

            //TGETiposOperaciones operacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.OrdenesCobrosAfiliados);
            foreach (CobOrdenesCobrosDetalles detalle in pParametro.OrdenesCobrosDetalles)
            {
                if (detalle.CuentaCorriente.IdCuentaCorriente > 0)
                {
                    CarCuentasCorrientes cobroCtaCte = new CarCuentasCorrientes();
                    cobroCtaCte.IdRefCuentaCorriente = detalle.CuentaCorriente.IdCuentaCorriente;
                    cobroCtaCte.FechaMovimiento = DateTime.Now;
                    cobroCtaCte.Periodo = detalle.CuentaCorriente.Periodo;
                    cobroCtaCte.Estado.IdEstado = (int)EstadosCuentasCorrientes.Cobrado;
                    cobroCtaCte.IdAfiliado = pParametro.Afiliado.IdAfiliado;
                    cobroCtaCte.IdRefTipoOperacion = pParametro.IdOrdenCobro;
                    cobroCtaCte.TipoOperacion.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                    cobroCtaCte.Concepto = string.Concat(pParametro.TipoOperacion.TipoOperacion, " ", pParametro.IdOrdenCobro.ToString());
                    //cobroCtaCte.TipoCargo.TipoOperacion.IdTipoOperacion = 
                    cobroCtaCte.TipoOperacion.TipoMovimiento.IdTipoMovimiento = (int)EnumTGETiposMovimientos.Credito;
                    cobroCtaCte.Importe = detalle.Importe; //.CuentaCorriente.Importe;
                    cobroCtaCte.IdReferenciaRegistro = detalle.IdOrdenCobroDetalle;
                    //Por ahora se deja Efectivo
                    cobroCtaCte.TipoValor.IdTipoValor = (int)EnumTiposValores.Efectivo;
                    cobroCtaCte.IdAfiliado = pParametro.Afiliado.IdAfiliado;
                    cobroCtaCte.Moneda.IdMoneda = pParametro.Moneda.IdMoneda;
                    if (!CargosF.CuentasCorrientesAgregar(cobroCtaCte, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(cobroCtaCte, pParametro);
                        return false;
                    }
                    detalle.CuentaCorriente.ImporteCobrado = detalle.CuentaCorriente.ImporteCobradoOriginal + detalle.Importe;
                    if (detalle.CuentaCorriente.Importe == detalle.CuentaCorriente.ImporteCobrado)
                        detalle.CuentaCorriente.Estado.IdEstado = (int)EstadosCuentasCorrientes.Cobrado;
                    else
                        detalle.CuentaCorriente.Estado.IdEstado = (int)EstadosCuentasCorrientes.CobroParcial;

                    if (!CargosF.CuentasCorrientesActualizarEstado(detalle.CuentaCorriente, bd, tran))
                    {
                        AyudaProgramacionLN.MapearError(detalle.CuentaCorriente, pParametro);
                        return false;
                    }

                    //Valido Si el Cargo es de un Prestamo y se cobro la cuota
                    if (!actualizarPrestamos && detalle.CuentaCorriente.Estado.IdEstado == (int)EstadosCuentasCorrientes.Cobrado
                            && (detalle.CuentaCorriente.IdRefTipoOperacion == (int)EnumTiposCargos.AyudaEconomicaCortoPlazo
                            || detalle.CuentaCorriente.IdRefTipoOperacion == (int)EnumTiposCargos.AyudaEconomicaLargoPlazo
                            || detalle.CuentaCorriente.IdRefTipoOperacion == (int)EnumTiposCargos.AyudaEconomicaGrilla)
                        )
                    {
                        actualizarPrestamos = true;
                    }
                }
            }

            if (pParametro.OrdenesCobrosDetalles.Exists(x => x.CuentaCorriente.IdCuentaCorriente > 0))
            {
                BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "CobOrdenesCobrosDetallesActualizarIdRefEstado");
            }

            if (actualizarPrestamos)
            {
                Objeto retorno = new Objeto();
                if (!PrePrestamosF.PrestamosCuotasActualizar(retorno, bd, tran))
                {
                    AyudaProgramacionLN.MapearError(retorno, pParametro);
                    return false;
                }
            }

            return true;
        }

        private bool Validar(CobOrdenesCobros pParametro)
        {
            if (pParametro.OrdenesCobrosDetalles.FindAll(x => x.EstadoColeccion == EstadoColecciones.Agregado).Count == 0)
            {
                pParametro.CodigoMensaje = "ValidarOrdenesCobrosDetalles";
                return false;
            }
            if (pParametro.OrdenesCobrosDetalles.FindAll(x => x.EstadoColeccion == EstadoColecciones.Agregado).Count > 0
                && pParametro.OrdenesCobrosDetalles.FindAll(x => x.Importe > 0).Sum(x => x.Importe) == 0
                )
            {
                pParametro.CodigoMensaje = "ValidarImporteTotal";
                return false;
            }
            //if (pParametro.ImporteTotal < 0
            //    || (pParametro.ImporteTotal == 0
            //        && !pParametro.OrdenesCobrosDetalles.Exists(x=> x.Factura.TipoFactura.IdTipoFactura==(int)EnumTiposFacturas.NotaCreditoA
            //                                                    || x.Factura.TipoFactura.IdTipoFactura==(int)EnumTiposFacturas.NotaCreditoB
            //                                                    || x.Factura.TipoFactura.IdTipoFactura==(int)EnumTiposFacturas.NotaCreditoC
            //                                                    || x.Factura.TipoFactura.IdTipoFactura==(int)EnumTiposFacturas.CbteInternoCredito)
            //        && pParametro.OrdenesCobrosAnticipos.Where(x=>x.IncluirEnOC==true).Sum(x=>x.ImporteAplicar)==0)
            //    )
            //{
            //    pParametro.CodigoMensaje = "ValidarImporteTotal";
            //    return false;
            //}

            //Esto tiene relacion con CajasMovimientosConfirmar.aspx.cs
            //Ordenes de Cobros Valores solo esta para Modulo de Facturas por ahora
            //if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas
            //    || pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas)
            //{
            if (pParametro.Afiliado.IdAfiliado == 0)
            {
                pParametro.CodigoMensaje = "ValidarCliente";
                return false;
            }
            if (pParametro.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.NotaCreditoCargos)
            {
                if (pParametro.OrdenesCobrosValores.Sum(x => x.Importe)
                    + pParametro.OrdenesCobrosTiposRetenciones.Sum(x => x.ImporteTotalRetencion)
                    + pParametro.OrdenesCobrosAnticipos.Where(x => x.IncluirEnOC).Sum(x => x.ImporteAplicar)
                    != pParametro.OrdenesCobrosDetalles.Where(x => x.IncluirEnOP).Sum(x => x.Importe))
                {
                    pParametro.CodigoMensaje = "ValidarImporteTotal";
                    return false;
                }

                if (pParametro.OrdenesCobrosValores.Sum(x => x.Importe) > pParametro.ImporteSubTotal
                    && pParametro.OrdenesCobrosDetalles.Exists(x => !x.IncluirEnOP))
                {
                    pParametro.CodigoMensaje = "ValidarImporteValoresItemesIncluidos";
                    return false;
                }

                /*  Validacion de Ordenes de Cobro Anticipo para Tipos de Valores Prestamos y Cargos
                 * para poder trazar los remitos con las facturas que se generan posteriormente
                */
                if (pParametro.OrdenesCobrosValores.Exists(x => x.TipoValor.IdTipoValor == (int)EnumTiposValores.Cargos
                         || x.TipoValor.IdTipoValor == (int)EnumTiposValores.Prestamos))
                {
                    if (pParametro.OrdenesCobrosDetalles.Exists(x => x.Factura.IdFactura > 0
                        && x.EsAnticipo))
                    {
                        pParametro.CodigoMensaje = "ValidarNoPermiteAnticipoYFacturaParaTiposValores";
                        return false;
                    }

                    if (pParametro.OrdenesCobrosDetalles.Exists(x => x.EsAnticipo)
                        && pParametro.Remitos.Count == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarAnticipoRemitoParaTiposValores";
                        return false;
                    }
                }

                //}

                if ((pParametro.PrefijoNumeroRecibo != string.Empty && Convert.ToInt32(pParametro.PrefijoNumeroRecibo) > 0)
                        || (pParametro.NumeroRecibo != string.Empty && Convert.ToInt32(pParametro.NumeroRecibo) > 0)
                    )
                {
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CobOrdenesCobrosValidarNumeroComprobante"))
                    {
                        pParametro.CodigoMensaje = "ErrorValidarNumeroComprobanteManual";
                        return false;
                    }
                }

                if (pParametro.CargoDescuentoAfiliado)
                {
                    if (pParametro.FormaCobroAfiliado.IdFormaCobroAfiliado == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarFormaCobroAfiliado";
                        return false;
                    }
                    if (!pParametro.CuotasDescuentoAfiliado.HasValue || pParametro.CuotasDescuentoAfiliado.Value == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarCuotasDescuentoAfiliado";
                        return false;
                    }
                    if (pParametro.ImporteRetenciones > 0)
                    {
                        pParametro.CodigoMensaje = "ValidarCargoDescuentoAfiliadoTiposRetenciones";
                        return false;
                    }
                }

                if (pParametro.OrdenesCobrosTiposRetenciones.Exists(x => x.ImporteTotalRetencion > 0 && x.TipoRetencion.IdTipoRetencion == 0))
                {
                    pParametro.CodigoMensaje = "ValidarTiposRetencionesImporte";
                    return false;
                }

                //List<CobOrdenesCobrosDetalles> itemsInlcuidos = pParametro.OrdenesCobrosDetalles.Where(x => x.IncluirEnOP).ToList();
                //if (itemsInlcuidos.Select(x => x.Factura.TipoFactura.Contabilizar).Distinct().Count() > 1)
                //{
                //    pParametro.CodigoMensaje = "ValidarTiposComprobantesContabilizar";
                //    return false;
                //}
            }
            return true;
        }

        private List<InterfazValoresImportes> ObtenerValoresImporte(CobOrdenesCobros pParametro)
        {
            List<InterfazValoresImportes> resultado = new List<InterfazValoresImportes>();
            InterfazValoresImportes item;
            foreach (CobOrdenesCobrosValores mov in pParametro.OrdenesCobrosValores)
            {
                switch (mov.TipoValor.IdTipoValor)
                {
                    case (int)EnumTiposValores.Efectivo:
                        item = new InterfazValoresImportes();
                        item.IdTipoValor = mov.TipoValor.IdTipoValor;
                        item.Importe = mov.Importe;
                        resultado.Add(item);
                        break;
                    //case (int)EnumTiposValores.Cheque:
                    //    foreach (TESCheques ch in mov.Cheques)
                    //    {
                    //        item = new InterfazValoresImportes();
                    //        item.IdTipoValor = mov.TipoValor.IdTipoValor;
                    //        item.Importe = ch.Importe;
                    //        item.IdBancoCuenta = ch.IdBancoCuenta;
                    //        resultado.Add(item);
                    //    }
                    //    break;
                    case (int)EnumTiposValores.Transferencia:
                        if (!resultado.Exists(x => x.IdTipoValor == (int)EnumTiposValores.Transferencia
                                            && x.IdBancoCuenta == mov.BancoCuenta.IdBancoCuenta))
                        {
                            item = new InterfazValoresImportes();
                            item.IdTipoValor = mov.TipoValor.IdTipoValor;
                            item.IdBancoCuenta = mov.BancoCuenta.IdBancoCuenta;
                            resultado.Add(item);
                        }
                        item = resultado.Find(x => x.IdTipoValor == (int)EnumTiposValores.Transferencia
                                            && x.IdBancoCuenta == mov.BancoCuenta.IdBancoCuenta);
                        item.Importe += mov.Importe;
                        break;
                    case (int)EnumTiposValores.ChequeTercero:
                        if (!resultado.Exists(x => x.IdTipoValor == (int)EnumTiposValores.ChequeTercero))
                        {
                            item = new InterfazValoresImportes();
                            item.IdTipoValor = mov.TipoValor.IdTipoValor;
                            resultado.Add(item);
                        }
                        item = resultado.Find(x => x.IdTipoValor == (int)EnumTiposValores.ChequeTercero);
                        item.Importe += mov.Importe;
                        break;
                    case (int)EnumTiposValores.TarjetaCredito:
                        if (!resultado.Exists(x => x.IdTipoValor == (int)EnumTiposValores.TarjetaCredito))
                        {
                            item = new InterfazValoresImportes();
                            item.IdTipoValor = mov.TipoValor.IdTipoValor;
                            resultado.Add(item);
                        }
                        item = resultado.Find(x => x.IdTipoValor == (int)EnumTiposValores.TarjetaCredito);
                        item.Importe += mov.Importe;
                        break;
                    default:
                        break;
                }
            }

            return resultado;
        }

        public List<CobOrdenesCobrosDetalles> ObtenerPendientePago(CobOrdenesCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobrosDetalles>("CobFacturasPendientePago", pParametro);
        }

        //public List<CobOrdenesCobros> ObtenerFacturaListaFiltro(CobOrdenesCobros pParametro)
        //{
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobros>("CobOrdenesCobrosFacturaSeleccionarFiltro", pParametro);
        //}

        public DataTable ObtenerFacturaListaFiltro(CobOrdenesCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CobOrdenesCobrosFacturaSeleccionarFiltro", pParametro);
        }

        public List<CobOrdenesCobros> AnticiposPendientesAplicar(CobOrdenesCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobros>("CobOrdenesCobrosAnticiposSeleccionarPendientesAplicar", pParametro);
        }

        public CobOrdenesCobros ObtenerDatosFacturaDatosCompletos(CobOrdenesCobros pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CobOrdenesCobros>("CobOrdenesCobrosFacturasSeleccionar", pParametro);
            pParametro.OrdenesCobrosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobrosDetalles>("CobOrdenesCobrosDetalllesSeleccionrPorOrdenesCobros", pParametro);
            pParametro.OrdenesCobrosValores = BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobrosValores>("CobOrdenesCobrosValoresSeleccionarPorOrdenCobro", pParametro);
            CobOrdenesCobrosTiposRetenciones tipoRet = new CobOrdenesCobrosTiposRetenciones();
            tipoRet.IdOrdenCobro = pParametro.IdOrdenCobro;
            pParametro.OrdenesCobrosTiposRetenciones = new CobOrdenesCobrosTiposRetencionesLN().ObtenerListaFiltro(tipoRet);
            pParametro.ImporteRetenciones = pParametro.OrdenesCobrosTiposRetenciones.Sum(x => x.ImporteTotalRetencion);
            pParametro.OrdenesCobrosAnticipos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobros>("CobOrdenesCobrosAnticiposSeleccionarAplicados", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            return pParametro;
        }

        public List<CarCuentasCorrientes> ObtenerCobradasPorIdOrdenCobro(CobOrdenesCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CarCuentasCorrientes>("CarCuentasCorrientesObtenerPorIdOrdenCobro", pParametro);
        }

        /// <summary>
        /// Devuelve los Archivos de Facturas y Remitos Asociados a la Orden de Cobro
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<TGEArchivos> ObtenerArchivos(CobOrdenesCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEArchivos>("CobOrdenesCobrosSeleccionarArchivosFacturaRemito", pParametro);
        }
        public List<TGEArchivos> ObtenerArchivos(VTAFacturas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEArchivos>("CobOrdenesCobrosSeleccionarArchivosFacturaRemitoPorIdFactura", pParametro);
        }
        #region Anticipos
        public bool AgregarAnticipos(CobOrdenesCobros pParametro)
        {
            bool resultado = true;

            if (pParametro.ImporteTotal <= 0)
            {
                pParametro.CodigoMensaje = "ValidarImporteTotal";
                return false;
            }

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)EstadosOrdenesCobro.Activo;
            //pParametro.FechaEmision = DateTime.Now;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo la Orden de Cobro
                    pParametro.IdOrdenCobro = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CobOrdenesCobrosInsertar");
                    if (pParametro.IdOrdenCobro == 0)
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
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
            }

            return resultado;
        }
        #endregion

        public List<CobOrdenesCobros> ObtenerOCFNoImputadas(CobOrdenesCobros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobros>("CobOrdenesCobrosObtenerNoImputadas", pParametro);
        }
        //private bool GenerarActualizarPDF(VTAFacturas pParametro)//, Database bd, DbTransaction tran)
        //{
        //    #region Generao y Guardo el Comprobante
        //    bool resultadoPDF = true;
        //    pParametro.FacturaPDF = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.VTAComprobantes, "VTAComprobantes", pParametro, pParametro.UsuarioLogueado);
        //    return resultadoPDF;
        //    #endregion
        //}

        //public VTAFacturas ObtenerArchivo(VTAFacturas pFactura)
        //{
        //    this.GenerarActualizarPDF(pFactura);
        //    return pFactura;
        //}

        public bool ArmarMailFactura(CobOrdenesCobros pParametro, MailMessage mail)
        {
            bool resultado = true;

            List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
            TGEArchivos archivo = new TGEArchivos();
            archivo.Archivo = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.CobOrdenesCobros, "OrdenesCobros", pParametro, pParametro.UsuarioLogueado);

            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
            string nombreArchivo = string.Concat(empresa.CUIT, "_", pParametro.TipoOperacion.TipoOperacion.Replace(" ", ""), "_", pParametro.IdOrdenCobro, ".pdf");
            archivo.NombreArchivo = nombreArchivo;
            listaArchivos.Add(archivo);

            string template = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Templates\\MailEnviarFactura.htm");
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "OrdenesCobrosMail";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            if (pParametro.Afiliado.CorreoElectronico.Trim() != string.Empty)
            {
                if (pParametro.Afiliado.CorreoElectronico.Trim().Contains(";"))
                {
                    List<string> lista = pParametro.Afiliado.CorreoElectronico.Trim().Split(';').ToList();
                    foreach (string item in lista)
                        mail.To.Add(new MailAddress(item.Trim(), pParametro.Afiliado.ApellidoNombre.Trim()));
                }
                else
                    mail.To.Add(new MailAddress(pParametro.Afiliado.CorreoElectronico.Trim(), pParametro.Afiliado.ApellidoNombre.Trim()));
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
                mail.Body = mail.Body.Replace("%ApellidoNombre%", pParametro.Afiliado.ApellidoNombre);
                mail.Body = mail.Body.Replace("%TipoFacturaNumeroCompleto%", string.Concat(pParametro.TipoOperacion.TipoOperacion));
                mail.Body = mail.Body.Replace("%Empresa%", empresa.Empresa);
            }

            foreach (TGEArchivos attach in listaArchivos)
                mail.Attachments.Add(new Attachment(new MemoryStream(attach.Archivo), attach.NombreArchivo));

            return resultado;
        }

    }
}
