using Comunes.Entidades;
using Generales.Entidades;
using Seguridad.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Seguridad.Controles
{
    public partial class SegUsuariosNotificacionesModificarPopUp : ControlesSeguros
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                this.CargarNotificaciones();
            }
        }

        protected void btnRecargarNotificaciones_Click(object sender, EventArgs e)
        {
            CargarNotificaciones();
            upModalNotificaciones.Update();
        }
        private void CargarNotificaciones()
        {
            List<SegNotificaciones> notifiaciones = new List<SegNotificaciones>();
            notifiaciones = SeguridadF.UsuariosObtenerNotificaciones(this.UsuarioActivo);
            this.gvDatos.DataSource = notifiaciones;
            this.gvDatos.DataBind();
            this.gvDatos.Visible = gvDatos.Rows.Count > 0;
            this.btnProcesar.Visible = gvDatos.Visible;
            this.lblMensaje.Visible= !gvDatos.Visible;
            hdfNotificaciones.Value = gvDatos.Visible ? "1" : "0";
        }
    }
}