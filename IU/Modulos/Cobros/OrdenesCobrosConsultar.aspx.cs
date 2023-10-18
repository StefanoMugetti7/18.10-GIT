using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Cobros.Entidades;
using Cobros;
using Comunes.Entidades;

namespace IU.Modulos.Cobros
{
    public partial class OrdenesCobrosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrlDatos.OrdenesCobrosModificarDatosAceptar += new IU.Modulos.Cobros.Controles.OrdenesCobrosDatos.OrdenesCobrosDatosAceptarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosAceptar);
            this.ctrlDatos.OrdenesCobrosModificarDatosCancelar += new IU.Modulos.Cobros.Controles.OrdenesCobrosDatos.OrdenesCobrosDatosCancelarEventHandler(ctrlDatos_OrdenesCobrosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdOrdenCobro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdOrdenCobro"]);
                CobOrdenesCobros ordenCobro=new CobOrdenesCobros();
                ordenCobro.IdOrdenCobro = parametro;
                //ordenCobro = CobrosF.OrdenesCobrosObtenerDatosCompletos(ordenCobro);
                this.ctrlDatos.IniciarControl(ordenCobro, Gestion.Consultar);
                //this.ctrlDatosFormasCobros.IniciarControl(ordenCobro, null, Gestion.Consultar); 
            }
        }

        void ctrlDatos_OrdenesCobrosModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosListar.aspx"), true);
        }

        //void ctrlDatos_OrdenesCobrosModificarDatosAceptar(CobOrdenesCobros e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Cobros/OrdenesCobrosListar.aspx"), true);
        //}
    }
}
