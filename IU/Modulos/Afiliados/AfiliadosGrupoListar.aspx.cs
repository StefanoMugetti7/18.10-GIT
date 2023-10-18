using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosGrupoListar : PaginaSegura
    {
        private DataTable MisAfiliados
        {
            get { return (DataTable)Session[this.MiSessionPagina + "AfiliadosGrupoListarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "AfiliadosGrupoListarMisAfiliados"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                paginaAfi.Guardar(this.MiSessionPagina, new AfiAfiliados());
                //paginaAfi.MiAfiliado = new AfiAfiliados();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNombre, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);

                this.CargarCombos();

                AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSocio.Text = parametros.NumeroSocio;
                    this.ddlTipoDocumento.SelectedValue = parametros.TipoDocumento.IdTipoDocumento.ToString();
                    this.txtNumeroDocumento.Text = parametros.NumeroDocumento.ToString();
                    this.txtApellido.Text = parametros.Apellido;
                    this.txtNombre.Text = parametros.Nombre;
                    this.txtMatricula.Text = parametros.MatriculaIAF.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            this.CargarLista(parametros);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                //|| e.CommandName == Gestion.Impresion.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            AfiAfiliados afiliado = new AfiAfiliados();
            afiliado.IdAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());

            this.MisParametrosUrl = new Hashtable
            {
                { "Gestion", e.CommandName },
                { "IdAfiliado", afiliado.IdAfiliado }
            };

            afiliado = AfiliadosF.AfiliadosObtenerDatos(afiliado);
            PaginaAfiliados paginaAfi = new PaginaAfiliados();
            paginaAfi.Guardar(this.MiSessionPagina, afiliado);
            //paginaAfi.MiAfiliado = afiliado;
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                //string url = string.Concat("~/Modulos/Afiliados/PosicionGlobal.aspx", parametros);
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
            }
            //else if (e.CommandName == Gestion.Impresion.ToString())
            //{
            //    TGEPlantillas plantilla = new TGEPlantillas();
            //    plantilla.Codigo = "AfiliadoSolicitudIngreso";
            //    plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

            //    if (plantilla.HtmlPlantilla.Trim().Length > 0)
            //    {
            //        TGEComprobantes comprobante = TGEGeneralesF.ComprobantesObtenerDatosCompletos(EnumTGEComprobantes.AltaAfiliado);
            //        DataSet ds = ExportPDF.ObtenerDatosReporteComprobante(paginaAfi.Obtener(this.MiSessionPagina), comprobante);
            //        ExportPDF.ConvertirHtmlEnPdf(this.UpdatePanel1, plantilla, ds, this.UsuarioActivo);
            //    }
            //    else
            //    {
            //        this.ctrPopUpComprobantes.CargarReporte(paginaAfi.Obtener(this.MiSessionPagina), EnumTGEComprobantes.AltaAfiliado);
            //        this.UpdatePanel1.Update();
            //    }
            //}
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                //Permisos btnEliminar
                //ibtnConsultar.Visible = this.ValidarPermiso("AfiliadosConsultar.aspx");
                //ibtnImprimir.Visible = true;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisAfiliados.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisAfiliados = this.OrdenarGrillaDatos<AfiAfiliados>(this.MisAfiliados, e);
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.DataBind();
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.DataBind();
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MisAfiliados);
        }
        private void CargarCombos()
        {
            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            this.ddlTipoDocumento.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlTipoDocumento.SelectedValue = "-1";
        }
        private void CargarLista(AfiAfiliados pAfiliado)
        {
            //pAfiliado.UsuarioActivo = this.UsuarioActivo;
            pAfiliado.NumeroSocio = this.txtNumeroSocio.Text.Trim();
            pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.Apellido = this.txtApellido.Text.Trim();
            pAfiliado.Nombre = this.txtNombre.Text.Trim();
            pAfiliado.MatriculaIAF = this.txtMatricula.Text == string.Empty ? 0 : Convert.ToInt64(this.txtMatricula.Text);
            pAfiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Socios;
            pAfiliado.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(pAfiliado);
            this.MisAfiliados = AfiliadosF.AfiliadosObtenerGrupoGrilla(pAfiliado);
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.PageIndex = pAfiliado.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisAfiliados.Rows.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }
    }
}
