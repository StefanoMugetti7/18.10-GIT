using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Seguridad.FachadaNegocio;
using Servicio.Encriptacion;
using System.Xml;
using Generales.FachadaNegocio;
using Generales.Entidades;

namespace IU
{
    public partial class RecuperarContraseniaPaso2 : System.Web.UI.Page
    {
        private Usuarios MiUsuario
        {
            get { return (Usuarios)Session["RecuperarContrasenaPaso2MiUsuario"]; }
            set { Session["RecuperarContrasenaPaso2MiUsuario"] = value; }
        }

        protected TGEEmpresas UsuarioEmpresa
        {
            get
            {
                if (this.Application["UsuarioEmpresa"] != null)
                { return (TGEEmpresas)this.Application["UsuarioEmpresa"]; }
                else
                { return new TGEEmpresas(); }
            }
            set { this.Application["UsuarioEmpresa"] = value; }
        }

        private XmlDocument MensajesSistema
        {
            get
            {
                if (this.Application["MensajesSistema"] != null)
                { return (XmlDocument)this.Application["MensajesSistema"]; }
                else
                {
                    XmlDocument xmlDoc = TGEGeneralesF.TGELeerXMLObtenerMensajesSistema();
                    this.Application.Add("MensajesSistema", xmlDoc);
                    return xmlDoc;
                }
            }
            set { this.Application["MensajesSistema"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar+=new Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                if (this.UsuarioEmpresa.IdEmpresa == 0)
                    this.UsuarioEmpresa = TGEGeneralesF.EmpresasSeleccionar();

                string base64String = this.UsuarioEmpresa.Logo == null ? string.Empty : Convert.ToBase64String(this.UsuarioEmpresa.Logo, 0, this.UsuarioEmpresa.Logo.Length);
                if (base64String != string.Empty)
                    this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
                else
                    this.imgLogo.ImageUrl = "Imagenes/Logo.png";
                this.imgLogo.AlternateText = this.UsuarioEmpresa.Empresa;

                string parametro = this.Request.QueryString["parametros"];
                if (string.IsNullOrEmpty(parametro))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);

                parametro = Encriptar.DesencriptarTexto(parametro);
                string[] datos = parametro.Split('|');
                string correo, fechaEvento;
                //cuit = datos[0];
                correo = datos[0];
                fechaEvento = datos[1];
                DateTime fechaValidar = new DateTime(Convert.ToInt32(fechaEvento.Substring(0, 4)), Convert.ToInt32(fechaEvento.Substring(4, 2)), Convert.ToInt32(fechaEvento.Substring(6, 2)));
                TimeSpan ts = DateTime.Now - fechaValidar;
                if (ts.Days > 1)
                {
                    //Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
                    this.popUpMensajes.MostrarMensaje("El link ya no es valido. Solicite un nuevo cambio de contraseña en la pagina Ingreso al Sistema en el link \"Olvide mi usuario o contraseña\"");
                    this.btnAceptar.Visible = false;
                }
                    

                this.MiUsuario = new Usuarios();
                this.MiUsuario.CorreoElectronico = correo;
                this.MiUsuario = SeguridadF.UsuariosObtenerPorCorreoElectronico(this.MiUsuario);

                if (this.MiUsuario.IdUsuario == 0)
                {
                    //Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
                    this.btnAceptar.Visible = false;
                    this.popUpMensajes.MostrarMensaje("El correo electronico ingresado no es valido");
                }
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Validate();
            if (!this.IsValid)
                return;
            this.MiUsuario = SeguridadF.UsuariosObtenerDatosCompleto(this.MiUsuario);
            
            this.MiUsuario.Contrasenia = txtContrasenia.Text;
            this.MiUsuario.CambiarContrasenia = false;
            this.MiUsuario.ResetearContrasenia = true;
            this.MiUsuario.EstadoColeccion = EstadoColecciones.Modificado;
            this.MiUsuario.IdUsuarioEvento = this.MiUsuario.IdUsuario;

            if (SeguridadF.UsuariosModificar(this.MiUsuario))
            {
                this.btnAceptar.Visible = false;
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiUsuario.CodigoMensaje));
                this.MiUsuario = null;
            }
            else
            {
                PaginaSegura pagina = new PaginaSegura();
                pagina.MostrarMensaje(MiUsuario.CodigoMensaje, true);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
        }

        private string ObtenerMensajeSistema(string codigoMensaje)
        {
            XmlNode nodo = this.MensajesSistema.GetElementsByTagName(codigoMensaje).Item(0);
            if (nodo != null)
                return nodo.InnerText;
            else
                return codigoMensaje;
        }
    }
}
