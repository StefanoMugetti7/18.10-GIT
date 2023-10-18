using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Tesorerias;
using Tesorerias.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using System.Reflection;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasDetalles : PaginaCajas
    {
        public List<TESCajasMovimientos> MisCajasMovimientos
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaCajasMisCajasMovimientos"] != null)
                    return (List<TESCajasMovimientos>)Session[this.MiSessionPagina + "PaginaCajasMisCajasMovimientos"];
                else
                {
                    return (List<TESCajasMovimientos>)(Session[this.MiSessionPagina + "PaginaCajasMisCajasMovimientos"] = new List<TESCajasMovimientos>());
                }
            }
            set { Session[this.MiSessionPagina + "PaginaCajasMisCajasMovimientos"] = value; }
        }

        public bool ComprobantePlantillaCajas
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] != null)
                    return (bool)Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"];
                else
                {
                    return (bool)(Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] = false);
                }
            }
            set { Session[this.MiSessionPagina + "PaginaCajasComprobantePlantillaCajas"] = value; }
        }

        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            base.PageLoadEventCajas(sender, e);

            if (!this.IsPostBack)
            {
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComprobantePlantillaCajas);
                ComprobantePlantillaCajas = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;

                this.MisCajasMovimientos = this.MiCaja.ObtenerCajasMovimientos();
                this.MisCajasMovimientos = AyudaProgramacion.AcomodarIndices<TESCajasMovimientos>(this.MisCajasMovimientos);
                this.gvDatos.DataSource = this.MisCajasMovimientos;
                this.gvDatos.DataBind();
            }
        }
        
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //this.MisCajasMovimientos = this.MisCajasMovimientosPendientes[indiceColeccion];
            TESCajasMovimientos movimiento = new TESCajasMovimientos();
            //movimiento.IdCajaMovimiento = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            int IdTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdTipoOperacion")).Value);
            int IdRefTipoOperacion = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdRefTipoOperacion")).Value);
            int IdCajaMovimiento = Convert.ToInt32(((HiddenField)this.gvDatos.Rows[index].FindControl("hdfIdCajaMovimiento")).Value);
            movimiento.IdRefTipoOperacion = IdRefTipoOperacion;
            movimiento.TipoOperacion.IdTipoOperacion = IdTipoOperacion;
            movimiento.IdCajaMovimiento = IdCajaMovimiento;

            if (e.CommandName == Gestion.Impresion.ToString())
            {
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);
                miPlantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(miPlantilla);
                TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ComprobantePlantillaCajas);
                bool bvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;

                int idComprobante = movimiento.IdCajaMovimiento;
                if (ComprobantePlantillaCajas)
                {

                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.TESCajasMovimientos, "TESCajasMovimientos", movimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                }
                else if (miPlantilla.HtmlPlantilla.Trim().Length > 0)
                {
                    //Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), movimiento.GetType().Name);

                    //if (comprobante != null)
                    //{
                    //TGEPlantillas plantilla = new TGEPlantillas();
                    //plantilla = TGEGeneralesF.PlantillasObtenerDatosPorComprobante((EnumTGEComprobantes)comprobante);


                    Objeto MiTipoOperacion = AyudaProgramacion.ObtenerIdTipoOperacion(movimiento);


                    byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.SinComprobante, miPlantilla.Codigo, MiTipoOperacion, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                    ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Comprobante_", movimiento.IdCajaMovimiento.ToString().PadLeft(10, '0')), this.UsuarioActivo);
                    //}
                }
                else
                {


                    Objeto refTipoOperacion = TesoreriasF.CajasObtenerMovimientoPendienteConfirmacion(movimiento);
                    if (Enum.IsDefined(typeof(EnumTGEComprobantes), refTipoOperacion.GetType().Name))
                    {
                        Object comprobante = Enum.Parse(typeof(EnumTGEComprobantes), refTipoOperacion.GetType().Name);
                        byte[] pdf = ReportesF.ExportPDFGenerarReportePDF((EnumTGEComprobantes)comprobante, "TESCajasMovimientos", movimiento, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                        ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Prestamo_", idComprobante.ToString().PadLeft(10, '0')), this.UsuarioActivo);


                    }
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TESCajasMovimientos movimiento = (TESCajasMovimientos)e.Row.DataItem;
                ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                if (ComprobantePlantillaCajas)
                    ibtnImprimir.Visible = true;
                else
                {
                    switch (movimiento.TipoOperacion.IdTipoOperacion)
                    {
                        case (int)EnumTGETiposOperaciones.DepositoPlazoFijo:
                        case (int)EnumTGETiposOperaciones.RenovacionPlazosFijos:
                        case (int)EnumTGETiposOperaciones.ExtraccionPlazoFijo:
                        case (int)EnumTGETiposOperaciones.CancelacionAnticipadaPlazoFijo:
                        case (int)EnumTGETiposOperaciones.EgresosCajas:
                        case (int)EnumTGETiposOperaciones.IngresosCajas:
                        case (int)EnumTGETiposOperaciones.IngresosCajasInternos:
                        case (int)EnumTGETiposOperaciones.EgresosCajasInternos:
                            ibtnImprimir.Visible = true;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TESCajasMovimientos parametros = this.BusquedaParametrosObtenerValor<TESCajasMovimientos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TESCajasMovimientos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCajasMovimientos;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCajasMovimientos = this.OrdenarGrillaDatos<TESCajasMovimientos>(this.MisCajasMovimientos, e);
            this.gvDatos.DataSource = this.MisCajasMovimientos;
            this.gvDatos.DataBind();
        }
    }
}
