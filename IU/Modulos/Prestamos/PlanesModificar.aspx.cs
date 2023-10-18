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
using Prestamos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Prestamos
{
    public partial class PlanesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.PlanesDatosAceptar += new IU.Modulos.Prestamos.Controles.PlanesDatos.PlanesDatosAceptarEventHandler(ModificarDatos_PlanesDatosAceptar);
            this.ModificarDatos.PlanesDatosCancelar += new IU.Modulos.Prestamos.Controles.PlanesDatos.PlanesDatosCancelarEventHandler(ModificarDatos_PlanesDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdPrestamoPlan"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PlanesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdPrestamoPlan"]);
                PrePrestamosPlanes plan = new PrePrestamosPlanes();
                plan.IdPrestamoPlan = parametro;
                this.ModificarDatos.IniciarControl(plan, Gestion.Modificar);
            }
        }

        void ModificarDatos_PlanesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PlanesListar.aspx"), true);
        }

        void ModificarDatos_PlanesDatosAceptar(object sender, global::Prestamos.Entidades.PrePrestamosPlanes e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PlanesListar.aspx"), true);
        }
    }
}