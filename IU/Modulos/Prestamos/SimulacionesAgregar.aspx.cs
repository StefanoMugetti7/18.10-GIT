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
    public partial class SimulacionesAgregar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            //this.AgregarDatos.PrestamosAfiliadosModificarDatosAceptar +=new IU.Modulos.Prestamos.Controles.PrestamoSimulacionModificarDatos.PrestamosAfiliadosDatosAceptarEventHandler(AgregarDatos_PrestamosAfiliadosModificarDatosAceptar);
            this.AgregarDatos.PrestamosAfiliadosModificarDatosCancelar+=new IU.Modulos.Prestamos.Controles.PrestamoSimulacionModificarDatos.PrestamosAfiliadosDatosCancelarEventHandler(AgregarDatos_PrestamosAfiliadosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                PrePrestamos prestamos = new PrePrestamos();
                prestamos.Afiliado = this.MiAfiliado;
                this.AgregarDatos.IniciarControl(prestamos, Gestion.Agregar);
            }
        }

        protected void AgregarDatos_PrestamosAfiliadosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/SimulacionesListar.aspx"), true);
        }

        //protected void AgregarDatos_PrestamosAfiliadosModificarDatosAceptar(object sender, PrePrestamos e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/SimulacionesListar.aspx"), true);
        //}
    }
}
