using System;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProcesosDatos.Entidades;
using ProcesosDatos;
using Reportes.Entidades;
using AjaxControlToolkit;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Servicio.AccesoDatos;
using System.Threading;
using System.Web.Caching;
using Comunes.Entidades;

namespace IU.Modulos.ProcesosDatos.Controles
{
    public partial class ProcesosDatosModificarDatos : ControlesSeguros
    {
        
        private DataSet DataSetResultado
        {
            get { return (DataSet)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosDataSetResultado"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosDataSetResultado"] = value; }
        }

        private string SessionId
        {
            get { return (string)Session["ProcesosDatosModificarSessionId"]; }
            set { Session["ProcesosDatosModificarSessionId"] = value; }
        }
        
        protected SisProcesosProcesamiento MiProcesoProcesamiento
        {
            get { return (SisProcesosProcesamiento)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiProcesoProcesamiento"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiProcesoProcesamiento"] = value; }
        }
        public delegate void ProcesosDatosAceptarEventHandler(object sender, SisProcesosProcesamiento e);
        public event ProcesosDatosAceptarEventHandler ProcesosDatosModificarDatosAceptar;
        public delegate void ProcesosDatosCancelarEventHandler();
        public event ProcesosDatosCancelarEventHandler ProcesosDatosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.ctrMensajesPostBack.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(ctrMensajesPostBack_popUpMensajesPostBackAceptar);
            //if (this.IsPostBack)
            //{
            //    if (this.MiProcesoProcesamiento.Proceso.TieneArchivo)
            //        this.btnContinuar.Visible = true;
            //}

            if (!string.IsNullOrEmpty(Request["__EVENTTARGET"]))
            {
                string target = Request["__EVENTTARGET"] as string;
                if (this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => target.EndsWith("$" + x.Parametro) && x.TipoParametro.IdTipoParametro == (int)EnumSisTipoParametros.DropDownListSPAutoComplete))
                {
                    SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.First(x => target.EndsWith("$" + x.Parametro) && x.TipoParametro.IdTipoParametro == (int)EnumSisTipoParametros.DropDownListSPAutoComplete);
                    this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
                }
            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.IsPostBack)
            {
                if (this.MiProcesoProcesamiento != null && this.MiProcesoProcesamiento.Proceso.IdProceso > 0)
                {
                    this.ObtenerValoresRequestForm(this.MiProcesoProcesamiento.Proceso);
                    this.ArmarTablaParametros(this.MiProcesoProcesamiento.Proceso);
                }
            }
        }

        protected void ObtenerValoresRequestForm(SisProcesos proceso)
        {
            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("$ModifDatos$")).ToList();
            foreach (SisParametros parametro in proceso.Parametros)
            {
                foreach (string k in keys)
                {
                    if (k.EndsWith(parametro.Parametro))
                    {
                        switch (parametro.TipoParametro.IdTipoParametro)
                        {
                            case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                                if (k.EndsWith("select2HdfValue" + parametro.Parametro))
                                    parametro.ValorParametro = this.Request.Form[k];
                                else if (k.EndsWith("select2HdfText" + parametro.Parametro))
                                    parametro.ValorParametroDescripcion = this.Request.Form[k];
                                break;
                        }
                    }
                }
            }
        }       

        void ctrMensajesPostBack_popUpMensajesPostBackAceptar()
        {
            if (!this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.DevuelveResultado)
            {
                if (this.ProcesosDatosModificarDatosAceptar != null)
                    this.ProcesosDatosModificarDatosAceptar(null, this.MiProcesoProcesamiento);
            }
            else
            {
                //this.btnContinuar.Visible = false;
                this.pnlArchivo.Visible = false;
                this.tablaParametros.Visible = false;
                this.btnExportarExcel.Visible = true;
            }
        }

        public void IniciarControl(SisProcesos pParametro)
        {
            //procesando = false;
            this.MiProcesoProcesamiento = new SisProcesosProcesamiento();
            this.MiProcesoProcesamiento.Proceso = ProcesosDatosF.ProcesosObtenerDatosCompletos(pParametro);
            this.lblNombreProceso.Text = this.MiProcesoProcesamiento.Proceso.Descripcion;

            List<SisProcesosArchivos> lista = new List<SisProcesosArchivos>();
            lista.Add(this.MiProcesoProcesamiento.Proceso.ProcesoArchivo);
            this.gvArchivo.DataSource = lista;
            this.gvArchivo.DataBind();
            this.gvDatos.DataSource = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.ProcesosArchivosCampos;
            this.gvDatos.DataBind();

            this.pnlArchivo.Visible = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.ProcesaArchivo;
            if (this.MiProcesoProcesamiento.Proceso.Parametros.Count > 0)
            {
                this.ArmarTablaParametros(this.MiProcesoProcesamiento.Proceso);
            }
        }

        protected void ForEachParametros()
        {
            Control ctrl;
            DateTime fechaHasta;


            foreach (SisParametros parametro in this.MiProcesoProcesamiento.Proceso.Parametros)
            {
                foreach (Control tr in tablaParametros.Controls)
                {
                    ctrl = tr.FindControl(parametro.Parametro);
                    if (ctrl != null)
                    {
                        if (ctrl is TextBox)
                        {
                            if (parametro.TipoParametro.IdTipoParametro == (int)EnumRepTipoParametros.DateTime && parametro.Parametro.ToLower().EndsWith("hasta"))
                            {
                                if (((TextBox)ctrl).Text != string.Empty) { 
                                fechaHasta = ((TextBox)ctrl).Text == string.Empty ? DateTime.MaxValue : Convert.ToDateTime(((TextBox)ctrl).Text);
                                fechaHasta = fechaHasta.AddDays(1);
                                parametro.ValorParametro = fechaHasta;
                                }
                                else
                                {
                                    fechaHasta = ((TextBox)ctrl).Text == string.Empty ? DateTime.MaxValue : Convert.ToDateTime(((TextBox)ctrl).Text);
                                }
                                //parametro.ValorParametroDescripcion = fechaHasta.ToShortDateString();
                            }
                            else
                            {
                                parametro.ValorParametro = ((TextBox)ctrl).Text;
                                //parametro.ValorParametroDescripcion = ((TextBox)ctrl).Text;
                            }
                            ctrl = null;
                            break;
                        }
                        else if (ctrl is YearCombo)
                        {
                            parametro.ValorParametro = ((YearCombo)ctrl).SelectedValue;
                            parametro.ValorParametroDescripcion = ((YearCombo)ctrl).SelectedItem.Text;
                            //MiProceso.Anio = ((YearCombo)ctrl).SelectedValue;
                            break;
                        }
                        else if (ctrl is MonthCombo)
                        {
                            parametro.ValorParametro = ((MonthCombo)ctrl).SelectedValue;
                            parametro.ValorParametroDescripcion = ((MonthCombo)ctrl).SelectedItem.Text;
                            //MiProceso.Mes = ((MonthCombo)ctrl).SelectedValue;
                            break;
                        }
                        else if (ctrl is DropDownList)
                        {
                            parametro.ValorParametro = ((DropDownList)ctrl).SelectedValue;
                            parametro.ValorParametroDescripcion = ((DropDownList)ctrl).SelectedValue == string.Empty ? string.Empty : ((DropDownList)ctrl).SelectedItem.Text;
                            break;
                        }
                        else if (ctrl is AsyncFileUpload)
                        {
                            parametro.ValorParametro = ((AsyncFileUpload)ctrl).FileBytes;
                            parametro.ValorParametroDescripcion = ((AsyncFileUpload)ctrl).FileName;
                            break;
                        }
                        else if (ctrl is CheckBoxList)
                        {
                            parametro.ValorParametro = string.Empty;
                            CheckBoxList chkList = (CheckBoxList)ctrl;
                            string separador = "";
                            foreach (ListItem item in chkList.Items)
                            {
                                if (item.Selected)
                                {
                                    parametro.ValorParametro = string.Concat(parametro.ValorParametro, separador, item.Value);
                                    parametro.ValorParametroDescripcion = string.Concat(parametro.ValorParametroDescripcion, separador, item.Text);
                                    separador = ",";
                                }
                            }
                            break;
                        }
                        else if (ctrl is GridViewCheckFecha)
                        {
                            GridViewCheckFecha gvCheckFecha = (GridViewCheckFecha)ctrl;
                            if (!gvCheckFecha.ObtenerValores(parametro))
                                return;
                            break;
                        }
                        else if (ctrl is GridViewCheckDinamico)
                        {
                            GridViewCheckDinamico gvCheckDinamico = (GridViewCheckDinamico)ctrl;
                            if (!gvCheckDinamico.ObtenerValores(parametro))
                                return;
                            break;
                        }
                        else if (ctrl is GridViewCheckImporte)
                        {
                            GridViewCheckImporte gvCheckImporte = (GridViewCheckImporte)ctrl;
                            if (!gvCheckImporte.ObtenerValores(parametro))
                                return;
                            break;
                        }
                    }
                }
            }
        }

        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            //procesando = true;
            //this.MisMensajes = new List<string>();
            this.btnProcesar.Visible = false;

            
            if (this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.ProcesaArchivo &&
                !this.MiProcesoProcesamiento.Proceso.TieneArchivo)
            {
                this.btnProcesar.Visible = true;
                //this.MostrarMensaje("ValidarSubirArchivo", true);
                this.MiProcesoProcesamiento.CodigoMensaje = "ValidarSubirArchivo";
                HttpRuntime.Cache.Insert(Session.SessionID + "CacheProcesoProcesando", "#ERROR", null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                HttpRuntime.Cache.Insert(Session.SessionID + "objProcesoProcesamiento", this.MiProcesoProcesamiento, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                //ScriptManager.RegisterStartupScript(this, this.GetType(), "fnErrorValidacionesScript", " fnErrorValidaciones();", true);
                return;
            }

            this.ObtenerValoresRequestForm(this.MiProcesoProcesamiento.Proceso);

            #region For Each de los parametros
            ForEachParametros();
            #endregion
            this.MiProcesoProcesamiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "wsEjecutarProcesoScript", " fnEjecutarProceso();", true);
            
            HttpRuntime.Cache.Insert(Session.SessionID + "CacheProcesoProcesando", "#PROCESANDO", null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
            Thread workerThread = new Thread(new ParameterizedThreadStart(EjecutarProceso));
            workerThread.CurrentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            workerThread.CurrentUICulture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            //var newCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            //newCulture.NumberFormat.CurrencyDecimalDigits = 2;
            //newCulture.NumberFormat.CurrencyDecimalSeparator = ",";
            //newCulture.NumberFormat.CurrencyGroupSeparator = ".";
            //newCulture.NumberFormat.CurrencyGroupSizes = new int[] { 3, 3, 3, 3 };
            //newCulture.NumberFormat.CurrencySymbol = "$ ";
            //newCulture.NumberFormat.CurrencyNegativePattern = 12; // $ -n
            //workerThread.CurrentCulture = newCulture;
            this.MiProcesoProcesamiento.Filtro = Session.SessionID;
            workerThread.Start(this.MiProcesoProcesamiento);
        }
        
        private void EjecutarProceso(object pProcesoProcesamiento)
        {
            bool resultado = false;
                SisProcesosProcesamiento objProc = (SisProcesosProcesamiento)pProcesoProcesamiento;
                this.SessionId = this.MiProcesoProcesamiento.Filtro;

                if (!objProc.Proceso.ProcesoArchivo.DevuelveResultado)
                {
                    //resultado = ProcesosDatosF.ProcesosEjecutarProcesos(this.MiProcesoProcesamiento);
                    global::ProcesosDatos.LogicaNegocio.ProcesosDatosLN procDatosLN = new global::ProcesosDatos.LogicaNegocio.ProcesosDatosLN();
                    procDatosLN.ProcesoDatosEjecutarSPMensajesCallback += new global::ProcesosDatos.LogicaNegocio.ProcesosDatosLN.ProcesosDatosEjecutarSPMensajes(procDatosLN_ProcesoDatosEjecutarSPMensajesCallback);
                    resultado = procDatosLN.EjecutarProcesos(objProc);
                    procDatosLN = null;
                }
                else
                {
                    DataSet ds = new DataSet();
                    resultado = ProcesosDatosF.ProcesosEjecutarProcesos(objProc, ref ds);
                    this.DataSetResultado = ds.Copy();
                }
                string CacheProcesoProcesando;
                if (resultado)
                    CacheProcesoProcesando = "#FINALIZADO";
                else
                    CacheProcesoProcesando = "#ERROR";
                HttpRuntime.Cache.Insert(this.SessionId + "CacheProcesoProcesando", CacheProcesoProcesando, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                HttpRuntime.Cache.Insert(this.SessionId + "Resultado",resultado, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
                HttpRuntime.Cache.Insert(this.SessionId + "objProcesoProcesamiento", objProc, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
        }

        protected void btnFinalizarProceso_Click(object sender, EventArgs e)
        {
            HttpRuntime.Cache.Remove(Session.SessionID + "CacheMensajes");
            HttpRuntime.Cache.Remove(Session.SessionID + "CacheProcesoProcesando");
            object proc = HttpRuntime.Cache.Get(Session.SessionID + "Resultado");
            bool resultado = false;
            if (proc != null)
                Boolean.TryParse(proc.ToString(), out resultado);
            HttpRuntime.Cache.Remove(Session.SessionID + "Resultado");

            SisProcesosProcesamiento sisProcProcesa = (SisProcesosProcesamiento)HttpRuntime.Cache.Get(Session.SessionID + "objProcesoProcesamiento");
            HttpRuntime.Cache.Remove(Session.SessionID + "objProcesoProcesamiento");

            if (resultado)
            {
                //this.ctrMensajesPostBack.MostrarMensaje(this.ObtenerMensajeSistema(this.MiProcesoProcesamiento.CodigoMensaje));
                this.btnProcesar.Visible = false;
                this.MostrarMensaje(sisProcProcesa.CodigoMensaje, false);
            }
            else
            {
                this.btnProcesar.Visible = true;
                if (sisProcProcesa == null)
                {
                    sisProcProcesa = new SisProcesosProcesamiento();
                    sisProcProcesa.CodigoMensaje = "El proceso no se pudo ejecutar. Vuelvalo a intentar.";
                }
                this.MostrarMensaje(this.ObtenerMensajeSistema(sisProcProcesa.CodigoMensaje), true, sisProcProcesa.CodigoMensajeArgs);
                if (sisProcProcesa.dsResultado != null)
                    this.ctrPopUpGrilla.IniciarControl(sisProcProcesa);
            }
        }

        void procDatosLN_ProcesoDatosEjecutarSPMensajesCallback(List<string> e)
        {
            HttpRuntime.Cache.Insert(Session.SessionID + "CacheMensajes", e, null, DateTime.Now.AddMinutes(30), Cache.NoSlidingExpiration);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.DataSetResultado = null;
            //this.listaMsgCallback = null;
            if (this.ProcesosDatosModificarDatosCancelar != null)
                this.ProcesosDatosModificarDatosCancelar();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            ExportData exportar = new ExportData();
            exportar.ExportExcel(this.Page, this.DataSetResultado, true, string.Empty, string.Empty);
        }

        #region "ArmarControlesParametros"
        const string cssLabel = "col-sm-3 col-form-label text-right";
        const string cssRow = "form-group row";
        const string cssCol12 = "col-sm-12";
        const string cssCol9 = "col-sm-9";
        const string cssCol8 = "col-sm-8";
        const string cssCol6 = "col-sm-6";
        const string cssCol4 = "col-sm-4";
        const string cssCol3 = "col-sm-3";
        const string cssCol2 = "col-sm-2";
        const string cssCol1 = "col-sm-1";
        private void ArmarTablaParametros(SisProcesos pProceso)
        {
            //this.tablaParametros = new Table();
            this.pnlArchivo.Visible = pProceso.ProcesoArchivo.ProcesaArchivo;
            this.Accordion1.Visible = pProceso.ProcesoArchivo.ProcesaArchivo;
            this.tablaParametros.Controls.Clear();

            Control ctrExiste;
            int countControl = 0;
            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssRow;
            foreach (SisParametros parametro in pProceso.Parametros)
            {
                DataSet dsParametros = new DataSet();
                SisProcesos proc;
                SisParametros param;


                if (!string.IsNullOrEmpty(parametro.StoredProcedure)
                    && parametro.ParamDependiente.Trim()==string.Empty
                    && parametro.TipoParametro.IdTipoParametro != (int)EnumSisTipoParametros.DropDownListSPAutoComplete)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    proc = new SisProcesos();
                    proc.ProcesoArchivo.StoredProcedure = parametro.StoredProcedure;
                    if (parametro.ParamDependiente.Trim() == "#TodasAnteriores#")
                    {
                        foreach (SisParametros p in pProceso.Parametros.Where(x => x.Orden < parametro.Orden))
                        {
                            proc.Parametros.Add(p);
                        }
                    }
                    else
                    {
                        param = new SisParametros();
                        param = pProceso.Parametros.Find(x => x.Parametro == parametro.ParamDependiente);
                        if (param != null)
                        {
                            proc.Parametros.Add(param);
                        }
                    }

                    SisParametros parametros = new SisParametros();
                    parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.Int;
                    parametros.Parametro = "IdProceso";
                    parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.IdProceso;
                    proc.Parametros.Add(parametros);

                    parametros = new SisParametros();
                    parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.Int;
                    parametros.Parametro = "IdProcesoArchivo";
                    parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.IdProcesoArchivo;
                    proc.Parametros.Add(parametros);

                    parametros = new SisParametros();
                    parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.TextBox;
                    parametros.Parametro = "NombreArchivo";
                    parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo;
                    proc.Parametros.Add(parametros);

                    parametros = new SisParametros();
                    parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.TextBox;
                    parametros.Parametro = "Path";
                    parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.Path;
                    proc.Parametros.Add(parametros);

                    proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    dsParametros = ProcesosDatosF.ProcesosObtenerDatosParametro(proc);
                }
                else if (parametro.ParamDependiente.Trim() == "#BotonBuscar#" && parametro.ParametroDibujado)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    proc = new SisProcesos();
                    proc.ProcesoArchivo.StoredProcedure = parametro.StoredProcedure;
                    foreach (SisParametros p in pProceso.Parametros.Where(x => x.Orden < parametro.Orden))
                    {
                        proc.Parametros.Add(p);
                    }

                    SisParametros parametros = new SisParametros();
                    parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.Int;
                    parametros.Parametro = "IdProceso";
                    parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.IdProceso;
                    proc.Parametros.Add(parametros);

                    parametros = new SisParametros();
                    parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.Int;
                    parametros.Parametro = "IdProcesoArchivo";
                    parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.IdProcesoArchivo;
                    proc.Parametros.Add(parametros);

                    parametros = new SisParametros();
                    parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.TextBox;
                    parametros.Parametro = "NombreArchivo";
                    parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo;
                    proc.Parametros.Add(parametros);

                    parametros = new SisParametros();
                    parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.TextBox;
                    parametros.Parametro = "Path";
                    parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.Path;
                    proc.Parametros.Add(parametros);
                    proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    dsParametros = ProcesosDatosF.ProcesosObtenerDatosParametro(proc);
                }

                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumSisTipoParametros.Int:
                        pnlRow.Controls.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.CheckBoxList:
                        pnlRow.Controls.Add(AddCheckBoxListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                        pnlRow.Controls.Add(AddListBoxAutocompleteRow(parametro));
                        break;
                    case (int)EnumSisTipoParametros.GridViewCheckFecha:
                        pnlRow.Controls.Add(AddGridViewCheckFechaBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.DateTime:
                        pnlRow.Controls.Add(AddDateBoxRow(parametro.NombreParametro, parametro.Parametro));
                        //AddDateControl(parametro.NombreParametro, counter, parametro.Parametro);
                        break;
                    case (int)EnumSisTipoParametros.TextBox:
                        pnlRow.Controls.Add(AddTextBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.IntNumericInput:
                        pnlRow.Controls.Add(AddNumericInputBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.DateTimeRange:
                        pnlRow.Controls.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.YearMonthCombo:
                        break;
                    case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                        pnlRow.Controls.Add(AddGridViewCheckDinamicoBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.GridViewCheckImporte:
                        pnlRow.Controls.Add(AddGridViewCheckImporteBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    default:
                        break;
                }
                if (parametro.ParamDependiente.Trim() != "#BotonBuscar#")
                    parametro.ParametroDibujado = true;

                countControl++;

                if (countControl == 1)
                {
                    this.tablaParametros.Controls.Add(pnlRow);
                    countControl = 0;
                    pnlRow = new Panel();
                    pnlRow.CssClass = cssRow;
                }
            }
        }

        private void CargarValoresParametros(SisParametros parametroDatos, SisParametros parametroLlenar)
        {
            DataSet dsParametros = new DataSet();
            SisProcesos proc;
            ForEachParametros();
            if (!string.IsNullOrEmpty(parametroDatos.StoredProcedure)
                && parametroDatos.TipoParametro.IdTipoParametro != (int)EnumSisTipoParametros.DropDownListSPAutoComplete)
            {
                //Obtengo los valores para llenar las opciones del parametro
                proc = new SisProcesos();
                proc.ProcesoArchivo.StoredProcedure = parametroDatos.StoredProcedure;
                proc.Parametros.Add(parametroDatos);
                dsParametros = ProcesosDatosF.ProcesosObtenerDatosParametro(proc);
            }
          
            this.CargarValoresParametrosControl(parametroLlenar, parametroLlenar.Parametro, dsParametros);
        }

        private void CargarValoresParametrosControl(SisParametros parametroLlenar, string pNombreControl, DataSet dsParametros)
        {
            Control control;
            switch (parametroLlenar.TipoParametro.IdTipoParametro)
            {
                case (int)EnumSisTipoParametros.Int:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        DropDownList ddl = (DropDownList)control;
                        ddl.Items.Clear();
                        ddl.SelectedValue = null;
                        ddl.DataSource = dsParametros;
                        ddl.DataValueField = parametroLlenar.Parametro;
                        ddl.DataTextField = "Descripcion";
                        ddl.DataBind();
                        AyudaProgramacion.InsertarItemSeleccione(ddl, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));

                    break;
                case (int)EnumSisTipoParametros.CheckBoxList:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        CheckBoxList ddl = (CheckBoxList)control;
                        ddl.Items.Clear();
                        ddl.SelectedValue = null;
                        ddl.DataSource = dsParametros;
                        ddl.DataValueField = parametroLlenar.Parametro;
                        ddl.DataTextField = "Descripcion";
                        ddl.RepeatColumns = 1;
                        ddl.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                        ddl.RepeatLayout = RepeatLayout.Flow;
                        ddl.AutoPostBack = true;
                        ddl.SelectedIndexChanged += new EventHandler(cheFiltro_SelectedIndexChanged);
                        ddl.DataBind();
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.GridViewCheckFecha:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        GridViewCheckFecha gv = (GridViewCheckFecha)control;
                        gv.IniciarControl(dsParametros, string.Empty, string.Empty);
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        GridViewCheckDinamico gv = (GridViewCheckDinamico)control;
                        gv.IniciarControl(dsParametros, parametroLlenar.Parametro, parametroLlenar.NombreParametro);
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.GridViewCheckImporte:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        GridViewCheckImporte gv = (GridViewCheckImporte)control;
                        gv.IniciarControl(dsParametros, parametroLlenar.Parametro, parametroLlenar.NombreParametro);
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.DateTime:
                    //tablaParametros.Rows.Add(AddDateBoxRow(parametro.NombreParametro, parametro.Parametro));

                    break;
                case (int)EnumSisTipoParametros.TextBox:
                    break;
                case (int)EnumSisTipoParametros.IntNumericInput:
                    break;
                case (int)EnumSisTipoParametros.DateTimeRange:
                    //tablaParametros.Rows.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.YearMonthCombo:
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

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;

            DropDownList drpFiltro = new DropDownList();
            drpFiltro.CssClass = "form-control select2";

            drpFiltro.DataValueField = Parametro;
            drpFiltro.DataTextField = "Descripcion";
            if (dsDatos.Tables.Count > 0)
            {
                drpFiltro.DataSource = dsDatos;
                drpFiltro.DataBind();
            }
            AyudaProgramacion.InsertarItemSeleccione(drpFiltro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            
            drpFiltro.ID = Parametro;
            drpFiltro.SelectedIndexChanged += new EventHandler(drpFiltro_SelectedIndexChanged);
            drpFiltro.AutoPostBack = this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == Parametro)
                || this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");
            
            TextBox Text = new TextBox();
            Text.CssClass = "textbox";
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;

            pnlRow.Controls.Add(drpFiltro);
            pnlRow.Controls.Add(Text);
            miPanel.Controls.Add(pnlRow);

            return miPanel;
        }

        void drpFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID);
            this.RecargarControles(paramValor, ((DropDownList)sender).SelectedValue);
        }

        private void RecargarControles(SisParametros paramValor, string valor)
        {

            SisParametros paramLlenar = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.ParamDependiente == paramValor.Parametro);
            paramValor.ValorParametro =valor;
            if (paramLlenar != null)
            {
                SisParametros nuevo = new SisParametros();
                nuevo.Parametro = paramValor.Parametro;
                nuevo.TipoParametro = paramValor.TipoParametro;
                nuevo.StoredProcedure = paramLlenar.StoredProcedure;
                nuevo.ValorParametro = paramValor.ValorParametro;
                this.CargarValoresParametros(nuevo, paramLlenar);
            }

            if (this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden))
            {
                List<SisParametros> lstParamLlenar = this.MiProcesoProcesamiento.Proceso.Parametros.FindAll(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden);
                SisProcesos proc;
                foreach (SisParametros p in lstParamLlenar)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    proc = new SisProcesos();
                    proc.ProcesoArchivo.StoredProcedure = p.StoredProcedure;
                    foreach (SisParametros depParam in this.MiProcesoProcesamiento.Proceso.Parametros.Where(x => x.Orden < p.Orden))
                    {
                        //this.CargarParametroValor(depParam);
                        proc.Parametros.Add(depParam);
                    }
                    ForEachParametros();
                    proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    DataSet dsParametros = ProcesosDatosF.ProcesosObtenerDatosParametro(proc);
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
            
            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel panelCheckboxList = new Panel();
            panelCheckboxList.CssClass = cssCol6 + " overflow-auto";
            panelCheckboxList.Style.Add("max-height", "200px");
            CheckBoxList cheFiltro = new CheckBoxList();
            cheFiltro.RepeatLayout = RepeatLayout.Flow;
            cheFiltro.CssClass = "form-check";
            cheFiltro.DataValueField = Parametro;
            cheFiltro.DataTextField = "Descripcion";
            if (dsDatos.Tables.Count > 0)
            {
                cheFiltro.DataSource = dsDatos;
                cheFiltro.RepeatLayout = RepeatLayout.Flow;
                cheFiltro.AutoPostBack = true;
                cheFiltro.SelectedIndexChanged += new EventHandler(cheFiltro_SelectedIndexChanged);
                cheFiltro.DataBind();
            }
            cheFiltro.ID = Parametro;

            panelCheckboxList.Controls.Add(cheFiltro);
            miPanel.Controls.Add(panelCheckboxList);

            TextBox Text = new TextBox();
            Text.CssClass = "textbox";
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;
            panelCheckboxList.Controls.Add(Text);

            return miPanel;
        }

        void cheFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((CheckBoxList)sender).ID);

            paramValor.ValorParametro = string.Empty;
            CheckBoxList chkList = (CheckBoxList)sender;
            string separador = "";
            foreach (ListItem item in chkList.Items)
            {
                if (item.Selected)
                {
                    paramValor.ValorParametro = string.Concat(paramValor.ValorParametro, separador, item.Value);
                    paramValor.ValorParametroDescripcion = string.Concat(paramValor.ValorParametroDescripcion, separador, item.Text);
                    separador = ",";
                }
            }
            this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
        }

        void btnCheckbox_Click(object sender, EventArgs e)
        {
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => "btnChk" + x.Parametro == ((Button)sender).ID);
            this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
        }

        private PlaceHolder AddListBoxAutocompleteRow(SisParametros parametro)
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
            ddlListaOpciones.AutoPostBack = this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == parametro.Parametro)
                || this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");
            
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

        private PlaceHolder AddGridViewCheckFechaBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder panel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            panel.Controls.Add(pnlCol);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol9;
            GridViewCheckFecha gvCheckFecha = (GridViewCheckFecha)LoadControl("~/Modulos/ProcesosDatos/Controles/GridviewCheckFecha.ascx");
            gvCheckFecha.ID = Parametro;
            gvCheckFecha.IniciarControl(dsDatos, Parametro, NombreParametro);
            pnlRow.Controls.Add(gvCheckFecha);

            panel.Controls.Add(pnlRow);
            return panel;
        }

        private PlaceHolder AddGridViewCheckDinamicoBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder panel = new PlaceHolder();
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == Parametro);
            if (paramValor.ParamDependiente.Trim() == "#BotonBuscar#")
            {
                Panel pnlButtonLeft = new Panel();
                pnlButtonLeft.CssClass = cssCol4;
                panel.Controls.Add(pnlButtonLeft);

                Panel pnlButton = new Panel();
                pnlButton.CssClass = cssCol8;
                Button btnGVDinamico = new Button();
                btnGVDinamico.ID = "btnGVDinamico" + Parametro;
                btnGVDinamico.CssClass = "botonesEvol";
                btnGVDinamico.Text = NombreParametro;
                btnGVDinamico.Click += BtnGVDinamico_Click;
                pnlButton.Controls.Add(btnGVDinamico);
                panel.Controls.Add(pnlButton);
            }
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //panel.Controls.Add(pnlCol);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol12;
            GridViewCheckDinamico gvCheck = (GridViewCheckDinamico)LoadControl("~/Modulos/ProcesosDatos/Controles/GridViewCheckDinamico.ascx");
            gvCheck.ID = Parametro;
            gvCheck.IniciarControl(dsDatos, Parametro, NombreParametro);
            pnlRow.Controls.Add(gvCheck);
            panel.Controls.Add(pnlRow);
            return panel;
        }

        private PlaceHolder AddGridViewCheckImporteBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder panel = new PlaceHolder();
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == Parametro);
            if (paramValor.ParamDependiente.Trim() == "#BotonBuscar#")
            {
                Panel pnlButtonLeft = new Panel();
                pnlButtonLeft.CssClass = cssCol4;
                panel.Controls.Add(pnlButtonLeft);

                Panel pnlButton = new Panel();
                pnlButton.CssClass = cssCol8;
                Button btnGVDinamico = new Button();
                btnGVDinamico.ID = "btnGVImporte" + Parametro;
                btnGVDinamico.CssClass = "botonesEvol";
                btnGVDinamico.Text = NombreParametro;
                btnGVDinamico.Click += BtnGVImporte_Click;
                pnlButton.Controls.Add(btnGVDinamico);
                panel.Controls.Add(pnlButton);
            }
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //panel.Controls.Add(pnlCol);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol12;
            GridViewCheckImporte gvCheck = (GridViewCheckImporte)LoadControl("~/Modulos/ProcesosDatos/Controles/GridViewCheckImporte.ascx");
            gvCheck.ID = Parametro;
            gvCheck.IniciarControl(dsDatos, Parametro, NombreParametro);
            pnlRow.Controls.Add(gvCheck);
            panel.Controls.Add(pnlRow);
            return panel;
        }

        private void BtnGVDinamico_Click(object sender, EventArgs e)
        {
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => "btnGVDinamico" + x.Parametro == ((Button)sender).ID);
            paramValor.ParametroDibujado = true;
            Control gv = BuscarControlRecursivo(((Button)sender).Parent.Parent, paramValor.Parametro);
            if (gv != null)
            {
                List<SisParametros> lstParamLlenar = this.MiProcesoProcesamiento.Proceso.Parametros.FindAll(x => x.Orden < paramValor.Orden);
                SisProcesos proc;
                //Obtengo los valores para llenar las opciones del parametro
                proc = new SisProcesos();
                proc.ProcesoArchivo.StoredProcedure = paramValor.StoredProcedure;
                proc.Parametros.AddRange(lstParamLlenar);
                ForEachParametros();

                SisParametros parametros = new SisParametros();
                parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.Int;
                parametros.Parametro = "IdProceso";
                parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.IdProceso;
                proc.Parametros.Add(parametros);
                
                parametros = new SisParametros();
                parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.Int;
                parametros.Parametro = "IdProcesoArchivo";
                parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.IdProcesoArchivo;
                proc.Parametros.Add(parametros);
                
                parametros = new SisParametros();
                parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.TextBox;
                parametros.Parametro = "NombreArchivo";
                parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo;
                proc.Parametros.Add(parametros);

                parametros = new SisParametros();
                parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.TextBox;
                parametros.Parametro = "Path";
                parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.Path;
                proc.Parametros.Add(parametros);


                proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                DataSet dsParametros = ProcesosDatosF.ProcesosObtenerDatosParametro(proc);
                GridViewCheckDinamico _gv = (GridViewCheckDinamico)gv;
                _gv.IniciarControl(dsParametros, paramValor.Parametro, paramValor.NombreParametro);
            }
        }

        private void BtnGVImporte_Click(object sender, EventArgs e)
        {
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => "btnGVImporte" + x.Parametro == ((Button)sender).ID);
            paramValor.ParametroDibujado = true;
            paramValor.Filtro = "true";
            Control gv = BuscarControlRecursivo(((Button)sender).Parent.Parent, paramValor.Parametro);
            if (gv != null)
            {
                List<SisParametros> lstParamLlenar = this.MiProcesoProcesamiento.Proceso.Parametros.FindAll(x => x.Orden < paramValor.Orden);
                SisProcesos proc;
                //Obtengo los valores para llenar las opciones del parametro
                proc = new SisProcesos();
                proc.ProcesoArchivo.StoredProcedure = paramValor.StoredProcedure;
                proc.Parametros.AddRange(lstParamLlenar);
                ForEachParametros();

                SisParametros parametros = new SisParametros();
                parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.Int;
                parametros.Parametro = "IdProceso";
                parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.IdProceso;
                proc.Parametros.Add(parametros);

                parametros = new SisParametros();
                parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.Int;
                parametros.Parametro = "IdProcesoArchivo";
                parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.IdProcesoArchivo;
                proc.Parametros.Add(parametros);

                parametros = new SisParametros();
                parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.TextBox;
                parametros.Parametro = "NombreArchivo";
                parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo;
                proc.Parametros.Add(parametros);

                parametros = new SisParametros();
                parametros.TipoParametro.IdTipoParametro = (int)EnumSisTipoParametros.TextBox;
                parametros.Parametro = "Path";
                parametros.ValorParametro = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.Path;
                proc.Parametros.Add(parametros);


                proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                DataSet dsParametros = ProcesosDatosF.ProcesosObtenerDatosParametro(proc);
                GridViewCheckImporte _gv = (GridViewCheckImporte)gv;
                _gv.IniciarControl(dsParametros, paramValor.Parametro, paramValor.NombreParametro);
            }
        }
        private PlaceHolder AddMultipleListBoxRow(string NombreParametro, DataSet dsDatos, int Counter, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass =cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;

            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            ListBox lstMultipleList = new ListBox();
            lstMultipleList.CssClass = "form-control";
            lstMultipleList.ID = "MultipleListBox" + Counter.ToString();
            lstMultipleList.SelectionMode = ListSelectionMode.Multiple;
            lstMultipleList.Rows = 8;

            pnlRow.Controls.Add(lstMultipleList);
            //tc2.CssClass = "TableCell";

            TextBox Text = new TextBox();
            Text.CssClass = "textbox";
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;
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

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol6;
            TextBox Text = new TextBox();
            Text.CssClass = "form-control";
            Text.ID = Parametro;
            //TextBox Text2 = new TextBox();
            //Text2.CssClass = "textbox";
            //Text2.ID = "txt2" + Parametro;
            //Text2.Visible = false;
            //Text2.Text = Parametro;
            pnlRow.Controls.Add(Text);

            //pnlRow.Controls.Add(Text2);
            miPanel.Controls.Add(pnlRow);

            return miPanel;
        }

        private PlaceHolder AddNumericInputBoxRow(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            TextBox txtNumerico = new TextBox();
            txtNumerico.ID = Parametro;
            FilteredTextBoxExtender fte = new FilteredTextBoxExtender();
            fte.TargetControlID = Parametro;
            fte.FilterType = FilterTypes.Numbers;
            txtNumerico.CssClass = "form-control";

            if (this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == Parametro)
               || this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#"))
            {
                txtNumerico.AutoPostBack = true;
                txtNumerico.TextChanged += TxtNumerico_TextChanged;
            }

                pnlRow.Controls.Add(txtNumerico);

            miPanel.Controls.Add(pnlRow);
            return miPanel;
        }

        private void TxtNumerico_TextChanged(object sender, EventArgs e)
        {
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((TextBox)sender).ID);
            this.RecargarControles(paramValor, ((TextBox)sender).Text);
        }

        private PlaceHolder AddDateRangeBoxRow(string NombreParametro, string Parametro)
        {
            return new PlaceHolder();
            //Panel tr = new Panel();

            //Label lbl = new Label();
            //lbl.CssClass = "label";
            //lbl.ID = "lbl" + Parametro;
            //lbl.Text = NombreParametro;
            //tr.Controls.Add(lbl);

            //// RANGO DESDE
            //TextBox dateSelectorDesde = new TextBox();
            //// TODO:Cambiar año
            //dateSelectorDesde.Text = DateTime.Now.AddYears(-1).ToShortDateString();
            //dateSelectorDesde.CssClass = "textbox";
            //dateSelectorDesde.Columns = 10;
            //dateSelectorDesde.ID = "dtDesde" + Parametro;
            //tr.Controls.Add(dateSelectorDesde);

            //TextBox txtBoxDesde = new TextBox();
            //txtBoxDesde.Text = "@Rango" + Parametro.Replace("@", "") + "Inicial";
            //txtBoxDesde.CssClass = "textbox";
            //txtBoxDesde.ID = "TxtBoxDesde" + Parametro;
            //txtBoxDesde.Visible = false;
            //tr.Controls.Add(txtBoxDesde);

            //Literal ltrl = new Literal();
            //ltrl.Text = "  ";
            //tr.Controls.Add(ltrl);

            //Image imgCalendarDesde = new Image();
            //imgCalendarDesde.ImageUrl = "~/Imagenes/Calendario.png";
            //imgCalendarDesde.ID = "imgCalendarDesde" + Parametro;
            //tr.Controls.Add(imgCalendarDesde);

            //AjaxControlToolkit.CalendarExtender fechaCalendarDesde = new AjaxControlToolkit.CalendarExtender();
            //fechaCalendarDesde.Format = "dd/MM/yyyy";
            //fechaCalendarDesde.ID = "fechaCalendarDesde" + Parametro;
            //fechaCalendarDesde.TargetControlID = dateSelectorDesde.ID;
            //fechaCalendarDesde.PopupButtonID = imgCalendarDesde.ClientID;
            //tr.Controls.Add(fechaCalendarDesde);

            //Literal ltrl2 = new Literal();
            //ltrl2.Text = "  ";
            //tr.Controls.Add(ltrl2);

            //// RANGO HASTA
            //TextBox dateSelectorHasta = new TextBox();
            //dateSelectorHasta.Text = DateTime.Now.AddYears(1).ToShortDateString();
            //dateSelectorHasta.Columns = 10;
            //dateSelectorHasta.CssClass = "textbox";
            //dateSelectorHasta.ID = "dtHasta" + Parametro;
            //tr.Controls.Add(dateSelectorHasta);

            //TextBox txtBoxHasta = new TextBox();
            //txtBoxHasta.Text = "@Rango" + Parametro.Replace("@", "") + "Final";
            //txtBoxHasta.CssClass = "TextBox";
            //txtBoxHasta.ID = "TxtBoxHasta" + Parametro;
            //txtBoxHasta.Visible = false;
            //tr.Controls.Add(txtBoxHasta);

            //Literal ltrl1 = new Literal();
            //ltrl1.Text = "  ";
            //tr.Controls.Add(ltrl1);

            //Image imgCalendarHasta = new Image();
            //imgCalendarHasta.ImageUrl = "~/Imagenes/Calendario.png";
            //imgCalendarHasta.ID = "imgCalendarHasta" + Parametro;
            //tr.Controls.Add(imgCalendarHasta);

            //AjaxControlToolkit.CalendarExtender fechaCalendarHasta = new AjaxControlToolkit.CalendarExtender();
            //fechaCalendarHasta.Format = "dd/MM/yyyy";
            //fechaCalendarHasta.ID = "fechaCalendarHasta" + Parametro;
            //fechaCalendarHasta.TargetControlID = dateSelectorHasta.ID;
            //fechaCalendarHasta.PopupButtonID = imgCalendarHasta.ID;
            //tr.Controls.Add(fechaCalendarHasta);

            //AjaxControlToolkit.MaskedEditExtender fechaExtenderDesde = new AjaxControlToolkit.MaskedEditExtender();
            //fechaExtenderDesde.Mask = "99/99/9999";
            //fechaExtenderDesde.ID = "fechaExtenderDesde" + Parametro;
            //fechaExtenderDesde.MaskType = AjaxControlToolkit.MaskedEditType.Date;
            //fechaExtenderDesde.TargetControlID = dateSelectorDesde.ID;
            ////fechaExtenderDesde.CultureName = "es-AR";
            //fechaExtenderDesde.UserDateFormat = MaskedEditUserDateFormat.DayMonthYear;
            //tr.Controls.Add(fechaExtenderDesde);

            //AjaxControlToolkit.MaskedEditExtender fechaExtenderHasta = new AjaxControlToolkit.MaskedEditExtender();
            //fechaExtenderHasta.Mask = "99/99/9999";
            //fechaExtenderHasta.ID = "fechaExtenderHasta" + Parametro;
            //fechaExtenderHasta.MaskType = AjaxControlToolkit.MaskedEditType.Date;
            //fechaExtenderHasta.TargetControlID = dateSelectorHasta.ID;
            ////fechaExtenderHasta.CultureName = "es-AR";
            //fechaExtenderHasta.UserDateFormat = MaskedEditUserDateFormat.DayMonthYear;
            //tr.Controls.Add(fechaExtenderHasta);

            //AjaxControlToolkit.MaskedEditValidator fechaValidatorDesde = new AjaxControlToolkit.MaskedEditValidator();
            //fechaValidatorDesde.ID = "fechaValidatorDesde" + Parametro;
            //fechaValidatorDesde.MaximumValue = "31/12/2100";
            //fechaValidatorDesde.MinimumValue = "01/01/1950";
            //fechaValidatorDesde.Display = ValidatorDisplay.Dynamic;
            //fechaValidatorDesde.EmptyValueMessage = "&nbsp;Fechas obligatorias";
            //fechaValidatorDesde.IsValidEmpty = false;
            //fechaValidatorDesde.ControlExtender = fechaExtenderDesde.ID;
            //fechaValidatorDesde.ControlToValidate = dateSelectorDesde.ID;
            //fechaValidatorDesde.InvalidValueMessage = "&nbsp;Formato inválido";
            //fechaValidatorDesde.MinimumValueMessage = "&nbsp;El valor mínimo es 01/01/1950";
            //fechaValidatorDesde.MaximumValueMessage = "&nbsp;El valor máximo es 31/12/2100";
            //tr.Controls.Add(fechaValidatorDesde);

            //AjaxControlToolkit.MaskedEditValidator fechaValidatorHasta = new AjaxControlToolkit.MaskedEditValidator();
            //fechaValidatorHasta.ID = "fechaValidatorHasta" + Parametro;
            //fechaValidatorHasta.MaximumValue = "31/12/2100";
            //fechaValidatorHasta.MinimumValue = "01/01/1950";
            //fechaValidatorHasta.Display = ValidatorDisplay.Dynamic;
            //fechaValidatorHasta.EmptyValueMessage = "&nbsp;Fechas obligatorias";
            //fechaValidatorHasta.IsValidEmpty = false;
            //fechaValidatorHasta.ControlExtender = fechaExtenderHasta.ID;
            //fechaValidatorHasta.ControlToValidate = dateSelectorHasta.ID;
            //fechaValidatorHasta.InvalidValueMessage = "&nbsp;Formato inválido";
            //fechaValidatorHasta.MinimumValueMessage = "&nbsp;El valor mínimo es 01/01/1950";
            //fechaValidatorHasta.MaximumValueMessage = "&nbsp;El valor máximo es 31/12/2100";
            //tr.Controls.Add(fechaValidatorHasta);

            //return tr;
        }

        void BtnPostBack_Click(object sender, EventArgs e)
        {
            string id = ((Button)sender).ID.Replace("btnPostBack", string.Empty);
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == id);
            Control txt = BuscarControlRecursivo(((Button)sender).Parent, id);
            if (txt != null && txt is TextBox)
                paramValor.ValorParametro = ((TextBox)txt).Text;
            List<SisParametros> parametrosLlenar = this.MiProcesoProcesamiento.Proceso.Parametros.FindAll(x => x.ParamDependiente == paramValor.Parametro);
            if (parametrosLlenar != null)
            {
                //SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID);
                this.RecargarControles(paramValor, ((TextBox)txt).Text);
            }

           
        }

        private PlaceHolder AddDateBoxRow(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            Panel pnlCol = new Panel();
            pnlCol.CssClass = cssCol1;
            miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            TextBox dateSelector = new TextBox();
            //if (Parametro == "FechaDesde")
            //    dateSelector.Text = DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToShortDateString();
            //else
            //    dateSelector.Text = DateTime.Now.ToShortDateString();
            dateSelector.CssClass = "form-control datepicker";
            dateSelector.Columns = 10;
            dateSelector.ID = Parametro;
            //dateSelector.AutoPostBack = this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == Parametro)
            //    || this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");
            //if (dateSelector.AutoPostBack)
            //{
            //    dateSelector.TextChanged += DateSelector_TextChanged;
            //}
            pnlRow.Controls.Add(dateSelector);
            miPanel.Controls.Add(pnlRow);
            Panel pnlCol4 = new Panel();
            pnlCol4.CssClass = cssCol3;
            pnlRow.Controls.Add(pnlCol4);
           
            if (this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == Parametro)
               || this.MiProcesoProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#"))
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
                dateSelector.Attributes.Add("onchange", "return btnPostBackClick" + dateSelector.ID + "();");

                StringBuilder scriptBtn = new StringBuilder();
                scriptBtn.AppendLine("function btnPostBackClick|CTRLID|(ctrl) {");
                scriptBtn.AppendFormat("var hdfDate = $('input[id$={0}]');", hdfDate.ID);
                scriptBtn.AppendFormat("var txtFecha = $('input[id$={0}]');", dateSelector.ID);
                scriptBtn.AppendLine(" if (txtFecha.val()!=hdfDate.val()) {");
                scriptBtn.AppendLine(" hdfDate.val(txtFecha.val());");
                scriptBtn.AppendFormat("$(\"[id$='{0}']\").click();", btn.ID);
                //scriptBtn.AppendLine("__doPostBack(btn.name, '');");
                scriptBtn.AppendLine("}");
                scriptBtn.AppendLine("}");
                scriptBtn.Replace("|CTRLID|", dateSelector.ID);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel1, this.UpdatePanel1.GetType(), "ScriptAgregar" + btn.ID, scriptBtn.ToString(), true);
            }


            return miPanel;
        }

        //private void DateSelector_TextChanged(object sender, EventArgs e)
        //{
        //    SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((TextBox)sender).ID);

        //    paramValor.ValorParametro = string.Empty;
        //    TextBox chkList = (TextBox)sender;
        //    paramValor.ValorParametro = chkList.Text;
        
        //    this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
        //}

        private PlaceHolder AddYearMonthCombo(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            return miPanel;
            //Panel tr = new Panel();

            //Label lbl = new Label();
            //lbl.CssClass = "label";
            //lbl.ID = "lbl" + Parametro;
            //lbl.Text = NombreParametro;
            //tr.Controls.Add(lbl);

            //YearMonthCombo YMCYear = new YearMonthCombo();
            //YMCYear.ID = "Year" + Parametro;

            //for (int i = 1900; i < 2100; i++)
            //{
            //    YMCYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //}

            //YearMonthCombo YMCMonth = new YearMonthCombo();
            //YMCMonth.ID = "Month" + Parametro;

            //YMCMonth.Items.Add(new ListItem("Enero", "01"));
            //YMCMonth.Items.Add(new ListItem("Febrero", "02"));
            //YMCMonth.Items.Add(new ListItem("Marzo", "03"));
            //YMCMonth.Items.Add(new ListItem("Abril", "04"));
            //YMCMonth.Items.Add(new ListItem("Mayo", "05"));
            //YMCMonth.Items.Add(new ListItem("Junio", "06"));
            //YMCMonth.Items.Add(new ListItem("Julio", "07"));
            //YMCMonth.Items.Add(new ListItem("Agosto", "08"));
            //YMCMonth.Items.Add(new ListItem("Septiembre", "09"));
            //YMCMonth.Items.Add(new ListItem("Octubre", "10"));
            //YMCMonth.Items.Add(new ListItem("Noviembre", "11"));
            //YMCMonth.Items.Add(new ListItem("Diciembre", "12"));

            //YMCYear.Text = DateTime.Now.Year.ToString();
            //YMCMonth.Text = (DateTime.Now.Month < 10 ? ("0" + DateTime.Now.Month.ToString()) : DateTime.Now.Month.ToString());

            //TextBox Text = new TextBox();
            //Text.CssClass = "textbox";
            //Text.ID = "TxtBox" + Parametro;
            //Text.Visible = false;
            //Text.Text = Parametro;

            //tr.Controls.Add(YMCYear);
            //tr.Controls.Add(YMCMonth);
            //tr.Controls.Add(Text);

            //return tr;
        }

        #endregion


        protected void FileUploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            //this.MiProceso.ArchivoImagen = this.AsyncFileUpload1.FileBytes;
            //this.MiProceso.UsuarioActivo = this.UsuarioActivo;
            string filename = System.IO.Path.GetFileName(AsyncFileUpload1.FileName);
            //Variable para la utilizacion del Async
            this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo = AsyncFileUpload1.FileName;

            string HoyTexto = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");

            TGEParametrosValores RutaDelArchivo = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ProcesosDatosDirectorioArchivo);
            //this.MiProceso = TGEGeneralesF.TGEParametrosObtenerUno(EnumTGEParametros.Ruta).ValorParametros;

            string RutaDelArchivoTexto = string.Concat(RutaDelArchivo.ParametroValor.ToString(), this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo);

            if (File.Exists(RutaDelArchivoTexto))// + strFileName))
            {
                File.Copy(RutaDelArchivoTexto, RutaDelArchivoTexto + "-" + HoyTexto);
                File.Delete(RutaDelArchivoTexto);
            }
            //Utilizar el AsyncFileUpload
            AsyncFileUpload1.SaveAs(RutaDelArchivoTexto);
            this.MiProcesoProcesamiento.Proceso.TieneArchivo = true;
            //SistemasF.SistemaActualizar(MiProceso);           
            //}
            
        }

    }
    // Clase para saber cuando se utiliza este tipo de funcionalidad
    public class AsyncFileUpload : AjaxControlToolkit.AsyncFileUpload
    {

    }
    // Clase para saber cuando se utiliza este tipo de funcionalidad
    public class YearMonthCombo : System.Web.UI.WebControls.DropDownList
    {

    }
    public class YearCombo : System.Web.UI.WebControls.DropDownList
    {
    }
    public class MonthCombo : System.Web.UI.WebControls.DropDownList
    {
    }
}