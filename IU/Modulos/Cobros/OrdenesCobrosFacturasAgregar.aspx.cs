using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Cobros.Entidades;
using Comunes.Entidades;
using Generales.Entidades;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosFacturasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.OrdenesDeCobroDatosAceptar += new Controles.OrdenesCobrosFacturasDatos.OrdenesDeCobroDatosAceptarEventHandler(AgregarDatos_OrdenesDeCobroDatosAceptar);
            this.AgregarDatos.OrdenesDeCobroDatosCancelar += new Controles.OrdenesCobrosFacturasDatos.OrdenesDeCobroDatosCancelarEventHandler(AgregarDatos_OrdenesDeCobroDatosCancelar);
            if (!this.IsPostBack)
            {
                CobOrdenesCobros parametro = new CobOrdenesCobros();
                //Control y Validacion de Parametros
                if (this.MisParametrosUrl.Contains("IdAfiliado"))
                    parametro.Afiliado.IdAfiliado = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"]);
                if (this.MisParametrosUrl.Contains("IdRefFacturaOCAutomatica"))
                    parametro.IdRefFacturaOCAutomatica = Convert.ToInt32(this.MisParametrosUrl["IdRefFacturaOCAutomatica"]);
                
                if (this.MisParametrosUrl.Contains("IdTipoOperacion"))
                {
                    int idTipoOperacion = Convert.ToInt32(this.MisParametrosUrl["IdTipoOperacion"]);
                    if (idTipoOperacion == (int)EnumTGETiposOperaciones.FacturaVenta
                            || idTipoOperacion == (int)EnumTGETiposOperaciones.NotaCreditoVenta)
                    {
                        parametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosFacturas;
                    }
                    else
                    {
                        parametro.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.OrdenesCobrosFacturasInternas;
                    }
                }
                this.AgregarDatos.IniciarControl(parametro, Gestion.Agregar);
            }
        }

        protected void AgregarDatos_OrdenesDeCobroDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx"), true);
        }

        protected void AgregarDatos_OrdenesDeCobroDatosAceptar(global::Cobros.Entidades.CobOrdenesCobros e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosFacturasListar.aspx"), true);
        }
    }
}