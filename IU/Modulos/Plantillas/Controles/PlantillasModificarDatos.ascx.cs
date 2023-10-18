using Cargos.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Mailing;
using Mailing.Entidades;
using SharpCompress.Compressor.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Web.UI.WebControls;

namespace IU.Modulos.Plantillas.Controles
{
    public partial class PlantillasModificarDatos : ControlesSeguros
    {
        private TGEPlantillas MiPlantilla
        {
            get { return (TGEPlantillas)Session[this.MiSessionPagina + "PlantillasModificarMiPlantilla"]; }
            set { Session[this.MiSessionPagina + "PlantillasModificarMiPlantilla"] = value; }
        }

        private List<TGEPlantillas> MiPlantillaHijas
        {
            get { return (List<TGEPlantillas>)Session[this.MiSessionPagina + "PlantillasModificarMiPlantillaHijas"]; }
            set { Session[this.MiSessionPagina + "PlantillasModificarMiPlantillaHijas"] = value; }
        }

        private List<TGEListasValoresDetalles> MisTiposPlantillas
        {
            get { return (List<TGEListasValoresDetalles>)Session[this.MiSessionPagina + "PlantillasModificarMisTiposPlantillas"]; }
            set { Session[this.MiSessionPagina + "PlantillasModificarMisTiposPlantillas"] = value; }
        }

        public delegate void PlantillasDatosAceptarEventHandler(object sender, TGEPlantillas e);
        public event PlantillasDatosAceptarEventHandler PlanesDatosAceptar;

        public delegate void PlantillasDatosCancelarEventHandler();
        public event PlantillasDatosCancelarEventHandler PlanesDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!IsPostBack)
            {
                this.CKEditor1.BasePath = this.ResolveUrl("~/ckeditor/");
                this.CKEditor1.FilebrowserImageBrowseUrl = this.ResolveUrl("~/ImagenesCliente/");
                this.CKEditor1.FilebrowserImageUploadUrl = this.ResolveUrl("~/Upload.ashx");
                this.CKEditor1.IgnoreEmptyParagraph = false;
            }
        }

        public void IniciarControl(TGEPlantillas pParametro, Gestion pGestion)
        {
            if (UsuarioActivo.EsAdministradorGeneral)
            {
                ddlEstados.Enabled = true;
                rfvCodigo.Enabled = true;
                 rfvPlantilla.Enabled = true;
                txtCodigo.Enabled = true;
                txtHojaAlto.Enabled = true;
                txtHojaAncho.Enabled = true;
                txtMargenIzquierdo.Enabled = true;
                txtMargenDerecho.Enabled = true;
                txtMargenSuperior.Enabled = true;
                txtMargenInferior.Enabled = true;
                txtPlantilla.Enabled = true;
                txtStore.Enabled = true;
                ddlTipoPlanilla.Enabled = true;
                txtCantidadCopias.Enabled = true;
                txtRellenarFilasVacias.Enabled = true;
            }
            btnCancelar.Enabled = true;
            this.imgLogo.Visible = false;
            this.MiPlantilla = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.btnAceptar.Visible = true;
                    rfvCodigo.Enabled = true;
                    btnPlantilla.Visible = false;
                    this.ctrCamposValores.IniciarControl(this.MiPlantilla, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    rfvCodigo.Enabled = true;
                    this.MiPlantilla = TGEGeneralesF.PlantillasObtenerDatosCompletos(this.MiPlantilla);
                    this.MapearObjetoAControles(this.MiPlantilla);
                    this.ctrCamposValores.IniciarControl(this.MiPlantilla, new Objeto(), this.GestionControl);
                    this.btnAceptar.Visible = true;
                    break;
                case Gestion.Consultar:
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            //this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));


            MisTiposPlantillas = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposPlantilla);
            this.ddlTipoPlanilla.DataSource = MisTiposPlantillas;
            this.ddlTipoPlanilla.DataValueField = "IdListaValorDetalle";//"IdTipoPlanilla";
            this.ddlTipoPlanilla.DataTextField = "Descripcion";//"TipoPlanilla";
            this.ddlTipoPlanilla.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoPlanilla, ObtenerMensajeSistema("SeleccioneOpcion"));

            List<TGEMailingProcesos> MisProcesos = new List<TGEMailingProcesos>();

            MisProcesos = MailingF.TGEMailingObtenerListaMailingProceso();
            this.ddlTipoProceso.DataSource = MisProcesos;
            this.ddlTipoProceso.DataValueField = "IdMailingProceso";
            this.ddlTipoProceso.DataTextField = "Descripcion";
            this.ddlTipoProceso.DataBind();
            if (this.ddlTipoProceso.Items.Count != 1)
                AyudaProgramacion.InsertarItemSeleccione(this.ddlTipoProceso, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.MiPlantilla.HtmlPlantilla = CKEditor1.Text;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiPlantilla);

            this.MiPlantilla.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = TGEGeneralesF.PlantillasAgregar(this.MiPlantilla);
                    break;
                case Gestion.Anular:
                    this.MiPlantilla.Estado.IdEstado = (int)Estados.Baja;
                    guardo = TGEGeneralesF.PlantillasModificar(this.MiPlantilla);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.PlantillasModificar(this.MiPlantilla);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.MostrarMensaje(this.MiPlantilla.CodigoMensaje, false);
                        this.IniciarControl(this.MiPlantilla, Gestion.Modificar);  
            }
            else
                this.MostrarMensaje(this.MiPlantilla.CodigoMensaje, true);

        }
        //private void MapearObjetoAControles(TGEPlantillas pParametro)
        //{
        //    this.txtCodigo.Text = pParametro.Codigo;
        //    txtHojaAlto.Text = pParametro.HojaAlto.ToString();
        //    txtHojaAncho.Text = pParametro.HojaAncho.ToString();
        //    txtMargenIzquierdo.Text = pParametro.MargenIzquierdo.ToString();
        //    txtPlantilla.Text = pParametro.NombrePlantilla.ToString();
        //    //txtStore.Text = pParametro.
        //}
        protected void btnAgregarPlantilla_Click(object sender, EventArgs e)
        {
            if (this.MiPlantilla.IdPlantilla > 0)
            {
                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdPlantillaRef", this.MiPlantilla.IdPlantilla);
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasAgregar.aspx"), true);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.MiPlantilla.IdPlantillaRef.HasValue)
            {   
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("IdPlantilla", this.MiPlantilla.IdPlantillaRef.Value);
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasModificar.aspx"), true);
            }
            if (this.PlanesDatosCancelar != null)
            {
               
                    this.PlanesDatosCancelar();
            }
            //if (this.ViewState["UrlReferrer"] != null)
            //    this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            //else
            //    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasListar.aspx"), true);
        }
        private void MapearObjetoAControles(TGEPlantillas pParametro)
        {
            //this.MiPlantilla.IdPlantilla = Convert.ToInt32(this.MisParametrosUrl["IdPlantilla"]);
            //this.MisParametrosUrl = new Hashtable(); ???????????????
            //this.MiPlantilla = TGEGeneralesF.PlantillasObtenerDatosCompletos(this.MiPlantilla);

            this.ddlEstados.SelectedValue = this.MiPlantilla.Estado.IdEstado.ToString();
            this.txtPlantilla.Text = this.MiPlantilla.NombrePlantilla;
            this.chkAjustarUnaHoja.Checked = this.MiPlantilla.AjustarUnaHoja;
            chkUsarHojaPorDefecto.Checked = this.MiPlantilla.NoUsarHojaPorDefecto;
            if (!string.IsNullOrEmpty(MiPlantilla.NombreSP))
      this.txtStore.Text = this.MiPlantilla.NombreSP.ToString();
            this.CKEditor1.Text = this.MiPlantilla.HtmlPlantilla; 
            this.txtCodigo.Text = this.MiPlantilla.Codigo;
            this.txtHojaAlto.Text = this.MiPlantilla.HojaAlto.HasValue ? this.MiPlantilla.HojaAlto.Value.ToString("N2") : string.Empty;
            this.txtHojaAncho.Text = MiPlantilla.HojaAncho.HasValue? this.MiPlantilla.HojaAncho.Value.ToString("N2") : string.Empty;
            this.txtMargenIzquierdo.Text = MiPlantilla.MargenIzquierdo.HasValue ? this.MiPlantilla.MargenIzquierdo.Value.ToString("N2") : string.Empty;
            this.txtMargenDerecho.Text = MiPlantilla.MargenDerecho.HasValue ? this.MiPlantilla.MargenDerecho.Value.ToString("N2") : string.Empty;
            this.txtMargenSuperior.Text = MiPlantilla.MargenSuperior.HasValue ? this.MiPlantilla.MargenSuperior.Value.ToString("N2") : string.Empty;
            this.txtMargenInferior.Text = MiPlantilla.MargenInferior.HasValue ? this.MiPlantilla.MargenInferior.Value.ToString("N2") : string.Empty;
            this.txtCantidadCopias.Text = MiPlantilla.CantidadCopias.ToString();
            this.txtRellenarFilasVacias.Text = MiPlantilla.RellenarFilasVacias.ToString();
            this.txtKeysPDFCorte.Text = MiPlantilla.KeysPDFCorte;


            if (MiPlantilla.IdTipoPlantilla.HasValue)
            {
                if(MiPlantilla.IdTipoPlantilla > 0)
                this.ddlTipoPlanilla.SelectedValue = this.MiPlantilla.IdTipoPlantilla.Value.ToString();

            }

            if (MiPlantilla.IdTipoProceso.HasValue)
            {
                if (MiPlantilla.IdTipoProceso > 0)
                    this.ddlTipoProceso.SelectedValue = this.MiPlantilla.IdTipoProceso.Value.ToString();
                ddlTipoPlantilla_OnSelectedIndexChanged(null, EventArgs.Empty);
            }



            this.ctrCamposValores.IniciarControl(this.MiPlantilla, new Objeto(), this.GestionControl);
            this.ctrCamposValores.IniciarControl(this.MiPlantilla, this.MiPlantilla, this.GestionControl);
            this.lstCampos.DataSource = this.MiPlantilla.PlantillasCampos;
            this.lstCampos.DataValueField = "Nombre";
            this.lstCampos.DataTextField = "Nombre";
            this.lstCampos.DataBind();
            ddlNumerarPaginas.SelectedValue = MiPlantilla.NumerarPaginas.ToString();
            //string base64String = pParametro.ImagenFondo == null ? string.Empty : Convert.ToString(pParametro.ImagenFondo);
            //this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
            this.imgLogo.ImageUrl = string.IsNullOrWhiteSpace(this.MiPlantilla.ImagenFondo) ? string.Empty : string.Concat("~/", this.MiPlantilla.ImagenFondo.Replace("\\", "/"));
            this.imgLogo.Visible = string.IsNullOrEmpty(this.MiPlantilla.ImagenFondo) ? false : true;
            this.btnBorrarImagen.Visible = imgLogo.Visible;
            TGEPlantillas plantillaFiltro = new TGEPlantillas();
            plantillaFiltro.IdPlantillaRef = this.MiPlantilla.IdPlantilla;
            this.MiPlantillaHijas = TGEGeneralesF.PlantillasObtenerListaFiltro(plantillaFiltro);
            AyudaProgramacion.CargarGrillaListas<TGEPlantillas>(this.MiPlantillaHijas, false, this.gvDatos, true);


        }

        private void MapearControlesAObjeto(TGEPlantillas pParametro)
        {
            if (MiPlantilla.IdPlantillaRef.HasValue)
                pParametro.IdPlantillaRef = MiPlantilla.IdPlantillaRef;
            pParametro.NoUsarHojaPorDefecto = chkUsarHojaPorDefecto.Checked;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.NombrePlantilla = txtPlantilla.Text;
            pParametro.NombreSP = txtStore.Text;
            pParametro.Codigo = txtCodigo.Text;
            pParametro.HojaAlto = txtHojaAlto.Text == string.Empty ? default(decimal?) : Convert.ToDecimal(this.txtHojaAlto.Text);
            pParametro.HojaAncho = txtHojaAncho.Text == string.Empty ? default(decimal?) : Convert.ToDecimal(this.txtHojaAncho.Text);
            pParametro.MargenIzquierdo = txtMargenIzquierdo.Text == string.Empty ? default(decimal?) : Convert.ToDecimal(this.txtMargenIzquierdo.Text);
            pParametro.MargenDerecho = txtMargenDerecho.Text == string.Empty ? default(decimal?) : Convert.ToDecimal(this.txtMargenDerecho.Text);
            pParametro.MargenSuperior = txtMargenSuperior.Text == string.Empty ? default(decimal?) : Convert.ToDecimal(this.txtMargenSuperior.Text);
            pParametro.MargenInferior = txtMargenInferior.Text == string.Empty ? default(decimal?) : Convert.ToDecimal(this.txtMargenInferior.Text);
            pParametro.RellenarFilasVacias = txtRellenarFilasVacias.Text == string.Empty ? 0 : Convert.ToInt32(this.txtRellenarFilasVacias.Text);
            pParametro.CantidadCopias = txtCantidadCopias.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadCopias.Text);
            pParametro.NumerarPaginas = Convert.ToInt32(ddlNumerarPaginas.SelectedValue);
            pParametro.HtmlPlantilla = this.CKEditor1.Text;
            pParametro.AjustarUnaHoja = this.chkAjustarUnaHoja.Checked;
            pParametro.ImagenFondo =this.MiPlantilla.ImagenFondo;
            pParametro.IdTipoPlantilla = ddlTipoPlanilla.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoPlanilla.SelectedValue);
            pParametro.IdTipoProceso = ddlTipoProceso.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoProceso.SelectedValue);
            pParametro.KeysPDFCorte = this.txtKeysPDFCorte.Text;
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
            pParametro.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();


        }
        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            this.button_Click(sender, e);
        }
        protected void button_Click(object sender, EventArgs e)
        {
            //this.MiPlantilla.ImagenFondo = this.StreamToByteArray(this.afuLogo.FileContent);
            this.MiPlantilla.ImagenFondo = string.Concat("ImagenesCliente\\\\", afuLogo.FileName); 
            this.afuLogo.FailedValidation = false;
            string filepath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "ImagenesCliente\\", afuLogo.FileName);
            afuLogo.SaveAs(filepath);
            //string base64String = Convert.ToString(this.MiPlantilla.ImagenFondo);
            this.imgLogo.ImageUrl = string.Concat("~/", this.MiPlantilla.ImagenFondo.Replace("\\\\", "/"));
            this.imgLogo.Visible = true;
            btnBorrarImagen.Visible = true;

        }

        protected void ddlTipoPlantilla_OnSelectedIndexChanged(object sender, EventArgs e)
        {


            if (!string.IsNullOrEmpty(ddlTipoPlanilla.SelectedValue) && Convert.ToInt32(ddlTipoPlanilla.SelectedValue) == MisTiposPlantillas.Find(x => x.CodigoValor == "Mailing").IdListaValorDetalle)
            {

                ddlTipoProceso.Enabled = true;

            }
            else
            {
                ddlTipoProceso.SelectedValue = "";
                ddlTipoProceso.Enabled = false;
            }



        }

        protected void btnBorrarImagen_Click(object sender, EventArgs e)
        {
            //this.MiPlantilla.ImagenFondo = this.StreamToByteArray(this.afuLogo.FileContent);
            this.MiPlantilla.ImagenFondo =  "";
            this.afuLogo.FailedValidation = false;
            this.imgLogo.ImageUrl = "";
            this.imgLogo.Visible = false;
            btnBorrarImagen.Visible = false;

        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            // This is optional
            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }

        #region Grilla
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEPlantillas sector = this.MiPlantillaHijas[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPlantilla", sector.IdPlantilla);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                bool guardo = true;
                this.btnAceptar.Visible = false;
                this.MiPlantilla.HtmlPlantilla = CKEditor1.Text;
                guardo = TGEGeneralesF.PlantillasModificar(this.MiPlantilla);
                if (!guardo)
                {
                    this.btnAceptar.Visible = true;
                    this.MostrarMensaje(this.MiPlantilla.CodigoMensaje, true, this.MiPlantilla.CodigoMensajeArgs);
                    return;
                }
                else
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/PlantillasConsultar.aspx"), true);
            }

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGEPlantillas plantilla = (TGEPlantillas)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("PlantillasModificar.aspx");
                //consultar.Visible = this.ValidarPermiso("PlantillasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }
        #endregion
    }
}
