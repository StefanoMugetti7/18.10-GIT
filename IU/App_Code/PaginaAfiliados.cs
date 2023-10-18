using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using System.Collections.Generic;
using Generales.FachadaNegocio;
using System.Collections;
using System.Web.SessionState;

namespace IU
{
    public class PaginaAfiliados : PaginaSegura 
    {
        protected AfiAfiliados MiAfiliado
        {
            get
            {
                if (Session[this.MiSessionPagina + "AFIAfiliadosMiAfiliado"] != null)
                    return (AfiAfiliados)Session[this.MiSessionPagina + "AFIAfiliadosMiAfiliado"];
                else
                {
                    return (AfiAfiliados)(Session[this.MiSessionPagina + "AFIAfiliadosMiAfiliado"] = new AfiAfiliados());
                }
            }
            set { Session[this.MiSessionPagina + "AFIAfiliadosMiAfiliado"] = value; }
        }

        private new string MiSessionPagina
        {
            get
            {
                return Request.QueryString["tabName"] == null ? string.Empty : Request.QueryString["tabName"].ToString();
            }
            set
            {
                //this._sessionPaginaHija = value;
            }
        }

        public void Guardar(string key, AfiAfiliados pAfi)
        {
            Session[key + "AFIAfiliadosMiAfiliado"] = pAfi;
        }

        public AfiAfiliados Obtener(string key)
        {
            return (AfiAfiliados)Session[key + "AFIAfiliadosMiAfiliado"];
        }

        virtual protected void PageLoadEventAfiliados(object sender, System.EventArgs e) { }

        /// <summary>
        /// Devuelve una instancia del control Label Mensaje en la Master para
        /// mostrar un mensaje en el sistema.
        /// </summary>
        //protected Label Afiliado
        //{
        //    get { return (Label)this.MaestraPrincipal.FindControl("ContentPlaceEncabezado").FindControl("lblAfiliado"); }
        //    set
        //    {
        //        Label _mensaje = (Label)MaestraPrincipal.FindControl("ContentPlaceEncabezado").FindControl("lblAfiliado");
        //        _mensaje = value;
        //    }
        //}

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
                this.MenuPadre = EnumMenues.Afiliados;

            //Valido la sesion del Socio
            AfiAfiliados MiAfiliado = this.Obtener(this.MiSessionPagina);
            if (MiAfiliado == null || MiAfiliado.IdAfiliado == 0)
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosListar.aspx"), true);
            }

            //Valido que el esado del socio tenga permisos para el tipo de funcionalidad
            if(this.paginaActual.IdTipoFuncionalidad>0)
            {
                List<TGETiposFuncionalidades> lista = TGEGeneralesF.TGETiposFuncionalidadesObtenerListaFiltro(MiAfiliado.Estado);
                if (!lista.Exists(x=>x.IdTipoFuncionalidad== this.paginaActual.IdTipoFuncionalidad))
                {
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("CodigoMensaje", "ValidarTipoFuncionalidadEstado");
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlMensajes.aspx"), true);
                }
            }
            //this.Afiliado.Text = string.Concat(this.MiAfiliado.Apellido, ", ", this.MiAfiliado.Nombre);        
            this.PageLoadEventAfiliados(sender, e);
        }
    }
}
