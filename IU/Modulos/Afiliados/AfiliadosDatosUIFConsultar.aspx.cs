using Afiliados;
using Afiliados.Entidades;
using Afiliados.Entidades.Entidades;
using Comunes.Entidades;
using System;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosDatosUIFConsultar : PaginaAfiliados
    {
        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            this.ModificarDatos.AfiliadosModificarDatosUIFAceptar += ModificarDatos_AfiliadosModificarDatosUIFAceptar;
            this.ModificarDatos.AfiliadosModificarDatosUIFCancelar += ModificarDatos_AfiliadosModificarDatosUIFCancelar;

            if (!this.IsPostBack)
            {
                AfiAfiliados afiliado = this.MiAfiliado;// new PaginaAfiliados().MiAfiliado;
                AfiAfiliadosDatosUIF afi = new AfiAfiliadosDatosUIF();
                afi.IdAfiliado = this.MiAfiliado.IdAfiliado;
                afi = AfiliadosF.AfiliadosObtenerDatosUIFPorIdAfiliado(afi);
                if (afi.IdAfiliadoDatosUIF > 0)
                    this.ModificarDatos.IniciarControl(afi, afiliado, Gestion.Consultar);
                else
                    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatosUIFModificar.aspx"), true);
            }
        }

        void ModificarDatos_AfiliadosModificarDatosUIFCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
        }

        void ModificarDatos_AfiliadosModificarDatosUIFAceptar(object sender, AfiAfiliados e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
        }
    }
}