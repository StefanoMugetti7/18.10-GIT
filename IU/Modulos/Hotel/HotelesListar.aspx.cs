using Comunes.Entidades;
using Generales.FachadaNegocio;
using Hoteles;
using Hoteles.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel
{
    public partial class HotelesListar : PaginaSegura
    {
        private DataTable MisHoteles
        {
            get { return (DataTable)Session[this.MiSessionPagina + "HotelesListarMisHoteles"]; }
            set { Session[this.MiSessionPagina + "HotelesListarMisHoteles"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                this.CargarCombos();


                HTLHoteles parametros = this.BusquedaParametrosObtenerValor<HTLHoteles>();
                if (parametros.BusquedaParametros)
                {
                    CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            HTLHoteles parametros = this.BusquedaParametrosObtenerValor<HTLHoteles>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HotelesAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            HTLHoteles reserva = new HTLHoteles();
            reserva.IdHotel = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdHotel"].ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdHotel", reserva.IdHotel);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Hotel/HotelesModificar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                DataRowView dr = (DataRowView)e.Row.DataItem;

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {

            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            HTLHoteles parametros = BusquedaParametrosObtenerValor<HTLHoteles>();
            parametros.IndiceColeccion = e.NewPageIndex;
            BusquedaParametrosGuardarValor<HTLHoteles>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = MisHoteles;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            MisHoteles = OrdenarGrillaDatos<DataTable>(MisHoteles, e);
            gvDatos.DataSource = MisHoteles;
            gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            gvDatos.AllowPaging = false;
            gvDatos.DataSource = MisHoteles;
            gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", gvDatos);
        }

        private void CargarCombos()
        {
            this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscal.DataTextField = "Descripcion";
            this.ddlCondicionFiscal.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales; //TGEGeneralesF.FilialesObenerLista();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            DateTime StartTime = DateTime.ParseExact("00:00", "HH:mm", null);
            DateTime EndTime = DateTime.ParseExact("23:55", "HH:mm", null);
            TimeSpan Interval = new TimeSpan(0, 30, 0);
            ddlHoraIngreso.Items.Clear();
            ddlHoraEgreso.Items.Clear();
            while (StartTime <= EndTime)
            {
                ddlHoraIngreso.Items.Add(new ListItem(StartTime.ToString("HH:mm"), StartTime.ToString("HH:mm")));
                ddlHoraEgreso.Items.Add(new ListItem(StartTime.ToString("HH:mm"), StartTime.ToString("HH:mm")));
                StartTime = StartTime.Add(Interval);
            }
            AyudaProgramacion.InsertarItemSeleccione(this.ddlHoraIngreso, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            AyudaProgramacion.InsertarItemSeleccione(this.ddlHoraEgreso, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void CargarLista(HTLHoteles pHoteles)
        {
            pHoteles.Descripcion = txtDescripcion.Text;
            //pHoteles.Afiliado.CondicionFiscal.IdCondicionFiscal = ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlCondicionFiscal.SelectedValue);
            pHoteles.Estado.IdEstado = ddlEstado.SelectedValue == string.Empty ? -1 : Convert.ToInt32(ddlEstado.SelectedValue);
            pHoteles.Filial.IdFilial = ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlFilial.SelectedValue);

            pHoteles.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pHoteles.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<HTLHoteles>(pHoteles);
            this.MisHoteles = HotelesF.HotelesObtenerListaGrilla(pHoteles);
            this.gvDatos.DataSource = this.MisHoteles;
            this.gvDatos.PageIndex = pHoteles.IndiceColeccion;
            this.gvDatos.DataBind();
            AyudaProgramacion.FixGridView(gvDatos);

            if (this.MisHoteles.Rows.Count > 0)
            {
                btnExportarExcel.Visible = true;
            }
            else
            {
                btnExportarExcel.Visible = false;
            }
        }
    }
}
