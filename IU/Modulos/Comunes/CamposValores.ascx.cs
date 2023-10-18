using Comunes.Entidades;
using Evol.Controls;
using Generales.Entidades;
using Generales.FachadaNegocio;
using log4net.Util;
using Servicio.AccesoDatos;
using Servicio.Encriptacion;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;

namespace IU.Modulos.Comunes
{
    public partial class CamposValores : ControlesSeguros
    {

        private Objeto MiTablaValor
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiTablaValor"] == null ? new Objeto() : (Objeto)Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiTablaValor"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiTablaValor"] = value; }
        }

        private int MiIdRefTablaValor
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiIdRefTablaValor"] == null ? 0 : (int)Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiIdRefTablaValor"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiIdRefTablaValor"] = value; }
        }

        private int? MiIdRefValor
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiIdRefValor"] == null ? default(int?) : (int?)Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiIdRefValor"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiIdRefValor"] = value; }
        }

        private bool MiArmarParametros
        {
            get
            {
                return (bool)Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiArmarParametros"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresMiArmarParametros"] = value; }
        }

        private bool HabilitarControles
        {
            get
            {
                return (bool)Session[this.MiSessionPagina + this.ClientID + "CamposValoresHabilitarControles"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresHabilitarControles"] = value; }
        }

        private List<TGECampos> MisCampos
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "CamposValoresMisCampos"] == null ?
                    (List<TGECampos>)(Session[this.MiSessionPagina + this.ClientID + "CamposValoresMisCampos"] = new List<TGECampos>()) : (List<TGECampos>)Session[this.MiSessionPagina + this.ClientID + "CamposValoresMisCampos"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresMisCampos"] = value; }
        }

        //private bool Select2IncluirArchivos
        //{
        //    get
        //    {
        //        return (bool)Session[this.MiSessionPagina + "CamposValoresSelect2IncluirArchivos"];
        //    }
        //    set { Session[this.MiSessionPagina + "CamposValoresSelect2IncluirArchivos"] = value; }
        //}

        //private List<TGEListasValoresDetalles> MisListasValoresDetalles
        //{
        //    get
        //    {
        //        return (Session[this.MiSessionPagina + "CamposValoresMisListasValoresDetalles"] == null ?
        //            (List<TGEListasValoresDetalles>)(Session[this.MiSessionPagina + "CamposValoresMisListasValoresDetalles"] = new List<TGEListasValoresDetalles>()) : (List<TGEListasValoresDetalles>)Session[this.MiSessionPagina + "CamposValoresMisListasValoresDetalles"]);
        //    }
        //    set { Session[this.MiSessionPagina + "CamposValoresMisListasValoresDetalles"] = value; }
        //}

        private Dictionary<string, List<TGECampos>> MisDatos
        {
            get
            {
                return (Session[this.MiSessionPagina + this.ClientID + "CamposValoresMisDatos"] == null ?
                    (Dictionary<string, List<TGECampos>>)(Session[this.MiSessionPagina + this.ClientID + "CamposValoresMisDatos"] = new Dictionary<string, List<TGECampos>>()) : (Dictionary<string, List<TGECampos>>)Session[this.MiSessionPagina + this.ClientID + "CamposValoresMisDatos"]);
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresMisDatos"] = value; }
        }

        public bool MostrarControl
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "CamposValoresMostrarControl"] == null ? false : (bool)Session[this.MiSessionPagina + this.ClientID + "CamposValoresMostrarControl"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresMostrarControl"] = value; }
        }

        const string _validationGroup = "Aceptar";
        const string _cssLabel = "col-sm-3 col-form-label";
        const string _cssLabelCol6 = "col-lg-12 col-form-label";
        const string _cssRow = "row";
        const string _cssCol3 = "col-sm-9";
        const string _cssCol2 = "col-sm-12";
        const string _cssContainer = "col-12 col-md-8 col-lg-4";

        public string ctrValidationGroup { get; set; }
        public string cssLabel { get; set; }
        public string cssLabelCol6 { get; set; }
        public string cssRow { get; set; }
        public string cssCol { get; set; }
        public string ccsContainer { get; set; }

        private string MiValidationGroup
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "CamposValoresvalidationGroup"] == null ? _validationGroup : (string)Session[this.MiSessionPagina + this.ClientID + "CamposValoresvalidationGroup"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresvalidationGroup"] = value; }
        }

        private string MicssLabel
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "CamposValoresccsLabel"] == null ? _cssLabel : (string)Session[this.MiSessionPagina + this.ClientID + "CamposValoresccsLabel"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValoresccsLabel"] = value; }
        }

        private string MicssLabelCol6
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "CamposValorescssLabelCol6"] == null ? _cssLabelCol6 : (string)Session[this.MiSessionPagina + this.ClientID + "CamposValoresccsLabelCol6"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValorescssLabelCol6"] = value; }
        }

        private string MicssRow
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "CamposValorescssRow"] == null ? _cssRow : (string)Session[this.MiSessionPagina + this.ClientID + "CamposValorescssRow"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValorescssRow"] = value; }
        }

        private string MicssCol
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "CamposValorescssCol"] == null ? _cssCol3 : (string)Session[this.MiSessionPagina + this.ClientID + "CamposValorescssCol"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValorescssCol"] = value; }
        }

        private string MiccsContainer
        {
            get
            {
                return Session[this.MiSessionPagina + this.ClientID + "CamposValorescssContainer"] == null ? _cssContainer : (string)Session[this.MiSessionPagina + this.ClientID + "CamposValorescssContainer"];
            }
            set { Session[this.MiSessionPagina + this.ClientID + "CamposValorescssContainer"] = value; }
        }

        //private int MiIndiceColeccion
        //{
        //    get
        //    {
        //        return (Session[this.MiSessionPagina + "CamposValoresMiIndiceColeccion"] == null ? 0 : (int)Session[this.MiSessionPagina + "CamposValoresMiIndiceColeccion"]);
        //    }
        //    set { Session[this.MiSessionPagina + "CamposValoresMiIndiceColeccion"] = value; }
        //}

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!IsPostBack)
            {
                //MicssLabel = null;
                //MicssLabelCol6 = null;
                //MicssRow = null;
                //MicssCol = null;
                //MiccsContainer = ccsContainer;
            }
        }
        const string preid = "EvolCvId";
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (!this.IsPostBack)
            {
                this.MiArmarParametros = false;
                this.MisDatos = new Dictionary<string, List<TGECampos>>();
                this.MiValidationGroup = ctrValidationGroup;
                this.MicssCol = cssCol;
                this.MicssLabel = cssLabel;
                this.MicssLabelCol6 = cssLabelCol6;
                this.MiccsContainer = ccsContainer;
                this.MicssRow = cssRow;
            }

            //Literal lnkCss = new Literal();
            //lnkCss.ID = "Selec2Styles";
            //lnkCss.Text = string.Format("<link href=\"{0}\" rel=\"stylesheet\" />", ResolveUrl("~/assets/global/plugins/select2/css/select2.min.css"));
            //this.Controls.Add(lnkCss);
            //ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "mkSelec2js", ResolveUrl("~/assets/global/plugins/select2/js/select2.full.min.js"));
            //ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "mkSelec2jsEs", ResolveUrl("~/assets/global/plugins/select2/js/i18n/es.js"));

            if (this.MiArmarParametros)
            {
                this.ObtenerValoresParametros(this.MisCampos);
                this.ArmarTablaParametros(this.MisCampos, this.HabilitarControles);
                this.upCamposValores.Update();
            }
        }

        //void ctrCamposValoresPopUp_CamposValoresPopUpAceptar(TGECampos e)
        //{
        //    this.MisCampos[this.MiIndiceColeccion] = e;
        //    AyudaProgramacion.CargarGrillaListas<TGECampos>(this.MisCampos, false, this.gvArchivos, true);
        //}


        public void IniciarControl(Objeto pTablaValor, Objeto pTablaParametro, int pIdAfiliado, Gestion pGestion)
        {
            this.IniciarControl(pTablaValor, pTablaParametro, pGestion);
            ////this.MiIdAfiliado = pIdAfiliado;
            //this.MiTablaValor = pTablaValor;
            ////this.MisCampos = TGEGeneralesF.CamposObtenerListaFiltro(pTablaValor, pTablaParametro, pIdAfiliado);
            //List<TGECampos> campos = TGEGeneralesF.CamposObtenerListaFiltro(pTablaValor, pTablaParametro, pIdAfiliado);
            //if(this.MisDatos.ContainsKey(pTablaParametro.GetType().Name))
            //    this.MisDatos[pTablaParametro.GetType().Name] = campos;
            //else
            //    this.MisDatos.Add(pTablaParametro.GetType().Name, campos);
            ////this.IniciarControl(this.MisCampos, pGestion);
            //this.IniciarControl(this.MisDatos.Values.SelectMany(x => x).ToList(), pGestion);
        }

        public void IniciarControl(Objeto pTablaValor, Objeto pTablaParametro, Gestion pGestion)
        {
            IniciarControl(pTablaValor, pTablaParametro, pGestion, null);
        }
        public void IniciarControl(Objeto pTablaValor, Objeto pTablaParametro, Gestion pGestion, int? pIdRefValor)
        {
            bool regenerarControles = false;
            int cantidad = this.MisDatos.Values.SelectMany(x => x).ToList().Count;
            this.MiTablaValor = pTablaValor;
            this.MiIdRefTablaValor = TGEGeneralesF.CamposObtenerIdRefTablaValor(pTablaValor);

            MiIdRefValor = pIdRefValor;
            //this.MisCampos = TGEGeneralesF.CamposObtenerListaFiltro(pTablaValor, pTablaParametro);
            List<TGECampos> campos = TGEGeneralesF.CamposObtenerListaFiltro(pTablaValor, pTablaParametro);

            if (this.MisDatos.ContainsKey(pTablaParametro.GetType().Name))
                this.MisDatos[pTablaParametro.GetType().Name] = campos;
            else
                this.MisDatos.Add(pTablaParametro.GetType().Name, campos);
            //this.IniciarControl(this.MisCampos, pGestion);
            regenerarControles = cantidad != this.MisDatos.Values.SelectMany(x => x).ToList().Count;


            this.IniciarControl(this.MisDatos.Values.SelectMany(x => x).ToList(), pGestion, regenerarControles);
        }

        public void IniciarControl(Objeto pTablaValor, Objeto pTablaParametro, Gestion pGestion, int? pIdRefValor, List<TGECampos> camposMemoria)
        {
            int cantidad = this.MisDatos.Values.SelectMany(x => x).ToList().Count;
            this.MiTablaValor = pTablaValor;
            this.MiIdRefTablaValor = TGEGeneralesF.CamposObtenerIdRefTablaValor(pTablaValor);
            MiIdRefValor = pIdRefValor;
            //this.MisCampos = TGEGeneralesF.CamposObtenerListaFiltro(pTablaValor, pTablaParametro);
            List<TGECampos> camposControl = this.MisDatos.Values.SelectMany(x => x).ToList();

            List<TGECampos> campos = TGEGeneralesF.CamposObtenerListaFiltro(pTablaValor, pTablaParametro);
            if (this.MisDatos.ContainsKey(pTablaParametro.GetType().Name))
                this.MisDatos[pTablaParametro.GetType().Name] = campos;
            else
                this.MisDatos.Add(pTablaParametro.GetType().Name, campos);
            //this.IniciarControl(this.MisCampos, pGestion);

            List<TGECampos> camposBD = this.MisDatos.Values.SelectMany(x => x).ToList();
            foreach (TGECampos c in camposBD)
            {
                if (camposMemoria.Exists(x => x.IdCampo == c.IdCampo))
                    AyudaProgramacion.MatchObjectProperties(camposMemoria.FirstOrDefault(x => x.IdCampo == c.IdCampo).CampoValor, c.CampoValor);
            }
            bool regenerarControles = (cantidad != this.MisDatos.Values.SelectMany(x => x).ToList().Count);


            this.IniciarControl(camposBD, pGestion, regenerarControles);
        }

        public void IniciarControl(Objeto pTablaValor, TGEEstados pTablaParametro, Gestion pGestion)
        {
            this.MiTablaValor = pTablaValor;
            this.MiIdRefTablaValor = TGEGeneralesF.CamposObtenerIdRefTablaValor(pTablaValor);
            //this.MisCampos = TGEGeneralesF.CamposObtenerListaFiltro(pTablaValor, pTablaParametro);
            List<TGECampos> campos = TGEGeneralesF.CamposObtenerListaFiltro(pTablaValor, pTablaParametro);
            if (this.MisDatos.ContainsKey(pTablaParametro.GetType().Name))
                this.MisDatos[pTablaParametro.GetType().Name] = campos;
            else
                this.MisDatos.Add(pTablaParametro.GetType().Name, campos);
            //this.IniciarControl(this.MisCampos, pGestion);
            this.IniciarControl(this.MisDatos.Values.SelectMany(x => x).ToList(), pGestion);
        }

        public void IniciarControl(List<TGECampos> pCampos, Gestion pGestion)
        {
            this.IniciarControl(pCampos, pGestion, false);
        }

        public void IniciarControl(List<TGECampos> pCampos, Gestion pGestion, bool regenerarControles)
        {
            this.MisCampos = pCampos;
            this.HabilitarControles = false;
            this.GestionControl = pGestion;
            if (pGestion == Gestion.Agregar || pGestion == Gestion.Modificar || pGestion == Gestion.Renovar || pGestion == Gestion.Listar)
                this.HabilitarControles = true;

            if (regenerarControles)
                this.pnlCamposDinamicos.Controls.Clear();

            this.ArmarTablaParametros(this.MisCampos, this.HabilitarControles);
            this.MiArmarParametros = this.MisCampos.Count > 0;
            this.MostrarControl = this.MisCampos.Count > 0;
            this.pnlCamposDinamicos.Visible = this.MisCampos.Count > 0;

            this.upCamposValores.Update();
        }

        /// <summary>
        /// Obtiene la lista de Campos y sus valores actualizada para ser guardada
        /// en la base de datos.
        /// </summary>
        /// <returns></returns>
        public List<TGECampos> ObtenerLista()
        {
            //this.ObtenerValoresParametros(this.MisCampos);
            return this.MisCampos;
        }

        public XmlDocument ObtenerListaCamposValores()
        {
            List<TGECampos> lista = this.ObtenerLista();

            XmlDocument loteCampos = new XmlDocument();
            XmlNode nodos = loteCampos.CreateElement("CamposValores");
            loteCampos.AppendChild(nodos);

            XmlNode itemNodo;
            XmlAttribute itemAttribute;
            foreach (TGECampos item in lista)
            {
                itemNodo = loteCampos.CreateElement("CampoValor");

                itemAttribute = loteCampos.CreateAttribute("IdCampo");
                itemAttribute.Value = item.IdCampo.ToString();
                itemNodo.Attributes.Append(itemAttribute);
                itemAttribute = loteCampos.CreateAttribute("Nombre");
                itemAttribute.Value = item.Nombre;
                itemNodo.Attributes.Append(itemAttribute);
                itemAttribute = loteCampos.CreateAttribute("Valor");
                itemAttribute.Value = item.CampoValor.Valor;
                itemNodo.Attributes.Append(itemAttribute);

                nodos.AppendChild(itemNodo);
            }

            return loteCampos;
        }

        public bool BorrarControlesParametros(Objeto pTablaParametro)
        {
            string tabla = pTablaParametro.GetType().Name;
            if (this.MisDatos.ContainsKey(tabla))
                this.MisDatos.Remove(tabla);
            
            this.MisCampos = this.MisCampos.Where(x=> x.Tabla != tabla).ToList();
            this.pnlCamposDinamicos.Controls.Clear();
            return true;
        }

        public bool BorrarControlesParametros()
        {
            this.MisDatos.Clear();
            this.MisCampos = new List<TGECampos>();
            this.pnlCamposDinamicos.Controls.Clear();
            return true;
        }

        //protected void gvArchivos_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (!(e.CommandName==Gestion.Modificar.ToString()))
        //        return;

        //    int index = Convert.ToInt32(e.CommandArgument);
        //    this.MiIndiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
        //    this.ctrCamposValoresPopUp.IniciarControl(this.MisCampos[this.MiIndiceColeccion]);
        //}

        #region "ArmarControlesParametros"

        private void ObtenerValoresParametros(List<TGECampos> pCampos)
        {
            CultureInfo culture = CultureInfo.CurrentUICulture;
            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("$" + this.ID + "$")).ToList();
            string k, l;
            foreach (TGECampos parametro in pCampos)
            {
                switch (parametro.CampoTipo.IdCampoTipo)
                {
                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        k = keys.Find(z => z.EndsWith(preid + "select2HdfText" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.ListaValor = this.Request.Form[k];
                        break;
                    case (int)EnumCamposTipos.ComboBoxSP:
                        k = keys.Find(x => x.EndsWith(preid + "ListComboBox" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        break;
                    case (int)EnumCamposTipos.CheckBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = string.IsNullOrEmpty(this.Request.Form[k]) ? false.ToString() : this.Request.Form[k] == "on" ? true.ToString() : false.ToString();
                        else
                            parametro.CampoValor.Valor = false.ToString();
                        break;
                    case (int)EnumCamposTipos.DropDownList:
                    case (int)EnumCamposTipos.DropDownListSP:
                    case (int)EnumCamposTipos.DropDownListQuery:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            parametro.CampoValor.Valor = this.Request.Form[k];
                            if (parametro.CampoValor.Valor.Trim() == string.Empty)
                                parametro.CampoValor.ListaValor = string.Empty;
                            else
                            {
                                l = keys.Find(z => z.EndsWith(preid + "HdfText" + parametro.IdCampo.ToString()));
                                parametro.CampoValor.ListaValor = string.IsNullOrEmpty(l) ? string.Empty : this.Request.Form[l];
                            }
                        }
                        break;
                    case (int)EnumCamposTipos.DropDownListMultiple:
                        //k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            parametro.CampoValor.Valor = this.Request.Form[k];
                        }
                        else
                        {
                            parametro.CampoValor.Valor = "";
                        }
                        break;
                    case (int)EnumCamposTipos.TextBox:
                    case (int)EnumCamposTipos.DateTime:
                    case (int)EnumCamposTipos.IntegerTextBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = this.Request.Form[k];

                        break;
                    case (int)EnumCamposTipos.CurrencyTextBox:
                    case (int)EnumCamposTipos.NumericTextBox:
                    case (int)EnumCamposTipos.PercentTextBox:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                            parametro.CampoValor.Valor = decimal.Parse(this.Request.Form[k].Replace(culture.NumberFormat.CurrencySymbol, string.Empty)).ToString();
                        break;
                    case (int)EnumCamposTipos.GrillaDinamicaAB:
                        k = keys.Find(x => x.EndsWith(preid + "select2HdfValue" + parametro.IdCampo.ToString()));
                        if (!string.IsNullOrEmpty(k))
                        {
                            string value = this.Request.Form[k];
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                List<string> values = parametro.CampoValor.Valor.Split(';').ToList();
                                if (!values.Exists(x => x == value))
                                {
                                    if (parametro.CampoValor.Valor.Trim().Length > 0)
                                        parametro.CampoValor.Valor += ";" + value;
                                    else
                                        parametro.CampoValor.Valor = value;
                                }
                            }
                        }
                        //k = keys.Find(z => z.EndsWith(preid + "select2HdfText" + parametro.IdCampo.ToString()));
                        //if (!string.IsNullOrEmpty(k))
                        //    parametro.CampoValor.ListaValor = this.Request.Form[k];
                        break;
                    case (int)EnumCamposTipos.Nicho:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$hdfIdNicho"));
                        if (!string.IsNullOrEmpty(k))
                        {
                            string value = this.Request.Form[k];
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                //List<string> values = parametro.CampoValor.Valor.Split(';').ToList();
                                //if (!values.Exists(x => x == value))
                                //{
                                //    if (parametro.CampoValor.Valor.Trim().Length > 0)
                                //        parametro.CampoValor.Valor += ";" + value;
                                //    else

                                //}
                                parametro.CampoValor.Valor = value;
                            }
                        }
                        break;
                    case (int)EnumCamposTipos.Turismo:
                        k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$hdfIdTurismo"));
                        Control ctrTur = BuscarControlRecursivo(this, preid + parametro.IdCampo.ToString());
                        if (!string.IsNullOrEmpty(k))
                        {
                            string value = this.Request.Form[k];
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                string xml = Encriptar.DesencriptarTexto(value);
                                XmlDocument loteCampos = new XmlDocument();
                                XmlAttribute itemAttribute;
                                loteCampos.LoadXml(xml);
                                XmlNode nodos = loteCampos.SelectSingleNode("TurismoDetalles");
                                if (nodos != null)
                                {
                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtFechaSalida"));
                                    if (nodos.Attributes["FechaSalida"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("FechaSalida");
                                        itemAttribute.Value = this.Request.Form[k];
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["FechaSalida"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["FechaSalida"].Value = this.Request.Form[k];
                                    }

                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtFechaRegreso"));
                                    if (nodos.Attributes["FechaRegreso"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("FechaRegreso");
                                        itemAttribute.Value = this.Request.Form[k];
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["FechaRegreso"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["FechaRegreso"].Value = this.Request.Form[k];
                                    }

                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtDetalle"));
                                    if (nodos.Attributes["Detalle"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("Detalle");
                                        itemAttribute.Value = this.Request.Form[k];
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["Detalle"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["Detalle"].Value = this.Request.Form[k];
                                    }

                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtImpuestoPais"));
                                    if (nodos.Attributes["ImpuestoPais"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("ImpuestoPais");
                                        itemAttribute.Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["ImpuestoPais"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["ImpuestoPais"].Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                    }

                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtPercepcionRG4815"));
                                    if (nodos.Attributes["PercepcionRG4815"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("PercepcionRG4815");
                                        itemAttribute.Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["PercepcionRG4815"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["PercepcionRG4815"].Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                    }

                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtPercepcionRG3819"));
                                    if (nodos.Attributes["PercepcionRG3819"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("PercepcionRG3819");
                                        itemAttribute.Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["PercepcionRG3819"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["PercepcionRG3819"].Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                    }

                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtPercepcionRG5272"));
                                    if (nodos.Attributes["PercepcionRG5272"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("PercepcionRG5272");
                                        itemAttribute.Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["PercepcionRG5272"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["PercepcionRG5272"].Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                    }
                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtImporteReintegrar"));
                                    if (nodos.Attributes["ImporteReintegrar"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("ImporteReintegrar");
                                        itemAttribute.Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["ImporteReintegrar"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["ImporteReintegrar"].Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                    }

                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$ddlCuentasAhorros"));
                                    if (nodos.Attributes["IdCuenta"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("IdCuenta");
                                        itemAttribute.Value = this.Request.Form[k] == string.Empty ? "" : this.Request.Form[k];
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["IdCuenta"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["IdCuenta"].Value = this.Request.Form[k] == string.Empty ? "" : this.Request.Form[k];
                                    }

                                    k = keys.Find(x => x.EndsWith(preid + parametro.IdCampo.ToString() + "$txtCostoCancelar"));
                                    if (nodos.Attributes["CostoCancelar"] == null && !string.IsNullOrEmpty(k))
                                    {
                                        itemAttribute = loteCampos.CreateAttribute("CostoCancelar");
                                        itemAttribute.Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                        nodos.Attributes.Append(itemAttribute);
                                    }
                                    else if (nodos.Attributes["CostoCancelar"] != null && !string.IsNullOrEmpty(k))
                                    {
                                        nodos.Attributes["CostoCancelar"].Value = this.Request.Form[k] == string.Empty ? "0.00" : decimal.Parse(this.Request.Form[k], NumberStyles.Currency).ToString("N2").Replace(".", "").Replace(",", ".");
                                    }

                                    parametro.CampoValor.Valor = loteCampos.InnerXml;
                                }
                            }
                        }
                        break;

                    default:
                        break;
                }
                parametro.CampoValor.IdRefTablaValor = this.MiIdRefTablaValor;
                parametro.CampoValor.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(parametro.CampoValor, this.GestionControl);

                if (parametro.CampoValor.IdCampoValor == 0 && parametro.CampoValor.Valor.Trim() == string.Empty)
                {
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.SinCambio;
                }
                else if (parametro.CampoValor.IdCampoValor == 0)
                {
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.Agregado;
                    //parametro.CampoValor.IdAfiliado = this.MiIdAfiliado;
                    parametro.CampoValor.IdRefTablaValor = this.MiIdRefTablaValor;
                }
                else
                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.Modificado;

            }
            #region no va mas
            //Control control;
            //bool proximoCampo = false;
            //string idControl;
            //foreach (TGECampos parametro in pCampos)
            //{
            //    proximoCampo = false;
            //    foreach (Control panel in this.pnlCamposDinamicos.Controls)
            //    {
            //        if (panel is PlaceHolder)
            //        {
            //            switch (parametro.CampoTipo.IdCampoTipo)
            //            {
            //                //case (int)EnumCamposTipos.DropDownListSPAutoComplete:

            //                //    break;
            //                case (int)EnumCamposTipos.ComboBoxSP:
            //                    idControl = "ListComboBox" + parametro.IdCampo.ToString();
            //                    break;
            //                case (int)EnumCamposTipos.CheckBox:
            //                    idControl = string.Concat( parametro.IdCampo.ToString(), "|", parametro.Nombre);
            //                    break;
            //                default:
            //                    idControl = parametro.IdCampo.ToString();
            //                    break;
            //            }
            //            //idControl = parametro.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.ComboBoxSP ? "ListComboBox" + parametro.IdCampo.ToString() : parametro.IdCampo.ToString();
            //            control = panel.FindControl(idControl);
            //            if (control != null)
            //            {
            //                switch (parametro.CampoTipo.IdCampoTipo)
            //                {
            //                    case (int)EnumCamposTipos.DropDownList:
            //                    case (int)EnumCamposTipos.DropDownListSP:
            //                    case (int)EnumCamposTipos.DropDownListQuery:
            //                        parametro.CampoValor.Valor = ((DropDownList)control).SelectedValue;
            //                        parametro.CampoValor.ListaValor = ((DropDownList)control).SelectedItem.Text;
            //                        proximoCampo = true;
            //                        break;
            //                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
            //                    case (int)EnumCamposTipos.ComboBoxSP:
            //                        proximoCampo = true;
            //                        break;
            //                    case (int)EnumCamposTipos.TextBox:
            //                    case (int)EnumCamposTipos.DateTime:
            //                        parametro.CampoValor.Valor = ((TextBox)control).Text;
            //                        proximoCampo = true;
            //                        break;
            //                    case (int)EnumCamposTipos.NumericTextBox:
            //                        parametro.CampoValor.Valor = ((SKP.ASP.Controls.NumericTextBox)control).Text;
            //                        proximoCampo = true;
            //                        break;
            //                    case (int)EnumCamposTipos.CurrencyTextBox:
            //                        parametro.CampoValor.Valor = ((CurrencyTextBox)control).Decimal.ToString();
            //                        proximoCampo = true;
            //                        break;
            //                    case (int)EnumCamposTipos.CheckBox:
            //                        parametro.CampoValor.Valor = ((CheckBox)control).Checked.ToString();
            //                        proximoCampo = true;
            //                        break;
            //                    default:
            //                        break;
            //                }

            //                //parametro.CampoValor.IdAfiliado = this.MiIdAfiliado;
            //                parametro.CampoValor.IdRefTablaValor = this.MiIdRefTablaValor;
            //                parametro.CampoValor.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(parametro.CampoValor, this.GestionControl);

            //                if (parametro.CampoValor.IdCampoValor == 0 && parametro.CampoValor.Valor.Trim() == string.Empty)
            //                {
            //                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.SinCambio;
            //                }
            //                else if (parametro.CampoValor.IdCampoValor == 0)
            //                {
            //                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.Agregado;
            //                    //parametro.CampoValor.IdAfiliado = this.MiIdAfiliado;
            //                    parametro.CampoValor.IdRefTablaValor = this.MiIdRefTablaValor;
            //                }
            //                else
            //                    parametro.CampoValor.EstadoColeccion = EstadoColecciones.Modificado;
            //            }
            //        }
            //        if (proximoCampo)
            //            break;
            //    }
            //}
            #endregion
        }

        private void ArmarTablaParametros(List<TGECampos> pCampos, bool pHabilitar)
        {
            //this.tablaParametros = new Table();
            //this.pnlCamposDinamicos.Controls.Clear();
            List<Control> controlesEliminar = new List<Control>();
            foreach (Control ctr in this.pnlCamposDinamicos.Controls)
            {
                if (ctr is PlaceHolder)
                {
                    if (!pCampos.Exists(x => "panel" + x.IdCampo.ToString() == ctr.ID))
                        controlesEliminar.Add(ctr);
                }
            }
            foreach (Control ctr in controlesEliminar)
                this.pnlCamposDinamicos.Controls.Remove(ctr);
            pCampos = pCampos.OrderBy(x => x.Orden).ToList();
            //List<TGECamposValores> listaOpciones;
            List<TGEListasValoresDetalles> listaValoresDetalles;
            Control ctrExiste;

            //int countControl = 0;
            Panel pnlRow = new Panel();
            pnlRow.CssClass = "form-group row";
            //Panel pnlCol;
            foreach (TGECampos parametro in pCampos)
            {
                //listaOpciones = new List<TGECamposValores>();
                //TGECamposValores param;
                ctrExiste = this.pnlCamposDinamicos.FindControl("panel" + parametro.IdCampo.ToString());
                if (ctrExiste != null)
                {
                    foreach (Control c in ctrExiste.Controls)
                        if (c is WebControl)
                        {
                            //((WebControl)c).Enabled = pHabilitar;
                            if (c is PlaceHolder)
                                foreach (Control d in c.Controls)
                                    if (d is WebControl)
                                        ((WebControl)d).Enabled = pHabilitar;
                        }
                    continue;
                }
                if (parametro.SaltoLinea)
                {
                    Panel pnlWrap = new Panel();
                    pnlWrap.CssClass = "w-100";
                    pnlRow.Controls.Add(pnlWrap);
                }
                listaValoresDetalles = new List<TGEListasValoresDetalles>();
                switch (parametro.CampoTipo.IdCampoTipo)
                {
                    case (int)EnumCamposTipos.DropDownList:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresObtenerListaDetalle(parametro.ListaValor);
                        pnlRow.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListSP:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(parametro, this.MiTablaValor);
                        pnlRow.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
                        pnlRow.Controls.Add(AddListBoxAutocompleteRow(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListQuery:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerConsultaDinamica(parametro, this.MiTablaValor);
                        pnlRow.Controls.Add(AddListBoxRow(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DropDownListMultiple:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(parametro, this.MiTablaValor);
                        pnlRow.Controls.Add(AddListBoxRowMultiple(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.ComboBoxSP:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(parametro, this.MiTablaValor);
                        pnlRow.Controls.Add(AddListComboBox(parametro, listaValoresDetalles, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.TextBox:
                        PlaceHolder textboxEnPlaceHolder = AddTextBoxRow(parametro, pHabilitar);
                        pnlRow.Controls.Add(textboxEnPlaceHolder);
                        pnlRow.Controls.Add(AddLabelRow(parametro, textboxEnPlaceHolder));
                        break;
                    case (int)EnumCamposTipos.IntegerTextBox:
                        pnlRow.Controls.Add(AddNumericInputBoxRow(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.CurrencyTextBox:
                    case (int)EnumCamposTipos.NumericTextBox:
                    case (int)EnumCamposTipos.PercentTextBox:

                    case (int)EnumCamposTipos.EmailTextBox:
                    case (int)EnumCamposTipos.PasswordTextBox:
                        pnlRow.Controls.Add(AddCurrencyInputBoxRow(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.CheckBox:
                        pnlRow.Controls.Add(AddCheckBoxRow(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.DateTime:
                        pnlRow.Controls.Add(AddDateTimeRow(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.GrillaDinamicaAB:
                        if (GestionControl != Gestion.Listar)
                            pnlRow.Controls.Add(AddGrillaDinamicaAB(parametro, pHabilitar));
                        break;
                    case (int)EnumCamposTipos.Nicho:
                        pnlRow.Controls.Add(AddControlNichos(parametro, pHabilitar, GestionControl));
                        break;
                    case (int)EnumCamposTipos.Turismo:
                        pnlRow.Controls.Add(AddControlTurismo(parametro, pHabilitar, GestionControl));
                        break;
                    default:
                        break;
                }
            }
            pnlCamposDinamicos.Controls.Add(pnlRow);
        }

        //private void CargarValoresParametros(RepParametros parametro, string pNombreControl)
        //{
        //    List<TGECamposValores> listaOpciones = new List<TGECamposValores>();
        //    TGECamposValores param;

        //    if (parametro.CampoTipo.IdCampoTipo == (int)EnumCamposTipos.DropDownList
        //        && !string.IsNullOrEmpty(parametro.StoredProcedure))
        //    {
        //        param = new TGECamposValores();
        //        param.Valor = parametro.StoredProcedure;
        //        param.IdCampo = parametro.IdCampo;
        //        param.IdAfiliado = this.MiIdAfiliado;

        //        //Obtengo los valores para llenar las opciones del parametro
        //        listaOpciones = TGEGeneralesF.CamposObtenerListaValoresGenerica(param);
        //    }
        //    switch (parametro.TipoParametro.IdTipoParametro)
        //    {
        //        case (int)EnumRepTipoParametros.Int:
        //            Control control = this.BuscarControlRecursivo(this.tablaParametros, "lst" + pNombreControl);
        //            if (control != null)
        //            {
        //                DropDownList ddl = (DropDownList)control;
        //                ddl.Items.Clear();
        //                ddl.DataSource = dsParametros;
        //                ddl.DataValueField = pNombreControl;
        //                ddl.DataTextField = "Descripcion";
        //                ddl.DataBind();
        //                ListItem item = new ListItem("Seleccione una opción", "0");
        //                item.Selected = true;
        //                ddl.Items.Add(item);
        //            }
        //            //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));

        //            break;
        //        case (int)EnumRepTipoParametros.DateTime:
        //            //tablaParametros.Rows.Add(AddDateBoxRow(parametro.NombreParametro, parametro.Parametro));

        //            break;
        //        case (int)EnumRepTipoParametros.TextBox:
        //            break;
        //        case (int)EnumRepTipoParametros.IntNumericInput:
        //            break;
        //        case (int)EnumRepTipoParametros.DateTimeRange:
        //            //tablaParametros.Rows.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
        //            break;
        //        case (int)EnumRepTipoParametros.YearMonthCombo:
        //            break;
        //        default:
        //            break;
        //    }
        //}

        private PlaceHolder AddListBoxRow(TGECampos pCampo, List<TGECamposValores> dsDatos, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;
            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.DataValueField = "IdCampoValor";
            ddlListaOpciones.DataTextField = "Valor";
            ddlListaOpciones.DataSource = dsDatos;
            ddlListaOpciones.DataBind();
            ddlListaOpciones.CssClass += string.Concat(" ", pCampo.Clase);
            ListItem item = new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), string.Empty);
            ddlListaOpciones.Items.Add(item);
            ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
            item = ddlListaOpciones.Items.FindByValue(pCampo.CampoValor.Valor);
            if (item != null)
                ddlListaOpciones.SelectedValue = pCampo.CampoValor.Valor;
            else
                ddlListaOpciones.SelectedValue = string.Empty;

            ddlListaOpciones.Enabled = pHabilitar;

            HiddenField hdfText = new HiddenField();
            hdfText.ID = preid + "HdfText" + pCampo.IdCampo.ToString();
            hdfText.Value = pCampo.CampoValor.ListaValor;

            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfText);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "Aceptar";
                pnlRow.Controls.Add(validador);
            }
            pnlRow.Controls.Add(pnlRow);

            if (pHabilitar)
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControl|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);
                //Select Change
                script.AppendLine("control.change(function() { ");
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendFormat("hdfText.val( $('select[name$={0}] option:selected').text() );", ddlListaOpciones.ID);
                script.AppendLine("});");
                //end Select Change

                script.AppendLine("};");
                //end function
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControl|CTRLID|);");
                script.AppendLine("InitControl|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);

            return panel;
        }

        private PlaceHolder AddListBoxRowMultiple(TGECampos pCampo, List<TGEListasValoresDetalles> dsDatos, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;

            ListBox ddlListaOpciones = new ListBox();
            ddlListaOpciones.SelectionMode = ListSelectionMode.Multiple;
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.DataValueField = "IdListaValorDetalle";
            ddlListaOpciones.DataTextField = "Descripcion";
            ddlListaOpciones.DataSource = dsDatos;
            ddlListaOpciones.DataBind();
            ddlListaOpciones.CssClass += string.Concat(" ", pCampo.Clase);
            ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
            ddlListaOpciones.Enabled = pHabilitar;

            List<string> selected = pCampo.CampoValor.Valor.Split(',').ToList();
            if (pCampo.CampoValor.Valor.Length > 0)
            {
                foreach (string row in selected)
                {
                    ddlListaOpciones.Items.FindByValue(row).Selected = true;
                }
            }

            //if (this.MisCampos.Exists(x => x.IdCampoDependiente.HasValue && x.IdCampoDependiente == pCampo.IdCampo))
            //{
            //    ddlListaOpciones.AutoPostBack = true;
            //    ddlListaOpciones.SelectedIndexChanged += DdlListaOpciones_SelectedIndexChanged;
            //}

            //HiddenField hdfText = new HiddenField();
            //hdfText.ID = preid + "HdfText" + pCampo.IdCampo.ToString();
            //hdfText.Value = pCampo.CampoValor.ListaValor;

            HiddenField hdfValue = new HiddenField();
            hdfValue.ID = preid + "select2HdfValue" + pCampo.IdCampo.ToString();
            hdfValue.Value = pCampo.CampoValor.Valor;

            pnlRow.Controls.Add(ddlListaOpciones);
            //pnlRow.Controls.Add(hdfText);
            pnlRow.Controls.Add(hdfValue);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = MiValidationGroup;
                pnlRow.Controls.Add(validador);
            }
            panel.Controls.Add(pnlRow);
            if (pHabilitar)
            {
                StringBuilder script = new StringBuilder();
                //script.AppendLine(" $(document).ready(function () {");
                script.AppendLine(" function pageLoad() {");
                //start function
                script.AppendLine(" function InitControl|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);

                //select2 start
                script.AppendLine("control.select2({");
                script.AppendLine("tags: false,");
                script.AppendFormat("placeholder: '{0}',", pCampo.Titulo);
                script.AppendLine("language: 'es',");
                script.AppendLine("allowClear: true,");
                script.AppendLine("multiple: 'true',");
                script.AppendLine("});");
                //end select2
                ;

                script.AppendLine("control.on('select2:select select2:unselect', function(e) { ");
                //script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendLine("hdfValue.val($(this).val());");
                script.AppendLine("});");

                script.AppendLine("};");
                //end function
                //script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControl|CTRLID|);");

                script.AppendLine("InitControl|CTRLID|();");

                //script.AppendLine("});");
                script.AppendLine("}");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);
            return panel;
        }

        private PlaceHolder AddListBoxRow(TGECampos pCampo, List<TGEListasValoresDetalles> dsDatos, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;

            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.CssClass = "form-control select2";

            ddlListaOpciones.DataValueField = "IdListaValorDetalle";
            ddlListaOpciones.DataTextField = "Descripcion";
            ddlListaOpciones.DataSource = dsDatos;
            ddlListaOpciones.DataBind();
            ddlListaOpciones.CssClass += string.Concat(" ", pCampo.Clase);
            ListItem item = new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), string.Empty);
            ddlListaOpciones.Items.Add(item);
            ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
            item = ddlListaOpciones.Items.FindByValue(pCampo.CampoValor.Valor);
            if (item != null)
                ddlListaOpciones.SelectedValue = pCampo.CampoValor.Valor;
            else
                ddlListaOpciones.SelectedValue = string.Empty;
            ddlListaOpciones.Enabled = pHabilitar;

            if (this.MisCampos.Exists(x => x.IdCampoDependiente.HasValue && x.IdCampoDependiente == pCampo.IdCampo))
            {
                ddlListaOpciones.AutoPostBack = true;
                ddlListaOpciones.SelectedIndexChanged += DdlListaOpciones_SelectedIndexChanged;
            }

            HiddenField hdfText = new HiddenField();
            hdfText.ID = preid + "HdfText" + pCampo.IdCampo.ToString();
            hdfText.Value = pCampo.CampoValor.ListaValor;

            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfText);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = MiValidationGroup;
                pnlRow.Controls.Add(validador);
            }
            panel.Controls.Add(pnlRow);
            if (pHabilitar)
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControl|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);
                //Select Change
                script.AppendLine("control.change(function() { ");
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendFormat("hdfText.val( $('select[name$={0}] option:selected').text() );", ddlListaOpciones.ID);

                if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
                {
                    script.AppendLine(pCampo.EventoJavaScript);
                }

                script.AppendLine("});");
                //end Select Change

                script.AppendLine("};");
                //end function
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControl|CTRLID|);");
                script.AppendLine("InitControl|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);
            return panel;
        }

        private void DdlListaOpciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList ddl = ((DropDownList)sender);
            //preid + pCampo.IdCampo.ToString();
            int idCampo = Convert.ToInt32(ddl.ID.Replace(preid, string.Empty));
            List<TGECampos> camposActualizar = this.MisCampos.FindAll(x => x.IdCampoDependiente == idCampo);
            List<TGEListasValoresDetalles> listaValoresDetalles;
            this.MiTablaValor.Filtro = ddl.SelectedValue;
            foreach (TGECampos campo in camposActualizar)
            {
                switch (campo.CampoTipo.IdCampoTipo)
                {
                    case (int)EnumCamposTipos.DropDownListSP:
                    case (int)EnumCamposTipos.DropDownListSPAutoComplete:
                    case (int)EnumCamposTipos.DropDownListQuery:
                    case (int)EnumCamposTipos.ComboBoxSP:
                        listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(campo, MiTablaValor);
                        Control ctrl = this.BuscarControlRecursivo(this, string.Concat(preid, campo.IdCampo.ToString()));
                        if (ctrl != null)
                        {
                            DropDownList ddlCtrl = (DropDownList)ctrl;
                            ddlCtrl.Items.Clear();
                            ddlCtrl.SelectedIndex = -1;
                            ddlCtrl.SelectedValue = null;
                            ddlCtrl.ClearSelection();
                            ddlCtrl.DataSource = listaValoresDetalles;
                            ddlCtrl.DataBind();
                            AyudaProgramacion.AgregarItemSeleccione(ddlCtrl, ObtenerMensajeSistema("SeleccioneOpcion"));
                        }
                        break;
                    case (int)EnumCamposTipos.Turismo:
                        Control ctrl1 = this.BuscarControlRecursivo(this, string.Concat(preid, campo.IdCampoDependiente.ToString()));

                        if (ctrl1 != null)
                        {
                            DropDownList ddlCtrl1 = (DropDownList)ctrl1;
                            if (!string.IsNullOrEmpty(ddlCtrl1.SelectedValue))
                            {
                                Turismo ctrTurismo = (Turismo)this.BuscarControlRecursivo(this, string.Concat(preid, campo.IdCampo.ToString()));
                                ctrTurismo.IniciarControl(this.MiTablaValor, campo, false, Gestion.Consultar, Convert.ToInt32(ddlCtrl1.SelectedValue));
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            this.MiTablaValor.Filtro = string.Empty;
        }
        private PlaceHolder AddListBoxAutocompleteRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;
            //panel.Controls.Add(lblParametro);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;
            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.Enabled = pHabilitar;
            ddlListaOpciones.CssClass += string.Concat(" ", pCampo.Clase);

            if (pCampo.CampoValor.Valor.Trim().Length > 0)
            {
                TGECamposValores parametro = new TGECamposValores();
                parametro.Valor = pCampo.CampoValor.Valor.Trim();
                parametro.IdRefTablaValor = 0;
                DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet(pCampo.StoredProcedure, parametro);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    pCampo.CampoValor.ListaValor = ds.Tables[0].Rows[0]["Descripcion"].ToString();

                ListItem item = new ListItem(pCampo.CampoValor.ListaValor, pCampo.CampoValor.Valor);
                ddlListaOpciones.Items.Add(item);
            }

            HiddenField hdfValue = new HiddenField();
            hdfValue.ID = preid + "select2HdfValue" + pCampo.IdCampo.ToString();
            hdfValue.Value = pCampo.CampoValor.Valor;
            HiddenField hdfText = new HiddenField();
            hdfText.ID = preid + "select2HdfText" + pCampo.IdCampo.ToString();
            hdfText.Value = pCampo.CampoValor.ListaValor;
            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfValue);
            pnlRow.Controls.Add(hdfText);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = MiValidationGroup;
                pnlRow.Controls.Add(validador);
            }


            if (pHabilitar)
            {
                HiddenField hdfJsValue = new HiddenField();
                hdfJsValue.ID = preid + "hdfJsTitulo" + pCampo.IdCampo.ToString();
                hdfJsValue.Value = pCampo.Titulo;
                pnlRow.Controls.Add(hdfJsValue);

                hdfJsValue = new HiddenField();
                hdfJsValue.ID = preid + "hdfJsStoredProcedure" + pCampo.IdCampo.ToString();
                hdfJsValue.Value = pCampo.StoredProcedure;
                pnlRow.Controls.Add(hdfJsValue);

                hdfJsValue = new HiddenField();
                hdfJsValue.ID = preid + "hdfJsMiIdRefTablaValor" + pCampo.IdCampo.ToString();
                hdfJsValue.Value = this.MiIdRefTablaValor.ToString();
                pnlRow.Controls.Add(hdfJsValue);

                hdfJsValue = new HiddenField();
                hdfJsValue.ID = preid + "hdfJsMiIdRefValor" + pCampo.IdCampo.ToString();
                hdfJsValue.Value = MiIdRefValor.HasValue ? MiIdRefValor.Value.ToString() : string.Empty;
                pnlRow.Controls.Add(hdfJsValue);

                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControlSelect2|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);
                script.AppendFormat("var Titulo = $('input[type=hidden][id$={0}]').val();", preid + "hdfJsTitulo" + pCampo.IdCampo.ToString());
                script.AppendFormat("var StoredProcedure = $('input[type=hidden][id$={0}]').val();", preid + "hdfJsStoredProcedure" + pCampo.IdCampo.ToString());
                script.AppendFormat("var MiIdRefTablaValor = $('input[type=hidden][id$={0}]').val();", preid + "hdfJsMiIdRefTablaValor" + pCampo.IdCampo.ToString());
                script.AppendFormat("var MiIdRefValor = $('input[type=hidden][id$={0}]').val();", preid + "hdfJsMiIdRefValor" + pCampo.IdCampo.ToString());

                //select2 start
                script.AppendLine("control.select2({");
                script.AppendLine("placeholder: Titulo,");
                script.AppendLine("minimumInputLength: 4,");
                script.AppendLine("theme: 'bootstrap4',");
                script.AppendLine("language: 'es',");
                script.AppendLine("allowClear: true,");
                script.AppendLine("ajax: {");
                script.AppendLine("type: 'POST',");
                script.AppendLine("contentType: 'application/json; charset=utf-8',");
                script.AppendLine("dataType: 'json',");
                script.AppendFormat("url: '{0}',", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica2"));
                script.AppendLine("delay: 250,");
                script.AppendLine("data: function (params) {");
                script.AppendLine("return JSON.stringify( {");
                script.AppendLine("value: control.val(), // search term");
                script.AppendLine("filtro: params.term, // search term");
                script.AppendLine("sp: StoredProcedure,");
                script.AppendLine("idRefTablaValor: MiIdRefTablaValor,");
                script.AppendLine("idRefValor: MiIdRefValor");
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
                //select2 ON select
                script.AppendLine("control.on('select2:select', function(e) { ");
                script.AppendLine("var newOption = new Option(e.params.data.text, e.params.data.id, false, true);");
                script.AppendFormat("$('select[id$={0}]').append(newOption).trigger('change');", ddlListaOpciones.ID);
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendLine("hdfValue.val(e.params.data.id);");
                script.AppendLine("hdfText.val(e.params.data.text);");
                if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
                {
                    script.AppendLine("ControlSelect2|CTRLID|On(e.params.data.id);");
                }
                script.AppendLine("});");
                //end select2 ON

                //select2 ON unselect
                script.AppendLine("control.on('select2:unselect', function(e) { ");
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendLine("hdfValue.val('');");
                script.AppendLine("hdfText.val('');");
                script.AppendLine("});");
                //end select2 ON unselect

                script.AppendLine("};");
                //end function
                //start scriptEvent
                script.AppendLine("function ControlSelect2|CTRLID|On(e) {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent

                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlSelect2|CTRLID|);");
                script.AppendLine("InitControlSelect2|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);
            return panel;
        }

        private PlaceHolder AddListComboBox(TGECampos pCampo, List<TGEListasValoresDetalles> dsDatos, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;
            //panel.Controls.Add(lblParametro);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;
            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.ID = preid + "ListComboBox" + pCampo.IdCampo.ToString();
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.Enabled = pHabilitar;

            ddlListaOpciones.DataValueField = "Descripcion";
            ddlListaOpciones.DataTextField = "Descripcion";
            ddlListaOpciones.DataSource = dsDatos;
            ddlListaOpciones.DataBind();
            ddlListaOpciones.CssClass += string.Concat(" ", pCampo.Clase);
            ListItem item = new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), string.Empty);
            ddlListaOpciones.Items.Add(item);
            if (pCampo.CampoValor.Valor.Trim().Length > 0)
            {
                item = ddlListaOpciones.Items.FindByValue(pCampo.CampoValor.Valor);
                if (item == null)
                    ddlListaOpciones.Items.Add(new ListItem(pCampo.CampoValor.Valor));
                ddlListaOpciones.SelectedValue = pCampo.CampoValor.Valor;
            }
            else
                ddlListaOpciones.SelectedValue = string.Empty;

            pnlRow.Controls.Add(ddlListaOpciones);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = ddlListaOpciones.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = "ValidadorControlesDinamicos";
                pnlRow.Controls.Add(validador);
            }
            //panel.Controls.Add(pnlRow);

            if (pHabilitar)
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControlComboBoxSelec2|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);

                //select2 start
                script.AppendLine("control.select2({");
                script.AppendLine("tags: true,");
                script.AppendFormat("placeholder: '{0}',", pCampo.Titulo);
                script.AppendLine("language: 'es',");
                script.AppendLine("allowClear: true,");

                script.AppendLine("});");
                //end select2

                script.AppendLine("};");
                //end function
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlComboBoxSelec2|CTRLID|);");
                script.AppendLine("InitControlComboBoxSelec2|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID, script.ToString(), true);
            }

            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);
            return panel;
        }

        //void ddlListaOpciones_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    DropDownList ddl = (DropDownList)sender;
        //    TGECampos campo = new TGECampos();
        //    campo.CampoValor.Valor = ddl.SelectedValue;
        //    campo.CampoValor.ListaValor = ddl.SelectedItem.Text;
        //}

        void AddHtmlLink(string rel, string href, string type)
        {
            ScriptManager manager = ScriptManager.GetCurrent(Page);
            if (manager.IsInAsyncPostBack)
            {
                Dictionary<string, object> values = new Dictionary<string, object>();
                values.Add("rel", rel);
                values.Add("href", href);
                values.Add("type", type);

                JavaScriptSerializer serializer = new JavaScriptSerializer();

                StringBuilder scriptBuilder = new StringBuilder();

                scriptBuilder.Append("registerLink(");
                serializer.Serialize(values, scriptBuilder);
                scriptBuilder.AppendLine(");");
                ScriptManager.RegisterClientScriptInclude(this, this.GetType(), "LinkRegister", ResolveUrl("~") + "Recursos/linkRegister.js");

                ScriptManager.RegisterClientScriptBlock(this, GetType(), "dynLink", scriptBuilder.ToString(), true);
            }
            else
            {
                HtmlLink link = new HtmlLink();
                link.Href = href;
                link.Attributes["rel"] = rel;
                link.Attributes["type"] = type;
                Page.Header.Controls.Add(link);
            }
        }

        private PlaceHolder AddLabelRow(TGECampos pCampo, PlaceHolder txtPlaceHolder)
        {
            PlaceHolder placeHolder = new PlaceHolder();
            if (!string.IsNullOrWhiteSpace(pCampo.StoredProcedureLeyenda))
            {
                placeHolder.ID = "panel" + "Leyenda" + pCampo.IdCampo.ToString();

                Panel divContainer = new Panel();
                divContainer.ID = string.Concat("dv", preid, "Leyenda", pCampo.IdCampo.ToString());
                divContainer.CssClass = MiccsContainer;
                placeHolder.Controls.Add(divContainer);

                Panel divRowLeyenda = new Panel();
                divRowLeyenda.CssClass = MicssRow;

                TextBox Text = (TextBox)BuscarControlRecursivo(txtPlaceHolder, preid + pCampo.IdCampo.ToString());
                Text.AutoPostBack = true;
                Text.TextChanged += Text_TextChanged;
                pCampo.Filtro = Text.Text;
                Label lblLeyenda = new Label();
                lblLeyenda.ID = preid + "Leyenda" + pCampo.IdCampo.ToString();
                lblLeyenda.CssClass = MicssLabelCol6;

                divRowLeyenda.Controls.Add(lblLeyenda);
                if (pCampo.Filtro.Length > 0)
                {
                    TGEListasValoresDetalles valorDetalle = TGEGeneralesF.ListasValoresDetallesObtenerLeyenda(pCampo, this.MiTablaValor);

                    if (!string.IsNullOrEmpty(valorDetalle.Descripcion))
                        lblLeyenda.Text = valorDetalle.Descripcion;
                    else
                        lblLeyenda.Text = string.Empty;
                }
                divContainer.Controls.Add(divRowLeyenda);
            }
            return placeHolder;
        }

        private PlaceHolder AddTextBoxRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder placeHolder = new PlaceHolder();
            placeHolder.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlInputCtrl = new Panel();
            pnlInputCtrl.CssClass = MicssCol;

            TextBox Text = new TextBox();
            Text.CssClass = "form-control";
            Text.CssClass += string.Concat(" ", pCampo.Clase);
            Text.ID = preid + pCampo.IdCampo.ToString();
            Text.Text = pCampo.CampoValor.Valor;
            Text.Enabled = pHabilitar;
            Text.MaxLength = pCampo.TamanioMaximo;
            pnlInputCtrl.Controls.Add(Text);

            if (!string.IsNullOrWhiteSpace(pCampo.StoredProcedure))
            {
                List<TGEListasValoresDetalles> listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(pCampo, this.MiTablaValor);
                if (listaValoresDetalles.Count > 0)
                    Text.Text = listaValoresDetalles[0].Descripcion;
            }

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = Text.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = MiValidationGroup;
                pnlInputCtrl.Controls.Add(validador);
            }

            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlInputCtrl);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            placeHolder.Controls.Add(divContainer);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start scriptEvent
                script.AppendLine("function ControlTextBox|CTRLID|() {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlTextBox|CTRLID|);");
                script.AppendLine("ControlTextBox|CTRLID|();");
                script.AppendLine("});");
                script.Replace("|CTRLID|", Text.ID);
                ScriptManager.RegisterClientScriptBlock(this.upCamposValores, this.upCamposValores.GetType(), "Script" + Text.ID, script.ToString(), true);
            }
            return placeHolder;
        }

        private void Text_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = ((TextBox)sender);
            int idCampo = Convert.ToInt32(txt.ID.Replace(preid, string.Empty));
            TGECampos campoActualizar = this.MisCampos.FirstOrDefault(x => x.IdCampo == idCampo);
            Label lbl = (Label)BuscarControlRecursivo(this, preid + "Leyenda" + idCampo);
            if (!string.IsNullOrWhiteSpace(campoActualizar.StoredProcedureLeyenda))
            {
                campoActualizar.Filtro = txt.Text;
                if (campoActualizar.Filtro.Length > 0)
                {
                    TGEListasValoresDetalles valorDetalle = TGEGeneralesF.ListasValoresDetallesObtenerLeyenda(campoActualizar, this.MiTablaValor);
                    if (!string.IsNullOrEmpty(valorDetalle.Descripcion))
                        lbl.Text = valorDetalle.Descripcion;
                }
                else
                    lbl.Text = string.Empty;
            }
        }

        private PlaceHolder AddNumericInputBoxRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;
            CurrencyTextBox Text = new CurrencyTextBox();
            Text.CssClass = "form-control";
            Text.CssClass += string.Concat(" ", pCampo.Clase);
            Text.Prefix = string.Empty;
            Text.DecimalSeparator = string.Empty;
            Text.ThousandsSeparator = string.Empty;
            Text.NumberOfDecimals = 0;
            Text.ID = preid + pCampo.IdCampo.ToString();
            Text.Text = pCampo.CampoValor.Valor;
            Text.Enabled = pHabilitar;
            Text.MaxLength = pCampo.TamanioMaximo;
            pnlRow.Controls.Add(Text);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = Text.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = MiValidationGroup;
                pnlRow.Controls.Add(validador);
            }
            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);

            return panel;
        }

        private PlaceHolder AddCurrencyInputBoxRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;
            //panel.Controls.Add(lblParametro);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;
            CurrencyTextBox Text = new CurrencyTextBox();
            Text.CssClass = "form-control";
            Text.CssClass += string.Concat(" ", pCampo.Clase);
            Text.ID = preid + pCampo.IdCampo.ToString();

            if (pCampo.StoredProcedure.Trim().Length > 0 && pCampo.CampoValor.Valor.Trim().Length == 0)//&& pCampo.Deshabilitado
            {
                List<TGEListasValoresDetalles> listaValoresDetalles = TGEGeneralesF.ListasValoresDetallesObtenerGenerica(pCampo, this.MiTablaValor);
                if (listaValoresDetalles.Count > 0)
                    pCampo.CampoValor.Valor = listaValoresDetalles[0].Descripcion;
            }

            switch (pCampo.CampoTipo.IdCampoTipo)
            {
                case (int)EnumCamposTipos.CurrencyTextBox:
                    Text.Text = pCampo.CampoValor.Valor == string.Empty ? (0).ToString("C2") : Convert.ToDecimal(pCampo.CampoValor.Valor).ToString("C2");
                    break;
                case (int)EnumCamposTipos.NumericTextBox:
                    Text.Prefix = string.Empty;
                    Text.Text = pCampo.CampoValor.Valor == string.Empty ? (0).ToString("N2") : Convert.ToDecimal(pCampo.CampoValor.Valor).ToString("N2");
                    break;
                case (int)EnumCamposTipos.PercentTextBox:
                    Text.Prefix = string.Empty;
                    Text.NumberOfDecimals = 4;
                    Text.Text = pCampo.CampoValor.Valor == string.Empty ? (0).ToString("N4") : Convert.ToDecimal(pCampo.CampoValor.Valor).ToString("N4");
                    break;
                default:
                    break;
            }

            Text.Enabled = pHabilitar;
            Text.MaxLength = pCampo.TamanioMaximo;
            pnlRow.Controls.Add(Text);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = Text.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = MiValidationGroup;
                pnlRow.Controls.Add(validador);
            }
            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start scriptEvent
                script.AppendLine("function ControlTextBox|CTRLID|() {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlTextBox|CTRLID|);");
                script.AppendLine("ControlTextBox|CTRLID|();");
                script.AppendLine("});");
                script.Replace("|CTRLID|", Text.ID);
                ScriptManager.RegisterClientScriptBlock(this.upCamposValores, this.upCamposValores.GetType(), "Script" + Text.ID, script.ToString(), true);
            }

            return panel;
        }

        private PlaceHolder AddCheckBoxRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;
            //panel.Controls.Add(lblParametro);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;
            CheckBox Text = new CheckBox();
            Text.CssClass = "form-control";
            Text.CssClass += string.Concat(" ", pCampo.Clase);
            Text.ID = preid + pCampo.IdCampo.ToString();
            Text.Checked = pCampo.CampoValor.Valor == string.Empty ? false : Convert.ToBoolean(pCampo.CampoValor.Valor);
            Text.Enabled = pHabilitar;

            //HiddenField hdfText = new HiddenField();
            //hdfText.ID = preid + "HdfText" + pCampo.IdCampo.ToString();
            //hdfText.Value = pCampo.CampoValor.Valor;

            pnlRow.Controls.Add(Text);
            //pnlRow.Controls.Add(hdfText);           

            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);

            return panel;
        }

        private PlaceHolder AddDateTimeRow(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Label lblParametro = new Label();
            lblParametro.CssClass = MicssLabel;
            lblParametro.Text = pCampo.Titulo;
            //panel.Controls.Add(lblParametro);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = MicssCol;
            TextBox Text = new TextBox();
            Text.CssClass = "form-control datepicker";
            Text.CssClass += string.Concat(" ", pCampo.Clase);
            Text.ID = preid + pCampo.IdCampo.ToString();
            Text.Text = pCampo.CampoValor.Valor;
            Text.Enabled = pHabilitar;
            pnlRow.Controls.Add(Text);

            if (pCampo.Requerido && pHabilitar)
            {
                RequiredFieldValidator validador = new RequiredFieldValidator();
                validador.ControlToValidate = Text.ID;
                validador.ErrorMessage = "*";
                validador.CssClass = "Validador";
                validador.ValidationGroup = MiValidationGroup;
                pnlRow.Controls.Add(validador);
            }
            Panel divRow = new Panel();
            divRow.CssClass = MicssRow;
            divRow.Controls.Add(lblParametro);
            divRow.Controls.Add(pnlRow);

            Panel divContainer = new Panel();
            divContainer.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divContainer.CssClass = MiccsContainer;
            divContainer.Controls.Add(divRow);
            panel.Controls.Add(divContainer);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start scriptEvent
                script.AppendLine("function ControlTextBox|CTRLID|() {");
                script.AppendFormat("{0}", pCampo.EventoJavaScript);
                script.AppendLine("}");
                //end scriptEvent
                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlTextBox|CTRLID|);");
                script.AppendLine("ControlTextBox|CTRLID|();");
                script.AppendLine("});");
                script.Replace("|CTRLID|", Text.ID);
                ScriptManager.RegisterClientScriptBlock(this.upCamposValores, this.upCamposValores.GetType(), "Script" + Text.ID, script.ToString(), true);
            }

            return panel;
        }

        private PlaceHolder AddGrillaDinamicaAB(TGECampos pCampo, bool pHabilitar)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();

            Panel divGroupCtrl = new Panel();
            divGroupCtrl.ID = string.Concat("dv", preid, pCampo.IdCampo.ToString());
            divGroupCtrl.CssClass = "col-12";

            Panel divGroupRow = new Panel();
            divGroupRow.CssClass = MicssRow;

            if (pHabilitar)
            {
                Label lblParametro = new Label();
                lblParametro.CssClass = MicssLabel;
                lblParametro.Text = pCampo.Titulo;
                //panel.Controls.Add(lblParametro);

                Panel pnlCol = new Panel();
                pnlCol.CssClass = MicssCol;
                DropDownList ddlListaOpciones = new DropDownList();
                ddlListaOpciones.ID = preid + pCampo.IdCampo.ToString();
                ddlListaOpciones.CssClass = "form-control select2";
                ddlListaOpciones.Enabled = pHabilitar;

                HiddenField hdfValue = new HiddenField();
                hdfValue.ID = preid + "select2HdfValue" + pCampo.IdCampo.ToString();
                //hdfValue.Value = pCampo.CampoValor.Valor;
                //HiddenField hdfText = new HiddenField();
                //hdfText.ID = preid + "select2HdfText" + pCampo.IdCampo.ToString();
                //hdfText.Value = pCampo.CampoValor.ListaValor;

                pnlCol.Controls.Add(ddlListaOpciones);
                pnlCol.Controls.Add(hdfValue);
                //pnlCol.Controls.Add(hdfText);

                Panel divRow = new Panel();
                divRow.CssClass = MicssRow;
                divRow.Controls.Add(lblParametro);
                divRow.Controls.Add(pnlCol);
                Panel divContainer = new Panel();
                divContainer.CssClass = MiccsContainer;
                divContainer.Controls.Add(divRow);
                //container.Controls.Add(divContainer);

                Button btn = new Button();
                btn.ID = preid + "btnGridAB" + pCampo.IdCampo.ToString();
                btn.CssClass = "botonesEvol";
                btn.Text = "Agregar";
                btn.ValidationGroup = "AgregarGrillaAB" + pCampo.IdCampo.ToString();
                btn.Click += BtnAgregarGrillaAB_Click;

                Panel pnlColBtn = new Panel();
                pnlColBtn.CssClass = MicssCol;
                pnlColBtn.Controls.Add(btn);
                pnlCol.Controls.Add(pnlColBtn);

                Panel divRowBtn = new Panel();
                divRowBtn.CssClass = MicssRow;
                divRowBtn.Controls.Add(pnlColBtn);
                Panel divContainerBtn = new Panel();
                divContainerBtn.CssClass = MiccsContainer;
                divContainerBtn.Controls.Add(divRowBtn);
                //container.Controls.Add(divContainerBtn);

                if (pCampo.Requerido)
                {
                    RequiredFieldValidator validador = new RequiredFieldValidator();
                    validador.ControlToValidate = ddlListaOpciones.ID;
                    validador.ErrorMessage = "*";
                    validador.CssClass = "Validador";
                    validador.ValidationGroup = "AgregarGrillaAB" + pCampo.IdCampo.ToString();
                    pnlCol.Controls.Add(validador);
                }

                #region Javascript
                StringBuilder script = new StringBuilder();
                script.AppendLine(" $(document).ready(function () {");
                //start function
                script.AppendLine(" function InitControlSelect2|CTRLID|() {");
                script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);

                //select2 start
                script.AppendLine("control.select2({");
                script.AppendFormat("placeholder: '{0}',", pCampo.Titulo);
                script.AppendLine("minimumInputLength: 4,");
                script.AppendLine("theme: 'bootstrap4',");
                script.AppendLine("language: 'es',");
                script.AppendLine("allowClear: true,");
                script.AppendLine("ajax: {");
                script.AppendLine("type: 'POST',");
                script.AppendLine("contentType: 'application/json; charset=utf-8',");
                script.AppendLine("dataType: 'json',");
                script.AppendFormat("url: '{0}',", ResolveClientUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
                script.AppendLine("delay: 250,");
                script.AppendLine("data: function (params) {");
                script.AppendLine("return JSON.stringify( {");
                script.AppendLine("value: control.val(), // search term");
                script.AppendLine("filtro: params.term, // search term");
                script.AppendFormat("sp: '{0}',", pCampo.StoredProcedure);
                script.AppendFormat("idRefTablaValor: '{0}',", this.MiIdRefTablaValor);
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
                //select2 ON select
                script.AppendLine("control.on('select2:select', function(e) { ");
                script.AppendLine("var newOption = new Option(e.params.data.text, e.params.data.id, false, true);");
                script.AppendFormat("$('select[id$={0}]').append(newOption).trigger('change');", ddlListaOpciones.ID);
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                //script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendLine("hdfValue.val(e.params.data.id);");
                //script.AppendLine("hdfText.val(e.params.data.text);");
                script.AppendLine("});");
                //end select2 ON

                //select2 ON unselect
                script.AppendLine("control.on('select2:unselect', function(e) { ");
                script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
                //script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
                script.AppendLine("hdfValue.val('');");
                //script.AppendLine("hdfText.val('');");
                script.AppendLine("});");
                //end select2 ON unselect

                script.AppendLine("};");
                //end function

                script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlSelect2|CTRLID|);");
                script.AppendLine("InitControlSelect2|CTRLID|();");
                script.AppendLine("});");

                script.Replace("|CTRLID|", ddlListaOpciones.ID);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);
                #endregion

                divGroupRow.Controls.Add(divContainer);
                divGroupRow.Controls.Add(divContainerBtn);
            }

            HiddenField Text = new HiddenField();
            Text.ID = preid + "txtgvValues" + pCampo.IdCampo.ToString();
            Text.Value = pCampo.CampoValor.Valor;
            divGroupRow.Controls.Add(Text);

            Panel divGrid = new Panel();
            divGrid.CssClass = "container-fluid table-responsive";
            GridView gv = new GridView();
            gv.ID = preid + "GridAB" + pCampo.IdCampo.ToString();
            gv.AutoGenerateColumns = false;
            gv.ShowHeader = true;
            gv.ShowHeaderWhenEmpty = true;
            gv.ShowFooter = true;
            gv.SkinID = "GrillaResponsive";
            DataTable dt = BaseDatos.ObtenerBaseDatos().ObtenerLista(pCampo.ConsultaDinamica, pCampo);
            BoundField bfield;
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
                    bfield.DataFormatString = "{0:MM/dd/yyyy}";

                gv.Columns.Add(bfield);
            }
            if (pHabilitar)
            {
                gv.RowDataBound += gvAB_RowDataBound;
                gv.RowCommand += gvAB_RowCommand;
                TemplateField tf = new TemplateField();
                tf.HeaderText = "Acciones";
                gv.Columns.Add(tf);
            }
            gv.DataSource = dt;
            gv.DataBind();
            gv.UseAccessibleHeader = true;
            gv.HeaderRow.TableSection = TableRowSection.TableHeader;
            divGrid.Controls.Add(gv);

            divGroupRow.Controls.Add(Text);
            divGroupRow.Controls.Add(divGrid);
            divGroupCtrl.Controls.Add(divGroupRow);
            panel.Controls.Add(divGroupCtrl);

            if (!string.IsNullOrWhiteSpace(pCampo.EventoJavaScript))
            {
                StringBuilder scriptGV = new StringBuilder();
                //scriptGV.AppendLine(" $(document).ready(function () {");
                //start scriptEvent
                scriptGV.AppendLine("function ControlGrillaAB|CTRLID|() {");
                scriptGV.AppendFormat("{0}", pCampo.EventoJavaScript);
                scriptGV.AppendLine("}");
                //end scriptEvent
                //scriptGV.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlGrillaAB|CTRLID|);");
                //scriptGV.AppendLine("ControlGrillaAB|CTRLID|();");
                //scriptGV.AppendLine("});");
                scriptGV.Replace("|CTRLID|", gv.ID);
                ScriptManager.RegisterClientScriptBlock(this.upCamposValores, this.upCamposValores.GetType(), "Script" + gv.ID, scriptGV.ToString(), true);
            }

            return panel;
        }

        private void gvAB_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                int ultimaColumna = e.Row.Cells.Count - 1;
                ImageButton ibtn = new ImageButton
                {
                    CommandName = "Borrar",
                    CommandArgument = dr[0].ToString(),
                    ImageUrl = "~/Imagenes/Baja.png"
                };
                //string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema("ConfirmarAccion"));
                //ibtn.Attributes.Add("OnClick", funcion);
                e.Row.Cells[ultimaColumna].Controls.Add(ibtn);
            }
        }

        private void gvAB_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Borrar")
            {
                GridView gv = (GridView)sender;
                string id = gv.ID;
                string val = e.CommandArgument.ToString();
                TGECampos campo = this.MisCampos.First(x => x.IdCampo.ToString() == id.Replace(preid + "GridAB", string.Empty));
                List<string> values = campo.CampoValor.Valor.Split(';').ToList();
                if (values.Exists(x => x == val))
                {
                    values.Remove(val);
                    campo.CampoValor.Valor = string.Join(";", values);
                    gv.DataSource = BaseDatos.ObtenerBaseDatos().ObtenerLista(campo.ConsultaDinamica, campo);
                    gv.DataBind();

                    Control CtrlPanel = BuscarControlRecursivo(((GridView)sender).Parent.Parent.Parent.Parent, "panel" + campo.IdCampo);
                    if (CtrlPanel != null)
                    {
                        Control ctrHdv = BuscarControlRecursivo(CtrlPanel, preid + "txtgvValues" + campo.IdCampo);
                        if (ctrHdv != null)
                            ((HiddenField)ctrHdv).Value = campo.CampoValor.Valor;
                    }
                }
                StringBuilder scriptGV = new StringBuilder();
                scriptGV.AppendLine("ControlGrillaAB|CTRLID|();");
                scriptGV.Replace("|CTRLID|", gv.ID);
                ScriptManager.RegisterClientScriptBlock(this.upCamposValores, this.upCamposValores.GetType(), "ScriptBorrar" + gv.ID, scriptGV.ToString(), true);
            }
        }

        private void BtnAgregarGrillaAB_Click(object sender, EventArgs e)
        {
            string idCampo = ((Button)sender).ID.Replace(preid + "btnGridAB", string.Empty);
            string idGV = ((Button)sender).ID.Replace("btnGridAB", "GridAB");
            Control CtrlPanel = BuscarControlRecursivo(((Button)sender).Parent.Parent.Parent.Parent.Parent.Parent, "panel" + idCampo);
            TGECampos campo = this.MisCampos.First(x => x.IdCampo.ToString() == idCampo);
            if (CtrlPanel != null)
            {
                PlaceHolder pnl = (PlaceHolder)CtrlPanel;
                Control ctrHdv = BuscarControlRecursivo(pnl, preid + "select2HdfValue" + idCampo);
                if (ctrHdv != null)
                {
                    ((HiddenField)ctrHdv).Value = string.Empty;
                }

                ctrHdv = BuscarControlRecursivo(pnl, preid + "txtgvValues" + idCampo);
                if (ctrHdv != null)
                {
                    ((HiddenField)ctrHdv).Value = campo.CampoValor.Valor;
                }
            }
            StringBuilder scriptGV = new StringBuilder();
            scriptGV.AppendLine("ControlGrillaAB|CTRLID|();");
            scriptGV.Replace("|CTRLID|", idGV);
            ScriptManager.RegisterClientScriptBlock(this.upCamposValores, this.upCamposValores.GetType(), "ScriptAgregar" + idGV, scriptGV.ToString(), true);
            //string scriptGV = "ControlGrillaAB|CTRLID|();";
            //scriptGV.Replace("|CTRLID|", idGV);
            //if (!string.IsNullOrWhiteSpace(campo.EventoJavaScript))
            //{
            //    StringBuilder scriptGV = new StringBuilder();
            //    scriptGV.AppendLine(" $(document).ready(function () {");
            //    //start scriptEvent
            //    //scriptGV.AppendLine("function ControlGrillaAB|CTRLID|() {");
            //    scriptGV.AppendFormat("{0}", campo.EventoJavaScript);
            //    //scriptGV.AppendLine("}");
            //    //end scriptEvent
            //    //scriptGV.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(ControlGrillaAB|CTRLID|);");
            //    //scriptGV.AppendLine("ControlGrillaAB|CTRLID|();");
            //    scriptGV.AppendLine("});");
            //    //scriptGV.Replace("|CTRLID|", gv.ID);
            //    ScriptManager.RegisterClientScriptBlock(this.upCamposValores, this.upCamposValores.GetType(), "ScriptAgregar" + idGV, scriptGV.ToString(), true);
            //}
        }

        private PlaceHolder AddControlNichos(TGECampos pCampo, bool pHabilitar, Gestion pGestion)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();
            Nichos ctrNichos = (Nichos)LoadControl("~/Modulos/Comunes/Nichos.ascx");

            Panel pnlRow = new Panel();
            pnlRow.CssClass = _cssCol2;

            ctrNichos.ID = preid + pCampo.IdCampo.ToString();
            ctrNichos.IniciarControl(pCampo, pHabilitar, pGestion);

            pnlRow.Controls.Add(ctrNichos);
            panel.Controls.Add(pnlRow);

            return panel;
        }

        private PlaceHolder AddControlTurismo(TGECampos pCampo, bool pHabilitar, Gestion pGestion)
        {
            pHabilitar = pCampo.Deshabilitado ? false : pHabilitar;
            PlaceHolder panel = new PlaceHolder();
            panel.ID = "panel" + pCampo.IdCampo.ToString();
            Turismo ctrTurismo = (Turismo)LoadControl("~/Modulos/Comunes/Turismo.ascx");

            Panel pnlRow = new Panel();
            pnlRow.CssClass = _cssCol2;

            ctrTurismo.ID = preid + pCampo.IdCampo.ToString();
            ctrTurismo.IniciarControl(this.MiTablaValor, pCampo, pHabilitar, pGestion);

            pnlRow.Controls.Add(ctrTurismo);
            panel.Controls.Add(pnlRow);

            return panel;
        }
        #endregion
    }
}