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
    public partial class CotizacionesAutorizar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.CotizacionesModificarDatosAceptar += new Controles.CotizacionesDatos.CotizacionesDatosAceptarEventHandler(ModificarDatos_CotizacionesModificarDatosAceptar);
            this.ModificarDatos.CotizacionesModificarDatosCancelar += new Controles.CotizacionesDatos.CotizacionesDatosCancelarEventHandler(ModificarDatos_CotizacionesModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                CmpCotizaciones cotizacion = new CmpCotizaciones();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdCotizacion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdCotizacion"]);
                cotizacion.IdCotizacion = parametro;

                this.ModificarDatos.IniciarControl(cotizacion, Gestion.Autorizar);
            }
        }

        void ModificarDatos_CotizacionesModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesListar.aspx"), true);
        }

        void ModificarDatos_CotizacionesModificarDatosAceptar(object sender, global::Compras.Entidades.CmpCotizaciones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/CotizacionesListar.aspx"), true);
        }
    }
}