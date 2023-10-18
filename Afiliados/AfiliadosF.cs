using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Afiliados.Entidades;
using Afiliados.LogicaNegocio;
using Comunes.Entidades;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Net.Mail;
using Afiliados.Entidades.Entidades;
using Generales.Entidades;

namespace Afiliados
{
    public class AfiliadosF
    {
       
        #region Afiliados

        public static DataTable AfiliadosObtenerGrilla(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerGrilla(pAfiliado);
        }

        public static DataTable ClientesObtenerGrilla(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ClientesObtenerGrilla(pAfiliado);
        }  
    
        public static DataTable PacientesObtenerGrilla(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().PacientesObtenerGrilla(pAfiliado);
        }

        public static DataTable AfiliadosObtenerGrupoGrilla(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerGrupoGrilla(pAfiliado);
        }

        public static AfiAfiliados AfiliadosObtenerDatos(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerDatos(pAfiliado);
        }

        public static bool AfiliadosObtenerDatosAFIP(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerDatosAFIP(pAfiliado);
        }

        public static AfiAfiliados AfiliadosObtenerDatos(AfiAfiliados pAfiliado, Database bd, DbTransaction tran)
        {
            return new AfiAfiliadosLN().ObtenerDatos(pAfiliado, bd, tran);
        }

        public static AfiAfiliados AfiliadosObtenerPorTipoOperacionRefTipoOperacion(TGETiposOperacionesFiltros pParametro)
        {
            return new AfiAfiliadosLN().ObtenerPorTipoOperacionRefTipoOperacion(pParametro);
        }

        public static AfiTelefonos AfiliadosObtenerTelefonoCelular(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().ObtenerTelefonoCelular(pParametro);
        }

        public static List<AfiDomicilios> AfiliadosObtenerDomicilios(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().ObtenerDomicilios(pParametro);
        }

        /// <summary>
        /// Devuelve un Afiliado con todos sus datos completos
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static AfiAfiliados AfiliadosObtenerDatosCompletos(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerDatosCompletos(pAfiliado);
        }

        public  static List<AfiAfiliadosTipos> AFiliadosObtenerTiposAfiliados(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().AfiliadosObtenerTiposAfiliados(pAfiliado);
        }

        public static AfiPacientes PacientesObtenerDatos(AfiPacientes pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerDatos(pAfiliado);
        }
        public static AfiPacientes PacientesObtenerDatosCompletos(AfiPacientes pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerDatosCompletos(pAfiliado);
        }
        /// <summary>
        /// Devuelve un Afiliado por Numero de Socio
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static AfiAfiliados AfiliadosObtenerPorNumeroSocio(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerPorNumeroSocio(pAfiliado);
        }

        /// <summary>
        /// Devuelve una  copia de un Afiliado para genera un Alta
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static AfiAfiliados ObtenerDatosCompletosCopia(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().ObtenerDatosCompletosCopia(pParametro);
        }

        /// <summary>
        /// Devuelve un Afiliado por TipoDocumento y Numero
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static AfiAfiliados AfiliadosObtenerPorTipoDocumento(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().ObtenerPorTipoDocumento(pParametro);
        }

        /// <summary>
        /// Devuelve un Afiliado por CUIL
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static AfiAfiliados AfiliadosObtenerPorCUIL(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().ObtenerPorCUIL(pParametro);
        }

        public static List<AfiAfiliados> AfiliadosObtenerDatosAFIPTxtPorDNI(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().ObtenerDatosAFIPTxtPorDNI(pParametro);
        }

        public static List<AfiAfiliados> AfiliadosObtenerTitularFamiliares(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().AfiliadoObtenerTitularFamiliares(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de afiliados filtrada
        /// </summary>
        /// <param name="pAfiliado"></param>
        /// <returns></returns>
        public static List<AfiAfiliados> AfiliadosObtenerListaFiltro(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerListaFiltro(pAfiliado);
        }
        public static List<AfiPacientes> PacientesObtenerListaFiltro(AfiPacientes pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerListaFiltro(pAfiliado);
        }

        public static List<TGEListasValoresDetalles> AfiliadosObtenerAfiliadosPagoRecibosCOM(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().ObtenerAfiliadosPagoRecibosCOM(pParametro);
        }

        /// <summary>
        /// Devuelve el próximo Numero de Socio
        /// </summary>
        /// <returns></returns>
        public static bool AfiliadosObtenerProximoNumeroSocio(AfiAfiliados pParametro)
        {
            return new AfiAfiliadosLN().ObtenerProximoNumeroSocio(pParametro);
        }

        public static bool AfiliadosAgregar(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().Agregar(pAfiliado);
        }
        public static bool AfiliadosAgregarRegistroAsociados(AfiAfiliados pAfiliado, TGEFormasCobrosAfiliados fca)
        {
            return new AfiAfiliadosLN().AgregarRegistroAsociados(pAfiliado, fca);
        }

        public static bool AfiliadosValidar(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ValidarNumeroDoc(pAfiliado);
        }

        public static bool AfiliadosModificar(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().Modificar(pAfiliado);
        }  
        public static bool AfiliadosDesvincularSocio(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().DesvincularSocio(pAfiliado);
        }

        public static bool AfiliadosAgregarDomicilio(AfiDomicilios pParametro)
        {
            return new AfiAfiliadosLN().AgregarDomicilio(pParametro);
        }

        public static bool AfiliadosArmarMailResumenCuenta(AfiAfiliados cliente, MailMessage mail)
        {
            return new AfiAfiliadosLN().ArmarMailResumenCuenta(cliente, mail);
        }

        public static bool AfiliadosArmarMailLinkFirmarDocumento(AfiAfiliados pParametro, MailMessage mail)
        {
            return new AfiAfiliadosLN().ArmarMailLinkFirmarDocumento(pParametro, mail);
        }

        public static DataSet AfiliadosObtenerEstadoCuenta(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerEstadoCuenta(pAfiliado);
        }

        public static DataTable AfiliadosObtenerDatosCentroMedicoOMINT(AfiAfiliados pAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerDatosCentroMedicoOMINT(pAfiliado);
        }
        public static AfiAfiliados AfiliadosObtenerDatosSocioTitular(AfiAfiliados miAfiliado)
        {
            return new AfiAfiliadosLN().ObtenerDatosSocioTitular(miAfiliado);
        }
        #endregion

        #region Afiliados Visitas

        public static bool AfiliadosVisitasAgregar (AfiAfiliadosVisitas pAfiliado)
        {
            return new AfiAfiliadosVisitasLN().Agregar(pAfiliado);
        }

        public static DataTable AfiliadosVisitasObtenerGrilla(AfiAfiliadosVisitas pAfiliado)
        {
            return new AfiAfiliadosVisitasLN().ObtenerListaGrilla(pAfiliado);
        }
        #endregion

        #region Alertas Tipos
        public static List<AfiAlertasTipos> AlertasTiposObtenerListaFiltro(AfiAlertasTipos pParametro)
        {
            return new AfiAlertasTiposLN().ObtenerListaFiltro(pParametro);
        }
        #endregion

        #region Armas

        public static bool ArmasDestinosAgregar(AfiArmasDestinos pParametro)
        {
            return new AfiArmasDestinosLN().Agregar(pParametro);
        }

        public static AfiArmasDestinos ArmasDestinosObtenerDatosCompletos(AfiArmasDestinos pParametro)
        {
            return new AfiArmasDestinosLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool ArmasDestinosModificar(AfiArmasDestinos pParametro)
        {
            return new AfiArmasDestinosLN().Modificar(pParametro);
        }

        public static List<AfiArmasDestinos> ArmasDestinosObtenerListaFiltro(AfiArmasDestinos pParametro)
        {
            return new AfiArmasDestinosLN().ObtenerListaFiltro(pParametro);
        }
        #endregion

        #region Categorias

        /// <summary>
        /// Devuelve una lista de Categorias en estado Activo
        /// </summary>
        /// <returns></returns>
        public static List<AfiCategorias> CategoriasObtenerListaActiva(AfiAfiliados pParametro)
        {
            return new AfiCategoriasLN().ObtenerListaActiva(pParametro);
        }

        public static List<AfiCategorias> CategoriasObtenerListaFiltro(AfiCategorias pParametro)
        {
            return new AfiCategoriasLN().ObtenerListaFiltro(pParametro);
        }

        public static AfiCategorias CategoriaObtenerDatosCompletos(AfiCategorias pParametro)
        {
            return new AfiCategoriasLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool CategoriasAgregar(AfiCategorias pParametro)
        {
            return new AfiCategoriasLN().Agregar(pParametro);
        }

        public static bool CategoriasModificar(AfiCategorias pParametro)
        {
            return new AfiCategoriasLN().Modificar(pParametro);
        }
        public static List<AfiCategorias> CategoriasObtenerTiposCategoria()
        {
            return new AfiCategoriasLN().ObtenerTiposCategorias();
        }
        #endregion

        #region Certificado Supervivencia

        public static AfiCertificadosSupervivencia CertificadosSupervivenciaObtenerDatosCompletos(AfiCertificadosSupervivencia pParametro)
        {
            return new AfiCertificadoSupervivenciaLN().ObtenerDatosCompletos(pParametro);
        }

        /// <summary>
        /// Devuelve una lista de Cuentas de Ahorro del Afiliado
        /// </summary>
        /// <param name="pParametro">IdAfiliado, [IdCuentaTipo]</param>
        /// <returns></returns>
        public static List<AfiCertificadosSupervivencia> CertificadosSupervivenciaObtenerListaFiltro(AfiCertificadosSupervivencia pParametro)
        {
            return new AfiCertificadoSupervivenciaLN().ObtenerListaFiltro(pParametro);
        }

        public static bool CertificadosSupervivenciaAgregar(AfiCertificadosSupervivencia pParametro)
        {
            return new AfiCertificadoSupervivenciaLN().Agregar(pParametro);
        }

        public static bool CertificadosSupervivenciaModificar(AfiCertificadosSupervivencia pParametro)
        {
            return new AfiCertificadoSupervivenciaLN().Modificar(pParametro);
        }

        #endregion

        #region Compensaciones
        public static bool CompensacionesAgregar(AfiCompensaciones pParametro)
        {
            return new AfiCompensacionesLN().Agregar(pParametro);
        }

        public static List<AfiCompensaciones> CompensacionesObtenerListaFiltro(AfiCompensaciones pParametro)
        {
            return new AfiCompensacionesLN().ObtenerListaFiltro(pParametro);
        }

        public static AfiCompensaciones CompensacionesObtenerDatosCompletos(AfiCompensaciones pParametro)
        {
            return new AfiCompensacionesLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool CompensacionesModificar(AfiCompensaciones pParametro)
        {
            return new AfiCompensacionesLN().Modificar(pParametro);
        }

        public static bool CompensacionesAnular(AfiCompensaciones pParametro)
        {
            return new AfiCompensacionesLN().Anular(pParametro);
        }
        #endregion

        #region "Domicilios"

        public static List<AfiDomiciliosTipos> DomiciliosTiposObtenerLista()
        {
            return new AfiDomiciliosTiposLN().ObtenerLista();
        }

        #endregion

        #region Estado Civil

        /// <summary>
        /// Devuelve una lista de Estados Civiles
        /// </summary>
        /// <returns></returns>
        public static List<AfiEstadoCivil> EstadosCivilesObtenerLista()
        {
            return new AfiEstadoCivilLN().ObtenerLista();
        }

        #endregion

        #region Grados

        /// <summary>
        /// Devuevle una lista de grados en estado activo
        /// </summary>
        /// <returns></returns>
        public static List<AfiGrados> GradosObtenerListaActiva()
        {
            return new AfiGradosLN().ObtenerListaActiva();
        }

        #endregion

        #region Grupos Sanguineos
        public static List<AfiGruposSanguineos> GruposSanguineosObtenerListar()
        {
            return new AfiGruposSanguineosLN().ObtenerListaActiva();
        }
        #endregion

        #region Mensajes Alertas

        public static List<AfiMensajesAlertas> MensajesAlertasObtenerListaFiltro(AfiMensajesAlertas pParametro)
        {
            return new AfiMensajesAlertasLN().ObtenerListaFiltro(pParametro);
        }
        public static AfiMensajesAlertas MensajesAlertasObtenerDatosCompletos(AfiMensajesAlertas pParametro)
        {
            return new AfiMensajesAlertasLN().ObtenerDatosCompletos(pParametro);
        }

        public static bool MensajesAlertasAgregar(AfiMensajesAlertas pParametro)
        {
            return new AfiMensajesAlertasLN().Agregar(pParametro);
        }

        public static bool MensajesAlertasModificar(AfiMensajesAlertas pParametro)
        {
            return new AfiMensajesAlertasLN().Modificar(pParametro);
        }

        #endregion

        #region "Parentesco"
        public static List<AfiParentesco> ParentescoObtenerListaActiva()
        {
            return new AfiParentescoLN().ObtenerListaActiva();
        }
        #endregion

        #region Renaper
        public static bool RenaperValidarObtenerDatos(AfiAfiliados pParametro)
        {
            return new RenaperLN().ValidarObtenerDatos(pParametro);
        }
        #endregion

        #region Sexo

        /// <summary>
        /// Devuelve una lista de sexos
        /// </summary>
        /// <returns></returns>
        public static List<AfiSexos> SexoObtenerLista()
        {
            return new AfiSexoLN().ObtenerLista();
        }
        #endregion

        #region "Telefonos"

        public static List<AfiTelefonosTipos> TelefonosTiposObtenerLista()
        {
            return new AfiTelefonosTiposLN().ObtenerLista();
        }

        public static List<AfiEmpresasTelefonicas> EmpresasTelefonicasObtenerLista()
        {
            return new AfiEmpresasTelefonicasLN().ObtenerLista();
        }

        #endregion

        #region Tipo Apoderado

        public static List<AfiTiposApoderados> TiposApoderadosObtenerLista()
        {
            return new AfiTiposApoderadosLN().ObtenerLista();
        }

        #endregion

        #region Tipo Documento

        public static List<AfiTiposDocumentos> TipoDocumentosObtenerLista()
        {
            return new AfiTiposDocumentosLN().ObtenerLista();
        }



        #endregion

        #region Datos UIF
        public static bool AfiAfiliadosDatosUIFAgregar(AfiAfiliadosDatosUIF pParametro)
        {
            return new AfiAfiliadosDatosUIFLN().Agregar(pParametro);
        }
        public static bool AfiAfiliadosDatosUIFModificar(AfiAfiliadosDatosUIF pParametro)
        {
            return new AfiAfiliadosDatosUIFLN().Modificar(pParametro);
        }
        public static AfiAfiliadosDatosUIF AfiliadosObtenerDatosUIFPorIdAfiliado(AfiAfiliadosDatosUIF pAfiliado)
        {
            return new AfiAfiliadosDatosUIFLN().ObtenerDatosCompletosPorIdAfiliado(pAfiliado);
        }

        public static DataTable AfiAfiliadosDatosUIFObtenerMatricesRiesgo(AfiAfiliadosDatosUIF pParametro)
        {
            return new AfiAfiliadosDatosUIFLN().ObtenerMatricesDeRiesgo(pParametro);
        }

        #endregion
    }
}
