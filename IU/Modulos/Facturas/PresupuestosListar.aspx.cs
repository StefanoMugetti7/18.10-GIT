using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Facturas.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Afiliados.Entidades;
using Afiliados;
using Facturas;
using System.Collections;
using Generales.Entidades;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using CrystalDecisions.Web;
using System.IO;
using Comunes.LogicaNegocio;
using CrystalDecisions.Shared;

namespace IU.Modulos.Facturas
{
    public partial class PresupuestosListar : PaginaSegura
    {
        private DataTable MisDatosGrillas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PresupuestosListarMisDatosGrillas"]; }
            set { Session[this.MiSessionPagina + "PresupuestosListarMisDatosGrillas"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.ctrBuscarClientePopUp.AfiliadosBuscarSeleccionar += new Afiliados.Controles.ClientesBuscarPopUp.AfiliadosBuscarEventHandler(ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar);

            if (!this.IsPostBack)
            {

                //BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtNumeroSocio, this.btnBuscar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtPrefijoNumeroFactura, this.btnBuscar);

                this.btnAgregar.Visible = this.ValidarPermiso("PresupuestosAgregar.aspx");
                this.CargarCombos();

                VTAPresupuestos parametros = this.BusquedaParametrosObtenerValor<VTAPresupuestos>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlNumeroSocio.SelectedValue = parametros.Afiliado.IdAfiliado.ToString();
                    this.txtPrefijoNumeroFactura.Text = parametros.IdPresupuesto.ToString();
                    this.CargarLista(parametros);
                }
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            VTAPresupuestos parametros = this.BusquedaParametrosObtenerValor<VTAPresupuestos>();
            CargarLista(parametros);
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int idPresupuesto = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //VTAPresupuestos factura = this.MisDatosGrillas[id];

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPresupuesto", idPresupuesto);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosModificar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Facturas/PresupuestosConsultar.aspx"), true);
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                TGEArchivos archivo = new TGEArchivos();
                VTAPresupuestos presupuestoPDF = new VTAPresupuestos();
                presupuestoPDF.IdPresupuesto = idPresupuesto;
                presupuestoPDF = FacturasF.PresupuestosObtenerArchivo(presupuestoPDF);
                archivo.Archivo = presupuestoPDF.PresupuestoPDF;
                listaArchivos.Add(archivo);
                TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                string nombreArchivo = string.Concat(empresa.CUIT, "_Presupuesto_", idPresupuesto.ToString().PadLeft(10, '0'), ".pdf");
                ExportPDF.ConvertirArchivoEnPdf(this.UpdatePanel1, listaArchivos, nombreArchivo);               
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton ibtnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
                ImageButton ibtnAutorizar = (ImageButton)e.Row.FindControl("btnAutorizar");

                //Permisos btnEliminar
                ibtnConsultar.Visible = this.ValidarPermiso("FacturasConsultar.aspx");
                //ibtnConsultar.Visible = true;

                //DataRow fila = (DataRow)e.Row.DataItem;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblCantidadRegistros = (Label)e.Row.FindControl("lblCantidadRegistros");
                lblCantidadRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisDatosGrillas.Rows .Count);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<VTAFacturas>(parametros);

            gvDatos.PageIndex = e.NewPageIndex;
            gvDatos.DataSource = this.MisDatosGrillas;
            gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //DataTable dataTable = gvDatos.DataSource as DataTable;
            DataView dataView = new DataView(this.MisDatosGrillas);
            dataView.Sort = e.SortExpression + " " + e.SortDirection.ToString().Substring(0, 3).ToUpper();
            this.gvDatos.DataSource = dataView;
            this.gvDatos.DataBind();

            //this.MisDatosGrillas = this.OrdenarGrillaDatos<VTAFacturas>(this.MisDatosGrillas, e);
            //this.gvDatos.DataSource = this.MisDatosGrillas;
            //this.gvDatos.DataBind();
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MisDatosGrillas;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        //protected void txtNumeroSocio_TextChanged(object sender, EventArgs e)
        //{
        //    string txtNumeroSocio = ((TextBox)sender).Text;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = this.txtNumeroSocio.Text == string.Empty ? 0 : Convert.ToInt32(txtNumeroSocio);
        //    if (parametro.IdAfiliado == 0)
        //        return;

        //    AfiAfiliados Afiliado = AfiliadosF.AfiliadosObtenerDatos(parametro);
        //    if (Afiliado.IdAfiliado != 0)
        //    {
        //        this.MapearObjetoAControlesAfiliado(Afiliado);
        //    }
        //    else
        //    {
        //        parametro.CodigoMensaje = "NumeroClienteNoExiste";
        //        this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}

        //protected void btnBuscarCliente_Click(object sender, EventArgs e)
        //{
        //    this.ctrBuscarClientePopUp.IniciarControl(true);
        //}

        //protected void btnLimpiar_Click(object sender, EventArgs e)
        //{
        //    VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
        //    parametros.Afiliado = new AfiAfiliados();
        //    this.txtNumeroSocio.Text = string.Empty;
        //    this.txtSocio.Text = string.Empty;
        //    this.BusquedaParametrosGuardarValor<VTAFacturas>(parametros);
        //    this.btnLimpiar.Visible = false;
        //    this.btnBuscarSocio.Visible = true;
        //    this.UpdatePanel1.Update();
        //}

        //void ctrBuscarClientePopUp_AfiliadosBuscarSeleccionar(AfiAfiliados pAfiliado)
        //{
        //    this.MapearObjetoAControlesAfiliado(pAfiliado);
        //    VTAFacturas parametros = this.BusquedaParametrosObtenerValor<VTAFacturas>();
        //    parametros.Afiliado = pAfiliado;
        //    this.BusquedaParametrosGuardarValor<VTAFacturas>(parametros);
        //    this.btnLimpiar.Visible = true;
        //    this.btnBuscarSocio.Visible = false;
        //    this.UpdatePanel1.Update();
        //}

        //private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        //{
        //    this.txtNumeroSocio.Text = pAfiliado.IdAfiliado.ToString();
        //    this.txtSocio.Text = pAfiliado.Apellido;
        //}


        //private void MapearObjetoAControlesAfiliado(AfiAfiliados pAfiliado)
        //{
        //    this.ddlNumeroSocio.Items.Add(new ListItem(pAfiliado.DescripcionAfiliado, pAfiliado.IdAfiliado.ToString()));
        //    this.ddlNumeroSocio.SelectedValue = pAfiliado.IdAfiliado.ToString();

        //    //if (this.MisParametrosUrl.Contains("IdAfiliado"))
        //    //    this.MisParametrosUrl["IdAfiliado"] = pAfiliado.IdAfiliado;

        //    //this.txtSocio.Text = pAfiliado.RazonSocial;
        //}

        //protected void button_Click(object sender)
        //{
        //    string txtNumeroSocio = this.hdfIdAfiliado.Value;
        //    AfiAfiliados parametro = new AfiAfiliados();
        //    parametro.IdAfiliado = txtNumeroSocio == string.Empty ? 0 : Convert.ToInt32(txtNumeroSocio);
        //    parametro = AfiliadosF.AfiliadosObtenerDatos(parametro);
        //    if (parametro.IdAfiliado != 0)
        //    {

        //        //this.ddlNumeroSocio.Items.Add(new ListItem(parametro.DescripcionAfiliado.ToString(), parametro.IdAfiliado.ToString()));
        //        this.ddlNumeroSocio.SelectedValue = parametro.IdAfiliado.ToString();
        //        //this.ddlNumeroSocio.SelectedIndex = parametro.DescripcionAfiliado;
        //        //this.MapearObjetoAControlesAfiliado(parametroFacturas.Afiliado);
        //    }
        //    else
        //    {
        //        //this.txtSocio.Text = string.Empty;
        //        //parametro.CodigoMensaje = "NumeroSocioNoExiste";
        //        //this.UpdatePanel1.Update();
        //        //this.MostrarMensaje(parametro.CodigoMensaje, true);
        //    }
        //}

        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.Items.Add(new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));
            this.ddlEstados.SelectedValue = ((int)EstadosTodos.Todos).ToString();

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales;
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();
        }

        private void CargarLista(VTAPresupuestos pPresupuesto)
        {
            pPresupuesto.Afiliado.IdAfiliado = this.hdfIdAfiliado.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdAfiliado.Value);
            if (pPresupuesto.Afiliado.IdAfiliado != 0)
            {
                this.ddlNumeroSocio.Items.Add(new ListItem(hdfRazonSocial.Value, hdfIdAfiliado.Value));
                this.ddlNumeroSocio.SelectedValue = hdfIdAfiliado.Value;
            }
            else
            {
                AyudaProgramacion.AgregarItemSeleccione(ddlNumeroSocio, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            pPresupuesto.IdPresupuesto = this.txtPrefijoNumeroFactura.Text==string.Empty ? 0 : Convert.ToInt32(this.txtPrefijoNumeroFactura.Text);
            pPresupuesto.FechaDesde = this.txtFechaDesde.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaDesde.Text);
            pPresupuesto.FechaHasta = this.txtFechaHasta.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaHasta.Text);
            pPresupuesto.IdFilial = this.ddlFilial.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pPresupuesto.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);

            pPresupuesto.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<VTAPresupuestos>(pPresupuesto);
            this.MisDatosGrillas = FacturasF.PresupuestosObtenerListaGrilla(pPresupuesto);
            this.gvDatos.DataSource = this.MisDatosGrillas;
            this.gvDatos.PageIndex = pPresupuesto.IndiceColeccion;
            this.gvDatos.DataBind();

            if (this.MisDatosGrillas.Rows.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }
    }
}
