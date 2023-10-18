
using Ahorros.Entidades;
using Auditoria;
using Auditoria.Entidades;
using Bancos.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bancos.LogicaNegocio
{
    class TesPlazosFijosLN : BaseLN<TESPlazosFijos>
    {
        public override bool Agregar(TESPlazosFijos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true; 

            if (!this.ValidarAgregar(pParametro, new TESPlazosFijos()))
                return false;
          
            pParametro.FechaAlta = DateTime.Now;
         

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {

                    //Guardo el Plazo Fijo
                    pParametro.IdPlazoFijo = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "TesPlazosFijosInsertar");
                    if (pParametro.IdPlazoFijo == 0)
                        resultado = false;

                    if (resultado && !new InterfazContableLN().PlazosFijosPropiosAgregar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
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

      

        private bool ValidarAgregar(TESPlazosFijos pParametro, TESPlazosFijos pAnterior)
        {
        
            //if (pParametro.TasaInteres == 0)
            //{
            //    pParametro.CodigoMensaje = "ValidarTasaInteres";
            //    pParametro.CodigoMensajeArgs.Add(pParametro.TasaInteres.ToString("N2"));
            //    return false;
            //}

            if (pParametro.FechaInicioVigencia.Date > DateTime.Now.Date)
            {
                pParametro.CodigoMensaje = "ValidarFechaInicioVigencia";
                pParametro.CodigoMensajeArgs.Add(pParametro.FechaInicioVigencia.ToShortDateString());
                return false;
            }

         
            //if (pParametro.ImporteCapital == pAnterior.ImporteTotal)
            //{
            //    if (!pParametro.ConfirmarRenovar)
            //    {
            //        pParametro.ConfirmarAccion = true;
            //        pParametro.CodigoMensaje = "PlazoFijoConfirmarRenovar";
            //        pParametro.CodigoMensajeArgs.Add(pParametro.ImporteTotal.ToString("C2"));
            //        pParametro.CodigoMensajeArgs.Add(pParametro.FechaVencimiento.ToShortDateString());
            //        return false;
            //    }
            //}

            return true;
        }


        public override bool Modificar(TESPlazosFijos pParametro)
        {
           
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            TESPlazosFijos valorViejo = new TESPlazosFijos();
            valorViejo.IdPlazoFijo = pParametro.IdPlazoFijo;
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
                    //Guardo el Plazo Fijo
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TesPlazosFijosActualizar"))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    //if (resultado && pPlazoFijoAnterior.IdPlazoFijo > 0)
                    //{
                    //    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pPlazoFijoAnterior, bd, tran, "TesPlazosFijosActualizar"))
                    //    {
                    //        AyudaProgramacionLN.MapearError(pPlazoFijoAnterior, pParametro);
                    //        resultado = false;
                    //    }
                    //}

                    //Control Comentarios y Archivos
                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;
                    //Fin control Comentarios y Archivos

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


        public override TESPlazosFijos ObtenerDatosCompletos(TESPlazosFijos pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<TESPlazosFijos>("TesPlazosFijosSeleccionarDescripcion", pParametro);
            pParametro.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return pParametro;
        }

        public override List<TESPlazosFijos> ObtenerListaFiltro(TESPlazosFijos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TESPlazosFijos>("TesPlazosFijosSeleccionarFiltro", pParametro);
        }

        public DataTable ObtenerListaFiltroDT(TESPlazosFijos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("TesPlazosFijosSeleccionarFiltro", pParametro);
        }


        public bool Borrar(TESPlazosFijos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            TESPlazosFijos valorViejo = new TESPlazosFijos();
            valorViejo.IdPlazoFijo = pParametro.IdPlazoFijo;
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
                    //Guardo el Plazo Fijo
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TesPlazosFijosAnular"))
                        resultado = false;

                    if (resultado && !new InterfazContableLN().PlazosFijosPropiosAnular(pParametro, bd, tran))
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

        public bool Pagar(TESPlazosFijos pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;
            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            TESPlazosFijos valorViejo = new TESPlazosFijos();
            valorViejo.IdPlazoFijo = pParametro.IdPlazoFijo;
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
                    //Guardo el Plazo Fijo
                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "TesPlazosFijosCobrar"))
                        resultado = false;


                    if (resultado && !new InterfazContableLN().PlazosFijosPropiosAcreditacion(pParametro, bd, tran))
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
    }
}
