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
    public partial class DepartamentosModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.DepartamentoDatosAceptar += new Controles.DepartamentosDatos.DepartamentoDatosAceptarEventHandler(ModificarDatos_DepartamentoDatosAceptar);
            this.ModificarDatos.DepartamentoDatosCancelar += new Controles.DepartamentosDatos.DepartamentoDatosCancelarEventHandler(ModificarDatos_DepartamentoDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdDepartamento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosListar.aspx"), true);

                CtbDepartamentos departamento = new CtbDepartamentos();
                departamento.IdDepartamento = Convert.ToInt32(this.MisParametrosUrl["IdDepartamento"]);
                this.ModificarDatos.IniciarControl(departamento, Gestion.Modificar);
            }
        }

        protected void ModificarDatos_DepartamentoDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosListar.aspx"), true);
        }

        protected void ModificarDatos_DepartamentoDatosAceptar(object sender, global::Contabilidad.Entidades.CtbDepartamentos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosListar.aspx"), true);
        }
    }
}