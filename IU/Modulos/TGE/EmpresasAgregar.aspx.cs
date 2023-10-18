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
    public partial class EmpresasAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.EmpresaModificarDatosAceptar += new Control.EmpresasDatos.EmpresasDatosAceptarEventHandler(ModificarDatos_EmpresaModificarDatosAceptar);
            this.ModificarDatos.EmpresaModificarDatosCancelar += new Control.EmpresasDatos.EmpresasDatosCancelarEventHandler(ModificarDatos_EmpresaModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                this.ModificarDatos.IniciarControl(new TGEEmpresas(), Gestion.Agregar);
            }
        }

        void ModificarDatos_EmpresaModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/EmpresasListar.aspx"), true);
        }

        void ModificarDatos_EmpresaModificarDatosAceptar(object sender, Generales.Entidades.TGEEmpresas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/EmpresasListar.aspx"), true);
        }
    }
}