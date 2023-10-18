using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Prestamos.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;
using System.Xml;

namespace Prestamos.LogicaNegocio
{
    public class PrePrestamosCesionesLN : BaseLN<PrePrestamosCesiones>
    {
        public override PrePrestamosCesiones ObtenerDatosCompletos(PrePrestamosCesiones pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosCesiones>("PrePrestamosCesionesSeleccionar", pParametro);
            pParametro.PrestamosCesionesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCesionesDetalles>("PrePrestamosCesionesDetallesSeleccionarPorPrePrestamosCesiones", pParametro);
            pParametro.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<PrePrestamosCesiones> ObtenerListaFiltro(PrePrestamosCesiones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCesiones>("PrePrestamosCesionesSeleccionarFiltro", pParametro);
        }

        public List<PrePrestamosCesionesDetalles> ObtenerPrestamosDisponibles(PrePrestamosCesionesDetalles pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<PrePrestamosCesionesDetalles>("PrePrestamosCesionesDetallesSeleccionarDisponibles", pParametro);
        }

        public bool CalcularVAN(PrePrestamosCesiones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            List<PrePrestamosCesionesDetalles> lista = pParametro.PrestamosCesionesDetalles.FindAll(x => x.Incluir);

            if (lista.Count==0)
            {
                pParametro.CodigoMensaje = "ValidarPrestamosCesionesCantidadCalcularVAN";
                return false;
            }

            pParametro.LotePrestamos = new XmlDocument();
            //XmlNode docNode = pParametro.LotePrestamos.CreateXmlDeclaration("1.0", "UTF-8", null);
            //pParametro.LotePrestamos.AppendChild(docNode);

            XmlNode prestamosNode = pParametro.LotePrestamos.CreateElement("PrePrestamos");
            pParametro.LotePrestamos.AppendChild(prestamosNode);

            XmlNode prestamoNode;
            XmlAttribute prestamoAttribute;
            foreach (PrePrestamosCesionesDetalles pre in lista)
            {
                prestamoNode = pParametro.LotePrestamos.CreateElement("PrePrestamo");
                prestamoAttribute = pParametro.LotePrestamos.CreateAttribute("IdPrestamo");
                prestamoAttribute.Value = pre.IdPrestamo.ToString();
                prestamoNode.Attributes.Append(prestamoAttribute);
                prestamosNode.AppendChild(prestamoNode);

                //XmlNode nameNode = pParametro.LotePrestamos.CreateElement("IdPrestamo");
                //nameNode.AppendChild(pParametro.LotePrestamos.CreateTextNode(pre.IdPrestamo.ToString()));
                //prestamoNode.AppendChild(nameNode);

            }

            PrePrestamosCesiones resultado = BaseDatos.ObtenerBaseDatos().Obtener<PrePrestamosCesiones>("[PrePrestamosCalculosSesionesVAN]", pParametro);
            pParametro.VAN = resultado.VAN;
            return true;
        }

        public override bool Agregar(PrePrestamosCesiones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;
            pParametro.Estado.IdEstado = (int)Estados.Activo;
            pParametro.FechaAlta = DateTime.Now;
            pParametro.Cantidad = pParametro.PrestamosCesionesDetalles.Count;
            pParametro.TotalAmortizacion = pParametro.PrestamosCesionesDetalles.Sum(x => x.ImporteAmortizacion);
            pParametro.TotalInteres = pParametro.PrestamosCesionesDetalles.Sum(x => x.ImporteInteres);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //Guardo el Prestamo
                    pParametro.IdPrestamoCesion = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "PrePrestamosCesionesInsertar");
                    if (pParametro.IdPrestamoCesion == 0)
                        resultado = false;

                    if (resultado && !this.PrestamosCesionesActualizar(pParametro, new PrePrestamosCesiones(), bd, tran))
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

        private bool PrestamosCesionesActualizar(PrePrestamosCesiones pParametro, PrePrestamosCesiones valorViejo, Database bd, DbTransaction tran)
        {
            foreach (PrePrestamosCesionesDetalles detalle in pParametro.PrestamosCesionesDetalles)
            {
                switch (detalle.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        detalle.IdPrestamoCesion = pParametro.IdPrestamoCesion;
                        detalle.IdPrestamoCesionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(detalle, bd, tran, "PrePrestamosCesionesDetallesInsertar");
                        if (detalle.IdPrestamoCesionDetalle == 0)
                            return false;
                        break;
                    #endregion
                    #region Modificado
                    case EstadoColecciones.Modificado:
                        detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, bd, tran, "PrePrestamosCesionesDetallesActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(detalle, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            valorViejo.PrestamosCesionesDetalles.Find(x => x.IdPrestamoCesionDetalle == detalle.IdPrestamoCesionDetalle), Acciones.Update, detalle, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(detalle, pParametro);
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

        public override bool Modificar(PrePrestamosCesiones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            PrePrestamosCesiones valorViejo = new PrePrestamosCesiones();
            valorViejo.IdPrestamoCesion = pParametro.IdPrestamoCesion;
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
                    if (resultado && !this.Modificar(pParametro, valorViejo, bd, tran))
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
                    return false;
                }
            }
            return resultado;
        }

        public bool Autorizar(PrePrestamosCesiones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            PrePrestamosCesiones valorViejo = new PrePrestamosCesiones();
            valorViejo.IdPrestamoCesion = pParametro.IdPrestamoCesion;
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
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "PrePrestamosCesionesValidarAutorizar"))
                        resultado = false;

                    if (resultado && !this.Modificar(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !new InterfazContableLN().AsientoCesiones(pParametro, bd, tran))
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
                    return false;
                }
            }
            return resultado;
        }

        private bool Modificar(PrePrestamosCesiones pParametro, PrePrestamosCesiones valorViejo, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "PrePrestamosCesionesActualizar"))
                resultado = false;

            if (resultado && !this.PrestamosCesionesActualizar(pParametro, valorViejo, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                resultado = false;

            return resultado;
        }
    }
}
