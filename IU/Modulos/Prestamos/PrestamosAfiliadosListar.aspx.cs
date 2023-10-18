using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Prestamos.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Prestamos;
using Generales.Entidades;
using Reportes.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using Tesorerias.Entidades;

namespace IU.Modulos.Prestamos
{
    public partial class PrestamosAfiliadosListar : PaginaAfiliados
    {
        private DataTable MisPrestamosAfiliados
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PrestamosListarMisPrestamosAfiliados"]; }
            set { Session[this.MiSessionPagina + "PrestamosListarMisPrestamosAfiliados"] = value; }
        }

        private DataTable MisPrestamosHistoricos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PrestamosListarMisPrestamosHistoricos"]; }
            set { Session[this.MiSessionPagina + "PrestamosListarMisPrestamosHistoricos"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("PrestamosAfiliadosAgregar.aspx");
                this.CargarLista();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName=="Page")
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            PrePrestamos prestamoAfiliado = new PrePrestamos();
            prestamoAfiliado.IdPrestamo = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdPrestamo"].ToString());
            prestamoAfiliado.TipoOperacion.IdTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoOperacion"].ToString());
            //string parametros = string.Format("?IdPrestamo={0}", prestamoAfiliado.IdPrestamo);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdPrestamo", prestamoAfiliado.IdPrestamo);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosConsultar.aspx"), true);
            else if (e.CommandName == "PreAutorizar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosPreAutorizar.aspx"), true);
            else if (e.CommandName == "Autorizar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAutorizar.aspx"), true);
            else if (e.CommandName == Gestion.Anular.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAnular.aspx"), true);
            else if (e.CommandName == Gestion.AnularConfirmar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAnularConfirmado.aspx"), true);
            else if (e.CommandName == "Cancelar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosCancelar.aspx"), true);
            else if (e.CommandName == "AnularCancelar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosCancelar.aspx"), true);
            else if (e.CommandName == "AplicarCheque")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAplicarCheque.aspx"), true);
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                EnumTGEComprobantes enumTGEComprobantes;
                if (prestamoAfiliado.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
                    enumTGEComprobantes = EnumTGEComprobantes.PrePrestamosBancoSol;
                else
                    enumTGEComprobantes = EnumTGEComprobantes.PrePrestamos;

                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                movimiento.IdRefTipoOperacion = prestamoAfiliado.IdPrestamo;
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                movimiento.TipoOperacion.IdTipoOperacion = prestamoAfiliado.TipoOperacion.IdTipoOperacion;
                movimiento.Estado.IdEstado = prestamoAfiliado.Estado.IdEstado;
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(enumTGEComprobantes, miPlantilla.Codigo, prestamoAfiliado, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Prestamo_", prestamoAfiliado.IdPrestamo.ToString().PadLeft(10, '0')), this.UsuarioActivo);

                   //TGEPlantillas plantilla = new TGEPlantillas();
                //plantilla.Codigo = "PrestamosSolicitudOtorgamiento";
                //plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                //if (plantilla.HtmlPlantilla.Trim().Length > 0)
                //{
                //    TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.PrePrestamos);
                //    DataSet ds = ExportPDF.ObtenerDatosReporteComprobante(prestamoAfiliado, comprobante);
                //    ExportPDF.ConvertirHtmlEnPdf(this.UpdatePanel1, plantilla, ds, this.UsuarioActivo);
                //}
                //else
                //{
                //    if (prestamoAfiliado.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
                //        this.ctrPopUpComprobantes.CargarReporte(prestamoAfiliado, EnumTGEComprobantes.PrePrestamosBancoSol);
                //    else
                //        this.ctrPopUpComprobantes.CargarReporte(prestamoAfiliado, EnumTGEComprobantes.PrePrestamos);
                //}
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = this.ValidarPermiso("PrestamosAfiliadosConsultar.aspx");

                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton ibtnPreAutorizar = (ImageButton)e.Row.FindControl("btnPreAutorizar");
                ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnCancelar = (ImageButton)e.Row.FindControl("btnCancelar");
                ImageButton ibtnAnularCancelar = (ImageButton)e.Row.FindControl("btnAnularCancelar");
                ImageButton ibtnAnularConfirmado = (ImageButton)e.Row.FindControl("btnAnularConfirmado");
                ImageButton ibtnAplicarCheque = (ImageButton)e.Row.FindControl("btnAplicarCheque");
                bool permisoAnular = this.ValidarPermiso("PrestamosAfiliadosAnular.aspx");
                DataRowView dr = (DataRowView)e.Row.DataItem;

                switch (Convert.ToInt32(dr["IdEstado"]))
                {
                    case (int)EstadosPrestamos.Activo:
                        ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
                        ibtnPreAutorizar.Visible = this.ValidarPermiso("PrestamosAfiliadosPreAutorizar.aspx");
                        ibtnAnular.Visible = permisoAnular;
                        break;
                    case (int)EstadosPrestamos.Anulado:
                        break;
                    case (int)EstadosPrestamos.Finalizado:
                        break;
                    case (int)EstadosPrestamos.Cancelado:
                        ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
                        break;
                    case (int)EstadosPrestamos.Autorizado:
                        ibtnAnular.Visible = permisoAnular;
                        ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
                        break;
                    case (int)EstadosPrestamos.Confirmado:
                        //if (prestamo.TipoOperacion.IdTipoOperacion ==(int)EnumTGETiposOperaciones.PrestamosLargoPlazo
                        //    || prestamo.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.Prestamos46
                        //    || prestamo.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.Prestamos49)
                            ibtnCancelar.Visible = this.ValidarPermiso("PrestamosAfiliadosCancelar.aspx");
                            ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
                            ibtnAnularConfirmado.Visible = this.ValidarPermiso("PrestamosAfiliadosAnularConfirmado.aspx");
                        if (Convert.ToInt32(dr["IdTipoOperacion"])== (int)EnumTGETiposOperaciones.CompraDeCheque)
                        {
                            ibtnAplicarCheque.Visible = this.ValidarPermiso("PrestamosAfiliadosAplicarCheque.aspx");
                        }
                        break;
                    case (int)EstadosPrestamos.PendienteCancelacion:
                        ibtnAnularCancelar.Visible = this.ValidarPermiso("PrestamosAfiliadosCancelar.aspx");
                        break;
                    case (int)EstadosPrestamos.PreAutorizado:
                        ibtnAutorizar.Visible = this.ValidarPermiso("PrestamosAfiliadosAutorizar.aspx");
                        ibtnAnular.Visible = permisoAnular;
                        break;
                    case (int)EstadosPrestamos.Pendiente:
                        ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
                        ibtnPreAutorizar.Visible = this.ValidarPermiso("PrestamosAfiliadosPreAutorizar.aspx");
                        ibtnAnular.Visible = permisoAnular;
                        break;
                    default:
                        break;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestamosAfiliados.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrePrestamos parametros = this.BusquedaParametrosObtenerValor<PrePrestamos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<PrePrestamos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisPrestamosAfiliados;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPrestamosAfiliados = this.OrdenarGrillaDatos<PrePrestamos>(this.MisPrestamosAfiliados, e);
            this.gvDatos.DataSource = this.MisPrestamosAfiliados;
            this.gvDatos.DataBind();
        }

        protected void gvPrestamosHistoricos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            PrePrestamos prestamoAfiliado = new PrePrestamos();
            prestamoAfiliado.IdPrestamo = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdPrestamo"].ToString());
            //string parametros = string.Format("?IdPrestamo={0}", prestamoAfiliado.IdPrestamo);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdPrestamo", prestamoAfiliado.IdPrestamo);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosConsultar.aspx"), true);
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                //this.ctrPopUpComprobantes.CargarReporte(prestamoAfiliado, EnumTGEComprobantes.PrePrestamos);

                EnumTGEComprobantes enumTGEComprobantes;
                if (prestamoAfiliado.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
                    enumTGEComprobantes = EnumTGEComprobantes.PrePrestamosBancoSol;
                else
                    enumTGEComprobantes = EnumTGEComprobantes.PrePrestamos;

                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                movimiento.IdRefTipoOperacion = prestamoAfiliado.IdPrestamo;
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                movimiento.TipoOperacion.IdTipoOperacion = prestamoAfiliado.TipoOperacion.IdTipoOperacion;
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(enumTGEComprobantes, miPlantilla.Codigo, prestamoAfiliado, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Prestamo_", prestamoAfiliado.IdPrestamo.ToString().PadLeft(10, '0')), this.UsuarioActivo);

            }
        }

        protected void gvPrestamosHistoricos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = this.ValidarPermiso("PrestamosAfiliadosConsultar.aspx");

                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestamosHistoricos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvPrestamosHistoricos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrePrestamos parametros = this.BusquedaParametrosObtenerValor<PrePrestamos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<PrePrestamos>(parametros);

            this.gvPrestamosHistoricos.PageIndex = e.NewPageIndex;
            this.gvPrestamosHistoricos.DataSource = this.MisPrestamosHistoricos;
            this.gvPrestamosHistoricos.DataBind();
        }

        protected void gvPrestamosHistoricos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPrestamosAfiliados = this.OrdenarGrillaDatos<PrePrestamos>(this.MisPrestamosHistoricos, e);
            this.gvPrestamosHistoricos.DataSource = this.MisPrestamosHistoricos;
            this.gvPrestamosHistoricos.DataBind();
        }

        protected void btnImprimir_Click(object sender, ImageClickEventArgs e)
        {
            RepReportes reporte = new RepReportes();
            RepParametros param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdAfiliado";
            param.ValorParametro = this.MiAfiliado.IdAfiliado.ToString();
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdTipoOperacion";
            param.ValorParametro =((int)EnumTGETiposOperaciones.PrestamosCortoPlazo).ToString();
            reporte.Parametros.Add(param);
            param = new RepParametros();
            param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.Int;
            param.Parametro = "IdEstado";
            param.ValorParametro = ((int)EstadosPrestamos.Confirmado).ToString();
            reporte.Parametros.Add(param);
            this.ctrPopUpComprobantes.CargarReporte(reporte, EnumTGEComprobantes.PrePrestamosAnalitico);
            

        }

        private void CargarLista()
        {
            this.MisPrestamosAfiliados = new DataTable();
            this.MisPrestamosHistoricos = new DataTable();
            DataTable lista = PrePrestamosF.PrestamosObtenerPorAfiliado(this.MiAfiliado);
            if (lista.AsEnumerable().ToList().Exists(x => x.Field<int>("IdEstado") == (int)EstadosPrestamos.Activo
                || x.Field<int>("IdEstado") == (int)EstadosPrestamos.Autorizado
                || x.Field<int>("IdEstado") == (int)EstadosPrestamos.Confirmado
                || x.Field<int>("IdEstado") == (int)EstadosPrestamos.PendienteCancelacion
                || x.Field<int>("IdEstado") == (int)EstadosPrestamos.PreAutorizado
                || x.Field<int>("IdEstado") == (int)EstadosPrestamos.RenovacionPendienteConfirmacion
                || x.Field<int>("IdEstado") == (int)EstadosPrestamos.Pendiente))
            {
                this.MisPrestamosAfiliados = lista.AsEnumerable().Where(x => x.Field<int>("IdEstado") == (int)EstadosPrestamos.Activo
                    || x.Field<int>("IdEstado") == (int)EstadosPrestamos.Autorizado
                    || x.Field<int>("IdEstado") == (int)EstadosPrestamos.Confirmado
                    || x.Field<int>("IdEstado") == (int)EstadosPrestamos.PendienteCancelacion
                    || x.Field<int>("IdEstado") == (int)EstadosPrestamos.PreAutorizado
                    || x.Field<int>("IdEstado") == (int)EstadosPrestamos.RenovacionPendienteConfirmacion
                    || x.Field<int>("IdEstado") == (int)EstadosPrestamos.Pendiente).CopyToDataTable();
            }
            if (lista.AsEnumerable().ToList().Exists(p => !MisPrestamosAfiliados.AsEnumerable().Any(p2 => p2.Field<int>("IdPrestamo") == p.Field<int>("IdPrestamo"))))
            {
                this.MisPrestamosHistoricos = lista.AsEnumerable().Where(p => !MisPrestamosAfiliados.AsEnumerable().Any(p2 => p2.Field<int>("IdPrestamo") == p.Field<int>("IdPrestamo"))).CopyToDataTable();
            }
            this.gvDatos.DataSource = this.MisPrestamosAfiliados;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            this.gvPrestamosHistoricos.DataSource = this.MisPrestamosHistoricos;
            this.gvPrestamosHistoricos.DataBind();
            AyudaProgramacion.FixGridView(gvPrestamosHistoricos);
            //this.btnImprimir.Visible = this.MisPrestamosAfiliados.AsEnumerable().Where(x => x.Field<int>("IdTipoOperacion") == (int)EnumTGETiposOperaciones.PrestamosCortoPlazo).ToList().Count > 0;
        }
    }
}
