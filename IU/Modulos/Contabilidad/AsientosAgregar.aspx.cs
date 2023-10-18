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
    public partial class AsientosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.AsientoContableDatosAceptar += new Controles.AsientosDatos.AsientoContableDatosAceptarEventHandler(AgregarDatos_AsientoContableDatosAceptar);
            this.AgregarDatos.AsientoContableDatosCancelar += new Controles.AsientosDatos.AsientoContableDatosCancelarEventHandler(AgregarDatos_AsientoContableDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (this.MisParametrosUrl.Contains("Gestion")
                    && (Gestion)this.MisParametrosUrl["Gestion"]==Gestion.Copiar)
                {
                    CtbAsientosContables asiento = new CtbAsientosContables();
                    asiento.IdAsientoContable = Convert.ToInt32(this.MisParametrosUrl["IdAsientoContable"]);
                    this.AgregarDatos.IniciarControl(asiento, Gestion.Copiar);
                }
                else
                    this.AgregarDatos.IniciarControl(new CtbAsientosContables(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_AsientoContableDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosListar.aspx"), true);
        }

        protected void AgregarDatos_AsientoContableDatosAceptar(object sender, global::Contabilidad.Entidades.CtbAsientosContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosListar.aspx"), true);
        }
    }
}