using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Data;
using CrystalDecisions.Web;
using CrystalDecisions.Shared;
using System.Threading;
using Reportes.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using Generales.Entidades;
using System.IO;
using SKP.ASP.Controls;
using Evol.Controls;
using System.Text;
using Servicio.AccesoDatos;
using Comunes.LogicaNegocio;
using CrystalDecisions.CrystalReports.Engine;
using Comunes.Entidades;

namespace IU.Modulos.Reportes
{
    public partial class ReportesSeleccionar : PaginaReportes
    {
        
        private List<RepReportes> MisReportes
        {
            get { return this.PropiedadObtenerValor<List<RepReportes>>("ReportesSeleccionarMisReportes"); }
            set { this.PropiedadGuardarValor("ReportesSeleccionarMisReportes", value); }
        }

        //private DataSet MiResultadoReporte
        //{
        //    get { return this.PropiedadObtenerValor<DataSet>("ReportesSeleccionarMiResultadoReporte"); }
        //    set { this.PropiedadGuardarValor("ReportesSeleccionarMiResultadoReporte", value); }
        //}

        private bool MiActualizarDatosReportes
        {
            get { return this.PropiedadObtenerValor<bool>("ReportesSeleccionarMiModificaParametroValor"); }
            set { this.PropiedadGuardarValor("ReportesSeleccionarMiModificaParametroValor", value); }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                //this.MiResultadoReporte = null;
                this.MiActualizarDatosReportes = false;
                this.MiReporte = new RepReportes();
                this.MiReporte.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.CargarModulosSistema();
                AyudaProgramacion.InsertarItemSeleccione(this.ddlReportes, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                //if (this.MiReporte!= null && this.MiReporte.IdReporte > 0)
                //{
                //    this.ddlModulosSistema.SelectedValue = this.MiReporte.ModulosSistema.IdModulosSistema.ToString();
                //    this.CargarReportes();
                //    //this.ddlReportes.SelectedValue = this.MiReporte.IdReporte.ToString();
                //}
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.IsPostBack)
            {
                if (this.MiReporte != null && this.MiReporte.IdReporte > 0)
                {
                    this.ObtenerValoresRequestForm(this.MiReporte);
                    this.ArmarTablaParametros(this.MiReporte);
                }
            }
        }

        protected void ObtenerValoresRequestForm(RepReportes reporte)
        {
            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("$select2Hdf")).ToList();
            foreach (RepParametros parametro in reporte.Parametros)
            {
                foreach (string k in keys)
                {
                    if (k.EndsWith(parametro.Parametro))
                    {
                        switch (parametro.TipoParametro.IdTipoParametro)
                        {
                            case (int)EnumRepTipoParametros.DropDownListSPAutoComplete:
                                if (k.EndsWith("$select2HdfValue" + parametro.Parametro))
                                    parametro.ValorParametro = this.Request.Form[k];
                                else if (k.EndsWith("$select2HdfText" + parametro.Parametro))
                                    parametro.ValorParametroDescripcion = this.Request.Form[k];
                                break;
                            //case (int)EnumCamposTipos.ComboBoxSP:
                            //    if (k.EndsWith("ListComboBox" + parametro.IdCampo.ToString()))
                            //    {
                            //        parametro.CampoValor.Valor = this.Request.Form[k];
                            //    }
                            //    break;
                        }
                    }
                }
            }
        }       

        private void CargarModulosSistema()
        {
            this.ddlModulosSistema.DataSource = ReportesF.ReportesObtenerModulosPorUsuario(AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            this.ddlModulosSistema.DataTextField = "ModuloSistema";
            this.ddlModulosSistema.DataValueField = "IdModuloSistema";
            this.ddlModulosSistema.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(ddlModulosSistema, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarReportes()
        {
            this.ddlReportes.DataSource = this.MisReportes;
            this.ddlReportes.DataValueField = "IdReporte";
            this.ddlReportes.DataTextField = "Descripcion";
            this.ddlReportes.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(ddlReportes, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlModulosSistema_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MiActualizarDatosReportes = true;
            if (!string.IsNullOrEmpty(ddlModulosSistema.SelectedValue))
            {
                RepReportes reporte = new RepReportes();
                reporte.ModuloSistema.IdModuloSistema = Convert.ToInt32(ddlModulosSistema.SelectedValue);
                reporte.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MisReportes = ReportesF.ReportesObtenerPorModulosPerfiles(reporte);
                this.CargarReportes();
            }
            else
            {
                this.MisReportes = new List<RepReportes>();
                this.CargarReportes();
            }
            
            //this.tablaParametros = new Table();
            this.pnlParametros.Controls.Clear();
            this.txtDetalle.Visible = false;
            this.pnlParametros = new Panel();
            this.pnlParametros.Visible = false;
            this.btnExportarExcel.Visible = false;
            this.btnExportarPDF.Visible = false;
            this.btnExportarTxt.Visible = false;
            this.pnlGrillas.Controls.Clear();
            this.lblMensajeFilas.Visible = false;
            this.upGrillasPantalla.Update();
            this.btnPantalla.Visible = false;            
        }

        protected void ddlReportes_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MiActualizarDatosReportes = true;
            if (!string.IsNullOrEmpty(ddlReportes.SelectedValue))
            {
                this.MiReporte = this.MisReportes.Find(x=>x.IdReporte.ToString()==ddlReportes.SelectedValue);
                //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("ReportesParametros.aspx"), true);
                this.MiReporte = ReportesF.ReportesObtenerUno(this.MiReporte);
                this.txtDetalle.Text = this.MiReporte.Detalle;
                this.txtDetalle.Visible = this.MiReporte.Detalle.Length > 0;
                this.ArmarTablaParametros(this.MiReporte);
                this.pnlParametros.Visible = this.MiReporte.Parametros.Count > 0;
                this.btnExportarExcel.Visible = this.MiReporte.Excel;
                this.btnExportarTxt.Visible = this.MiReporte.Texto;
                this.chkNombreCampos.Visible = this.MiReporte.Texto;
                this.chkSeparador.Visible = this.MiReporte.Texto;
                this.btnExportarPDF.Visible = this.MiReporte.NombreCrystal.Trim().Length > 0;
                this.btnExportarDBF.Visible = this.MiReporte.DBF;
                this.chkSeparador.Checked = this.MiReporte.IncluirSeparador;
                this.chkNombreCampos.Checked = this.MiReporte.IncluirNombreCampos;

                this.pnlGrillas.Controls.Clear();
                this.lblMensajeFilas.Visible = false;
                this.upGrillasPantalla.Update();
                this.btnPantalla.Visible = this.MiReporte.Excel;
            }
            else
            {
                this.txtDetalle.Text = string.Empty;
                this.txtDetalle.Visible = false;
                this.btnExportarExcel.Visible = false;
                this.btnExportarPDF.Visible = false;
                this.pnlParametros.Controls.Clear();
                this.pnlParametros = new Panel();
                this.pnlParametros.Visible = false;
                this.btnExportarTxt.Visible = false;
                this.chkNombreCampos.Visible = false;
                this.chkSeparador.Visible = false;
                this.btnExportarDBF.Visible = false;
                this.pnlGrillas.Controls.Clear();
                this.lblMensajeFilas.Visible = false;
                this.upGrillasPantalla.Update();
                this.btnPantalla.Visible = false;
            }
        }

        protected void btnPantalla_Click(object sender, ImageClickEventArgs e)
        {
            this.Validate();
            if (!this.IsValid)
                return;

            this.pnlGrillas.Controls.Clear();
            this.lblMensajeFilas.Visible = false;
            DataSet datos = new DataSet();
            if (this.CargarDatosReporte(ref datos))
            {
                //ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "gvScrolljs", ResolveUrl("~/assets/js/gridviewscroll.js"));
                GridView gv;
                int gvId = 0;
                LiteralControl lt = new LiteralControl("<br /><br />");
                BoundField bfield;
                foreach (DataTable dt in datos.Tables)
                {
                    if (dt.Rows.Count > 0)
                    {
                        gvId++;
                        gv = new GridView();
                        gv.ID = "gvId" + gvId.ToString();  
                        gv.AutoGenerateColumns = false;
                        gv.ShowFooter = true;
                        //gv.EnabledScrollbar = false;
                        gv.SkinID = "GrillaResponsive";

                        foreach (DataColumn col in dt.Columns)
                        {
                            bfield = new BoundField();
                            bfield.DataField = col.ColumnName;
                            bfield.HeaderText = col.ColumnName;

                            if (col.DataType == typeof(decimal))
                            {
                                bfield.DataFormatString = "{0:N2}";
                                bfield.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                                bfield.FooterText = dt.AsEnumerable().Take(1000).Sum(r => r.Field<decimal?>(col.ColumnName) ?? 0).ToString("N2");
                                bfield.FooterStyle.HorizontalAlign = HorizontalAlign.Right;
                            }
                            else if (col.DataType == typeof(DateTime))
                                bfield.DataFormatString = "{0:dd/MM/yyyy}";
                            
                            gv.Columns.Add(bfield);
                        }


                        gv.DataSource = dt.AsEnumerable().Take(1000).CopyToDataTable();
                        gv.DataBind();
                        gv.UseAccessibleHeader = true;
                        gv.HeaderRow.TableSection = TableRowSection.TableHeader;
                        gv.Width = Unit.Percentage(100);
                        this.pnlGrillas.Controls.Add(gv);
                        this.pnlGrillas.Controls.Add(lt);

                        if(gv.Rows.Count > 100)
                            this.lblMensajeFilas.Visible = true;

                        //string jscript = string.Concat("$(document).ready(function () { $('#", gv.ClientID, "').Scrollable({ScrollHeight: 500, Width:100});});");
                        StringBuilder script = new StringBuilder();
                        script.AppendFormat(" IniciarScroll({0}); ", gv.ID);

                        ScriptManager.RegisterStartupScript(this, this.upGrillasPantalla.GetType(), "EvolGridViewInit" + gv.ClientID, script.ToString(), true);
                    }
                }
            }
            this.upGrillasPantalla.Update();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.Validate();
            if (!this.IsValid)
                return;

            DataSet datos = new DataSet();
            if (this.CargarDatosReporte(ref datos))
                this.ExportarExcelReporte(datos);
        }

        protected void btnExportarTxt_Click(object sender, ImageClickEventArgs e)
        {
            this.Validate();
            if (!this.IsValid)
                return;
            DataSet datos = new DataSet();
            if (this.CargarDatosReporte(ref datos))
                this.ExportarTxtReporte(datos);
        }

        protected void btnExportarPDF_Click(object sender, ImageClickEventArgs e)
        {
            this.Validate();
            if (!this.IsValid)
                return;
            DataSet datos = new DataSet();
            switch (this.MiReporte.IdReporte)
            {
                case (int)EnumReportes.RecibosHaberes:
                    ExportData exportar = new ExportData();
                    this.CargarParametros();
                    this.MiReporte.AppPath = this.Request.PhysicalApplicationPath;
                    TGEArchivos archivo = ReportesF.ReportesRecibosHaberes(this.MiReporte);
                    if (this.ValidarDatosReporte(archivo))
                    {
                        archivo.RutaFisica = this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");
                        archivo.RutaFisica += archivo.NombreArchivo;
                        exportar.ExportGenericFile(this, archivo, true);
                    }
                    break;
                default:
                    if (this.CargarDatosReporte(ref datos))
                    {
                        TGEComprobantes comprobante = new TGEComprobantes();
                        comprobante.NombreSP = this.MiReporte.StoredProcedure;
                        comprobante.NombreRPT = string.Concat("Modulos\\Reportes\\RPT\\", this.MiReporte.NombreCrystal);

                        TGEPlantillas Plantilla = new TGEPlantillas();
                        Plantilla.Codigo = this.MiReporte.CodigoPlantilla;
                        Plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(Plantilla);

                        //byte[] pdf = ReportesF.ExportPDFConvertirHtmlEnPdfMultiple(plantilla, this.MiResultadoReporte, this.MiReporte.KeysPDFCorte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                        byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(comprobante, Plantilla, datos, this.MiReporte.KeysPDFCorte, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                        ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, this.MiReporte.NombreArchivo, this.UsuarioActivo);
                    }
                    break;
            }
        }

        protected void btnExportarDBF_Click(object sender, ImageClickEventArgs e)
        {
            this.Validate();
            if (!this.IsValid)
                return;
            this.CargarParametros();
            TGEArchivos archivo = ReportesF.ReportesDBFObtenerDatos(this.MiReporte);
            if (this.ValidarDatosReporte(archivo))
                this.ExportarDBF(archivo);
        }

        /// <summary>
        /// Carga los datos de los parametros y trae los datos del reporte
        /// </summary>
        /// <returns></returns>
        private bool CargarDatosReporte(ref DataSet ds)
        {
            this.CargarParametros();
            if (this.MiActualizarDatosReportes)
                ds = ReportesF.ReportesObtenerDatos(this.MiReporte);

            if (!this.ValidarDatosReporte(ds))
            {
                this.MiActualizarDatosReportes = true;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Carga los valores de los parametros en el objeto Reporte
        /// </summary>
        private void CargarParametros()
        {
            this.MiReporte.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //Control control;
            foreach (RepParametros parametro in this.MiReporte.Parametros)
            {
                #region comentado
                //foreach (Control tr in this.pnlParametros.Controls)
                //{
                //    ctrl = tr.FindControl(parametro.Parametro);
                //    if (ctrl != null)
                //    {
                //        if (ctrl is TextBox)
                //        {
                //            if (parametro.TipoParametro.IdTipoParametro == (int)EnumRepTipoParametros.DateTime
                //                && parametro.Parametro.ToLower().EndsWith("hasta"))
                //            {
                //                fechaHasta = Convert.ToDateTime(((TextBox)ctrl).Text);
                //                fechaHasta = fechaHasta.AddDays(1);
                //                parametro.ValorParametro = fechaHasta;
                //            }
                //            else
                //            {
                //                parametro.ValorParametro = ((TextBox)ctrl).Text;
                //            }
                //            ctrl = null;
                //            break;
                //        }
                //    }
                //}
                #endregion
                this.CargarParametroValor(parametro);                
            }
        }

        private void CargarParametroValor(RepParametros parametro)
        {
            Control control;
            switch (parametro.TipoParametro.IdTipoParametro)
            {
                case (int)EnumRepTipoParametros.Int:
                case (int)EnumRepTipoParametros.ComboBox:
                    control = this.BuscarControlRecursivo(this.pnlParametros, "lst" + parametro.Parametro);
                    if (control != null)
                    {
                        DropDownList ddl = (DropDownList)control;
                        if (parametro.ValorParametro != null && parametro.ValorParametro.ToString() != ddl.SelectedValue)
                            this.MiActualizarDatosReportes = true;
                        parametro.ValorParametro = ddl.SelectedValue;
                    }
                    break;
                //case (int)EnumRepTipoParametros.ComboBox:
                //        control = this.BuscarControlRecursivo(this.pnlParametros, "cbx" + parametro.Parametro);
                //        if (control != null)
                //        {
                //            ComboBox ddl = (ComboBox)control;
                //            if (parametro.ValorParametro != null && parametro.ValorParametro.ToString() != ddl.SelectedValue)
                //                this.MiActualizarDatosReportes = true;
                //            parametro.ValorParametro = ddl.SelectedValue;
                //        }
                //        break;
                case (int)EnumRepTipoParametros.CheckBoxList:
                    control = this.BuscarControlRecursivo(this.pnlParametros, "cbxLst" + parametro.Parametro);
                    if (control != null)
                    {
                        CheckBoxList chkList = (CheckBoxList)control;
                        string separador = "";
                        string valores = string.Empty;
                        foreach (ListItem item in chkList.Items)
                        {
                            if (item.Selected)
                            {
                                valores = string.Concat(valores, separador, item.Value);
                                separador = ",";
                            }
                        }
                        if (parametro.ValorParametro != null && parametro.ValorParametro.ToString() != valores)
                            this.MiActualizarDatosReportes = true;
                        parametro.ValorParametro = valores;
                    }
                    break;
                case (int)EnumRepTipoParametros.DateTime:
                    control = this.BuscarControlRecursivo(this.pnlParametros, "txtD" + parametro.Parametro);
                    if (control != null)
                    {
                        if (parametro.ValorParametro != null && parametro.ValorParametro.ToString() != ((TextBox)control).Text)
                            this.MiActualizarDatosReportes = true;
                        parametro.ValorParametro = ((TextBox)control).Text;
                    }
                    break;
                case (int)EnumRepTipoParametros.TextBox:
                    control = this.BuscarControlRecursivo(this.pnlParametros, "txt" + parametro.Parametro);
                    if (control != null)
                    {
                        if (parametro.ValorParametro != null && parametro.ValorParametro.ToString() != ((TextBox)control).Text)
                            this.MiActualizarDatosReportes = true;
                        parametro.ValorParametro = ((TextBox)control).Text;
                    }
                    break;
                case (int)EnumRepTipoParametros.IntNumericInput:
                    control = this.BuscarControlRecursivo(this.pnlParametros, "txtNi" + parametro.Parametro);
                    if (control != null)
                    {
                        if (parametro.ValorParametro != null && parametro.ValorParametro.ToString() != ((NumericTextBox)control).Text)
                            this.MiActualizarDatosReportes = true;
                        parametro.ValorParametro = ((NumericTextBox)control).Text;
                    }
                    break;
                case (int)EnumRepTipoParametros.DateTimeRange:
                    break;
                case (int)EnumRepTipoParametros.YearMonthCombo:
                    break;
                default:
                    break;
            }
        }

        private bool ValidarDatosReporte(DataSet ds)
        {
            if (this.MiReporte.ErrorAccesoDatos)
            {
                this.MostrarMensaje(this.MiReporte.CodigoMensaje, true, new List<string>() { this.MiReporte.ErrorException });
                this.MiReporte.ErrorAccesoDatos = false;
                this.MiReporte.CodigoMensaje = string.Empty;
                this.MiReporte.CodigoMensajeArgs = new List<string>();
                this.MiReporte.ErrorException = string.Empty;
                return false;
            }
            if (ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
            {
                this.MostrarMensaje("RepNoHayDatos", true);
                return false;
            }
            return true;
        }

        private bool ValidarDatosReporte(TGEArchivos archivo)
        {
            if (this.MiReporte.ErrorAccesoDatos)
            {
                this.MostrarMensaje(this.MiReporte.CodigoMensaje, true, new List<string>() { this.MiReporte.ErrorException });
                this.MiReporte.ErrorAccesoDatos = false;
                this.MiReporte.CodigoMensaje = string.Empty;
                this.MiReporte.CodigoMensajeArgs = new List<string>();
                this.MiReporte.ErrorException = string.Empty;
                return false;
            }
            if (!File.Exists(archivo.RutaFisica))
            {
                this.MostrarMensaje("RepNoHayDatos", true);
                return false;
            }   
            return true;
        }

        private void ExportarExcelReporte(DataSet ds)
        {
            ExportData exportar = new ExportData();
            exportar.ExportExcel(this, ds, this.chkNombreCampos.Checked, this.MiReporte.NombreArchivo, this.MiReporte.NombreSolapa);
        }

        private void ExportarTxtReporte(DataSet ds)
        {
            string separador = this.chkSeparador.Checked ? (this.MiReporte.Separador == string.Empty ? ";" : this.MiReporte.Separador): string.Empty;
            ExportData exportar = new ExportData();
            exportar.ExportFile(this, ds.Tables[0], separador, this.chkNombreCampos.Checked, this.MiReporte.NombreArchivo);
        }

        private void ExportarPDFReporte(DataSet ds)
        {
            //ds.WriteXmlSchema(string.Concat(this.Page.Request.PhysicalApplicationPath, "\\Modulos\\Reportes\\RPT\\BalanceStockMovimientosDetallesXML.xml"));
            try
            {
                List<Stream> lstArchivos = new List<Stream>();
                CrystalReportSource CryReportSource;
                ParameterFieldDefinitions crParameterdef;
                Stream factura;
                string tipoCopia = string.Empty;
                int cantidadCopias = 1;
                //if (this.MiReporte.IdReporte == 86 || this.MiReporte.IdReporte == 139)
                //    cantidadCopias = 3;
                for (int i = 1; i <= cantidadCopias; i++)
                {
                    switch (i)
                    {
                        case 1:
                            tipoCopia = "ORIGINAL";
                            break;
                        case 2:
                            tipoCopia = "DUPLICADO";
                            break;
                        default:
                            break;
                    }
                    string archivoReporteLeer = string.Concat("Modulos\\Reportes\\RPT\\", this.MiReporte.NombreCrystal.Trim());
                    CryReportSource = new CrystalReportSource();
                    CryReportSource.CacheDuration = 1;
                    CryReportSource.Report.FileName = string.Concat(this.Page.Request.PhysicalApplicationPath, archivoReporteLeer);
                    CryReportSource.ReportDocument.SetDataSource(ds);

                    crParameterdef = CryReportSource.ReportDocument.DataDefinition.ParameterFields;
                    foreach (CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition def in crParameterdef)
                    {
                        if (def.Name.Equals("TipoCopia")) // check if parameter exists in report
                        {
                            CryReportSource.ReportDocument.SetParameterValue("TipoCopia", tipoCopia); // set the parameter value in the report
                        }
                    }
                    factura = CryReportSource.ReportDocument.ExportToStream(ExportFormatType.PortableDocFormat);
                    CryReportSource.ReportDocument.Close();
                    CryReportSource.Dispose();
                    if (factura.Length > 0)
                        lstArchivos.Add(factura);
                }
                ExportData exportar = new ExportData();
                TGEArchivos archivo = new TGEArchivos();
                archivo.NombreArchivo = string.IsNullOrEmpty(this.MiReporte.NombreArchivo) ? this.MiReporte.Descripcion : this.MiReporte.NombreArchivo;
                archivo.TipoArchivo = "pdf";
                if (lstArchivos.Count > 0)
                {
                    archivo.Archivo = PdfMerger.MergeFiles(lstArchivos);
                    exportar.ExportGenericFile(this, archivo, false);
                }
                else
                {
                    this.MostrarMensaje("No se encontraron datos para el reporte.", true);
                }
            }
            catch (ThreadAbortException)
            { }
            catch (Exception ex)
            {
                throw new Exception("Ha ocurrido un error al generar el reporte. - " + ex.Message);
            }
            //CrystalReportSource CryReportSource = new CrystalReportSource();
            //try
            //{
            //    CryReportSource.CacheDuration = 1;
            //    CryReportSource.Report.FileName = string.Concat(this.Page.Request.PhysicalApplicationPath, archivoReporteLeer);
            //    CryReportSource.ReportDocument.SetDataSource(ds);

            //    // Stop buffering the response
            //    Response.Buffer = false;
            //    // Clear the response content and headers
            //    Response.ClearContent();
            //    Response.ClearHeaders();
            //    Response.ContentType = "application/pdf";

            //    CryReportSource.ReportDocument.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, true, this.MiReporte.Descripcion);
            //}
            //catch (CrystalReportsException ex)
            //{
            //    this.MostrarMensaje("RepConvertirPDF", true, new List<string>() { ex.Message });
            //}
            //catch (ThreadAbortException)
            //{
            //    //throw new Exception("Ha ocurrido un error al generar el reporte. - " + ex.Message);
            //}
            //finally
            //{
            //    CryReportSource.ReportDocument.Close();
            //    CryReportSource.ReportDocument.Dispose();
            //    CryReportSource = null;
            //}
        }

        private void ExportarDBF(TGEArchivos archivo)
        {
            ExportData exportar = new ExportData();
            exportar.ExportGenericFile(this, archivo, true);
        }

        #region "ArmarControlesParametros"
        const string cssLabel = "col-sm-3 col-form-label text-right";
        const string cssRow = "form-group row";
        const string cssCol6 = "col-sm-6";
        const string cssCol4 = "col-sm-4";
        const string cssCol3 = "col-sm-3";
        const string cssCol2 = "col-sm-2";
        const string cssCol1 = "col-sm-1";
        private void ArmarTablaParametros(RepReportes pReporte)
        {
            DataSet dsParametros;
            this.pnlParametros.Controls.Clear();

            int countControl = 0;
            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssRow;
            foreach (RepParametros parametro in pReporte.Parametros)
            {
                dsParametros = new DataSet();
                RepReportes rep;
                RepParametros param;

                if (!string.IsNullOrEmpty(parametro.StoredProcedure)
                    && parametro.TipoParametro.IdTipoParametro != (int)EnumRepTipoParametros.DropDownListSPAutoComplete)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    rep = new RepReportes();
                    rep.StoredProcedure = parametro.StoredProcedure;

                    if (parametro.ParamDependiente.Trim() == "#TodasAnteriores#")
                    {
                        foreach (RepParametros p in this.MiReporte.Parametros.Where(x => x.Orden < parametro.Orden))
                        {
                            rep.Parametros.Add(p);
                        }
                    }
                    else
                    {
                        param = new RepParametros();
                        param = this.MiReporte.Parametros.Find(x => x.Parametro == parametro.ParamDependiente);
                        if (param != null)
                        {
                            param.ValorParametro = "0";
                            rep.Parametros.Add(param);
                        }
                    }
                    rep.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    dsParametros = ReportesF.ReportesObtenerDatos(rep);
                }

                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumRepTipoParametros.Int:
                    case (int)EnumRepTipoParametros.ComboBox:
                        pnlRow.Controls.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumRepTipoParametros.CheckBoxList:
                        pnlRow.Controls.Add(AddCheckBoxListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumRepTipoParametros.DropDownListSPAutoComplete:
                        pnlRow.Controls.Add(AddListBoxAutocompleteRow(parametro));
                        break;
                    //case (int)EnumRepTipoParametros.ComboBox:
                    //    pnlRow.Controls.Add(AddComboBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    //    break;
                    case (int)EnumRepTipoParametros.DateTime:
                        pnlRow.Controls.Add(AddDateBoxRow(parametro.NombreParametro, parametro.Parametro, dsParametros));
                        //AddDateControl(parametro.NombreParametro, counter, parametro.Parametro);
                        break;
                    case (int)EnumRepTipoParametros.TextBox:
                        pnlRow.Controls.Add(AddTextBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumRepTipoParametros.IntNumericInput:
                        pnlRow.Controls.Add(AddNumericInputBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumRepTipoParametros.DateTimeRange:
                        pnlRow.Controls.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumRepTipoParametros.YearMonthCombo:
                        break;
                    default:
                        break;
                }
                parametro.ParametroDibujado = true;
                countControl++;

                if (countControl == 1)
                {
                    this.pnlParametros.Controls.Add(pnlRow);
                    countControl = 0;
                    pnlRow = new Panel();
                    pnlRow.CssClass = cssRow;
                }
            }
        }

        private void CargarValoresParametros(RepParametros parametroBuscar, RepParametros paramLlenar)
        {
            DataSet dsParametros = new DataSet();
            RepReportes rep;

            if (!string.IsNullOrEmpty(parametroBuscar.StoredProcedure)
                && parametroBuscar.TipoParametro.IdTipoParametro != (int)EnumRepTipoParametros.DropDownListSPAutoComplete)
            {
                //Obtengo los valores para llenar las opciones del parametro
                rep = new RepReportes();
                rep.StoredProcedure = parametroBuscar.StoredProcedure;
                rep.Parametros.Add(parametroBuscar);
                rep.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                dsParametros = ReportesF.ReportesObtenerDatos(rep);
            }
            this.CargarValoresParametrosControl(paramLlenar, paramLlenar.Parametro, dsParametros);
        }

        private void CargarValoresParametrosControl(RepParametros parametro, string pNombreControl, DataSet dsParametros)
        {
            Control control;
            switch (parametro.TipoParametro.IdTipoParametro)
            {
                case (int)EnumRepTipoParametros.Int:
                case (int)EnumRepTipoParametros.ComboBox:
                    control = this.BuscarControlRecursivo(this.pnlParametros, "lst" + pNombreControl);
                    if (control != null)
                    {
                        DropDownList ddl = (DropDownList)control;
                        ddl.Items.Clear();
                        ddl.SelectedIndex = -1;
                        ddl.SelectedValue = null;
                        ddl.ClearSelection();

                        ddl.DataSource = dsParametros;
                        ddl.DataValueField = pNombreControl;
                        ddl.DataTextField = "Descripcion";
                        ddl.DataBind();
                        AyudaProgramacion.InsertarItemSeleccione(ddl, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                        //ListItem item = new ListItem("Seleccione una opción", "0");
                        //item.Selected = true;
                        //ddl.Items.Add(item);
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));

                    break;
                case (int)EnumRepTipoParametros.CheckBoxList:
                    control = this.BuscarControlRecursivo(this.pnlParametros, "cbxLst" + pNombreControl);
                    if (control != null)
                    {
                        CheckBoxList ddl = (CheckBoxList)control;
                        ddl.Items.Clear();
                        ddl.SelectedValue = null;
                        ddl.DataSource = dsParametros;
                        ddl.DataValueField = pNombreControl;
                        ddl.DataTextField = "Descripcion";
                        ddl.DataBind();
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    break;
                //case (int)EnumRepTipoParametros.ComboBox:
                //    control = this.BuscarControlRecursivo(this.pnlParametros, "cbx" + pNombreControl);
                //    if (control != null)
                //    {
                //        ComboBox ddl = (ComboBox)control;
                //        ddl.Items.Clear();
                //        ddl.SelectedIndex = -1;
                //        ddl.SelectedValue = null;
                //        ddl.ClearSelection();

                //        ddl.DataSource = dsParametros;
                //        ddl.DataValueField = pNombreControl;
                //        ddl.DataTextField = "Descripcion";
                //        ddl.DataBind();
                //        //AyudaProgramacion.InsertarItemSeleccione(ddl, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                //        //ListItem item = new ListItem("Seleccione una opción", "0");
                //        //item.Selected = true;
                //        //ddl.Items.Add(item);
                //    }
                //    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));

                //    break;
                case (int)EnumRepTipoParametros.DateTime:
                    control = this.BuscarControlRecursivo(this.pnlParametros, "txtD" + pNombreControl);
                    if (control != null)
                    {
                        TextBox dateSelector = (TextBox)control;
                        if (dsParametros.Tables.Count > 0)
                        {
                            if (dsParametros.Tables[0].Rows.Count > 0)
                            {
                                if (dsParametros.Tables[0].Columns.Contains(pNombreControl))
                                    dateSelector.Text = Convert.ToDateTime(dsParametros.Tables[0].Rows[0][pNombreControl]).ToShortDateString();
                            }
                        }
                    }
                    control = this.BuscarControlRecursivo(this.pnlParametros, "fechaCalendar" + pNombreControl);
                    if (control != null)
                    {
                        CalendarExtender fechaCalendar = (CalendarExtender)control;
                        if (dsParametros.Tables.Count > 0)
                        {
                            if (dsParametros.Tables[0].Rows.Count > 0)
                            {
                                if (dsParametros.Tables[0].Columns.Contains("FechaDesde"))
                                    fechaCalendar.StartDate = Convert.ToDateTime(dsParametros.Tables[0].Rows[0]["FechaDesde"]);
                                if (dsParametros.Tables[0].Columns.Contains("FechaHasta"))
                                    fechaCalendar.EndDate = Convert.ToDateTime(dsParametros.Tables[0].Rows[0]["FechaHasta"]);
                            }
                        }
                    }
                    break;
                case (int)EnumRepTipoParametros.TextBox:
                    break;
                case (int)EnumRepTipoParametros.IntNumericInput:
                    break;
                case (int)EnumRepTipoParametros.DateTimeRange:
                    //tablaParametros.Rows.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
                    break;
                case (int)EnumRepTipoParametros.YearMonthCombo:
                    break;
                default:
                    break;
            }
        }

        private PlaceHolder AddListBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label miLabel = new Label();
            miLabel.CssClass = cssLabel;
            miLabel.Text = NombreParametro;
            miPanel.Controls.Add(miLabel);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            DropDownList drpFiltro = new DropDownList();
            drpFiltro.CssClass = "form-control select2";

            if (dsDatos.Tables.Count > 0)
            {
                drpFiltro.DataValueField = Parametro;
                drpFiltro.DataTextField = "Descripcion";
                drpFiltro.DataSource = dsDatos;
                drpFiltro.DataBind();
            }
            AyudaProgramacion.InsertarItemSeleccione(drpFiltro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            
            drpFiltro.ID = "lst" + Parametro;
            
            drpFiltro.SelectedIndexChanged += new EventHandler(drpFiltro_SelectedIndexChanged);
            drpFiltro.AutoPostBack = this.MiReporte.Parametros.Exists(x=>x.ParamDependiente==Parametro)
                || this.MiReporte.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");
            

            TextBox Text = new TextBox();
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;

            pnlRow.Controls.Add(drpFiltro);
            pnlRow.Controls.Add(Text);
            miPanel.Controls.Add(pnlRow);

            #region ScriptSelect2
            //StringBuilder script = new StringBuilder();
            //script.AppendLine(" $(document).ready(function () {");
            ////start function
            //script.AppendLine(" function InitControlSelect2|CTRLID|() {");
            //script.AppendFormat("var control = $('select[name$={0}]');", drpFiltro.ID);
            ////select2 start
            //script.AppendLine("control.select2();");
            ////end function
            //script.AppendLine("};");
            //script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlSelect2|CTRLID|);");
            //script.AppendLine("InitControlSelect2|CTRLID|();");
            //script.AppendLine("});");
            //script.Replace("|CTRLID|", drpFiltro.ID);

            //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + drpFiltro.ID.ToString(), script.ToString(), true);

            #endregion

            return miPanel;
        }

        private PlaceHolder AddComboBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            miPanel.ID = "pnl" + Parametro;
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label miLabel = new Label();
            miLabel.CssClass = cssLabel;
            miLabel.ID = "lbl" + Parametro;
            miLabel.Text = NombreParametro;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            ComboBox drpFiltro = new ComboBox();
            drpFiltro.DataValueField = Parametro;
            drpFiltro.DataTextField = "Descripcion";
            drpFiltro.DataSource = dsDatos;
            drpFiltro.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(drpFiltro, string.Empty);
            drpFiltro.ID = "cbx" + Parametro;
            drpFiltro.AutoPostBack = this.MiReporte.Parametros.Exists(x => x.ParamDependiente == Parametro); ;
            drpFiltro.DropDownStyle = ComboBoxStyle.DropDownList;
            drpFiltro.AutoCompleteMode = ComboBoxAutoCompleteMode.Suggest;
            drpFiltro.CaseSensitive = false;
            drpFiltro.CssClass = "select";
            drpFiltro.RenderMode = ComboBoxRenderMode.Inline;
            drpFiltro.MaxLength = 500;

            TextBox Text = new TextBox();
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;

            miPanel.Controls.Add(miLabel);
            pnlRow.Controls.Add(drpFiltro);
            pnlRow.Controls.Add(Text);
            miPanel.Controls.Add(pnlRow);
            return miPanel;
        }

        void drpFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            RepParametros paramValor = this.MiReporte.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID.Remove(0, 3));
            paramValor.ValorParametro = ((DropDownList)sender).SelectedValue;
            List<RepParametros> parametrosLlenar = this.MiReporte.Parametros.FindAll(x => x.ParamDependiente == paramValor.Parametro);
            if (parametrosLlenar != null)
            {
                foreach (RepParametros paramLlenar in parametrosLlenar)
                {
                    RepParametros nuevo = new RepParametros();
                    nuevo.Parametro = paramValor.Parametro;
                    nuevo.StoredProcedure = paramLlenar.StoredProcedure;
                    nuevo.TipoParametro = paramValor.TipoParametro;
                    nuevo.ValorParametro = paramValor.ValorParametro;
                    this.CargarValoresParametros(nuevo, paramLlenar);
                }
            }

            //if (this.MiReporte.Parametros.Count == paramValor.IndiceColeccion + 2
            //        && this.MiReporte.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#"))
            if (this.MiReporte.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden))
            {
                List<RepParametros> lstParamLlenar = this.MiReporte.Parametros.FindAll(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden);
                RepReportes rep;
                foreach (RepParametros p in lstParamLlenar)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    rep = new RepReportes();
                    rep.StoredProcedure = p.StoredProcedure;
                    foreach (RepParametros depParam in this.MiReporte.Parametros.Where(x => x.Orden < p.Orden))
                    {
                        this.CargarParametroValor(depParam);
                        rep.Parametros.Add(depParam);
                    }
                    rep.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    DataSet dsParametros = ReportesF.ReportesObtenerDatos(rep);
                    this.CargarValoresParametrosControl(p, p.Parametro, dsParametros);
                }
            }
        }

        private PlaceHolder AddCheckBoxListBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label miLabel = new Label();
            miLabel.CssClass = cssLabel;
            miLabel.Text = NombreParametro;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            CheckBoxList drpFiltro = new CheckBoxList();
            drpFiltro.RepeatLayout = RepeatLayout.Flow;
            drpFiltro.CssClass = "form-check";
            if (dsDatos.Tables.Count > 0)
            {
                drpFiltro.DataValueField = Parametro;
                drpFiltro.DataTextField = "Descripcion";
                drpFiltro.DataSource = dsDatos;
                drpFiltro.DataBind();
                drpFiltro.ID = "cbxLst" + Parametro;
            }
            miPanel.Controls.Add(miLabel);
            pnlRow.Controls.Add(drpFiltro);
            miPanel.Controls.Add(pnlRow);
            
            return miPanel;
        }

        private PlaceHolder AddListBoxAutocompleteRow(RepParametros parametro)
        {
            PlaceHolder panel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            panel.Controls.Add(pnlCol);
            
            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + parametro.Parametro;
            lbl.Text = parametro.NombreParametro;
            panel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.ID = parametro.Parametro;
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.DataValueField = parametro.Parametro;
            ddlListaOpciones.DataTextField = "Descripcion";
            ddlListaOpciones.SelectedIndexChanged += new EventHandler(drpFiltro_SelectedIndexChanged);
            ddlListaOpciones.AutoPostBack = this.MiReporte.Parametros.Exists(x => x.ParamDependiente == parametro.Parametro)
                || this.MiReporte.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");
            
            //Busco el detalle en la BD
            if (parametro.ValorParametro.ToString().Trim().Length > 0)
            {
                TGECamposValores campoValor = new TGECamposValores();
                campoValor.Valor = parametro.ValorParametro.ToString().Trim();
                DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet(parametro.StoredProcedure, parametro);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    parametro.ValorParametroDescripcion = ds.Tables[0].Rows[0]["Descripcion"].ToString();

                ListItem item = new ListItem(parametro.ValorParametroDescripcion, parametro.ValorParametro.ToString());
                ddlListaOpciones.Items.Add(item);
            }

            HiddenField hdfValue = new HiddenField();
            hdfValue.ID = "select2HdfValue" + parametro.Parametro;
            hdfValue.Value = parametro.ValorParametro.ToString();
            HiddenField hdfText = new HiddenField();
            hdfText.ID = "select2HdfText" + parametro.Parametro;
            hdfText.Value = parametro.ValorParametroDescripcion;
            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfValue);
            pnlRow.Controls.Add(hdfText);
            panel.Controls.Add(pnlRow);

            StringBuilder script = new StringBuilder();
            script.AppendLine(" $(document).ready(function () {");
            //start function
            script.AppendLine(" function InitControlSelect2|CTRLID|() {");
            script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);

            //select2 start
            script.AppendLine("control.select2({");
            script.AppendFormat("placeholder: '{0}',", parametro.NombreParametro);
            script.AppendLine("minimumInputLength: 4,");
            script.AppendLine("language: 'es',");
            script.AppendLine("allowClear: true,");
            script.AppendLine("ajax: {");
            script.AppendLine("type: 'POST',");
            script.AppendLine("contentType: 'application/json; charset=utf-8',");
            script.AppendLine("dataType: 'json',");
            script.AppendFormat("url: '{0}',", ResolveUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
            script.AppendLine("delay: 250,");
            script.AppendLine("data: function (params) {");
            script.AppendLine("return JSON.stringify( {");
            script.AppendLine("value: control.val(), // search term");
            script.AppendLine("filtro: params.term, // search term");
            script.AppendFormat("sp: '{0}',", parametro.StoredProcedure);
            //script.AppendFormat("idRefTablaValor: '{0}',", this.MiIdRefTablaValor);
            script.AppendLine(" });");
            script.AppendLine("},");
            script.AppendLine("processResults: function (data, params) {");
            script.AppendLine("return {");
            script.AppendLine("results: data.d,");
            script.AppendLine("};");
            script.AppendLine(" cache: true");
            script.AppendLine("},");
            script.AppendLine("}");
            script.AppendLine("});");
            //end select2
            //select2 ON Select
            script.AppendLine("control.on('select2:select', function(e) { ");
            script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
            script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
            script.AppendLine("hdfValue.val(e.params.data.id);");
            script.AppendLine("hdfText.val(e.params.data.text);");
            script.AppendLine("});");
            //end select2 ON
            //select2 ON unselect
            script.AppendLine("control.on('select2:unselect', function(e) { ");
            script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
            script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
            //script.AppendLine("control.val(null);");
            script.AppendLine("hdfValue.val(null);");
            script.AppendLine("hdfText.val(null);");
            script.AppendLine("});");
            //end select2 unselect

            script.AppendLine("};");
            //end function
            script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlSelect2|CTRLID|);");
            script.AppendLine("InitControlSelect2|CTRLID|();");
            script.AppendLine("});");

            script.Replace("|CTRLID|", ddlListaOpciones.ID);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);

            return panel;
        }

        private PlaceHolder AddMultipleListBoxRow(string NombreParametro, DataSet dsDatos, int Counter, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label miLabel = new Label();
            miLabel.CssClass = cssLabel;
            miLabel.Text = NombreParametro;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            ListBox lstMultipleList = new ListBox();
            lstMultipleList.CssClass = "form-control";
            lstMultipleList.ID = Parametro; 
            lstMultipleList.SelectionMode = ListSelectionMode.Multiple;
            lstMultipleList.Rows = 8;

            TextBox Text = new TextBox();
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;

            miPanel.Controls.Add(miLabel);
            pnlRow.Controls.Add(lstMultipleList);
            pnlRow.Controls.Add(Text);
            miPanel.Controls.Add(pnlRow);
            return miPanel;
        }

        private PlaceHolder AddTextBoxRow(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label miLabel = new Label();
            miLabel.CssClass = cssLabel;
            miLabel.Text = NombreParametro;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            TextBox Text = new TextBox();
            Text.CssClass = "form-control";
            Text.ID = "txt" + Parametro;
            
            miPanel.Controls.Add(miLabel);
            pnlRow.Controls.Add(Text);
            miPanel.Controls.Add(pnlRow);
            return miPanel;
        }
                
        private PlaceHolder AddNumericInputBoxRow(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label miLabel = new Label();
            miLabel.CssClass = cssLabel;
            miLabel.Text = NombreParametro;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            NumericTextBox txtNumerico = new NumericTextBox();
            txtNumerico.CssClass = "form-control";
            txtNumerico.ID = "txtNi" + Parametro;
            
            miPanel.Controls.Add(miLabel);
            pnlRow.Controls.Add(txtNumerico);
            miPanel.Controls.Add(pnlRow);
            return miPanel;
        }

        private PlaceHolder AddDateRangeBoxRow(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label miLabel = new Label();
            miLabel.CssClass = cssLabel;
            miLabel.Text = NombreParametro;
            miPanel.Controls.Add(miLabel);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            // RANGO DESDE
            TextBox dateSelectorDesde = new TextBox();
            dateSelectorDesde.CssClass = "form-control";
            dateSelectorDesde.ID = "dtDesde" + Parametro;
            pnlRow.Controls.Add(dateSelectorDesde);
            // RANGO HASTA
            TextBox dateSelectorHasta = new TextBox();
            dateSelectorHasta.CssClass = "form-control";
            dateSelectorHasta.ID = "dtHasta" + Parametro;
            pnlRow.Controls.Add(dateSelectorHasta);

            miPanel.Controls.Add(pnlRow);
                        
            return miPanel;
        }

        private PlaceHolder AddDateBoxRow(string NombreParametro, string Parametro, DataSet dsDatos)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label miLabel = new Label();
            miLabel.CssClass = cssLabel;
            miLabel.Text = NombreParametro;
            miPanel.Controls.Add(miLabel);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            TextBox dateSelector = new TextBox();
            dateSelector.CssClass = "form-control datepicker";
             dateSelector.Text = DateTime.Now.ToShortDateString();
            dateSelector.Columns = 10;
            dateSelector.ID = "txtD" + Parametro;
            pnlRow.Controls.Add(dateSelector);
            
            miPanel.Controls.Add(pnlRow);
            Panel pnlCol4 = new Panel();
            pnlCol4.CssClass = cssCol3;
            pnlRow.Controls.Add(pnlCol4);

            if (this.MiReporte.Parametros.Exists(x => x.ParamDependiente == Parametro)
                || this.MiReporte.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#"))
            {
                Button btn = new Button();
                btn.ID = dateSelector.ID + "btnPostBack";
                btn.Style.Add("visibility", "hidden");
                btn.Click += BtnPostBack_Click;
                HiddenField hdfDate = new HiddenField();
                hdfDate.Value = dateSelector.Text;
                hdfDate.ID = dateSelector.ID + "hdfDate";
                pnlRow.Controls.Add(btn);
                pnlRow.Controls.Add(hdfDate);
                dateSelector.Attributes.Add("onchange", "return btnPostBackClick(); ");

                StringBuilder scriptBtn = new StringBuilder();
                scriptBtn.AppendLine("function btnPostBackClick(ctrl) {");
                scriptBtn.AppendFormat("var hdfDate = $('input[id$={0}]');", hdfDate.ID);
                scriptBtn.AppendFormat("var txtFecha = $('input[id$={0}]');", dateSelector.ID);
                scriptBtn.AppendLine(" if (txtFecha.val()!=hdfDate.val()) {");
                scriptBtn.AppendLine(" hdfDate.val(txtFecha.val());");
                scriptBtn.AppendFormat("$(\"[id$='{0}']\").click();", btn.ID);
                scriptBtn.AppendLine("}");
                scriptBtn.AppendLine("}");
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "ScriptAgregar" + btn.ID, scriptBtn.ToString(), true);
                
            }

            return miPanel;
        }

        void BtnPostBack_Click(object sender, EventArgs e)
        {
            string id = ((Button)sender).ID.Remove(0, 4).Replace("btnPostBack", string.Empty);
            RepParametros paramValor = this.MiReporte.Parametros.Find(x => x.Parametro == id);
            Control txt = BuscarControlRecursivo(((Button)sender).Parent, "txtD" + id);
            if (txt != null && txt is TextBox)
                paramValor.ValorParametro = ((TextBox)txt).Text;
            List<RepParametros> parametrosLlenar = this.MiReporte.Parametros.FindAll(x => x.ParamDependiente == paramValor.Parametro);
            if (parametrosLlenar != null)
            {
                foreach (RepParametros paramLlenar in parametrosLlenar)
                {
                    RepParametros nuevo = new RepParametros();
                    nuevo.Parametro = paramValor.Parametro;
                    nuevo.StoredProcedure = paramLlenar.StoredProcedure;
                    nuevo.TipoParametro = paramValor.TipoParametro;
                    nuevo.ValorParametro = paramValor.ValorParametro;
                    this.CargarValoresParametros(nuevo, paramLlenar);
                }
            }

            if (this.MiReporte.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden))
            {
                List<RepParametros> lstParamLlenar = this.MiReporte.Parametros.FindAll(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden);
                RepReportes rep;
                foreach (RepParametros p in lstParamLlenar)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    rep = new RepReportes();
                    rep.StoredProcedure = p.StoredProcedure;
                    foreach (RepParametros depParam in this.MiReporte.Parametros.Where(x => x.Orden < p.Orden))
                    {
                        this.CargarParametroValor(depParam);
                        rep.Parametros.Add(depParam);
                    }
                    rep.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    DataSet dsParametros = ReportesF.ReportesObtenerDatos(rep);
                    this.CargarValoresParametrosControl(p, p.Parametro, dsParametros);
                }
            }
        }

        private PlaceHolder AddYearMonthCombo(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            return miPanel;
        }

        #endregion
        
    }

    // Clase para saber cuando se utiliza este tipo de funcionalidad
    public class YearMonthCombo : System.Web.UI.WebControls.DropDownList
    {

    }
}
