using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Contabilidad
{
    public partial class AsientosModelosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.AsientoModeloDatosAceptar += new Controles.AsientosModelosDatos.AsientoModeloDatosAceptarEventHandler(AgregarDatos_AsientoModeloDatosAceptar);
            this.AgregarDatos.AsientoModeloDatosCancelar += new Controles.AsientosModelosDatos.AsientoModeloDatosCancelarEventHandler(AgregarDatos_AsientoModeloDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbAsientosModelos(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_AsientoModeloDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosListar.aspx"), true);
        }

        protected void AgregarDatos_AsientoModeloDatosAceptar(object sender, CtbAsientosModelos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosModelosListar.aspx"), true);
        }
    }
}