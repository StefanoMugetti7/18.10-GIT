using Afiliados.Entidades.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
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

namespace Afiliados.LogicaNegocio
{
    class AfiAfiliadosVisitasLN : BaseLN<AfiAfiliadosVisitas>

    {
        public override bool Agregar(AfiAfiliadosVisitas pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (pParametro.IdAfiliadoVisita > 0)
                return true;

            //if (!this.Validar(pParametro, new AfiAfiliados()))
            //    return false;

            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //if (pParametro.IdAfiliadoFallecido == 0
                    //    && pParametro.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                    //    pParametro.NumeroSocio = this.ObtenerProximoNumeroSocio(pParametro.Categoria);
                    //if (!this.ValidarNumeroSocio(pParametro, bd, tran))
                    //    return false;

                    pParametro.IdAfiliadoVisita = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiAfiliadosVisitasInsertar");
                    if (pParametro.IdAfiliadoVisita == 0)
                        resultado = false;

                    //if (resultado && !this.DomiciliosActualizar(pParametro, new AfiAfiliados(), bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.TelefonoActualizar(pParametro, new AfiAfiliados(), bd, tran))
                    //    resultado = false;

                    //if (resultado && !this.AlertasTiposActualizar(pParametro, new AfiAfiliados(), bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.IdAfiliadoVisita = 0;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.IdAfiliadoVisita = 0;
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public override bool Modificar(AfiAfiliadosVisitas pParametro)
        {
            throw new NotImplementedException();
        }

        public override AfiAfiliadosVisitas ObtenerDatosCompletos(AfiAfiliadosVisitas pParametro)
        {
            throw new NotImplementedException();
        }

        public override List<AfiAfiliadosVisitas> ObtenerListaFiltro(AfiAfiliadosVisitas pParametro)
        {
            throw new NotImplementedException();
        }

        public DataTable ObtenerListaGrilla(AfiAfiliadosVisitas pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AfiAfiliadosVisitasSeleccionarDescripcionPorFiltroGrilla", pParametro);
        }
    }
}
