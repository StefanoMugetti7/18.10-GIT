using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Subsidios.Entidades;
using System.Collections;
using Subsidios;
using Generales.FachadaNegocio;

namespace IU.Modulos.Subsidios
{
    public partial class SubsidiosListar : PaginaSegura
    {
        private List<SubSubsidios> MisSubsidios
        {
            get { return (List<SubSubsidios>)Session[this.MiSessionPagina + "SubsidiosListarMisSubsidios"]; }
            set { Session[this.MiSessionPagina + "SubsidiosListarMisSubsidios"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible =  this.ValidarPermiso("SubsidiosAgregar.aspx");
                this.CargarCombos();
                SubSubsidios parametros = this.BusquedaParametrosObtenerValor<SubSubsidios>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtSubsidio.Text = parametros.Descripcion;
                    //this.txtDenominacion.Text = parametros.Denominacion;
                    //this.ddlBancos.SelectedValue = parametros.Banco.IdBanco.ToString();
                    //this.ddlFiliales.SelectedValue = parametros.Filial.IdFilial.ToString();
                    //this.ddlMonedas.SelectedValue = parametros.Moneda.IdMoneda.ToString();
                    //this.ddlEstado.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
      


            protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SubsidiosAgregar.aspx"), true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            SubSubsidios parametros = this.BusquedaParametrosObtenerValor<SubSubsidios>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            SubSubsidios subsidio = this.MisSubsidios[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdSubsidio", subsidio.IdSubsidio);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SubsidiosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Subsidios/SubsidiosConsultar.aspx"), true);
            }
            
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                SubSubsidios subsidio = (SubSubsidios)e.Row.DataItem;

                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                consultar.Visible = this.ValidarPermiso("SubsidiosConsultar.aspx");

                switch (subsidio.Estado.IdEstado)
                {
                    case (int)Estados.Activo:
                        break;
                    case (int)Estados.Baja:
                        modificar.Visible = this.ValidarPermiso("SubsidiosModificar.aspx");
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisSubsidios.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SubSubsidios parametros = this.BusquedaParametrosObtenerValor<SubSubsidios>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<SubSubsidios>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisSubsidios;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisSubsidios = this.OrdenarGrillaDatos<SubSubsidios>(this.MisSubsidios, e);
            this.gvDatos.DataSource = this.MisSubsidios;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
        }

        private void CargarLista(SubSubsidios pParametro)
        {
            pParametro.Descripcion = this.txtSubsidio.Text;
            //pParametro.Denominacion = this.txtDenominacion.Text;
            //pParametro.Banco.IdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
            //pParametro.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            //pParametro.Moneda.IdMoneda = this.ddlMonedas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMonedas.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<SubSubsidios>(pParametro);
            this.MisSubsidios = SubsidiosF.SubsidiosObtenerListaFiltro(pParametro);
            this.gvDatos.DataSource = this.MisSubsidios;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
