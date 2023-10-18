using AjaxControlToolkit;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using IU.Modulos.ProcesosDatos.Controles;
using Mailing;
using Mailing.Entidades;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using Reportes.Entidades;
using Servicio.AccesoDatos;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Mailing.Controles
{
    public partial class MailingDatos : ControlesSeguros
    {
        private TGEMailing MiMailing
        {
            get { return (TGEMailing)Session[this.MiSessionPagina + "TGEMailingDatosMiMailing"]; }
            set { Session[this.MiSessionPagina + "TGEMailingDatosMiMailing"] = value; }
        }
        private DataTable DetalleEnvioProcesamiento
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarArchivoDatos"]; }
            set { Session[this.MiSessionPagina + "ImportarArchivoDatos"] = value; }
        }

        //private List<TGEMailingAdjuntos> MiMailingAdjunto
        //{
        //    get { return (List<TGEMailingAdjuntos>)Session[this.MiSessionPagina + "TGEMailingDatosMiMailingAdjunto"]; }
        //    set { Session[this.MiSessionPagina + "TGEMailingDatosMiMailingAdjunto"] = value; }
        //}

        private List<TGEMailingProcesos> MiMailingProceso
        {
            get { return (List<TGEMailingProcesos>)Session[this.MiSessionPagina + "TGEMailingDatosMiMailingProceso"]; }
            set { Session[this.MiSessionPagina + "TGEMailingDatosMiMailingProceso"] = value; }
        }

        public delegate void MailingDatosCancelarEventHandler();
        public event MailingDatosCancelarEventHandler MailingModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
            }

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.IsPostBack)
            {
                if (this.MiMailing.MailingProcesamiento != null && this.MiMailing.MailingProcesamiento.Proceso.IdMailingProceso > 0)
                {
                    this.ObtenerValoresRequestForm(this.MiMailing.MailingProcesamiento.Proceso);
                    this.ArmarTablaParametros(this.MiMailing.MailingProcesamiento.Proceso);
                }
            }
        }

        public void IniciarControl(TGEMailing pMailing, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiMailing = pMailing;
            this.CargarCombos();
            pMailing.Estado.IdEstado = (int)Estados.Activo;

            TGEMailing parametros = this.BusquedaParametrosObtenerValor<TGEMailing>();

            if (MiMailing.IdMailing != parametros.IdMailing)
            {
                parametros = new TGEMailing();
                this.BusquedaParametrosGuardarValor<TGEMailing>(parametros);
            }
            else if (parametros.BusquedaParametros)
            {
                tcDatos.ActiveTabIndex = parametros.HashTransaccion;
                if (tcDatos.ActiveTab.ID == "tpPlantillas")
                {
                    gvDatos.PageIndex = parametros.IndiceColeccion;
                }
                BusquedaParametrosGuardarValor<TGEMailing>(parametros);
            }


            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.txtFechaInicio.Text = DateTime.Now.ToShortDateString();
                    this.ddlProceso.Enabled = true;
                    this.txtDescripcion.Enabled = true;
                    this.txtFechaInicio.Enabled = true;
                    this.txtFechaFin.Enabled = true;
                    this.ddlPeriocidad.Enabled = true;
                    this.txtDiaEjecucion.Enabled = true;
                    this.ddlPlantilla.Enabled = true;
                    this.btnEditarPlantilla.Visible = false;
                    this.ddlProceso.Visible = true;
                    this.btnAdjuntosAgregar.Visible = true;
                    break;
                case Gestion.Modificar:
                    this.MiMailing = MailingF.TGEMailingObtenerDatosCompletos(pMailing);
                    MiMailing.MailingProcesamiento.Proceso = MailingF.MailingProcesosObtenerDatosCompletos(MiMailing);
                    this.MapearObjetoAControles(this.MiMailing);
                    if (this.MisParametrosUrl.Contains("UrlReferrer"))
                        this.paginaSegura.viewStatePaginaSegura = this.MisParametrosUrl["UrlReferrer"].ToString();
                    this.ddlProceso.Enabled = true;
                    this.txtDescripcion.Enabled = true;
                    this.txtFechaInicio.Enabled = true;
                    this.txtFechaFin.Enabled = true;
                    this.ddlPeriocidad.Enabled = true;
                    this.txtDiaEjecucion.Enabled = true;
                    this.ddlPlantilla.Enabled = true;
                    ddlProceso_OnSelectedIndexChanged(null, EventArgs.Empty);
                    this.ArmarTablaParametros(this.MiMailing.MailingProcesamiento.Proceso);

                    //this.btnPruebaEnvio.Visible = true;
                    this.btnEjecutarAhora.Visible = true;
                    this.ddlAdjuntos.Visible = true;
                    this.btnAdjuntosAgregar.Visible = true;
                    break;
                case Gestion.Consultar:
                    this.MiMailing = MailingF.TGEMailingObtenerDatosCompletos(pMailing);
                    MiMailing.MailingProcesamiento.Proceso = MailingF.MailingProcesosObtenerDatosCompletos(MiMailing);
                    this.MapearObjetoAControles(this.MiMailing);
                    this.btnAceptar.Visible = false;
                    this.btnEditarPlantilla.Visible = false;
                    //this.btnPruebaEnvio.Visible = true;
                    ddlProceso_OnSelectedIndexChanged(null, EventArgs.Empty);
                    this.ArmarTablaParametros(this.MiMailing.MailingProcesamiento.Proceso);
                    this.btnEjecutarAhora.Visible = true;
                    this.ddlAdjuntos.Visible = true;
                    this.ddlAdjuntos.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(TGEMailing pMailing)
        {
            this.txtDescripcion.Text = pMailing.Descripcion;
            this.txtFechaInicio.Text = pMailing.FechaInicio.HasValue ? pMailing.FechaInicio.Value.ToShortDateString() : string.Empty;
            this.txtFechaFin.Text = pMailing.FechaFin.HasValue ? pMailing.FechaFin.Value.ToShortDateString() : string.Empty;
            this.txtDiaEjecucion.Text = pMailing.DiaEjecucion.ToString();
            txtAsunto.Text = pMailing.Asunto;

            ListItem listItem = this.ddlProceso.Items.FindByValue(pMailing.MailingProcesos.IdMailingProceso.ToString());
            if (listItem == null && pMailing.MailingProcesos.IdMailingProceso > 0)
                this.ddlProceso.Items.Add(new ListItem(pMailing.MailingProcesos.Descripcion, pMailing.MailingProcesos.IdMailingProceso.ToString()));
            this.ddlProceso.SelectedValue = pMailing.MailingProcesos.IdMailingProceso == 0 ? string.Empty : pMailing.MailingProcesos.IdMailingProceso.ToString();

            listItem = this.ddlPeriocidad.Items.FindByValue(pMailing.ListasValoresDetalles.IdRefListaValorDetalle.ToString());
            if (listItem == null)
                this.ddlPeriocidad.Items.Add(new ListItem(pMailing.ListasValoresDetalles.DescripcionRef, pMailing.ListasValoresDetalles.IdRefListaValorDetalle.ToString()));
            this.ddlPeriocidad.SelectedValue = pMailing.ListasValoresDetalles.IdRefListaValorDetalle.ToString();

            listItem = this.ddlPlantilla.Items.FindByValue(pMailing.Plantillas.IdPlantilla.ToString());
            if (listItem == null)
                this.ddlPlantilla.Items.Add(new ListItem(pMailing.Plantillas.NombrePlantilla, pMailing.Plantillas.IdPlantilla.ToString()));
            this.ddlPlantilla.SelectedValue = pMailing.Plantillas.IdPlantilla.ToString();


            TGEMailing parametros = this.BusquedaParametrosObtenerValor<TGEMailing>();
            this.gvDatos.PageIndex = parametros.IndiceColeccion;
            gvDatos.DataSource = pMailing.MailingAdjuntos;
            gvDatos.DataBind();
            gvDatos.Visible = true;
        
        }

        private void MapearControlesAObjeto(TGEMailing pMailing)
        {
            pMailing.Descripcion = this.txtDescripcion.Text;
            pMailing.FechaInicio = this.txtFechaInicio.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaInicio.Text);
            pMailing.FechaFin = this.txtFechaFin.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaFin.Text);
            pMailing.DiaEjecucion = Convert.ToInt32(this.txtDiaEjecucion.Text);
            pMailing.Asunto = txtAsunto.Text;
            pMailing.MailingProcesos = MiMailingProceso.FirstOrDefault(x => x.IdMailingProceso == Convert.ToInt32(this.ddlProceso.SelectedValue));// == string.Empty ? 0 : Convert.ToInt32(this.ddlProceso.SelectedValue);
            pMailing.ListasValoresDetalles.IdRefListaValorDetalle = this.ddlPeriocidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriocidad.SelectedValue);
            pMailing.Plantillas.IdPlantilla = this.ddlPlantilla.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPlantilla.SelectedValue);

            pMailing.IdPeriocididad = this.ddlPeriocidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPeriocidad.SelectedValue);

            pMailing.MailingAdjuntos = MiMailing.MailingAdjuntos;
        }


        private void CargarCombos()
        {
            MiMailingProceso = MailingF.TGEMailingObtenerListaMailingProceso();
            this.ddlProceso.DataSource = MiMailingProceso;
            this.ddlProceso.DataValueField = "IdMailingProceso";
            this.ddlProceso.DataTextField = "Descripcion";
            this.ddlProceso.DataBind();
            if (this.ddlProceso.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlProceso, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //this.MiMailing.ListasValores.IdRefListaValor = 64;
            this.ddlPeriocidad.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.MailingPeriodicidades);
            this.ddlPeriocidad.DataValueField = "IdListaValorDetalle";
            this.ddlPeriocidad.DataTextField = "Descripcion";
            this.ddlPeriocidad.DataBind();
            if (this.ddlPeriocidad.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPeriocidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlPlantilla.DataSource = TGEGeneralesF.PlantillasObtenerLista();
            this.ddlPlantilla.DataValueField = "IdPlantilla";
            this.ddlPlantilla.DataTextField = "NombrePlantilla";
            this.ddlPlantilla.DataBind();
            if (this.ddlPlantilla.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlPlantilla, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlAdjuntos.DataSource = TGEGeneralesF.PlantillasObtenerLista();
            this.ddlAdjuntos.DataValueField = "IdPlantilla";
            this.ddlAdjuntos.DataTextField = "NombrePlantilla";
            this.ddlAdjuntos.DataBind();
            if (this.ddlAdjuntos.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlAdjuntos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlProceso_OnSelectedIndexChanged(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(ddlProceso.SelectedValue))
            {

                MiMailing.IdMailing = Convert.ToInt32(ddlProceso.SelectedValue);
                MiMailing = MailingF.TGEMailingObtenerDatosCompletos(MiMailing);
                ddlPlantilla.SelectedValue = MiMailing.Plantillas.IdPlantilla.ToString();

                TGEMailing parametro = new TGEMailing();
                parametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                parametro.IdMailing = Convert.ToInt32(ddlProceso.SelectedValue);
                parametro.MailingProcesos.IdMailingProceso = Convert.ToInt32(ddlProceso.SelectedValue);

                MiMailing.MailingProcesamiento.Proceso = MailingF.MailingProcesosObtenerDatosCompletos(parametro);
                MiMailing.MailingProcesamiento.Proceso.IdMailingProceso = Convert.ToInt32(ddlProceso.SelectedValue);

                if (MiMailing.MailingProcesamiento.Proceso.Parametros.Count > 0)
                {
                    this.ArmarTablaParametros(this.MiMailing.MailingProcesamiento.Proceso);
                }

            }
            else
            {
             
            }

     


        }


        protected void ObtenerValoresRequestForm(TGEMailingProcesos proceso)
        {
            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("$ModifDatos$")).ToList();
            foreach (TGEMailingParametros parametro in proceso.Parametros)
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

        protected void ForEachParametros()
        {
            Control ctrl;
            DateTime fechaHasta;


            foreach (TGEMailingParametros parametro in this.MiMailing.MailingProcesamiento.Proceso.Parametros)
            {
                foreach (Control tr in tablaParametros.Controls)
                {
                    ctrl = tr.FindControl(parametro.Parametro);
                    if (ctrl != null)
                    {
                        if (ctrl is TextBox)
                        {
                            if (parametro.TipoParametro.IdTipoParametro == (int)EnumRepTipoParametros.DateTime )
                            {
                              
                                  fechaHasta = ((TextBox)ctrl).Text == string.Empty ? DateTime.MaxValue : Convert.ToDateTime(((TextBox)ctrl).Text);
                                parametro.ValorParametro = fechaHasta.ToShortDateString();
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
                        //else if (ctrl is AsyncFileUpload)
                        //{
                        //    parametro.ValorParametro = ((AsyncFileUpload)ctrl).FileBytes;
                        //    parametro.ValorParametroDescripcion = ((AsyncFileUpload)ctrl).FileName;
                        //    break;
                        //}
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
                    }
                }
            }
        }
        #region "ArmarControlesParametros"
        const string cssLabel = "col-sm-1 col-form-label";
        const string cssRow = "form-group row";
        const string cssCol12 = "col-sm-12";
        const string cssCol9 = "col-sm-9";
        const string cssCol8 = "col-sm-8";
        const string cssCol6 = "col-sm-6";
        const string cssCol4 = "col-sm-4";
        const string cssCol3 = "col-sm-3";
        const string cssCol2 = "col-sm-2";
        const string cssCol1 = "col-sm-1";
        private void ArmarTablaParametros(TGEMailingProcesos pProceso)
        {
            //this.tablaParametros = new Table();
            //this.pnlArchivo.Visible = pProceso.ProcesoArchivo.ProcesaArchivo;
            //this.Accordion1.Visible = pProceso.ProcesoArchivo.ProcesaArchivo;
            this.tablaParametros.Controls.Clear();

            int countControl = 0;
            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssRow;
            foreach (TGEMailingParametros parametro in pProceso.Parametros)
            {
                DataSet dsParametros = new DataSet();
                SisProcesos proc;
                TGEMailingParametros param;

                if (!string.IsNullOrEmpty(parametro.StoredProcedure)
                    && parametro.ParamDependiente.Trim() == string.Empty
                    && parametro.TipoParametro.IdTipoParametro != (int)EnumSisTipoParametros.DropDownListSPAutoComplete)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    proc = new SisProcesos();
                    proc.ProcesoArchivo.StoredProcedure = parametro.StoredProcedure;
                    if (parametro.ParamDependiente.Trim() == "#TodasAnteriores#")
                    {
                        foreach (TGEMailingParametros p in pProceso.Parametros.Where(x => x.Orden < parametro.Orden))
                        {
                            proc.Parametros.Add(p);
                        }
                    }
                    else
                    {
                        param = new TGEMailingParametros();
                        param = pProceso.Parametros.Find(x => x.Parametro == parametro.ParamDependiente);
                        if (param != null)
                        {
                            proc.Parametros.Add(param);
                        }
                    }
                    proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    dsParametros = ProcesosDatosF.ProcesosObtenerDatosParametro(proc);
                }

                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumSisTipoParametros.Int:
                        pnlRow.Controls.Add(AddListBoxRow(parametro, dsParametros));
                        break;
                    case (int)EnumSisTipoParametros.CheckBoxList:
                        pnlRow.Controls.Add(AddCheckBoxListBoxRow(parametro, dsParametros));
                        break;
                    case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                        pnlRow.Controls.Add(AddListBoxAutocompleteRow(parametro));
                        break;
                    case (int)EnumSisTipoParametros.GridViewCheckFecha:
                        pnlRow.Controls.Add(AddGridViewCheckFechaBoxRow(parametro, dsParametros));
                        break;
                    case (int)EnumSisTipoParametros.DateTime:
                        pnlRow.Controls.Add(AddDateBoxRow(parametro));
                        //AddDateControl(parametro.NombreParametro, counter, parametro.Parametro);
                        break;
                    case (int)EnumSisTipoParametros.TextBox:
                        pnlRow.Controls.Add(AddTextBoxRow(parametro));
                        break;
                    case (int)EnumSisTipoParametros.IntNumericInput:
                        pnlRow.Controls.Add(AddNumericInputBoxRow(parametro));
                        break;
                    case (int)EnumSisTipoParametros.DateTimeRange:
                        pnlRow.Controls.Add(AddDateRangeBoxRow(parametro));
                        break;
                    case (int)EnumSisTipoParametros.YearMonthCombo:
                        break;
                    case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                        pnlRow.Controls.Add(AddGridViewCheckDinamicoBoxRow(parametro, dsParametros));
                        break;
                    default:
                        break;
                }
                parametro.ParametroDibujado = true;
                countControl++;

                if (countControl == 1)
                {
                    this.tablaParametros.Controls.Add(pnlRow);
                    countControl = 0;
                    //pnlRow = new Panel();
                    //pnlRow.CssClass = cssRow;
                }
            }
        }

        private void CargarValoresParametros(TGEMailingParametros parametroDatos, TGEMailingParametros parametroLlenar)
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

        private void CargarValoresParametrosControl(TGEMailingParametros parametroLlenar, string pNombreControl, DataSet dsParametros)
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

        private PlaceHolder AddListBoxRow(TGEMailingParametros pParametro, DataSet dsDatos)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + pParametro.Parametro;
            lbl.Text = pParametro.NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;

            DropDownList drpFiltro = new DropDownList();
            drpFiltro.CssClass = "form-control select2";

            drpFiltro.DataValueField = pParametro.Parametro;
            drpFiltro.DataTextField = "Descripcion";
            if (dsDatos.Tables.Count > 0)
            {
                drpFiltro.DataSource = dsDatos;
                drpFiltro.DataBind();
            }
            AyudaProgramacion.InsertarItemSeleccione(drpFiltro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            ListItem item = drpFiltro.Items.FindByValue(pParametro.ValorParametro.ToString());
            if (item != null)
                drpFiltro.SelectedValue = pParametro.ValorParametro.ToString();
            else
                drpFiltro.SelectedValue = string.Empty;
            drpFiltro.ID = pParametro.Parametro;
            drpFiltro.SelectedIndexChanged += new EventHandler(drpFiltro_SelectedIndexChanged);
            drpFiltro.AutoPostBack = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == pParametro.Parametro)
                || this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");

            TextBox Text = new TextBox();
            Text.CssClass = "textbox";
            Text.ID = "txt" + pParametro.Parametro;
            Text.Visible = false;
            Text.Text = pParametro.Parametro;

            pnlRow.Controls.Add(drpFiltro);
            pnlRow.Controls.Add(Text);
            miPanel.Controls.Add(pnlRow);

            return miPanel;
        }

        void drpFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID);
            this.RecargarControles(paramValor, ((DropDownList)sender).SelectedValue);
        }

        private void RecargarControles(TGEMailingParametros paramValor, string valor)
        {

            TGEMailingParametros paramLlenar = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.ParamDependiente == paramValor.Parametro);
            paramValor.ValorParametro = valor;
            if (paramLlenar != null)
            {
                TGEMailingParametros nuevo = new TGEMailingParametros();
                nuevo.Parametro = paramValor.Parametro;
                nuevo.TipoParametro = paramValor.TipoParametro;
                nuevo.StoredProcedure = paramLlenar.StoredProcedure;
                nuevo.ValorParametro = paramValor.ValorParametro;
                this.CargarValoresParametros(nuevo, paramLlenar);
            }

            if (this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden))
            {
                List<TGEMailingParametros> lstParamLlenar = this.MiMailing.MailingProcesamiento.Proceso.Parametros.FindAll(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden);
                SisProcesos proc;
                foreach (TGEMailingParametros p in lstParamLlenar)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    proc = new SisProcesos();
                    proc.ProcesoArchivo.StoredProcedure = p.StoredProcedure;
                    foreach (TGEMailingParametros depParam in this.MiMailing.MailingProcesamiento.Proceso.Parametros.Where(x => x.Orden < p.Orden))
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

        private PlaceHolder AddCheckBoxListBoxRow(TGEMailingParametros pParametro, DataSet dsDatos)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + pParametro.Parametro;
            lbl.Text = pParametro.NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel panelCheckboxList = new Panel();
            panelCheckboxList.CssClass = cssCol3 + " overflow-auto";
            panelCheckboxList.Style.Add("max-height", "200px");
            CheckBoxList cheFiltro = new CheckBoxList();
            cheFiltro.RepeatLayout = RepeatLayout.Flow;
            cheFiltro.CssClass = "form-check";
            cheFiltro.DataValueField = pParametro.Parametro;
            cheFiltro.DataTextField = "Descripcion";
            if (dsDatos.Tables.Count > 0)
            {
                cheFiltro.DataSource = dsDatos;
                cheFiltro.RepeatLayout = RepeatLayout.Flow;
                cheFiltro.AutoPostBack = true;
                cheFiltro.SelectedIndexChanged += new EventHandler(cheFiltro_SelectedIndexChanged);
                cheFiltro.DataBind();
            }
            cheFiltro.ID = pParametro.Parametro;

            panelCheckboxList.Controls.Add(cheFiltro);
            miPanel.Controls.Add(panelCheckboxList);

            TextBox Text = new TextBox();
            Text.CssClass = "textbox";
            Text.ID = "txt" + pParametro.Parametro;
            Text.Visible = false;
            Text.Text = pParametro.Parametro;
            panelCheckboxList.Controls.Add(Text);

            return miPanel;
        }

        void cheFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((CheckBoxList)sender).ID);

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
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => "btnChk" + x.Parametro == ((Button)sender).ID);
            this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
        }

        private PlaceHolder AddListBoxAutocompleteRow(TGEMailingParametros pParametro)
        {
            PlaceHolder panel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //panel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + pParametro.Parametro;
            lbl.Text = pParametro.NombreParametro;
            panel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.ID = pParametro.Parametro;
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.DataValueField = pParametro.Parametro;
            ddlListaOpciones.DataTextField = "Descripcion";

            ddlListaOpciones.SelectedIndexChanged += new EventHandler(drpFiltro_SelectedIndexChanged);
            ddlListaOpciones.AutoPostBack = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == pParametro.Parametro)
                || this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");

            //Busco el detalle en la BD
            if (pParametro.ValorParametro.ToString().Trim().Length > 0)
            {
                TGECamposValores campoValor = new TGECamposValores();
                campoValor.Valor = pParametro.ValorParametro.ToString().Trim();
                DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet(pParametro.StoredProcedure, pParametro);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pParametro.ValorParametroDescripcion = ds.Tables[0].Rows[0]["Descripcion"].ToString();

                ListItem item = new ListItem(pParametro.ValorParametroDescripcion, pParametro.ValorParametro.ToString());
                ddlListaOpciones.Items.Add(item);
            }

            HiddenField hdfValue = new HiddenField();
            hdfValue.ID = "select2HdfValue" + pParametro.Parametro;
            hdfValue.Value = pParametro.ValorParametro.ToString();
            HiddenField hdfText = new HiddenField();
            hdfText.ID = "select2HdfText" + pParametro.Parametro;
            hdfText.Value = pParametro.ValorParametroDescripcion;

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
            script.AppendFormat("placeholder: '{0}',", pParametro.NombreParametro);
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
            script.AppendFormat("sp: '{0}',", pParametro.StoredProcedure);
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

        private PlaceHolder AddGridViewCheckFechaBoxRow(TGEMailingParametros pParametro, DataSet dsDatos)
        {
            PlaceHolder panel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //panel.Controls.Add(pnlCol);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol9;
            GridViewCheckFecha gvCheckFecha = (GridViewCheckFecha)LoadControl("~/Modulos/ProcesosDatos/Controles/GridviewCheckFecha.ascx");
            gvCheckFecha.ID = pParametro.Parametro;
            gvCheckFecha.IniciarControl(dsDatos, pParametro.Parametro, pParametro.NombreParametro);
            pnlRow.Controls.Add(gvCheckFecha);

            panel.Controls.Add(pnlRow);
            return panel;
        }

        private PlaceHolder AddGridViewCheckDinamicoBoxRow(TGEMailingParametros pParametro, DataSet dsDatos)
        {
            PlaceHolder panel = new PlaceHolder();
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == pParametro.Parametro);
            if (paramValor.ParamDependiente.Trim() == "#BotonBuscar#")
            {
                Panel pnlButtonLeft = new Panel();
                pnlButtonLeft.CssClass = cssCol4;
                panel.Controls.Add(pnlButtonLeft);

                Panel pnlButton = new Panel();
                pnlButton.CssClass = cssCol8;
                Button btnGVDinamico = new Button();
                btnGVDinamico.ID = "btnGVDinamico" + pParametro.Parametro;
                btnGVDinamico.CssClass = "botonesEvol";
                btnGVDinamico.Text = pParametro.NombreParametro;
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
            gvCheck.ID = pParametro.Parametro;
            gvCheck.IniciarControl(dsDatos, pParametro.Parametro, pParametro.NombreParametro);
            pnlRow.Controls.Add(gvCheck);
            panel.Controls.Add(pnlRow);
            return panel;
        }

        private void BtnGVDinamico_Click(object sender, EventArgs e)
        {
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => "btnGVDinamico" + x.Parametro == ((Button)sender).ID);
            Control gv = BuscarControlRecursivo(((Button)sender).Parent.Parent, paramValor.Parametro);
            if (gv != null)
            {
                List<TGEMailingParametros> lstParamLlenar = this.MiMailing.MailingProcesamiento.Proceso.Parametros.FindAll(x => x.Orden < paramValor.Orden);
                TGEMailingProcesos proc;
                //Obtengo los valores para llenar las opciones del parametro
                proc = new TGEMailingProcesos();

                proc.Parametros.AddRange(lstParamLlenar);
                ForEachParametros();
                proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                DataSet dsParametros = MailingF.ProcesosObtenerDatosParametro(proc);
                GridViewCheckDinamico _gv = (GridViewCheckDinamico)gv;
                _gv.IniciarControl(dsParametros, paramValor.Parametro, paramValor.NombreParametro);
            }
        }

        private PlaceHolder AddMultipleListBoxRow(string NombreParametro, DataSet dsDatos, int Counter, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;

            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
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

        private PlaceHolder AddTextBoxRow(TGEMailingParametros pParametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + pParametro.Parametro;
            lbl.Text = pParametro.NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            TextBox Text = new TextBox();
            Text.CssClass = "form-control";
            Text.ID = "txt1" + pParametro.ValorParametro.ToString();
         
            pnlRow.Controls.Add(Text);


            miPanel.Controls.Add(pnlRow);

            return miPanel;
        }

        private PlaceHolder AddNumericInputBoxRow(TGEMailingParametros pParametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + pParametro.Parametro;
            lbl.Text = pParametro.NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            TextBox txtNumerico = new TextBox();
            txtNumerico.ID = pParametro.Parametro;
            FilteredTextBoxExtender fte = new FilteredTextBoxExtender();
            fte.TargetControlID = pParametro.Parametro;
            fte.FilterType = FilterTypes.Numbers;
            txtNumerico.CssClass = "form-control";
            txtNumerico.Text = pParametro.ValorParametro.ToString();
            if (this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == pParametro.Parametro)
               || this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#"))
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
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((TextBox)sender).ID);
            this.RecargarControles(paramValor, ((TextBox)sender).Text);
        }

        private PlaceHolder AddDateRangeBoxRow(TGEMailingParametros parametro)
        {
            return new PlaceHolder();
           
        }

        void BtnPostBack_Click(object sender, EventArgs e)
        {
            string id = ((Button)sender).ID.Replace("btnPostBack", string.Empty);
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == id);
            Control txt = BuscarControlRecursivo(((Button)sender).Parent, id);
            if (txt != null && txt is TextBox)
                paramValor.ValorParametro = ((TextBox)txt).Text;
            List<TGEMailingParametros> parametrosLlenar = this.MiMailing.MailingProcesamiento.Proceso.Parametros.FindAll(x => x.ParamDependiente == paramValor.Parametro);
            if (parametrosLlenar != null)
            {
                //TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID);
                this.RecargarControles(paramValor, ((TextBox)txt).Text);
            }


        }

        private PlaceHolder AddDateBoxRow(TGEMailingParametros pParametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + pParametro.Parametro;
            lbl.Text = pParametro.NombreParametro;
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
            dateSelector.ID = pParametro.Parametro;
            dateSelector.Text = pParametro.ValorParametro.ToString();
            //dateSelector.AutoPostBack = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == Parametro)
            //    || this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");
            //if (dateSelector.AutoPostBack)
            //{
            //    dateSelector.TextChanged += DateSelector_TextChanged;
            //}
            pnlRow.Controls.Add(dateSelector);
            miPanel.Controls.Add(pnlRow);
            Panel pnlCol4 = new Panel();
            pnlCol4.CssClass = cssCol3;
            pnlRow.Controls.Add(pnlCol4);

            if (this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == pParametro.Parametro)
               || this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#"))
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
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, this.UpdatePanel2.GetType(), "ScriptAgregar" + btn.ID, scriptBtn.ToString(), true);
            }


            return miPanel;
        }

        //private void DateSelector_TextChanged(object sender, EventArgs e)
        //{
        //    TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((TextBox)sender).ID);

        //    paramValor.ValorParametro = string.Empty;
        //    TextBox chkList = (TextBox)sender;
        //    paramValor.ValorParametro = chkList.Text;

        //    this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
        //}

        private PlaceHolder AddYearMonthCombo(TGEMailingParametros parametro)
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


        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiMailing);
            this.MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            ForEachParametros();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiMailing.FechaAlta = DateTime.Now;
                    guardo = MailingF.TGEMailingAgregar(this.MiMailing);
                    break;
                case Gestion.Modificar:
                    guardo = MailingF.TGEMailingModificar(this.MiMailing);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.MostrarMensaje(this.MiMailing.CodigoMensaje, false);
                this.btnAceptar.Visible = false;
                this.btnEditarPlantilla.Visible = true;
                //this.btnPruebaEnvio.Visible = true;
                this.btnEjecutarAhora.Visible = true;
            }
            else
            {
                this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdMailing", this.MiMailing.IdMailing);
            if (!string.IsNullOrEmpty(this.paginaSegura.viewStatePaginaSegura))
                this.MisParametrosUrl.Add("UrlReferrer", this.paginaSegura.viewStatePaginaSegura.ToString());
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingListar.aspx"), true);

        }

        protected void btnEditarPlantilla_Click(object sender, EventArgs e)
        {
            this.btnAceptar_Click(null, EventArgs.Empty);
            this.MapearControlesAObjeto(this.MiMailing);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdMailing", MiMailing.IdMailing);
            this.MisParametrosUrl.Add("IdPlantilla", MiMailing.Plantillas.IdPlantilla);
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasModificar.aspx"), true);
        }

        //protected void btnPruebaEnvio_Click (object sender, EventArgs e)
        //{
        //    bool guardo = true;
        //    //this.MapearControlesAObjeto(this.MiMailing);
        //    this.btnAceptar_Click(sender, e);
        //    this.MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
        //    this.MiMailing.MailingProcesos.PruebaEnvio = true;
        //    MiMailing.MailingProcesamiento.EnvioManual = false;
        //    guardo = MailingF.TGEMailingEnviarMails(this.MiMailing);
        //    if (guardo)
        //    {
        //        this.MostrarMensaje("Se ha enviado un correo de prueba al mail " + this.UsuarioActivo.CorreoElectronico, false);
        //        //this.MostrarMensaje(this.MiMailing.CodigoMensaje, false);
        //        this.btnAceptar.Visible = false;
        //    }
        //    else
        //    {
        //        this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
        //    }
        //}

        protected void btnEjecutarAhora_Click (object sender, EventArgs e)
        {
            bool guardo = true;
            //this.MapearControlesAObjeto(this.MiMailing);
            this.btnAceptar_Click(sender, e);
            MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MiMailing.MailingProcesamiento.EnvioManual = true;
            MiMailing.MailingProcesos.PruebaEnvio = false;
            guardo = MailingF.TGEMailingEnviarMails(MiMailing);
      

            if (guardo)
            {
                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdMailingProcesamiento", MiMailing.MailingProcesamiento.IdMailingProcesamiento);
     
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingEnvioManual.aspx"), true);
            }
            else
            {
                this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //this.MapearControlesAObjeto(this.MiMailing);
            this.btnAceptar_Click(sender, e);
            MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MiMailing.MailingProcesamiento.EnvioManual = true;
            MiMailing.MailingProcesos.PruebaEnvio = false;
            guardo = MailingF.TGEMailingEnviarMails(MiMailing);


            if (guardo)
            {
                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdMailingProcesamiento", MiMailing.MailingProcesamiento.IdMailingProcesamiento);

                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingProcesamientosListar.aspx"), true);
            }
            else
            {
                this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
            }
        }




        protected void btnAdjuntosAgregar_Click(object sender, EventArgs e)
        {
            gvDatos.Visible = true;
            TGEMailingAdjuntos mailingAdjuntos = new TGEMailingAdjuntos();
            mailingAdjuntos.Estado.IdEstado = (int)Estados.Activo;
            mailingAdjuntos.EstadoColeccion = EstadoColecciones.Agregado;
            mailingAdjuntos.Plantilla.IdPlantilla = Convert.ToInt32(ddlAdjuntos.SelectedValue);
            mailingAdjuntos.Plantilla.NombrePlantilla = ddlAdjuntos.SelectedItem.Text;
            MiMailing.MailingAdjuntos.Add(mailingAdjuntos);
            mailingAdjuntos.IdMailingAdjunto = -1 * MiMailing.MailingAdjuntos.IndexOf(mailingAdjuntos);
            gvDatos.DataSource = MiMailing.MailingAdjuntos;
            gvDatos.DataBind();
        }
     
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEMailing parametros = this.BusquedaParametrosObtenerValor<TGEMailing>();
            parametros.IndiceColeccion = e.NewPageIndex;
            parametros.HashTransaccion = tpPlantillasAdjuntos.TabIndex;
            parametros.IdMailing = MiMailing.IdMailing;
            parametros.BusquedaParametros = true;
            BusquedaParametrosGuardarValor<TGEMailing>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = MiMailing.DetalleEnvio;
            gvDatos.DataBind();
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int IdMailingAdjunto = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            TGEMailing parametros = this.BusquedaParametrosObtenerValor<TGEMailing>();
            parametros.HashTransaccion = tpPlantillasAdjuntos.TabIndex;
            parametros.IdMailing = MiMailing.IdMailing;
            parametros.BusquedaParametros = true;


            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPlantilla", MiMailing.Plantillas.IdPlantilla);
            this.MisParametrosUrl.Add("IdMailing", MiMailing.IdMailing);
     
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.btnAceptar_Click(sender, e);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasModificar.aspx"), true);
            }
            else if (e.CommandName == "Eliminar")
            {
                if (IdMailingAdjunto < 0)
                {
                    MiMailing.MailingAdjuntos.Remove(MiMailing.MailingAdjuntos.First(x => x.IdMailingAdjunto == IdMailingAdjunto));
                }
                else
                {
                    //TGEMailingAdjuntos item = MiMailing.MailingAdjuntos.Find(x => x.IdMailingAdjunto == IdMailingAdjunto);
                    //item.Estado.IdEstado = (int)Estados.Baja;
                    //item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(item, Gestion.Anular);
                    TGEMailingAdjuntos mad = MiMailing.MailingAdjuntos.First(x => x.IdMailingAdjunto == IdMailingAdjunto);
                    mad.Estado.IdEstado = (int)Estados.Baja;
                    mad.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(mad, this.GestionControl);
                }
                AyudaProgramacion.CargarGrillaListas<TGEMailingAdjuntos>(MiMailing.MailingAdjuntos, true, this.gvDatos, true);
            }
        }
     

    }
}