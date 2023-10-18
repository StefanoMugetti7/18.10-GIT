using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Medicina
{
    public partial class PrestadoresAgregar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrModificarDatos.PrestadoresModificarDatosAceptar += new Controles.PrestadoresModificarDatos.PrestadoresModificarDatosAceptarEventHandler(ctrModificarDatos_PrestadoresModificarDatosAceptar);
            this.ctrModificarDatos.PrestadoresModificarDatosCancelar += new Controles.PrestadoresModificarDatos.PrestadoresModificarDatosCancelarEventHandler(ctrModificarDatos_PrestadoresModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ctrModificarDatos.IniciarControl(new MedPrestadores(), Gestion.Agregar);
            }
        }

        void ctrModificarDatos_PrestadoresModificarDatosAceptar(MedPrestadores e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestadoresListar.aspx"), true);
        }


        void ctrModificarDatos_PrestadoresModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestadoresListar.aspx"), true);
        }
    }
}