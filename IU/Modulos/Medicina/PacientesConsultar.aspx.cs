using Afiliados.Entidades;
using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Medicina
{
    public partial class PacientesConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.AfiliadosModificarDatosAceptar += new IU.Modulos.Medicina.Controles.PacientesDatos.AfiliadoDatosAceptarEventHandler(ModifDatos_AfiliadosModificarDatosAceptar);
            this.ModifDatos.AfiliadosModificarDatosCancelar += new IU.Modulos.Medicina.Controles.PacientesDatos.AfiliadoDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                AfiPacientes afiliado = new AfiPacientes();
                if (!this.MisParametrosUrl.Contains("IdAfiliado"))
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdAfiliado"]);
                afiliado.IdAfiliado = parametro;
                afiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Clientes;
                this.ModifDatos.IniciarControl(afiliado, Gestion.Consultar);

                if (this.MisParametrosUrl.Contains("Atender") && Convert.ToBoolean(this.MisParametrosUrl["Atender"])==true)
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestacionesAgregar.aspx"), true);
            }
        }

        void ModifDatos_AfiliadosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesListar.aspx"), true);
        }

        void ModifDatos_AfiliadosModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiPacientes e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesListar.aspx"), true);
        }
    }
}