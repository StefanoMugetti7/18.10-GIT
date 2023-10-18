using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AFIP.WebServices.ar.gov.afip.fe;

namespace AFIP.WebServices
{
    public class MonedasCotizacionesLN
    {
        const string _serice = "wsfe";
        string _token;
        string _sign;
        long _cuit;
        public string CodigoMensaje { get; set; }
        public bool Autenticado { get; set; }

        public MonedasCotizacionesLN()
        {
            ObtenerAutenticacion();
        }

        private void ObtenerAutenticacion()
        {
            //if (this._aut != null && !string.IsNullOrEmpty(this._aut.Token))
            //    return true;

            ar.com.evol.erp.Objeto parametro = new ar.com.evol.erp.Objeto();
            parametro.CodigoMensaje = "ERP.EVOL.COM.AR.WS.INTERNAL.CODE";
            parametro.Filtro = _serice;
            ar.com.evol.erp.AfipServiciosWebTickets ticket = new ar.com.evol.erp.AfipServiciosWebTickets();
            ticket = new ar.com.evol.erp.WSEvol().ObtenerAutenticacion(parametro);

            if (ticket.IdLoginTicket == 0)
            {
                Autenticado = false;
                CodigoMensaje = parametro.CodigoMensaje;
                _token = string.Empty;
                _sign = string.Empty;
            }
            else
            {
                Autenticado = true;
                CodigoMensaje = string.Empty;
                _token = ticket.Token;
                _sign = ticket.Sign;
            }


            string cuit = "30714930083";//TGEGeneralesF.EmpresasSeleccionar().CUIT; // TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSCuit).ParametroValor;
            if (string.IsNullOrEmpty(cuit))
            {
                CodigoMensaje = "ErrorValidarWSCuit";
                Autenticado = false;
            }
            long cuitNumero;
            if (!long.TryParse(cuit, out cuitNumero))
            {
                CodigoMensaje = "ErrorValidarWSCuit";
                Autenticado = false;
            }
            _cuit = cuitNumero;
        }

        public FECotizacionResponse ObtenerCotizacion(string idMoneda)
        {
            Service servicio = new Service();
            FEAuthRequest aut = new FEAuthRequest();
            aut.Token = _token;
            aut.Sign = _sign;
            aut.Cuit = _cuit;
            FECotizacionResponse cotizacionResponse = servicio.FEParamGetCotizacion(aut, idMoneda);
            return cotizacionResponse;
        }
        
    }
}
