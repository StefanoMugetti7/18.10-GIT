using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using Servicio.AccesoDatos;
using Comunes.Entidades;
using Facturas.Entidades;
using Afiliados.Entidades;

namespace IU
{
    public partial class prueba2 : System.Web.UI.Page
    {
        DataTable dt;
        protected void Page_Load(object sender, EventArgs e)
        {
            CustomPaging_GridView.PageSizeEvent += CustomPaging_GridView_PageSizeEvent;
            if (!IsPostBack)
            {
                CustomPaging_GridView.PageSize = 10;
                CustomPaging_GridView.VirtualItemCount = BaseDatos.ObtenerBaseDatos().EjecutarSP(new Objeto(),"PruebaCantidadDatatableNet");
                CambiarIndex(0, 10);
            }
            
        }

        private void CustomPaging_GridView_PageSizeEvent(int pageSize)
        {
            CustomPaging_GridView.PageSize = pageSize;
            CambiarIndex(CustomPaging_GridView.PageIndex, pageSize);
        }

        protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CustomPaging_GridView.PageIndex = e.NewPageIndex;
            CambiarIndex(e.NewPageIndex, CustomPaging_GridView.PageSize);
        }

        public void CambiarIndex(int PageIndex, int pagesize)
        {
            Index obj = new Index();
            obj.PageSize = pagesize;
            obj.PageIndex = PageIndex;

            dt = BaseDatos.ObtenerBaseDatos().ObtenerLista("PruebaDatatableNet", obj);

            CustomPaging_GridView.DataSource = dt;
            CustomPaging_GridView.DataBind();
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }
    }

    public class Index : Objeto
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
    }
}