using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Compras.Entidades;

namespace IU.Modulos.Compras
{
    public partial class InformesRecepcionesAnular : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.InformesRecepcionesModificarDatosAceptar += new Controles.InformesRecepcionesDatos.InformesRecepcionesDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.InformesRecepcionesModificarDatosCancelar += new Controles.InformesRecepcionesDatos.InformesRecepcionesDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CmpInformesRecepciones informe = new CmpInformesRecepciones();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdInformeRecepcion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdInformeRecepcion"]);
                informe.IdInformeRecepcion = parametro;

                this.ModificarDatos.IniciarControl(informe, Gestion.Anular);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesListar.aspx"), true);
        }

        void ModificarDatos_ModificarDatosAceptar(object sender, global::Compras.Entidades.CmpInformesRecepciones e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/InformesRecepcionesListar.aspx"), true);
        }
    }
}