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
    public partial class ClientesBuscar : ControlesSeguros
    {
        private List<AfiAfiliados> MisAfiliados
        {
            get { return (List<AfiAfiliados>)Session[this.MiSessionPagina + "AfiliadosBuscarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "AfiliadosBuscarMisAfiliados"] = value; }
        }

        private List<AfiAfiliados> MisAfiliadosSeleccionados
        {
            get { return (List<AfiAfiliados>)Session[this.MiSessionPagina + "AfiliadosBuscarMisAfiliadosSeleccionados"]; }
            set { Session[this.MiSessionPagina + "AfiliadosBuscarMisAfiliadosSeleccionados"] = value; }
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
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtMatricula, this.btnBuscar);

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
            this.MisAfiliadosSeleccionados = new List<AfiAfiliados>();
            this.pnlBuscar.Visible = true;
            this.Iniciar(pLimpiar);
        }

        public void IniciarControl(AfiAfiliados pAfiliado, bool pPanelBuscar, EnumAfiliadosTipos pAfiliadoTipo, bool pLimpiarDatos)
        {
            this.IniciarControl(pAfiliado, pPanelBuscar, pAfiliadoTipo, pLimpiarDatos, new List<AfiAfiliados>());
        }

        public void IniciarControl(AfiAfiliados pAfiliado, bool pPanelBuscar, EnumAfiliadosTipos pAfiliadoTipo, bool pLimpiarDatos, List<AfiAfiliados> pFiltrarAfiliados)
        {
            this.MisAfiliadosSeleccionados = pFiltrarAfiliados;
            this.pnlBuscar.Visible = pPanelBuscar;
            ////AfiAfiliados filtro = new AfiAfiliados();
            ////filtro.IdAfiliadoRef = pAfiliado.IdAfiliado;
            ////filtro.AfiliadoTipo.IdAfiliadoTipo = (int)pAfiliadoTipo;
            ////filtro.Estado.IdEstado = (int)EstadosTodos.Todos;
            ////filtro.TipoDocumento.IdTipoDocumento = (int)EstadosTodos.Todos;
            ////this.MisAfiliados = AfiliadosF.AfiliadosObtenerListaFiltro(filtro);
            ////AyudaProgramacion.CargarGrillaListas<AfiAfiliados>(this.MisAfiliados, false, this.gvDatos, false);
            this.txtNumeroSocio.Text = pAfiliado.IdAfiliado.ToString();
            this.txtNumeroDocumento.Text = pAfiliado.NumeroDocumento.ToString();
            //this.ddlTipoDocumento.SelectedValue = pAfiliado.TipoDocumento.IdTipoDocumento.ToString();
            this.txtApellido.Text = pAfiliado.RazonSocial;
            this.txtMatricula.Text = pAfiliado.MatriculaIAF.ToString();
            this.ddlEstados.SelectedValue = pAfiliado.Estado.IdEstado.ToString();
            //this.CargarLista();
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
            this.ddlEstados.SelectedValue = ((int)EstadosAfiliados.Vigente).ToString();

        }

        private void CargarLista()
        {
            ////pAfiliado.UsuarioActivo = this.UsuarioActivo;
            AfiAfiliados pAfiliado = new AfiAfiliados();
            pAfiliado.IdAfiliado = this.txtNumeroSocio.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumeroSocio.Text);
            pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pAfiliado.Apellido = this.txtApellido.Text.Trim();
            pAfiliado.MatriculaIAF = this.txtMatricula.Text == string.Empty ? 0 : Convert.ToInt64(this.txtMatricula.Text);
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pAfiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Clientes;
            //pAfiliado.BusquedaParametros = true;
            //this.BusquedaParametrosGuardarValor<AfiAfiliados>(pAfiliado);
            this.MisAfiliados = AfiliadosF.AfiliadosObtenerListaFiltro(pAfiliado);
            this.MisAfiliados = AyudaProgramacion.AcomodarIndices<AfiAfiliados>(this.MisAfiliados.Where(p => !this.MisAfiliadosSeleccionados.Any(p2 => p2.IdAfiliado == p.IdAfiliado)).ToList());
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
