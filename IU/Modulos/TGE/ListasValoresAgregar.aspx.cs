using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Generales.Entidades;
using Comunes.Entidades;
using System.Collections;

namespace IU.Modulos.TGE
{
    public partial class ListasValoresAgregar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ListasValoresDatosCancelar += new IU.Modulos.TGE.Control.ListasValoresDetalles.ListasValoresDatosCancelarEventHandler(ModificarDatos_ListasValoresDatosCancelar);
            this.ModificarDatos.ListasValoresDatosAceptar += new IU.Modulos.TGE.Control.ListasValoresDetalles.ListasValoresDatosAceptarEventHandler(ModificarDatos_ListasValoresDatosAceptar);
            if (!this.IsPostBack)
            {
                TGEListasValores lista = new TGEListasValores();

                this.ModificarDatos.IniciarControl(lista, Gestion.Agregar);
            }
        }

        void ModificarDatos_ListasValoresDatosAceptar(object sender, TGEListasValores e)
        {
            //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ListasValoresListar.aspx"), true);

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", Gestion.Modificar);
            this.MisParametrosUrl.Add("IdListaValor", e.IdListaValor);

            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ListasValoresModificar.aspx"), true);
        }

        void ModificarDatos_ListasValoresDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ListasValoresListar.aspx"), true);
        }
    }
}