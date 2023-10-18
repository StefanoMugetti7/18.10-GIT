using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Tesorerias;
using Tesorerias.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class TesoreriasCerrar : PaginaSegura
    {
        public TESTesorerias MiTesoreria
        {
            get
            {
                if (Session[this.MiSessionPagina + "TesoreriasCerrarMiTesoreria"] != null)
                    return (TESTesorerias)Session[this.MiSessionPagina + "TesoreriasCerrarMiTesoreria"];
                else
                {
                    return (TESTesorerias)(Session[this.MiSessionPagina + "TesoreriasCerrarMiTesoreria"] = new TESTesorerias());
                }
            }
            set { Session[this.MiSessionPagina + "TesoreriasCerrarMiTesoreria"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.MiTesoreria = new TESTesorerias();
                this.MiTesoreria.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.MiTesoreria.Filial.IdFilial = UsuarioActivo.FilialPredeterminada.IdFilial;

                //Valida que no haya Tesorerias Abiertas al día de la Fecha
                //if (!TesoreriasF.TesoreriasValidarAbiertaFechaAnterior(this.MiTesoreria))
                //    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/TesoreriasCierreAnterior.aspx"), true);
                
                //Valida que la Tesoreria NO este Cerrada para la fecha
                if (!TesoreriasF.TesoreriasValidarCerrada(this.MiTesoreria))
                {
                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("CodigoMensaje", "TesoreriaValidarCerrada");
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlMensajes.aspx"), true);
                }

                this.MiTesoreria = TesoreriasF.TesoreriasObtenerAbierta(this.MiTesoreria);

                if (this.MiTesoreria.IdTesoreria == 0)
                {

                    this.MisParametrosUrl = new Hashtable();
                    this.MisParametrosUrl.Add("CodigoMensaje", "TesoreriaValidarAbierta");
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlMensajes.aspx"), true);
                }

                this.ctrTesoreriaDatos.IniciarControl(this.MiTesoreria, IU.Modulos.Tesoreria.Controles.TipoCierre.CierreDia);
            }
        }
    }
}