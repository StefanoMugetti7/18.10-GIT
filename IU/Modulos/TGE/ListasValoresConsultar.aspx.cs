using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Generales.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class ListasValoresConsultar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ModificarDatos.ListasValoresDatosCancelar += new IU.Modulos.TGE.Control.ListasValoresDetalles.ListasValoresDatosCancelarEventHandler(ModificarDatos_ListasValoresDatosCancelar);
            if (!this.IsPostBack)
            {
                TGEListasValores lista = new TGEListasValores();
                //Control y Validacion de Parametros
                if (!this.MisParametrosUrl.Contains("IdListaValor"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ListasValoresListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdListaValor"]);
                lista.IdListaValor = parametro;
                this.ModificarDatos.IniciarControl(lista, Gestion.Consultar);
            }
        }

        void ModificarDatos_ListasValoresDatosCancelar()
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/ListasValoresListar.aspx"), true);
        }
    }
}