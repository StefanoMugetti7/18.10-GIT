using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class FormasCobrosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.FormasCobrosModificarDatosAceptar += new Control.FormasCobrosDatos.FormasCobrosAceptarEventHandler(ModificarDatos_FormasCobrosModificarDatosAceptar);
            this.ModificarDatos.FormasCobrosModificarDatosCancelar += new Control.FormasCobrosDatos.FormasCobrosCancelarEventHandler(ModificarDatos_FormasCobrosModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                TGEFormasCobros formaCobro = new TGEFormasCobros();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdFormaCobro"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdFormaCobro"]);
                formaCobro.IdFormaCobro = parametro;
                this.ModificarDatos.IniciarControl(formaCobro, Gestion.Modificar);
            }
        }

        void ModificarDatos_FormasCobrosModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosListar.aspx"), true);
        }

        void ModificarDatos_FormasCobrosModificarDatosAceptar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasCobrosListar.aspx"), true);
        }
    }
}