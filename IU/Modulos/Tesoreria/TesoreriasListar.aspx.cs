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
using Tesorerias.Entidades;
using Tesorerias;
using System.Collections.Generic;

namespace IU.Modulos.Tesoreria
{
    public partial class TesoreriasListar : PaginaSegura
    {
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            { }
        }

        private void CargarCombos()
        {
            this.ddlAgencias.DataSource = this.UsuarioActivo.Filiales;
            this.ddlAgencias.DataValueField = "IdFilial";
            this.ddlAgencias.DataTextField = "Filial";
            this.ddlAgencias.DataBind();
            this.ddlAgencias.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            if (this.ddlAgencias.Items.Count == 1)
                this.ddlAgencias_SelectedIndexChanged(null, EventArgs.Empty);

        }

        private void CargarLista(TESTesorerias pTesoreria)
        {
            pTesoreria = TesoreriasF.TesoreriasObtenerDatosCompletos(pTesoreria);
            List<TESTesorerias> lista = new List<TESTesorerias>();
            lista.Add(pTesoreria);
            this.gvDatosCabecera.DataSource = lista;
            this.gvDatosCabecera.DataBind();

            this.gvDatos.DataSource = pTesoreria.TesoreriasMonedas;
            this.gvDatos.DataBind();
        }

        protected void ddlAgencias_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlAgencias.SelectedValue))
            {
                TESTesorerias teso = new TESTesorerias();
                teso.IdTesoreria = Convert.ToInt32(this.ddlAgencias.SelectedValue);
                teso.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                this.CargarLista(teso);
            }
        }
    }
}
