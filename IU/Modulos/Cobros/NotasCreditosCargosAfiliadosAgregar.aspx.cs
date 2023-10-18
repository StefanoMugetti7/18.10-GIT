using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Cobros.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.Cobros
{
    public partial class NotasCreditosCargosAfiliadosAgregar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrlDatos.OrdenesCobrosModificarDatosAceptar += new IU.Modulos.Cobros.Controles.OrdenesCobrosAfiliadosDatos.OrdenesCobrosDatosAceptarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosAceptar);
            this.ctrlDatos.OrdenesCobrosModificarDatosCancelar += new IU.Modulos.Cobros.Controles.OrdenesCobrosAfiliadosDatos.OrdenesCobrosDatosCancelarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
                ordenCobro.Afiliado = this.MiAfiliado;
                ordenCobro.TipoOperacion = TGEGeneralesF.TiposOperacionesObtenerDatosCompletos(EnumTGETiposOperaciones.NotaCreditoCargos);
                this.ctrlDatos.IniciarControl(ordenCobro, Gestion.Agregar);
            }
        }

        void ctrlDatos_OrdenesCobrosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosListar.aspx"), true);
        }

        //void ctrlDatos_OrdenesCobrosModificarDatosAceptar(CobOrdenesCobros e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosListar.aspx"), true);
        //}
    }
}
