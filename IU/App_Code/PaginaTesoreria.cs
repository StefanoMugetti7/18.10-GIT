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
using Tesorerias.Entidades;
using Tesorerias;
using IU.Modulos.Comunes;
using Comunes.Entidades;
using Generales.Entidades;
using System.Collections;

namespace IU
{
    public class PaginaTesoreria : PaginaSegura
    {
        protected TESTesorerias MiTesoreria
        {
            get
            {
                if (Session[this.MiSessionPagina + "PaginaTesoreriaMiTesoreria"] != null)
                    return (TESTesorerias)Session[this.MiSessionPagina + "PaginaTesoreriaMiTesoreria"];
                else
                {
                    return (TESTesorerias)(Session[this.MiSessionPagina + "PaginaTesoreriaMiTesoreria"] = new TESTesorerias());
                }
            }
            set { Session[this.MiSessionPagina + "PaginaTesoreriaMiTesoreria"] = value; }
        }
       
        public void Guardar(string key, TESTesorerias pObj)
        {
            Session[key + "PaginaTesoreriaMiTesoreria"] = pObj;
        }

        public TESTesorerias Obtener(string key)
        {
            return (TESTesorerias)Session[key + "PaginaTesoreriaMiTesoreria"];
        }

        protected void CargarReporte(Objeto pDatosReporte, EnumTGEComprobantes pComprobante)
        {
            popUpComprobantes comprobante = (popUpComprobantes)this.MaestraPrincipal.FindControl("ContentPlaceEncabezado").FindControl("ctrPopUpComprobantes");
            comprobante.CargarReporte(pDatosReporte, pComprobante);
            UpdatePanel up = (UpdatePanel)this.MaestraPrincipal.FindControl("ContentPlaceEncabezado").FindControl("UpdatePanel2");
            up.Update();
        }

        virtual protected void PageLoadEventTesoreria(object sender, System.EventArgs e) { }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            TESTesorerias tesoreria = new TESTesorerias(); //Inicializo Instancia
            UsuarioLogueado usuarioLog = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo); //lo guardo porque la proxima ida a la DB me pisa el usuario

            tesoreria.UsuarioLogueado = usuarioLog;
            tesoreria.FechaAbrirEvento = DateTime.Now;
            tesoreria.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;
            tesoreria = TesoreriasF.TesoreriasObtenerAbierta(tesoreria);
            tesoreria.FechaAbrir = DateTime.Now;
            //ME FIJO SI HAY TESORERIA ABIERTA, por usuario/filial
            if (tesoreria.IdTesoreria != 0)
            {
                //Vuelvo a cargar el usuario Activo para mapear IdUsuarioEvento que lo necesito para obtener los datos completos de la tesoreria
                tesoreria.UsuarioLogueado = usuarioLog; 
                this.MiTesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(tesoreria); //Obtengo la tesoreria para ser usada por las paginas que Hereden this.Page
          
            }
            //SI NO EXISTE NINGUNA TESORERIA ABIERTA VOY A ABRIR
            else
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesTesoreriasAbrir.aspx"), true);
            }
            //}

            this.PageLoadEventTesoreria(sender, e);
        }
    }
}
