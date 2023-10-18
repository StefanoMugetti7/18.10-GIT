using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosListar : PaginaSegura
    {
        private DataTable MisAfiliados
        {
            get { return (DataTable)Session[this.MiSessionPagina + "AfiliadosListarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "AfiliadosListarMisAfiliados"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.gvDatos.PageSizeEvent += this.GvDatos_PageSizeEvent;

            if (!this.IsPostBack)
            {
                this.MenuPadre = EnumMenues.General;
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                paginaAfi.Guardar(this.MiSessionPagina, new AfiAfiliados());
                //paginaAfi.MiAfiliado = new AfiAfiliados();
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNombre, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("AfiliadosAgregar.aspx");
                this.CargarCombos();

                AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSocio.Text = parametros.NumeroSocio;
                    this.ddlTipoDocumento.SelectedValue = parametros.TipoDocumento.IdTipoDocumento.ToString();
                    this.txtNumeroDocumento.Text = parametros.NumeroDocumento.ToString();
                    this.txtApellido.Text = parametros.Apellido;
                    this.txtNombre.Text = parametros.Nombre;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlAlertas.SelectedValue = parametros.AlertaTipo.IdAlertaTipo.ToString();
                    this.ddlCategoria.SelectedValue = parametros.Categoria.IdCategoria.ToString();
                    this.chkFamiliares.Checked = parametros.IdAfiliadoRef == 0 ? false : true;
                    this.txtMatricula.Text = parametros.MatriculaIAF.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        private void GvDatos_PageSizeEvent(int pageSize)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            this.CargarLista(parametros);
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.PageIndex = 0;
            this.gvDatos.PageIndex = 0;
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosAgregar.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            AfiAfiliados afiliado = new AfiAfiliados();
            AfiAfiliados afiliadoImprimir = new AfiAfiliados();
            afiliadoImprimir.IdAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());
            afiliado.IdAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());
            if (!string.IsNullOrEmpty(((GridView)sender).DataKeys[index]["IdAfiliadoRef"].ToString()))
                afiliado.IdAfiliadoRef = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliadoRef"].ToString());
            afiliado.AfiliadoTipo.IdAfiliadoTipo = Convert.ToInt32(((GridView)sender).DataKeys[index]["AfiliadoTipoIdAfiliadoTipo"].ToString());
            afiliado = AfiliadosF.AfiliadosObtenerDatos(afiliado);

            //paginaAfi.MiAfiliado = afiliado;
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (!string.IsNullOrEmpty(afiliado.IdAfiliadoRef.ToString()))
                {
                    if (afiliado.IdAfiliadoRef > 0 && afiliado.AfiliadoTipo.IdAfiliadoTipo != (int)EnumAfiliadosTipos.Socios)
                    {
                        int idref = afiliado.IdAfiliadoRef;
                        afiliado = new AfiAfiliados();
                        afiliado.IdAfiliado = idref;
                        afiliado.TipoDocumento.IdTipoDocumento = -1;
                        afiliado.Estado.IdEstado = (int)EstadosTodos.Todos;
                        afiliado = AfiliadosF.AfiliadosObtenerListaFiltro(afiliado)[0];
                    }
                }
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                paginaAfi.Guardar(this.MiSessionPagina, afiliado);

                //string parametros = string.Format("?Gestion={0}&IdAfiliado={1}", e.CommandName, afiliado.IdAfiliado);
                this.MisParametrosUrl = new Hashtable
                {
                    { "Gestion", e.CommandName },
                    { "IdAfiliado", afiliado.IdAfiliado }
                };
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosDatos.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                afiliado = AfiliadosF.AfiliadosObtenerDatos(afiliado);
                TGEPlantillas plantilla = new TGEPlantillas();
                EnumTGEComprobantes aux;
                if (afiliado.Estado.IdEstado == (int)EstadosAfiliados.Baja)
                {
                    plantilla.Codigo = "AfiliadoSolicitudBaja";
                    aux = EnumTGEComprobantes.AfiliadoSolicitudBaja;
                }
                else
                {
                    plantilla.Codigo = "AfiliadoSolicitudIngreso";
                    aux = EnumTGEComprobantes.AltaAfiliado;
                }

                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                movimiento.IdRefTipoOperacion = afiliado.IdAfiliado;
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                movimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.Afiliados;
                movimiento.Filtro = plantilla.Codigo;
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(aux, miPlantilla.Codigo, afiliadoImprimir, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Afiliado_", afiliadoImprimir.IdAfiliado.ToString().PadLeft(10, '0')), this.UsuarioActivo);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnImprimir = (ImageButton)e.Row.FindControl("btnImprimir");
                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("AfiliadosConsultar.aspx");
                ibtnImprimir.Visible = true;
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
            AfiAfiliados parametros = BusquedaParametrosObtenerValor<AfiAfiliados>();
            this.gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;
            this.CargarLista(parametros);
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisAfiliados = OrdenarGrillaDatos<AfiAfiliados>(this.MisAfiliados, e);
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

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("AfiAfiliados");
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.EstadoAfiliadoListarPorDefecto);
            ListItem item = this.ddlEstados.Items.FindByValue(paramValor.ParametroValor);
            if (item != null)
                this.ddlEstados.SelectedValue = item.Value;
            else
                this.ddlEstados.SelectedValue = ((int)EstadosAfiliados.Vigente).ToString();

            this.ddlAlertas.DataSource = AfiliadosF.AlertasTiposObtenerListaFiltro(new AfiAlertasTipos());
            this.ddlAlertas.DataValueField = "IdAlertaTipo";
            this.ddlAlertas.DataTextField = "AlertaTipo";
            this.ddlAlertas.DataBind();
            this.ddlAlertas.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "0"));
            this.ddlAlertas.SelectedValue = "0";

            AfiAfiliados afiFiltro = new AfiAfiliados();
            afiFiltro.Estado.IdEstado = (int)Estados.Activo;
            this.ddlCategoria.DataSource = AfiliadosF.CategoriasObtenerListaActiva(afiFiltro);
            this.ddlCategoria.DataValueField = "IdCategoria";
            this.ddlCategoria.DataTextField = "Categoria";
            this.ddlCategoria.DataBind();
            this.ddlCategoria.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "0"));
            this.ddlCategoria.SelectedValue = "0";
        }
        private void CargarLista(AfiAfiliados pAfiliado)
        {
            pAfiliado.NumeroSocio = this.txtNumeroSocio.Text.Trim();
            pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.Apellido = this.txtApellido.Text.Trim();
            pAfiliado.Nombre = this.txtNombre.Text.Trim();
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pAfiliado.IdAfiliadoRef = this.chkFamiliares.Checked ? -1 : 0;
            pAfiliado.AlertaTipo.IdAlertaTipo = Convert.ToInt32(this.ddlAlertas.SelectedValue);
            pAfiliado.Categoria.IdCategoria = Convert.ToInt32(this.ddlCategoria.SelectedValue);
            pAfiliado.MatriculaIAF = this.txtMatricula.Text == string.Empty ? 0 : Convert.ToInt64(this.txtMatricula.Text);
            pAfiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Socios;
            pAfiliado.BusquedaParametros = true;
            pAfiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pAfiliado.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(this.UsuarioActivo.PageSize);
            this.gvDatos.PageSize = pAfiliado.PageSize;

            this.BusquedaParametrosGuardarValor<AfiAfiliados>(pAfiliado);
            this.MisAfiliados = AfiliadosF.AfiliadosObtenerGrilla(pAfiliado);
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.VirtualItemCount = this.MisAfiliados.Rows.Count > 0 ? Convert.ToInt32(this.MisAfiliados.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + this.gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisAfiliados.Rows.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }
    }
}
