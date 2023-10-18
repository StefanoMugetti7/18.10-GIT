using Afiliados.Entidades;
using Comunes.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Afiliados.LogicaNegocio
{
    class RenaperLN
    {
        public bool ValidarObtenerDatos(AfiAfiliados pParametro)
        {
            bool resultado = false;
            try
            {
                DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet("ApiRenaperSeleccionarPorDniSexo", pParametro);
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        Mapeador.SetearEntidadPorFila(ds.Tables[0].Rows[0], pParametro);
                        resultado = true;
                        pParametro.CodigoMensaje = "ApiRenaperDatosImportados";
                    }
                    else {
                        pParametro.CodigoMensaje = "ApiRenaperDatosNoValidos";
                    }
                }
                if (ds.Tables.Count > 1)
                {
                    AfiDomicilios domicilios = new AfiDomicilios();
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        Mapeador.SetearEntidadPorFila(ds.Tables[1].Rows[0], domicilios);
                        domicilios.EstadoColeccion = EstadoColecciones.Agregado;
                        pParametro.Domicilios.Add(domicilios);
                    }
                }
            }
            catch (Exception ex)
            {
                pParametro.CodigoMensaje = ex.Message;
            }
            return resultado;
        }
    }
}
