using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.Entidades;
using Contabilidad.Entidades;

namespace IU.Modulos.TGE
{
    public partial class ValoresSistemaCuentasContablesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            
            this.ModificarDatos.ValoresSistemasCuentasContablesDatosCancelar += new IU.Modulos.TGE.Control.ValoresSistemaCuentasContablesDatos.ValoresSistemasCuentasContablesDatosCancelarEventHandler(ModificarDatos_ListasValoresSistemasCuentasContablesDatosCancelar);
            if (!this.IsPostBack)
            {
                TGEListasValoresSistemasDetallesCuentasContables lista = new TGEListasValoresSistemasDetallesCuentasContables();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdListaValorSistemaDetalleCuentaContable"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ValoresSistemaCuentasContablesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdListaValorSistemaDetalleCuentaContable"]);
                lista.IdListaValorSistemaDetalleCuentaContable = parametro;
                this.ModificarDatos.IniciarControl(lista, Gestion.Consultar);
            }
        }

       
        void ModificarDatos_ListasValoresSistemasCuentasContablesDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ValoresSistemaCuentasContablesListar.aspx"), true);
        }
    }
}