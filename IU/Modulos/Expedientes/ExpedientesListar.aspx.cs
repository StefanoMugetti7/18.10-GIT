using Comunes.Entidades;
using Expedientes;
using Expedientes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Expedientes
{
    public partial class ExpedientesListar : PaginaSegura
    {
        private List<ExpExpedientes> MisExpedientes
        {
            get { return (List<ExpExpedientes>)Session[this.MiSessionPagina + "ExpedientesListarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "ExpedientesListarMisAfiliados"] = value; }
        }
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtIdExpediente, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTitulo, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("ExpedientesAgregar.aspx");
                this.CargarCombos();

                ExpExpedientes parametros = this.BusquedaParametrosObtenerValor<ExpExpedientes>();
                if (parametros.BusquedaParametros)
                {
                    this.txtIdExpediente.Text = parametros.IdExpediente.ToString();
                    this.ddlExpedientesTipos.SelectedValue = parametros.ExpedienteTipo.IdExpedienteTipo.ToString();
                    this.txtTitulo.Text = parametros.Titulo.ToString();
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.ddlEstadosTracking.SelectedValue = parametros.IdEstadoExpedienteTracking.ToString();
                    this.CargarLista(parametros);
                }
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            ExpExpedientes parametros = this.BusquedaParametrosObtenerValor<ExpExpedientes>();
            this.CargarLista(parametros);
        }
        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Expedientes/ExpedientesAgregar.aspx"), true);
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == "Aceptar"
                || e.CommandName == "Rechazar"
                || e.CommandName == Gestion.Impresion.ToString())
                )
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            ExpExpedientes expediente = this.MisExpedientes[indiceColeccion];

            //string parametros = string.Format("?Gestion={0}&IdAfiliado={1}", e.CommandName, afiliado.IdAfiliado);
            this.MisParametrosUrl = new Hashtable
            {
                { "Gestion", e.CommandName },
                { "IdExpediente", expediente.IdExpediente }
            };

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Expedientes/ExpedientesConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == "Aceptar"
                || e.CommandName == "Rechazar")
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Expedientes/ExpedientesModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                this.ctrPopUpComprobantes.CargarReporte(expediente, EnumTGEComprobantes.ExpExpedientes);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton ibtnAceptarDerivadocion = (ImageButton)e.Row.FindControl("btnAceptarDerivacion");
                ImageButton ibtnRechazarDerivacion = (ImageButton)e.Row.FindControl("btnRechazarDerivacion");

                ibtnConsultar.Visible = this.ValidarPermiso("ExpedientesConsultar.aspx");
                bool permisoModificar = this.ValidarPermiso("ExpedientesModificar.aspx");
                //ibtnModificar.Visible = this.ValidarPermiso("ExpedientesModificar.aspx");

                ExpExpedientes expediente = (ExpExpedientes)e.Row.DataItem;
                //Si tiene Derivación
                if (expediente.ExpedienteDerivado.Sector.IdSector > 0
                    && expediente.ExpedienteDerivado.Sector.IdSector == this.UsuarioActivo.SectorPredeterminado.IdSector)
                {
                    ibtnAceptarDerivadocion.Visible = permisoModificar;
                    //string mensaje = this.ObtenerMensajeSistema("AceptarDerivacion");
                    //mensaje = string.Format(mensaje, string.Concat(expediente.Titulo));
                    //string funcion = string.Format("showConfirm(this,'{0}'); ", mensaje);
                    //ibtnAceptarDerivadocion.Attributes.Add("OnClick", funcion);

                    ibtnRechazarDerivacion.Visible = permisoModificar;
                    //mensaje = this.ObtenerMensajeSistema("AceptarDerivacion");
                    //mensaje = string.Format(mensaje, string.Concat(expediente.Titulo));
                    //funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    //ibtnRechazarDerivacion.Attributes.Add("OnClick", funcion);
                }
                else if (expediente.ExpedienteTracking.Sector.IdSector == this.UsuarioActivo.SectorPredeterminado.IdSector)
                {
                    ibtnModificar.Visible = permisoModificar;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisExpedientes.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            ExpExpedientes parametros = this.BusquedaParametrosObtenerValor<ExpExpedientes>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<ExpExpedientes>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisExpedientes;
            this.gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisExpedientes = this.OrdenarGrillaDatos<ExpExpedientes>(this.MisExpedientes, e);
            this.gvDatos.DataSource = this.MisExpedientes;
            this.gvDatos.DataBind();
        }
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisExpedientes;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }
        private void CargarCombos()
        {
            this.ddlExpedientesTipos.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.ExpedientesTipos);
            this.ddlExpedientesTipos.DataValueField = "IdListaValorDetalle";
            this.ddlExpedientesTipos.DataTextField = "Descripcion";
            this.ddlExpedientesTipos.DataBind();
            this.ddlExpedientesTipos.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlExpedientesTipos.SelectedValue = "-1";

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosExpedientes));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstados.SelectedValue = "-1";
            //this.ddlEstadosTracking.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosExpedientesTracking));
            List<TGEEstados> lista = new List<TGEEstados>();
            TGEEstados item = new TGEEstados();
            item.IdEstado = (int)EstadosExpedientesTracking.Derivado;
            lista.Add(TGEGeneralesF.TGEEstadosObtener(item));
            this.ddlEstadosTracking.DataSource = lista;
            this.ddlEstadosTracking.DataValueField = "IdEstado";
            this.ddlEstadosTracking.DataTextField = "Descripcion";
            this.ddlEstadosTracking.DataBind();
            this.ddlEstadosTracking.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstadosTracking.SelectedValue = "-1";
        }
        private void CargarLista(ExpExpedientes pExpediente)
        {
            pExpediente.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pExpediente.IdExpediente = this.txtIdExpediente.Text == string.Empty ? 0 : Convert.ToInt32(this.txtIdExpediente.Text);
            pExpediente.ExpedienteTipo.IdExpedienteTipo = Convert.ToInt32(this.ddlExpedientesTipos.SelectedValue);
            pExpediente.Titulo = this.txtTitulo.Text.Trim();
            pExpediente.Descripcion = this.txtDescripcion.Text.Trim();
            pExpediente.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pExpediente.IdEstadoExpedienteTracking = Convert.ToInt32(this.ddlEstadosTracking.SelectedValue);
            pExpediente.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<ExpExpedientes>(pExpediente);
            this.MisExpedientes = ExpedientesF.ExpedientesObtenerListaFiltro(pExpediente);
            this.gvDatos.DataSource = this.MisExpedientes;
            this.gvDatos.PageIndex = pExpediente.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisExpedientes.Count > 0)
                this.btnExportarExcel.Visible = true;
            else
                this.btnExportarExcel.Visible = false;
        }
    }
}