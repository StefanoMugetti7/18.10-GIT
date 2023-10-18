using Elecciones;
using Elecciones.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Extensions.Primitives;
using Reportes.Entidades;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Documents;

namespace IU.Modulos.Elecciones
{
    public partial class EleccionesVotosGraficos : PaginaSegura
    {
        private EleListasElecciones MiListaEleccion
        {
            get { return (EleListasElecciones)Session[this.MiSessionPagina + "EleccionModificarDatosEleccion"]; }
            set { Session[this.MiSessionPagina + "EleccionModificarDatosEleccion"] = value; }
        }
        private EleElecciones MiEleccion
        {
            get { return (EleElecciones)Session[this.MiSessionPagina + "EleccionModificarDatosMiEleccion"]; }
            set { Session[this.MiSessionPagina + "EleccionModificarDatosMiEleccion"] = value; }
        }

        private DataTable MisVotos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "EleccionVotosDatosEleccion"]; }
            set { Session[this.MiSessionPagina + "EleccionVotosDatosEleccion"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                EleListasElecciones requerimiento = new EleListasElecciones();
                if (!this.MisParametrosUrl.Contains("IdEleccion"))
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesListar.aspx"), true);

                int parametro = Convert.ToInt32(this.MisParametrosUrl["IdEleccion"]);
                MiListaEleccion = new EleListasElecciones();
                MiListaEleccion.Eleccion.IdEleccion = parametro;
                EleElecciones aux = new EleElecciones();
                aux.IdEleccion = parametro;
                this.MiEleccion = EleccionesF.EleccionesObtenerDatosCompletos(aux);
                lblInfoEleccion.Text = this.MiEleccion.Anio.ToString() + " - " + this.MiEleccion.Eleccion + " ";                 
                this.CargarCombos();
            }
        }
        private void CargarGrafico()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("google.charts.load(");
            sb.Append("'current', { packages: [");
            sb.AppendLine("'corechart'] });");
            sb.Append("google.charts.setOnLoadCallback(drawChart);");
            sb.Append("function drawChart() { ");
            sb.Append("var data = google.visualization.arrayToDataTable([");
            sb.Append("['Lista','Votos']");
            foreach (DataRow item in this.MisVotos.Rows)
            {
                sb.Append(string.Format(",[('{0}'),parseInt({1})]", item.ItemArray[1].ToString(), item.ItemArray[0].ToString()));
            }
            sb.Append("]);");
            sb.Append("var options = {title: 'Resultados de Votacion',is3D: true,};");
            sb.Append("var chart = new google.visualization.PieChart(document.getElementById('piechart_3d')); chart.draw(data, options); }");
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "drawChart", sb.ToString(), true);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesListar.aspx"), true);
        }
        private void CargarCombos()
        {
            this.ddlTipoLista.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.EleccionesTiposListas);
            this.ddlTipoLista.DataValueField = "IdListaValorDetalle";
            this.ddlTipoLista.DataTextField = "Descripcion";
            this.ddlTipoLista.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlTipoLista, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlTipoLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlTipoLista.SelectedValue))
            {
                this.ddlRegion.Enabled= false;
                this.ddlRegion.Visible = false;
                this.lblRegion.Visible = false;
            }
            else
            {
                EleListasElecciones aux = new EleListasElecciones();
                aux.Eleccion.IdEleccion = MiListaEleccion.Eleccion.IdEleccion;
                aux.IdTipoLista = Convert.ToInt32(ddlTipoLista.SelectedValue);
                this.ddlRegion.Items.Clear();
                this.ddlRegion.DataSource = EleccionesF.ListasEleccionesObtenerRegiones(aux);
                this.ddlRegion.DataValueField = "IdListaValorDetalle";
                this.ddlRegion.DataTextField = "Descripcion";
                this.ddlRegion.DataBind();
                this.ddlRegion.Visible = ddlRegion.Items.Count > 0;
                this.ddlRegion.Enabled = ddlRegion.Items.Count > 0;
                this.lblRegion.Visible = ddlRegion.Items.Count > 0;

                if(ddlRegion.Items.Count == 0)
                {
                    this.MisVotos = EleccionesF.EleccionesObtenerResultadosVotacion(aux);
                    this.CargarGrafico();
                }
                else
                {
                    AyudaProgramacion.InsertarItemSeleccione(this.ddlRegion, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                }
            }
        }
        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlRegion.SelectedValue))
            {
                EleListasElecciones aux = new EleListasElecciones();
                aux.Eleccion.IdEleccion = MiListaEleccion.Eleccion.IdEleccion;
                aux.IdTipoLista = Convert.ToInt32(ddlTipoLista.SelectedValue);
                aux.IdTipoRegion = Convert.ToInt32(ddlRegion.SelectedValue);
                this.MisVotos = EleccionesF.EleccionesObtenerResultadosVotacion(aux);
                this.CargarGrafico();
            }
        }
    }
}