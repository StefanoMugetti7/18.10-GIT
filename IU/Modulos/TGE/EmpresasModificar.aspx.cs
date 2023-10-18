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
    public partial class EmpresasModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.EmpresaModificarDatosAceptar += new Control.EmpresasDatos.EmpresasDatosAceptarEventHandler(ModificarDatos_EmpresaModificarDatosAceptar);
            this.ModificarDatos.EmpresaModificarDatosCancelar += new Control.EmpresasDatos.EmpresasDatosCancelarEventHandler(ModificarDatos_EmpresaModificarDatosCancelar);
            
            if (!this.IsPostBack)
            {
                TGEEmpresas empresa = new TGEEmpresas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdEmpresa"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/EmpresasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdEmpresa"]);
                empresa.IdEmpresa = parametro;
                this.ModificarDatos.IniciarControl(empresa, Gestion.Modificar);
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