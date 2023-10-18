using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cobros.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;

namespace IU.Modulos.Cobros
{
    public partial class CancelacionAnticipadaPrestamosCuotas : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrlDatos.OrdenesCobrosModificarDatosCancelar += new IU.Modulos.Cobros.Controles.CancelacionAnticipadaPrestamosCuotas.OrdenesCobrosDatosCancelarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
                ordenCobro.Afiliado = this.MiAfiliado;
                ordenCobro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.CancelacionAnticipadaCuotaPrestamo);
                this.ctrlDatos.IniciarControl(ordenCobro, Gestion.Agregar);
            }
        }

        void ctrlDatos_OrdenesCobrosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosListar.aspx"), true);
        }

    }
}
