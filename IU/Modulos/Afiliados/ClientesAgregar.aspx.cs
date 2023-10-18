using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;
using System.Collections;

namespace IU.Modulos.Afiliados
{
    public partial class ClientesAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModifDatos.AfiliadosModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.ClientesModificarDatos.AfiliadoDatosAceptarEventHandler(ModifDatos_AfiliadosModificarDatosAceptar);
            this.ModifDatos.AfiliadosModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.ClientesModificarDatos.AfiliadoDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                AfiAfiliados afiliado = new AfiAfiliados();
                afiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Clientes;
                this.ModifDatos.IniciarControl(afiliado, Gestion.Agregar);
            }
        }

        void ModifDatos_AfiliadosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesListar.aspx"), true);
        }

        void ModifDatos_AfiliadosModificarDatosAceptar(object sender, global::Afiliados.Entidades.AfiAfiliados e)
        {
            //Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesListar.aspx"), true);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", Gestion.Modificar);
            this.MisParametrosUrl.Add("IdAfiliado", e.IdAfiliado);

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesModificar.aspx"), true);
        }
    }
}
