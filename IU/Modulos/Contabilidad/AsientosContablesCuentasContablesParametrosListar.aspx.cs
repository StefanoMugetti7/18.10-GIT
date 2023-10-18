using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using System.Collections;
using Generales.FachadaNegocio;
using Generales.Entidades;
using Contabilidad;
using System.Data;

namespace IU.Modulos.Contabilidad
{
    public partial class AsientosContablesCuentasContablesParametrosListar : PaginaSegura
    {
        //private List<CtbAsientosContablesCuentasContablesParametros> MisAsientosCuentasParametros
        //{
        //    get { return (List<CtbAsientosContablesCuentasContablesParametros>)Session[this.MiSessionPagina + "MisAsientosCuentasParametros"]; }
        //    set { Session[this.MiSessionPagina + "MisAsientosCuentasParametros"] = value; }
        //}

        private DataTable MisAsientosCuentasParametros
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MisAsientosCuentasParametros"]; }
            set { Session[this.MiSessionPagina + "MisAsientosCuentasParametros"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtDenominacion, this.btnBuscar);
                this.btnAgregar.Visible = this.ValidarPermiso("BancosCuentasAgregar.aspx");
                this.CargarCombos();
                CtbAsientosContablesCuentasContablesParametros parametros = this.BusquedaParametrosObtenerValor<CtbAsientosContablesCuentasContablesParametros>();
                if (parametros.BusquedaParametros)
                {
                    ListItem item = this.ddlTipoValor.Items.FindByValue(parametros.TipoValor.IdTipoValor.ToString());
                    if (item != null && parametros.TipoValor.IdTipoValor > 0)
                        this.ddlTipoValor.SelectedValue = parametros.TipoValor.IdTipoValor.ToString();
                    
                    item = this.ddlFiliales.Items.FindByValue(parametros.Filial.IdFilial.ToString());
                    if (item != null && parametros.Filial.IdFilial > 0)
                        this.ddlFiliales.SelectedValue = parametros.Filial.IdFilial.ToString();

                    item = this.ddlBancos.Items.FindByValue(parametros.BancoCuentaBancoIdBanco.ToString());
                    if (item != null && parametros.BancoCuentaBancoIdBanco > 0)
                        this.ddlBancos.SelectedValue = parametros.BancoCuentaBancoIdBanco.ToString();

                    this.txtDenominacion.Text = parametros.Filtro.ToString();

                    this.ddlMonedas.SelectedValue = parametros.Moneda.IdMoneda == 0 ? string.Empty :  parametros.Moneda.IdMoneda.ToString();
                    this.ddlEstado.SelectedValue = parametros.Estado.IdEstado == 0 ? string.Empty : parametros.Estado.IdEstado.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosContablesCuentasContablesParametrosAgregar.aspx"), true);
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbAsientosContablesCuentasContablesParametros parametros = this.BusquedaParametrosObtenerValor<CtbAsientosContablesCuentasContablesParametros>();
            this.CargarLista(parametros);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Consultar.ToString()
                ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
           // CtbAsientosContablesCuentasContablesParametros asientoParametros = this.MisAsientosCuentasParametros[indiceColeccion];
            
            //this.MisParametrosUrl = new Hashtable();
           //this.MisParametrosUrl.Add("IdAsientoContableCuentaContableParametro", asientoParametros.IdAsientoContableCuentaContableParametro);

            ListaParametros parametros = new ListaParametros(this.MiSessionPagina);
            parametros.Agregar("IdAsientoContableCuentaContableParametro", indiceColeccion);//proveedor.IdProveedor);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosContablesCuentasContablesParametrosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Contabilidad/AsientosContablesCuentasContablesParametrosConsultar.aspx"), true);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //CtbAsientosContablesCuentasContablesParametros asientoParametros = (CtbAsientosContablesCuentasContablesParametros)e.Row.DataItem;

                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("AsientosContablesCuentasContablesParametrosModificar.aspx");
               // consultar.Visible = this.ValidarPermiso("AsientosContablesCuentasContablesParametrosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisAsientosCuentasParametros.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbAsientosContablesCuentasContablesParametros parametros = this.BusquedaParametrosObtenerValor<CtbAsientosContablesCuentasContablesParametros>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbAsientosContablesCuentasContablesParametros>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisAsientosCuentasParametros;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisAsientosCuentasParametros = this.OrdenarGrillaDatos<CtbAsientosContablesCuentasContablesParametros>(this.MisAsientosCuentasParametros, e);
            this.gvDatos.DataSource = this.MisAsientosCuentasParametros;
            this.gvDatos.DataBind();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            this.ddlEstado.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), "-1"));
            this.ddlEstado.SelectedValue = "-1";

            this.ddlMonedas.DataSource = TGEGeneralesF.MonedasObtenerLista();
            this.ddlMonedas.DataValueField = "IdMoneda";
            this.ddlMonedas.DataTextField = "miMonedaDescripcion";
            this.ddlMonedas.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlMonedas, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoValor.DataSource = TGEGeneralesF.TiposValoresObtenerLista();
            this.ddlTipoValor.DataValueField = "IdTipoValor";
            this.ddlTipoValor.DataTextField = "TipoValor";
            this.ddlTipoValor.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoValor, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlBancos.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Bancos);
            this.ddlBancos.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlBancos.DataTextField = "Descripcion";
            this.ddlBancos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlBancos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlFiliales.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            //this.ddlFiliales.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        private void CargarLista(CtbAsientosContablesCuentasContablesParametros pParametro)
        {
            pParametro.BancoCuentaBancoIdBanco = this.ddlBancos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlBancos.SelectedValue);
            pParametro.TipoValor.IdTipoValor = this.ddlTipoValor.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoValor.SelectedValue);
            pParametro.Filial.IdFilial = this.ddlFiliales.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFiliales.SelectedValue);
            pParametro.Moneda.IdMoneda = this.ddlMonedas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMonedas.SelectedValue);
            pParametro.Filtro = this.txtDenominacion.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbAsientosContablesCuentasContablesParametros>(pParametro);
            this.MisAsientosCuentasParametros = ContabilidadF.AsientosContablesCuentasContablesParametrosObtenerListaFiltroDT(pParametro);
            this.gvDatos.DataSource = this.MisAsientosCuentasParametros;
            this.gvDatos.PageIndex = pParametro.IndiceColeccion;
            this.gvDatos.DataBind();
        }
    }
}
