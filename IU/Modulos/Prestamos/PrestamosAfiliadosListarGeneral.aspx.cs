using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Prestamos;
using Prestamos.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;

namespace IU.Modulos.Prestamos
{
    public partial class PrestamosAfiliadosListarGeneral : PaginaSegura
    {
        private DataTable CardsBootStrapDataTable
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PrestamosListarMisPrestamosAfiliados"]; }
            set { Session[this.MiSessionPagina + "PrestamosListarMisPrestamosAfiliados"] = value; }
        }
        private DataTable MisPrestamosAfiliadosGeneral
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PrestamosListarGeneralMisPrestamosAfiliadosGeneral"]; }
            set { Session[this.MiSessionPagina + "PrestamosListarGeneralMisPrestamosAfiliadosGeneral"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellidoNombre, this.btnBuscar);
                this.CardsBootStrap = new StringBuilder();
                this.CargarCardsBootStrap();
                this.MenuCards = this.CardsBootStrap == null ? string.Empty : this.CardsBootStrap.ToString();
                this.ltrCards.Text = this.MenuCards;
                CargarCombos();

                PrePrestamos parametros = this.BusquedaParametrosObtenerValor<PrePrestamos>();
                if (parametros.BusquedaParametros)
                {
                    this.txtNumeroSocio.Text = parametros.Afiliado.NumeroSocio;
                    this.txtNumeroDocumento.Text = parametros.Afiliado.NumeroDocumento.ToString();
                    this.txtFechaDesde.Text = parametros.FechaDesde.HasValue ? parametros.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = parametros.FechaHasta.HasValue ? parametros.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.txtApellidoNombre.Text = parametros.Filtro;
                    this.ddlVendedor.SelectedValue = parametros.Filial.IdFilial.ToString();
                    this.ddlFormaCobro.SelectedValue = parametros.FormaCobroAfiliado.IdFormaCobroAfiliado.ToString();
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            PrePrestamos parametros = this.BusquedaParametrosObtenerValor<PrePrestamos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }

        private string MenuCards
        {
            get
            {
                if (Session["MaestraMenuHtml"] != null)
                { return (string)Session["MaestraMenuHtml"]; }
                else
                { return string.Empty; }
            }
            set { Session["MaestraMenuHtml"] = value; }
        }
        private StringBuilder CardsBootStrap;
        private void CargarCardsBootStrap()
        {
            string accion = string.Empty;
            string appPath = string.Empty;
            string target = string.Empty;

            PrePrestamos filtro = new PrePrestamos();
            DataTable cards = PrePrestamosF.PrestamosCargarCardsBootStrap(filtro);
            if (cards.Rows.Count > 0)
            {
                

                foreach (DataRow fila in cards.Rows)
                {

                    //                < div class="card" style="width: 10rem; ">
                    //  <div class="card-body bg-info ">
                    //    <h5 class="card-title" style="padding: 1rem; "> <asp:Label CssClass = "col-sm-1 col-form-label" ID="Label3" runat="server" Text="informacion"></asp:Label>
                    //             </h5>
                    //    <p class="card-text text-right" style="padding: 0.5rem;"> <asp:Label CssClass = "col-sm-1 col-form-label" ID="Label4" runat="server" Text="Columna"></asp:Label>
                    //             </p>

                    //  </div>
                    //</div>




                    this.CardsBootStrap.AppendFormat("     <a href=\"#\" class=\"link-prestamos\" onclick=\"javascript:EjecutarFiltro({0});\">", fila["IdEstado"]);
                    this.CardsBootStrap.AppendLine(" <div class=\"card card-prestamos\">");
                    this.CardsBootStrap.AppendFormat(" <div class=\"card-body {0} \">", fila["Color"]);
                    this.CardsBootStrap.AppendFormat(" <h5 class=\"card-title text-left card-prestamosh5\" >{0}</h5>", fila["Cantidad"]);
                    this.CardsBootStrap.AppendFormat("<p class=\"card-prestamosp card-text text-left\">{0}</p>", fila["EstadoDescripcion"]);
                    this.CardsBootStrap.AppendLine("</div>");
                    this.CardsBootStrap.AppendLine("</div>");
                    this.CardsBootStrap.AppendLine("</a>");
                
                }
            }

           
        }
      
        //protected void btnAgregar_Click(object sender, EventArgs e)
        //{
        //    //this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAgregar.aspx"), true);
        //}

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            PrePrestamos parametros = this.BusquedaParametrosObtenerValor<PrePrestamos>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Sort" || e.CommandName == "Page")
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            PrePrestamos prestamoAfiliado = new PrePrestamos();
            prestamoAfiliado.IdPrestamo = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdPrestamo"].ToString());
            prestamoAfiliado.TipoOperacion.IdTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdTipoOperacion"].ToString());
            //string parametros = string.Format("?IdPrestamo={0}", prestamoAfiliado.IdPrestamo);
            
            AfiAfiliados afiliado = new AfiAfiliados();
            afiliado.IdAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());
            afiliado = AfiliadosF.AfiliadosObtenerDatos(afiliado);
            PaginaAfiliados paginaAfi = new PaginaAfiliados();
            paginaAfi.Guardar(this.MiSessionPagina, afiliado);

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdPrestamo", prestamoAfiliado.IdPrestamo);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosConsultar.aspx"), true);
            else if (e.CommandName == "PreAutorizar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosPreAutorizar.aspx"), true);
            else if (e.CommandName == "Autorizar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAutorizar.aspx"), true);
            else if (e.CommandName == Gestion.Anular.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAnular.aspx"), true);
            else if (e.CommandName == Gestion.AnularConfirmar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosAnularConfirmado.aspx"), true);
            else if (e.CommandName == "Cancelar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosCancelar.aspx"), true);
            else if (e.CommandName == "AnularCancelar")
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Prestamos/PrestamosAfiliadosCancelar.aspx"), true);
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                EnumTGEComprobantes enumTGEComprobantes;
                if (prestamoAfiliado.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.PrestamosBancoDelSol)
                    enumTGEComprobantes = EnumTGEComprobantes.PrePrestamosBancoSol;
                else
                    enumTGEComprobantes = EnumTGEComprobantes.PrePrestamos;

                TESCajasMovimientos movimiento = new TESCajasMovimientos();
                movimiento.IdRefTipoOperacion = prestamoAfiliado.IdPrestamo;
                movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
                movimiento.TipoOperacion.IdTipoOperacion = prestamoAfiliado.TipoOperacion.IdTipoOperacion;
                TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(enumTGEComprobantes, miPlantilla.Codigo, prestamoAfiliado, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Prestamo_", prestamoAfiliado.IdPrestamo.ToString().PadLeft(10, '0')), this.UsuarioActivo);

            }

        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ibtnConsultar.Visible = this.ValidarPermiso("PrestamosAfiliadosConsultar.aspx");

                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton ibtnPreAutorizar = (ImageButton)e.Row.FindControl("btnPreAutorizar");
                ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");
                ImageButton ibtnAnular = (ImageButton)e.Row.FindControl("btnAnular");
                ImageButton ibtnCancelar = (ImageButton)e.Row.FindControl("btnCancelar");
                ImageButton ibtnAnularCancelar = (ImageButton)e.Row.FindControl("btnAnularCancelar");
                ImageButton ibtnAnularConfirmado = (ImageButton)e.Row.FindControl("btnAnularConfirmado");
                bool permisoAnular = this.ValidarPermiso("PrestamosAfiliadosAnular.aspx");
                DataRowView dr = (DataRowView)e.Row.DataItem;

                switch (Convert.ToInt32(dr["IdEstado"]))
                {
                    case (int)EstadosPrestamos.Activo:
                        ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
                        ibtnPreAutorizar.Visible = this.ValidarPermiso("PrestamosAfiliadosPreAutorizar.aspx");
                        ibtnAnular.Visible = permisoAnular;
                        break;
                    case (int)EstadosPrestamos.Anulado:
                        break;
                    case (int)EstadosPrestamos.Finalizado:
                        break;
                    case (int)EstadosPrestamos.Cancelado:
                        break;
                    case (int)EstadosPrestamos.Autorizado:
                        ibtnAnular.Visible = permisoAnular;
                        ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
                        break;
                    case (int)EstadosPrestamos.Confirmado:
                        //if (prestamo.TipoOperacion.IdTipoOperacion ==(int)EnumTGETiposOperaciones.PrestamosLargoPlazo
                        //    || prestamo.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.Prestamos46
                        //    || prestamo.TipoOperacion.IdTipoOperacion == (int)EnumTGETiposOperaciones.Prestamos49)
                        ibtnCancelar.Visible = this.ValidarPermiso("PrestamosAfiliadosCancelar.aspx");
                        ibtnModificar.Visible = this.ValidarPermiso("PrestamosAfiliadosModificar.aspx");
                        ibtnAnularConfirmado.Visible = this.ValidarPermiso("PrestamosAfiliadosAnularConfirmado.aspx");
                        break;
                    case (int)EstadosPrestamos.PendienteCancelacion:
                        ibtnAnularCancelar.Visible = this.ValidarPermiso("PrestamosAfiliadosCancelar.aspx");
                        break;
                    case (int)EstadosPrestamos.PreAutorizado:
                        ibtnAutorizar.Visible = this.ValidarPermiso("PrestamosAfiliadosAutorizar.aspx");
                        ibtnAnular.Visible = permisoAnular;
                        break;
                    default:
                        break;
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                //int cellCount = e.Row.Cells.Count;
                //e.Row.Cells.Clear();
                //TableCell tableCell = new TableCell();
                //tableCell.ColumnSpan = cellCount;
                //tableCell.HorizontalAlign = HorizontalAlign.Right;
                //tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestamosAfiliadosGeneral.Rows.Count);
                //e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            PrePrestamos parametros = this.BusquedaParametrosObtenerValor<PrePrestamos>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;

            CargarLista(parametros);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPrestamosAfiliadosGeneral = this.OrdenarGrillaDatos<DataTable>(this.MisPrestamosAfiliadosGeneral, e);
            this.gvDatos.DataSource = this.MisPrestamosAfiliadosGeneral;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosPrestamos));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            if (ddlEstados.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //this.ddlEstados.SelectedValue = "-1";

            this.ddlFormaCobro.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
            this.ddlFormaCobro.DataTextField = "FormaCobro";
            this.ddlFormaCobro.DataValueField = "IdFormaCobro";
            this.ddlFormaCobro.DataBind();
            if (ddlFormaCobro.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlVendedor.DataSource = this.UsuarioActivo.Filiales;
            this.ddlVendedor.DataValueField = "IdFilial";
            this.ddlVendedor.DataTextField = "Filial";
            this.ddlVendedor.DataBind();
            if (ddlVendedor.Items.Count > 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlVendedor, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarLista(PrePrestamos pParametro)
        {
            pParametro.Afiliado.NumeroSocio = this.txtNumeroSocio.Text.Trim();
            pParametro.Afiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pParametro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? (int)EstadosTodos.Todos : Convert.ToInt32(this.ddlEstados.SelectedValue);
            pParametro.Filial.IdFilial = this.ddlVendedor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlVendedor.SelectedValue);
            pParametro.FormaCobroAfiliado.IdFormaCobroAfiliado = this.ddlFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFormaCobro.SelectedValue);
            pParametro.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pParametro.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pParametro.Filtro = this.txtApellidoNombre.Text.Trim();
            pParametro.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pParametro.PageSize;
            pParametro.BusquedaParametros = true;

            this.BusquedaParametrosGuardarValor<PrePrestamos>(pParametro);
            this.MisPrestamosAfiliadosGeneral = PrePrestamosF.PrestamosObtenerPorAfiliadoGeneral(pParametro);
            this.gvDatos.DataSource = this.MisPrestamosAfiliadosGeneral;
            this.gvDatos.VirtualItemCount = MisPrestamosAfiliadosGeneral.Rows.Count > 0 ? Convert.ToInt32(MisPrestamosAfiliadosGeneral.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);
        }

    }
}