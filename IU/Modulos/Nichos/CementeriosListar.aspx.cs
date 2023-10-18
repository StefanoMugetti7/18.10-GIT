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
    public partial class CementeriosListar : PaginaSegura
    {
        private DataTable MisCementerios
        {
            get { return (DataTable)Session[this.MiSessionPagina + "CementeriosListarMisCementerios"]; }
            set { Session[this.MiSessionPagina + "CementeriosListarMisCementerios"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();

                NCHCementerios parametros = this.BusquedaParametrosObtenerValor<NCHCementerios>();
                if (parametros.BusquedaParametros)
                {
                    txtDescripcion.Text = parametros.Descripcion;
                    txtCodigo.Text = parametros.Codigo;
                    txtDomicilio.Text = parametros.Domicilio;
                    CargarLista(parametros);
                }
            }


        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            NCHCementerios parametros = this.BusquedaParametrosObtenerValor<NCHCementerios>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/CementeriosAgregar.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idCementerio = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCementerio", idCementerio);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/CementeriosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/CementeriosConsultar.aspx"), true);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("CementeriosModificar.aspx");
                consultar.Visible = this.ValidarPermiso("CementeriosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCementerios.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            NCHCementerios parametros = BusquedaParametrosObtenerValor<NCHCementerios>();
            parametros.IndiceColeccion = e.NewPageIndex;
            BusquedaParametrosGuardarValor<NCHCementerios>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = MisCementerios;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisCementerios = OrdenarGrillaDatos<DataTable>(MisCementerios, e);
            gvDatos.DataSource = MisCementerios;
            gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            gvDatos.AllowPaging = false;
            gvDatos.DataSource = MisCementerios;
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

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales; //TGEGeneralesF.FilialesObenerLista();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

        }

        private void CargarLista(NCHCementerios pCementerios)
        {
            pCementerios.Descripcion = txtDescripcion.Text;
            //pHoteles.Afiliado.CondicionFiscal.IdCondicionFiscal = ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlCondicionFiscal.SelectedValue);
            pCementerios.Estado.IdEstado = ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlEstado.SelectedValue);
            pCementerios.Filial.IdFilial = ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlFilial.SelectedValue);
            pCementerios.Codigo = txtCodigo.Text;
            pCementerios.Domicilio = txtDomicilio.Text;

            pCementerios.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pCementerios.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<NCHCementerios>(pCementerios);
            this.MisCementerios = CementeriosF.CementeriosObtenerListaGrilla(pCementerios);
            this.gvDatos.DataSource = this.MisCementerios;
            this.gvDatos.PageIndex = pCementerios.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisCementerios.Rows.Count > 0)
            {
                btnExportarExcel.Visible = true;
            }
            else
            {
                btnExportarExcel.Visible = false;
            }
        }
    }
}
