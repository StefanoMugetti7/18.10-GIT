using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Contabilidad.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System.Xml;

namespace Contabilidad.LogicaNegocio
{
    class CtbAsientosModelosLN : BaseLN<CtbAsientosModelos>
    {
        public override bool Agregar(CtbAsientosModelos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (pParametro.TipoAsiento.IdTipoAsiento == (int)EnumTiposAsientos.Manuales)
                pParametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.AsientoContableManual;

            if (!this.Validar(pParametro))
                return false;

            pParametro.FechaEvento = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdAsientoModelo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CtbAsientosModelosInsertar");
                    if (pParametro.IdAsientoModelo == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarAsientosModelosDetalles(pParametro, new CtbAsientosModelos(), bd, tran))
                        resultado = false;

                    ////Inserta los Asientos Modelos Detalles
                    //foreach (CtbAsientosModelosDetalles asientoModeloDetalle in pParametro.AsientosModelosDetalles)
                    //{
                    //    resultado = GuardarAsientosModelosDetalles(pParametro, resultado, tran, bd, asientoModeloDetalle);
                    //}    

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

        public override bool Modificar(CtbAsientosModelos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro))
                return false;



            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CtbAsientosModelos asientoViejo = new CtbAsientosModelos();
            asientoViejo.IdAsientoModelo = pParametro.IdAsientoModelo;
            asientoViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            asientoViejo = this.ObtenerDatosCompletos(asientoViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Modificar(pParametro, bd, tran);

                    if (resultado && !this.ActualizarAsientosModelosDetalles(pParametro, asientoViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.EliminarAsientosModelosDetalles(asientoViejo, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(asientoViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && pParametro.ModificarAsientos && pParametro.LoteCuentasContables.HasChildNodes)
                    {
                        resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, bd, tran, "CtbAsientosModelosActualizarAsientosContablesProceso");
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

        public override CtbAsientosModelos ObtenerDatosCompletos(CtbAsientosModelos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosModelos>("CtbAsientosModelosSeleccionar", pParametro);
            pParametro.AsientosModelosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosModelosDetalles>("CtbAsientosModelosDetallesSeleccionarPorAsientosModelos", pParametro);
            return pParametro;
        }

        ///// <summary>
        ///// Devuelve un Asiento Modelo por Tipo de Operacion
        ///// </summary>
        ///// <param name="pParametro">IdTipoOperacion</param>
        ///// <param name="db"></param>
        ///// <param name="tran"></param>
        ///// <returns></returns>
        //public CtbAsientosModelos ObtenerDatosCompletos(TGETiposOperaciones pParametro)
        //{
        //    CtbAsientosModelos modelo = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosModelos>("CtbAsientosModelosSeleccionarPorTipoOperacion", pParametro);
        //    modelo.AsientosModelosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosModelosDetalles>("CtbAsientosModelosDetallesSeleccionarPorAsientosModelos", modelo);
        //    return modelo;
        //}

        /// <summary>
        /// Devuelve un Asiento Modelo por Tipo de Operacion
        /// </summary>
        /// <param name="pParametro">IdTipoOperacion</param>
        /// <param name="db"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        public CtbAsientosModelos ObtenerDatosCompletos(CtbAsientosContables pParametro, Database db, DbTransaction tran)
        {
            CtbAsientosModelos modelo = BaseDatos.ObtenerBaseDatos().Obtener<CtbAsientosModelos>("CtbAsientosModelosSeleccionarPorTipoOperacion", pParametro, db, tran);
            modelo.AsientosModelosDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosModelosDetalles>("CtbAsientosModelosDetallesSeleccionarPorAsientosModelos", modelo, db, tran);
            return modelo;
        }

        public List<CtbAsientosModelos> ObtenerLista(CtbAsientosModelos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosModelos>("CtbAsientosModelosListar", pParametro);
        }

        public override List<CtbAsientosModelos> ObtenerListaFiltro(CtbAsientosModelos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbAsientosModelos>("CtbAsientosModelosListarFiltro", pParametro);
        }

        private bool Modificar(CtbAsientosModelos pParametro, Database bd, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CtbAsientosModelosActualizar"))
                return false;

            return true;
        }

        //private static bool GuardarAsientosModelosDetalles(CtbAsientosModelos pParametro, bool resultado, DbTransaction tran, Database bd, CtbAsientosModelosDetalles asientoModeloDetalle)
        //{
        //    asientoModeloDetalle.IdAsientoModelo = pParametro.IdAsientoModelo;
        //    asientoModeloDetalle.Estado.IdEstado = (int)Estados.Activo;
        //    asientoModeloDetalle.IdAsientoModeloDetalle = BaseDatos.ObtenerBaseDatos().Agregar(asientoModeloDetalle, bd, tran, "CtbAsientosModelosDetallesInsertar");
        //    if (asientoModeloDetalle.IdAsientoModeloDetalle == 0)
        //        resultado = false;
        //    return resultado;
        //}

        private bool ActualizarAsientosModelosDetalles(CtbAsientosModelos pParametro, CtbAsientosModelos pValorViejo, Database bd, DbTransaction tran)
        {
            pParametro.LoteCuentasContables = new XmlDocument();
            XmlNode cuentasNodo = pParametro.LoteCuentasContables.CreateElement("CuentasContables");
            pParametro.LoteCuentasContables.AppendChild(cuentasNodo);

            XmlNode cuentaNode;
            XmlAttribute cuentaAttribute;
            XmlAttribute cuentaAnteriorAttribute;

            CtbAsientosModelosDetalles asientoModeloDetalleViejo;
            foreach (CtbAsientosModelosDetalles asientoModeloDetalle in pParametro.AsientosModelosDetalles)
            {
                switch (asientoModeloDetalle.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        asientoModeloDetalle.IdAsientoModelo = pParametro.IdAsientoModelo;
                        asientoModeloDetalle.Estado.IdEstado = (int)Estados.Activo;
                        asientoModeloDetalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        asientoModeloDetalle.IdAsientoModeloDetalle = BaseDatos.ObtenerBaseDatos().Agregar(asientoModeloDetalle, bd, tran, "CtbAsientosModelosDetallesInsertar");
                        if (asientoModeloDetalle.IdAsientoModeloDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(asientoModeloDetalle, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(asientoModeloDetalle, bd, tran, "CtbAsientosModelosDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(asientoModeloDetalle, pParametro);
                            return false;
                        }

                        asientoModeloDetalleViejo =pValorViejo.AsientosModelosDetalles.Find(x => x.IdAsientoModeloDetalle == asientoModeloDetalle.IdAsientoModeloDetalle);
                        if (!AuditoriaF.AuditoriaAgregar(asientoModeloDetalleViejo, Acciones.Update, asientoModeloDetalle, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(asientoModeloDetalle, pParametro);
                            return false;
                        }
                        if (asientoModeloDetalle.CuentaContable.IdCuentaContable != asientoModeloDetalleViejo.CuentaContable.IdCuentaContable)
                        {
                            cuentaNode = pParametro.LoteCuentasContables.CreateElement("CuentaContable");
                            cuentaAttribute = pParametro.LoteCuentasContables.CreateAttribute("IdCuentaContable");
                            cuentaAttribute.Value = asientoModeloDetalle.CuentaContable.IdCuentaContable.ToString();
                            cuentaNode.Attributes.Append(cuentaAttribute);
                            cuentaAnteriorAttribute = pParametro.LoteCuentasContables.CreateAttribute("IdCuentaContableAnterior");
                            cuentaAnteriorAttribute.Value = asientoModeloDetalleViejo.CuentaContable.IdCuentaContable.ToString();
                            cuentaNode.Attributes.Append(cuentaAnteriorAttribute);
                            cuentasNodo.AppendChild(cuentaNode);
                        }
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool EliminarAsientosModelosDetalles(CtbAsientosModelos pAsientoViejo, CtbAsientosModelos pParametro, Database bd, DbTransaction tran)
        {
            foreach (CtbAsientosModelosDetalles asientoDetalle in pAsientoViejo.AsientosModelosDetalles)
            {
                if (!pParametro.AsientosModelosDetalles.Exists(x => x.IdAsientoModeloDetalle == asientoDetalle.IdAsientoModeloDetalle))
                {
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(asientoDetalle, bd, tran, "CtbAsientosModelosDetallesEliminar"))
                    {
                        AyudaProgramacionLN.MapearError(asientoDetalle, pParametro);
                        return false;
                    }
                }
            }
            return true;
        }

        private bool Validar(CtbAsientosModelos pParametro)
        {
            if (pParametro.AsientosModelosDetalles.Count < 2)
            {
                pParametro.CodigoMensaje = "AsientoModeloDosCuentasContables";
                return false;
            }
            var contDebe = 0;
            var contHaber = 0;
            var codigoAM = 0;

            foreach (var asientoModeloDetalle in pParametro.AsientosModelosDetalles)
            {
                if (asientoModeloDetalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo != 0)
                    codigoAM++;
                if (asientoModeloDetalle.CuentaContable.IdCuentaContable == 0
                    && !asientoModeloDetalle.AsientoModeloDetalleCodigo.CodigoValor.StartsWith("Tabla")
                    //&& !(asientoModeloDetalle.AsientoModeloDetalleCodigo.CodigoValor==EnumCodigosAsientosModelos.TablaBanco.ToString()
                    //       || asientoModeloDetalle.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.TablaCaja.ToString()
                    //       //|| asientoModeloDetalle.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.TablaChequesTerceros.ToString()
                    //       //|| asientoModeloDetalle.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.ValDepo.ToString()
                    //       || asientoModeloDetalle.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.TablaCmpFliaProducto.ToString()
                    //       || asientoModeloDetalle.AsientoModeloDetalleCodigo.CodigoValor == EnumCodigosAsientosModelos.TablaTiposCargos.ToString()
                    //       )
                    )
                {
                    pParametro.CodigoMensaje = "SeleccioneCuentaContable";
                    return false;
                }
                if (pParametro.TipoAsiento.IdTipoAsiento == (int)EnumTiposAsientos.Automaticos)
                {
                    switch (asientoModeloDetalle.TipoImputacion.IdTipoImputacion)
                    {
                        case (int)EnumTiposImputaciones.Debe:
                            contDebe++;
                            break;
                        case (int)EnumTiposImputaciones.Haber:
                            contHaber++;
                            break;
                        default:
                            break;
                    }
                }
            }
            if (pParametro.TipoAsiento.IdTipoAsiento == (int)EnumTiposAsientos.Automaticos && !(contDebe >= 1 && contHaber >= 1))
            {
                pParametro.CodigoMensaje = "SeleccioneDebeHaber";
                return false;
            }

            if (pParametro.TipoAsiento.IdTipoAsiento == (int)EnumTiposAsientos.Automaticos)
            {
                if (codigoAM < 2)
                {
                    pParametro.CodigoMensaje = "AsientoModeloValidarCodigoAM";
                    return false;
                }

                if (pParametro.TipoOperacion.IdTipoOperacion == 0)
                {
                    pParametro.CodigoMensaje = "AsientoModeloValidarTipoOperacion";
                    return false;
                }

                //AsientoModeloValidarTipoOperacionExiste
                if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CtbAsientosModelosValidaciones"))
                {
                    pParametro.CodigoMensaje = "AsientoModeloValidarTipoOperacion";
                    return false;
                }
                
            }
            return true;
        }

        /// <summary>
        /// Agrega un Asiento Detalle al Asiento Contable
        /// </summary>
        /// <param name="pAsiento"></param>
        /// <param name="pImporte"></param>
        /// <param name="pModelo"></param>
        /// <param name="pCodigoAsientoModelo"></param>
        /// <returns></returns>
        public bool ObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo)
        {
            return this.ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo, null, 0, 0, null, null, null, null, null);
        }

        public bool ObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo, TGEFiliales pFilial, int pIdTipoValor, int pIdBancoCuenta, Database bd, DbTransaction tran)
        {
            return this.ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo, pFilial, pIdTipoValor, pIdBancoCuenta, null, null, null, bd, tran);
        }

        public bool ObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo, CtbCuentasContables pCuentaContable, CtbCentrosCostosProrrateos pCentroCosto)
        {
            return this.ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo, null, 0, 0, pCuentaContable, null, pCentroCosto, null, null);
        }

        public bool ObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo, TGEListasValoresSistemasDetalles pListaValorSistemaDetalle, Database bd, DbTransaction tran)
        {
            return this.ObtenerAsientoDetalle(pAsiento, pImporte, pModelo, pCodigoAsientoModelo, null, 0, 0, null, pListaValorSistemaDetalle, null, bd, tran);
        }
        
        /// <summary>
        /// Agrega un Asiento Detalle al Asiento Contable
        /// </summary>
        /// <param name="pAsiento"></param>
        /// <param name="pImporte"></param>
        /// <param name="pModelo"></param>
        /// <param name="pCodigoAsientoModelo"></param>
        /// <param name="pFilial"></param>
        /// <param name="pTipoValor"></param>
        /// <param name="pIdBancoCuenta"></param>
        /// <returns></returns>
        private bool ObtenerAsientoDetalle(CtbAsientosContables pAsiento, decimal pImporte, CtbAsientosModelos pModelo, EnumCodigosAsientosModelos pCodigoAsientoModelo, TGEFiliales pFilial, int pIdTipoValor, int pIdBancoCuenta, CtbCuentasContables pCuentaContable, TGEListasValoresSistemasDetalles pListaValorSistemaDetalle, CtbCentrosCostosProrrateos pCentroCosto, Database bd, DbTransaction tran)
        {
            CtbAsientosContablesDetalles asientoDetalle = new CtbAsientosContablesDetalles();
            asientoDetalle.Estado.IdEstado = (int)EstadosContabilidad.Activo;
            asientoDetalle.EstadoColeccion = Comunes.Entidades.EstadoColecciones.Agregado;
            asientoDetalle.CentroCostoProrrateo = pCentroCosto==null? new CtbCentrosCostosProrrateos() : pCentroCosto;
            CtbAsientosModelosDetalles modeloDetalle = pModelo.AsientosModelosDetalles.Find(x => x.AsientoModeloDetalleCodigo.CodigoValor == pCodigoAsientoModelo.ToString());
            if (modeloDetalle == null && pImporte > 0)
            {
                pAsiento.CodigoMensaje = "AsientoModeloDetalleCodigoNoExiste";
                pAsiento.CodigoMensajeArgs.Add(pCodigoAsientoModelo.ToString());
                pAsiento.CodigoMensajeArgs.Add(pModelo.Detalle);
                pAsiento.CodigoMensajeArgs.Add(pModelo.TipoOperacion.TipoOperacion);
                return false;
            }
            else if (modeloDetalle == null && pImporte == 0)
            {
                return true;
            }

            switch (pCodigoAsientoModelo)
            {
                case EnumCodigosAsientosModelos.TablaCaja:
                case EnumCodigosAsientosModelos.TablaBanco:
                case EnumCodigosAsientosModelos.TablaBancoDestino:
                    CtbAsientosContablesCuentasContablesParametros filtro = new CtbAsientosContablesCuentasContablesParametros();
                    filtro.Filial = pFilial;
                    if (pIdTipoValor == (int)EnumTiposValores.Cheque || pIdTipoValor == (int)EnumTiposValores.ChequeTercero)
                        pIdTipoValor = (int)EnumTiposValores.Transferencia;
                    filtro.TipoValor.IdTipoValor = pIdTipoValor;
                    filtro.BancoCuentaIdBancoCuenta = pIdBancoCuenta;
                    filtro.Moneda.IdMoneda = (int)EnumTGEMonedas.PesosArgentinos;

                    if (bd == null || tran == null)
                    {
                        pAsiento.CodigoMensaje = "Parametro Database o Tran NULL";
                        return false;
                    }

                    filtro = ContabilidadF.AsientosContablesCuentasContablesParametrosObtenerCuentaContable(filtro, bd, tran);

                    if (filtro == null || filtro.IdAsientoContableCuentaContableParametro == 0)
                    {
                        pAsiento.CodigoMensaje = "AsientoModeloDetalleParametosCuentaContable";
                        if (pFilial != null)
                            pAsiento.CodigoMensajeArgs.Add(pFilial.Filial);
                        pAsiento.CodigoMensajeArgs.Add(EnumTGEMonedas.PesosArgentinos.ToString());
                        pAsiento.CodigoMensajeArgs.Add(pIdTipoValor.ToString());
                        return false;
                    }
                    asientoDetalle.CuentaContable = filtro.CuentaContable;
                    break;
                case EnumCodigosAsientosModelos.TablaListaValorCuentaContable:
                    TGEListasValoresSistemasDetallesCuentasContables listaCuenta = new TGEListasValoresSistemasDetallesCuentasContables();
                    listaCuenta.ListaValorSistemaDetalle.IdListaValorSistemaDetalle=pListaValorSistemaDetalle.IdListaValorSistemaDetalle;

                    if (bd == null || tran == null)
                    {
                        pAsiento.CodigoMensaje = "Parametro Database o Tran NULL";
                        return false;
                    }

                    listaCuenta = new TGEListasValoresSistemasDetallesCuentasContablesLN().ObtenerDatosCompletos(listaCuenta.ListaValorSistemaDetalle, bd, tran);

                    if (listaCuenta == null || listaCuenta.CuentaContable.IdCuentaContable== 0)
                    {
                        pAsiento.CodigoMensaje = "AsientoModeloDetalleListaValorSistemaCuentaContable";
                        pAsiento.CodigoMensajeArgs.Add(pListaValorSistemaDetalle.IdListaValorSistemaDetalle.ToString());
                        return false;
                    }
                    asientoDetalle.CuentaContable = listaCuenta.CuentaContable;
                    break;
                default:
                    asientoDetalle.CuentaContable = pCuentaContable == null ? modeloDetalle.CuentaContable : pCuentaContable;
                    break;
            }

            if (modeloDetalle.TipoImputacion.IdTipoImputacion == (int)EnumTiposImputaciones.Debe)
                asientoDetalle.Debe = pImporte;
            else
                asientoDetalle.Haber = pImporte;

            if (asientoDetalle.Debe > 0 || asientoDetalle.Haber > 0)
                pAsiento.AsientosContablesDetalles.Add(asientoDetalle);
            //if (asientoDetalle.Debe > 0)
            //{
            //    if (pAsiento.AsientosContablesDetalles.Exists(x => x.Debe > 0 && x.CuentaContable.IdCuentaContable == asientoDetalle.CuentaContable.IdCuentaContable))
            //        pAsiento.AsientosContablesDetalles.Find(x => x.Debe > 0 && x.CuentaContable.IdCuentaContable == asientoDetalle.CuentaContable.IdCuentaContable).Debe += asientoDetalle.Debe;
            //    else
            //        pAsiento.AsientosContablesDetalles.Add(asientoDetalle);
            //}

            //if (asientoDetalle.Haber > 0)
            //{
            //    if (pAsiento.AsientosContablesDetalles.Exists(x => x.Haber > 0 && x.CuentaContable.IdCuentaContable == asientoDetalle.CuentaContable.IdCuentaContable))
            //        pAsiento.AsientosContablesDetalles.Find(x => x.Haber > 0 && x.CuentaContable.IdCuentaContable == asientoDetalle.CuentaContable.IdCuentaContable).Haber += asientoDetalle.Haber;
            //    else
            //        pAsiento.AsientosContablesDetalles.Add(asientoDetalle);
            //}

            return true;
        }
    }
}
