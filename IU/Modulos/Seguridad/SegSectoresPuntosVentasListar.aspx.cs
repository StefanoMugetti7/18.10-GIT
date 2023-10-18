using Comunes.Entidades;
using Facturas;
using Facturas.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Seguridad
{
    public partial class SegSectoresPuntosVentasListar : PaginaSegura
    {
        private DataTable MisDatosGrillas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "SectoresPuntosVentasListarMisDatosGrillas"]; }
            set { Session[this.MiSessionPagina + "SectoresPuntosVentasListarMisDatosGrillas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                this.CargarGrilla();
            }
        }

        private void CargarCombos()
        {
            this.ddlFilial.DataSource = TGEGeneralesF.FilialesObenerLista();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarGrilla()
        {
            VTASectoresPuntosVentas filtro = new VTASectoresPuntosVentas();
            filtro.Filial.IdFilial = this.ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            filtro.PuntoVenta = this.txtPuntoVenta.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPuntoVenta.Text);
            this.MisDatosGrillas = FacturasF.VTASectoresPuntosVentasObtenerListaGrilla(filtro);
            this.gvDatos.DataSource = this.MisDatosGrillas;
            this.gvDatos.DataBind();

            if (this.MisDatosGrillas.Rows.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }



        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            this.CargarGrilla();
        }


        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
           || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idsectorpuntoventa = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "IdSectorPuntoVenta", idsectorpuntoventa }
            };

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegSectoresPuntosVentasModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Seguridad/SegSectoresPuntosVentasConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = true;//this.ValidarPermiso("SegSectoresPuntosVentaModificar.aspx");
                consultar.Visible = true;//this.ValidarPermiso("SegSectoresPuntosVentaConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDatosGrillas.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTASectoresPuntosVentas parametros = this.BusquedaParametrosObtenerValor<VTASectoresPuntosVentas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTASectoresPuntosVentas>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisDatosGrillas;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            DataView dataView = new DataView(this.MisDatosGrillas);
            dataView.Sort = e.SortExpression + " " + e.SortDirection.ToString().Substring(0, 3).ToUpper();
            this.gvDatos.DataSource = dataView;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisDatosGrillas;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
    }
}
