using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Auditoria.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Seguridad.Entidades;
using Auditoria;
using AjaxControlToolkit;

namespace IU.Modulos.Auditoria.Control
{
    public partial class AuditoriaDatos : ControlesSeguros
    {
        private List<HistorialCambios> MisHistorialCambios
        {
            get { return (List<HistorialCambios>)Session[this.MiSessionPagina + "AuditoriaDatosMisHistorialCambios"]; }
            set { Session[this.MiSessionPagina + "AuditoriaDatosMisHistorialCambios"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

        }

        public void IniciarControl(Objeto pObjeto)
        {
            this.IniciarControl(pObjeto, string.Empty);
        }

        public void IniciarControl(Objeto pObjeto, string pCampoCambiado)
        {
            pObjeto.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MisHistorialCambios = AuditoriaF.AuditoriaObtenerLista(pObjeto, pCampoCambiado);
            this.IniciarControl();
        }

        public void IniciarControl(Usuarios pObjeto)
        {
            pObjeto.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.MisHistorialCambios = AuditoriaF.AuditoriaObtenerLista(pObjeto, string.Empty);
            this.IniciarControl();
        }

        private void IniciarControl()
        {
            if (this.MisHistorialCambios.Count == 0)
            {
                if (this.Parent is System.Web.UI.Control && this.Parent.Parent is TabPanel)
                    this.Parent.Parent.Visible=false;
            }
            else
            {
                this.gvDatos.DataSource = this.MisHistorialCambios;
                this.gvDatos.DataBind();
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisHistorialCambios;
            gvDatos.DataBind();
        }
    }
}