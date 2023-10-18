using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Proveedores.Entidades;
using Servicio.AccesoDatos;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Generales.FachadaNegocio;
using Auditoria;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Auditoria.Entidades;
using System.Net.Mail;
using Generales.Entidades;
using System.IO;
using System.Data;

namespace Proveedores.LogicaNegocio
{
    class CapProveedoresLN : BaseLN<CapProveedores>
    {
        public override CapProveedores ObtenerDatosCompletos(CapProveedores pParametro)
        {
            
            CapProveedores proveedor = BaseDatos.ObtenerBaseDatos().Obtener<CapProveedores>("CapProveedoresSeleccionarDescripcion", pParametro);
            proveedor.ProveedoresDomicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedoresDomicilios>("CapProveedoresDomiciliosSeleccionarPorCapProveedores", pParametro);
            proveedor.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedoresTelefonos>("CapProveedoresTelefonosSeleccionarDescripcion", pParametro);
            proveedor.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            proveedor.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return proveedor;
        }

        public override List<CapProveedores> ObtenerListaFiltro(CapProveedores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedores>("CapProveedoresSeleccionarDescripcionPorFiltro", pParametro);
        }

        public DataTable ObtenerListaFiltroDT(CapProveedores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("CapProveedoresSeleccionarDescripcionPorFiltroDT", pParametro);
        }

        public List<CapProveedores> ObtenerEsVendedor()
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<CapProveedores>("CapProveedoresSeleccionarEsVendedor");
        }

        public override bool Agregar(CapProveedores pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (!this.Validar(pParametro, new CapProveedores()))
                return false;


            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = this.Agregar(pParametro, bd, tran);

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
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

        internal bool Agregar(CapProveedores pParametro, Database bd, DbTransaction tran)
        {

            bool resultado = true;
            pParametro.IdProveedor = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "CapProveedoresInsertar");
            if (pParametro.IdProveedor == 0)
                resultado = false;

            if (resultado && !AuditoriaF.AuditoriaAgregar(new CapProveedores(), Acciones.Insert, pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                resultado = false;

            if (resultado && !this.DomiciliosActualizar(pParametro, new CapProveedores(), bd, tran))
                resultado = false;

            if (resultado && !this.TelefonoActualizar(pParametro, new CapProveedores(), bd, tran))
                resultado = false;

            return resultado;
        }

        private bool Validar(CapProveedores pParametro, CapProveedores pValorViejo)
        {
            pParametro.CUIT = pParametro.CUIT.Replace("-", "");
            CapProveedores proveedor = new CapProveedores();
            List<CapProveedores> listaCUIT;
            List<TGEListasValoresDetalles> listaExcepciones;
            TGEEmpresas empresa;
            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.SinCambio:
                    break;
                case EstadoColecciones.Agregado:

                    proveedor.CUIT = pParametro.CUIT;
                    listaCUIT = this.ObtenerListaFiltro(proveedor);
                    empresa = TGEGeneralesF.EmpresasSeleccionar();
                    listaCUIT = listaCUIT.Where(x => x.CUIT != empresa.CUIT).ToList();
                    listaExcepciones = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CuitProveedoresMultiples);
                    listaCUIT = listaCUIT.Where(p => !listaExcepciones.Any(x => p.CUIT.Trim() == x.CodigoValor.Trim())).ToList();
                    if (listaCUIT.Count > 0)
                    {
                        pParametro.CodigoMensaje = "ProveedorExisteCUIT";
                        pParametro.CodigoMensajeArgs.Add(listaCUIT[0].CUIT);
                        return false;
                    }

                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:
                    break;
                case EstadoColecciones.Modificado:
                    proveedor.CUIT = pParametro.CUIT;
                    listaCUIT = this.ObtenerListaFiltro(proveedor);
                    empresa = TGEGeneralesF.EmpresasSeleccionar();
                    listaCUIT = listaCUIT.Where(x => x.CUIT != empresa.CUIT && x.IdProveedor != pParametro.IdProveedor).ToList();
                    listaExcepciones = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.CuitProveedoresMultiples);
                    listaCUIT = listaCUIT.Where(p => !listaExcepciones.Any(x => p.CUIT.Trim() == x.CodigoValor.Trim())).ToList();
                    if (listaCUIT.Count > 0)
                    {
                        pParametro.CodigoMensaje = "ProveedorExisteCUIT";
                        pParametro.CodigoMensajeArgs.Add(listaCUIT[0].CUIT);
                        return false;
                    }
                    break;
                default:
                    break;
            }
            //VALIDO CAMPOS
            if (pParametro.RazonSocial == String.Empty || pParametro.CUIT == String.Empty)
            {
                pParametro.CodigoMensaje = "ProveedoresCamposObligatorios";
                return false;
            }

            String CBU = string.Concat(pParametro.CBU8Digitos, pParametro.CBU14Digitos);
            if (CBU.Length < 22 && CBU != String.Empty)
            {
                pParametro.CodigoMensaje = "ProveedoresCBUFaltaDigitos";
                return false;
            }

            return true;
        }

        public override bool Modificar(CapProveedores pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            if (!this.Validar(pParametro, new CapProveedores()))
                return false;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            CapProveedores valorViejo = new CapProveedores();
            valorViejo.IdProveedor = pParametro.IdProveedor;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    resultado = BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "CapProveedoresActualizar");

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.DomiciliosActualizar(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.TelefonoActualizar(pParametro, valorViejo, bd, tran))
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

        public bool ArmarMailResumenCuenta(CapProveedores pParametro, MailMessage mail)
        {
            bool resultado = true;

            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();

            string template = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Templates\\MailEnviarClienteResumenCuenta.htm");
            //if (pParametro.CorreoElectronico.Trim() != string.Empty)
            //    mail.To.Add(new MailAddress(pParametro.CorreoElectronico, pParametro.ApellidoNombre));
            //if (pParametro.CorreoElectronico.Trim() != string.Empty)
            //{
            //    if (pParametro.CorreoElectronico.Trim().Contains(";"))
            //    {
            //        List<string> lista = pParametro.CorreoElectronico.Trim().Split(';').ToList();
            //        foreach (string item in lista)
            //            mail.To.Add(new MailAddress(item.Trim(), pParametro.RazonSocial.Trim()));
            //    }
            //    else
            //        mail.To.Add(new MailAddress(pParametro.CorreoElectronico.Trim(), pParametro.RazonSocial.Trim()));
            //}

            mail.Subject = "Resumen de Cuenta";
            mail.IsBodyHtml = true;
            mail.Body = new StreamReader(template).ReadToEnd();
            mail.Body = mail.Body.Replace("%ApellidoNombre%", pParametro.RazonSocial);
            mail.Body = mail.Body.Replace("%Empresa%", empresa.Empresa);
            //mail.Body = mail.Body.Replace("%EmpresaDatos%", empresa.);

            return resultado;
        }

        public bool ObtenerDatosAFIP(CapProveedores pProveedor)
        {
            bool resultado = true; ;
            try
            {
                AFIP.WebServices.ConsultarPadronLN consultarPadronLN = new AFIP.WebServices.ConsultarPadronLN();
                if (consultarPadronLN.Autenticado)
                {
                    AFIP.WebServices.ar.gov.afip.aws.personaReturn persona = consultarPadronLN.ConsultarPadronPorCUIT(pProveedor.CUITNumero);
                    if (persona.errorConstancia != null)
                    {
                        resultado = false;
                        string separador = string.Empty;
                        foreach (string s in persona.errorConstancia.error)
                        {
                            pProveedor.CodigoMensaje = string.Concat(pProveedor.CodigoMensaje, separador, s);
                            separador = " - ";
                            return false;
                        }
                    }
                    if (persona.datosGenerales.tipoPersona.ToUpper() == "FISICA")
                    {
                        pProveedor.RazonSocial = string.Concat(persona.datosGenerales.apellido, " ", persona.datosGenerales.nombre);
                    }
                    else
                    {
                        pProveedor.RazonSocial = persona.datosGenerales.razonSocial;
                    }

                    //if (persona.datosGenerales.domicilioFiscal != null)
                    //{
                    //    CapProveedoresDomicilios dom = new CapProveedoresDomicilios();
                    //    dom.Calle = persona.datosGenerales.domicilioFiscal.direccion;
                    //    if (int.TryParse(persona.datosGenerales.domicilioFiscal.codPostal, out int cp))
                    //        dom.Localidad.CodigoPostal = cp;
                    //    TGEProvincias prov = new TGEProvincias();
                    //    prov.CodigoAfip = persona.datosGenerales.domicilioFiscal.idProvincia;
                    //    dom.Localidad.Provincia = TGEGeneralesF.ProvinciaObtenerPorCodigoAfip(prov);
                    //    if (dom.Localidad.Provincia.IdProvincia == 1)
                    //        dom.Localidad.IdCodigoPostal = 22136;

                    //}

                    pProveedor.CondicionFiscal.IdCondicionFiscal = (int)EnumTGECondicionesFiscales.ConsumidorFinal;
                    if (persona.datosMonotributo != null)
                        pProveedor.CondicionFiscal.IdCondicionFiscal = (int)EnumTGECondicionesFiscales.ResponsableMonotributista;
                    else if (persona.datosRegimenGeneral != null)
                    {
                        if (persona.datosRegimenGeneral.impuesto != null)
                        {
                            if (persona.datosRegimenGeneral.impuesto.ToList().Exists(x => x.idImpuesto == 30)) //IVA
                                pProveedor.CondicionFiscal.IdCondicionFiscal = (int)EnumTGECondicionesFiscales.IVAResponsableInscripto;
                            else if (persona.datosRegimenGeneral.impuesto.ToList().Exists(x => x.idImpuesto == 32)) //IVA EXENTO
                                pProveedor.CondicionFiscal.IdCondicionFiscal = (int)EnumTGECondicionesFiscales.IVASujetoExcento;
                        }
                    }
                }
                else
                {
                    pProveedor.CodigoMensaje = consultarPadronLN.CodigoMensaje;
                    resultado = false;
                }
            }
            catch (Exception ex)
            {
                pProveedor.CodigoMensaje = ex.Message;
                resultado = false;
            }
            return resultado;
        }

        public CapProveedores ObtenerDatosCbu(CapProveedores pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<CapProveedores>("CapOrdenesPagosValoresObtenerDatosCbu", pParametro);
        }

        #region domicilios

        private bool DomiciliosActualizar(CapProveedores pParametro, CapProveedores pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (CapProveedoresDomicilios domicilio in pParametro.ProveedoresDomicilios)
            {
                switch (domicilio.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        domicilio.IdProveedor = pParametro.IdProveedor;
                        domicilio.IdProveedorDomicilio = BaseDatos.ObtenerBaseDatos().Agregar(domicilio, bd, tran, "CapProveedoresDomiciliosInsertar");
                        if (domicilio.IdProveedorDomicilio == 0)
                        {
                            AyudaProgramacionLN.MapearError(domicilio, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    case EstadoColecciones.Borrado:
                        break;
                    #region Modificado
                    case EstadoColecciones.Modificado:
                        domicilio.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(domicilio, bd, tran, "CapProveedoresDomiciliosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(domicilio, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.ProveedoresDomicilios.Find(x => x.IdProveedorDomicilio == domicilio.IdProveedorDomicilio), Acciones.Update, domicilio, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(domicilio, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            return true;
        }
        #endregion

        #region telefonos

        private bool TelefonoActualizar(CapProveedores pParametro, CapProveedores pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (CapProveedoresTelefonos telefono in pParametro.Telefonos)
            {
                switch (telefono.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        telefono.IdProveedor = Convert.ToInt32(pParametro.IdProveedor);
                        telefono.IdTelefono = BaseDatos.ObtenerBaseDatos().Agregar(telefono, bd, tran, "CapProveedoresTelefonosInsertar");
                        if (telefono.IdTelefono == 0)
                        {
                            AyudaProgramacionLN.MapearError(telefono, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    case EstadoColecciones.Borrado:
                        break;
                    #region Modificado
                    case EstadoColecciones.Modificado:
                        telefono.UsuarioLogueado = pParametro.UsuarioLogueado;
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(telefono, bd, tran, "CapProveedoresTelefonosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(telefono, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.Telefonos.Find(x => x.IdTelefono == telefono.IdTelefono), Acciones.Update, telefono, bd, tran))
                        {
                            AyudaProgramacionLN.MapearError(telefono, pParametro);
                            return false;
                        }
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            return true;
        }
        #endregion

    }



}
