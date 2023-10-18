using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Contabilidad
{
    public partial class DepartamentosAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.AgregarDatos.DepartamentoDatosAceptar += new Controles.DepartamentosDatos.DepartamentoDatosAceptarEventHandler(AgregarDatos_DepartamentoDatosAceptar);
            this.AgregarDatos.DepartamentoDatosCancelar += new Controles.DepartamentosDatos.DepartamentoDatosCancelarEventHandler(AgregarDatos_DepartamentoDatosCancelar);
            if (!this.IsPostBack)
            {
                this.AgregarDatos.IniciarControl(new CtbDepartamentos(), Gestion.Agregar);
            }
        }

        protected void AgregarDatos_DepartamentoDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosListar.aspx"), true);
        }

        protected void AgregarDatos_DepartamentoDatosAceptar(object sender, global::Contabilidad.Entidades.CtbDepartamentos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosListar.aspx"), true);
        }
    }
}