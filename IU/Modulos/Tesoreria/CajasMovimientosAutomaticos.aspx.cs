using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using Tesorerias.Entidades;
using System.Collections.Generic;
using Tesorerias;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Comunes.Entidades;
using Afiliados.Entidades;
using Afiliados;

namespace IU.Modulos.Tesoreria
{
    public partial class CajasMovimientosAutomaticos : PaginaCajas
    {
      
        private DataTable MisCajasMovimientosPendientes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PaginaCajasMisCajasMovimientosPendientes"]; }
            set { Session[this.MiSessionPagina + "PaginaCajasMisCajasMovimientosPendientes"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                this.btnSocioOperaciones.Visible = this.ValidarPermiso("CajasAfiliadosListar.aspx");
                this.btnAgregarMovimiento.Visible = this.ValidarPermiso("CajasMovimientosAgregar.aspx");
                this.btnAgregarOrdenescobrosFacturas.Visible = this.ValidarPermiso("CajasOrdenesCobrosFacturas.aspx");
                this.MiCajaMovimientoPendiente = new TESCajasMovimientos();
                //this.CargarCombos();
                this.CargarGrilla(new TGETiposOperaciones());
                this.CargarCombos();
                this.CargarMenu();
                hdfIdUsuarioEvento.Value = this.UsuarioActivo.IdUsuarioEvento.ToString();
            }
        }

        private void CargarMenu()
        {
            
        }

        private void CargarGrilla(TGETiposOperaciones pTipoOperacion)
        {
            TESTesoreriasFiltro filtro = new TESTesoreriasFiltro();
            filtro.Filial.IdFilial = this.MiCaja.Tesoreria.Filial.IdFilial;
            filtro.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
            filtro.IdTipoOperacion = pTipoOperacion.IdTipoOperacion;
            this.MisCajasMovimientosPendientes = TesoreriasF.CajasObtenerMovimientosPendientes(filtro);
            if (pTipoOperacion.IdTipoOperacion > 0)
            {
                this.MisCajasMovimientosPendientes = this.MisCajasMovimientosPendientes.AsEnumerable().Where(x => x.Field<int>("TipoOperacionIdTipoOperacion") == pTipoOperacion.IdTipoOperacion).CopyToDataTable();
            }
            this.gvDatos.DataSource = this.MisCajasMovimientosPendientes.Rows.Count > 0 ? this.MisCajasMovimientosPendientes : null;
            this.gvDatos.DataBind();
            this.UpdatePanel1.Update();
        }

        private void CargarCombos()
        {
            //this.ddlTiposOperaciones.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(EnumTGETiposFuncionalidades.CajasMovimientos);
            // this.ddlTiposOperaciones.DataSource = this.MisCajasMovimientosPendientes.AsEnumerable().Select(x=>x.Field<string>("TipoOperacion")).GroupBy(x=>x..Field<string>("TipoOperacion")).Select(x=>x.First());
            if (this.MisCajasMovimientosPendientes.Rows.Count > 0)
            {
                DataTable filter = this.MisCajasMovimientosPendientes.AsEnumerable().GroupBy(r => new { TipoOperacionTipoOperacion = r.Field<string>("TipoOperacionTipoOperacion") }).Select(g => g.First()).CopyToDataTable();
                this.ddlTiposOperaciones.DataSource = filter;
                this.ddlTiposOperaciones.DataValueField = "TipoOperacionIdTipoOperacion";
                this.ddlTiposOperaciones.DataTextField = "TipoOperacionTipoOperacion";
                this.ddlTiposOperaciones.DataBind();
                this.ddlTiposOperaciones.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
                this.ddlTiposOperaciones.SelectedValue = "-1";
            }
            else
            {
                ddlTiposOperaciones.Items.Add("Seleccione una opcion");
            }
        }

        protected void ddlTiposOperaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTiposOperaciones.SelectedValue))
            {
                TGETiposOperaciones tipoOperacion = new TGETiposOperaciones();
                tipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTiposOperaciones.SelectedValue);
                this.CargarGrilla(tipoOperacion);
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            int IdRefTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdRefTipoOperacion"].ToString());
            int TipoOperacionIdTipoOperacion = Convert.ToInt32(((GridView)sender).DataKeys[index]["TipoOperacionIdTipoOperacion"].ToString());
            DataRow fila = this.MisCajasMovimientosPendientes.AsEnumerable().Where(x => x.Field<int>("IdRefTipoOperacion") == IdRefTipoOperacion
            && x.Field<int>("TipoOperacionIdTipoOperacion") == TipoOperacionIdTipoOperacion).First();
            this.MiCajaMovimientoPendiente = new TESCajasMovimientos();
            Servicio.AccesoDatos.Mapeador.SetearEntidadPorFila(fila, this.MiCajaMovimientoPendiente);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                //this.MisParametrosUrl = new Hashtable();
                //this.MisParametrosUrl.Add("IdRefTipoOperacion", IdRefTipoOperacion);
                //this.MisParametrosUrl.Add("TipoOperacionIdTipoOperacion", TipoOperacionIdTipoOperacion);
                string url = "~/Modulos/Tesoreria/CajasMovimientosConfirmar.aspx";
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
            //}
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                object suma;
                Label lblRegistros = (Label)e.Row.FindControl("lblRegistros");
                suma = this.MisCajasMovimientosPendientes.Rows.Count;
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), suma);
            }
        }

        protected void btnAgregarMovimiento_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosAgregar.aspx"), true);
        }

        protected void btnAgregarOrdenescobrosFacturas_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasOrdenesCobrosFacturas.aspx"), true);
        }

        protected void btnCompraVenta_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasMovimientosCompraVentaMoneda.aspx"), true);
        }

        protected void btnSocioOperaciones_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAfiliadosListar.aspx"), true);
        }

        protected void btnIngresarSocio_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdfIdAfiliado.Value))
            {
                AfiAfiliados afiliados = new AfiAfiliados();
                afiliados.IdAfiliado = Convert.ToInt32(hdfIdAfiliado.Value);
                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                paginaAfi.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatos(afiliados));

                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Tesoreria/CajasAfiliadosInicio.aspx"), true);
            }
            else
                return;
        }
    }
}
