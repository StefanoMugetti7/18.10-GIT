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
    public partial class PacientesListar : PaginaSegura
    {
        private DataTable MisPacientes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PacientesListarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "PacientesListarMisAfiliados"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                //// Cuando salgo del Afiliado vuelvo a cargar el Menu General
                //if (this.MenuPadre == EnumMenues.Afiliados)
                //{
                //    this.MenuPadre = EnumMenues.General;
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                paginaAfi.Guardar(this.MiSessionPagina, new AfiPacientes());
                //}

                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtIdAfiliado, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("PacientesAgregar.aspx");
                this.CargarCombos();

                AfiPacientes parametros = this.BusquedaParametrosObtenerValor<AfiPacientes>();
                AfiPacientes cliente = new AfiPacientes();
                cliente.IdAfiliado = parametros.IdAfiliado;
                cliente.Campos.AddRange(parametros.Campos);

                if (parametros.BusquedaParametros)
                {
                    //this.txtIdAfiliado.Text = parametros.IdAfiliado.ToString();
                    this.ddlTipoDocumento.SelectedValue = parametros.TipoDocumento.IdTipoDocumento.ToString();
                    this.txtNumeroDocumento.Text = parametros.NumeroDocumento.ToString();
                    this.txtApellido.Text = parametros.Apellido;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
           
                    this.txtNombre.Text = parametros.Nombre;
                 
                    this.CargarLista(parametros);
                }
             
                  
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiPacientes parametros = this.BusquedaParametrosObtenerValor<AfiPacientes>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesAgregar.aspx"), true);
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
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PacientesModificar.aspx"), true);
            }
        
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnConsultar");
            

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("PacientesConsultar.aspx");
                ibtnModificar.Visible = this.ValidarPermiso("PacientesModificar.aspx");
                
             
            }
            //if (e.Row.RowType == DataControlRowType.Footer)


            //{
            //    Label lblImporteTotal = (Label)e.Row.FindControl("lblImporteTotal");
            //    lblImporteTotal.Text = this.MisPacientes.AsEnumerable().Sum(x => x.Field<decimal>("SaldoActual")).ToString("C2");

            //    Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
            //    lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPacientes.Rows.Count);
            //}
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiPacientes parametros = this.BusquedaParametrosObtenerValor<AfiPacientes>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiPacientes>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisPacientes;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPacientes = this.OrdenarGrillaDatos<AfiPacientes>(this.MisPacientes, e);
            this.gvDatos.DataSource = this.MisPacientes;
            this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisPacientes;
            this.gvDatos.DataBind();
            ExportData exportData = new ExportData();
            exportData.ExportExcel(this.Page, this.MisPacientes);
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
            this.ddlEstados.SelectedValue = ((int)EstadosAfiliados.Vigente).ToString();

            //this.ddlAlertas.DataSource = AfiliadosF.AlertasTiposObtenerListaFiltro(new AfiAlertasTipos());
            //this.ddlAlertas.DataValueField = "IdAlertaTipo";
            //this.ddlAlertas.DataTextField = "AlertaTipo";
            //this.ddlAlertas.DataBind();
            //this.ddlAlertas.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "0"));
            //this.ddlAlertas.SelectedValue = "0";

        }

        private void CargarLista(AfiPacientes pAfiliado)
        {
            //pAfiliado.UsuarioActivo = this.UsuarioActivo;
            //pAfiliado.IdAfiliado = this.txtIdAfiliado.Text.Trim() == string.Empty ? 0 : Convert.ToInt32(this.txtIdAfiliado.Text.Trim());
            pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.Apellido = this.txtApellido.Text.Trim();
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //pAfiliado.TieneSaldo = this.chkTieneSaldo.Checked;
            //pAfiliado.Campos = this.ctrCamposValores.ObtenerLista();
            //pAfiliado.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
            pAfiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.PacientesExternos;
            pAfiliado.Nombre = this.txtNombre.Text.Trim();
            pAfiliado.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(pAfiliado);
            this.MisPacientes = AfiliadosF.PacientesObtenerGrilla(pAfiliado);
            this.gvDatos.DataSource = this.MisPacientes;
            this.gvDatos.PageIndex = pAfiliado.IndiceColeccion;
            this.gvDatos.DataBind();
            //this.gvDatos.UseAccessibleHeader = true;
            //this.gvDatos.HeaderRow.TableSection = TableRowSection.TableHeader;

            if (this.MisPacientes.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}
