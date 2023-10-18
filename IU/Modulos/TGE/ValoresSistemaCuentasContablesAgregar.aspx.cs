using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Comunes.Entidades;
using Contabilidad.Entidades;

namespace IU.Modulos.TGE
{
    public partial class ValoresSistemaCuentasContablesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ValoresSistemasCuentasContablesDatosCancelar += new IU.Modulos.TGE.Control.ValoresSistemaCuentasContablesDatos.ValoresSistemasCuentasContablesDatosCancelarEventHandler(ModificarDatos_ListasValoresDatosCancelar);
            this.ModificarDatos.ValoresSistemasCuentasContablesDatosAceptar += new IU.Modulos.TGE.Control.ValoresSistemaCuentasContablesDatos.ValoresSistemasCuentasContablesDatosAceptarEventHandler(ModificarDatos_ListasValoresDatosAceptar);
            if (!this.IsPostBack)
            {
                TGEListasValoresSistemasDetallesCuentasContables lista = new TGEListasValoresSistemasDetallesCuentasContables();

                this.ModificarDatos.IniciarControl(lista, Gestion.Agregar);
            }
        }

        void ModificarDatos_ListasValoresDatosAceptar(object sender, TGEListasValoresSistemasDetallesCuentasContables e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ValoresSistemaCuentasContablesListar.aspx"), true);
        }

        void ModificarDatos_ListasValoresDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ValoresSistemaCuentasContablesListar.aspx"), true);
        }
    }
}