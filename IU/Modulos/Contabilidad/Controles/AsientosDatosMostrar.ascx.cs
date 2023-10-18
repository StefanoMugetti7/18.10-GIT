using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Evol.Controls;
using System.Globalization;
using System.Reflection;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class AsientosDatosMostrar : ControlesSeguros
    {

        CtbAsientosContables asiento;
        /// <summary>
        /// Inicializa el control para mostrar un asiento contable
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(Objeto pObjeto)
        {
            if (!this.ValidarPermiso("AsientosConsultar.aspx"))
                return;

            asiento = new CtbAsientosContables();
            PropertyInfo prop = pObjeto.GetType().GetProperties().Where(p => p.GetCustomAttributes(typeof(PrimaryKeyAttribute), false).Count() == 1).ToList()[0];
            asiento.IdRefTipoOperacion = Convert.ToInt32(prop.GetValue(pObjeto, null));
            if (pObjeto.GetType().GetProperty("TipoOperacion") != null)
                asiento.IdTipoOperacion = Convert.ToInt32(pObjeto.GetType().GetProperty("TipoOperacion").GetValue(pObjeto, null).GetType().GetProperty("IdTipoOperacion").GetValue(pObjeto.GetType().GetProperty("TipoOperacion").GetValue(pObjeto, null), null));

            asiento = ContabilidadF.AsientosContablesObtenerDatosCompletosPorTipoOperacion(asiento);

            if (asiento.IdAsientoContable == 0)
                return;

            this.MapearObjetoAControles(asiento);
            this.pnlAsientoMostrar.Visible = true;

        }

        private void MapearObjetoAControles(CtbAsientosContables pAsientoContable)
        {
            this.txtDetalle.Text = pAsientoContable.DetalleGeneral;
            this.txtFiliales.Text = pAsientoContable.Filial.Filial;
            this.txtNumeroAsiento.Text = pAsientoContable.NumeroAsiento;
            this.txtEstado.Text = pAsientoContable.Estado.Descripcion;
            this.txtTipoOperacion.Text = pAsientoContable.TipoOperacion;
            this.txtRefTipoOperacion.Text = pAsientoContable.IdRefTipoOperacion.ToString();
            this.txtFechaAsiento.Text = pAsientoContable.FechaAsiento.ToShortDateString();
            this.txtEjercicioContable.Text = pAsientoContable.EjercicioDescripcion;
            AyudaProgramacion.CargarGrillaListas(pAsientoContable.AsientosContablesDetalles, false, this.gvDatos, true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblDebe = (Label)e.Row.FindControl("lblDebe");
                Label lblHaber = (Label)e.Row.FindControl("lblHaber");
                lblDebe.Text = Convert.ToDecimal(asiento.TotalDebe).ToString("C2");
                lblHaber.Text = Convert.ToDecimal(asiento.TotalHaber).ToString("C2");
            }
        }
    }
}