using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using System.IO;


namespace IU.Modulos.TGE.Control
{
    public partial class EmpresasDatos : ControlesSeguros
    {   
        TGEEmpresas MiEmpresa
        {
            get { return (TGEEmpresas)Session[this.MiSessionPagina + "EmpresasDatosMiEmpresa"]; }
            set { Session[this.MiSessionPagina + "EmpresasDatosMiEmpresa"] = value; }
        }

        public delegate void EmpresasDatosAceptarEventHandler(object sender, TGEEmpresas e);
        public event EmpresasDatosAceptarEventHandler EmpresaModificarDatosAceptar;

        public delegate void EmpresasDatosCancelarEventHandler();
        public event EmpresasDatosCancelarEventHandler EmpresaModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (this.IsPostBack)
            {
                if (this.MiEmpresa == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una operacion
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(TGEEmpresas pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiEmpresa = pParametro;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrComentarios.IniciarControl(this.MiEmpresa, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiEmpresa, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiEmpresa = TGEGeneralesF.EmpresaObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiEmpresa);
                    break;
                case Gestion.Consultar:
                    this.MiEmpresa = TGEGeneralesF.EmpresaObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiEmpresa);
                    this.txtEmpresa.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.txtCalle.Enabled = false;
                    this.txtNumero.Enabled = false;
                    this.txtPiso.Enabled = false;
                    this.txtOficina.Enabled = false;
                    this.txtTelefono.Enabled = false;
                    this.txtCUIT.Enabled = false;
                    this.txtNumeroIIBB.Enabled = false;
                    this.txtMatriculaINAES.Enabled = false;
                    this.txtFechaInicioActividad.Enabled = false;
                    this.txtCodigoPostal.Enabled = false;
            
                    this.ddlCondicionFiscal.Enabled = false;
                    
                    this.txtCantidadUsuarios.Enabled = false;
                    this.txtCantidadFiliales.Enabled = false;
                    //TAPO TODO LOGO
                    this.lblLogo.Visible = false;
                    this.afuLogo.Visible = false;
                    this.imgUploadFile.Visible = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscal.DataTextField = "Descripcion";
            this.ddlCondicionFiscal.DataBind();
            if(this.ddlCondicionFiscal.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiEmpresa);
            this.MiEmpresa.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiEmpresa.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = TGEGeneralesF.EmpresaAgregar(this.MiEmpresa);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.EmpresaModificar(this.MiEmpresa);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiEmpresa.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiEmpresa.CodigoMensaje, true, this.MiEmpresa.CodigoMensajeArgs);
            }
        }

        private void MapearControlesAObjeto(TGEEmpresas pParametro)
        {
            pParametro.Empresa = this.txtEmpresa.Text;
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.DireccionCalle = this.txtCalle.Text;
            pParametro.DireccionNumero = this.txtNumero.Text.Trim() == string.Empty ? 0: Convert.ToInt32(this.txtNumero.Text.Trim());
            pParametro.DireccionPiso = this.txtPiso.Text.Trim()==string.Empty? 0 : Convert.ToInt32(this.txtPiso.Text.Trim());
            pParametro.DireccionOficina = this.txtOficina.Text;
            pParametro.Telefono = this.txtTelefono.Text;
            pParametro.CodigoPostal = this.txtCodigoPostal.Text;
            pParametro.CUIT = this.txtCUIT.Text;
            pParametro.NumeroIIBB = this.txtNumeroIIBB.Text;
            pParametro.MatriculaINAES = this.txtMatriculaINAES.Text;
            pParametro.FechaInicioActividad = this.txtFechaInicioActividad.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaInicioActividad.Text);
            pParametro.CondicionFiscal.IdCondicionFiscal = this.ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscal.SelectedValue);
            if(this.afuLogo.HasFile)
                pParametro.Logo = this.StreamToByteArray(this.afuLogo.FileContent);
            pParametro.CantidadUsuarios = this.txtCantidadUsuarios.Text.Trim()==string.Empty? 0 : Convert.ToInt32(this.txtCantidadUsuarios.Text.Trim());
            pParametro.CantidadFiliales = this.txtCantidadFiliales.Text.Trim()==string.Empty? 0 : Convert.ToInt32(this.txtCantidadFiliales.Text.Trim());

            pParametro.Comentarios = ctrComentarios.ObtenerLista();
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
        }

        private void MapearObjetoAControles(TGEEmpresas pParametro)
        {
            this.txtEmpresa.Text = pParametro.Empresa ;
            this.txtDescripcion.Text = pParametro.Descripcion ;
            this.txtCalle.Text = pParametro.DireccionCalle;
            this.txtNumero.Text = pParametro.DireccionNumero.ToString();
            this.txtPiso.Text = pParametro.DireccionPiso.ToString();
            this.txtOficina.Text = pParametro.DireccionOficina;
            this.txtTelefono.Text = pParametro.Telefono;
            this.txtCodigoPostal.Text = pParametro.CodigoPostal;
            this.txtCUIT.Text = pParametro.CUIT;
            this.txtNumeroIIBB.Text = pParametro.NumeroIIBB;
            this.txtMatriculaINAES.Text = pParametro.MatriculaINAES;
            this.txtFechaInicioActividad.Text = pParametro.FechaInicioActividad.ToString();
            this.ddlCondicionFiscal.SelectedValue = pParametro.CondicionFiscal.IdCondicionFiscal.ToString();
            //probando imagen
            string base64String = pParametro.Logo ==null? string.Empty : Convert.ToBase64String(pParametro.Logo, 0, pParametro.Logo.Length);
            this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
            this.imgLogo.Visible = true;
            
            //hasta aca

            this.txtCantidadUsuarios.Text = pParametro.CantidadUsuarios.ToString();
            this.txtCantidadFiliales.Text = pParametro.CantidadFiliales.ToString();

            this.ctrComentarios.IniciarControl(pParametro, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pParametro);
            this.ctrCamposValores.IniciarControl(this.MiEmpresa, new Objeto(), this.GestionControl);
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
            this.button_Click(sender, e);
        }

        protected void button_Click(object sender, EventArgs e)
        {
            this.MiEmpresa.Logo = this.StreamToByteArray(this.afuLogo.FileContent);
            this.afuLogo.FailedValidation = false;
            string base64String = Convert.ToBase64String(this.MiEmpresa.Logo, 0, this.MiEmpresa.Logo.Length);
            this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
            this.imgLogo.Visible = true;
        }
        
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.EmpresaModificarDatosCancelar != null)
                this.EmpresaModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.EmpresaModificarDatosAceptar != null)
                this.EmpresaModificarDatosAceptar(null, this.MiEmpresa);
        }
    }
}