using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data.Common;
using Comunes.Entidades;
using Cobros.Entidades;

namespace Cobros.LogicaNegocio
{
    class CobOrdenesCobrosTiposRetencionesLN : BaseLN<CobOrdenesCobrosTiposRetenciones>
    {
        public override bool Agregar(CobOrdenesCobrosTiposRetenciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override bool Modificar(CobOrdenesCobrosTiposRetenciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override CobOrdenesCobrosTiposRetenciones ObtenerDatosCompletos(CobOrdenesCobrosTiposRetenciones pParametro)
        {
            throw new NotImplementedException();
        }

        public override List<CobOrdenesCobrosTiposRetenciones> ObtenerListaFiltro(CobOrdenesCobrosTiposRetenciones pParametro)
        {
            List<CobOrdenesCobrosTiposRetenciones> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobrosTiposRetenciones>("CobOrdenesCobrosTiposRetencionesSeleccionarFiltro", pParametro);
            //foreach (CobOrdenesCobrosTiposRetenciones item in lista)
            //{
            //    item.OrdenesCobrosTiposRetencionesDetalle = BaseDatos.ObtenerBaseDatos().ObtenerLista<CobOrdenesCobrosTiposRetencionesDetalles>("CobOrdenesCobrosTiposRetencionesDetallesSeleccionarFiltro", item);
            //    foreach (CobOrdenesCobrosTiposRetencionesDetalles det in item.OrdenesCobrosTiposRetencionesDetalle)
            //        det.OrdenCobroTipoRetencion = item;
            //}            
            return lista;
        }

        public bool AgregarOrdenesCobrosTiposRetenciones(CobOrdenesCobros pParametro, Database db, DbTransaction tran)
        {
            foreach (CobOrdenesCobrosTiposRetenciones opTipoRetencion in pParametro.OrdenesCobrosTiposRetenciones)
            {
                opTipoRetencion.UsuarioLogueado = pParametro.UsuarioLogueado;
                opTipoRetencion.IdOrdenCobro = pParametro.IdOrdenCobro;
                opTipoRetencion.Estado.IdEstado = (int)Estados.Activo;
                opTipoRetencion.IdOrdenCobroTipoRetencion = BaseDatos.ObtenerBaseDatos().Agregar(opTipoRetencion, db, tran, "CobOrdenesCobrosTiposRetencionesInsertar");
                if (opTipoRetencion.IdOrdenCobroTipoRetencion == 0)
                {
                    AyudaProgramacionLN.MapearError(opTipoRetencion, pParametro);
                    return false;
                }

                //foreach (CobOrdenesCobrosTiposRetencionesDetalles det in opTipoRetencion.OrdenesCobrosTiposRetencionesDetalle)
                //{
                //    det.UsuarioLogueado = pParametro.UsuarioLogueado;
                //    det.OrdenCobroTipoRetencion.IdOrdenCobroTipoRetencion = opTipoRetencion.IdOrdenCobroTipoRetencion;
                //    det.Estado.IdEstado = (int)Estados.Activo;
                //    det.IdOrdenCobroTipoRetencionDetalle = BaseDatos.ObtenerBaseDatos().Agregar(det, db, tran, "CobOrdenesCobrosTiposRetencionesDetallesInsertar");
                //    if (det.IdOrdenCobroTipoRetencionDetalle == 0)
                //    {
                //        AyudaProgramacionLN.MapearError(det, pParametro);
                //        return false;
                //    }
                //}
            }
            return true;
        }
    }
}
