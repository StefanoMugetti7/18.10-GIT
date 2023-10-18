using Comunes.Entidades;
using Prestamos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Prestamos
{
    public partial class PrestamosAfiliadosAgregarManual : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.AgregarDatos.PrestamosAfiliadosModificarDatosAceptar += new IU.Modulos.Prestamos.Controles.PrestamoAfiliadoModificarDatos.PrestamosAfiliadosDatosAceptarEventHandler(AgregarDatos_PrestamosAfiliadosModificarDatosAceptar);
            this.AgregarDatos.PrestamosAfiliadosModificarDatosCancelar += new IU.Modulos.Prestamos.Controles.PrestamoAfiliadoModificarDatos.PrestamosAfiliadosDatosCancelarEventHandler(AgregarDatos_PrestamosAfiliadosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                PrePrestamos prestamos = new PrePrestamos();
                prestamos.Afiliado = this.MiAfiliado;
                this.AgregarDatos.IniciarControl(prestamos, Gestion.Agregar, IU.Modulos.Prestamos.Controles.PrestamoAfiliadoModificarDatos.TipoAutorizar.SinPrivilegio, Generales.Entidades.EnumTGETiposOperaciones.PrestamosManual);
            }
        }

        protected void AgregarDatos_PrestamosAfiliadosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosListar.aspx"), true);
        }

        protected void AgregarDatos_PrestamosAfiliadosModificarDatosAceptar(object sender, PrePrestamos e)
        {

            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosListar.aspx"), true);
        }
    }
}