using Comunes.Entidades;
using LavaYa;
using LavaYa.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.LavaYa
{
    public partial class EdificiosListar : PaginaSegura
    {
        private DataTable MisEdificios
        {
            get { return (DataTable)Session[this.MiSessionPagina + "EdificiosListarMisEdificios"]; }
            set { Session[this.MiSessionPagina + "EdificiosListarMisEdificios"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                LavEdificios parametros = this.BusquedaParametrosObtenerValor<LavEdificios>();
                if (parametros.BusquedaParametros)
                {
                    txtDescripcion.Text = parametros.Descripcion;
                    CargarLista(parametros);
                }
            }

        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            LavEdificios parametros = this.BusquedaParametrosObtenerValor<LavEdificios>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            LavEdificios parametros = this.BusquedaParametrosObtenerValor<LavEdificios>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/EdificiosAgregar.aspx"), true);
        }

        #region GV

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idEdificio = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdEdificio", idEdificio);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/EdificiosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/EdificiosConsultar.aspx"), true);

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("EdificiosModificar.aspx");
                consultar.Visible = this.ValidarPermiso("EdificiosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisEdificios.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            LavEdificios parametros = BusquedaParametrosObtenerValor<LavEdificios>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisEdificios = OrdenarGrillaDatos<DataTable>(MisEdificios, e);
            gvDatos.DataSource = MisEdificios;
            gvDatos.DataBind();
        }

        #endregion
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            gvDatos.AllowPaging = false;
            gvDatos.DataSource = MisEdificios;
            gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", gvDatos);
        }
        private void CargarLista(LavEdificios pParametro)
        {
            pParametro.Descripcion = txtDescripcion.Text;


            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pParametro.PageSize;
            this.BusquedaParametrosGuardarValor<LavEdificios>(pParametro);

            this.MisEdificios = EdificiosF.EdificiosObtenerListaGrilla(pParametro);
            this.gvDatos.DataSource = this.MisEdificios;
            this.gvDatos.VirtualItemCount = MisEdificios.Rows.Count > 0 ? Convert.ToInt32(MisEdificios.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.PageIndex = pParametro.PageIndex;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisEdificios.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}