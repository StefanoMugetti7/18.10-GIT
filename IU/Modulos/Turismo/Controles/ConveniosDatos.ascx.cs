using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Turismo;
using Turismo.Entidades;

namespace IU.Modulos.Turismo.Controles
{
    public partial class ConveniosDatos : ControlesSeguros
    {
        private TurConvenios MiConvenio
        {
            get { return (TurConvenios)Session[this.MiSessionPagina + "ConvenioModificarDatosMiConvenio"]; }
            set { Session[this.MiSessionPagina + "ConvenioModificarDatosMiConvenio"] = value; }
        }
        private List<TGEListasValoresDetalles> TiposHabitaciones
        {
            get { return (List<TGEListasValoresDetalles>)Session[this.MiSessionPagina + "ConvenioModificarDatosMisTiposHabitaciones"]; }
            set { Session[this.MiSessionPagina + "ConvenioModificarDatosMisTiposHabitaciones"] = value; }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {

            }
        }
        public void IniciarControl(TurConvenios pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.TiposHabitaciones = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TiposHabitaciones);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiConvenio = pParametro;
                    this.IniciarGrillas();
                    this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);
                    this.txtCantidadDiasPlazas.Text = "0";
                    this.txtCantidadPlazas.Text = "0";
                    break;
                case Gestion.Modificar:
                    this.MiConvenio = TurismoF.ConveniosObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiConvenio);
                    break;
                case Gestion.Consultar:
                    this.MiConvenio = TurismoF.ConveniosObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiConvenio);
                    this.OcultarControles();
                    this.gvDetalles.Columns[this.gvDetalles.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.gvExcepciones.Columns[this.gvExcepciones.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "Calcular", "CalcularTotales();", true);
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("TurConvenios");
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TurConvenios conv = new TurConvenios();
            this.ddlHotel.DataSource = TurismoF.ConveniosObtenerHoteles(conv);
            this.ddlHotel.DataValueField = "IdHotel";
            this.ddlHotel.DataTextField = "Hotel";
            this.ddlHotel.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlHotel, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        private void MapearObjetoAControles(TurConvenios pParametro)
        {
            ListItem item2 = ddlEstado.Items.FindByValue(pParametro.Estado.IdEstado.ToString());
            if (item2 == null)
                this.ddlEstado.Items.Add(new ListItem(pParametro.Estado.Descripcion, pParametro.Estado.IdEstado.ToString()));

            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();

            ListItem item = ddlHotel.Items.FindByValue(pParametro.IdHotel.ToString());
            if (item == null)
                this.ddlHotel.Items.Add(new ListItem(pParametro.Hotel, pParametro.IdHotel.ToString()));

            this.ddlHotel.SelectedValue = pParametro.IdHotel.ToString();

            this.txtFechaInicioConvenio.Text = Convert.ToDateTime(pParametro.FechaInicioConvenio.ToString()).ToShortDateString();
            this.txtFechaFinalConvenio.Text = Convert.ToDateTime(pParametro.FechaFinalConvenio.ToString()).ToShortDateString();
            this.txtFechaInicioTemporadaAlta.Text = pParametro.FechaInicioTemporadaAlta.ToShortDateString();
            this.txtFechaFinalTemporadaAlta.Text = pParametro.FechaFinalTemporadaAlta.ToShortDateString();
            this.txtCantidadPlazas.Text = pParametro.CantidadPlazas.ToString();
            this.txtCantidadDiasPlazas.Text = pParametro.CantidadPlazasDia.ToString();

            this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);
            AyudaProgramacion.CargarGrillaListas<TurConveniosDetalles>(this.MiConvenio.Detalles, false, this.gvDetalles, true);
            AyudaProgramacion.CargarGrillaListas<TurConveniosExcepciones>(this.MiConvenio.Excepciones, false, this.gvExcepciones, true);
        }
        private void MapearControlesAObjeto(TurConvenios pParametro)
        {
            pParametro.IdHotel = this.ddlHotel.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlHotel.SelectedValue);
            pParametro.FechaInicioConvenio = this.txtFechaInicioConvenio.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaInicioConvenio.Text);
            pParametro.FechaFinalConvenio = this.txtFechaFinalConvenio.Text == string.Empty ? (Nullable<DateTime>)null : Convert.ToDateTime(this.txtFechaFinalConvenio.Text);
            pParametro.FechaInicioTemporadaAlta = Convert.ToDateTime(this.txtFechaInicioTemporadaAlta.Text);
            pParametro.FechaFinalTemporadaAlta = Convert.ToDateTime(this.txtFechaFinalTemporadaAlta.Text);
            pParametro.CantidadPlazas = Convert.ToInt32(this.txtCantidadPlazas.Text);
            pParametro.CantidadPlazasDia = Convert.ToInt32(this.txtCantidadDiasPlazas.Text);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Campos = this.ctrCamposValores.ObtenerLista();
        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiConvenio);
            this.PersistirGrillas();
            this.MiConvenio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = TurismoF.ConveniosAgregar(this.MiConvenio);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiConvenio.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = TurismoF.ConveniosModificar(this.MiConvenio);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiConvenio.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiConvenio.CodigoMensaje, true, this.MiConvenio.CodigoMensajeArgs);
                if (this.MiConvenio.dsResultado != null)
                {
                    this.MiConvenio.dsResultado = null;
                }
            }
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }
        #region DETALLES
        protected void gvDetalles_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Anular.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //int MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.PersistirDatosGrillaDetalles();
                this.MiConvenio.Detalles.RemoveAt(index);
                this.MiConvenio.Detalles = AyudaProgramacion.AcomodarIndices<TurConveniosDetalles>(this.MiConvenio.Detalles);
                AyudaProgramacion.CargarGrillaListas<TurConveniosDetalles>(this.MiConvenio.Detalles, false, this.gvDetalles, true);
                ScriptManager.RegisterStartupScript(this.pnlPrinc, this.pnlPrinc.GetType(), "Calcular", "CalcularTotales();", true);
            }
        }
        protected void gvDetalles_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton cancelar = (ImageButton)e.Row.FindControl("btnCancelar");
                TurConveniosDetalles item = (TurConveniosDetalles)e.Row.DataItem;
                TextBox cant = (TextBox)e.Row.FindControl("txtCantidad");

                DropDownList ddlTiposHabitaciones = ((DropDownList)e.Row.FindControl("ddlTipoHabitacion"));
                ddlTiposHabitaciones.DataSource = this.TiposHabitaciones;
                ddlTiposHabitaciones.DataValueField = "IdListaValorDetalle";
                ddlTiposHabitaciones.DataTextField = "Descripcion";
                ddlTiposHabitaciones.DataBind();

                cant.Attributes.Add("onchange", "CalcularPlazas();");

                if (this.GestionControl != Gestion.Agregar)
                {
                    cant.Text = item.Cantidad.ToString();

                    if (item.IdTipoHabitacion > 0)
                    {
                        ListItem aux = ddlTiposHabitaciones.Items.FindByValue(item.IdTipoHabitacion.ToString());
                        if (aux == null)
                            ddlTiposHabitaciones.Items.Add(new ListItem(item.TipoHabitacion, item.IdTipoHabitacion.ToString()));

                        ddlTiposHabitaciones.SelectedValue = item.IdTipoHabitacion.ToString();
                    }

                    if (this.GestionControl == Gestion.Consultar)
                    {
                        cant.Enabled = false;
                        ddlTiposHabitaciones.Enabled = false;
                    }
                }
            }
        }
        private void PersistirDatosGrillaDetalles()
        {
            if (this.MiConvenio.Detalles.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvDetalles.Rows)
            {
                DropDownList ddlTipoHabitacion = ((DropDownList)fila.FindControl("ddlTipoHabitacion"));
                int cantidad = Convert.ToInt32(((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidad")).Decimal);

                if (ddlTipoHabitacion.SelectedValue != "" && Convert.ToInt32(ddlTipoHabitacion.SelectedValue) > 0)
                {
                    this.MiConvenio.Detalles[fila.RowIndex].IdTipoHabitacion = Convert.ToInt32(ddlTipoHabitacion.SelectedValue);
                    this.MiConvenio.Detalles[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                }
                if (cantidad > 0)
                {
                    this.MiConvenio.Detalles[fila.RowIndex].Cantidad = cantidad;
                }

            }
        }
        protected void btnAgregarItemDetalles_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrillaDetalles();
            this.AgregarItemDetalles();
            this.txtCantidadAgregarDetalles.Text = string.Empty;
        }
        private void AgregarItemDetalles()
        {
            TurConveniosDetalles item;
            if (this.txtCantidadAgregarDetalles.Text == string.Empty || this.txtCantidadAgregarDetalles.Text == "0")
            {
                this.txtCantidadAgregarDetalles.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregarDetalles.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new TurConveniosDetalles();
                this.MiConvenio.Detalles.Add(item);
                item.IndiceColeccion = this.MiConvenio.Detalles.IndexOf(item);
                item.IdConvenioDetalle = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<TurConveniosDetalles>(this.MiConvenio.Detalles, false, this.gvDetalles, true);
        }
        private void IniciarGrillaDetalles()
        {
            TurConveniosDetalles item;
            for (int i = 0; i < 2; i++)
            {
                item = new TurConveniosDetalles();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiConvenio.Detalles.Add(item);
                item.IndiceColeccion = this.MiConvenio.Detalles.IndexOf(item);
                item.IdConvenioDetalle = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<TurConveniosDetalles>(this.MiConvenio.Detalles, false, this.gvDetalles, true);
        }
        #endregion
        #region EXCEPCIONES
        protected void gvExcepciones_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Anular.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //int MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.PersistirDatosGrillaExcepciones();
                this.MiConvenio.Excepciones.RemoveAt(index);
                this.MiConvenio.Excepciones = AyudaProgramacion.AcomodarIndices<TurConveniosExcepciones>(this.MiConvenio.Excepciones);
                AyudaProgramacion.CargarGrillaListas<TurConveniosExcepciones>(this.MiConvenio.Excepciones, false, this.gvExcepciones, true);
            }
        }
        protected void gvExcepciones_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton cancelar = (ImageButton)e.Row.FindControl("btnBaja");
                TurConveniosExcepciones item = (TurConveniosExcepciones)e.Row.DataItem;
                TextBox txtFecha = (TextBox)e.Row.FindControl("txtFecha");
                TextBox cant = (TextBox)e.Row.FindControl("txtCantidadPlazasExcepcion");
                cant.Attributes.Add("onchange", "CalcularPlazasDias();");
                if (this.GestionControl != Gestion.Agregar)
                {
                    cant.Text = item.CantidadPlazasExcepcion.ToString();

                    if (this.GestionControl == Gestion.Consultar)
                    {
                        cant.Enabled = false;
                        txtFecha.Enabled = false;
                    }
                }
                txtFecha.Text = item.FechaExcepcion.ToShortDateString();
            }
        }
        protected void btnAgregarItemExcepciones_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrillaExcepciones();
            this.AgregarItemExcepciones();
            this.txtCantidadAgregarExcepciones.Text = string.Empty;
        }
        private void AgregarItemExcepciones()
        {
            TurConveniosExcepciones item;
            if (this.txtCantidadAgregarExcepciones.Text == string.Empty || this.txtCantidadAgregarExcepciones.Text == "0")
            {
                this.txtCantidadAgregarExcepciones.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregarExcepciones.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new TurConveniosExcepciones();
                this.MiConvenio.Excepciones.Add(item);
                item.FechaExcepcion = DateTime.Now;
                item.IndiceColeccion = this.MiConvenio.Excepciones.IndexOf(item);
                item.IdConvenioExcepcion = item.IndiceColeccion * -1;
                //this.gvItems.Rows[item.IndiceColeccion].FindControl("ddlProducto").Focus();
            }
            AyudaProgramacion.CargarGrillaListas<TurConveniosExcepciones>(this.MiConvenio.Excepciones, false, this.gvExcepciones, true);
        }
        private void PersistirDatosGrillaExcepciones()
        {
            if (this.MiConvenio.Excepciones.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvExcepciones.Rows)
            {
                TextBox txtFecha = ((TextBox)fila.FindControl("txtFecha"));
                int cantidad = Convert.ToInt32(((Evol.Controls.CurrencyTextBox)fila.FindControl("txtCantidadPlazasExcepcion")).Decimal);

                if (!(string.IsNullOrEmpty(txtFecha.Text)))
                {
                    this.MiConvenio.Excepciones[fila.RowIndex].FechaExcepcion = Convert.ToDateTime(txtFecha.Text);
                    this.MiConvenio.Excepciones[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                }
                if (cantidad > 0)
                {
                    this.MiConvenio.Excepciones[fila.RowIndex].CantidadPlazasExcepcion = cantidad;
                }
            }
        }
        private void IniciarGrillaExcepciones()
        {
            TurConveniosExcepciones item;
            for (int i = 0; i < 2; i++)
            {
                item = new TurConveniosExcepciones();
                item.EstadoColeccion = EstadoColecciones.AgregadoPrevio;
                this.MiConvenio.Excepciones.Add(item);
                item.FechaExcepcion = DateTime.Now;
                item.IndiceColeccion = this.MiConvenio.Excepciones.IndexOf(item);
                item.IdConvenioExcepcion = item.IndiceColeccion * -1;
            }
            AyudaProgramacion.CargarGrillaListas<TurConveniosExcepciones>(this.MiConvenio.Excepciones, false, this.gvExcepciones, true);
        }
        #endregion
        private void OcultarControles()
        {
            this.lblCantidadAgregarDetalles.Visible = false;
            this.lblCantidadAgregarExcepciones.Visible = false;
            this.txtCantidadAgregarDetalles.Visible = false;
            this.txtCantidadAgregarExcepciones.Visible = false;
            this.btnAgregarItemDetalles.Visible = false;
            this.btnAgregarItemExcepciones.Visible = false;
            this.ddlHotel.Enabled = false;
            this.txtFechaInicioConvenio.Enabled = false;
            this.txtFechaFinalConvenio.Enabled = false;
            this.ddlEstado.Enabled = false;
            this.txtFechaInicioTemporadaAlta.Enabled = false;
            this.txtFechaFinalTemporadaAlta.Enabled = false;
            this.btnAceptar.Visible = false;
        }
        private void IniciarGrillas()
        {
            this.IniciarGrillaDetalles();
            this.IniciarGrillaExcepciones();
        }
        private void PersistirGrillas()
        {
            this.PersistirDatosGrillaDetalles();
            this.PersistirDatosGrillaExcepciones();
        }
    }
}