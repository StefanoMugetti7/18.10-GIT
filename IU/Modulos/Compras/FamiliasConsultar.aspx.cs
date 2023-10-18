using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Compras.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Compras
{
    public partial class FamiliasConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ModificarDatosAceptar += new Controles.FamiliasDatos.ModificarDatosAceptarEventHandler(ModificarDatos_ModificarDatosAceptar);
            this.ModificarDatos.ModificarDatosCancelar += new Controles.FamiliasDatos.ModificarDatosCancelarEventHandler(ModificarDatos_ModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                CMPFamilias parametro = new CMPFamilias();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdFamilia"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/FamiliasListar.aspx"), true);

                int valor = Convert.ToInt32(this.MisParametrosUrl["IdFamilia"]);
                parametro.IdFamilia = valor;

                this.ModificarDatos.IniciarControl(parametro, Gestion.Consultar);
            }
        }

        void ModificarDatos_ModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/FamiliasListar.aspx"), true);
        }

        void ModificarDatos_ModificarDatosAceptar(object sender, global::Compras.Entidades.CMPFamilias e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Compras/FamiliasListar.aspx"), true);
        }
    }
}