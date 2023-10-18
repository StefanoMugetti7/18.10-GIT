using Comunes.Entidades;
using Generales.FachadaNegocio;
using Nichos;
using Nichos.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Nichos
{
    public partial class PanteonesListar : PaginaSegura
    {
        private DataTable MisPanteones
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PanteonesListarMisPanteones"]; }
            set { Session[this.MiSessionPagina + "PanteonesListarMisPanteones"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();

                NCHPanteones parametros = this.BusquedaParametrosObtenerValor<NCHPanteones>();
                if (parametros.BusquedaParametros)
                    CargarLista(parametros);
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            NCHPanteones parametros = this.BusquedaParametrosObtenerValor<NCHPanteones>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
              || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idPanteon = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPanteon", idPanteon);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/PanteonesConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("PanteonesModificar.aspx");
                consultar.Visible = this.ValidarPermiso("PanteonesConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPanteones.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            NCHPanteones parametros = BusquedaParametrosObtenerValor<NCHPanteones>();
            parametros.IndiceColeccion = e.NewPageIndex;
            BusquedaParametrosGuardarValor<NCHPanteones>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = MisPanteones;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisPanteones = OrdenarGrillaDatos<DataTable>(MisPanteones, e);
            gvDatos.DataSource = MisPanteones;
            gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            gvDatos.AllowPaging = false;
            gvDatos.DataSource = MisPanteones;
            gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", gvDatos);
        }

        private void CargarCombos()
        {
           
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));


            ddlCementerio.DataSource = CementeriosF.CementeriosObtenerListaActiva(new NCHCementerios());
            ddlCementerio.DataValueField = "IdCementerio";
            ddlCementerio.DataTextField = "Descripcion";
            ddlCementerio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlCementerio, ObtenerMensajeSistema("SeleccioneOpcion"));


        }

        private void CargarLista(NCHPanteones pParametro)
        {
            pParametro.Descripcion = txtDescripcion.Text;
            pParametro.Codigo = txtCodigo.Text;
            pParametro.Estado.IdEstado = ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlEstado.SelectedValue);
            //  ???    pParametro.IdCementerio = ddlCementerio.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlCementerio.SelectedValue);

            

            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            
            this.BusquedaParametrosGuardarValor<NCHPanteones>(pParametro);
            this.MisPanteones = PanteonesF.PanteonesObtenerListaGrilla(pParametro);
            
            this.gvDatos.DataSource = this.MisPanteones;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisPanteones.Rows.Count > 0)          
                btnExportarExcel.Visible = true;            
            else            
                btnExportarExcel.Visible = false;
            
        }
    }
}
