using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class SectoresAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.SectorModificarDatosAceptar += new Control.SectoresDatos.SectoresDatosAceptarEventHandler(ModificarDatos_SectorModificarDatosAceptar);
            this.ModificarDatos.SectorModificarDatosCancelar += new Control.SectoresDatos.SectoresDatosCancelarEventHandler(ModificarDatos_SectorModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                
                this.ModificarDatos.IniciarControl(new TGESectores(), Gestion.Agregar);
            }
        }

        void ModificarDatos_SectorModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/SectoresListar.aspx"), true);
        }

        void ModificarDatos_SectorModificarDatosAceptar(object sender, TGESectores e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/SectoresListar.aspx"), true);
        }
    }
}
