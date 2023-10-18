using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Nichos;
using Nichos.Entidades;
using System;
using System.Collections;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Nichos
{
    public partial class NichosListar : PaginaSegura
    {
        private DataTable MisNichos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "NichosListarMisNichos"]; }
            set { Session[this.MiSessionPagina + "NichosListarMisNichos"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();
                NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();
                if (parametros.BusquedaParametros)
                { 
                    txtCodigo.Text = parametros.Codigo;
                    CargarLista(parametros);
                }
            }
        }
        protected void ddlTipoNicho_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTipoNicho.SelectedValue))
            {
                TGEListasValoresDetalles lista = new TGEListasValoresDetalles();
                lista.IdRefListaValorDetalle = Convert.ToInt32(ddlTipoNicho.SelectedValue);
                this.ddlNichoCapacidad.Items.Clear();
                this.ddlNichoCapacidad.DataSource = TGEGeneralesF.ListasValoresDetallesDependientes(lista);
                this.ddlNichoCapacidad.DataValueField = "IdListaValorDetalle";
                this.ddlNichoCapacidad.DataTextField = "Descripcion";
                this.ddlNichoCapacidad.DataBind();
                this.ddlNichoCapacidad.Enabled = true;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlNichoCapacidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                ddlNichoCapacidad.Items.Clear();
                ddlNichoCapacidad.Enabled = false;
                AyudaProgramacion.AgregarItemSeleccione(ddlNichoCapacidad, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        protected void ddlCementerio_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!ddlPanteon.Enabled)
                ddlPanteon.Enabled = true;
            if (!string.IsNullOrEmpty(ddlCementerio.SelectedValue))
            {
                NCHPanteones panteones = new NCHPanteones();
                panteones.Cementerio.IdCementerio = Convert.ToInt32(ddlCementerio.SelectedValue);
                ddlPanteon.DataSource = PanteonesF.PanteonesObtenerListaActiva(panteones);
                ddlPanteon.DataValueField = "IdPanteon";
                ddlPanteon.DataTextField = "Descripcion";
                ddlPanteon.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                ddlPanteon.Items.Clear();
                ddlPanteon.Enabled = false;
                AyudaProgramacion.AgregarItemSeleccione(ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/NichosAgregar.aspx"), true);
        }

        #region GV

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idNicho = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdNicho", idNicho);

            if (e.CommandName == Gestion.Modificar.ToString())            
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/NichosModificar.aspx"), true);          
            else if (e.CommandName == Gestion.Consultar.ToString())         
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Nichos/NichosConsultar.aspx"), true);
            
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("NichosModificar.aspx");
                consultar.Visible = this.ValidarPermiso("NichosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisNichos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            NCHNichos parametros = BusquedaParametrosObtenerValor<NCHNichos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            BusquedaParametrosGuardarValor<NCHNichos>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = MisNichos;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisNichos = OrdenarGrillaDatos<DataTable>(MisNichos, e);
            gvDatos.DataSource = MisNichos;
            gvDatos.DataBind();
        }

        #endregion
        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            gvDatos.AllowPaging = false;
            gvDatos.DataSource = MisNichos;
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

            this.ddlCementerio.DataSource = CementeriosF.CementeriosObtenerListaActiva(new NCHCementerios());
            this.ddlCementerio.DataValueField = "IdCementerio";
            this.ddlCementerio.DataTextField = "Descripcion";
            this.ddlCementerio.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCementerio, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoNicho.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposNichos);
            this.ddlTipoNicho.DataValueField = "IdListaValorDetalle";
            this.ddlTipoNicho.DataTextField = "Descripcion";
            this.ddlTipoNicho.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoNicho, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlNichoCapacidad.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposNichosCapacidad);
            this.ddlNichoCapacidad.DataValueField = "IdListaValorDetalle";
            this.ddlNichoCapacidad.DataTextField = "Descripcion";
            this.ddlNichoCapacidad.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlNichoCapacidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlUbicacion.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.NichosUbicacion);
            this.ddlUbicacion.DataValueField = "IdListaValorDetalle";
            this.ddlUbicacion.DataTextField = "Descripcion";
            this.ddlUbicacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlUbicacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlSubUbicacion.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.NichosSubUbicacion);
            this.ddlSubUbicacion.DataValueField = "IdListaValorDetalle";
            this.ddlSubUbicacion.DataTextField = "Descripcion";
            this.ddlSubUbicacion.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlSubUbicacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.AgregarItemSeleccione(this.ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
        }

            private void CargarLista(NCHNichos pParametro)
        {
            pParametro.Codigo = txtCodigo.Text;
            pParametro.Estado.IdEstado = ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlEstado.SelectedValue);
            pParametro.Panteon.IdPanteon = ddlPanteon.SelectedValue == string.Empty ? 0: Convert.ToInt32(ddlPanteon.SelectedValue);
            pParametro.TipoNicho.IdTipoNicho = ddlTipoNicho.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoNicho.SelectedValue);
            pParametro.NichoCapacidad.IdNichosCapacidad = ddlNichoCapacidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlNichoCapacidad.SelectedValue);
            pParametro.NichoUbicacion.IdNichosUbicacion= ddlUbicacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlUbicacion.SelectedValue);
            pParametro.NichoSubUbicacion.IdNichosSubUbicacion = ddlSubUbicacion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlSubUbicacion.SelectedValue);
            pParametro.Panteon.Cementerio.IdCementerio = ddlCementerio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlCementerio.SelectedValue);

            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<NCHNichos>(pParametro);

            this.MisNichos = NichosF.NichosObtenerListaGrilla(pParametro);
            this.gvDatos.DataSource = this.MisNichos;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisNichos.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }

    }
}
