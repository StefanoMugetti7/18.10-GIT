using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Nichos;
using Nichos.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace IU.Modulos.Comunes
{
    [Serializable]
    public partial class Nichos : ControlesSeguros
    {
        private DataTable MiNicho
        {
            get { return (DataTable)Session[this.MiSessionPagina + "NichosListarMisNichosDisponibles"]; }
            set { Session[this.MiSessionPagina + "NichosListarMisNichosDisponibles"] = value; }
        }

        private enum Acciones
        {
            Seleccionar,
            Cancelar,
        }
        private Acciones MiAccion
        {
            get { return (Acciones)Session[this.MiSessionPagina + "NichosListarMiAccion"]; }
            set { Session[this.MiSessionPagina + "NichosListarMiAccion"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();
                if (parametros.BusquedaParametros)
                    CargarLista(parametros);
            }
        }
        public void IniciarControl(TGECampos pParametro, bool pHabilitar, Gestion pGestion)
        {
            MiAccion = Acciones.Seleccionar;
            this.CargarCombos();
             
            hdfIdNicho.Value = pParametro.CampoValor.Valor;
            switch (pGestion)
            {
                case Gestion.Consultar:
                    break;
                case Gestion.Agregar:
                    break;
                case Gestion.Modificar:
                    int idNicho = 0;
                    if (!string.IsNullOrWhiteSpace(hdfIdNicho.Value))                    
                        idNicho = Convert.ToInt32(hdfIdNicho.Value);             
                    NCHNichos nichoSeleccionado = new NCHNichos();
                    nichoSeleccionado.IdNicho = idNicho;                
                    this.LimpiarLista();
                    this.CargarLista(nichoSeleccionado);
                    this.pnlBuscador.Visible = false;
                    this.pnlGrilla.Visible = true;
                    this.gvDatos.Columns[7].Visible = false;// OCULTA COLUMNA ACCIONES
                    break;
                case Gestion.Anular:
                    break;
                default:
                    break;
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
                this.ddlPanteon.Items.Clear();
                this.ddlPanteon.SelectedIndex = -1;
                this.ddlPanteon.SelectedValue = null;
                this.ddlPanteon.ClearSelection();
                this.ddlPanteon.DataSource = PanteonesF.PanteonesObtenerListaActiva(panteones);
                this.ddlPanteon.DataValueField = "IdPanteon";
                this.ddlPanteon.DataTextField = "Descripcion";
                this.ddlPanteon.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                this.ddlPanteon.Items.Clear();
                this.ddlPanteon.Enabled = false;
                AyudaProgramacion.AgregarItemSeleccione(ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        protected void ddlTipoNicho_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlTipoNicho.SelectedValue))
            {
                TGEListasValoresDetalles lista = new TGEListasValoresDetalles();
                lista.IdRefListaValorDetalle = Convert.ToInt32(ddlTipoNicho.SelectedValue);
                this.ddlNichoCapacidad.Items.Clear();
                this.ddlNichoCapacidad.SelectedIndex = -1;
                this.ddlNichoCapacidad.SelectedValue = null;
                this.ddlNichoCapacidad.ClearSelection();
                this.ddlNichoCapacidad.DataSource = TGEGeneralesF.ListasValoresDetallesDependientes(lista);
                this.ddlNichoCapacidad.DataValueField = "IdListaValorDetalle";
                this.ddlNichoCapacidad.DataTextField = "Descripcion";
                this.ddlNichoCapacidad.DataBind();
                this.ddlNichoCapacidad.Enabled = true;
                AyudaProgramacion.AgregarItemSeleccione(this.ddlNichoCapacidad, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                this.ddlNichoCapacidad.Items.Clear();
                this.ddlNichoCapacidad.Enabled = false;
                AyudaProgramacion.AgregarItemSeleccione(ddlNichoCapacidad, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        private void CargarCombos()
        {
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

            AyudaProgramacion.AgregarItemSeleccione(this.ddlNichoCapacidad, ObtenerMensajeSistema("SeleccioneOpcion"));

            AyudaProgramacion.AgregarItemSeleccione(this.ddlPanteon, ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();

            //TextBox fecha = (TextBox)BuscarControlRecursivo(this.Page, "txtFechaAlta");           
            //if(String.IsNullOrWhiteSpace(fecha.Text))
            //parametros.FechaEvento = Convert.ToDateTime(fecha.Text);
            this.pnlGrilla.Visible = true;
            MiAccion = Acciones.Seleccionar;
            parametros.IdNicho = 0;
            parametros.Codigo = "";
            parametros.Estado.IdEstado = 1;
            parametros.Panteon.IdPanteon = ddlPanteon.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlPanteon.SelectedValue);
            parametros.TipoNicho.IdTipoNicho = ddlTipoNicho.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTipoNicho.SelectedValue);
            parametros.NichoCapacidad.IdNichosCapacidad = ddlNichoCapacidad.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlNichoCapacidad.SelectedValue);
            parametros.NichoUbicacion.IdNichosUbicacion = 0;
            parametros.NichoSubUbicacion.IdNichosSubUbicacion = 0;
            parametros.Panteon.Cementerio.IdCementerio = ddlCementerio.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlCementerio.SelectedValue);

            this.CargarLista(parametros);
        }
        #region GV
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Cancelar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idNicho = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            NCHNichos parametros = this.BusquedaParametrosObtenerValor<NCHNichos>();

            if (e.CommandName == Gestion.Cancelar.ToString())
            {
                MiAccion = Acciones.Seleccionar;
                this.LimpiarLista();
                hdfIdNicho.Value = string.Empty;
                this.btnBuscar_Click(sender, new EventArgs());
                this.pnlBuscador.Visible = true;
            }
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                NCHNichos nichoSeleccionado = new NCHNichos();
                nichoSeleccionado.IdNicho = idNicho;
                hdfIdNicho.Value = idNicho.ToString();
                MiAccion = Acciones.Cancelar;
                this.LimpiarLista();
                this.CargarLista(nichoSeleccionado);
                this.pnlBuscador.Visible = false;
                AfiAfiliados afiliado = new PaginaAfiliados().Obtener(this.MiSessionPagina);
                nichoSeleccionado.Filtro = afiliado.IdAfiliado.ToString();
                this.ImporteNichos(nichoSeleccionado);
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton cancelar = (ImageButton)e.Row.FindControl("btnCancelar");
                //ImageButton imprimir = (ImageButton)e.Row.FindControl("btnImprimir");

                switch (MiAccion)
                {
                    case Acciones.Seleccionar:
                        consultar.Visible = true;
                        cancelar.Visible = false;
                        break;
                    case Acciones.Cancelar:
                        consultar.Visible = false;
                        cancelar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            NCHNichos parametros = BusquedaParametrosObtenerValor<NCHNichos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            BusquedaParametrosGuardarValor<NCHNichos>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = MiNicho;
            gvDatos.DataBind();
        }
        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MiNicho = OrdenarGrillaDatos<DataTable>(MiNicho, e);
            gvDatos.DataSource = MiNicho;
            gvDatos.DataBind();
        }
        private void CargarLista(NCHNichos pParametro)
        {        
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            if (pParametro.IdNicho == 0)
                this.MiNicho = NichosF.NichosObtenerDisponibles(pParametro);
            else
                this.MiNicho = NichosF.NichosObtenerListaGrilla(pParametro);

            this.gvDatos.DataSource = this.MiNicho;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);
        }
        private void LimpiarLista()
        {
            this.gvDatos.DataSource = null;
            this.gvDatos.DataBind();
        }
            #endregion

        private void ImporteNichos(NCHNichos pParametro)
        {
            decimal importeDecimal = Convert.ToDecimal(NichosF.NichosObtenerImporte(pParametro).Rows[0]["ImporteCargo"]);
            if(importeDecimal > 0)
            {              
                string script = string.Format("SetearImporteNichos({0});", importeDecimal);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "CalcularImporteNichos", script, true);
            }
        }
    }
} 