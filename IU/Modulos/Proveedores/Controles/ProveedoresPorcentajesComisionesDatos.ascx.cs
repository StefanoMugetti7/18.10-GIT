using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Proveedores.Entidades;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Proveedores;
using Generales.Entidades;

namespace IU.Modulos.Proveedores.Controles
{
    public partial class ProveedoresPorcentajesComisionesDatos : ControlesSeguros
    {
        private CapProveedoresPorcentajesComisiones MiProvPorcComi
        {
            get { return (CapProveedoresPorcentajesComisiones)Session[this.MiSessionPagina + "MiProvPorcComi"]; }
            set { Session[this.MiSessionPagina + "MiProvPorcComi"] = value; }
        }



    public delegate void ModificarDatosAceptarEventHandler(object sender, CapProveedoresPorcentajesComisiones e);
    public event ModificarDatosAceptarEventHandler ModificarDatosAceptar;

    public delegate void ModificarDatosCancelarEventHandler();
    public event ModificarDatosCancelarEventHandler ModificarDatosCancelar;

    protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                //this.MiProvPorcComi = new CapProveedoresPorcentajesComisiones();
            }
        }

        public void IniciarControl(CapProveedoresPorcentajesComisiones pParametro)
        {
            this.CargarCombos();
            this.txtFechaInicio.Text = DateTime.Now.ToShortDateString();
            this.txtPorcentaje.Decimal = 0;
            this.MiProvPorcComi = pParametro;
       
        }

        public void IniciarControl(CapProveedoresPorcentajesComisiones pParametro, Gestion pGestion)
        {
            //OJO CON ESTO!
            //CapProveedoresPorcentajesComisiones parametros = this.BusquedaParametrosObtenerValor<CapProveedores>();
            //if (pParametro.IdProveedor != parametros.IdProveedor)
            //{
            //    parametros = new CapProveedoresPorcentajesComisiones();
            //    this.BusquedaParametrosGuardarValor<CapProveedoresPorcentajesComisiones>(parametros);
            //}


            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiProvPorcComi = pParametro;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:

                    break;
                case Gestion.Anular:
                    this.MiProvPorcComi = ProveedoresF.ProveedoresPorcentajesComisionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiProvPorcComi);
                    txtFechaInicio.Enabled = false;
                    ddlNumeroProveedor.Enabled = false;
                    ddlVendedor.Enabled = false;
                    ddlTiposOperaciones.Enabled = false;
                    txtPorcentaje.Enabled = false;
                    ddlEstados.Enabled = false;
                    ddlFormaCobro.Enabled = false;
                    break;
                case Gestion.Consultar:

                    ddlFormaCobro.Enabled = false;
                    btnAceptar.Visible = false;
                    this.MiProvPorcComi = ProveedoresF.ProveedoresPorcentajesComisionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiProvPorcComi);
                    txtFechaInicio.Enabled = false;
                    ddlNumeroProveedor.Enabled = false;
                    ddlVendedor.Enabled = false;
                    ddlTiposOperaciones.Enabled = false;
                    txtPorcentaje.Enabled = false;
                    ddlEstados.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(CapProveedoresPorcentajesComisiones ProvPorcComi)
        {
            txtFechaInicio.Text = Convert.ToDateTime(MiProvPorcComi.FechaInicioVigencia).ToShortDateString().ToString();

            ddlNumeroProveedor.Items.Add(new ListItem(MiProvPorcComi.Proveedor.RazonSocial, MiProvPorcComi.Proveedor.IdProveedor.ToString()));
            ddlNumeroProveedor.SelectedValue = MiProvPorcComi.Proveedor.IdProveedor.ToString();

            if (MiProvPorcComi.Vendedor.IdVendedor > 0)
            {
                ddlVendedor.Items.Add(new ListItem(MiProvPorcComi.Vendedor.RazonSocial, MiProvPorcComi.Vendedor.IdVendedor.ToString()));
                ddlVendedor.SelectedValue = MiProvPorcComi.Vendedor.IdVendedor.ToString();
            }
           
            hdfIdProveedor.Value = MiProvPorcComi.Proveedor.IdProveedor.ToString();
            hdfIdVendedor.Value = MiProvPorcComi.Vendedor.IdVendedor.ToString();
            ddlFormaCobro.SelectedValue = MiProvPorcComi.FormaCobro.IdFormaCobro.ToString();
            ddlTiposOperaciones.SelectedValue = MiProvPorcComi.TipoOperacion.IdTipoOperacion.ToString();
            txtPorcentaje.Decimal = MiProvPorcComi.Porcentaje;
            ddlEstados.SelectedValue = MiProvPorcComi.Estado.IdEstado.ToString();

        }

        private void MapearControlesAObjeto(CapProveedoresPorcentajesComisiones ProvPorcComi)
        {
            if (MiProvPorcComi.IdProveedorPorcentajeComision > 0)
                ProvPorcComi.IdProveedorPorcentajeComision = MiProvPorcComi.IdProveedorPorcentajeComision;
            ProvPorcComi.FechaInicioVigencia = Convert.ToDateTime(this.txtFechaInicio.Text);
            ProvPorcComi.Proveedor.RazonSocial = hdfNumeroProveedor.Value;
            ProvPorcComi.Proveedor.IdProveedor = Convert.ToInt32(this.hdfIdProveedor.Value);

            if (!string.IsNullOrEmpty(hdfIdVendedor.Value))
            {
                ProvPorcComi.Vendedor.RazonSocial = hdfVendedor.Value;
                ProvPorcComi.Vendedor.IdVendedor = Convert.ToInt32(this.hdfIdVendedor.Value);
            }

            ProvPorcComi.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTiposOperaciones.SelectedValue);
            ProvPorcComi.Porcentaje = this.txtPorcentaje.Decimal;
            ProvPorcComi.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            ProvPorcComi.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            ProvPorcComi.EstadoColeccion = EstadoColecciones.Agregado;
            ProvPorcComi.FormaCobro.IdFormaCobro = ddlFormaCobro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlFormaCobro.SelectedValue);
        }


        private void CargarCombos()
        {
            //this.ddlNumeroProveedor.DataSource = ProveedoresF.ProveedoresObtenerEsVendedor();
            //this.ddlNumeroProveedor.DataValueField = "IdProveedor";
            //this.ddlNumeroProveedor.DataTextField = "RazonSocial";
            //this.ddlNumeroProveedor.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlNumeroProveedor, this.ObtenerMensajeSistema("SeleccioneOpcion"));
             this.ddlFormaCobro.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
            this.ddlFormaCobro.DataTextField = "FormaCobro";
            this.ddlFormaCobro.DataValueField = "IdFormaCobro";
            this.ddlFormaCobro.DataBind();


            AyudaProgramacion.AgregarItemSeleccione(this.ddlFormaCobro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            TGETiposOperaciones tipoOp = new TGETiposOperaciones();
            tipoOp.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            this.ddlTiposOperaciones.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(tipoOp);
            this.ddlTiposOperaciones.DataValueField = "IdTipoOperacion";
            this.ddlTiposOperaciones.DataTextField = "TipoOperacion";
            this.ddlTiposOperaciones.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposOperaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            btnAceptar.Visible = false;
            if (this.txtPorcentaje.Decimal < 0)
            {
          
                return;
            }
            this.MapearControlesAObjeto(this.MiProvPorcComi);

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ProveedoresF.ProveedoresPorcentajesComisionesAgregar(this.MiProvPorcComi);
                    break;
                case Gestion.Anular:
                    MiProvPorcComi.Estado.IdEstado = Convert.ToInt32(Estados.Baja);
                    guardo = ProveedoresF.ProveedoresPorcentajesComisionesModificar(this.MiProvPorcComi);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.MostrarMensaje(this.MiProvPorcComi.CodigoMensaje, false);
         
                //if (this.ModificarDatosAceptar != null)
                //    this.ModificarDatosAceptar(null, this.MiProvPorcComi);
            }
            else
            {
                btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiProvPorcComi.CodigoMensaje, true, this.MiProvPorcComi.CodigoMensajeArgs);
            
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ModificarDatosCancelar != null)
                this.ModificarDatosCancelar();
        }
    }
}