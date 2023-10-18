using AjaxControlToolkit;
using Comunes.Entidades;
using Evol.Controls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using Reportes.Entidades;
using RestSharp.Extensions;
using Servicio.AccesoDatos;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
namespace IU.Modulos.ProcesosDatos.Controles
{
    public partial class TiposCargosLotesEnviadosDatos : ControlesSeguros
    {
        protected SisProcesosProcesamiento MiProcesoProcesamiento
        {
            get { return (SisProcesosProcesamiento)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiProcesoProcesamientoLotes"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiProcesoProcesamientoLotes"] = value; }
        }
        protected CarTiposCargosLotesEnviados MiLoteEnviado
        {
            get { return (CarTiposCargosLotesEnviados)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiLote"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMiLote"] = value; }
        }
        protected List<CarTiposCargosLotesEnviadosDetalles> MiLoteEnviadoDetalles
        {
            get { return (List<CarTiposCargosLotesEnviadosDetalles>)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetalles"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetalles"] = value; }
        }
        protected List<CarTiposCargosLotesEnviadosDetalles> MiLoteEnviadoDetallesModificados
        {
            get { return (List<CarTiposCargosLotesEnviadosDetalles>)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetallesModificados"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetallesModificados"] = value; }
        }
        protected List<CarTiposCargosLotesEnviadosDetalles> MiLoteEnviadoDetallesModificadosAdicionales
        {
            get { return (List<CarTiposCargosLotesEnviadosDetalles>)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetallesModificadosAdicionales"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetallesModificadosAdicionales"] = value; }
        }
        protected DataTable MiLoteEnviadoDetallesDT
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetallesDT"]; }
            set { Session[this.MiSessionPagina + "ProcesosDatosModificarDatosMisLotesDetallesDT"] = value; }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.MiProcesoProcesamiento = new SisProcesosProcesamiento();
                this.MiLoteEnviadoDetalles = new List<CarTiposCargosLotesEnviadosDetalles>();
                this.MiLoteEnviadoDetallesModificados = new List<CarTiposCargosLotesEnviadosDetalles>();
            }
        }
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.gvDatosLote.PageSizeEvent += this.GvDatosLotes_PageSizeEvent;
            if (this.IsPostBack)
            {
                if (this.MiProcesoProcesamiento != null && this.MiProcesoProcesamiento.Proceso.IdProceso > 0)
                {
                    this.ObtenerValoresRequestForm(this.MiProcesoProcesamiento.Proceso);
                    this.ArmarTablaParametros(this.MiProcesoProcesamiento.Proceso);
                }
            }
        }
        public void IniciarControl(CarTiposCargosLotesEnviados pParametro, Gestion pGestion)
        {
            try
            {
                this.GestionControl = pGestion;
                this.CargarCombos();
                this.MiLoteEnviado = pParametro;
                switch (pGestion)
                {
                    case Gestion.Consultar:
                        this.txtDescripcion.Visible = true;
                        this.lblDescripcion.Visible = true;
                        this.lblImputacion.Visible = true;
                        this.ddlImputacion.Visible = true;
                        this.ddlProceso.Visible = true;
                        this.lblNombreProceso.Visible = false;
                        this.btnBuscar.Visible = true;
                        this.upDatosFiltrar.Update();
                        this.gvDatosLote.Visible = true;
                        this.upDatos.Update();
                        this.upProceso.Update();
                        this.btnAceptar.Visible = false;
                        this.btnContinuar.Visible = false;
                        this.ddlProceso.Visible = false;
                        this.lblProcesos.Visible = false;
                        this.divParametros.Visible = false;
                        this.CargarListaSP(pParametro);
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "EsconderDiv", "EsconderDiv();", true);
                        break;
                    case Gestion.Agregar:
                        break;
                    case Gestion.Modificar:
                        break;
                    case Gestion.ConfirmarAgregar:
                        this.txtDescripcion.Visible = true;
                        this.lblDescripcion.Visible = true;
                        this.lblImputacion.Visible = true;
                        this.ddlImputacion.Visible = true;
                        this.lblNombreProceso.Visible = false;
                        this.btnBuscar.Visible = true;
                        this.upDatosFiltrar.Update();
                        this.gvDatosLote.Visible = true;
                        this.upDatos.Update();
                        this.upProceso.Update();

                        this.CargarListaSP(pParametro);
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "EsconderDiv", "EsconderDiv();", true);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                this.btnAceptar.Visible = false;
                this.btnContinuar.Visible = false;
                this.txtDescripcion.Visible = false;
                this.lblDescripcion.Visible = false;
                this.lblImputacion.Visible = false;
                this.ddlImputacion.Visible = false;
                this.btnBuscar.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "EsconderDiv", "EsconderDiv();", true);
                this.upDatosFiltrar.Update();
                this.upBotones.Update();
                this.MostrarMensaje(ex.Message, true);
            }
        }
        private void CargarListaSP(CarTiposCargosLotesEnviados pParametro)
        {
            DataSet resultado = new DataSet();
            this.MiProcesoProcesamiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            if (GestionControl == Gestion.Consultar)
            {
                this.MiLoteEnviadoDetalles = LotesCobranzasF.TiposCargosLotesObtenerListaDetallesPorID(pParametro);
                if (this.MiLoteEnviadoDetalles.Count > 0)
                {
                    this.gvDatosLote.DataSource = this.MiLoteEnviadoDetalles;
                    this.gvDatosLote.DataBind();
                    this.btnBuscar_Click(null, new EventArgs());
                }
            }
            else
            {
                ProcesosDatosF.ProcesosEjecutarProcesosObtenerGrilla(this.MiProcesoProcesamiento, ref resultado);
                if (resultado.Tables.Count > 1)
                {
                    //this.MiLoteEnviadoDetalles = MapearTablaLista(resultado.Tables[1].AsEnumerable().Where(x=>x.Field<int>("IdTipoLoteEnviadoDetalle");
                    this.MiProcesoProcesamiento.IdProcesoProcesamiento = Convert.ToInt32(resultado.Tables[0].Rows[0]["Resultado"].ToString());//IdProcesoProcesamiento
                    this.MiLoteEnviadoDetalles = MapearTablaLista(resultado.Tables[1]);
                    this.MiLoteEnviadoDetallesDT = resultado.Tables[1];
                    if (this.MiLoteEnviadoDetalles.Count > 0)
                    {
                        this.gvDatosLote.DataSource = this.MiLoteEnviadoDetalles;
                        this.gvDatosLote.DataBind();
                        this.btnBuscar_Click(null, new EventArgs());
                    }
                    else
                    {
                        throw new Exception("Error al cargar grilla.");
                    }
                }
            }
        }
        private List<CarTiposCargosLotesEnviadosDetalles> MapearTablaLista(DataTable table)
        {
            int flag = 0;
            List<CarTiposCargosLotesEnviadosDetalles> list = new List<CarTiposCargosLotesEnviadosDetalles>();
            foreach (DataRow row in table.Rows)
            {
                CarTiposCargosLotesEnviadosDetalles lote = new CarTiposCargosLotesEnviadosDetalles();
                lote.IdAfiliado = int.Parse(row["IdAfiliado"].ToString());
                lote.IdTipoCargoLoteEnviadoDetalle = int.Parse(row["IdTipoCargoLoteEnviadoDetalle"].ToString());
                lote.ImporteAAplicar = string.IsNullOrEmpty(row["ImporteAAplicar"].ToString()) ? (Nullable<decimal>)null : decimal.Parse(row["ImporteAAplicar"].ToString());
                lote.ImporteEnviado = decimal.Parse(row["ImporteEnviado"].ToString());
                lote.NumeroDocumento = long.Parse(row["NumeroDocumento"].ToString());
                lote.MatriculaIAF = long.Parse(row["MatriculaIAF"].ToString());
                lote.Observaciones = row["Observaciones"].ToString();
                lote.NumeroSocio = row["NumeroSocio"].ToString();
                lote.ApellidoNombre = row["ApellidoNombre"].ToString();
                lote.TipoCargoLoteEnviado = new CarTiposCargosLotesEnviados();
                lote.TipoCargoLoteEnviado.IdTipoCargoLoteEnviado = int.Parse(row["IdTipoCargoLoteEnviado"].ToString());
                lote.Estado = new TGEEstados();
                lote.Estado.IdEstado = int.Parse(row["IdEstado"].ToString());
                list.Add(lote);
                if (flag == 0)
                {
                    this.MiLoteEnviado.IdTipoCargoLoteEnviado = lote.TipoCargoLoteEnviado.IdTipoCargoLoteEnviado;
                    flag++;
                }
            }
            return list;
        }
        private void CargarCombos()
        {
            SisProcesos filtro = new SisProcesos();
            filtro.Filtro = "Cobranza";
            filtro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlProceso.DataSource = ProcesosDatosF.ProcesosObtenerListaFiltro(filtro);
            this.ddlProceso.DataTextField = "Descripcion";
            this.ddlProceso.DataValueField = "IdProceso";
            this.ddlProceso.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlProceso, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlImputacion.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TipoImputacionLote);
            this.ddlImputacion.DataTextField = "Descripcion";
            this.ddlImputacion.DataValueField = "CodigoValor";
            this.ddlImputacion.DataBind();
            ListItem item = ddlImputacion.Items.FindByValue("04");
            if (item != null)
                ddlImputacion.SelectedValue = "04";

        }
        protected void ddlProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlProceso.SelectedValue))
            {
                this.MiProcesoProcesamiento = new SisProcesosProcesamiento();
                this.MiProcesoProcesamiento.Proceso.IdProceso = Convert.ToInt32(this.ddlProceso.SelectedValue);
                this.MiProcesoProcesamiento.Proceso = ProcesosDatosF.ProcesosObtenerDatosCompletos(this.MiProcesoProcesamiento.Proceso);
                List<SisProcesosArchivos> lista = new List<SisProcesosArchivos>
                {
                    this.MiProcesoProcesamiento.Proceso.ProcesoArchivo
                };
                this.gvArchivo.DataSource = lista;
                this.gvArchivo.DataBind();
                this.gvDatos.DataSource = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.ProcesosArchivosCampos;
                this.gvDatos.DataBind();
                this.pnlArchivo.Visible = this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.ProcesaArchivo;
                this.upEstructura.Update();
                if (this.MiProcesoProcesamiento.Proceso.Parametros.Count > 0)
                {
                    this.ArmarTablaParametros(this.MiProcesoProcesamiento.Proceso);
                }
            }
            else
            {
                this.MiProcesoProcesamiento = new SisProcesosProcesamiento();
                this.gvArchivo.DataSource = null;
                this.gvArchivo.DataBind();
                this.gvDatos.DataSource = null;
                this.gvDatos.DataBind();
                this.upEstructura.Update();
                this.pnlArchivo.Visible = false;
                this.Accordion1.Visible = false;
                this.upEstructura.Update();
            }
        }
        #region Parametros Dinamicos
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
        private void ArmarTablaParametros(SisProcesos pProceso)
        {
            this.pnlArchivo.Visible = pProceso.ProcesoArchivo.ProcesaArchivo;
            this.Accordion1.Visible = pProceso.ProcesoArchivo.ProcesaArchivo;
            this.tablaParametros.Controls.Clear();

            int countControl = 0;
            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssRow;
            foreach (SisParametros parametro in pProceso.Parametros)
            {
                DataSet dsParametros = new DataSet();
                SisProcesos proc;
                SisParametros param;

                if (!string.IsNullOrEmpty(parametro.StoredProcedure)
                    && parametro.ParamDependiente.Trim() == string.Empty
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
                    default:
                        break;
                }
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
                                if (((TextBox)ctrl).Text != string.Empty)
                                {
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
                    }
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
            paramValor.ValorParametro = valor;
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
            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol12;
            GridViewCheckDinamico gvCheck = (GridViewCheckDinamico)LoadControl("~/Modulos/ProcesosDatos/Controles/GridViewCheckDinamico.ascx");
            gvCheck.ID = Parametro;
            gvCheck.IniciarControl(dsDatos, Parametro, NombreParametro);
            pnlRow.Controls.Add(gvCheck);
            panel.Controls.Add(pnlRow);
            return panel;
        }
        private void BtnGVDinamico_Click(object sender, EventArgs e)
        {
            SisParametros paramValor = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => "btnGVDinamico" + x.Parametro == ((Button)sender).ID);
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
            dateSelector.CssClass = "form-control datepicker";
            dateSelector.Columns = 10;
            dateSelector.ID = Parametro;
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
                ScriptManager.RegisterClientScriptBlock(this.upProceso, this.upProceso.GetType(), "ScriptAgregar" + btn.ID, scriptBtn.ToString(), true);
            }
            return miPanel;
        }
        //private void MapearObjetoAControles(LavEdificios pParametro)
        //{
        //    this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
        //    //this.txtCodigoPostal.Text = pParametro.CodigoPostal.ToString();
        //    this.txtContacto.Text = pParametro.Contacto;
        //    this.txtDescripcion.Text = pParametro.Descripcion;
        //    this.txtLocalidad.Text = pParametro.Localidad;
        //    this.txtNumero.Text = pParametro.NumeroDireccion.ToString();
        //    this.txtPartido.Text = pParametro.Partido;
        //    this.txtDireccion.Text = pParametro.Direccion;
        //    //this.txtLatitud.Text = pParametro.Latitud.ToString();
        //    //this.txtLongitud.Text = pParametro.Longitud.ToString();
        //    this.hdfLatitud.Value = pParametro.Latitud.ToString();
        //    this.hdfLongitud.Value = pParametro.Longitud.ToString();
        //    this.hdfCodigoPostal.Value = pParametro.CodigoPostal;
        //    this.hdfNumeroCasa.Value = pParametro.NumeroDireccion.ToString();
        //    this.hdfCalleCasa.Value = pParametro.Direccion.ToString();
        //    this.txtProvincia.Text = pParametro.Provincia;
        //    //this.hdfLocalidad.Value= pParametro.Localidad;
        //    //this.hdfProvincia.Value= pParametro.Provincia;
        //    this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);

        //    //ARMO QR
        //    //string base64String = pParametro.CodigoQR == null ? string.Empty : Convert.ToBase64String(pParametro.CodigoQRImagen, 0, pParametro.CodigoQRImagen.Length);
        //    //this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
        //    //this.imgLogo.Visible = true;

        //    this.txtFechaAlta.Text = pParametro.FechaPEM.ToShortDateString();
        //    this.ddlHorario.SelectedValue = pParametro.IdHorario.ToString();
        //    this.ddlContrato.SelectedValue = pParametro.IdContrato.ToString();
        //    this.txtCodigo.Text = pParametro.IdEdificio.ToString();
        //    this.txtFrecuenciaRecaudacion.Text = pParametro.FrecuenciaRecaudacion.ToString();
        //    this.txtFrecuenciaAspiracion.Text = pParametro.FrecuenciaAspiracion.ToString();

        //    this.txtCantidadMaquinasLavado.Text = pParametro.CantidadMaquinasLavado.ToString();
        //    this.txtCantidadMaquinasSecado.Text = pParametro.CantidadMaquinasSecado.ToString();

        //    this.hdfLocalizacionCompleta.Value = pParametro.Localizacion;

        //    if(pParametro.IdSistemaPago > 0)
        //    this.ddlSistemaPago.SelectedValue = pParametro.IdSistemaPago.ToString();

        //    this.txtUnidadesFuncionales.Text = pParametro.UnidadesFuncionales.HasValue ? pParametro.UnidadesFuncionales.ToString() : "";

        //    if (!string.IsNullOrEmpty(pParametro.Latitud.ToString()))
        //    {
        //        this.ddlLocalizacion.Items.Clear();
        //        this.ddlLocalizacion.Items.Add(new ListItem(pParametro.Localizacion, pParametro.Localizacion));
        //        this.ddlLocalizacion.SelectedValue = pParametro.Localizacion;
        //    }

        //    this.ctrArchivos.IniciarControl(pParametro, this.GestionControl);

        //    if(pParametro.Maquinas.Count > 0)
        //    {
        //        AyudaProgramacion.CargarGrillaListas<LavMaquinas>(pParametro.Maquinas, false, this.gvMaquinas, true);
        //        this.upMaquinas.Update();
        //    }

        //    if (!string.IsNullOrEmpty(pParametro.Servicios))
        //    {
        //        for (int i = 0; i <= this.ddlServicios.Items.Count - 1; i++)
        //        {
        //            if (pParametro.Servicios.Contains(this.ddlServicios.Items[i].Value))
        //            {
        //                this.ddlServicios.Items[i].Selected = true;
        //            }
        //        }
        //    }
        //}
        //private void MapearControlesAObjeto(LavEdificios pParametro)
        //{
        //    pParametro.CodigoPostal = this.txtCodigoPostal.Text;
        //    pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
        //    pParametro.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
        //    pParametro.Contacto = this.txtContacto.Text;
        //    pParametro.Descripcion = this.txtDescripcion.Text;
        //    pParametro.Localidad = this.txtLocalidad.Text;
        //    //pParametro.NumeroDireccion = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
        //    pParametro.Provincia = this.txtProvincia.Text;
        //    //pParametro.Partido = this.txtPartido.Text;

        //    pParametro.CodigoQR = "https://" + "maps.google.com/?q=" + this.hdfLatitud.Value.ToString().Replace(",", ".") + "," + this.hdfLongitud.Value.ToString().Replace(",", ".");

        //    pParametro.IdContrato = this.ddlContrato.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlContrato.SelectedValue);
        //    pParametro.IdHorario = this.ddlHorario.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlHorario.SelectedValue);
        //    pParametro.FechaPEM = this.txtFechaAlta.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaAlta.Text);
        //    pParametro.FrecuenciaAspiracion = this.txtFrecuenciaAspiracion.Text == string.Empty ? 0 : Convert.ToInt32(this.txtFrecuenciaAspiracion.Text);
        //    pParametro.FrecuenciaRecaudacion = this.txtFrecuenciaRecaudacion.Text == string.Empty ? 0 : Convert.ToInt32(this.txtFrecuenciaRecaudacion.Text);
        //    pParametro.CantidadMaquinasLavado = this.txtCantidadMaquinasLavado.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadMaquinasLavado.Text);
        //    pParametro.CantidadMaquinasSecado= this.txtCantidadMaquinasSecado.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadMaquinasSecado.Text);

        //    pParametro.IdSistemaPago = this.ddlSistemaPago.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlSistemaPago.SelectedValue);
        //    pParametro.UnidadesFuncionales = this.txtUnidadesFuncionales.Text == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.txtUnidadesFuncionales.Text);

        //    pParametro.NumeroDireccion = this.hdfNumeroCasa.Value == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.hdfNumeroCasa.Value);
        //    pParametro.Direccion = this.hdfCalleCasa.Value == string.Empty ? null : this.hdfCalleCasa.Value;
        //    pParametro.Latitud = this.hdfLatitud.Value == string.Empty ? 0 :Convert.ToDecimal(this.hdfLatitud.Value);
        //    pParametro.Longitud = this.hdfLongitud.Value == string.Empty ? 0 : Convert.ToDecimal(this.hdfLongitud.Value);
        //    pParametro.Localizacion = this.hdfLocalizacionCompleta.Value == string.Empty ? null : this.hdfLocalizacionCompleta.Value;
        //    pParametro.CodigoPostal = this.hdfCodigoPostal.Value == string.Empty ? null : this.hdfCodigoPostal.Value;

        //    if (!string.IsNullOrEmpty(this.ddlServicios.SelectedValue))
        //    {
        //        pParametro.Servicios = "";
        //        foreach (ListItem item in this.ddlServicios.Items)
        //        {
        //            if (item.Selected)
        //            {
        //                pParametro.Servicios += item.Value.ToString() + ",";
        //            }
        //        }
        //        pParametro.Servicios = pParametro.Servicios.Remove(pParametro.Servicios.Length - 1);
        //    }
        //    pParametro.Archivos = ctrArchivos.ObtenerLista();
        //}
        #endregion
        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            this.AsyncFileUpload1.FailedValidation = false;
            this.AsyncFileUpload1.ClearAllFilesFromPersistedStore();
        }
        protected void AsyncFileUpload1_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            this.AsyncFileUpload1.FailedValidation = false;
            this.AsyncFileUpload1.ClearAllFilesFromPersistedStore();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.PersistirGrilla(true);
            this.MiLoteEnviado.LoteDetalles = new XmlDocument();
            this.MiLoteEnviado.LoteDetalles.LoadXml(this.ArmarXML());
            this.MiLoteEnviado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MiLoteEnviado.IdProcesoProcesamiento = this.MiProcesoProcesamiento.IdProcesoProcesamiento;
            SisParametros fechaCobro = this.MiProcesoProcesamiento.Proceso.Parametros.Find(x => x.Parametro == "FechaCobro");
            if (fechaCobro != null)
            {
                this.MiLoteEnviado.FechaCobro = Convert.ToDateTime(fechaCobro.ValorParametro);
            }
            switch (this.GestionControl)
            {
                case Gestion.ConfirmarAgregar:
                    guardo = LotesCobranzasF.TiposCargosLotesEnviadosAgregar(this.MiLoteEnviado);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiLoteEnviado.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = LotesCobranzasF.TiposCargosLotesEnviadosModificar(this.MiLoteEnviado);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiLoteEnviado.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiLoteEnviado.CodigoMensaje, true, this.MiLoteEnviado.CodigoMensajeArgs);
                if (this.MiLoteEnviado.dsResultado != null)
                {
                    this.MiLoteEnviado.dsResultado = null;
                }
            }
        }
        protected void btnContinuar_Click(object sender, EventArgs e)
        {
            if (this.MiProcesoProcesamiento.Proceso.TieneArchivo)
            {
                #region OCULTO CONTROLES
                this.btnContinuar.Visible = false;
                this.btnAceptar.Visible = true;
                this.Accordion1.Visible = false;
                this.divParametros.Visible = false;
                this.tablaParametros.Visible = false;
                this.lblProcesos.Visible = false;
                this.ddlProceso.Visible = false;
                this.lblNombreProceso.Visible = true;
                this.lblNombreProceso.Text = this.MiProcesoProcesamiento.Proceso.Descripcion;
                this.upProceso.Update();
                this.upBotones.Update();
                this.upEstructura.Update();
                #endregion
                this.ObtenerValoresRequestForm(this.MiProcesoProcesamiento.Proceso);
                this.ForEachParametros();
                this.MiProcesoProcesamiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.IniciarControl(this.MiLoteEnviado, Gestion.ConfirmarAgregar);
            }
            else
            {
                this.MostrarMensaje("Falta cargar el archivo.", true);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }
        protected void FileUploadComplete(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            try
            {
                string filename = System.IO.Path.GetFileName(this.AsyncFileUpload1.FileName);
                this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo = this.AsyncFileUpload1.FileName;

                string HoyTexto = DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss");

                TGEParametrosValores RutaDelArchivo = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.ProcesosDatosDirectorioArchivo);

                string RutaDelArchivoTexto = string.Concat(RutaDelArchivo.ParametroValor.ToString(), this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo);

                if (File.Exists(RutaDelArchivoTexto))// + strFileName))
                {
                    File.Copy(RutaDelArchivoTexto, RutaDelArchivoTexto + "-" + HoyTexto);
                    File.Delete(RutaDelArchivoTexto);
                }
                this.AsyncFileUpload1.SaveAs(RutaDelArchivoTexto);
                this.MiProcesoProcesamiento.Proceso.TieneArchivo = true;
                this.btnContinuar.Visible = true;
                this.upBotones.Update();
            }
            catch (Exception ex)
            {
                this.MiProcesoProcesamiento.Proceso.ProcesoArchivo.NombreArchivo = "";
                this.MostrarMensaje("Error al cargar el archivo.", true);
            }
        }
        #region GVDatosLotes
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.PersistirGrilla(false);
            CarTiposCargosLotesEnviadosDetalles parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosLotesEnviadosDetalles>();
            parametros.PageIndex = 0;
            gvDatosLote.PageIndex = 0;
            CargarLista(parametros);
        }
        protected void gvDatosLote_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Listar.ToString()))//lupa
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            HiddenField id = (HiddenField)this.gvDatosLote.Rows[index].FindControl("hdfIdTipoCargoLoteEnviadoDetalle");
            HiddenField mostrar = (HiddenField)this.gvDatosLote.Rows[index].FindControl("hdfMostrarDetalle");
            HiddenField idAfiliado = (HiddenField)this.gvDatosLote.Rows[index].FindControl("hdfIdAfiliadoCabecera");
            Panel panel = (Panel)this.gvDatosLote.Rows[index].FindControl("pnlDatosDetalles");
            ImageButton lupa = (ImageButton)this.gvDatosLote.Rows[index].FindControl("btnBuscarCargos");
            ImageButton plus = (ImageButton)this.gvDatosLote.Rows[index].FindControl("btnConsultar");

            if (e.CommandName != Gestion.Listar.ToString())
            {
                if (plus.ImageUrl.Contains("plus.png"))
                {
                    plus.ImageUrl = "~/Imagenes/minus.png";
                }
                else
                {
                    plus.ImageUrl = "~/Imagenes/plus.png";
                }
                if (GestionControl != Gestion.Consultar)
                {
                    lupa.Visible = !lupa.Visible;
                }

                if (mostrar.Value == "1")
                {
                    //panel.Attributes.Remove("Style");
                    panel.Visible = true;
                    mostrar.Value = "0";
                }
                else
                {
                    //panel.Attributes.Add("Style", "display: none");
                    panel.Visible = false;
                    mostrar.Value = "1";
                }
            }
            this.upDatos.Update();

            CarTiposCargosLotesEnviadosDetalles detalle = new CarTiposCargosLotesEnviadosDetalles
            {
                IdTipoCargoLoteEnviadoDetalle = Convert.ToInt32(id.Value.HasValue() == false ? "-1" : id.Value)
            };
            if (this.GestionControl != Gestion.Consultar)
            {
                this.PersistirGrilla(false);
            }
            List<CarTiposCargosLotesEnviadosDetalles> lista = new List<CarTiposCargosLotesEnviadosDetalles>();
            List<CarTiposCargosLotesEnviadosDetalles> cargosAdicionales = new List<CarTiposCargosLotesEnviadosDetalles>();
            List<CarTiposCargosLotesEnviadosDetalles> datosModificados = this.MiLoteEnviadoDetallesModificados.FindAll(x => x.IdTipoCargoLoteEnviadoDetalle == detalle.IdTipoCargoLoteEnviadoDetalle && x.IdCuentaCorriente > 0);
            cargosAdicionales = this.MiLoteEnviadoDetallesModificados.FindAll(x => x.IdTipoCargoLoteEnviadoDetalle == 0 && x.IdCuentaCorriente > 0 && x.IdAfiliado == Convert.ToInt32(idAfiliado.Value == string.Empty ? "0" : idAfiliado.Value));
            if (e.CommandName == Gestion.Listar.ToString())
            {
                if (!(this.MiLoteEnviadoDetallesModificados.Exists(x => x.IdAfiliado == Convert.ToInt32(idAfiliado.Value == string.Empty ? "0" : idAfiliado.Value) && x.IdTipoCargoLoteEnviadoDetalle == 0)))
                {
                    detalle.IdAfiliado = Convert.ToInt32(idAfiliado.Value);
                    if (cargosAdicionales.Count == 0)
                    {
                        cargosAdicionales = LotesCobranzasF.TiposCargosLotesEnviadosObtenerDetallesPorAfiliado(detalle);
                    }
                    if (cargosAdicionales.Count > 0)
                    {
                        foreach (CarTiposCargosLotesEnviadosDetalles item in cargosAdicionales)
                        {
                            if (!(this.MiLoteEnviadoDetallesModificados.Exists(x => x.IdAfiliado == Convert.ToInt32(idAfiliado.Value == string.Empty ? "0" : idAfiliado.Value) && x.IdTipoCargoLoteEnviadoDetalle == 0)))
                            {
                                datosModificados.Add(item);
                            }
                        }
                        this.MiLoteEnviadoDetallesModificados.AddRange(cargosAdicionales);
                    }
                    else
                    {
                        this.MostrarMensaje("No se encontraron cargos adicionales.", true);
                    }
                }
            }

            if (datosModificados.Count == 0)
            {
                lista.AddRange(LotesCobranzasF.TiposCargosLotesEnviadosObtenerDetalles(detalle));
                this.MiLoteEnviadoDetallesModificados.AddRange(lista);
            }
            else
            {
                lista.AddRange(LotesCobranzasF.TiposCargosLotesEnviadosObtenerDetalles(detalle));
                if(GestionControl != Gestion.Consultar)
                {
                    lista.AddRange(cargosAdicionales);
                }
                foreach (CarTiposCargosLotesEnviadosDetalles item in lista)
                {
                    CarTiposCargosLotesEnviadosDetalles itemModificado = datosModificados.Find(x => x.IdTipoCargoLoteEnviadoDetalle == item.IdTipoCargoLoteEnviadoDetalle && x.IdCuentaCorriente == item.IdCuentaCorriente);
                    if (itemModificado != null)
                    {
                        item.FechaMovimiento = itemModificado.FechaMovimiento;
                        item.Concepto = itemModificado.Concepto;
                        item.Observaciones = itemModificado.Observaciones;
                        item.Periodo = itemModificado.Periodo;
                        item.DescripcionEstado = itemModificado.DescripcionEstado;
                        item.ImporteCC = itemModificado.ImporteCC;
                        item.ImporteCobrado = itemModificado.ImporteCobrado;
                        item.ImporteAAplicarLEDCC = itemModificado.ImporteAAplicarLEDCC;
                    }
                }
            }
            if (lista.Count > 0)
            {
                GridView gv = ((GridView)this.gvDatosLote.Rows[index].FindControl("gvDatosDetalles"));
                if (gv != null)
                {
                    gv.RowDataBound += gvDatosDetalles_RowDataBound;
                    gv.DataSource = lista;
                    gv.DataBind();
                }
            }
        }
        private string ArmarXML()
        {
            string XML = "<Lotes>";
            foreach (CarTiposCargosLotesEnviadosDetalles item in MiLoteEnviadoDetallesModificados)
            {
                XML = string.Concat(XML, "<Lote>" +
                    "<IdTipoCargoLoteEnviadoDetalle>", item.IdTipoCargoLoteEnviadoDetalle, "</IdTipoCargoLoteEnviadoDetalle>" +
                    "<IdCuentaCorriente>", item.IdCuentaCorriente, "</IdCuentaCorriente>" +
                    "<ImporteAImputar>", item.ImporteAAplicarLEDCC, "</ImporteAImputar>", "</Lote>");
            }
            return string.Concat(XML, "</Lotes>");
        }
        /// <summary>
        /// Recorre toda la grilla con sus subgrillas y guarda los cambios realizados hasta el momento.
        /// </summary>
        /// <param name="ultimaVez">Al pasarlo como "true" se le seteara el IdTipoCargoLoteEnviadoDetalle que le corresponda a ese registro. De lo contrario estara en 0.</param>
        private void PersistirGrilla(bool ultimaVez)
        {
            List<CarTiposCargosLotesEnviadosDetalles> lista = new List<CarTiposCargosLotesEnviadosDetalles>();
            foreach (GridViewRow fila in this.gvDatosLote.Rows)
            {
                int idLoteDetalle = Convert.ToInt32(gvDatosLote.DataKeys[fila.RowIndex]["IdTipoCargoLoteEnviadoDetalle"].ToString());
                GridView gv = ((GridView)fila.FindControl("gvDatosDetalles"));
                if (gv != null)
                {
                    foreach (GridViewRow item in gv.Rows)
                    {
                        HiddenField cc = (HiddenField)item.FindControl("hdfIdCuentaCorriente");
                        HiddenField afi = (HiddenField)item.FindControl("hdfIdAfiliado");
                        int idCuentaCorriente = Convert.ToInt32(cc.Value == string.Empty ? "-1" : cc.Value);
                        int idAfiliado = Convert.ToInt32(afi.Value == string.Empty ? "-1" : afi.Value);
                        CurrencyTextBox txtImporte = (CurrencyTextBox)item.FindControl("txtImporte");
                        if (txtImporte != null && (!string.IsNullOrEmpty(txtImporte.Text)))
                        {
                            CarTiposCargosLotesEnviadosDetalles aux = this.MiLoteEnviadoDetallesModificados.Find(x => x.IdCuentaCorriente == idCuentaCorriente && x.IdTipoCargoLoteEnviadoDetalle == idLoteDetalle);
                            if (aux == null)
                            {
                                List<CarTiposCargosLotesEnviadosDetalles> adicionales = this.MiLoteEnviadoDetallesModificados.FindAll(x => x.IdTipoCargoLoteEnviadoDetalle == 0 && x.IdCuentaCorriente > 0 && x.IdAfiliado == idAfiliado);
                                if (adicionales != null && adicionales.Count > 0)
                                {
                                    foreach (CarTiposCargosLotesEnviadosDetalles detalle in adicionales)
                                    {
                                        if (detalle.IdCuentaCorriente == idCuentaCorriente)
                                        {
                                            detalle.IdCuentaCorriente = idCuentaCorriente;
                                            detalle.ImporteAAplicarLEDCC = txtImporte.Decimal;// Convert.ToDecimal(txtImporte.Text.Replace("$", ""));
                                            detalle.EstadoColeccion = EstadoColecciones.Modificado;
                                            detalle.IdAfiliado = idAfiliado;
                                            if (ultimaVez)
                                            {
                                                detalle.IdTipoCargoLoteEnviadoDetalle = idLoteDetalle;
                                            }
                                        }
                                    }
                                }
                            }
                            if (aux != null)
                            {
                                aux.IdTipoCargoLoteEnviadoDetalle = idLoteDetalle;
                                aux.IdCuentaCorriente = idCuentaCorriente;
                                aux.ImporteAAplicarLEDCC = txtImporte.Decimal;//Convert.ToDecimal(txtImporte.Text.Replace("$", ""));
                                aux.EstadoColeccion = EstadoColecciones.Modificado;
                                aux.IdAfiliado = idAfiliado;
                            }
                        }
                    }
                }
            }
        }
        protected void gvDatosLote_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiLoteEnviadoDetallesDT.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatosLote_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosLotesEnviadosDetalles parametros = BusquedaParametrosObtenerValor<CarTiposCargosLotesEnviadosDetalles>();
            this.gvDatosLote.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }
        private void GvDatosLotes_PageSizeEvent(int pageSize)
        {
            CarTiposCargosLotesEnviadosDetalles parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosLotesEnviadosDetalles>();
            parametros.PageIndex = 0;
            this.gvDatosLote.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        private void CargarLista(CarTiposCargosLotesEnviadosDetalles pParametro)
        {
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            pParametro.IdProcesoProcesamientoDevolucion = this.MiProcesoProcesamiento.IdProcesoProcesamiento;
            pParametro.TipoCargoLoteEnviado.IdTipoCargoLoteEnviado = this.MiLoteEnviado.IdTipoCargoLoteEnviado;
            pParametro.Filtro = this.txtDescripcion.Text == string.Empty ? null : this.txtDescripcion.Text;
            pParametro.Observaciones = this.ddlImputacion.SelectedValue;
            this.gvDatosLote.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<CarTiposCargosLotesEnviadosDetalles>(pParametro);

            this.MiLoteEnviadoDetallesDT = LotesCobranzasF.TiposCargosLotesObtenerListaGrillaPaginado(pParametro);
            this.gvDatosLote.DataSource = this.MiLoteEnviadoDetallesDT;
            this.gvDatosLote.VirtualItemCount = this.MiLoteEnviadoDetallesDT.Rows.Count > 0 ? Convert.ToInt32(this.MiLoteEnviadoDetallesDT.Rows[0]["Cantidad"]) : 0;
            this.gvDatosLote.deTantos.Text = "de " + this.gvDatosLote.VirtualItemCount.ToString();
            this.gvDatosLote.PageIndex = pParametro.PageIndex;
            this.gvDatosLote.DataBind();
            AyudaProgramacion.FixGridView(this.gvDatosLote);
            this.upDatos.Update();
        }
        #endregion
        protected void gvDatosDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CurrencyTextBox control = ((CurrencyTextBox)e.Row.FindControl("txtImporte"));
                if (control != null)
                {
                    control.Attributes.Add("onchange", "ValidarTotal('" + control.ClientID + "');");
                    if (this.GestionControl == Gestion.Consultar)
                    {
                        control.Enabled = false;
                    }
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label control = ((Label)e.Row.FindControl("lblImporteTotal"));
                if (control != null)
                {
                    if(GestionControl != Gestion.Consultar)
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "InitFooterDetalle", "InitFooterDetalle('" + control.ClientID + "');", true);
                    }
                }
            }
        }
    }
}