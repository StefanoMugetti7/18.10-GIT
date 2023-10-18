using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Hoteles;
using Hoteles.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel.Controles
{
    public partial class ListaEsperaDatos : ControlesSeguros
    {
        public HTLListaEspera MiListaEspera
        {
            get { return this.PropiedadObtenerValor<HTLListaEspera>("ListaEsperaDatosMiListaEspera"); }
            set { this.PropiedadGuardarValor("ListaEsperaDatosMiListaEspera", value); }
        }

        //public delegate void ControlDatosAceptarEventHandler(object sender, HTLHabitaciones e);
        //public event ControlDatosAceptarEventHandler ControlModificarDatosAceptar;

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(HTLListaEspera pParametro, Gestion pGestion)
        {
            MiListaEspera = pParametro;
            GestionControl = pGestion;
            CargarCombos();

            switch (GestionControl)
            {
                case Gestion.Agregar:
                    txtNumeroDocumento.Enabled = false;
                    ddlEstado.Enabled = false;                    
                    txtFecha.Enabled = false;
                    txtFecha.Text = DateTime.Now.ToShortDateString();
                    ddlEstado.SelectedValue = 38.ToString();

                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    ddlApellido.Enabled = false;
                    txtNumeroDocumento.Enabled = false;                   
                    txtFecha.Enabled = false;
                    ddlProducto.Enabled = false;
                    MiListaEspera = HotelesF.ListaEsperaObtenerDatosCompletos(MiListaEspera);
                    MapearObjetoAControles(MiListaEspera);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            List<TGEEstados> estados = TGEGeneralesF.TGEEstadosObtenerLista("HTLListaEspera");
            ddlEstado.DataSource = estados;
            ddlEstado.DataValueField = "IdEstado";
            ddlEstado.DataTextField = "Descripcion";
            ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(ddlEstado, ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles(HTLListaEspera pListaEspera)
        {
            hdfIdAfiliado.Value = pListaEspera.Afiliado.IdAfiliado.ToString();
            ddlApellido.Items.Add(new ListItem(pListaEspera.Afiliado.Apellido, pListaEspera.Afiliado.IdAfiliado.ToString()));
            hdfApellido.Value = pListaEspera.Afiliado.Apellido;
            txtCantidad.Text = pListaEspera.Cantidad.ToString();
            ddlEstado.SelectedValue = pListaEspera.Estado.IdEstado == -1 ? 38.ToString() : pListaEspera.Estado.IdEstado.ToString();
            txtNumeroDocumento.Text = pListaEspera.Afiliado.NumeroDocumento.ToString();
            ddlProducto.Items.Add(new ListItem(pListaEspera.Producto.Descripcion, pListaEspera.Producto.IdProducto.ToString()));
            hdfIdProducto.Value = pListaEspera.Producto.IdProducto.ToString();
            hdfProductoDetalle.Value = pListaEspera.Producto.Descripcion;
            txtFecha.Text = DateTime.Now.ToShortDateString();
        }

        private void MapearControlesAObjeto(HTLListaEspera pListaEspera)
        {  
            if (!string.IsNullOrEmpty(hdfIdAfiliado.Value))
            {
                Int64 id = 0;
                if (Int64.TryParse(hdfIdAfiliado.Value, out id))
                    pListaEspera.Afiliado.IdAfiliado = (int)id;
                else
                    pListaEspera.Afiliado.IdAfiliado = (int)default(Int64?);
            }
            pListaEspera.Afiliado.Apellido = string.IsNullOrEmpty(hdfApellido.Value) ? default(string) : hdfApellido.Value;
            pListaEspera.Estado.IdEstado = Convert.ToInt32(ddlEstado.SelectedValue);
            pListaEspera.Afiliado.NumeroDocumento = Convert.ToInt32(txtNumeroDocumento.Text);
            pListaEspera.Cantidad = txtCantidad.Text == string.Empty ? 0 : Convert.ToInt32(txtCantidad.Text);

            pListaEspera.Producto.IdProducto = Convert.ToInt32(hdfIdProducto.Value);
            pListaEspera.Producto.Descripcion = hdfProductoDetalle.Value;
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            MapearControlesAObjeto(MiListaEspera);

            switch (GestionControl)
            {
                case Gestion.Agregar:
                    MiListaEspera.IdUsuarioAlta = UsuarioActivo.IdUsuarioEvento;
                    guardo = HotelesF.ListaEsperaAgregar(MiListaEspera);
                    if (guardo)
                    {
                        MostrarMensaje(MiListaEspera.CodigoMensaje, false, MiListaEspera.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = HotelesF.ListaEsperaModificar(MiListaEspera);
                    if (guardo)
                    {
                        MostrarMensaje(MiListaEspera.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (ControlModificarDatosCancelar != null)
                ControlModificarDatosCancelar();
        }
    }
}