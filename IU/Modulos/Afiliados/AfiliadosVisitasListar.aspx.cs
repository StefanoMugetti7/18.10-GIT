using Afiliados;
using Afiliados.Entidades.Entidades;
using Comunes.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Afiliados
{
    public partial class AfiliadosVisitasListar : PaginaSegura
    {
        private DataTable MiAfiliadoVisita
        {
            get { return (DataTable)Session[this.MiSessionPagina + "AfiliadosVisitasListarMiAfiliadoVisita"]; }
            set { Session[this.MiSessionPagina + "AfiliadosVisitasListarMiAfiliadoVisita"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                paginaAfi.Guardar(this.MiSessionPagina, new AfiAfiliadosVisitas());
                //paginaAfi.MiAfiliado = new AfiAfiliados();

                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtApellido, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNombre, this.btnBuscar);
                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroDocumento, this.btnBuscar);

                //this.btnAgregar.Visible = this.ValidarPermiso("AfiliadosAgregar.aspx");
                //this.CargarCombos();

                AfiAfiliadosVisitas pAfiliado = this.BusquedaParametrosObtenerValor<AfiAfiliadosVisitas>();
                if (pAfiliado.BusquedaParametros)
                {
                    //this.ddlNumeroDocumento.SelectedValue = pAfiliado.NumeroDocumento.ToString();
                    //this.ddlTipoDocumento.SelectedValue = pAfiliado.TipoDocumento.IdTipoDocumento.ToString();
                    //this.txtNombre.Text = pAfiliado.Nombre;
                    //this.txtApellido.Text = pAfiliado.Apellido;
                    this.txtFechaDesde.Text = pAfiliado.FechaDesde.HasValue ? pAfiliado.FechaDesde.Value.ToShortDateString() : string.Empty;
                    this.txtFechaHasta.Text = pAfiliado.FechaHasta.HasValue ? pAfiliado.FechaHasta.Value.ToShortDateString() : string.Empty;
                    this.CargarLista(pAfiliado);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            AfiAfiliadosVisitas pAfiliado = this.BusquedaParametrosObtenerValor<AfiAfiliadosVisitas>();
            CargarLista(pAfiliado);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosVisitasAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            AfiAfiliadosVisitas afiliado = new AfiAfiliadosVisitas();
            afiliado.IdAfiliado = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdAfiliado"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdAfiliado", afiliado.IdAfiliado);

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosVisitasDatos.aspx"), true);
            }
        }
        
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                //ibtnConsultar.Visible = this.ValidarPermiso("AfiliadosVisitasDatos.aspx");

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiAfiliadoVisita.Rows.Count);

            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            AfiAfiliadosVisitas pAfiliado = this.BusquedaParametrosObtenerValor<AfiAfiliadosVisitas>();
            pAfiliado.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<AfiAfiliadosVisitas>(pAfiliado);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MiAfiliadoVisita;
            gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            this.ddlTipoDocumento.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlTipoDocumento.SelectedValue = "-1";
        }

        private void CargarLista(AfiAfiliadosVisitas pAfiliado)
        {
            //pAfiliado.UsuarioActivo = this.UsuarioActivo;            
            //pAfiliado.NumeroDocumento = Convert.ToInt64(this.ddlNumeroDocumento.SelectedValue);
            //pAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            //pAfiliado.Nombre = this.txtNombre.Text;
            //pAfiliado.Apellido = this.txtApellido.Text;

            pAfiliado.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pAfiliado.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pAfiliado.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<AfiAfiliadosVisitas>(pAfiliado);
            this.MiAfiliadoVisita = AfiliadosF.AfiliadosVisitasObtenerGrilla(pAfiliado);
            this.gvDatos.DataSource = this.MiAfiliadoVisita;
            this.gvDatos.PageIndex = pAfiliado.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}