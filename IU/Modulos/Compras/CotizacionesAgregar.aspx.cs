using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Compras
{
    public partial class CotizacionesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.CotizacionesModificarDatosAceptar += new Controles.CotizacionesDatos.CotizacionesDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.CotizacionesModificarDatosCancelar += new Controles.CotizacionesDatos.CotizacionesDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new CmpCotizaciones(), Gestion.Agregar);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesListar.aspx"), true);
        }

        void ModificarDatos_ModificarDatosAceptar(object sender, global::Compras.Entidades.CmpCotizaciones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesListar.aspx"), true);
        }
    }
}