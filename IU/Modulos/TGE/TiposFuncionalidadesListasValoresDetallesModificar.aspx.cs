using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class TiposFuncionalidadesListasValoresDetallesModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrModifDatos.ModificarDatosAceptar += new Control.TiposFuncionalidadesListasValoresDetallesDatos.ModificarDatosAceptarEventHandler(ctrModifDatos_ModificarDatosAceptar);
            this.ctrModifDatos.ModificarDatosCancelar += new Control.TiposFuncionalidadesListasValoresDetallesDatos.ModificarDatosCancelarEventHandler(ctrModifDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ctrModifDatos.IniciarControl(Gestion.Modificar);
            }
        }

        void ctrModifDatos_ModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
        }

        void ctrModifDatos_ModificarDatosAceptar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/IngresoSistema.aspx"), true);
        }
    }
}
