using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Medicina.Entidades;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Comunes.Entidades;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Auditoria;
using Auditoria.Entidades;
using Generales.FachadaNegocio;

namespace Medicina.LogicaNegocio
{
    public class MedTurnerasLN : BaseLN<MedTurneras>
    {
        public override bool Agregar(MedTurneras pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(MedTurneras pParametro)
        {
            throw new NotImplementedException();
        }

        internal bool AgregarActualizar(MedTurneras pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

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
                    if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, bd, tran, "MedTurnerasValidarSeleccionarFiltros"))
                    {
                        pParametro.Estado.IdEstado = (int)Estados.Activo;
                        pParametro.FechaAlta = DateTime.Now;
                        pParametro.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        pParametro.IdTurnera = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "MedTurnerasInsertar");
                        if (pParametro.IdTurnera == 0)
                            resultado = false;
                    }


                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;


                    if (resultado && !this.ActualizarTurnos(pParametro, new MedTurneras(), bd, tran))
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

        private bool ActualizarTurnos(MedTurneras pParametro, MedTurneras pValorViejo, Database db, DbTransaction tran)
        {
            foreach (MedTurnos item in pParametro.Turnos)
            {
                switch (item.EstadoColeccion)
                {
                    #region "Agregado"
                    case EstadoColecciones.Agregado:
                        item.IdUsuarioAlta = pParametro.UsuarioLogueado.IdUsuarioEvento;
                        item.FechaAlta = DateTime.Now;
                        item.IdTurnera = pParametro.IdTurnera;
                        item.IdTurno = BaseDatos.ObtenerBaseDatos().Agregar(item, db, tran, "MedTurnosInsertar");
                        if (item.IdTurno == 0)
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        break;
                    #endregion

                    #region "Modificado"
                    case EstadoColecciones.Modificado:
                        item.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(item, db, tran, "MedTurnosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(item, pParametro);
                            return false;
                        }
                        //if (!AuditoriaF.AuditoriaAgregar(
                        //    pValorViejo.Turnos.Find(x => x.IdTurno == item.IdTurno), Acciones.Update, item, db, tran))
                        //{
                        //    AyudaProgramacionLN.MapearError(item, pParametro);
                        //    return false;
                        //}
                        break;
                    #endregion
                }
            }
            return true;
        }

        public bool ActualizarTurnos(MedTurnos pParametro, Database db, DbTransaction tran)
        {
            if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, db, tran, "MedTurnosActualizarEstado"))
                return false;

            return true;
        }

        public override MedTurneras ObtenerDatosCompletos(MedTurneras pParametro)
        {
            throw new NotImplementedException();
        }

        public MedTurneras ObtenerFiltro(MedTurneras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<MedTurneras>("MedTurnerasSeleccionarPorFiltros", pParametro);
        }

        public override List<MedTurneras> ObtenerListaFiltro(MedTurneras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedTurneras>("MedTurnerasSeleccionarPorFiltros", pParametro);
        }

        public List<MedTurnos> ObtenerTurnos(MedTurneras pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedTurnos>("MedTurnosSeleccionarFiltros", pParametro);
        }

        public List<MedTurnos> ObtenerTurnos(MedTurnos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedTurnos>("MedTurnosSeleccionarFiltros", pParametro);
        }
        public List<MedTurnos> ObtenerTurnosPacientes(MedTurnos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<MedTurnos>("MedTurnosSeleccionarFiltrosPacientes", pParametro);
        }

        public bool ValidarObtenerTurnos(MedTurnos pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "MedTurnosValidarSeleccionarFiltros");
        }

        public List<MedTurneras> ObtenerTurneras(MedTurneras pParametro, int pCantidad)
        {
            List<MedTurneras> turneras = new List<MedTurneras>();
            List<MedTurnos> turnosTomados;
            MedTurneras tur;
            int i = 0;
            DateTime dia = pParametro.Fecha;

            pParametro.Prestador = new MedPrestadoresLN().ObtenerDatosCompletosTurnera(pParametro.Prestador);
            if (pParametro.Especializacion.IdEspecializacion > 0)
            {
              
                pParametro.Prestador.PrestadoresDiasHoras = pParametro.Prestador.PrestadoresDiasHoras.FindAll(z=> z.Especializaciones.Exists(x => x.IdEspecializacion == pParametro.Especializacion.IdEspecializacion));
            }

            while (i < pCantidad)
            {
                turnosTomados = new List<MedTurnos>();
                
                if (pParametro.Prestador.PrestadoresDiasHoras.Exists(x => x.Dia.CodigoValor == ((int)dia.DayOfWeek+1).ToString()))
                {
                    MedPrestadoresDiasHoras diaHora = pParametro.Prestador.PrestadoresDiasHoras.Find(x => x.Dia.CodigoValor == ((int)dia.DayOfWeek+1).ToString() && x.Filial.IdFilial==pParametro.Filial.IdFilial);

                    if (diaHora == null)
                    {
                        return new List<MedTurneras>();
                    }

                    tur = new MedTurneras();
                    tur.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                    tur.Fecha = dia;
                    AyudaProgramacionLN.MatchObjectProperties(diaHora.Filial, tur.Filial);
                    AyudaProgramacionLN.MatchObjectProperties(pParametro.Prestador, tur.Prestador);
                    if (BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(tur, "MedTurnerasValidarSeleccionarFiltros"))
                    {
                        tur = this.ObtenerFiltro(tur);
                        turnosTomados = BaseDatos.ObtenerBaseDatos().ObtenerLista<MedTurnos>("MedTurnosSeleccionarPorMedTurneras", tur);
                    }
                    AyudaProgramacionLN.MatchObjectProperties(pParametro.Prestador, tur.Prestador);
                    tur.GuidTurnera = Guid.NewGuid();
                    tur.HashTransaccion = diaHora.IdPrestadorDiaHora;
                    tur.DesdeHora = diaHora.HoraDesde;
                    tur.HastaHora = diaHora.HoraHasta;
                    tur.Prestador = pParametro.Prestador;
                    tur.Turnos = this.ArmarTurnosDisponibles(tur, diaHora.Tiempo);

                    foreach (MedTurnos t in turnosTomados)
                    {
                        t.GuidTurno = Guid.NewGuid();
                        t.GuidTurnera = tur.GuidTurnera;
                        if (tur.Turnos.Exists(x => x.FechaHoraDesde == t.FechaHoraDesde))
                            tur.Turnos[tur.Turnos.FindIndex(x => x.FechaHoraDesde == t.FechaHoraDesde)] = t;
                        else
                        {
                            tur.Turnos.Add(t);
                        }
                    }

                    turneras.Add(tur);
                }

                dia = dia.AddDays(1);
                i++;
            }

            return turneras;
        }

        private List<MedTurnos> ArmarTurnosDisponibles(MedTurneras pTurnera, int pTiempo)
        {
            DateTime startTime = DateTime.Parse(pTurnera.DesdeHora.ToString());
            DateTime endTime = DateTime.Parse(pTurnera.HastaHora.ToString());
            MedTurnos turno;
            while (startTime <= endTime)
            {
                turno = new MedTurnos();
                //Estoy pasando el IdPrestadorDiaHora
                turno.HashTransaccion = pTurnera.HashTransaccion;
                turno.FechaHoraDesde = pTurnera.Fecha.Date;
                turno.FechaHoraDesde = turno.FechaHoraDesde.Value.AddHours(startTime.Hour).AddMinutes(startTime.Minute);
                turno.FechaHoraHasta = turno.FechaHoraDesde.Value.AddMinutes(pTiempo);
                turno.Estado.IdEstado = (int)EstadosTurnos.Disponible;
                pTurnera.Turnos.Add(turno);
                turno.GuidTurno = Guid.NewGuid();// pTurnera.Id;
                turno.GuidTurnera = pTurnera.GuidTurnera;
                startTime = startTime.AddMinutes(pTiempo);
            }
            return pTurnera.Turnos;
        }

        private bool Validar(MedTurneras pParametro)
        {
            foreach(MedTurnos turno in pParametro.Turnos)
            {
                switch (turno.EstadoColeccion)
            {
                case EstadoColecciones.Agregado:
                    if (BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(turno, "MedTurnosValidarSeleccionarAfiliado"))
                    {
                        pParametro.CodigoMensaje = "ValidarTurnoDoble";
                        return false;
                    }
                    break;
                case EstadoColecciones.Modificado:
                    break;
                case EstadoColecciones.SinCambio:
                    break;
                default:
                    break;
            }
            }
            return true;
        }
    }
}
