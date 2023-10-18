using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.TGE
{
    public partial class FormasCobrosAgregar : PaginaSegura
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
             
                this.ModificarDatos.IniciarControl(formaCobro, Gestion.Agregar);
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