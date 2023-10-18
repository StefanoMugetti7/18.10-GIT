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
using Generales.FachadaNegocio;
using System.Collections.Generic;
using Comunes.Entidades;

namespace IU.Modulos.TGE
{
    public partial class FormasPagosAfiliadosListar : PaginaAfiliados
    {
        private List<TGEFormasPagosAfiliados> MisFormasPagosAfiliados
        {
            get { return (List<TGEFormasPagosAfiliados>)Session[this.MiSessionPagina + "FormasPagosAfiliadosListarMisFormasPagosAfiliados"]; }
            set { Session[this.MiSessionPagina + "FormasPagosAfiliadosListarMisFormasPagosAfiliados"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                //this.btnAgregar.Visible = this.ValidarPermiso("FormasPagosAfiliadosAgregar.aspx");
                this.CargarLista();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasPagosAfiliadosAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            TGEFormasPagosAfiliados cobroAfiliado = this.MisFormasPagosAfiliados[indiceColeccion];
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdFormaPagoAfiliado", cobroAfiliado.IdFormaPagoAfiliado);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasPagosAfiliadosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/TGE/FormasPagosAfiliadosConsultar.aspx"), true);
            }

        }

        private void CargarLista()
        {
            TGEFormasPagosAfiliados parametro = new TGEFormasPagosAfiliados();
            parametro.IdAfiliado = this.MiAfiliado.IdAfiliado;

            this.MisFormasPagosAfiliados = TGEGeneralesF.FormasPagosAfiliadosObtenerListaFiltro(parametro);

            if (this.MisFormasPagosAfiliados.Count > 0)
            {
                this.gvDatos.DataSource = this.MisFormasPagosAfiliados;
                this.gvDatos.DataBind();
            }
            else
            {
                this.btnAgregar.Visible = this.ValidarPermiso("FormasPagosAfiliadosAgregar.aspx");
            }
        }
    }
}
