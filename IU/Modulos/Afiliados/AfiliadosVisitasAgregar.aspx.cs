using Afiliados.Entidades;
using Afiliados.Entidades.Entidades;
using Comunes.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosVisitasAgregar : PaginaSegura
    {

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ModifDatos.AfiliadosModificarDatosAceptar += new IU.Modulos.Afiliados.Controles.AfiliadosVisitasDatos.AfiliadoDatosAceptarEventHandler(ModifDatos_AfiliadosModificarDatosAceptar);
            this.ModifDatos.AfiliadosModificarDatosCancelar += new IU.Modulos.Afiliados.Controles.AfiliadosVisitasDatos.AfiliadoDatosCancelarEventHandler(ModifDatos_AfiliadosModificarDatosCancelar);

            if (!this.IsPostBack)
            {
                AfiAfiliadosVisitas afiliado = new AfiAfiliadosVisitas();
                //afiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Clientes;
                this.ModifDatos.IniciarControl(afiliado, Gestion.Agregar);
            }
        }
        //void ModifDatos_AfiliadosModificarDatosAceptar(object sender, global::Afiliados.Entidades.Entidades.AfiAfiliadosVisitas e)
        //{
        //    Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosVisitasDatos.aspx"), true);
        //}

        void ModifDatos_AfiliadosModificarDatosCancelar()
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosVisitasListar.aspx"), true);
        }
        
    }
}
