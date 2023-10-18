using Afiliados.Entidades;
using Auditoria;
using Auditoria.Entidades;
using Comunes;
using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Servicio.AccesoDatos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Afiliados.LogicaNegocio
{
    class AfiAfiliadosLN : BaseLN<AfiAfiliados>
    {
        public DataTable ObtenerGrilla(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AfiAfiliadosSeleccionarDescripcionPorFiltroGrilla", pParametro);
        }

        public DataTable ClientesObtenerGrilla(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AfiAfiliadosSeleccionarDescripcionPorFiltroGrillaClientes", pParametro);
        }
        public DataTable PacientesObtenerGrilla(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AfiAfiliadosSeleccionarDescripcionPorFiltroGrillaPacientes", pParametro);
        }

        public DataTable ObtenerGrupoGrilla(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AfiAfiliadosSeleccionarDescripcionPorFiltroGrupo", pParametro);
        }

        public AfiAfiliados ObtenerDatos(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarDescripcion", pParametro);
        }

        public AfiAfiliados ObtenerDatos(AfiAfiliados pParametro, Database bd, DbTransaction tran)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarDescripcion", pParametro, bd, tran);
        }
        public AfiAfiliados ObtenerDatosSocioTitular(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarSocioTitular", pParametro);
        }
        public bool ObtenerDatosAFIP(AfiAfiliados pParametro)
        {
            bool resultado = true; ;
            try
            {
                AFIP.WebServices.ConsultarPadronLN consultarPadronLN = new AFIP.WebServices.ConsultarPadronLN();
                if (consultarPadronLN.Autenticado)
                {
                    AFIP.WebServices.ar.gov.afip.aws.personaReturn persona = consultarPadronLN.ConsultarPadronPorCUIT(pParametro.NumeroDocumento);
                    if (persona.errorConstancia != null)
                    {
                        resultado = false;
                        string separador = string.Empty;
                        foreach (string s in persona.errorConstancia.error)
                        {
                            pParametro.CodigoMensaje = string.Concat(pParametro.CodigoMensaje, separador, s);
                            separador = " - ";
                            return false;
                        }
                    }
                    if (persona.datosGenerales.tipoPersona.ToUpper() == "FISICA")
                    {
                        pParametro.Apellido = string.Concat(persona.datosGenerales.apellido, " ", persona.datosGenerales.nombre);
                        pParametro.RazonSocial = string.Concat(persona.datosGenerales.apellido, " ", persona.datosGenerales.nombre);
                    }
                    else
                    {
                        pParametro.Apellido = persona.datosGenerales.razonSocial;
                        pParametro.RazonSocial = persona.datosGenerales.razonSocial;
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

                    pParametro.CondicionFiscal.IdCondicionFiscal = (int)EnumTGECondicionesFiscales.ConsumidorFinal;
                    if (persona.datosMonotributo != null)
                        pParametro.CondicionFiscal.IdCondicionFiscal = (int)EnumTGECondicionesFiscales.ResponsableMonotributista;
                    else if (persona.datosRegimenGeneral != null)
                    {
                        if (persona.datosRegimenGeneral.impuesto != null)
                        {
                            if (persona.datosRegimenGeneral.impuesto.ToList().Exists(x => x.idImpuesto == 30)) //IVA
                                pParametro.CondicionFiscal.IdCondicionFiscal = (int)EnumTGECondicionesFiscales.IVAResponsableInscripto;
                            else if (persona.datosRegimenGeneral.impuesto.ToList().Exists(x => x.idImpuesto == 32)) //IVA EXENTO
                                pParametro.CondicionFiscal.IdCondicionFiscal = (int)EnumTGECondicionesFiscales.IVASujetoExcento;
                        }
                    }
                }
                else
                {
                    pParametro.CodigoMensaje = consultarPadronLN.CodigoMensaje;
                    resultado = false;
                }
            }
            catch (Exception ex)
            {
                pParametro.CodigoMensaje = ex.Message;
                resultado = false;
            }
            return resultado;
        }

        public List<AfiAfiliados> ObtenerDatosAFIPTxtPorDNI(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliados>("AFIPPadronBuscarPorDNI", pParametro, BaseDatos.conexionErpComun);
        }

        public AfiAfiliados ObtenerPorTipoOperacionRefTipoOperacion(TGETiposOperacionesFiltros pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarPorTipoOperacionRefTipoOperacion", pParametro);
        }

        public List<AfiAfiliadosTipos> AfiliadosObtenerTiposAfiliados(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliadosTipos>("AfiAfiliadosTiposSeleccionar", pParametro);
        }


        public override AfiAfiliados ObtenerDatosCompletos(AfiAfiliados pParametro)
        {
            AfiAfiliados afiliado = BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarDescripcion", pParametro);
            afiliado.Domicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", pParametro);
            afiliado.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTelefonos>("AfiTelefonosSeleccionarDescripcion", pParametro);
            pParametro.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Familiares;
            afiliado.Familiares = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliados>("[AfiAfiliadosSeleccionarPorTipo]", pParametro);
            pParametro.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Apoderados;
            afiliado.Apoderados = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliados>("[AfiAfiliadosSeleccionarPorTipo]", pParametro);
            if (afiliado.Familiares.Count > 0)
            {
                foreach (AfiAfiliados fam in afiliado.Familiares)
                {
                    fam.Domicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", fam);
                    fam.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTelefonos>("AfiTelefonosSeleccionarDescripcion", fam);
                }
            }
            if (afiliado.Apoderados.Count > 0)
            {
                foreach (AfiAfiliados apo in afiliado.Apoderados)
                {
                    apo.Domicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", apo);
                    apo.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTelefonos>("AfiTelefonosSeleccionarDescripcion", apo);
                }
            }
            afiliado.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            afiliado.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            afiliado.AlertasTipos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAlertasTipos>("[AfiAlertasTiposSeleccionarAfiliado]", pParametro);
            return afiliado;
        }

        public AfiPacientes ObtenerDatos(AfiPacientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiPacientes>("AfiAfiliadosSeleccionarDescripcion", pParametro);
        }

        public AfiPacientes ObtenerDatosCompletos(AfiPacientes pParametro)
        {
            AfiPacientes afiliado = BaseDatos.ObtenerBaseDatos().Obtener<AfiPacientes>("AfiAfiliadosSeleccionarDescripcion", pParametro);
            afiliado.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            afiliado.Domicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", pParametro);
            afiliado.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTelefonos>("AfiTelefonosSeleccionarDescripcion", pParametro);
            afiliado.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            return afiliado;
        }

        public AfiTelefonos ObtenerTelefonoCelular(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiTelefonos>("AfiTelefonosSeleccionarCelular", pParametro);
        }

        public List<AfiDomicilios> ObtenerDomicilios(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", pParametro);
        }
        public AfiAfiliados ObtenerPorNumeroSocio(AfiAfiliados pParametro)
        {
            AfiAfiliados afiliado = BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarPorNumeroSocio", pParametro);
            //afiliado.Domicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", pParametro);
            //afiliado.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTelefonos>("AfiTelefonosSeleccionarDescripcion", pParametro);
            //pParametro.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Familiares;
            //afiliado.Familiares = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliados>("[AfiAfiliadosSeleccionarPorTipo]", pParametro);
            //pParametro.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Apoderados;
            //afiliado.Apoderados = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliados>("[AfiAfiliadosSeleccionarPorTipo]", pParametro);
            //if (afiliado.Familiares.Count > 0)
            //{
            //    foreach (AfiAfiliados fam in afiliado.Familiares)
            //    {
            //        fam.Domicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", fam);
            //        fam.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTelefonos>("AfiTelefonosSeleccionarDescripcion", fam);
            //    }
            //}
            //if (afiliado.Apoderados.Count > 0)
            //{
            //    foreach (AfiAfiliados apo in afiliado.Apoderados)
            //    {
            //        apo.Domicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", apo);
            //        apo.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTelefonos>("AfiTelefonosSeleccionarDescripcion", apo);
            //    }
            //}
            //afiliado.Comentarios = TGEGeneralesF.ComentariosObtenerLista(pParametro);
            //afiliado.Archivos = TGEGeneralesF.ArchivosObtenerLista(pParametro);
            //afiliado.AlertasTipos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAlertasTipos>("[AfiAlertasTiposSeleccionarAfiliado]", pParametro);
            return afiliado;
        }
        public List<AfiAfiliados> AfiliadoObtenerTitularFamiliares(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliados>("AfiAfiliadosSeleccionarTitularFamiliares", pParametro);

        }



        public AfiAfiliados ObtenerDatosCompletosCopia(AfiAfiliados pParametro)
        {
            AfiAfiliados afiliado = BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarDescripcion", pParametro);
            afiliado.Domicilios = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiDomicilios>("AfiDomiciliosSeleccionarDescripcion", pParametro);
            afiliado.Telefonos = BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiTelefonos>("AfiTelefonosSeleccionarDescripcion", pParametro);

            afiliado.IdAfiliado = 0;
            afiliado.IdAfiliadoRef = 0;
            afiliado.EstadoColeccion = EstadoColecciones.Agregado;

            foreach (AfiDomicilios dom in afiliado.Domicilios)
            {
                dom.IdAfiliado = 0;
                dom.EstadoColeccion = EstadoColecciones.Agregado;
            }
            foreach (AfiTelefonos tel in afiliado.Telefonos)
            {
                tel.IdAfiliado = 0;
                tel.EstadoColeccion = EstadoColecciones.Agregado;
            }
            return afiliado;
        }

        public AfiAfiliados ObtenerPorTipoDocumento(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarPorTipoDocumento", pParametro);
        }

        public AfiAfiliados ObtenerPorCUIL(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosSeleccionarPorCUIL", pParametro);
        }

        public override List<AfiAfiliados> ObtenerListaFiltro(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiAfiliados>("AfiAfiliadosSeleccionarDescripcionPorFiltro", pParametro);
        }

        public List<AfiPacientes> ObtenerListaFiltro(AfiPacientes pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<AfiPacientes>("AfiPacientesSeleccionarDescripcionPorFiltro", pParametro);
        }

        public List<TGEListasValoresDetalles> ObtenerAfiliadosPagoRecibosCOM(AfiAfiliados pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().ObtenerLista<TGEListasValoresDetalles>("AfiAfiliadosSeleccionarPagoRecibosCOM", pParametro);
        }


        /// <summary>
        /// Devuelve el próximo Numero de Socio
        /// </summary>
        /// <returns></returns>
        public bool ObtenerProximoNumeroSocio(AfiAfiliados pParametro)
        {
            bool resultado = false;
            try
            {
                pParametro.NumeroSocio = BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosObtenerProximoNumeroSocio", pParametro).NumeroSocio;
                resultado = true;
            }
            catch (Exception e)
            {
                pParametro.CodigoMensaje = "ErrorBuscarNumeroSocio";
                pParametro.CodigoMensajeArgs.Add(e.Message);
                resultado = false;
            }
            return resultado;
        }

        private string ObtenerProximoNumeroSocio(Database bd, DbTransaction tran)
        {
            AfiAfiliados afi = new AfiAfiliados();
            return BaseDatos.ObtenerBaseDatos().Obtener<AfiAfiliados>("AfiAfiliadosObtenerProximoNumeroSocio", afi, bd, tran).NumeroSocio;
        }



        public bool AgregarRegistroAsociados(AfiAfiliados pParametro, TGEFormasCobrosAfiliados fca)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            pParametro.FechaAlta = DateTime.Now;
            pParametro.LoteDomicilios = pParametro.ObtenerDomicilios();
            pParametro.LoteFamiliares = pParametro.ObtenerFamiliares();

            //pParametro.LoteCamposValores = pParametro.CamposValores();

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AfiAfiliadosValidacionesRegistroAsociados"))
                return false;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdAfiliado = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiAfiliadosInsertarRegistroAsociados");
                    if (pParametro.IdAfiliado == 0)
                        resultado = false;

                    if (fca.FormaCobro.IdFormaCobro > 0)
                    {
                        fca.IdAfiliado = pParametro.IdAfiliado;
                        if (!TGEGeneralesF.FormasCobrosAfiliadosAgregar(fca))
                        {
                            resultado = false;
                            AyudaProgramacionLN.MapearError(fca, pParametro);
                        }
                    }
                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.IdAfiliado = 0;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.IdAfiliado = 0;
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            AfiAfiliados afi = new AfiAfiliados();
            afi = AyudaProgramacionLN.Clone<AfiAfiliados>(pParametro);
            if (resultado)
                try
                {
                    BaseDatos.ObtenerBaseDatos().EjecutarSP(afi, "AfiAfiliadosEnviarMailRegistroAsociados", BaseDatos.conexionPredeterminada);
                }
                catch (Exception ex)
                {
                }
            return resultado;
        }


        public bool ValidarNumeroDoc(AfiAfiliados pParametro)
        {
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AfiAfiliadosValidacionesRegistroAsociados"))
                return false;

            return true;
        }

        public override bool Agregar(AfiAfiliados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (pParametro.IdAfiliado > 0)
                return true;

            /*  Obtener último numero socio */
            if (pParametro.CalculaNumeroSocio == true)
            {
                ObtenerProximoNumeroSocio(pParametro);
            }

            pParametro.LoteDomicilios = pParametro.ObtenerDomicilios();
            pParametro.LoteTelefonos = pParametro.ObtenerTelefonos();
            if (!this.Validar(pParametro, new AfiAfiliados()))
                return false;

            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //if (pParametro.IdAfiliadoFallecido == 0
                    //    && pParametro.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                    //    pParametro.NumeroSocio = this.ObtenerProximoNumeroSocio(pParametro.Categoria);
                    if (!this.ValidarNumeroSocio(pParametro, bd, tran))
                        return false;

                    pParametro.IdAfiliado = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiAfiliadosInsertar");
                    if (pParametro.IdAfiliado == 0)
                        resultado = false;

                    if (resultado && !this.DomiciliosActualizar(pParametro, new AfiAfiliados(), bd, tran))
                        resultado = false;

                    if (resultado && !this.TelefonoActualizar(pParametro, new AfiAfiliados(), bd, tran))
                        resultado = false;

                    if (resultado && !this.AlertasTiposActualizar(pParametro, new AfiAfiliados(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.IdAfiliado = 0;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.IdAfiliado = 0;
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        public bool DesvincularSocio(AfiAfiliados pParametro)
        {
            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();
            bool resultado = true;
            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    //if (!this.ValidarNumeroSocio(pParametro, bd, tran))
                    //    return false;

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiAfiliadosDesvincularSocio"))
                        resultado = false;

                    if (resultado)
                    {
                        pParametro.ConfirmarAccion = false;
                        pParametro.ConfirmarBajaFamiliares = false;
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
        public override bool Modificar(AfiAfiliados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);

            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Modificado;

            //Obtengo el valor actual del objeto antes de modificarlo
            //para el Historial de Auditoria
            AfiAfiliados valorViejo = new AfiAfiliados();
            valorViejo.IdAfiliado = pParametro.IdAfiliado;
            valorViejo.UsuarioLogueado = pParametro.UsuarioLogueado;
            valorViejo = this.ObtenerDatosCompletos(valorViejo);

            pParametro.LoteDomicilios = pParametro.ObtenerDomicilios();
            pParametro.LoteTelefonos = pParametro.ObtenerTelefonos();

            if (!this.Validar(pParametro, valorViejo))
                return false;

            this.ActualizarFechaCambioEstado(pParametro, valorViejo);

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    if (!this.ValidarNumeroSocio(pParametro, bd, tran))
                        return false;

                    if (!BaseDatos.ObtenerBaseDatos().Actualizar(pParametro, bd, tran, "AfiAfiliadosActualizar"))
                        resultado = false;

                    if (resultado && !AuditoriaF.AuditoriaAgregar(valorViejo, Acciones.Update, pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !this.DomiciliosActualizar(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.TelefonoActualizar(pParametro, valorViejo, bd, tran))
                        resultado = false;

                    if (resultado && !this.AlertasTiposActualizar(pParametro, new AfiAfiliados(), bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ComentariosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.ArchivosActualizar(pParametro, bd, tran))
                        resultado = false;

                    if (resultado && !TGEGeneralesF.CamposActualizarCamposValores(pParametro, bd, tran))
                        resultado = false;

                    if (pParametro.ConfirmarBajaFamiliares
                        && pParametro.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                    {
                        AfiAfiliados afi = new AfiAfiliados();
                        afi.IdAfiliado = pParametro.IdAfiliado;
                        afi.UsuarioLogueado = pParametro.UsuarioLogueado;
                        afi.Estado.IdEstado = (int)EstadosAfiliados.Baja;
                        afi.FechaBaja = DateTime.Now;
                        if (resultado && !BaseDatos.ObtenerBaseDatos().Actualizar(afi, bd, tran, "AfiAfiliadosActualizarFamiliares"))
                        {
                            AyudaProgramacionLN.MapearError(afi, pParametro);
                            resultado = false;
                        }
                    }
                    if (resultado)
                    {
                        pParametro.ConfirmarAccion = false;
                        pParametro.ConfirmarBajaFamiliares = false;
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

        public bool AgregarDomicilio(AfiDomicilios pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            bool resultado = true;
            pParametro.EstadoColeccion = EstadoColecciones.Agregado;

            if (pParametro.IdDomicilio > 0)
                return true;

            pParametro.FechaAlta = DateTime.Now;

            DbTransaction tran;
            DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.CreateDefault();

            using (DbConnection con = bd.CreateConnection())
            {
                con.Open();
                tran = con.BeginTransaction();

                try
                {
                    pParametro.IdDomicilio = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "AfiDomiciliosInsertar");
                    if (pParametro.IdDomicilio == 0)
                    {
                        AyudaProgramacionLN.MapearError(pParametro, pParametro);
                        return false;
                    }

                    if (resultado)
                    {
                        tran.Commit();
                        pParametro.CodigoMensaje = "ResultadoTransaccion";
                    }
                    else
                    {
                        pParametro.IdAfiliado = 0;
                        tran.Rollback();
                    }
                }
                catch (Exception ex)
                {
                    ExceptionHandler.HandleException(ex, "LogicaNegocio");
                    tran.Rollback();
                    pParametro.IdAfiliado = 0;
                    pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                    pParametro.CodigoMensajeArgs.Add(ex.Message);
                    return false;
                }
            }
            return resultado;
        }

        private bool DomiciliosActualizar(AfiAfiliados pParametro, AfiAfiliados pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (AfiDomicilios domicilio in pParametro.Domicilios)
            {
                switch (domicilio.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        domicilio.IdAfiliado = pParametro.IdAfiliado;
                        domicilio.IdDomicilio = BaseDatos.ObtenerBaseDatos().Agregar(domicilio, bd, tran, "AfiDomiciliosInsertar");
                        if (domicilio.IdDomicilio == 0)
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
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(domicilio, bd, tran, "AfiDomiciliosActualizar"))
                        {
                            AyudaProgramacionLN.MapearError(domicilio, pParametro);
                            return false;
                        }
                        if (!AuditoriaF.AuditoriaAgregar(
                            pValorViejo.Domicilios.Find(x => x.IdDomicilio == domicilio.IdDomicilio), Acciones.Update, domicilio, bd, tran))
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

        private bool TelefonoActualizar(AfiAfiliados pParametro, AfiAfiliados pValorViejo, Database bd, DbTransaction tran)
        {
            foreach (AfiTelefonos telefono in pParametro.Telefonos)
            {
                switch (telefono.EstadoColeccion)
                {
                    #region Agregado
                    case EstadoColecciones.Agregado:
                        telefono.IdAfiliado = pParametro.IdAfiliado;
                        telefono.IdTelefono = BaseDatos.ObtenerBaseDatos().Agregar(telefono, bd, tran, "AfiTelefonosInsertar");
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
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(telefono, bd, tran, "AfiTelefonosActualizar"))
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

        private bool AlertasTiposActualizar(AfiAfiliados pParametro, AfiAfiliados pValorViejo, Database pBd, DbTransaction pTran)
        {
            string sp = string.Empty;
            Hashtable param = new Hashtable();
            HistorialCambios cambio = new HistorialCambios();
            cambio.CampoCambiado = "Afiliados -> Alertas Tipos";
            foreach (AfiAlertasTipos alerta in pParametro.AlertasTipos)
            {
                param = new Hashtable();
                param.Add("IdAfiliado", pParametro.IdAfiliado);
                param.Add("IdAlertaTipo", alerta.IdAlertaTipo);
                cambio.ValorViejo = string.Empty;
                cambio.ValorNuevo = string.Empty;

                switch (alerta.EstadoColeccion)
                {
                    case EstadoColecciones.Agregado:
                        sp = "AfiAfiliadosAlertasTiposInsertar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorNuevo = alerta.AlertaTipo;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Insert, pBd, pTran))
                            return false;
                        break;
                    case EstadoColecciones.Borrado:
                        sp = "AfiAfiliadosAlertasTiposBorrar";
                        if (!BaseDatos.ObtenerBaseDatos().Actualizar(param, pBd, pTran, sp))
                            return false;
                        cambio.ValorViejo = alerta.AlertaTipo;
                        if (!AuditoriaF.AuditoriaAgregar(pParametro, cambio, Acciones.Delete, pBd, pTran))
                            return false;
                        break;
                    default:
                        break;
                }
            }
            return true;
        }



        private bool Validar(AfiAfiliados pParametro, AfiAfiliados pValorViejo)
        {
            if (pParametro.TipoDocumento.IdTipoDocumento == (int)EnumTiposDocumentos.CUIT
                || pParametro.TipoDocumento.IdTipoDocumento == (int)EnumTiposDocumentos.CUIL)
            {
                if (!this.ValidarCuit(pParametro.NumeroDocumento.ToString()))
                {
                    pParametro.CodigoMensaje = "CuitIncorrecto";
                    return false;
                }
            }

            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AfiAfiliadosValidaciones"))
                return false;

            AfiAfiliados afiliado = new AfiAfiliados();

            switch (pParametro.EstadoColeccion)
            {
                case EstadoColecciones.SinCambio:
                    break;
                case EstadoColecciones.Agregado:
                    afiliado.FechaAlta = DateTime.Now;

                    ////Valido el Tipo y Numero de Documento para los Socios.
                    //if (pParametro.IdAfiliadoFallecido == 0
                    //    && pParametro.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                    //{
                    //    afiliado.TipoDocumento.IdTipoDocumento = pParametro.TipoDocumento.IdTipoDocumento;
                    //    afiliado.NumeroDocumento = pParametro.NumeroDocumento;
                    //    afiliado.Estado.IdEstado = (int)EstadosTodos.Todos;
                    //    afiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Socios;
                    //    List<AfiAfiliados> lista = this.ObtenerListaFiltro(afiliado);
                    //    if (lista.Count > 0)
                    //    {
                    //        pParametro.CodigoMensaje = "AfiliadosExisteDocumento";
                    //        return false;
                    //    }
                    //}
                    break;
                case EstadoColecciones.AgregadoBorradoMemoria:
                    break;
                case EstadoColecciones.Borrado:
                    break;
                case EstadoColecciones.Modificado:
                    //Validacion Cambio de Estado
                    if ((pParametro.AfiliadoTipo.IdAfiliadoTipo != (int)EnumAfiliadosTipos.Clientes && pParametro.AfiliadoTipo.IdAfiliadoTipo != (int)EnumAfiliadosTipos.PacientesExternos) && (pParametro.Estado.IdEstado == (int)EstadosAfiliados.Baja
                        || pParametro.Estado.IdEstado == (int)EstadosAfiliados.Expulsado
                        || pParametro.Estado.IdEstado == (int)EstadosAfiliados.Renuncia
                        || pParametro.Estado.IdEstado == (int)EstadosAfiliados.Fallecido
                        || pParametro.Estado.IdEstado == (int)EstadosAfiliados.BajaArt171B
                        ))
                    {

                        if (pParametro.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.PacientesExternos)
                            pParametro.FechaBaja = DateTime.Now;
                        //Valido la Fecha de Baja
                        if (!pParametro.FechaBaja.HasValue)
                        {
                            pParametro.CodigoMensaje = "ValidarFechaBaja";
                            return false;
                        }
                        //Valido Confirmacion Baja Familiares para Socio Titular
                        if (!pParametro.ConfirmarBajaFamiliares
                            && pParametro.Estado.IdEstado != pValorViejo.Estado.IdEstado
                            && pParametro.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                        {
                            pParametro.ConfirmarAccion = true;
                            pParametro.CodigoMensaje = "ConfirmarBajaFamiliares";
                            pParametro.CodigoMensajeArgs.Add(pParametro.Estado.Descripcion);
                            return false;
                        }
                        //Validaciones de Baja para Familiares
                        if (pParametro.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Familiares)
                        {
                            //Validar que no sea Cotitular de Ahorro
                            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AfiAfiliadosValidarCotitualres"))
                            {
                                pParametro.CodigoMensaje = "ValidarAfiliadoCotitulares";
                                pParametro.CodigoMensajeArgs.Add(pParametro.Estado.Descripcion);
                                return false;
                            }
                        }
                    }

                    break;
                default:
                    break;
            }
            return true;
        }

        /// <summary>
        /// Valida si un numero de CUIT es valido
        /// </summary>
        /// <param name="cuit"></param>
        /// <returns></returns>
        private bool ValidarCuit(string cuit)
        {
            //return true;
            if (cuit == null)
            {
                return false;
            }
            //Quito los guiones, el cuit resultante debe tener 11 caracteres.
            cuit = cuit.Replace("-", string.Empty);
            if (cuit.Length != 11)
            {
                return false;
            }
            else
            {
                int calculado = CalcularDigitoCuit(cuit);

                int digito = int.Parse(cuit.Substring(10));
                return calculado == digito;
            }
        }

        /// <summary>
        /// Valida si es un cambio de estado y actualiza la fecha de ingreso
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="pValorViejo"></param>
        /// <returns></returns>
        private void ActualizarFechaCambioEstado(AfiAfiliados pParametro, AfiAfiliados pValorViejo)
        {
            if (pParametro.Estado.IdEstado != pValorViejo.Estado.IdEstado)
            {
                {
                    //BAJA --> NORMAL
                    if (pValorViejo.Estado.IdEstado == (int)EstadosAfiliados.Baja
                        && pParametro.Estado.IdEstado == (int)EstadosAfiliados.Normal)
                        pParametro.FechaIngreso = DateTime.Now;
                    //RENUNCIA --> NORMAL
                    if (pValorViejo.Estado.IdEstado == (int)EstadosAfiliados.Renuncia
                        && pParametro.Estado.IdEstado == (int)EstadosAfiliados.Normal)
                        pParametro.FechaIngreso = DateTime.Now;

                    if (pValorViejo.Estado.IdEstado == (int)EstadosAfiliados.BajaArt171B
                        && pParametro.Estado.IdEstado == (int)EstadosAfiliados.Normal)
                        pParametro.FechaIngreso = DateTime.Now;

                    ////Si el estado nuevo es Baja guardo la Fecha de Baja
                    //if (pParametro.Estado.IdEstado == (int)EstadosAfiliados.Baja)
                    //    pParametro.FechaBaja = DateTime.Now;
                    ////Si el estado viejo es Baja borro la fecha de baja
                    //if (pValorViejo.Estado.IdEstado == (int)EstadosAfiliados.Baja)
                    //    pParametro.FechaBaja = Convert.ToDateTime("1753/01/01 00:00:00.000");
                }
            }
        }

        /// <summary>
        /// Calcula el dígito verificador dado un CUIT completo o sin él.
        /// </summary>
        /// <param name="cuit">El CUIT como String sin guiones</param>
        /// <returns>El valor del dígito verificador calculado.</returns>
        private int CalcularDigitoCuit(string cuit)
        {
            int[] mult = new[] { 5, 4, 3, 2, 7, 6, 5, 4, 3, 2 };
            char[] nums = cuit.ToCharArray();
            int total = 0;
            for (int i = 0; i < mult.Length; i++)
            {
                total += int.Parse(nums[i].ToString()) * mult[i];
            }
            var resto = total % 11;
            return resto == 0 ? 0 : resto == 1 ? 9 : 11 - resto;
        }

        /// <summary>
        /// Valida que el Numero de Socio no Exista. Solo para Socios Titualres
        /// </summary>
        /// <param name="pParametro"></param>
        /// <param name="pBd"></param>
        /// <param name="pTran"></param>
        /// <returns></returns>
        private bool ValidarNumeroSocio(AfiAfiliados pParametro, Database pBd, DbTransaction pTran)
        {
            //if (pParametro.AfiliadoTipo.IdAfiliadoTipo != (int)EnumAfiliadosTipos.Socios)
            //    return true;

            //AfiAfiliados filtro = new AfiAfiliados();
            //filtro.NumeroSocio = pParametro.NumeroSocio;
            //filtro.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Socios; //solo me fijo dentro de los socios (afiliados titulares)
            //filtro = this.ObtenerPorNumeroSocio(filtro);
            //if (filtro.IdAfiliado != 0)
            //{
            //    pParametro.CodigoMensaje = "ValidarNumeroSocio";
            //    return false;
            //}
            if (!BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "AfiAfiliadosValidacionesNumeroSocio"))
                return false;

            return true;
        }

        public bool ArmarMailResumenCuenta(AfiAfiliados cliente, MailMessage mail)
        {
            bool resultado = true;

            TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();

            string template = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Templates\\MailEnviarClienteResumenCuenta.htm");
            if (cliente.CorreoElectronico.Trim() != string.Empty)
            {
                if (cliente.CorreoElectronico.Trim().Contains(";"))
                {
                    List<string> lista = cliente.CorreoElectronico.Trim().Split(';').ToList();
                    foreach (string item in lista)
                        mail.To.Add(new MailAddress(item.Trim(), cliente.ApellidoNombre.Trim()));
                }
                else
                    mail.To.Add(new MailAddress(cliente.CorreoElectronico.Trim(), cliente.ApellidoNombre.Trim()));
            }
            mail.Subject = "Resumen de Cuenta";
            mail.IsBodyHtml = true;
            mail.Body = new StreamReader(template).ReadToEnd();
            mail.Body = mail.Body.Replace("%ApellidoNombre%", cliente.ApellidoNombre);
            mail.Body = mail.Body.Replace("%Empresa%", empresa.Empresa);
            //mail.Body = mail.Body.Replace("%EmpresaDatos%", empresa.);

            return resultado;
        }

        public bool ArmarMailLinkFirmarDocumento(AfiAfiliados pParametro, MailMessage mail)
        {
            bool resultado = true;
            TGEPlantillas plantilla = new TGEPlantillas();
            plantilla.Codigo = "AfiliadosMailLinkFirmarDocumento";
            plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            if (pParametro.CorreoElectronico.Trim() != string.Empty)
            {
                if (pParametro.CorreoElectronico.Trim().Contains(";"))
                {
                    List<string> lista = pParametro.CorreoElectronico.Trim().Split(';').ToList();
                    foreach (string item in lista)
                        mail.To.Add(new MailAddress(item.Trim(), pParametro.ApellidoNombre.Trim()));
                }
                else
                    mail.To.Add(new MailAddress(pParametro.CorreoElectronico.Trim(), pParametro.ApellidoNombre.Trim()));
            }
            //TGEFirmarDocumentos firmarDoc = new TGEFirmarDocumentos();
            //firmarDoc.UsuarioLogueado = pParametro.UsuarioLogueado;
            //firmarDoc.IdRefTabla = pParametro.IdPrestamo;
            //firmarDoc.Tabla = "PrePrestamos";
            //firmarDoc.IdTipoOperacion = pParametro.TipoOperacion.IdTipoOperacion;
            //firmarDoc = TGEGeneralesF.FirmarDocumentosArmarLinkFirmarDocumento(firmarDoc);
            //pParametro.LinkFirmarDocumento = firmarDoc.Link;
            mail.IsBodyHtml = true;
            if (plantilla.HtmlPlantilla.Trim().Length > 0)
            {
                string htmlPlantilla = plantilla.HtmlPlantilla;
                List<StringStartEnd> posiciones = AyudaProgramacionLN.FindAllString(htmlPlantilla, "{", "}");
                List<string> campos = new List<string>();
                string campo;
                foreach (StringStartEnd pos in posiciones)
                {
                    campo = htmlPlantilla.Substring(pos.start, pos.end - pos.start).Replace("{", "").Replace("}", "");
                    if (campo.Length > 0)
                        campos.Add(campo);
                }
                AyudaProgramacionLN.MapearEntidad(ref htmlPlantilla, campos, pParametro);
                mail.Subject = HttpUtility.HtmlDecode(AyudaProgramacionLN.StripHtml(AyudaProgramacionLN.getBetween(htmlPlantilla, "[Evol:Asunto]", "[/Evol:Asunto]")
                    .Replace("[Evol:Asunto]", "").Replace("[/Evol:Asunto]", "")).Trim());
                mail.Body = AyudaProgramacionLN.getBetween(htmlPlantilla, "[Evol:Cuerpo]", "[/Evol:Cuerpo]")
                    .Replace("[Evol:Cuerpo]", "").Replace("[/Evol:Cuerpo]", "").Trim();
            }
            else
            {
                resultado = false;
                pParametro.CodigoMensaje = "Falta Definir la Plantilla para Enviar el Mail.";
            }
            return resultado;
        }
        public DataSet ObtenerEstadoCuenta(AfiAfiliados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            return BaseDatos.ObtenerBaseDatos().ObtenerDataSet("AfiAfiliadosObtenerEstadoCuenta", pParametro);
        }
        public DataTable ObtenerDatosCentroMedicoOMINT(AfiAfiliados pParametro)
        {
            AyudaProgramacionLN.LimpiarMensajesError(pParametro);
            return BaseDatos.ObtenerBaseDatos().ObtenerLista("AfiAfiliadosObtenerObtenerDatosCentroMedicoOMINT", pParametro);
        }

     
    }
}
