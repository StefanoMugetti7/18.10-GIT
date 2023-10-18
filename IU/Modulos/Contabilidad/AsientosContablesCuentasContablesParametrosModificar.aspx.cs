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
    public partial class AsientosContablesCuentasContablesParametrosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrDatos.ModificarDatosAceptar += new Controles.AsientosContablesCuentasContablesParametrosDatos.AsientosContablesCuentasContablesParametrosDatosAceptarEventHandler(ctrDatos_ModificarDatosAceptar);
            this.ctrDatos.ModificarDatosCancelar += new Controles.AsientosContablesCuentasContablesParametrosDatos.AsientosContablesCuentasContablesParametrosDatosCancelarEventHandler(ctrDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                //if (!this.MisParametrosUrl.Contains("IdAsientoContableCuentaContableParametro"))
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosContablesCuentasContablesParametrosListar.aspx"), true);

                CtbAsientosContablesCuentasContablesParametros parametro = new CtbAsientosContablesCuentasContablesParametros();
                //parametro.IdAsientoContableCuentaContableParametro = Convert.ToInt32(this.MisParametrosUrl["IdAsientoContableCuentaContableParametro"]);

                ListaParametros listaparametros = new ListaParametros(this.MiSessionPagina);
                if (!listaparametros.Existe("IdAsientoContableCuentaContableParametro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosContablesCuentasContablesParametrosListar.aspx"), true);

                int parametrovalor = listaparametros.ObtenerValor("IdAsientoContableCuentaContableParametro");
                parametro.IdAsientoContableCuentaContableParametro = parametrovalor;

                this.ctrDatos.IniciarControl(parametro, Gestion.Modificar);
                
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