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
    public partial class AsientosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.AsientoContableDatosAceptar += new Controles.AsientosDatos.AsientoContableDatosAceptarEventHandler(ModificarDatos_AsientoContableDatosAceptar);
            this.ModificarDatos.AsientoContableDatosCancelar += new Controles.AsientosDatos.AsientoContableDatosCancelarEventHandler(ModificarDatos_AsientoContableDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdAsientoContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosListar.aspx"), true);

                CtbAsientosContables asientoContable = new CtbAsientosContables();
                asientoContable.IdAsientoContable = Convert.ToInt32(this.MisParametrosUrl["IdAsientoContable"]);
                this.ModificarDatos.IniciarControl(asientoContable, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_AsientoContableDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosListar.aspx"), true);
        }

        protected void ModificarDatos_AsientoContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbAsientosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosListar.aspx"), true);
        }
    }
}