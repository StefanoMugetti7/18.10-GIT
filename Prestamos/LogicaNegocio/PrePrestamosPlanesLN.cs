using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Prestamos.Entidades;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using System.Collections;
using Generales.Entidades;
using System.Data;
using System.Xml;

namespace Prestamos.LogicaNegocio
{
    class PrePrestamosPlanesLN : BaseLN<PrePrestamosPlanes>
    {

        public override PrePrestamosPlanes ObtenerDatosCompletos(PrePrestamosPlanes pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosPlanes>("PrePrestamosPlanesSeleccionar", pParametro);
            pParametro.PrestamosPlanesTasas = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosPlanesTasas>("PrePrestamosPlanesTasasSeleccionarPorPlan", pParametro);
            pParametro.FormasCobros = BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEFormasCobros>("[PrePrestamosPlanesFormasCobrosSeleccionarPorCargos]", pParametro);
            pParametro.TiposOperaciones = BaseDatos.ObtenerBaseDatos().ObtenerLista<TGETiposOperaciones>("PrePrestamosPlanesTiposOperacionesSeleccionarPorPlan",pParametro);
            pParametro.PrestamosIpsPlanes = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosIpsPlanes>("PrePrestamosIpsPlanesSeleccionarPorPlan", pParametro);
            pParametro.PrestamosBancoSolParametros = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosBancoSolParametros>("PrePrestamosBancoSolParametrosSeleccionarPorPlan", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<PrePrestamosPlanes> ObtenerListaFiltro(PrePrestamosPlanes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosPlanes>("PrePrestamosPlanesListar", pParametro);
        }

        public DataTable ObtenerPrestamosBancoSolParametros(PrePrestamosPlanes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("PrePrestamosBancoSolParametrosSeleccionarPorPlanPlantilla", pParametro);
        }

        /// <summary>
        /// Devuelve la ultima Tasa para el Plan
        /// </summary>
        /// <param name="pParametro">IdPrestamoPlan</param>
        /// <returns></returns>
        public PrePrestamosPlanesTasas ObtenerTasaActiva(PrePrestamosPlanes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosPlanesTasas>("PrePrestamosPlanesTasasSeleccionarPorPrestamoPlan", pParametro);
        }

        public override bool Agregar(PrePrestamosPlanes pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdPrestamoPlan = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "PrePrestamosPlanesInsertar");
                    if (pParametro.IdPrestamoPlan == 0)
                        resultado = false;

                    if (resultado && !this.ActualizarPrestamosPlanesTasas(pParametro, new PrePrestamosPlanes(), bd, tran))
                        resultado = false;

                    if (resultado && !this.GuardarCargosFormasCobros(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.GuardarTiposOperaciones(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestamosIpsPlanes(pParametro, new PrePrestamosPlanes(), bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestamosBancoSolParametros(pParametro, new PrePrestamosPlanes(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        public override bool Modificar(PrePrestamosPlanes pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            PrePrestamosPlanes valorViejo = new PrePrestamosPlanes();
            valorViejo.IdPrestamoPlan = pParametro.IdPrestamoPlan;
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
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "PrePrestamosPlanesActualizar"))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestamosPlanesTasas(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.GuardarCargosFormasCobros(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.GuardarTiposOperaciones(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestamosIpsPlanes(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.ActualizarPrestamosBancoSolParametros(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        private bool ActualizarPrestamosPlanesTasas(PrePrestamosPlanes pParametro, PrePrestamosPlanes valorViejo, Database bd, DbTransaction tran)
        {
            foreach (PrePrestamosPlanesTasas planTasa in pParametro.PrestamosPlanesTasas)
            {
                switch (planTasa.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        planTasa.IdPrestamoPlan = pParametro.IdPrestamoPlan;
                        planTasa.IdPrestamoPlanTasa = BaseDatos.ObtenerBaseDatos().Agregar(planTasa, bd, tran, "PrePrestamosPlanesTasasInsertar");
                        if (planTasa.IdPrestamoPlanTasa == 0)
                            return false;
                        break;
                    #endregion
                    case EstadoColecciones.Borrado:
                        break;
                    #region Modificado
                    case EstadoColecciones.Modificado:
                        planTasa.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(planTasa, bd, tran, "PrePrestamosPlanesTasasActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(planTasa, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            valorViejo.PrestamosPlanesTasas.Find(x => x.IdPrestamoPlanTasa == planTasa.IdPrestamoPlanTasa), Acciones.Update, planTasa, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(planTasa, pParametro);
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

        private bool GuardarCargosFormasCobros(PrePrestamosPlanes pParametro, Database pBd, DbTransaction pTran)
        {
            //bool resultado = true;
            string sp = string.Empty;
            Hashtable param = new Hashtable();
            HistorialCambios cambio = new HistorialCambios();
            cambio.CampoCambiado = "TipoCargo -> FormaCobro";
            foreach (TGEFormasCobros fp in pParametro.FormasCobros)
            {
                param = new Hashtable();
                param.Add("IdPrestamoPlan", pParametro.IdPrestamoPlan);
                param.Add("IdFormaCobro", fp.IdFormaCobro);
                cambio.ValorViejo = string.Empty;
                cambio.ValorNuevo = string.Empty;

                switch (fp.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        sp = "PrePrestamosPlanesFormasCobrosInsertar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorNuevo = fp.FormaCobro;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Insert, pBd, pTran))
                            return false;
                        break;
                    case EstadoColecciones.Borrado:
                        sp = "PrePrestamosPlanesFormasCobrosBorrar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorViejo = fp.FormaCobro;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Delete, pBd, pTran))
                            return false;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool GuardarTiposOperaciones(PrePrestamosPlanes pParametro, Database pBd, DbTransaction pTran)
        {
            //bool resultado = true;
            string sp = string.Empty;
            Hashtable param = new Hashtable();
            HistorialCambios cambio = new HistorialCambios();
            cambio.CampoCambiado = "Prestamos Planes -> TipoOperacion";
            foreach (TGETiposOperaciones to in pParametro.TiposOperaciones)
            {
                param = new Hashtable();
                param.Add("IdPrestamoPlan", pParametro.IdPrestamoPlan);
                param.Add("IdTipoOperacion", to.IdTipoOperacion);
                cambio.ValorViejo = string.Empty;
                cambio.ValorNuevo = string.Empty;

                switch (to.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        sp = "PrePrestamosPlanesTiposOperacionesInsertar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorNuevo = to.TipoOperacion;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Insert, pBd, pTran))
                            return false;
                        break;
                    case EstadoColecciones.Borrado:
                        sp = "PrePrestamosPlanesTiposOperacionesBorrar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorViejo = to.TipoOperacion;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Delete, pBd, pTran))
                            return false;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }

        private bool ActualizarPrestamosIpsPlanes(PrePrestamosPlanes pParametro, PrePrestamosPlanes valorViejo, Database bd, DbTransaction tran)
        {
            foreach (PrePrestamosIpsPlanes planIps in pParametro.PrestamosIpsPlanes)
            {
                switch (planIps.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        planIps.UsuarioLogueado = pParametro.UsuarioLogueado;
                        planIps.IdPlan = pParametro.IdPrestamoPlan;
                        planIps.IdPrestamoIpsPlan = BaseDatos.ObtenerBaseDatos().Agregar(planIps, bd, tran, "PrePrestamosIpsPlanesInsertar");
                        if (planIps.IdPrestamoIpsPlan == 0)
                            return false;
                        break;
                    #endregion
                    case EstadoColecciones.Borrado:
                        break;
                    #region Modificado
                    case EstadoColecciones.Modificado:
                        planIps.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(planIps, bd, tran, "PrePrestamosIpsPlanesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(planIps, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            valorViejo.PrestamosIpsPlanes.Find(x => x.IdPrestamoIpsPlan == planIps.IdPrestamoIpsPlan), Acciones.Update, planIps, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(planIps, pParametro);
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

        private bool ActualizarPrestamosBancoSolParametros(PrePrestamosPlanes pParametro, PrePrestamosPlanes valorViejo, Database bd, DbTransaction tran)
        {
            if (pParametro.PrestamosBancoSolParametros.Count(x=>x.EstadoColeccion==EstadoColecciones.Agregado) == 0)
                return true;

            pParametro.LoteXML = new XmlDocument();
            //XmlNode docNode = pParametro.LotePrestamos.CreateXmlDeclaration("1.0", "UTF-8", null);
            //pParametro.LotePrestamos.AppendChild(docNode);

            XmlNode nodoraiz = pParametro.LoteXML.CreateElement("PrePrestamosBancoSolParametros");
            pParametro.LoteXML.AppendChild(nodoraiz);

            XmlNode nodo;
            XmlAttribute attribute;
            foreach (PrePrestamosBancoSolParametros pre in pParametro.PrestamosBancoSolParametros)
            {
                nodo = pParametro.LoteXML.CreateElement("PrePrestamosBancoSolParametro");
                
                attribute = pParametro.LoteXML.CreateAttribute("CantidadCuotas");
                attribute.Value = pre.CantidadCuotas.ToString();
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("Neto");
                attribute.Value = pre.Neto.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("Capital");
                attribute.Value = pre.Capital.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("Monto");
                attribute.Value = pre.Monto.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("ImporteCuota");
                attribute.Value = pre.ImporteCuota.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("ImporteSeguro.");
                attribute.Value = pre.ImporteSeguro.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("ImporteQuebrantoCuota");
                attribute.Value = pre.ImporteQuebrantoCuota.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("TotalCuota");
                attribute.Value = pre.TotalCuota.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("FondoQuebrantoNeto");
                attribute.Value = pre.FondoQuebrantoNeto.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("TasaAdm");
                attribute.Value = pre.TasaAdm.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("Sellado");
                attribute.Value = pre.Sellado.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("SueldoMinimo");
                attribute.Value = pre.SueldoMinimo.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("IdEstado");
                attribute.Value = pre.Estado.IdEstado.ToString();
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("CFT");
                attribute.Value = pre.CFT.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("TEA");
                attribute.Value = pre.TEA.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                attribute = pParametro.LoteXML.CreateAttribute("TNA");
                attribute.Value = pre.TNA.ToString().Replace(',', '.'); ;
                nodo.Attributes.Append(attribute);

                nodoraiz.AppendChild(nodo);
            }

            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "PrePrestamosBancoSolParametrosInsertarActualizar"))
            {
                return false;
            }                
            return true;
            #region backup
            //foreach (PrePrestamosBancoSolParametros item in pParametro.PrestamosBancoSolParametros)
            //{
            //    switch (item.EstadoColeccion)
            //    {
            //        #region Agregado
            //        case EstadoColecciones.Agregado:
            //            item.UsuarioLogueado = pParametro.UsuarioLogueado;
            //            item.IdPrestamoPlan = pParametro.IdPrestamoPlan;
            //            item.IdPrestamoBancoSolParametro = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "PrePrestamosBancoSolParametrosInsertar");
            //            if (item.IdPrestamoBancoSolParametro == 0)
            //                return false;
            //            break;
            //        #endregion
            //        case EstadoColecciones.Borrado:
            //            break;
            //        #region Modificado
            //        case EstadoColecciones.Modificado:
            //            item.UsuarioLogueado = pParametro.UsuarioLogueado;
            //            if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, bd, tran, "PrePrestamosBancoSolParametrosActualizar"))
            //            {
            //                AyudaProgramacionLN.MapearError(item, pParametro);
            //                return false;
            //            }
            //            if (!AuditoriaF.AuditoriaAgregar(
            //                valorViejo.PrestamosBancoSolParametros.Find(x => x.IdPrestamoBancoSolParametro == item.IdPrestamoBancoSolParametro), Acciones.Update, item, bd, tran))
            //            {
            //                AyudaProgramacionLN.MapearError(item, pParametro);
            //                return false;
            //            }
            //            break;
            //        #endregion
            //        default:
            //            break;
            //    }
            //}
            #endregion
        }
    }
}
