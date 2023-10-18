using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seguridad.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Seguridad
{
    public partial class SegGestionarPerfilesUsuarios : ControlesSeguros
    {
        private Perfiles MiPerfil
        {
            get { return (Perfiles)Session[this.MiSessionPagina + "SegGestionarPerfilesUsuariosMiPerfil"]; }
            set { Session[this.MiSessionPagina + "SegGestionarPerfilesUsuariosMiPerfil"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //Limpio las variables de sesion
                this.MiPerfil = new Perfiles();
            }    
        }

        public void IniciarControl(Perfiles pPerfil)
        {
            this.MiPerfil = pPerfil;
            this.lbxUsuarios.DataSource = this.MiPerfil.Usuarios;
            this.lbxUsuarios.DataTextField = "ApellidoNombre";
            this.lbxUsuarios.DataValueField = "IdUsuario";
            this.lbxUsuarios.DataBind();
            this.mpePopUp.Show();
        }
    }
}