using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using System.Collections;
using Comunes.Entidades;
using Afiliados;

namespace IU.Modulos.Afiliados
{
    public partial class CertificadoSupervivenciaListar : PaginaAfiliados
    {
        private List<AfiCertificadosSupervivencia> MisCertificados
        {
            get { return (List<AfiCertificadosSupervivencia>)Session[this.MiSessionPagina + "CertificadoSupervivenciaListarMisCertificados"]; }
            set { Session[this.MiSessionPagina + "CertificadoSupervivenciaListarMisCertificados"] = value; }
        }

        protected override void PageLoadEventAfiliados(object sender, EventArgs e)
        {
            base.PageLoadEventAfiliados(sender, e);
            if (!this.IsPostBack)
            {
                this.btnAgregar.Visible = this.ValidarPermiso("CertificadoSupervivenciaAgregar.aspx");
                this.CargarLista();
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CertificadoSupervivenciaAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AfiCertificadosSupervivencia certificado = this.MisCertificados[indiceColeccion];
            //string parametros = string.Format("?IdCuenta={0}", cuentafiliado.IdCuenta);
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdCertificadoSupervivencia", certificado.IdCertificadoSupervivencia);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CertificadoSupervivenciaModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                //string url = string.Concat(, parametros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/CertificadoSupervivenciaConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiCertificadosSupervivencia certificado = (AfiCertificadosSupervivencia)e.Row.DataItem;

                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                modificar.Visible = this.ValidarPermiso("CertificadoSupervivenciaModificar.aspx");
                consultar.Visible = this.ValidarPermiso("CertificadoSupervivenciaConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCertificados.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiCertificadosSupervivencia parametros = this.BusquedaParametrosObtenerValor<AfiCertificadosSupervivencia>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiCertificadosSupervivencia>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCertificados;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCertificados = this.OrdenarGrillaDatos<AfiCertificadosSupervivencia>(this.MisCertificados, e);
            this.gvDatos.DataSource = this.MisCertificados;
            this.gvDatos.DataBind();
        }

        private void CargarLista()
        {
            AfiCertificadosSupervivencia certificado = new AfiCertificadosSupervivencia();
            certificado.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.MisCertificados = AfiliadosF.CertificadosSupervivenciaObtenerListaFiltro(certificado);
            this.gvDatos.DataSource = this.MisCertificados;
            this.gvDatos.DataBind();
        }
    }
}
