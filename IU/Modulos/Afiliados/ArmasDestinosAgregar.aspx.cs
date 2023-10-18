using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class ArmasDestinosAgregar : PaginaSegura
    {
        
         protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ArmasDestinosModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.ArmasDestinosModificarDatos.ArmasDestinosDatosAceptarEventHandler(ModificarDatos_ArmasDestinosModificarDatosAceptar);
            this.ModificarDatos.ArmasDestinosModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.ArmasDestinosModificarDatos.ArmasDestinosDatosCancelarEventHandler(ModificarDatos_ArmasDestinosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new AfiArmasDestinos(), Gestion.Agregar);
            }
        }

        void ModificarDatos_ArmasDestinosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ArmasDestinosListar.aspx"), true);
        }

        void ModificarDatos_ArmasDestinosModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiArmasDestinos e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ArmasDestinosListar.aspx"), true);
        }
        
    }
}