using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Haberes.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Haberes
{
    public partial class RemesasModificar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.RemesasModificarDatosAceptar += new IU.Modulos.Haberes.Controles.RemesasControlDatos.RemesasDatosAceptarEventHandler(ModificarDatos_RemesasModificarDatosAceptar);
            this.ModificarDatos.RemesasModificarDatosCancelar += new IU.Modulos.Haberes.Controles.RemesasControlDatos.RemesasDatosCancelarEventHandler(ModificarDatos_RemesasModificarDatosCancelar);
            if (!this.IsPostBack)
            {
                HabRemesas remesa = new HabRemesas();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("Periodo"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/RemesasListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["Periodo"]);
                remesa.Periodo = parametro;
                remesa.RemesaTipo.IdRemesaTipo = Convert.ToInt32(this.MisParametrosUrl["IdRemesaTipo"]);
                this.ModificarDatos.IniciarControl(remesa, Gestion.Modificar);
            }
        }

        void ModificarDatos_RemesasModificarDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/RemesasListar.aspx"), true);
        }

        void ModificarDatos_RemesasModificarDatosAceptar(object sender, HabRemesas e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Haberes/RemesasListar.aspx"), true);
        }
    }
}
