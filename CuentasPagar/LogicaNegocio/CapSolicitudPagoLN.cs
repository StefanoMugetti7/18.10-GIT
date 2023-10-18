using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using Comunes;
using Comunes.Entidades;
using Compras.LogicaNegocio;
using CuentasPagar.Entidades;
using Proveedores;
using Comunes.LogicaNegocio;
using Auditoria.Entidades;
using Subsidios.Entidades;
using Subsidios;
using System.Reflection;
using Generales.Entidades;
using Compras.Entidades;
using Compras;
using Cargos.Entidades;
using Cargos;
using Contabilidad;
using Contabilidad.Entidades;
using System.Data;
using System.Xml;
using Proveedores.Entidades;
using ProcesosDatos.Entidades;

namespace CuentasPagar.LogicaNegocio
{
    public class CapSolicitudPagoLN : BaseLN<CapSolicitudPago>
    {
        public override CapSolicitudPago ObtenerDatosCompletos(CapSolicitudPago pParametro)
        {
            CapSolicitudPago SolicitudPago = BaseDatos.ObtenerBaseDatos().Obtener<CapSolicitudPago>("CapSolicitudPagoSeleccionar", pParametro);
            SolicitudPago.SolicitudPagoDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPagoDetalles>("CapSolicitudPagoDetallesSeleccionarPorSolicitudPago", pParametro);
            SolicitudPago.SolicitudPagoTiposPercepciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPagoTipoPercepcion>("CapSolicitudPagoTipoPercepcionSeleccionarPorSolicitudPago", pParametro);
            SolicitudPago.ComprobantesAsociados = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapSolicitudPagoNotasCreditosSeleccionarPorIdSolicitud", pParametro);
            SolicitudPago.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            SolicitudPago.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return SolicitudPago;
        }

        /// <summary>
        /// Metodo para Auditoria (solo cabecera)
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="bd"></param>
        /// <param name="tran"></param>
        /// <returns></returns>
        private CapSolicitudPago ObtenerDatosCompletos(CapSolicitudPago pParametro, Database bd, DbTransaction tran)
        {
            CapSolicitudPago SolicitudPago = BaseDatos.ObtenerBaseDatos().Obtener<CapSolicitudPago>("CapSolicitudPagoSeleccionar", pParametro, bd, tran);
            //SolicitudPago.SolicitudPagoDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPagoDetalles>("CapSolicitudPagoDetallesSeleccionarPorSolicitudPago", pParametro);
            //SolicitudPago.SolicitudPagoTiposPercepciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPagoTipoPercepcion>("CapSolicitudPagoTipoPercepcionSeleccionarPorSolicitudPago", pParametro);
            return SolicitudPago;
        }

        //public CapSolicitudPago ObtenerDatos(CapSolicitudPago pParametro, Database db, DbTransaction tran)
        //{
        //    CapSolicitudPago SolicitudPago = BaseDatos.ObtenerBaseDatos().Obtener<CapSolicitudPago>("CapSolicitudPagoSeleccionar", pParametro, db, tran);
        //    return SolicitudPago;
        //}
    
        public DataTable ObtenerGrilla(CapSolicitudPago pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapSolicitudPagoSeleccionarDescripcionPorFiltro", pParametro);
        }
        public override List<CapSolicitudPago> ObtenerListaFiltro(CapSolicitudPago pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapSolicitudPagoSeleccionarDescripcionPorFiltro", pParametro);
        }

        public List<CapSolicitudPago> ObtenerPendientePago(CapOrdenesPagos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapSolicitudPagoSeleccionarPendientePago", pParametro);
        }

        public List<CapSolicitudPago> ObtenerTercerosPendienteCobro(CapSolicitudPago pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapSolicitudPagoSeleccionarTercerosPendienteCobro", pParametro);
        }

        public List<CapSolicitudPago> ObtenerAnticiposPendientesPorProveedor(CapOrdenesPagos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapSolicitudesAnticiposObtenerPorEntidad", pParametro);
        }

        public override bool Agregar(CapSolicitudPago pParametro)
        {
            throw new NotImplementedException();
        }

        public bool Agregar(CapSolicitudPago pParametro, CmpInformesRecepciones pRemito)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!pParametro.FechaContable.HasValue)
                pParametro.FechaContable = DateTime.Now; //pParametro.FechaFactura;

            pParametro.Estado.IdEstado = (int)Estados.Activo;

            if(pParametro.ComprobantesAsociados.Count > 0)
            {
                pParametro.IdsSolicitudPagoNotaCredito = new XmlDocument();

                XmlNode items = pParametro.IdsSolicitudPagoNotaCredito.CreateElement("Ids");
                pParametro.IdsSolicitudPagoNotaCredito.AppendChild(items);

                XmlNode itemNodo;
                XmlNode ValorNode;

                foreach (CapSolicitudPago item in pParametro.ComprobantesAsociados)
                {
                    itemNodo = pParametro.IdsSolicitudPagoNotaCredito.CreateElement("Id");

                    ValorNode = pParametro.IdsSolicitudPagoNotaCredito.CreateElement("IdSolicitudPagoNotaCredito");
                    ValorNode.InnerText = item.IdSolicitudPago.ToString();
                    itemNodo.AppendChild(ValorNode);
                    items.AppendChild(itemNodo);
                }
            }

            //Las SP Terceros no se cambian!!!
            if (pParametro.TipoOperacion.IdTipoOperacion != (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros
                && pParametro.TiposFacturas.IdTipoFactura > 0)
            {
                TGETiposOperacionesTiposFacturas tipoOpFact = new TGETiposOperacionesTiposFacturas();
                //tipoOpFact.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
                tipoOpFact.IdTipoFactura = pParametro.TiposFacturas.IdTipoFactura;
                TGETiposOperaciones tipoOpera = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(tipoOpFact);
                if (tipoOpera.IdTipoOperacion == 0)
                {
                    pParametro.CodigoMensaje = "ValidarSolicitudPagoTipoOperacionComprobante";
                    pParametro.CodigoMensajeArgs.Add(pParametro.TipoOperacion.IdTipoOperacion.ToString());
                    pParametro.CodigoMensajeArgs.Add(pParametro.TiposFacturas.IdTipoFactura.ToString());
                    return false;
                }
                pParametro.TipoOperacion = tipoOpera;
            }

            if (pParametro.TipoSolicitudPago.IdTipoSolicitudPago == (int)EnumTiposSolicitudPago.Compras)
            {

                if (!this.Validar(pParametro, new CapSolicitudPago()))
                    return false;

            }
            else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosSubsidios)
            {
                if (!this.ValidarSubsidio(pParametro))
                    return false;

                pParametro.Estado.IdEstado = (int)EstadosSolicitudesPagos.Autorizado;
            }

            if (!this.CircuitoAprobacion(pParametro))
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
                    resultado = this.Agregar(pParametro, pRemito, bd, tran);

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

        private bool Agregar(CapSolicitudPago pParametro, Database bd, DbTransaction tran)
        {
            return this.Agregar(pParametro, new CmpInformesRecepciones(), bd, tran);
        }

        private bool Agregar(CapSolicitudPago pParametro, CmpInformesRecepciones pRemito, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            pParametro.IdSolicitudPago = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CapSolicitudPagoInsertar");
            if (pParametro.IdSolicitudPago == 0)
                resultado = false;

            if (resultado && !this.ActualizarDetalles(pParametro, new CapSolicitudPago(), bd, tran))
                resultado = false;

                    if (resultado && !this.ActualizarTiposPercepciones(pParametro, new CapSolicitudPago(), bd, tran))
                        resultado = false;


            if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                resultado = false;

            if (pParametro.TipoSolicitudPago.IdTipoSolicitudPago == (int)EnumTiposSolicitudPago.Subsidios)
            {
                //if (resultado && !new InterfazContableLN().AgregarSolicitudPagoSubsidios(pParametro, bd, tran))
                //    resultado = false;

                if (resultado && !this.ActualizarCausanteBeneficio(pParametro, bd, tran))
                    resultado = false;
            }
            
            if (pParametro.TipoOperacion.Contabiliza)
            {
                if (resultado && !new InterfazContableLN().AgregarSolicitudPago(pParametro, bd, tran))
                    resultado = false;
            }

            if (resultado && pParametro.RemitoAutomatico)
            {
                pRemito.NumeroRemitoPrefijo = pRemito.NumeroRemitoPrefijo.PadLeft(4, '0');
                pRemito.NumeroRemitoSufijo = pRemito.NumeroRemitoSufijo.PadLeft(8, '0');
                pRemito.UsuarioLogueado = pParametro.UsuarioLogueado;
                pRemito.IdSolicitudPago = pParametro.IdSolicitudPago;
                if (!BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pRemito, bd, tran, "CmpInformesRecepcionesInsertarProceso"))
                {
                    resultado = false;
                    AyudaProgramacionLN.MapearError(pRemito, pParametro);
                }
            }

            return resultado;
        }

        public override bool Modificar(CapSolicitudPago pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapSolicitudPago valorViejo = new CapSolicitudPago();
            valorViejo.IdSolicitudPago = pParametro.IdSolicitudPago;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapSolicitudPagoActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarDetalles(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    //if (resultado && !this.ActualizarTiposPercepciones(pParametro, valorViejo, bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;

        }

        public bool Modificar(CapSolicitudPago pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "CapSolicitudPagoActualizar"))
                return false;

            return true;
        }

        public bool ModificarEstado(CapSolicitudPago pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "CapSolicitudPagoActualizarEstado"))
                return false;

            return true;
        }

        public bool Anular(CapSolicitudPago pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaAnulacion = DateTime.Now;

            // VER en CapSolicitudPago.cs
            pParametro.IdUsuarioAnulacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)Estados.Baja;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapSolicitudPago valorViejo = new CapSolicitudPago();
            valorViejo.IdSolicitudPago = pParametro.IdSolicitudPago;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            if (!this.ValidarAnulacion(pParametro))
                return false;
            
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapSolicitudPagoActualizar"))
                        resultado = false;

                    //Valido Si la SP esta relacionada con una Orden de Cobro
                    if (resultado && BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CmpOrdenesComprasDetallesSolicitudPagoDetalleValidar"))
                    {
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CmpOrdenesComprasDetallesSolicitudPagoDetalleActualizarOCD"))
                        {
                            resultado = false;
                        }
                    }

                    //Valido Si la SP Genero un Cargo a Descontar - Solicitudes Pagos Terceros
                    if (resultado && BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosSolicitudPagoValidar"))
                    {
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosSolicitudPagoActualizarTCAFC"))
                        {
                            resultado = false;
                        }
                    }

                    //Valido Si la SP Genero un Cargo a Descontar - Ordenes Cobros Terceros
                    if (resultado && BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosSolicitudPagoDetalleValidar"))
                    {
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CarTiposCargosAfiliadosFormasCobrosSolicitudPagoDetalleActualizarTCAFC"))
                        {
                            resultado = false;
                        }
                    }

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (pParametro.TipoOperacion.Contabiliza)
                    {
                        if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosSubsidios)
                        {
                            if (resultado && !new InterfazContableLN().AnularSolicitudPagoSubsidios(pParametro, bd, tran))
                                resultado = false;
                        }
                        else
                        {
                            if (resultado && !new InterfazContableLN().AnularSolicitudPago(pParametro, bd, tran))
                                resultado = false;
                        }
                    }

                    if (resultado && !BorrarRelacionSolicitudDetallesInformesRecepcion(pParametro, bd, tran))
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

        public bool Autorizar(CapSolicitudPago pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            pParametro.FechaAutorizacion = DateTime.Now;
            pParametro.IdUsuarioAutorizacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
            pParametro.Estado.IdEstado = (int)EstadosSolicitudesPagos.Autorizado;


            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapSolicitudPago valorViejo = new CapSolicitudPago();
            valorViejo.IdSolicitudPago = pParametro.IdSolicitudPago;
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

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapSolicitudPagoAutorizar"))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

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
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;

        }

        private bool Validar(CapSolicitudPago pParametro, CapSolicitudPago pValorViejo)
        {
            CapSolicitudPago SolicitudPago = new CapSolicitudPago();

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CapSolicitudPagoValidarNumeroFactura"))
                    {
                        pParametro.CodigoMensaje = "ValidarNumeroFactura";
                        return false;
                    }

                    if (pParametro.SolicitudPagoDetalles.Count(x => x.EstadoColeccion == EstadoColecciones.Agregado) == 0)
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetalles";
                        return false;
                    }

                    if (pParametro.SolicitudPagoDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.Cantidad <= 0))
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetallesCantidad";
                        return false;
                    }

                    if (pParametro.SolicitudPagoDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado
                                             && x.PrecioUnitarioSinIva < 0   && x.DescuentoImporte != 0))
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetalleDescuentoNegativo";
                        return false;
                    }

                    if (pParametro.SolicitudPagoDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado
                                            &&  ((x.PrecioUnitarioSinIva < 0 && x.DescuentoImporte > 0) || (x.PrecioUnitarioSinIva > 0 && x.DescuentoImporte < 0))))
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetalleImportesNegativos";
                        return false;
                    }

                    if (pParametro.SolicitudPagoDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.PrecioUnitarioSinIva == 0 ))
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetallesPrecio";
                        return false;
                    }

                    TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CentroCostoOpcionalSolicitudPago);
                    bool bValor = valor.ParametroValor == string.Empty ? false : Convert.ToBoolean(valor.ParametroValor);
                    if (!bValor && pParametro.SolicitudPagoDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado
                        && !x.CentroCostoProrrateo.IdCentroCostoProrrateo.HasValue))
                    {
                        pParametro.CodigoMensaje = "ValidarSolicitudDetallesCentroCosto";
                        return false;
                    }

                    if (pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.FacturasB
                        || pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.FacturasC
                        || pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesB
                        || pParametro.TiposFacturas.IdTipoFactura == (int)EnumTiposFacturas.FacturaCreditoElectronicaMyPymesC)
                    {
                        if (pParametro.SolicitudPagoDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.AlicuotaIVA > 0))
                        {
                            pParametro.CodigoMensaje = "ValidarSolicitudDetallesIVA";
                            return false;
                        }
                    }
                    if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosCompras)
                    {
                        CtbPeriodosIvas periodoFiltro = new CtbPeriodosIvas();
                        periodoFiltro.Periodo = AyudaProgramacionLN.ObtenerPeriodo(pParametro.FechaContable.Value);
                        if (ContabilidadF.PeriodosIvasValidarCierre(periodoFiltro))
                        {
                            pParametro.CodigoMensaje = "ValidarFechaContablePeriodoIvaCerrado";
                            pParametro.CodigoMensajeArgs.Add(periodoFiltro.Periodo.ToString());
                            return false;
                        }
                        //CtbPeriodosContables periodoContable = new CtbPeriodosContables();
                        //periodoContable.Periodo = AyudaProgramacionLN.ObtenerPeriodo(pParametro.FechaContable.Value);
                        //if (ContabilidadF.PeriodosContablesValidarCierre(periodoContable))
                        //{
                        //    pParametro.CodigoMensaje = "ValidarFechaContablePeriodoIvaCerrado";
                        //    pParametro.CodigoMensajeArgs.Add(periodoContable.Periodo.ToString());
                        //    return false;
                        //}
                    }
                    else if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosTerceros)
                    {

                        //if (!pParametro.CuotasDescuentoAfiliado.HasValue || pParametro.CuotasDescuentoAfiliado.Value == 0)
                        //{
                        //    pParametro.CodigoMensaje = "ValidarCuotasDescuentoAfiliado";
                        //    return false;
                        //}
                        //if (!pParametro.CuotasPagoProveedor.HasValue || pParametro.CuotasPagoProveedor.Value == 0)
                        //{
                        //    pParametro.CodigoMensaje = "ValidarCuotasPagoProveedor";
                        //    return false;
                        //}
                        if (pParametro.FormaCobroAfiliado.IdFormaCobroAfiliado == 0)
                        {
                            pParametro.CodigoMensaje = "ValidarFormaCobroAfiliado";
                            return false;
                        }
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

        private bool ValidarSubsidio(CapSolicitudPago pParametro)
        {
            bool resultado = true;

            if (pParametro.SolicitudPagoDetalles.Count(x => x.EstadoColeccion == EstadoColecciones.Agregado) == 0)
            {
                pParametro.CodigoMensaje = "ValidarSolicitudDetalles";
                return false;
            }

            if (pParametro.SolicitudPagoDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.Cantidad <= 0))
            {
                pParametro.CodigoMensaje = "ValidarSolicitudDetallesCantidad";
                return false;
            }

            if (pParametro.SolicitudPagoDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Agregado && x.PrecioUnitarioSinIva <= 0))
            {
                pParametro.CodigoMensaje = "ValidarSolicitudDetallesPrecio";
                return false;
            }

            return resultado;
        }
        
        private bool ValidarAnulacion(CapSolicitudPago pParametro)
        {
            if (pParametro.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.SolicitudesPagosCompras
                && pParametro.TipoOperacion.Contabiliza)
            {
                CtbPeriodosIvas periodoFiltro = new CtbPeriodosIvas();
                periodoFiltro.Periodo = AyudaProgramacionLN.ObtenerPeriodo(pParametro.FechaContable.Value);
                if (ContabilidadF.PeriodosIvasValidarCierre(periodoFiltro))
                {
                    pParametro.CodigoMensaje = "ValidarAnulacionPeriodosIvasCerrado";
                    pParametro.CodigoMensajeArgs.Add(periodoFiltro.Periodo.ToString());
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Funcion que valida el Circuito de Aprobacion y cambia el estado
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        private bool CircuitoAprobacion(CapSolicitudPago pParametro)
        {
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.SolicitudComprasCircuitoAprobacion);
            bool bvalor = false;
            if (!string.IsNullOrEmpty(valor.ParametroValor) && bool.TryParse(valor.ParametroValor, out bvalor))
            {
                if (!bvalor)
                {
                    pParametro.Estado.IdEstado = (int)EstadosSolicitudesPagos.Autorizado;
                    pParametro.FechaAutorizacion = DateTime.Now;
                    pParametro.IdUsuarioAutorizacion = pParametro.IdUsuarioAlta;
                }
            }

            return true;
        }

        public bool AgregarSPRemesa(Objeto pObjeto, int pIdRemesa, int pPeriodo, decimal pImporte, Database db, DbTransaction tran)
        {
            bool resultado = true;

            TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ProveedorParaSPRemesaIAF);
            if (paramValor.IdParametroValor == 0)
            {
                pObjeto.CodigoMensaje = "ValidarParametroValorProveedorParaSPRemesaIAF";
                return false;
            }

            CapSolicitudPago sp = new CapSolicitudPago();
            sp.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
            sp.Entidad.IdRefEntidad = Convert.ToInt32(paramValor.ParametroValor);
            sp.TiposFacturas.IdTipoFactura = (int)EnumTiposFacturas.FacturasB;
            sp.PrefijoNumeroFactura = (1).ToString().PadLeft(4,'0');
            sp.NumeroFactura = pPeriodo.ToString().PadLeft(8,'0');
            sp.FechaFactura = DateTime.Now;
            sp.FechaContable = DateTime.Now;
            sp.ImporteSinIVA = pImporte;
            sp.ImporteTotal = pImporte;
            sp.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.SolicitudesPagosCompras);
            sp.TipoSolicitudPago.IdTipoSolicitudPago = (int)EnumTiposSolicitudPago.Compras;
            sp.IdRefTipoSolicitudPago = pIdRemesa;
            sp.IdFilial = (int)EnumTGEFiliales.SedeCentral;
            sp.FechaAlta = DateTime.Now;
            sp.IdUsuarioAlta = pObjeto.UsuarioLogueado.IdUsuarioEvento;
            sp.Estado.IdEstado = (int)EstadosSolicitudesPagos.Activo;
            sp.UsuarioLogueado = pObjeto.UsuarioLogueado;
            sp.EstadoColeccion = EstadoColecciones.Agregado;

            paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.CodigoProductoParaRemesaIAF);
            if (paramValor.IdParametroValor == 0)
            {
                pObjeto.CodigoMensaje = "ValidarParametroValorCodigoProductoParaRemesaIAF";
                return false;
            }

            CMPProductos producto = new CMPProductos();
            producto.IdProducto = Convert.ToInt32(paramValor.ParametroValor);
            producto = ComprasF.ProductosObtenerPorIdProducto(producto);

            CapSolicitudPagoDetalles itemSP = new CapSolicitudPagoDetalles();
            itemSP.Producto = producto;
            itemSP.Descripcion = producto.Descripcion;
            itemSP.Cantidad = 1;
            itemSP.PrecioUnitarioSinIva = pImporte;
            itemSP.Estado.IdEstado = (int)Estados.Activo;
            itemSP.Filial.IdFilial = 1;
            itemSP.UsuarioLogueado = pObjeto.UsuarioLogueado;
            itemSP.EstadoColeccion = EstadoColecciones.Agregado;

            sp.SolicitudPagoDetalles.Add(itemSP);

            if (!this.Agregar(sp, db, tran))
            {
                resultado = false;
                AyudaProgramacionLN.MapearError(sp, pObjeto);
            }

            return resultado;
        }

        public DataTable CargaMasivaSeleccionarTabla(CapSolicitudPago pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapSolicitudPagoCargaMasivaSeleccionarTabla", pParametro);
        }

        public bool AgregarCargaMasiva(DataTable e, CapSolicitudPago pParametro)
        {
            //sp.IdUsuarioAlta = pObjeto.UsuarioLogueado.IdUsuarioEvento; !!!!!!!!!!

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            //pParametro.Estado.IdEstado = (int)EstadosSolicitudesPagos.Activo;
            pParametro.TipoSolicitudPago.IdTipoSolicitudPago = (int)EnumTiposSolicitudPago.Compras;
            if (pParametro.Estado.IdEstado == (int)EstadosSolicitudesPagos.Autorizado)
            {
                pParametro.FechaAutorizacion = DateTime.Now;
                pParametro.IdUsuarioAutorizacion = pParametro.UsuarioLogueado.IdUsuarioEvento;
            }
           // pParametro.FechaAlta = DateTime.Now.(;

            e.TableName = "SolicitudPagoCargaMasiva";
            pParametro.LoteSolicitudPagoCargaMasiva = e.ToXmlDocument();

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "CapSolicitudesPagosCargaMasivaValidaciones");
                    //if (resultado)
                    //{
                    //    int result = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CapSolicitudesPagosCargaMasivaInsertar");
                    //    if (result == 0)
                    //    {
                    //        resultado = false;
                    //        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    //    }
                    //}
                    if (resultado && !BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pParametro, bd, tran, "CapSolicitudesPagosCargaMasivaInsertar"))
                        resultado = false;
                    //if (pParametro.IdSolicitudPago == 0)
                    //    resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.CodigoMensaje = pParametro.CodigoMensaje == string.Empty ? "ResultadoTransaccionIncorrecto" : pParametro.CodigoMensaje;
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
                return resultado;
            }
        }

        #region "Solicitud Detalles"

        private bool ActualizarDetalles(CapSolicitudPago pParametro, CapSolicitudPago pValorViejo, Database db, DbTransaction tran)
        {
            foreach (CapSolicitudPagoDetalles Detalle in pParametro.SolicitudPagoDetalles)
            {
                switch (Detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        Detalle.Estado.IdEstado = (int)Estados.Activo;
                        Detalle.IdSolicitudPago = pParametro.IdSolicitudPago;

                        if (!ValidarDetalle(Detalle))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        Detalle.IdSolicitudPagoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, db, tran, "CapSolicitudPagoDetalleInsertar");

                        if (Detalle.IdSolicitudPagoDetalle == 0)
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        //ACTUALIZO LA RELACION DE SOL PAGO CON INFORME RECEPCION
                        foreach (CmpInformesRecepcionesDetalles informeDet in Detalle.InformesDetalles)
                        {
                            CapSolicitudPagoDetalleInformesRecpecionesDetalles relacion = new CapSolicitudPagoDetalleInformesRecpecionesDetalles();
                            relacion.IdSolicitudPagoDetalle = Detalle.IdSolicitudPagoDetalle;
                            relacion.IdInformeRecepcionDetalle = informeDet.IdInformeRecepcionDetalle;
                            //uso la cantidad pendiente porque al generar la solicitud del pago la genero por todo el item, 
                            //Generalmente CantidadPendiente = CantidadRecibida pero queda la posibilidad de cambiarlo a futuro
                            relacion.Cantidad = informeDet.CantidadPendiente;
                            relacion.IdSolicitudPagoDetalleInformeRecpecionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(relacion, db, tran, "CapSolicitudPagoDetalleInformesRecpecionesDetallesInsertar");
                            if (relacion.IdSolicitudPagoDetalleInformeRecpecionDetalle == 0)
                            {
                                AyudaProgramacionLN.MapearError(relacion, pParametro);
                                return false;
                            }
                        }

                        #region Orden de Compra
                        //if (Detalle.OrdenCompraDetalle.IdOrdenCompraDetalle > 0
                        //    && Detalle.OrdenCompraDetalle.OrdenCompra.TipoOrdenCompra.IdTipoOrdenCompra == (int)EnumTiposOrdenesCompras.Terceros)
                        //{
                        //    ocdSpd = new CmpOrdenesComprasDetallesSolicitudPagoDetalle();
                        //    ocdSpd.IdSolicitudPagoDetalle = Detalle.IdSolicitudPago;
                        //    ocdSpd.IdOrdenCompraDetalle = Detalle.IdSolicitudPagoDetalle;
                        //    ocdSpd.UsuarioLogueado = pParametro.UsuarioLogueado;
                        //    ocdSpd.IdOrdenCompraDetalleSolicitudPagoDetalle = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, db, tran, "CmpOrdenesComprasDetallesSolicitudPagoDetalleInsertar");
                        //    if (ocdSpd.IdOrdenCompraDetalleSolicitudPagoDetalle == 0)
                        //    {
                        //        AyudaProgramacionLN.MapearError(ocdSpd, pParametro);
                        //        return false;
                        //    }

                        //    //Actualizo la Orden de compra Detalle con lo Facturado
                        //    ordenCompraDetalle = new CmpOrdenesComprasDetalles();
                        //    ordenCompraDetalle = AyudaProgramacionLN.Clone<CmpOrdenesComprasDetalles>(Detalle.OrdenCompraDetalle);
                        //    ordenCompraDetalle.CantidadPagada = ordenCompraDetalle.CantidadPagada.HasValue ? ordenCompraDetalle.CantidadPagada + Detalle.Cantidad : Detalle.Cantidad;
                        //    ordenCompraDetalle.UsuarioLogueado = pParametro.UsuarioLogueado;

                        //    if (!ComprasF.OrdenCompraDetalleActualizarDetalle(ordenCompraDetalle, Detalle.OrdenCompraDetalle, db, tran))
                        //    {
                        //        AyudaProgramacionLN.MapearError(ordenCompraDetalle, pParametro);
                        //        return false;
                        //    }

                        //    ////Actualizo la Orden de Compra
                        //    ////Las OC de Terceros solo admiten un item.
                        //    //Detalle.OrdenCompraDetalle.OrdenCompra.Estado.IdEstado = (int)EstadosOrdenesCompras.Pagado;
                        //    //Detalle.OrdenCompraDetalle.OrdenCompra.UsuarioLogueado = pParametro.UsuarioLogueado;
                        //    //if (!ComprasF.OrdenCompraModificarEstado(Detalle.OrdenCompraDetalle.OrdenCompra, db, tran))
                        //    //{
                        //    //    AyudaProgramacionLN.MapearError(Detalle.OrdenCompraDetalle.OrdenCompra, pParametro);
                        //    //    return false;
                        //    //}

                        //    //Genero los Cargos a Descontar al Socio
                        //    CarTiposCargosAfiliadosFormasCobros cargoAfiliado = new CarTiposCargosAfiliadosFormasCobros();
                        //    cargoAfiliado.IdAfiliado = Detalle.OrdenCompraDetalle.OrdenCompra.Afiliado.IdAfiliado;
                        //    cargoAfiliado.TipoCargo.IdTipoCargo = (int)EnumTiposCargos.OrdenesComprasTerceros;
                        //    cargoAfiliado.FormaCobroAfiliado = Detalle.OrdenCompraDetalle.OrdenCompra.FormaCobroAfiliado;
                        //    cargoAfiliado.FechaAlta = DateTime.Now;
                        //    cargoAfiliado.Estado.IdEstado = (int)EstadosCargos.Activo;
                        //    cargoAfiliado.UsuarioAlta.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        //    cargoAfiliado.UsuarioLogueado = pParametro.UsuarioLogueado;
                        //    cargoAfiliado.CantidadCuotas = Detalle.OrdenCompraDetalle.OrdenCompra.CuotasDescuentoAfiliado.Value;
                        //    cargoAfiliado.ImporteCuota = Math.Round(Detalle.PrecioTotalItem / cargoAfiliado.CantidadCuotas, 2);
                        //    cargoAfiliado.ImporteTotal = Detalle.PrecioTotalItem;
                        //    cargoAfiliado.IdReferenciaRegistro = Detalle.IdSolicitudPagoDetalle;
                        //    cargoAfiliado.TablaReferenciaRegistro = Detalle.GetType().Name;
                        //    cargoAfiliado.Detalle = string.Concat("Nro: ", Detalle.OrdenCompraDetalle.OrdenCompra.IdOrdenCompra.ToString(), " ", pParametro.Entidad.Nombre);

                        //    if (!CargosF.TiposCargosAfiliadosAgregar(cargoAfiliado, db, tran))
                        //    {
                        //        AyudaProgramacionLN.MapearError(cargoAfiliado, pParametro);
                        //        return false;
                        //    }
                        //}
                        #endregion
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        Detalle.SelloTiempo = BaseDatos.ObtenerBaseDatos().ObtenerSelloTiempo("CapSolicitudPagoDetalleSeleccionar", Detalle, db, tran);
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CapSolicitudPagoDetalleActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.SolicitudPagoDetalles.Find(x => x.IdSolicitudPagoDetalle == Detalle.IdSolicitudPagoDetalle), Acciones.Update, Detalle, db, tran))
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

        private bool ValidarDetalle(CapSolicitudPagoDetalles detalle)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(detalle, "CapSolicitudPagoValidarDetalle");
        }

        private bool ActualizarTiposPercepciones(CapSolicitudPago pParametro, CapSolicitudPago pValorViejo, Database db, DbTransaction tran)
        {
            foreach (CapSolicitudPagoTipoPercepcion Detalle in pParametro.SolicitudPagoTiposPercepciones)
            {
                switch (Detalle.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        Detalle.Estado.IdEstado = (int)Estados.Activo;
                        Detalle.IdSolicitudPago = pParametro.IdSolicitudPago;
                        Detalle.IdSolicitudPagoTipoPercepcion = BaseDatos.ObtenerBaseDatos().Agregar(Detalle, db, tran, "CapSolicitudPagoTipoPercepcionInsertar");
                        if (Detalle.IdSolicitudPagoTipoPercepcion == 0)
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        Detalle.UsuarioLogueado = pParametro.UsuarioLogueado;           //CREAR STORE  ¬
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(Detalle, db, tran, "CapSolicitudPagoTipoPercepcionActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(Detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.SolicitudPagoTiposPercepciones.Find(x => x.IdSolicitudPagoTipoPercepcion == Detalle.IdSolicitudPagoTipoPercepcion), Acciones.Update, Detalle, db, tran))
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

        #region Subsidios

        public bool ObtenerImporteLiquidacion(CapSolicitudPago pParametro)
        {
            bool resultado = true;
            TGECampos campo;

            ////SubSubsidios subsidio = SubsidiosF.SubsidiosObtenerDatosCompletos(pParametro.SubSidio);

            //campo = pParametro.Campos.Find(x => x.Nombre == "CausanteBeneficio");
            //if (campo != null)
            //{
            //    if (campo.CampoValor.Valor == string.Empty)
            //    {
            //        pParametro.CodigoMensaje = "ValidarCampoCausanteBeneficio";
            //        resultado = false;
            //    }
            //    else
            //    {
            //        pParametro.SolicitudPagoCausanteBeneficio.IdAfiliado = Convert.ToInt32(campo.CampoValor.Valor);
            //        pParametro.Afiliado.IdAfiliado = Convert.ToInt32(campo.CampoValor.Valor);
            //    }
            //}

            if (!TGEGeneralesF.CamposValidar(pParametro, pParametro.Campos))
                return false;

            //Busco la FechaEventoSubsidio             CAMBIE FECHAEVENTOSUBSIDIO POR FECHAEVENTO
            campo = pParametro.Campos.Find(x => x.Nombre == "FechaEvento" && x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.DateTime);
            if (campo != null)
            {
                pParametro.FechaEventoSubsidio = campo.CampoValor.Valor == string.Empty ? default(DateTime?) : Convert.ToDateTime(campo.CampoValor.Valor);
            }
            //Busco la Cantidad Solicitada
            campo = pParametro.Campos.Find(x => x.Nombre == "CantidadSolicitada" && x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.IntegerTextBox);
            if (campo != null)
            {
                pParametro.CantidadSolicitada = campo.CampoValor.Valor==string.Empty ? 0 : Convert.ToInt32(campo.CampoValor.Valor);
            }
            else
                pParametro.CantidadSolicitada = 1;

            //Busco el Causante Beneficio
            campo = pParametro.Campos.Find(x => x.Nombre == "CausanteBeneficio" && 
                (x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.DropDownList
                || x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.DropDownListQuery
                || x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.DropDownListSP
                || x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.DropDownListSPAutoComplete
                || x.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.ComboBoxSP));
            if (campo != null)
            {
                pParametro.SolicitudPagoCausanteBeneficio.IdAfiliado = campo.CampoValor.Valor ==string.Empty? 0 : Convert.ToInt32(campo.CampoValor.Valor);
                pParametro.Afiliado.IdAfiliado = pParametro.SolicitudPagoCausanteBeneficio.IdAfiliado;
            }
            CapSolicitudPago validar = BaseDatos.ObtenerBaseDatos().Obtener<CapSolicitudPago>("CapSolictuPagoSubsidiosValidaciones", pParametro);

            if (validar.CodigoMensaje.Trim() != string.Empty)
            {
                resultado = false;
                if (validar.ErrorException.Length > 0)
                {
                    string[] argError = validar.ErrorException.Split(',');
                    validar.CodigoMensajeArgs.AddRange(argError.ToList());
                }
                AyudaProgramacionLN.MapearError(validar, pParametro);
            }
            else
            {
                pParametro.SolicitudPagoDetalles[0].PrecioUnitarioSinIva = validar.ImporteTotal;
                pParametro.SolicitudPagoDetalles[0].Cantidad = pParametro.CantidadSolicitada.Value;
            }
            pParametro.ImporteTotal = pParametro.SolicitudPagoDetalles.Sum(x => x.PrecioTotalItem);
            return resultado;
        }

        private bool ActualizarCausanteBeneficio(CapSolicitudPago pParametro, Database db, DbTransaction tran)
        {
            if (pParametro.SolicitudPagoCausanteBeneficio.IdAfiliado > 0 || pParametro.SolicitudPagoCausanteBeneficio.NumeroDocumento > 0)
            {
            pParametro.SolicitudPagoCausanteBeneficio.IdSolicitudPago = pParametro.IdSolicitudPago;
            switch (pParametro.EstadoColeccion)
            {
                #region "Agregado"
                case EstadoColecciones.Agregado:
                    pParametro.SolicitudPagoCausanteBeneficio.IdSolicitudPagoCausanteBeneficio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro.SolicitudPagoCausanteBeneficio, db, tran, "CapSolicitudPagoCausantesBeneficiosInsertar");
                    if (pParametro.SolicitudPagoCausanteBeneficio.IdSolicitudPagoCausanteBeneficio == 0)
                    {
                        AyudaProgramacionLN.MapearError(pParametro.SolicitudPagoCausanteBeneficio, pParametro);
                        return false;
                    }
                    break;
                #endregion

                #region "Modificado"
                case EstadoColecciones.Modificado:
                    break;
                #endregion
            }
            }
            return true;
        }

        #endregion

        #region SOLICITUDES PAGOS ANTICIPOS A PROVEEDORES
        public CapSolicitudPago ObtenerDatosCompletosAnticipos(CapSolicitudPago pParametro)
        {
            CapSolicitudPago solicitud = BaseDatos.ObtenerBaseDatos().Obtener<CapSolicitudPago>("CapSolicitudPagoSeleccionar", pParametro);
            solicitud.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            solicitud.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return solicitud;
        }

        public bool AgregarAnticipo(CapSolicitudPago pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            //VALIDO IMPORTE MAYOR QUE CERO 
            if (pParametro.ImporteTotal <= 0)
            {
                pParametro.CodigoMensaje = "ValidarImporteMayorCero";
                return false;
            }

            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)EstadosSolicitudesPagos.Autorizado;

            if (!this.CircuitoAprobacion(pParametro))
                return false;
            
            pParametro.FechaEvento = DateTime.Now;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.Entidad.IdEntidad = (int)EnumTGEEntidades.Proveedores;
            pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuario;
            pParametro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(pParametro.TipoOperacion);
            
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdSolicitudPago = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CapSolicitudPagoInsertar");
                    if (pParametro.IdSolicitudPago == 0)
                        resultado = false;

                    if (resultado && pParametro.TipoOperacion.Contabiliza && !new InterfazContableLN().AgregarSolicitudPagoAnticipo(pParametro, bd, tran))
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
        #endregion

        private bool BorrarRelacionSolicitudDetallesInformesRecepcion(CapSolicitudPago pParametro, Database db, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacion(pParametro, db, tran, "CapSolicitudPagoDetalleInformesRecpecionesDetallesBorrarPorSolicitud");
        }

        #region Comprobantes internos
        public List<CapSolicitudPago> ComprobantesInternosSeleccionarPorFechaDesde(CtbPeriodosIvas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapComprobantesInternosSeleccionarFiltro", pParametro);
        }

        public bool ModificarComprobantesInternos(List<CapSolicitudPago> pParametro, Objeto objeto)
        {
            AyudaProgramacionLN.LimpiarMensajesError(objeto);

            bool resultado = true;

            //if (!this.Validar(pParametro, valorViejo))
            //    return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapSolicitudPago valorViejo;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    CtbAsientosContables asientoViejo;
                    foreach (CapSolicitudPago solicitud in pParametro.Where(x => x.EstadoColeccion == EstadoColecciones.Modificado).ToList())
                    {
                        //Obtengo el valor actual del objeto antes de modificarlo
                        //para el Historial de Auditoria
                        valorViejo = new CapSolicitudPago();
                        valorViejo.IdSolicitudPago = solicitud.IdSolicitudPago;
                        valorViejo.UsuarioLogueado = objeto.UsuarioLogueado;
                        valorViejo = this.ObtenerDatosCompletos(valorViejo, bd, tran);

                        solicitud.UsuarioLogueado = objeto.UsuarioLogueado;
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(solicitud, bd, tran, "CapComprobantesInternosActualizar"))
                        {
                            resultado = false;
                            break;
                        }

                        if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, solicitud, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(valorViejo, objeto);
                            resultado = false;
                            break;
                        }

                        #region Actualizacion de Asiento Contable
                        if (resultado)
                        {
                            asientoViejo = new CtbAsientosContables();
                            asientoViejo.IdTipoOperacion = solicitud.TipoOperacion.IdTipoOperacion;
                            asientoViejo.IdRefTipoOperacion = solicitud.IdSolicitudPago;
                            asientoViejo = ContabilidadF.AsientosContablesObtenerDatosCompletosPorTipoOperacion(asientoViejo, bd, tran);
                            if (asientoViejo.IdAsientoContable == 0)
                                continue;

                            asientoViejo.FechaAsiento = solicitud.FechaContable.Value;
                            asientoViejo.UsuarioLogueado = objeto.UsuarioLogueado;
                            ////ACTUALIZO ASIENTO MODIFICADO
                            if (!ContabilidadF.AsientosContablesModificar(asientoViejo, bd, tran))
                            {
                                AyudaProgramacionLN.MapearError(asientoViejo, objeto);
                                resultado = false;
                                break;
                            }

                        }
                        #endregion

                    }
                    if (resultado)
                    {
                        tran.Commit();
                        objeto.CodigoMensaje = "ResultadoTransaccion";
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
                    objeto.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    objeto.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }
        #endregion

        public List<CapSolicitudPago> ObtenerComboAsociados(CapSolicitudPago pSolicitudPago)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("CapSolicitudPagoSeleccionarComboAsociados", pSolicitudPago);
        }

        public List<CapSolicitudPago> ObtenerListaFiltroComboAsociados(CapSolicitudPago pSolicitudPago)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapSolicitudPago>("VTAFacturasSeleccionarComboAsociadosPorFiltro", pSolicitudPago);
        }

        #region Turismo
        public DataTable ObtenerAnticiposReservasTurismoPendientes(CapProveedores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CarTiposCargosAfiliadosFormasCobrosObtenerReservasTurismoPendiente", pParametro);
        }

        public bool AgregarAnticiposTurismo(Objeto pResultado, DataTable Datos)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pResultado);
            bool resultado = true;

            Datos.TableName = "AnticiposTurismo";
            pResultado.LoteXML = Datos.ToXmlDocument();

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().EjecutarSPActualizacionDataSetResultado(pResultado, bd, tran, "CapSolicitudPagoInsertarAnticiposTurismo");
                   
                    if (resultado)
                    {
                        tran.Commit();
                        pResultado.CodigoMensaje = "ResultadoTransaccion";
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
                    pResultado.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pResultado.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public DataTable ObtenerGrillaAnticipoReservas(CapSolicitudPago pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapSolicitudPagoObtenerAnticipoReservas", pParametro);
        }

        public bool ReimputarAnticipo(CapSolicitudPago pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            
            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    pParametro.AnticiposReimputarXML = new XmlDocument();
                    pParametro.AnticiposReimputarXML.LoadXml(this.ArmarXML(pParametro));

                    if (!this.Validaciones(pParametro))
                        return false;

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapSolicitudPagoReimputarAnticipo"))
                    {
                        resultado = false;
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
        private string ArmarXML(CapSolicitudPago pParametro)
        {
            string XML = "<Detalles>";
            foreach (CapSolicitudPago item in pParametro.AnticiposReimputar)
            {
                XML = string.Concat(XML, "<Detalle>" +
                    "<IdRefTabla>", item.IdRefTabla, "</IdRefTabla>" +
                    "<ImporteAImputar>", item.ImporteTotal, "</ImporteAImputar>", "</Detalle>");
            }
            return string.Concat(XML, "</Detalles>");
        }
        private bool Validaciones(CapSolicitudPago pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "CapSolicitudPagoReimputarAnticipoValidaciones");
        }
        #endregion
    }
}

        
