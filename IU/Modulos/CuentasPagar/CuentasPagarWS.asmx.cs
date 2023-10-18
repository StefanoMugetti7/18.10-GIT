using Afiliados.Entidades;
using Proveedores.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace IU.Modulos.CuentasPagar
{
    /// <summary>
    /// Descripción breve de CuentasPagarWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class CuentasPagarWS : System.Web.Services.WebService
    {

        [WebMethod]
        public List<Select2DTO> BuscarPorEntidad(int IdEntidad, string filtro)
        {
            List<Select2DTO> resultado = new List<Select2DTO>();
           switch (IdEntidad)
            {
                case (int)EnumTGEEntidades.Proveedores:
           
                    CapProveedores Proveedor = new CapProveedores();
                    Proveedor.RazonSocial = filtro;
                    List<CapProveedoresDTO> ListaProv = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedoresDTO>("CapProveedoresSeleccionarComboAjax", Proveedor);
                    Select2DTO item;
                    foreach (CapProveedoresDTO t in ListaProv)
                    {
                        item = new Select2DTO();
                        item.id = t.IdProveedor;
                        item.text = t.RazonSocial;
                        resultado.Add(item);
                    }
                    break;
                case (int)EnumTGEEntidades.Afiliados:
                    AfiAfiliados afi = new AfiAfiliados();
       
                    afi.Apellido = filtro;
                    List<AfiAfiliadosDTO> listaAfi = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosDTO>("AfiAfiliadosSeleccionarAjaxComboApellido", afi);
                    
                    foreach (AfiAfiliadosDTO t in listaAfi)
                    {
                        item = new Select2DTO();
                        item.id = t.IdAfiliado;
                        item.text = t.DescripcionCombo;
                        resultado.Add(item);
                    }                   

                    break;
                default:
                    break;
            }

            return resultado;
        }
    }
}
