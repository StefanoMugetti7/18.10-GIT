using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestamos.Entidades;
using Comunes.Entidades;
using System.Data;
using Prestamos;
using System.Globalization;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PlanesGrillasDatos : ControlesSeguros
    {

        private PrePrestamosPlanes MiPlan
        {
            get
            {
                return (Session[this.MiSessionPagina + "PlanesGrillasDatosMiPlan"] == null ?
                    (PrePrestamosPlanes)(Session[this.MiSessionPagina + "PlanesGrillasDatosMiPlan"] = new PrePrestamosPlanes()) : (PrePrestamosPlanes)Session[this.MiSessionPagina + "PlanesGrillasDatosMiPlan"]);
            }
            set { Session[this.MiSessionPagina + "PlanesGrillasDatosMiPlan"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrImportarArchivo.ImportarArchivoDatosAceptar += new Comunes.ImportarArchivo.ImportarArchivoAceptarEventHandler(ctrImportarArchivo_ImportarArchivoDatosAceptar);
        }

        public void IniciarControl(PrePrestamosPlanes pParametro, Gestion pGestion)
        {
            this.MiPlan = pParametro;
            this.gvDatos.DataSource = pParametro.PrestamosBancoSolParametros;
            this.gvDatos.DataBind();
            this.btnExportarExcel.Visible = pParametro.PrestamosBancoSolParametros.Count > 0;

            string columnasArchivos = "CantidadCuotas, MontoSolicitado, ImportePrestamo,ImporteCuota, TotalCuota,ImporteSeguro, ImporteGastosAdm, ImporteSellado, CFT,	TEA, TNA";
            this.ctrImportarArchivo.IniciarControl(columnasArchivos);
        }

        public List<PrePrestamosBancoSolParametros> ObtenerLista()
        {
            return this.MiPlan.PrestamosBancoSolParametros;
        }

        void ctrImportarArchivo_ImportarArchivoDatosAceptar(object sender, System.Data.DataTable e)
        {
            List<PrePrestamosBancoSolParametros> listaImportar = new List<PrePrestamosBancoSolParametros>();
            Objeto mostrarErrores = new Objeto();
            PrePrestamosBancoSolParametros item;
            int validarInt = 0;
            decimal validarDecimal = 0;
            int posicion = 0;
            foreach (DataRow fila in e.Rows)
            {
                posicion++;

                if (fila["CantidadCuotas"] == DBNull.Value
                    || Convert.ToInt32(fila["CantidadCuotas"]) == 0)
                    continue;

                #region Validacioens
                CultureInfo culture = System.Threading.Thread.CurrentThread.CurrentCulture;
                if (!int.TryParse(fila["CantidadCuotas"].ToString(), out validarInt))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("CantidadCuotas");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["CantidadCuotas"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["MontoSolicitado"].ToString(), NumberStyles.Currency, culture,  out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("MontoSolicitado");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["MontoSolicitado"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["ImportePrestamo"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("ImportePrestamo");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["ImportePrestamo"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["ImporteCuota"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("ImporteCuota");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["ImporteCuota"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["TotalCuota"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("TotalCuota");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["TotalCuota"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["ImporteSeguro"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("ImporteSeguro");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["ImporteSeguro"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["ImporteGastosAdm"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("ImporteGastosAdm");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["ImporteGastosAdm"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["ImporteSellado"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("ImporteSellado");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["ImporteSellado"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["CFT"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("CFT");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["CFT"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["TEA"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("TEA");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["TEA"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                if (!decimal.TryParse(fila["TNA"].ToString(), NumberStyles.Currency, culture, out validarDecimal))
                {
                    mostrarErrores.CodigoMensajeArgs.Add("TNA");
                    mostrarErrores.CodigoMensajeArgs.Add(fila["TNA"].ToString());
                    mostrarErrores.CodigoMensajeArgs.Add(posicion.ToString());
                    this.MostrarMensaje("ImportarArchivoValidarFormatoCampo", true, mostrarErrores.CodigoMensajeArgs);
                    return;
                }

                #endregion

                item = new PrePrestamosBancoSolParametros();
                item.Estado.IdEstado = (int)Estados.Activo;
                item.EstadoColeccion = EstadoColecciones.Agregado;
                listaImportar.Add(item);
                item.IndiceColeccion = listaImportar.IndexOf(item);

                item.CantidadCuotas = Convert.ToInt32(fila["CantidadCuotas"]);
                item.Neto = fila["MontoSolicitado"].ToString() == string.Empty ? 0 : decimal.Parse(fila["MontoSolicitado"].ToString(), NumberStyles.Currency);
                item.Monto = fila["ImportePrestamo"].ToString() == string.Empty ? 0 : decimal.Parse(fila["ImportePrestamo"].ToString(), NumberStyles.Currency);
                item.ImporteCuota = fila["ImporteCuota"].ToString() == string.Empty ? 0 : decimal.Parse(fila["ImporteCuota"].ToString(), NumberStyles.Currency);
                item.TotalCuota = fila["TotalCuota"].ToString() == string.Empty ? 0 : decimal.Parse(fila["TotalCuota"].ToString(), NumberStyles.Currency);
                item.ImporteSeguro = fila["ImporteSeguro"].ToString() == string.Empty ? 0 : decimal.Parse(fila["ImporteSeguro"].ToString(), NumberStyles.Currency);
                item.TasaAdm = fila["ImporteGastosAdm"].ToString() == string.Empty ? 0 : decimal.Parse(fila["ImporteGastosAdm"].ToString(), NumberStyles.Currency);
                item.Sellado = fila["ImporteSellado"].ToString() == string.Empty ? 0 : decimal.Parse(fila["ImporteSellado"].ToString(), NumberStyles.Currency);
                item.Capital = item.Neto + item.TasaAdm;
                //item.SueldoMinimo = Convert.ToDecimal(fila["SueldoMinimo"]);
                item.CFT = fila["CFT"].ToString() == string.Empty ? 0 : decimal.Parse(fila["CFT"].ToString(), NumberStyles.Currency);
                item.TEA = fila["TEA"].ToString() == string.Empty ? 0 : decimal.Parse(fila["TEA"].ToString(), NumberStyles.Currency);
                item.TNA = fila["TNA"].ToString() == string.Empty ? 0 : decimal.Parse(fila["TNA"].ToString(), NumberStyles.Currency);
            }

            if (listaImportar.Count > 0)
            {
                this.MiPlan.PrestamosBancoSolParametros = new List<PrePrestamosBancoSolParametros>();
                this.MiPlan.PrestamosBancoSolParametros.AddRange(listaImportar);
            }
            this.gvDatos.DataSource = this.MiPlan.PrestamosBancoSolParametros;
            this.gvDatos.DataBind();
            this.btnExportarExcel.Visible = this.MiPlan.PrestamosBancoSolParametros.Count > 0;
            this.upPlanesGrillas.Update();
        }

        protected void btnDescargarPlantilla_Click(object sender, EventArgs e)
        {
            DataTable dt = PrePrestamosF.PrestamosBancoSolParametrosObtener(this.MiPlan);
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ExportData exportar = new ExportData();
            exportar.ExportExcel(this.Page, ds, true, "PlantillaPrestamosGrilla", "PlantillaPrestamosGrilla");
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MiPlan.PrestamosBancoSolParametros;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiPlan.PrestamosBancoSolParametros.Count );
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiPlan.PrestamosBancoSolParametros;
            this.gvDatos.DataBind();
        }
    }
}