using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using CuentasPagar.Entidades;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Arba.WebServices;

namespace CuentasPagar.LogicaNegocio
{
    class CapOrdenesPagosTiposRetencionesLN : BaseLN<CapOrdenesPagosTiposRetenciones>
    {
        public override bool Agregar(CapOrdenesPagosTiposRetenciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(CapOrdenesPagosTiposRetenciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override CapOrdenesPagosTiposRetenciones ObtenerDatosCompletos(CapOrdenesPagosTiposRetenciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override List<CapOrdenesPagosTiposRetenciones> ObtenerListaFiltro(CapOrdenesPagosTiposRetenciones pParametro)
        {
            List<CapOrdenesPagosTiposRetenciones> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesPagosTiposRetenciones>("CapOrdenesPagosTiposRetencionesSeleccionarFiltro", pParametro);
            foreach (CapOrdenesPagosTiposRetenciones item in lista)
                item.OrdenesPagosTiposRetencionesDetalle = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesPagosTiposRetencionesDetalles>("CapOrdenesPagosTiposRetencionesDetallesSeleccionarFiltro", item);

            return lista;
        }

        public List<CapOrdenesPagosTiposRetenciones> ObtenerListaFiltro(CapOrdenesPagosTiposRetenciones pParametro, Database db, DbTransaction tran)
        {
            List<CapOrdenesPagosTiposRetenciones> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesPagosTiposRetenciones>("CapOrdenesPagosTiposRetencionesSeleccionarFiltro", pParametro, db, tran);
            foreach (CapOrdenesPagosTiposRetenciones item in lista)
                item.OrdenesPagosTiposRetencionesDetalle = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesPagosTiposRetencionesDetalles>("CapOrdenesPagosTiposRetencionesDetallesSeleccionarFiltro", item, db, tran);

            return lista;
        }

        /// <summary>
        /// Devuelve una liasta de Retenciones para ser aplicadas en una Orden de Pago
        /// </summary>
        /// <param name="pParametro"></param>
        /// <returns></returns>
        public List<CapOrdenesPagosTiposRetenciones> ObtenerCalculosRetenciones(CapOrdenesPagos pParametro)
        {
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSArbaConsultaPadron);
            bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;
            if (bvalor)
            {

                
                ConsultarPadronLN padronLN = new ConsultarPadronLN();
                ConsultarPadronEntidad entidad = new ConsultarPadronEntidad();
                entidad.NumeroCUIT = Convert.ToInt64( pParametro.Entidad.Cuit.Replace("-",string.Empty));
                entidad.Fecha = pParametro.FechaPago.HasValue ? pParametro.FechaPago.Value : pParametro.FechaAlta;
                entidad.FechaVigenciaDesde = new DateTime(entidad.Fecha.Year, entidad.Fecha.Month, 1);
                entidad.FechaVigenciaHasta = entidad.FechaVigenciaDesde.AddMonths(1).AddDays(-1);
                if (!padronLN.ConsultarPadron(entidad))
                {
                    AyudaProgramacionLN.MapearError(entidad, pParametro);
              
                }
            }
            string XML = "<SolicitudesPagos>";
            foreach (CapSolicitudPago sp in pParametro.SolicitudesPagos.Where(x => x.IncluirEnOP && x.ImporteParcial!=0))
            {
                XML = string.Concat(XML, "<SolicitudPago><IdSolicitudPago>", sp.IdSolicitudPago.ToString(), "</IdSolicitudPago>",
                    "<IdEntidad>", sp.Entidad.IdEntidad.ToString(), "</IdEntidad>",
                    "<IdRefEntidad>", sp.Entidad.IdRefEntidad.ToString(), "</IdRefEntidad>",
                    "<ImporteParcial>", sp.ImporteParcial.ToString(), "</ImporteParcial>",
                    "</SolicitudPago>");
            }
            XML = string.Concat(XML, "</SolicitudesPagos>");

            pParametro.LoteSP = XML;

            List<CapOrdenesPagosTiposRetencionesDetalles> retencionesDetalles = BaseDatos.ObtenerBaseDatos().ObtenerLista<CapOrdenesPagosTiposRetencionesDetalles>("CapOrdenesPagosCalculosRetenciones", pParametro);
            List<CapOrdenesPagosTiposRetenciones> retenciones = retencionesDetalles
                .GroupBy(x => x.OrdenPagoTipoRetencion.TipoRetencion.IdTipoRetencion)
                .Select(y => new CapOrdenesPagosTiposRetenciones
                    {
                        TipoRetencion = { IdTipoRetencion = y.Key, Descripcion= y.First().OrdenPagoTipoRetencion.TipoRetencion.Descripcion,},
                        NumeroCertificado = y.First().OrdenPagoTipoRetencion.NumeroCertificado,
                        Concepto = y.First().OrdenPagoTipoRetencion.Concepto,
                        ImporteTotalRetencion = y.Sum(z => z.ImporteRetencion)
                    }).ToList();

            foreach (CapOrdenesPagosTiposRetenciones item in retenciones)
                item.OrdenesPagosTiposRetencionesDetalle = AyudaProgramacionLN.ReacomodarIndicesColecion<CapOrdenesPagosTiposRetencionesDetalles>(retencionesDetalles.Where(x => x.OrdenPagoTipoRetencion.TipoRetencion.IdTipoRetencion == item.TipoRetencion.IdTipoRetencion).ToList());

            return retenciones;
        }

        public bool AgregarOrdenesPagosTiposRetenciones(CapOrdenesPagos pParametro, Database db, DbTransaction tran)
        {
            foreach (CapOrdenesPagosTiposRetenciones opTipoRetencion in pParametro.OrdenesPagosTiposRetenciones)
            {
                opTipoRetencion.UsuarioLogueado = pParametro.UsuarioLogueado;
                opTipoRetencion.IdOrdenPago = pParametro.IdOrdenPago;
                opTipoRetencion.Estado.IdEstado = (int)Estados.Activo;
                opTipoRetencion.IdOrdenPagoTipoRetencion = BaseDatos.ObtenerBaseDatos().Agregar(opTipoRetencion, db, tran, "CapOrdenesPagosTiposRetencionesInsertar");
                if (opTipoRetencion.IdOrdenPagoTipoRetencion == 0)
                {
                    AyudaProgramacionLN.MapearError(opTipoRetencion, pParametro);
                    return false;
                }

                foreach (CapOrdenesPagosTiposRetencionesDetalles det in opTipoRetencion.OrdenesPagosTiposRetencionesDetalle)
                {
                    det.UsuarioLogueado = pParametro.UsuarioLogueado;
                    det.OrdenPagoTipoRetencion.IdOrdenPagoTipoRetencion = opTipoRetencion.IdOrdenPagoTipoRetencion;
                    det.Estado.IdEstado = (int)Estados.Activo;
                    det.IdOrdenPagoTipoRetencionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(det, db, tran, "CapOrdenesPagosTiposRetencionesDetallesInsertar");
                    if (det.IdOrdenPagoTipoRetencionDetalle == 0)
                    {
                        AyudaProgramacionLN.MapearError(det, pParametro);
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
