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
using Prestamos.Entidades;
using System.Collections.Generic;
using Comunes.Entidades;
using Prestamos;
using Generales.FachadaNegocio;

namespace IU.Modulos.Prestamos
{
    public partial class PlanesListar : PaginaSegura
    {
        private List<PrePrestamosPlanes> MisPrestamosPlanes
        {
            get { return (List<PrePrestamosPlanes>)Session[this.MiSessionPagina + "PlanesListarMisPrestamosPlanes"]; }
            set { Session[this.MiSessionPagina + "PlanesListarMisPrestamosPlanes"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("PlanesAgregar.aspx");
                this.CargarCombos();
                PrePrestamosPlanes parametros = this.BusquedaParametrosObtenerValor<PrePrestamosPlanes>();
          
                if (parametros.BusquedaParametros)
                {
                    this.txtPlan.Text = parametros.Descripcion.ToString();
              
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                 
                    this.ddlMoneda.SelectedValue = parametros.Moneda.IdMoneda.ToString();
                    this.CargarLista(parametros);

                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PrePrestamosPlanes parametros = this.BusquedaParametrosObtenerValor<PrePrestamosPlanes>();
            this.CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PlanesAgregar.aspx"), true);
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                   || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            PrePrestamosPlanes plan = this.MisPrestamosPlanes[indiceColeccion];
            //string parametros = string.Format("?IdPlazo={0}", plan.IdPlazos);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPrestamoPlan", plan.IdPrestamoPlan);
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PlanesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PlanesConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestamosPlanes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrePrestamosPlanes parametros = this.BusquedaParametrosObtenerValor<PrePrestamosPlanes>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<PrePrestamosPlanes>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisPrestamosPlanes;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPrestamosPlanes = this.OrdenarGrillaDatos<PrePrestamosPlanes>(this.MisPrestamosPlanes, e);
            this.gvDatos.DataSource = this.MisPrestamosPlanes;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";

            this.ddlMoneda.DataSource = TGEGeneralesF.MonedasObtenerListaActiva();
            this.ddlMoneda.DataValueField = "IdMoneda";
            this.ddlMoneda.DataTextField = "Descripcion";
            this.ddlMoneda.DataBind();
            if (this.ddlMoneda.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }
         

        private void CargarLista(PrePrestamosPlanes pPrestamoPlan)
        {
            pPrestamoPlan.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pPrestamoPlan.Moneda.IdMoneda = this.ddlMoneda.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMoneda.SelectedValue);
            pPrestamoPlan.Descripcion = this.txtPlan.Text.Trim();
            pPrestamoPlan.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<PrePrestamosPlanes>(pPrestamoPlan);
            this.MisPrestamosPlanes = PrePrestamosF.PrestamosPlanesObtenerLista(pPrestamoPlan);
            this.gvDatos.DataSource = this.MisPrestamosPlanes;
            this.gvDatos.PageIndex = pPrestamoPlan.IndiceColeccion;
            this.gvDatos.DataBind();


        }
    }
}