using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using AFIP.WebServices.ar.gov.afip.aws;
using AFIP.WebServices.ar.gov.afip.fe;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.FachadaNegocio;

namespace AFIP.WebServices
{
    public class ConsultarPadronLN
    {
        const string _serice = "ws_sr_constancia_inscripcion";
        string _token;
        string _sign;
        long _cuit;
        public string CodigoMensaje { get; set; }
        public bool Autenticado { get; set; }

        public ConsultarPadronLN()
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
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

        public personaReturn ConsultarPadronPorCUIT(long pCUIT)
        {
            try
            {
                PersonaServiceA5 personaServiceA5 = new PersonaServiceA5();
                return personaServiceA5.getPersona_v2(this._token, _sign, _cuit, pCUIT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
