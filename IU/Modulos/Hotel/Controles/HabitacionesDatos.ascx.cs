using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Hoteles;
using Hoteles.Entidades;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace IU.Modulos.Hotel.Controles
{
    public partial class HabitacionesDatos : ControlesSeguros
    {
        public HTLHabitaciones MiHabitacion
        {
            get { return this.PropiedadObtenerValor<HTLHabitaciones>("HabitacionesDatosMiHabitacion"); }
            set { this.PropiedadGuardarValor("HabitacionesDatosMiHabitacion", value); }
        }
        public List<TGEListasValoresDetalles> MisMoviliarios
        {
            get { return this.PropiedadObtenerValor<List<TGEListasValoresDetalles>>("HabitacionesDatosMisMoviliarios"); }
            set { this.PropiedadGuardarValor("HabitacionesDatosMisMoviliarios", value); }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        public void IniciarControl(HTLHabitaciones pParametro, Gestion pGestion)
        {
            this.MiHabitacion = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrCamposValores.IniciarControl(this.MiHabitacion, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Anular:
                    break;
                case Gestion.Modificar:
                    this.MiHabitacion = HotelesF.HabitacionesObtenerDatosCompletos(this.MiHabitacion);
                    this.MapearObjetoAControles(this.MiHabitacion);
                    break;
                case Gestion.Consultar:
                    this.MiHabitacion = HotelesF.HabitacionesObtenerDatosCompletos(this.MiHabitacion);
                    this.MapearObjetoAControles(this.MiHabitacion);
                    this.ddlEstado.Enabled = false;
                    this.ddlHoteles.Enabled = false;
                    this.ddlProductos.Enabled = false;
                    this.txtCantidad.Enabled = false;
                    this.txtNombre.Enabled = false;
                    this.txtNumeroHabitacion.Enabled = false;
                    this.txtPiso.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.btnAgregaDetalle.Visible = false;
                    this.gvDatos.Columns[this.gvDatos.Columns.Count - 1].Visible = false;
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.MisMoviliarios = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.HotelMoviliario);

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            HTLHoteles hotel = new HTLHoteles();
            hotel.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            this.ddlHoteles.DataSource = HotelesF.HotelesObtenerListaActiva(hotel);
            this.ddlHoteles.DataValueField = "IdHotel";
            this.ddlHoteles.DataTextField = "Descripcion";
            this.ddlHoteles.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlHoteles, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            if (this.MiHabitacion.IdHotel != 0)
            {
                this.ddlHoteles.SelectedValue = Convert.ToString(this.MiHabitacion.IdHotel);
            }

            this.ddlProductos.DataSource = HotelesF.ProductosObtenerTiposProductosHoteles(new Objeto());
            this.ddlProductos.DataValueField = "IdProducto";
            this.ddlProductos.DataTextField = "Descripcion";
            this.ddlProductos.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlProductos, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(HTLHabitaciones pHabitacion)
        {
            this.ddlEstado.SelectedValue = pHabitacion.Estado.IdEstado.ToString();
            this.ddlHoteles.SelectedValue = pHabitacion.IdHotel.ToString();
            this.ddlProductos.SelectedValue = pHabitacion.IdProducto.ToString();
            this.txtNumeroHabitacion.Text = pHabitacion.NumeroHabitacion;
            this.txtNombre.Text = pHabitacion.NombreHabitacion;
            this.txtPiso.Text = pHabitacion.Piso;
            this.txtCantidad.Text = pHabitacion.Cantidad == 0 ? string.Empty : pHabitacion.Cantidad.ToString();
            this.ctrCamposValores.IniciarControl(pHabitacion, new Objeto(), this.GestionControl);
            if (pHabitacion.HabitacionesDetalles.Count > 0)
                AyudaProgramacion.CargarGrillaListas<HTLHabitacionesDetalles>(pHabitacion.HabitacionesDetalles, false, this.gvDatos, true);
        }
        private void MapearControlesAObjeto(HTLHabitaciones pHabitacion)
        {
            pHabitacion.IdHotel = Convert.ToInt32(this.ddlHoteles.SelectedValue);
            pHabitacion.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pHabitacion.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pHabitacion.IdProducto = Convert.ToInt64(this.ddlProductos.SelectedValue);
            pHabitacion.NumeroHabitacion = this.txtNumeroHabitacion.Text;
            pHabitacion.NombreHabitacion = this.txtNombre.Text;
            pHabitacion.Piso = this.txtPiso.Text;
            pHabitacion.Cantidad = this.txtCantidad.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidad.Text);
            pHabitacion.Campos = this.ctrCamposValores.ObtenerLista();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiHabitacion);
            this.PersistirDetalleGrilla();
            this.MiHabitacion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = HotelesF.HabitacionesAgregar(this.MiHabitacion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiHabitacion.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Anular:
                    this.MiHabitacion.Estado.IdEstado = (int)Estados.Baja;
                    guardo = HotelesF.HabitacionesModificar(this.MiHabitacion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiHabitacion.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = HotelesF.HabitacionesModificar(this.MiHabitacion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiHabitacion.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiHabitacion.CodigoMensaje, true, this.MiHabitacion.CodigoMensajeArgs);
                if (this.MiHabitacion.dsResultado != null)
                {
                    this.MiHabitacion.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            this.ControlModificarDatosCancelar?.Invoke();
        }
        #region HABITACIONES DETALLES
        protected void btnAgregaDetalle_Click(object sender, EventArgs e)
        {
            this.PersistirDetalleGrilla();
            this.AgregarItem(1);
        }
        private void AgregarItem(int cantidad)
        {
            HTLHabitacionesDetalles item;
            for (int i = 0; i < cantidad; i++)
            {
                item = new HTLHabitacionesDetalles();
                item.Estado.IdEstado = (int)Estados.Activo;
                item.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiHabitacion.HabitacionesDetalles.Add(item);
                item.IndiceColeccion = this.MiHabitacion.HabitacionesDetalles.IndexOf(item);
                item.IdHabitacionDetalle = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<HTLHabitacionesDetalles>(this.MiHabitacion.HabitacionesDetalles, true, this.gvDatos, true);
        }
        private void PersistirDetalleGrilla()
        {
            if (this.MiHabitacion.HabitacionesDetalles.Count == 0)
                return;

            HTLHabitacionesDetalles det;
            bool modifica;
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                if (fila.RowType == DataControlRowType.DataRow)
                {
                    modifica = false;
                    det = this.MiHabitacion.HabitacionesDetalles.Find(x => x.IdHabitacionDetalle == Convert.ToInt64(this.gvDatos.DataKeys[fila.RowIndex]["IdHabitacionDetalle"].ToString()));
                    DropDownList ddlMoviliarios = ((DropDownList)fila.FindControl("ddlMoviliarios"));
                    if (!string.IsNullOrEmpty(ddlMoviliarios.SelectedValue) && det.Moviliario.IdMoviliario != Convert.ToInt32(ddlMoviliarios.SelectedValue))
                    {
                        modifica = true;
                        det.Moviliario.IdMoviliario = ddlMoviliarios.SelectedValue == string.Empty ? default(int) : Convert.ToInt32(ddlMoviliarios.SelectedValue);
                        det.Moviliario.Descripcion = ddlMoviliarios.SelectedItem.Text;
                    }

                    TextBox txtDescripcion = ((TextBox)fila.FindControl("txtDescripcion"));
                    if (txtDescripcion.Text != det.Descripcion)
                    {
                        modifica = true;
                        det.Descripcion = txtDescripcion.Text;
                    }
                    if (modifica)
                        det.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(det, this.GestionControl);

                }
            }
        }
        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HTLHabitacionesDetalles item = (HTLHabitacionesDetalles)e.Row.DataItem;
                TextBox txtDescripcion = ((TextBox)e.Row.FindControl("txtDescripcion"));
                DropDownList ddlMoviliarios = ((DropDownList)e.Row.FindControl("ddlMoviliarios"));
                ddlMoviliarios.DataSource = this.MisMoviliarios;
                ddlMoviliarios.DataValueField = "IdListaValorDetalle";
                ddlMoviliarios.DataTextField = "Descripcion";
                ddlMoviliarios.DataBind();
                AyudaProgramacion.InsertarItemSeleccione(ddlMoviliarios, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                ListItem lstitem = ddlMoviliarios.Items.FindByValue(item.Moviliario.IdMoviliario.ToString());
                if (lstitem == null && item.Moviliario.IdMoviliario > 0)
                {
                    ddlMoviliarios.Items.Add(new ListItem(item.Moviliario.Descripcion, item.Moviliario.IdMoviliario.ToString()));
                    ddlMoviliarios.SelectedValue = item.Moviliario.IdMoviliario.ToString();
                }
                if (lstitem != null)
                {
                    ddlMoviliarios.SelectedValue = item.Moviliario.IdMoviliario.ToString();
                }
                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                string mensaje = this.ObtenerMensajeSistema("ValidarHabitacionesDetallesEliminar");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnEliminar.Attributes.Add("OnClick", funcion);

                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Modificar:
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Consultar:
                        ddlMoviliarios.Enabled = false;
                        txtDescripcion.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
        }
        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int idHabitacionDetalle = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "Borrar")
            {
                this.PersistirDetalleGrilla();
                HTLHabitacionesDetalles item = this.MiHabitacion.HabitacionesDetalles.Find(x => x.IdHabitacionDetalle == idHabitacionDetalle);
                item.Estado.IdEstado = (int)Estados.Baja;
                item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(item, Gestion.Anular);
                AyudaProgramacion.CargarGrillaListas<HTLHabitacionesDetalles>(this.MiHabitacion.HabitacionesDetalles, true, this.gvDatos, true);
            }
        }
        #endregion
    }
}