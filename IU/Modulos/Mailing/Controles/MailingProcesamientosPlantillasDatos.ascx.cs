using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Mailing.Entidades;
using Mailing;

namespace IU.Modulos.Mailing.Controles
{
    public partial class MailingProcesamientosPlantillasDatos : ControlesSeguros
    {
        private TGEMailingProcesamientosPlantillas MiPlantilla
        {
            get { return (TGEMailingProcesamientosPlantillas)Session[this.MiSessionPagina + "PlantillasModificarMiPlantilla"]; }
            set { Session[this.MiSessionPagina + "PlantillasModificarMiPlantilla"] = value; }
        }

        private List<TGEMailingProcesamientosPlantillas> MiPlantillaHijas
        {
            get { return (List<TGEMailingProcesamientosPlantillas>)Session[this.MiSessionPagina + "PlantillasModificarMiPlantillaHijas"]; }
            set { Session[this.MiSessionPagina + "PlantillasModificarMiPlantillaHijas"] = value; }
        }

        public delegate void PlantillasDatosAceptarEventHandler(object sender, TGEMailingProcesamientosPlantillas e);
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
 
        public void IniciarControl(TGEMailingProcesamientosPlantillas pParametro, Gestion pGestion)
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
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    rfvCodigo.Enabled = true;
                    this.MiPlantilla = MailingF.PlantillasObtenerDatosCompletos(this.MiPlantilla);
                    this.MapearObjetoAControles(this.MiPlantilla);
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
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            //TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.esta);

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
                   
                    break;
                case Gestion.Anular:
             
                    break;
                case Gestion.Modificar:
                    guardo = MailingF.PlantillasModificar(this.MiPlantilla);
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
        //private void MapearObjetoAControles(TGEMailingProcesamientosPlantillas pParametro)
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
                this.MisParametrosUrl.Add("IdPlantilla", this.MiPlantilla.IdPlantilla);
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingProcesamientosPlantillas.aspx"), true);
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.MiPlantilla.IdPlantillaRef.HasValue)
            {
                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdPlantilla", this.MiPlantilla.IdPlantillaRef.Value);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/MailingProcesamientosPlantillas.aspx"), true);
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
        private void MapearObjetoAControles(TGEMailingProcesamientosPlantillas pParametro)
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
            this.txtHojaAncho.Text = MiPlantilla.HojaAncho.HasValue ? this.MiPlantilla.HojaAncho.Value.ToString("N2") : string.Empty;
            this.txtMargenIzquierdo.Text = MiPlantilla.MargenIzquierdo.HasValue ? this.MiPlantilla.MargenIzquierdo.Value.ToString("N2") : string.Empty;
            this.txtMargenDerecho.Text = MiPlantilla.MargenDerecho.HasValue ? this.MiPlantilla.MargenDerecho.Value.ToString("N2") : string.Empty;
            this.txtMargenSuperior.Text = MiPlantilla.MargenSuperior.HasValue ? this.MiPlantilla.MargenSuperior.Value.ToString("N2") : string.Empty;
            this.txtMargenInferior.Text = MiPlantilla.MargenInferior.HasValue ? this.MiPlantilla.MargenInferior.Value.ToString("N2") : string.Empty;
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
            TGEMailingProcesamientosPlantillas plantillaFiltro = new TGEMailingProcesamientosPlantillas();
            plantillaFiltro.IdPlantillaRef = this.MiPlantilla.IdPlantilla;
            this.MiPlantillaHijas = MailingF.PlantillasObtenerListaFiltro(plantillaFiltro);
            AyudaProgramacion.CargarGrillaListas<TGEMailingProcesamientosPlantillas>(this.MiPlantillaHijas, false, this.gvDatos, true);


        }

        private void MapearControlesAObjeto(TGEMailingProcesamientosPlantillas pParametro)
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
            pParametro.NumerarPaginas = Convert.ToInt32(ddlNumerarPaginas.SelectedValue);
            pParametro.HtmlPlantilla = this.CKEditor1.Text;
            pParametro.AjustarUnaHoja = this.chkAjustarUnaHoja.Checked;
            pParametro.ImagenFondo = this.MiPlantilla.ImagenFondo;


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

        protected void btnBorrarImagen_Click(object sender, EventArgs e)
        {
            //this.MiPlantilla.ImagenFondo = this.StreamToByteArray(this.afuLogo.FileContent);
            this.MiPlantilla.ImagenFondo = "";
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
            TGEMailingProcesamientosPlantillas sector = this.MiPlantillaHijas[indiceColeccion];

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
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Plantillas/MailingProcesamientosPlantillas.aspx"), true);
            }

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TGEMailingProcesamientosPlantillas plantilla = (TGEMailingProcesamientosPlantillas)e.Row.DataItem;
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                modificar.Visible = this.ValidarPermiso("MailingProcesamientosPlantillas.aspx");
                //consultar.Visible = this.ValidarPermiso("PlantillasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }
        #endregion
    }
}