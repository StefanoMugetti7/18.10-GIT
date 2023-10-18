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
    public partial class AsientosContablesCuentasContablesParametrosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrDatos.ModificarDatosAceptar += new Controles.AsientosContablesCuentasContablesParametrosDatos.AsientosContablesCuentasContablesParametrosDatosAceptarEventHandler(ctrDatos_ModificarDatosAceptar);
            this.ctrDatos.ModificarDatosCancelar += new Controles.AsientosContablesCuentasContablesParametrosDatos.AsientosContablesCuentasContablesParametrosDatosCancelarEventHandler(ctrDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ctrDatos.IniciarControl(new CtbAsientosContablesCuentasContablesParametros(), Gestion.Agregar);
            }
        }

        void ctrDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosContablesCuentasContablesParametrosListar.aspx"), true);
        }

        void ctrDatos_ModificarDatosAceptar(object sender, global::Contabilidad.Entidades.CtbAsientosContablesCuentasContablesParametros e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosContablesCuentasContablesParametrosListar.aspx"), true);
        }

    }
}