using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Elecciones.Entidades;
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

namespace Elecciones.LogicaNegocio
{
    public class EleccionesVotosLN : BaseLN<EleEleccionesVotos>
    {
        public override bool Agregar(EleEleccionesVotos pParametro)
        {
            throw new NotImplementedException();
        }

        public bool AgregarVotos(EleElecciones pParametro)
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
                    pParametro.LoteVotos = pParametro.Votos.ToDataTable().ToXmlDocument();

                    if (!this.Validaciones(pParametro))
                        return false;

                    if (resultado)
                    {
                        if(pParametro.Votos.Count == 0)
                        {
                            EleEleccionesVotos aux = new EleEleccionesVotos();
                            aux.IdAfiliado = 0;
                            aux.IdListaEleccion = 0;
                            pParametro.Votos.Add(aux);
                        }
                        foreach (EleEleccionesVotos item in pParametro.Votos)
                        {
                            item.IdEstado = (int)Estados.Activo;
                            item.UsuarioLogueado = pParametro.UsuarioLogueado;
                            item.IdEleccionVoto = BaseDatos.ObtenerBaseDatos().Agregar(item, bd, tran, "ELEEleccionesVotosInsertar");
                            if (item.IdEleccionVoto == 0)
                            {
                                resultado = false;
                                break;
                            }
                        }
                    }

                    //if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                    //    resultado = false;
                    //if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                    //    resultado = false;
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                        pParametro.CodigoMensajeArgs.Add(pParametro.IdEleccion.ToString());
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

        private bool Validaciones(EleElecciones pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "EleEleccionesValidarVotacion");
        }

        public override bool Modificar(EleEleccionesVotos pParametro)
        {
            throw new NotImplementedException();
        }

        public override EleEleccionesVotos ObtenerDatosCompletos(EleEleccionesVotos pParametro)
        {
            throw new NotImplementedException();
        }

        public override List<EleEleccionesVotos> ObtenerListaFiltro(EleEleccionesVotos pParametro)
        {
            throw new NotImplementedException();
        }

        public bool ValidarVotacion(EleElecciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            pParametro.LoteVotos = pParametro.Votos.ToDataTable().ToXmlDocument();
            
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "EleEleccionesValidarVotacion");

        }
        public bool ValidarVotacionConfirmar(EleElecciones pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            pParametro.LoteVotos = pParametro.Votos.ToDataTable().ToXmlDocument();

            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "EleEleccionesValidarVotacionConfirmar");

        }

    }
}
