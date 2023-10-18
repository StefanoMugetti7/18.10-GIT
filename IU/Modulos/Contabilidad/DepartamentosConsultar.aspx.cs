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
    public partial class DepartamentosConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ConsultarDatos.DepartamentoDatosAceptar += new Controles.DepartamentosDatos.DepartamentoDatosAceptarEventHandler(ConsultarDatos_DepartamentoDatosAceptar);
            this.ConsultarDatos.DepartamentoDatosCancelar += new Controles.DepartamentosDatos.DepartamentoDatosCancelarEventHandler(ConsultarDatos_DepartamentoDatosCancelar);
            if (!this.IsPostBack)
            {
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdDepartamento"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosListar.aspx"), true);

                CtbDepartamentos departamento = new CtbDepartamentos();
                departamento.IdDepartamento = Convert.ToInt32(this.MisParametrosUrl["IdDepartamento"]);
                this.ConsultarDatos.IniciarControl(departamento, Gestion.Consultar);
            }
        }

        protected void ConsultarDatos_DepartamentoDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosListar.aspx"), true);
        }

        protected void ConsultarDatos_DepartamentoDatosAceptar(object sender, global::Contabilidad.Entidades.CtbDepartamentos e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/DepartamentosListar.aspx"), true);
        }
    }
}