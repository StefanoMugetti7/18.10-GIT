using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Afiliados.Entidades;
using Comunes.Entidades;
using Afiliados;
using Generales.FachadaNegocio;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfiliadosBuscar : ControlesSeguros
    {
        private List<AfiAfiliados> MisAfiliados
        {
            get { return (List<AfiAfiliados>)Session[this.MiSessionPagina + "AfiliadosBuscarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "AfiliadosBuscarMisAfiliados"] = value; }
        }

        public delegate void AfiliadosBuscarEventHandler(AfiAfiliados e);
        public event AfiliadosBuscarEventHandler AfiliadosBuscarSeleccionar;
        public delegate void AfiliadosVolverEventHandler();
        //public event AfiliadosVolverEventHandler AfiliadosVolver;
        public delegate void AfiliadosEventosEventHandler();
        public event AfiliadosEventosEventHandler AfiliadosEventos;
        
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNombre, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);

                this.CargarCombos();
                //AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
                //if (parametros.BusquedaParametros)
                //{
                //    this.txtNumeroSocio.Text = parametros.NumeroSocio;
                //    this.ddlTipoDocumento.SelectedValue = parametros.TipoDocumento.IdTipoDocumento.ToString();
                //    this.txtNumeroDocumento.Text = parametros.NumeroDocumento.ToString();
                //    this.txtApellido.Text = parametros.Apellido;
                //    this.txtNombre.Text = parametros.Nombre;
                //    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                //    this.chkFamiliares.Checked = parametros.IdAfiliadoRef == 0 ? false : true;
                //    this.CargarLista(parametros);
                //}
            }
        }

        public void IniciarControl(bool pLimpiar)
        {
            this.pnlBuscar.Visible = true;
            this.Iniciar(pLimpiar);
        }

        public void IniciarControl(AfiAfiliados pAfiliado, bool pPanelBuscar, EnumAfiliadosTipos pAfiliadoTipo, bool pLimpiarDatos)
        {
            this.pnlBuscar.Visible = pPanelBuscar;
            AfiAfiliados filtro = new AfiAfiliados();
            filtro.IdAfiliadoRef = pAfiliado.IdAfiliado;
            filtro.AfiliadoTipo.IdAfiliadoTipo = (int)pAfiliadoTipo;
            filtro.Estado.IdEstado = (int)EstadosTodos.Todos;
            filtro.TipoDocumento.IdTipoDocumento = (int)EstadosTodos.Todos;
            this.MisAfiliados = AfiliadosF.AfiliadosObtenerListaFiltro(filtro);
            AyudaProgramacion.CargarGrillaListas<AfiAfiliados>(this.MisAfiliados, false, this.gvDatos, false);

            this.Iniciar(pLimpiarDatos);
        }

        private void Iniciar(bool pLimpiarDatos)
        {
            if (pLimpiarDatos)
            {
                this.MisAfiliados = new List<AfiAfiliados>();
                this.gvDatos.DataSource = this.MisAfiliados;
                this.gvDatos.DataBind();
            }
            this.txtNumeroSocio.Text = string.Empty;
            this.txtNumeroDocumento.Text = string.Empty;
            this.txtApellido.Text = string.Empty;
            this.txtNombre.Text = string.Empty;
            this.chkFamiliares.Checked = false;
            this.UpdatePanel1.Update();
            if (this.AfiliadosEventos != null)
                this.AfiliadosEventos();
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            //AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            CargarLista();
            //this.mpePopUp.Show();
            if (this.AfiliadosEventos != null)
                this.AfiliadosEventos();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            AfiAfiliados afiliado = this.MisAfiliados[indiceColeccion];

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                if (this.AfiliadosBuscarSeleccionar != null)
                {
                    this.AfiliadosBuscarSeleccionar(afiliado);
                    //this.mpePopUp.Hide();
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");

            //    //Permisos btnEliminar
            //    ibtnConsultar.Visible = this.ValidarPermiso("AfiliadosConsultar.aspx");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisAfiliados.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            //parametros.IndiceColeccion = e.NewPageIndex;
            //this.BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisAfiliados;
            gvDatos.DataBind();

            //this.mpePopUp.Show();
            if (this.AfiliadosEventos != null)
                this.AfiliadosEventos();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisAfiliados = this.OrdenarGrillaDatos<AfiAfiliados>(this.MisAfiliados, e);
            this.gvDatos.DataSource = this.MisAfiliados;
            this.gvDatos.DataBind();

            //this.mpePopUp.Show();
            if (this.AfiliadosEventos != null)
                this.AfiliadosEventos();
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

        }

        private void CargarLista()
        {
            ////pAfiliado.UsuarioActivo = this.UsuarioActivo;
            AfiAfiliados pAfiliado = new AfiAfiliados();
            pAfiliado.NumeroSocio = this.txtNumeroSocio.Text.Trim();
            pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.Apellido = this.txtApellido.Text.Trim();
            pAfiliado.Nombre = this.txtNombre.Text.Trim();
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pAfiliado.IdAfiliadoRef = this.chkFamiliares.Checked ? -1 : 0;
            //pAfiliado.BusquedaParametros = true;
            //this.BusquedaParametrosGuardarValor<AfiAfiliados>(pAfiliado);
            this.MisAfiliados = AfiliadosF.AfiliadosObtenerListaFiltro(pAfiliado);
            this.gvDatos.DataSource = this.MisAfiliados;
            //this.gvDatos.PageIndex = pAfiliado.IndiceColeccion;
            this.gvDatos.DataBind();

            //if (this.MisAfiliados.Count > 0)
            //    btnExportarExcel.Visible = true;
            //else
            //    btnExportarExcel.Visible = false;
        }
    }
}
