using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

namespace IU.Modulos.Seguridad
{
    public partial class SegGestionarUsuariosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.UsuariosDatosAceptar += new IU.Modulos.Seguridad.SegGestionarUsuariosDatos.SegGestionarUsuariosDatosAceptarEventHandler(ModificarDatos_UsuarioModificarDatosAceptar);
            this.ModificarDatos.UsuariosDatosCancelar += new IU.Modulos.Seguridad.SegGestionarUsuariosDatos.SegGestionarUsuariosDatosAceptarEventHandlerCancelarEventHandler(ModificarDatos_UsuarioModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                Usuarios usuario = new Usuarios();
                //Control y Validacion de Parametros
                //if (!this.MisParametrosUrl.Contains("IdUsuario"))
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegGestionarUsuarios.aspx"), true);

                //int parametro = Convert.ToInt32(this.MisParametrosUrl["IdUsuario"]);
                //usuario.IdUsuario = parametro;

                this.ModificarDatos.IniciarControl(usuario, Gestion.Agregar);
            }
        }

        void ModificarDatos_UsuarioModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegGestionarUsuarios.aspx"), true);
        }

        //void ModificarDatos_UsuarioModificarDatosAceptar(object sender, global::Comunes.Entidades.Usuarios e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegGestionarUsuarios.aspx"), true);
        //}
    }
}