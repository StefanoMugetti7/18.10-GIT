using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class FilialesConsultar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.FilialModificarDatosAceptar += new Control.FilialesDatos.FilialesDatosAceptarEventHandler(ModificarDatos_FilialModificarDatosAceptar);
            this.ModificarDatos.FilialModificarDatosCancelar += new Control.FilialesDatos.FilialesDatosCancelarEventHandler(ModificarDatos_FilialModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TGEFiliales filial = new TGEFiliales();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdFilial"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FilialesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdFilial"]);
                filial.IdFilial = parametro;
                this.ModificarDatos.IniciarControl(filial, Gestion.Consultar);
            }
        }

        void ModificarDatos_FilialModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FilialesListar.aspx"), true);
        }

        void ModificarDatos_FilialModificarDatosAceptar(object sender, TGEFiliales e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FilialesListar.aspx"), true);
        }
    }
}
