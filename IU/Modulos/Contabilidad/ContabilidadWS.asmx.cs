using Contabilidad;
using Contabilidad.Entidades;
using Newtonsoft.Json;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace IU.Modulos.Contabilidad
{
    /// <summary>
    /// Summary description for ContabilidadWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    [System.Web.Script.Services.ScriptService]
    public class ContabilidadWS : System.Web.Services.WebService
    {

        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CuentasContablesArmarArbol2(string idEjercicio, string descripcion)
        {
            if (string.IsNullOrEmpty(idEjercicio))
                new CtbCuentasContablesDTO();
            //Filtrar Arbol
            bool filtrar = descripcion.Trim() != string.Empty;
            CtbCuentasContables filtro = new CtbCuentasContables();
            filtro.IdEjercicioContable = Convert.ToInt32(idEjercicio);
            filtro.Descripcion = descripcion;
            List<CtbCuentasContables> cuentasContables = ContabilidadF.CuentasContablesObtenerListaRamaPorIdEjercicio(filtro);
            List<CtbCuentasContables> listaFiltro = filtrar ? cuentasContables.Where(x => x.Descripcion.ToLower().Contains(descripcion.Trim().ToLower())).ToList()
                                                            : cuentasContables;

            if (filtrar)
            {
                bool buscar = true;
                int desde = 0;
                int hasta = listaFiltro.Count;
                while (buscar)
                {
                    buscar = false;
                    for (int i = desde; i < hasta; i++)
                    {
                        if (!listaFiltro.Exists(c => listaFiltro[i].IdCuentaContableRama == c.IdCuentaContable)
                            && cuentasContables.Exists(c => listaFiltro[i].IdCuentaContableRama == c.IdCuentaContable))
                        {
                            listaFiltro.Add(cuentasContables.Find(c => listaFiltro[i].IdCuentaContableRama == c.IdCuentaContable));
                            buscar = true;
                        }
                    }
                    desde = hasta;
                    hasta = listaFiltro.Count;
                }
                //Agrego nodo RAIZ
                listaFiltro.AddRange(listaFiltro.Where(p => !cuentasContables.Any(p2 => p2.IdCuentaContable == p.IdCuentaContable && p2.Estado.IdEstado == -1)));
            }
            int idPlan = 0;
            if (listaFiltro.Exists(x => x.IdCuentaContableRama == 0))
            {
                idPlan = listaFiltro.First(x => x.IdCuentaContableRama == 0).IdCuentaContable;
            }
            List<CtbCuentasContablesDTO> resultdo = listaFiltro.Where(x => x.IdCuentaContableRama == idPlan).OrderBy(x=>x.NumeroCuenta).Select(x => new CtbCuentasContablesDTO
            {
                id = x.IdCuentaContable,
                idPadre = x.IdCuentaContableRama,
                text = x.Descripcion,
                imputable = x.Imputable,
                children = GetChildren(listaFiltro, x.IdCuentaContable)
            }).ToList();
            string r = JsonConvert.SerializeObject(resultdo, Newtonsoft.Json.Formatting.None);
            return r;
        }
        [WebMethod()]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<CtbCuentasContablesDTO> ContabilidadSeleccionarAjaxComboCuentas(string value, string filtro, string idEjercicioContable)
        {
            CtbCuentasContables cuenta = new CtbCuentasContables();

            string sp = "CTBCuentasContablesSeleccionarAjaxComboCuentas";
            cuenta.Filtro = filtro;
            cuenta.IdEjercicioContable = Convert.ToInt32(idEjercicioContable);
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCuentasContablesDTO>(sp, cuenta);
        }
         
        //[WebMethod()]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<CtbCuentasContablesDTO> ContabilidadSeleccionarAjaxComboCuentasBancosCuentas(string value, string filtro)
        //{
        //    CtbCuentasContables cuenta = new CtbCuentasContables();
        //    string sp = "CTBCuentasContablesSeleccionarAjaxComboCuentas";
        //    cuenta.Filtro = filtro;
        //    cuenta.IdEjercicioContable = 0;
        //    return BaseDatos.ObtenerBaseDatos().ObtenerLista<CtbCuentasContablesDTO>(sp, cuenta);
        //}


        private List<CtbCuentasContablesDTO> GetChildren(List<CtbCuentasContables> listaFiltro, int parentId)
        {
            return listaFiltro.Where(x => x.IdCuentaContableRama == parentId)//.OrderBy(l => l.OrderNumber)
                .Select(x => new CtbCuentasContablesDTO
                {
                    id = x.IdCuentaContable,
                    idPadre = x.IdCuentaContableRama,
                    text = x.Descripcion,
                    imputable = x.Imputable,
                    children = GetChildren(listaFiltro, x.IdCuentaContable)
                }).ToList();
        }
    }
}
