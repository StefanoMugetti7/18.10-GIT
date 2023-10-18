using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using System.Collections;
using Afiliados;
using Generales.FachadaNegocio;
using Medicina;

namespace IU.Modulos.Medicina
{
    public partial class PrestadoresListar : PaginaSegura
    {
        private List<MedPrestadores> MisPrestadores
        {
            get { return (List<MedPrestadores>)Session[this.MiSessionPagina + "PrestadoresListarMisAfiliados"]; }
            set { Session[this.MiSessionPagina + "PrestadoresListarMisAfiliados"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNombre, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtMatricula, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("PrestadoresAgregar.aspx");
                this.CargarCombos();

                MedPrestadores parametros = this.BusquedaParametrosObtenerValor<MedPrestadores>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlTipoDocumento.SelectedValue = parametros.TipoDocumento.IdTipoDocumento.ToString();
                    this.txtNumeroDocumento.Text = parametros.NumeroDocumento.ToString();
                    this.txtApellido.Text = parametros.Apellido;
                    this.txtNombre.Text = parametros.Nombre;
                    this.ddlEstados.SelectedValue = parametros.Estado.IdEstado.ToString();
                    this.txtMatricula.Text = parametros.Matricula.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            MedPrestadores parametros = this.BusquedaParametrosObtenerValor<MedPrestadores>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestadoresAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            MedPrestadores prestador = this.MisPrestadores[indiceColeccion];

            this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdPrestador", prestador.IdPrestador);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestadoresConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Modificar.ToString())
            {
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Medicina/PrestadoresModificar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("PrestadoresConsultar.aspx");
                ibtnModificar.Visible = this.ValidarPermiso("PrestadoresModificar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPrestadores.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            MedPrestadores parametros = this.BusquedaParametrosObtenerValor<MedPrestadores>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<MedPrestadores>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisPrestadores;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisPrestadores = this.OrdenarGrillaDatos<MedPrestadores>(this.MisPrestadores, e);
            this.gvDatos.DataSource = this.MisPrestadores;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            this.ddlTipoDocumento.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "0"));
            this.ddlTipoDocumento.SelectedValue = "0";

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstados.SelectedValue = "-1";
        }

        private void CargarLista(MedPrestadores pPrestador)
        {

            pPrestador.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pPrestador.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            pPrestador.Apellido = this.txtApellido.Text.Trim();
            pPrestador.Nombre = this.txtNombre.Text.Trim();
            pPrestador.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pPrestador.BusquedaParametros = true;
            pPrestador.Matricula = txtMatricula.Text.Trim();
            this.BusquedaParametrosGuardarValor<MedPrestadores>(pPrestador);
            this.MisPrestadores = MedicinaF.PrestadoresObtenerListaFiltro(pPrestador);
            this.gvDatos.DataSource = this.MisPrestadores;
            this.gvDatos.PageIndex = pPrestador.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
