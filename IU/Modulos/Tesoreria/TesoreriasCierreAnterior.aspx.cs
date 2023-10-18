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
using Tesorerias.Entidades;
using System.Collections.Generic;
using Tesorerias;

namespace IU.Modulos.Tesoreria
{
    public partial class TesoreriasCierreAnterior : PaginaSegura
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

                this.MiTesoreria = TesoreriasF.TesoreriasObtenerAbierta(this.MiTesoreria);

                this.ctrTesoreriaDatos.IniciarControl(this.MiTesoreria, IU.Modulos.Tesoreria.Controles.TipoCierre.CierreAnterior);
            }
        }
     }
}
