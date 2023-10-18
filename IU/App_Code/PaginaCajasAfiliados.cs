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
using System.Collections.Generic;
using Generales.FachadaNegocio;
using System.Collections;



namespace IU
{
    public class PaginaCajasAfiliados : PaginaCajas
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

        virtual protected void PageLoadEventCajasAfiliados(object sender, System.EventArgs e) { }

        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            base.PageLoadEventCajas(sender, e);

            if (!this.IsPostBack)
            {
                this.MenuPadre = EnumMenues.CajasAfiliados;
                if (this.MiAfiliado.IdAfiliado == 0)
                {
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAfiliadosListar.aspx"), true);
                }
            }

            //Valido que el esado del socio tenga permisos para el tipo de funcionalidad
            if (this.paginaActual.IdTipoFuncionalidad > 0)
            {
                List<TGETiposFuncionalidades> lista = TGEGeneralesF.TGETiposFuncionalidadesObtenerListaFiltro(this.MiAfiliado.Estado);
                if (!lista.Exists(x => x.IdTipoFuncionalidad == this.paginaActual.IdTipoFuncionalidad))
                {
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("CodigoMensaje", "ValidarTipoFuncionalidadEstado");
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlMensajes.aspx"), true);
                }
            }
            this.PageLoadEventCajasAfiliados(sender, e);
        }
    }
}
