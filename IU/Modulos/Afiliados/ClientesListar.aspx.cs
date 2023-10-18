using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;
using Afiliados;
using System.Collections;
using Generales.FachadaNegocio;
using System.Data;

namespace IU.Modulos.Afiliados
{
    public partial class ClientesListar : PaginaSegura
    {
        private DataTable MisAfiliados
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesListarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "ClientesListarMisAfiliados"] = value; }
        }

        private DataTable MisAfiliadosExportar
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ClientesListarMisAfiliadosExportar"]; }
            set { Session[this.MiSessionPagina + "ClientesListarMisAfiliadosExportar"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            gvDatos.PageSizeEvent += GvDatos_PageSizeEvent;
            if (!this.IsPostBack)
            {
                //// Cuando salgo del Afiliado vuelvo a cargar el Menu General
                //if (this.MenuPadre == EnumMenues.Afiliados)
                //{
                //    this.MenuPadre = EnumMenues.General;
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                paginaAfi.Guardar(this.MiSessionPagina, new AfiAfiliados());
                //}

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtIdAfiliado, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("ClientesAgregar.aspx");
                this.CargarCombos();

                AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
                AfiClientes cliente = new AfiClientes();
                cliente.IdAfiliado = parametros.IdAfiliado;
                cliente.Campos.AddRange(parametros.Campos);

                if (parametros.BusquedaParametros)
                {
                    this.txtIdAfiliado.Text = parametros.IdAfiliado.ToString();
                    this.ddlTipoDocumento.SelectedValue = parametros.TipoDocumento.IdTipoDocumento.ToString();
                    this.txtNumeroDocumento.Text = parametros.NumeroDocumento.ToString();
                    this.txtApellido.Text = parametros.RazonSocial;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.chkTieneSaldo.Checked = parametros.TieneSaldo;
                    this.txtDetalle.Text = parametros.Detalle;
                    this.ctrCamposValores.IniciarControl(cliente.Campos, Gestion.Modificar);
                    this.CargarLista(parametros);
                }
                else
                    this.ctrCamposValores.IniciarControl(cliente, new Objeto(), Gestion.Modificar);
            }
        }

        private void GvDatos_PageSizeEvent(int pageSize)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.PageIndex = 0;
            gvDatos.PageIndex = 0;
            this.UsuarioActivo.PageSize = pageSize;
            CargarLista(parametros);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == "AgregarComprobante"
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());

            this.MisParametrosUrl = new Hashtable();

            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdAfiliado", idAfiliado);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ClientesModificar.aspx"), true);
            }
            else if (e.CommandName == "AgregarComprobante")
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/FacturasAgregar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton btnAgregarComprobante = (ImageButton)e.Row.FindControl("btnAgregarComprobante");

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("ClientesConsultar.aspx");
                ibtnModificar.Visible = this.ValidarPermiso("ClientesModificar.aspx");
                btnAgregarComprobante.Visible = this.ValidarPermiso("FacturasAgregar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
                lblImporteTotal.Text = this.MisAfiliados.AsEnumerable().Sum(x => x.Field<decimal>("SaldoActual")).ToString("C2");

                Label lblImporteTotalDolar = (Label)e.Row.FindControl("lblImporteTotalDolar");
                lblImporteTotalDolar.Text = this.MisAfiliados.AsEnumerable().Sum(x => x.Field<decimal>("SaldoActualDolar")).ToString("C2");

                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisAfiliados.Rows.Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            gvDatos.PageIndex = e.NewPageIndex;
            parametros.PageIndex = e.NewPageIndex;

            CargarLista(parametros);
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
            AfiAfiliados afiliado = new AfiAfiliados();
            afiliado.PageSize = 50000;
            afiliado.TipoDocumento.IdTipoDocumento = -1;
            afiliado.PageIndex = 0;
            afiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            afiliado.TieneSaldo = this.chkTieneSaldo.Checked;
            afiliado.Campos = this.ctrCamposValores.ObtenerLista();
            afiliado.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
            afiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Clientes;
            afiliado.Detalle = this.txtDetalle.Text.Trim();
            afiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            MisAfiliadosExportar = AfiliadosF.ClientesObtenerGrilla(afiliado);
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MisAfiliadosExportar);
        }

        private void CargarCombos()
        {
            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            this.ddlTipoDocumento.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlTipoDocumento.SelectedValue = "-1";

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosAfiliados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstados.SelectedValue = "-1";

            //this.ddlAlertas.DataSource = AfiliadosF.AlertasTiposObtenerListaFiltro(new AfiAlertasTipos());
            //this.ddlAlertas.DataValueField = "IdAlertaTipo";
            //this.ddlAlertas.DataTextField = "AlertaTipo";
            //this.ddlAlertas.DataBind();
            //this.ddlAlertas.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "0"));
            //this.ddlAlertas.SelectedValue = "0";

        }

        private void CargarLista(AfiAfiliados pAfiliado)
        {
            //pAfiliado.UsuarioActivo = this.UsuarioActivo;
            pAfiliado.IdAfiliado =  this.txtIdAfiliado.Text.Trim()==string.Empty? 0 : Convert.ToInt32(this.txtIdAfiliado.Text.Trim());
            pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.RazonSocial = this.txtApellido.Text.Trim();
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pAfiliado.TieneSaldo = this.chkTieneSaldo.Checked;
            pAfiliado.Campos = this.ctrCamposValores.ObtenerLista();
            pAfiliado.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
            pAfiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Clientes;
            pAfiliado.Detalle = this.txtDetalle.Text.Trim();
            pAfiliado.BusquedaParametros = true;
            pAfiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pAfiliado.PageSize = AyudaProgramacion.ObtenerGridviewPageSize(UsuarioActivo.PageSize);
            gvDatos.PageSize = pAfiliado.PageSize;
            gvDatos.PageIndex = pAfiliado.PageIndex;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(pAfiliado);
            this.MisAfiliados = AfiliadosF.ClientesObtenerGrilla(pAfiliado);
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.VirtualItemCount = MisAfiliados.Rows.Count > 0 ? Convert.ToInt32(MisAfiliados.Rows[0]["Cantidad"]) : 0;
            this.gvDatos.deTantos.Text = "de " + gvDatos.VirtualItemCount.ToString();
        
            this.gvDatos.DataBind();
            //this.gvDatos.UseAccessibleHeader = true;
            //this.gvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (this.MisAfiliados.Rows.Count > 0 )
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}
