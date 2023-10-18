using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Cobros.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosAfiliadosConsultar : PaginaAfiliados
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrlDatos.OrdenesCobrosModificarDatosAceptar += new IU.Modulos.Cobros.Controles.OrdenesCobrosAfiliadosDatos.OrdenesCobrosDatosAceptarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosAceptar);
            this.ctrlDatos.OrdenesCobrosModificarDatosCancelar += new IU.Modulos.Cobros.Controles.OrdenesCobrosAfiliadosDatos.OrdenesCobrosDatosCancelarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosCancelar);
            this.ctrlCancelaciones.OrdenesCobrosModificarDatosCancelar += new Controles.CancelacionAnticipadaPrestamosCuotas.OrdenesCobrosDatosCancelarEventHandler(ctrlCancelaciones_OrdenesCobrosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdOrdenCobro") || !this.MisParametrosUrl.Contains("IdTipoOperacion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdOrdenCobro"]);
                int operacion = Convert.ToInt32(this.MisParametrosUrl["IdTipoOperacion"]);
                CobOrdenesCobros ordenCobro = new CobOrdenesCobros();
                ordenCobro.IdOrdenCobro = parametro;
                switch (operacion)
                {
                    case (int)EnumTGETiposOperaciones.OrdenesCobrosAfiliados:
                    case (int)EnumTGETiposOperaciones.NotaCreditoCargos:
                        this.ctrlDatos.Visible = true;    
                        this.ctrlDatos.IniciarControl(ordenCobro, Gestion.Consultar);
                        break;
                    case (int)EnumTGETiposOperaciones.CancelacionAnticipadaCuotaPrestamo:
                        this.ctrlCancelaciones.Visible = true;
                        this.ctrlCancelaciones.IniciarControl(ordenCobro, Gestion.Consultar);
                        break;
                    default:
                        break;
                }
            }
        }

        void ctrlCancelaciones_OrdenesCobrosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosListar.aspx"), true);
        }

        void ctrlDatos_OrdenesCobrosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosListar.aspx"), true);
        }

        //void ctrlDatos_OrdenesCobrosModificarDatosAceptar(CobOrdenesCobros e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosAfiliadosListar.aspx"), true);
        //}
    }
}
