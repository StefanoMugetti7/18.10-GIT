using Cargos.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Haberes.Entidades;
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

namespace Haberes.LogicaNegocio
{
    class HabFondoSuplementarioLN : BaseLN<HabFondoSuplementario>
    {
        public override bool Agregar(HabFondoSuplementario pParametro)
        {
            bool resultado = true;

            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdFondoSuplementario = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "HabFondoSuplementarioInsertar");
                    if (pParametro.IdFondoSuplementario == 0)
                        resultado = false;

                    if (!resultado || !AgregarModificar(pParametro,new HabFondoSuplementario(), bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        tran.Rollback();
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
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

        private bool AgregarModificar(HabFondoSuplementario pParametro,HabFondoSuplementario valorViejo, Database bd, DbTransaction tran)
        {
            bool resultado = true;

            foreach(HabSueldoHaberes detalle in pParametro.SueldosHaberes)
            {
                detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                switch (detalle.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        if (detalle.Coeficiente > 0)
                        {
                            detalle.IdFondoSuplementario = pParametro.IdFondoSuplementario;
                            detalle.IdAfiliado = pParametro.Afiliado.IdAfiliado;

                            detalle.IdSueldoSaltoGrande = BaseDatos.ObtenerBaseDatos().Agregar(detalle, bd, tran, "HabSueldosSaltoGrandeInsertar");
                        }
                        else
                            detalle.IdSueldoSaltoGrande = 1; //PARA QUE NO MAPEE EL ERROR EN LA SIGUIENTE LINEA

                        if (detalle.IdSueldoSaltoGrande == 0)
                        {
                            AyudaProgramacionLN.MapearError(detalle, pParametro);
                            return false;
                        }
                        break;
                    case EstadoColecciones.Modificado:
                        detalle.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if(detalle.Coeficiente > 0)
                        {
                            if (!BaseDatos.ObtenerBaseDatos().Actualizar(detalle, bd, tran, "HabSueldosSaltoGrandeModificar"))
                            {
                                AyudaProgramacionLN.MapearError(detalle, pParametro);
                                return false;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            return resultado;
        }
  
        public DataTable CalcularImporteJubilacion(HabFondoSuplementario pParametro)
        {
            pParametro.CargarLoteAportesJubilatoriosDetalles();
            if (ValidacionesCalculos(pParametro))
                return BaseDatos.ObtenerBaseDatos().ObtenerLista("HabSueldosCalcularAporteInicial", pParametro);


            pParametro.CodigoMensaje = pParametro.ErrorException;
            return new DataTable();
        }
        public override bool Modificar(HabFondoSuplementario pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;

            HabFondoSuplementario valorViejo = new HabFondoSuplementario();
            valorViejo.Afiliado.IdAfiliado = pParametro.Afiliado.IdAfiliado;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();
                try
                {
                    resultado = Validaciones(pParametro);
                    if (!resultado)
                        return false;

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "HabFondoSuplementarioModificar"))
                    {
                        AyudaProgramacionLN.MapearError(pParametro,pParametro);
                        return false;
                    }
                    #region Modificacion de SueldoHaberes

                    this.AgregarModificar(pParametro, valorViejo, bd, tran);
                    #endregion
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else                    
                        tran.Rollback();                    
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

        public bool ValidacionesCalculos(HabFondoSuplementario pParametro)
        {
            if (BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "HabSueldosCalcularAportesValidaciones"))
                return true;

            return false;
        }
        private bool Validaciones(HabFondoSuplementario pParametro)
        {
            if (BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "HabSueldosSaltoGrandeValidaciones"))
                return true;


            return false;
        }

        public override HabFondoSuplementario ObtenerDatosCompletos(HabFondoSuplementario pParametro)
        {
            HabFondoSuplementario retorno = BaseDatos.ObtenerBaseDatos().Obtener<HabFondoSuplementario>("HabFondoSuplementarioSeleccionar", pParametro);
            if (retorno.IdFondoSuplementario > 0)
            {
                retorno.SueldosHaberes = BaseDatos.ObtenerBaseDatos().ObtenerLista<HabSueldoHaberes>("HabFondoSuplementarioSeleccionarSueldos", retorno);
            }
            return retorno;
        }
        public DataTable ObtenerDatosCompletosDataTable(HabFondoSuplementario pParametro)
        {
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<HabFondoSuplementario>("HabFondoSuplementarioSeleccionar", pParametro);
            if (pParametro.IdFondoSuplementario > 0)
            {
                pParametro.SueldosHaberes = BaseDatos.ObtenerBaseDatos().ObtenerLista<HabSueldoHaberes>("HabFondoSuplementarioSeleccionarSueldos", pParametro);
                return pParametro.SueldosHaberes.ToDataTable<HabSueldoHaberes>();
            }
            return new DataTable();
        }

        public override List<HabFondoSuplementario> ObtenerListaFiltro(HabFondoSuplementario pParametro)
        {
            throw new NotImplementedException();
        }

        public List<CarCuentasCorrientes> ObtenerCargosPendientesCobro(CarCuentasCorrientes pParametro)
        {
            List<CarCuentasCorrientes> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CarCuentasCorrientes>("HabFondoSuplementarioSeleccionarPorAfiliadosCancelar", pParametro);
            return lista;
        }

        public bool EliminarAporte(HabSueldoHaberes pParametro)
        {
            if(BaseDatos.ObtenerBaseDatos().EjecutarSP(pParametro, "EliminarRegistroAporte") == 1)
                return true;
            
            return false;
        }


        public DataTable ObtenerPlantilla(HabFondoSuplementario pParametro)
        {
            DataTable tabla = BaseDatos.ObtenerBaseDatos().ObtenerLista("HabFondoSuplementarioPlantilla", pParametro);

            if (tabla.Rows.Count > 0)
                return tabla;

            return new DataTable();

        }

    }
}
