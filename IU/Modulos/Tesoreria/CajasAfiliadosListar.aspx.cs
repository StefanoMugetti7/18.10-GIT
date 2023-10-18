using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Afiliados.Entidades;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasAfiliadosListar : PaginaCajas
    {
        protected override void PageLoadEventCajas(object sender, EventArgs e)
        {
            base.PageLoadEventCajas(sender, e);
            this.ctrAfiliados.AfiliadosBuscarSeleccionar += new IU.Modulos.Afiliados.Controles.AfiliadosBuscar.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);

            if (!this.IsPostBack)
            {
                // Cuando salgo del Afiliado vuelvo a cargar el Menu General
                if (this.MenuPadre == EnumMenues.CajasAfiliados)
                {
                    this.MenuPadre = EnumMenues.General;
                    PaginaAfiliados paginaAfi = new PaginaAfiliados();
                    paginaAfi.Guardar(this.MiSessionPagina, new AfiAfiliados());
                }

                this.ctrAfiliados.IniciarControl(true);
            }
        }

        void ctrAfiliados_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        {
            PaginaAfiliados paginaAfi = new PaginaAfiliados();
            paginaAfi.Guardar(this.MiSessionPagina, e);

            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAfiliadosInicio.aspx"), true);
        }

        protected void btnVolver_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAutomaticos.aspx"), true);
        }
    }
}
