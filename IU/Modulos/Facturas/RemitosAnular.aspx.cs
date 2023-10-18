using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Facturas.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Facturas
{
    public partial class RemitosAnular : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModificarDatos.RemitosModificarDatosAceptar += new Controles.RemitosDatos.RemitosDatosAceptarEventHandler(ModificarDatos_RemitoModificarDatosAceptar);
            this.ModificarDatos.RemitosModificarDatosCancelar += new Controles.RemitosDatos.RemitosDatosCancelarEventHandler(ModificarDatos_RemitoModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                VTARemitos Remito = new VTARemitos();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdRemito"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdRemito"]);
                Remito.IdRemito = parametro;

                this.ModificarDatos.IniciarControl(Remito, Gestion.Anular);
            }
        }

        void ModificarDatos_RemitoModificarDatosCancelar()
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosListar.aspx"), true);
        }

        //void ModificarDatos_RemitoModificarDatosAceptar(object sender, global::Facturas.Entidades.VTARemitos e)
        //{
        //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/RemitosListar.aspx"), true);
        //}
    }
}