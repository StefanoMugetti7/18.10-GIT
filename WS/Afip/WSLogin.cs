using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Comunes.Entidades;
using Servicio.AccesoDatos;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;

namespace WS.Afip
{
    public class WSLogin
    {
        public WSLogin()
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
        }

        public AfipServiciosWebTickets ObtenerAutenticacion(Objeto pParametro)
        {
            bool resultado = true;

            AfipServiciosWebTickets ticket = new AfipServiciosWebTickets();

            if (pParametro.CodigoMensaje != "ERP.EVOL.COM.AR.WS.INTERNAL.CODE")
                return ticket;

            //Obtengo el Login Ticket
            LoginTicket objTicketRespuesta = new LoginTicket();
            //Objeto para Autenticacion
            //this._aut = new FEAuthRequest();
            string argServicio = "wsfe";
            if (pParametro.Filtro == string.Empty)
                pParametro.Filtro = argServicio;

            ticket.FechaEvento = DateTime.Now;
            ticket.Service = pParametro.Filtro;
            ticket = BaseDatos.ObtenerBaseDatos().Obtener<AfipServiciosWebTickets>("AfipServiciosWebTicketsSeleccionar", ticket, BaseDatos.conexionErpComun);
            if (ticket.IdLoginTicket > 0)
            {
                //this._aut.Token = ticket.Token;
                //this._aut.Sign = ticket.Sign;
                return ticket;
            }
            else
            {
                string path = "Certificados\\EvolCert.p12";
                if (!objTicketRespuesta.ObtenerLoginTicketResponse(pParametro, path))
                    return ticket;

                //ticket = new AfipServiciosWebTickets();
                //ticket.UniqueId = objTicketRespuesta.UniqueId;
                ticket.GenerationTime = objTicketRespuesta.GenerationTime;
                ticket.ExpirationTime = objTicketRespuesta.ExpirationTime;
                ticket.Sign = objTicketRespuesta.Sign;
                ticket.Token = objTicketRespuesta.Token;
                ticket.LoginTicketResponse = objTicketRespuesta.LoginTicketResponse;
                ticket.Service = pParametro.Filtro;

                //Guardo el Ticke en la BD
                DbTransaction tran;
                DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.Create(BaseDatos.conexionErpComun);

                using (DbConnection con = bd.CreateConnection())
                {
                    con.Open();
                    tran = con.BeginTransaction();

                    try
                    {
                        ticket.IdLoginTicket = BaseDatos.ObtenerBaseDatos().Agregar(ticket, bd, tran, "AfipServiciosWebTicketsInsertar");
                        if (ticket.IdLoginTicket == 0)
                            resultado = false;

                        if (resultado)
                        {
                            tran.Commit();
                            pParametro.CodigoMensaje = "ResultadoTransaccion";
                        }
                        else
                        {
                            tran.Rollback();
                            AyudaProgramacionLN.MapearError(ticket, pParametro);
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, "LogicaNegocio");
                        tran.Rollback();
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                        pParametro.CodigoMensajeArgs.Add(ex.Message);
                        return ticket;
                    }
                }
                return ticket;              
            }
        }
    }
}