using Comunes.Entidades;
using Generales.FachadaNegocio;
using LavaYa;
using LavaYa.Entidades;
using RestSharp.Extensions;
using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace IU.Modulos.LavaYa.Controles
{
    public partial class MaquinasDatos : ControlesSeguros
    {
        public LavMaquinas MiMaquina
        {
            get { return this.PropiedadObtenerValor<LavMaquinas>("MaquinasDatosMisMaquinas"); }
            set { this.PropiedadGuardarValor("MaquinasDatosMisMaquinas", value); }
        }

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        public void IniciarControl(LavMaquinas pParametro, Gestion pGestion)
        {
            this.MiMaquina = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrCamposValores.IniciarControl(this.MiMaquina, new Objeto(), this.GestionControl);
                    this.ddlEstado.Enabled = false;
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    this.MiMaquina = MaquinasF.MaquinasObtenerDatosCompletos(this.MiMaquina);
                    this.MapearObjetoAControles(this.MiMaquina);
                    this.lblImagen.Visible = true;
                    this.imgLogo.Visible = true;
                    if (this.MiMaquina.ManualPDF.HasValue() && this.MiMaquina.ManualPDF.Length > 0)
                    {
                        this.btnVerManual.Visible = true;
                        this.btnEliminarManual.Visible = true;
                        this.lblPdfCargado.Text = "MANUAL CARGADO CORRECTAMENTE - " + this.MiMaquina.ManualPDF.Remove(0, 9);
                        this.lblPdfCargado.BackColor = System.Drawing.Color.LightGreen;
                        this.lblPdfCargado.Visible = true;
                    }
                    break;
                case Gestion.Consultar:
                    this.MiMaquina = MaquinasF.MaquinasObtenerDatosCompletos(this.MiMaquina);
                    this.MapearObjetoAControles(this.MiMaquina);
                    this.ddlMarca.Enabled = false;
                    this.txtNroSerie.Enabled = false;
                    this.ddlModelo.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.lblImagen.Visible = true;
                    this.imgLogo.Visible = true;
                    this.ddlTipoMaquina.Enabled = false;
                    if (this.MiMaquina.ManualPDF.HasValue() && this.MiMaquina.ManualPDF.Length > 0)
                    {
                        this.btnVerManual.Visible = true;
                        this.lblPdfCargado.Text = "MANUAL CARGADO CORRECTAMENTE - " + this.MiMaquina.ManualPDF.Remove(0, 9);
                        this.lblPdfCargado.BackColor = System.Drawing.Color.LightGreen;
                        this.lblPdfCargado.Visible = true;
                    }
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("LavMaquinas");
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();

            this.ddlMarca.DataSource = MaquinasF.MaquinasObtenerMarcas();
            this.ddlMarca.DataValueField = "IdMarca";
            this.ddlMarca.DataTextField = "Marca";
            this.ddlMarca.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlMarca, ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoMaquina.DataSource = MaquinasF.MaquinasObtenerTiposMaquinas();
            this.ddlTipoMaquina.DataValueField = "IdTipoMaquina";
            this.ddlTipoMaquina.DataTextField = "TipoMaquina";
            this.ddlTipoMaquina.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoMaquina, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.AgregarItemSeleccione(this.ddlModelo, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(LavMaquinas pParametro)
        {
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.ddlMarca.SelectedValue = pParametro.IdMarca.ToString();
            if (!string.IsNullOrEmpty(ddlMarca.SelectedValue))
                this.CargarModelos(pParametro);

            this.txtNroSerie.Text = pParametro.NumeroSerie;
            this.ddlTipoMaquina.SelectedValue = pParametro.IdTipoMaquina.ToString();
            this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);

            string base64String = pParametro.CodigoQR == null ? string.Empty : Convert.ToBase64String(pParametro.CodigoQRImagen, 0, pParametro.CodigoQRImagen.Length);
            this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
            this.imgLogo.Visible = true;
        }
        private void MapearControlesAObjeto(LavMaquinas pParametro)
        {
            pParametro.IdTipoMaquina = Convert.ToInt32(this.ddlTipoMaquina.SelectedValue);
            pParametro.TipoMaquina = this.ddlTipoMaquina.SelectedItem.Text;
            pParametro.IdMarca = Convert.ToInt32(this.ddlMarca.SelectedValue);
            pParametro.Marca = this.ddlMarca.SelectedItem.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pParametro.IdModelo = Convert.ToInt32(this.ddlModelo.SelectedValue);
            pParametro.Modelo = this.ddlModelo.SelectedItem.Text;
            pParametro.NumeroSerie = this.txtNroSerie.Text;
            pParametro.CodigoQR = " Marca: " + pParametro.Marca + " Modelo: " + pParametro.Modelo + " Tipo: " + pParametro.TipoMaquina + " NumeroSerie: " + pParametro.NumeroSerie;
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiMaquina);
            this.MiMaquina.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    MiMaquina.IdMaquina = 0;
                    guardo = MaquinasF.MaquinasAgregar(this.MiMaquina);
                    break;
                case Gestion.Anular:
                    this.MiMaquina.Estado.IdEstado = (int)Estados.Baja;
                    guardo = MaquinasF.MaquinasModificar(this.MiMaquina);
                    break;
                case Gestion.Modificar:
                    guardo = MaquinasF.MaquinasModificar(this.MiMaquina);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.lblManual.Visible = false;
                this.afuPdf.Visible = false;
                this.btnEliminarManual.Visible = false;
                this.upImagen.Update();
                this.MostrarMensaje(this.MiMaquina.CodigoMensaje, false);
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiMaquina.CodigoMensaje, true, this.MiMaquina.CodigoMensajeArgs);
                if (this.MiMaquina.dsResultado != null)
                {
                    this.MiMaquina.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }
        protected void ddlMarca_SelectedIndexChanged(object sender, EventArgs e)
        {
            LavMaquinas maquina = new LavMaquinas();
            maquina.IdMarca = this.ddlMarca.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMarca.SelectedValue);
            var aux = MaquinasF.MaquinasObtenerModelos(maquina);
            if (aux.Count > 0)
            {
                this.ddlModelo.DataSource = aux;
                this.ddlModelo.DataValueField = "IdModelo";
                this.ddlModelo.DataTextField = "Modelo";
                this.ddlModelo.DataBind();
            }
            else
            {
                this.ddlModelo.Items.Clear();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlModelo, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        private void CargarModelos(LavMaquinas pParametro)
        {
            ListItem item2 = ddlModelo.Items.FindByValue(pParametro.IdModelo.ToString());
            if (item2 == null)
                ddlModelo.Items.Add(new ListItem(pParametro.Modelo, pParametro.IdModelo.ToString()));
            else
                ddlModelo.SelectedValue = pParametro.IdModelo.ToString();

            ddlModelo.SelectedValue = pParametro.IdModelo.ToString();

            LavMaquinas maquina = new LavMaquinas();
            maquina.IdMarca = this.ddlMarca.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMarca.SelectedValue);
            var aux = MaquinasF.MaquinasObtenerModelos(maquina);
            if (aux.Count > 0)
            {
                this.ddlModelo.DataSource = aux;
                this.ddlModelo.DataValueField = "IdModelo";
                this.ddlModelo.DataTextField = "Modelo";
                this.ddlModelo.DataBind();
            }
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
        //si la imagen se carga satisfactoriamente,  la muestro por pantalla
        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            this.afuPdf.FailedValidation = false;
            this.afuPdf.ClearAllFilesFromPersistedStore();
        }
        protected void afuPdf_UploadedFileError(object sender, AjaxControlToolkit.AsyncFileUploadEventArgs e)
        {
            this.afuPdf.FailedValidation = false;
            this.afuPdf.ClearAllFilesFromPersistedStore();
        }
        protected void button_Click(object sender, EventArgs e)
        {
            try
            {
                this.afuPdf.FailedValidation = false;
                this.afuPdf.ClearAllFilesFromPersistedStore();
                this.MiMaquina.Pdf = this.StreamToByteArray(this.afuPdf.FileContent);
                this.afuPdf.FailedValidation = false;
                string RutaDelArchivo = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "Publica\\", afuPdf.FileName);
                if (File.Exists(RutaDelArchivo))
                {
                    File.Delete(RutaDelArchivo);
                }
                afuPdf.SaveAs(RutaDelArchivo);
                this.lblPdfCargado.Text = "MANUAL CARGADO CORRECTAMENTE - " + afuPdf.FileName;
                this.lblPdfCargado.BackColor = System.Drawing.Color.LightGreen;
                this.lblPdfCargado.Visible = true;
                this.btnVerManual.Visible = true;
                this.btnEliminarManual.Visible = true;
                if (this.MiMaquina.Pdf.Length > 0)
                {
                    this.MiMaquina.ManualPDF = "\\Publica\\" + afuPdf.FileName;
                }
            }
            catch (Exception ex)
            {
                this.lblPdfCargado.Visible = false;
                this.btnVerManual.Visible = false;
                this.btnEliminarManual.Visible = false;
                this.MostrarMensaje("Error al cargar el manual.", true);
            }

        }
        protected void btnEliminarManual_Click(object sender, EventArgs e)
        {
            this.btnVerManual.Visible = false;
            this.btnEliminarManual.Visible = false;
            this.lblPdfCargado.Visible = false;
            this.MiMaquina.ManualPDF = string.Empty;
            this.MiMaquina.Pdf = new byte[0];
            this.afuPdf.ClearAllFilesFromPersistedStore();
        }
        protected void btnVerManual_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(this.MiMaquina.ManualPDF))
                {
                    string urlPath = string.Concat(HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority), HttpContext.Current.Request.ApplicationPath);
                    urlPath = string.Concat(urlPath.EndsWith("/") ? urlPath : string.Concat(urlPath, "/"), this.MiMaquina.ManualPDF.Replace("\\", "/"));
                    //string url = this.ObtenerAppPath() + this.MiMaquina.ManualPDF.Replace("\\", "/");
                    string _open = "window.open('" + urlPath + "', '_newtab');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), Guid.NewGuid().ToString(), _open, true);
                }
                else
                {
                    this.MostrarMensaje("La maquina no tiene un manual cargado.", true);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("Error al abrir el manual.", true);
            }
        }
    }
}