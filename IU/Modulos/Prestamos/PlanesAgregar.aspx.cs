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
    public partial class PlanesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.PlanesDatosAceptar += new IU.Modulos.Prestamos.Controles.PlanesDatos.PlanesDatosAceptarEventHandler(AgregarDatos_PlanesDatosAceptar);
            this.AgregarDatos.PlanesDatosCancelar += new IU.Modulos.Prestamos.Controles.PlanesDatos.PlanesDatosCancelarEventHandler(AgregarDatos_PlanesDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new PrePrestamosPlanes(), Gestion.Agregar);
            }
        }

        void AgregarDatos_PlanesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PlanesListar.aspx"), true);
        }

        void AgregarDatos_PlanesDatosAceptar(object sender, global::Prestamos.Entidades.PrePrestamosPlanes e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PlanesListar.aspx"), true);
        }
    }
}
