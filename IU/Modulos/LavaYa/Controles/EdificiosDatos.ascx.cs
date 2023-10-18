using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using LavaYa;
using LavaYa.Entidades;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.LavaYa.Controles
{
    public partial class EdificiosDatos : ControlesSeguros
    {
        public LavEdificios MiEdificio
        {
            get { return this.PropiedadObtenerValor<LavEdificios>("EdificiosDatosMisEdificios"); }
            set { this.PropiedadGuardarValor("EdificiosDatosMisEdificios", value); }
        }
        public List<LavMaquinas> MisMaquinas
        {
            get { return this.PropiedadObtenerValor<List<LavMaquinas>>("MaquinasEdificiosDatosMisEdificios"); }
            set { this.PropiedadGuardarValor("MaquinasEdificiosDatosMisEdificios", value); }
        }
        private DataTable MisRequerimientos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "RequerimientosMisRequerimientos"]; }
            set { Session[this.MiSessionPagina + "RequerimientosMisRequerimientos"] = value; }
        }
        private DataTable MisPuntosVentas
        {
            get { return (DataTable)Session[this.MiSessionPagina + "PuntosVentasMisPuntosVentas"]; }
            set { Session[this.MiSessionPagina + "PuntosVentasMisPuntosVentas"] = value; }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }
        public void IniciarControl(LavEdificios pParametro, Gestion pGestion)
        {
            this.MiEdificio = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrCamposValores.IniciarControl(this.MiEdificio, new Objeto(), this.GestionControl);
                    this.ddlEstado.Enabled = false;
                    this.lblImagen.Visible = false;
                    this.txtFechaAlta.Text = DateTime.Now.ToShortDateString();
                    this.ctrArchivos.IniciarControl(new Objeto(), this.GestionControl);
                    this.btnAgregarMaquina.Visible = this.ValidarPermiso("MaquinasAgregar.aspx");
                    this.txtCantidadMaquinasLavado.Text = 0.ToString();
                    this.txtCantidadMaquinasSecado.Text = 0.ToString();
                    break;
                case Gestion.Anular:
                    this.btnAgregarMaquina.Visible = false;
                    this.btnCargarMaquina.Visible = false;
                    break;
                case Gestion.Modificar:
                    this.MiEdificio = EdificiosF.EdificiosObtenerDatosCompletos(this.MiEdificio);
                    this.MapearObjetoAControles(this.MiEdificio);
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "MarcarMapa", "MarcarMapa();", true);
                    this.ctrArchivos.IniciarControl(MiEdificio, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiEdificio, new Objeto(), this.GestionControl);
                    this.btnAgregarMaquina.Visible = this.ValidarPermiso("MaquinasAgregar.aspx");
                    this.CargarListaRequerimientos(this.MiEdificio);
                    this.CargarListaPuntosVentas(this.MiEdificio);
                    break;
                case Gestion.Consultar:
                    this.MiEdificio = EdificiosF.EdificiosObtenerDatosCompletos(this.MiEdificio);
                    this.MapearObjetoAControles(this.MiEdificio);
                    this.ddlLocalizacion.Enabled = false;
                    this.txtContacto.Enabled = false;
                    this.txtDescripcion.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.ddlContrato.Enabled = false;
                    this.ddlHorario.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.txtFrecuenciaRecaudacion.Enabled = false;
                    this.txtFrecuenciaAspiracion.Enabled = false;
                    this.btnAgregarMaquina.Visible = false;
                    this.btnCargarMaquina.Visible = false;
                    this.ddlMaquinas.Visible = false;
                    this.lblMaquinas.Visible = false;
                    this.txtUnidadesFuncionales.Enabled = false;
                    this.ddlSistemaPago.Enabled = false;
                    this.ddlServicios.Enabled = false;
                    this.txtNumeroMaquina.Visible = false;
                    // ScriptManager.RegisterStartupScript(this, this.GetType(), "MarcarMapa", "MarcarMapa();", true);
                    this.ctrArchivos.IniciarControl(MiEdificio, this.GestionControl);
                    gvMaquinas.Columns[this.gvMaquinas.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.CargarListaRequerimientos(this.MiEdificio);
                    this.CargarListaPuntosVentas(this.MiEdificio);
                    break;
                default:
                    break;
            }
        }
        private void CargarCombos()
        {
            this.ddlHorario.DataSource = EdificiosF.EdificiosObtenerOpcionHorario();
            this.ddlHorario.DataValueField = "IdHorario";
            this.ddlHorario.DataTextField = "Horario";
            this.ddlHorario.DataBind();

            this.ddlContrato.DataSource = EdificiosF.EdificiosObtenerOpcionContrato();
            this.ddlContrato.DataValueField = "IdContrato";
            this.ddlContrato.DataTextField = "Contrato";
            this.ddlContrato.DataBind();

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.ddlSistemaPago.DataSource = EdificiosF.EdificiosObtenerOpcionSistemaPago();
            this.ddlSistemaPago.DataValueField = "IdSistemaPago";
            this.ddlSistemaPago.DataTextField = "SistemaPago";
            this.ddlSistemaPago.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlSistemaPago, ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlServicios.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.LavaYaTiposServicios);
            this.ddlServicios.DataValueField = "IdListaValorDetalle";
            this.ddlServicios.DataTextField = "Descripcion";
            this.ddlServicios.DataBind();

            CargarComboMaquinas();
        }
        private void MapearObjetoAControles(LavEdificios pParametro)
        {
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
            //this.txtCodigoPostal.Text = pParametro.CodigoPostal.ToString();
            this.txtContacto.Text = pParametro.Contacto;
            this.txtDescripcion.Text = pParametro.Descripcion;
            this.txtLocalidad.Text = pParametro.Localidad;
            this.txtNumero.Text = pParametro.NumeroDireccion.ToString();
            this.txtPartido.Text = pParametro.Partido;
            this.txtDireccion.Text = pParametro.Direccion;
            //this.txtLatitud.Text = pParametro.Latitud.ToString();
            //this.txtLongitud.Text = pParametro.Longitud.ToString();
            this.hdfLatitud.Value = pParametro.Latitud.ToString();
            this.hdfLongitud.Value = pParametro.Longitud.ToString();
            this.hdfCodigoPostal.Value = pParametro.CodigoPostal;
            this.hdfNumeroCasa.Value = pParametro.NumeroDireccion.ToString();
            this.hdfCalleCasa.Value = pParametro.Direccion.ToString();
            this.txtProvincia.Text = pParametro.Provincia;
            //this.hdfLocalidad.Value= pParametro.Localidad;
            //this.hdfProvincia.Value= pParametro.Provincia;
            this.ctrCamposValores.IniciarControl(pParametro, new Objeto(), this.GestionControl);

            //ARMO QR
            //string base64String = pParametro.CodigoQR == null ? string.Empty : Convert.ToBase64String(pParametro.CodigoQRImagen, 0, pParametro.CodigoQRImagen.Length);
            //this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
            //this.imgLogo.Visible = true;

            this.txtFechaAlta.Text = pParametro.FechaPEM.ToShortDateString();
            this.ddlHorario.SelectedValue = pParametro.IdHorario.ToString();
            this.ddlContrato.SelectedValue = pParametro.IdContrato.ToString();
            this.txtCodigo.Text = pParametro.IdEdificio.ToString();
            this.txtFrecuenciaRecaudacion.Text = pParametro.FrecuenciaRecaudacion.ToString();
            this.txtFrecuenciaAspiracion.Text = pParametro.FrecuenciaAspiracion.ToString();

            this.txtCantidadMaquinasLavado.Text = pParametro.CantidadMaquinasLavado.ToString();
            this.txtCantidadMaquinasSecado.Text = pParametro.CantidadMaquinasSecado.ToString();

            this.hdfLocalizacionCompleta.Value = pParametro.Localizacion;

            if (pParametro.IdSistemaPago > 0)
                this.ddlSistemaPago.SelectedValue = pParametro.IdSistemaPago.ToString();

            this.txtUnidadesFuncionales.Text = pParametro.UnidadesFuncionales.HasValue ? pParametro.UnidadesFuncionales.ToString() : "";

            if (!string.IsNullOrEmpty(pParametro.Latitud.ToString()))
            {
                this.ddlLocalizacion.Items.Clear();
                this.ddlLocalizacion.Items.Add(new ListItem(pParametro.Localizacion, pParametro.Localizacion));
                this.ddlLocalizacion.SelectedValue = pParametro.Localizacion;
            }

            this.ctrArchivos.IniciarControl(pParametro, this.GestionControl);

            if (pParametro.Maquinas.Count > 0)
            {
                AyudaProgramacion.CargarGrillaListas<LavMaquinas>(pParametro.Maquinas, false, this.gvMaquinas, true);
                this.upMaquinas.Update();
            }

            if (!string.IsNullOrEmpty(pParametro.Servicios))
            {
                for (int i = 0; i <= this.ddlServicios.Items.Count - 1; i++)
                {
                    if (pParametro.Servicios.Contains(this.ddlServicios.Items[i].Value))
                    {
                        this.ddlServicios.Items[i].Selected = true;
                    }
                }
            }
        }
        private void MapearControlesAObjeto(LavEdificios pParametro)
        {
            pParametro.CodigoPostal = this.txtCodigoPostal.Text;
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pParametro.Contacto = this.txtContacto.Text;
            pParametro.Descripcion = this.txtDescripcion.Text;
            pParametro.Localidad = this.txtLocalidad.Text;
            //pParametro.NumeroDireccion = this.txtNumero.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumero.Text);
            pParametro.Provincia = this.txtProvincia.Text;
            //pParametro.Partido = this.txtPartido.Text;

            pParametro.CodigoQR = "https://" + "maps.google.com/?q=" + this.hdfLatitud.Value.ToString().Replace(",", ".") + "," + this.hdfLongitud.Value.ToString().Replace(",", ".");

            pParametro.IdContrato = this.ddlContrato.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlContrato.SelectedValue);
            pParametro.IdHorario = this.ddlHorario.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlHorario.SelectedValue);
            pParametro.FechaPEM = this.txtFechaAlta.Text == string.Empty ? DateTime.Now : Convert.ToDateTime(this.txtFechaAlta.Text);
            pParametro.FrecuenciaAspiracion = this.txtFrecuenciaAspiracion.Text == string.Empty ? 0 : Convert.ToInt32(this.txtFrecuenciaAspiracion.Text);
            pParametro.FrecuenciaRecaudacion = this.txtFrecuenciaRecaudacion.Text == string.Empty ? 0 : Convert.ToInt32(this.txtFrecuenciaRecaudacion.Text);
            pParametro.CantidadMaquinasLavado = this.txtCantidadMaquinasLavado.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadMaquinasLavado.Text);
            pParametro.CantidadMaquinasSecado = this.txtCantidadMaquinasSecado.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadMaquinasSecado.Text);

            pParametro.IdSistemaPago = this.ddlSistemaPago.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlSistemaPago.SelectedValue);
            pParametro.UnidadesFuncionales = this.txtUnidadesFuncionales.Text == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.txtUnidadesFuncionales.Text);

            pParametro.NumeroDireccion = this.hdfNumeroCasa.Value == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.hdfNumeroCasa.Value);
            pParametro.Direccion = this.hdfCalleCasa.Value == string.Empty ? null : this.hdfCalleCasa.Value;
            pParametro.Latitud = this.hdfLatitud.Value == string.Empty ? 0 : Convert.ToDecimal(this.hdfLatitud.Value);
            pParametro.Longitud = this.hdfLongitud.Value == string.Empty ? 0 : Convert.ToDecimal(this.hdfLongitud.Value);
            pParametro.Localizacion = this.hdfLocalizacionCompleta.Value == string.Empty ? null : this.hdfLocalizacionCompleta.Value;
            pParametro.CodigoPostal = this.hdfCodigoPostal.Value == string.Empty ? null : this.hdfCodigoPostal.Value;

            if (!string.IsNullOrEmpty(this.ddlServicios.SelectedValue))
            {
                pParametro.Servicios = "";
                foreach (ListItem item in this.ddlServicios.Items)
                {
                    if (item.Selected)
                    {
                        pParametro.Servicios += item.Value.ToString() + ",";
                    }
                }
                pParametro.Servicios = pParametro.Servicios.Remove(pParametro.Servicios.Length - 1);
            }
            pParametro.Archivos = ctrArchivos.ObtenerLista();
        }
        protected void btnVerMapa_Click(object sender, EventArgs e)
        {
            if (this.hdfLongitud.Value != string.Empty && Decimal.TryParse(this.hdfLongitud.Value, out decimal result))
            {
                var lat = this.hdfLatitud.Value.Replace(",", ".");
                var lon = this.hdfLongitud.Value.Replace(",", ".");

                var url = string.Format("https://www.google.es/maps?q={0},{1}", lat, lon);
                string script = string.Format("<script>window.open('{0}');</script>", url);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Print", script, false);

                this.ddlLocalizacion.Items.Add(new ListItem(hdfLocalizacionCompleta.Value, hdfLocalizacionCompleta.Value));
                this.ddlLocalizacion.SelectedValue = this.hdfLocalizacionCompleta.Value;
                this.txtProvincia.Text = this.hdfProvincia.Value;
                this.txtLocalidad.Text = this.hdfLocalidad.Value;
            }
            this.txtCantidadMaquinasLavado.Text = this.hdfCantidadMaquinasLavado.Value == string.Empty ? "0" : this.hdfCantidadMaquinasLavado.Value;
            this.txtCantidadMaquinasSecado.Text = this.hdfCantidadMaquinasSecado.Value == string.Empty ? "0" : this.hdfCantidadMaquinasSecado.Value;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            if (this.hdfLongitud.Value != string.Empty && Decimal.TryParse(this.hdfLongitud.Value, out decimal result))
            {
                bool guardo = true;
                this.btnAceptar.Visible = false;
                this.MapearControlesAObjeto(this.MiEdificio);
                this.MiEdificio.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        MiEdificio.IdEdificio = 0;
                        guardo = EdificiosF.EdificiosAgregar(this.MiEdificio);
                        if (guardo)
                        {
                            this.btnAgregar.Visible = true;
                            this.MostrarMensaje(this.MiEdificio.CodigoMensaje, false);
                        }
                        break;
                    case Gestion.Anular:
                        this.MiEdificio.Estado.IdEstado = (int)Estados.Baja;
                        guardo = EdificiosF.EdificiosModificar(this.MiEdificio);
                        if (guardo)
                        {
                            this.MostrarMensaje(this.MiEdificio.CodigoMensaje, false);
                        }
                        break;
                    case Gestion.Modificar:
                        guardo = EdificiosF.EdificiosModificar(this.MiEdificio);
                        if (guardo)
                        {
                            this.MostrarMensaje(this.MiEdificio.CodigoMensaje, false);
                        }
                        break;
                    default:
                        break;
                }
                if (!guardo)
                {
                    this.btnAceptar.Visible = true;
                    this.MostrarMensaje(this.MiEdificio.CodigoMensaje, true, this.MiEdificio.CodigoMensajeArgs);
                    if (this.MiEdificio.dsResultado != null)
                    {
                        this.MiEdificio.dsResultado = null;
                    }
                }
            }
            else
            {
                this.MostrarMensaje("Debe ingresar la direccion.", true, this.MiEdificio.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/EdificiosAgregar.aspx"), true);
        }

        #region GRILLA MAQUINAS
        protected void btnAgregarMaquina_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/MaquinasAgregar.aspx"), true);
        }


        protected void gvMaquinas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Anular.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdMaquina", this.MiEdificio.Maquinas[indiceColeccion].IdMaquina);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.MiEdificio.Maquinas[indiceColeccion].EstadoColeccion = EstadoColecciones.Borrado;
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "RestarMaquinas", string.Format("RestarMaquinas('{0}');", this.MiEdificio.Maquinas[indiceColeccion].TipoMaquinaCodigo.ToLower().Trim()), true);
                AyudaProgramacion.AcomodarIndices<LavMaquinas>(this.MiEdificio.Maquinas);
                AyudaProgramacion.CargarGrillaListas<LavMaquinas>(this.MiEdificio.Maquinas, true, this.gvMaquinas, true);
            }
        }

        protected void gvMaquinas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                        btnEliminar.Visible = true;
                        break;
                    case Gestion.Consultar:
                        e.Row.FindControl("btnEliminar").Visible = false;
                        break;
                    default:
                        break;
                }
            }
        }

        private void CargarComboMaquinas()
        {
            this.ddlMaquinas.DataSource = MaquinasF.MaquinasEdificiosObtenerMaquinas();
            this.ddlMaquinas.DataValueField = "IdMaquina";
            this.ddlMaquinas.DataTextField = "InformacionCombo";
            this.ddlMaquinas.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlMaquinas, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnCargarMaquina_Click(object sender, EventArgs e)
        {
            this.Page.Validate("CargarMaquina");

            if (!this.Page.IsValid)
            {
                this.upMaquinas.Update();
                return;
            }

            LavMaquinas maquina = new LavMaquinas();
            maquina.IdMaquina = this.ddlMaquinas.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMaquinas.SelectedValue);

            if (maquina.IdMaquina > 0)
            {
                List<LavMaquinas> aux = new List<LavMaquinas>();
                maquina = MaquinasF.MaquinasObtenerDatosCompletos(maquina);
                maquina.Numero = this.txtNumeroMaquina.Text == string.Empty ? 0 : Convert.ToInt32(this.txtNumeroMaquina.Text);
                maquina.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiEdificio.Maquinas.Add(maquina);
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SumarMaquinas", string.Format("SumarMaquinas('{0}');", maquina.TipoMaquinaCodigo.ToLower().Trim()), true);
                aux = this.MiEdificio.Maquinas.FindAll(x => x.EstadoColeccion != EstadoColecciones.Borrado);
                AyudaProgramacion.CargarGrillaListas<LavMaquinas>(aux, false, this.gvMaquinas, true);
                this.upMaquinas.Update();
            }
        }
        #endregion
        #region GRILLA REQUERIMIENTOS

        protected void gvRequerimientos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdRequerimiento", id);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/CRM/RequerimientosConsultar.aspx"), true);

        }

        protected void gvRequerimientos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                modificar.Visible = this.ValidarPermiso("RequerimientosModificar.aspx");
                consultar.Visible = this.ValidarPermiso("RequerimientosConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisRequerimientos.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }


        private void CargarListaRequerimientos(LavEdificios edificio)
        {

            this.MisRequerimientos = EdificiosF.EdificiosObtenerListaGrillaRequerimientos(edificio);
            this.gvRequerimientos.DataSource = this.MisRequerimientos;
            this.gvRequerimientos.DataBind();
            AyudaProgramacion.FixGridView(gvRequerimientos);

        }
        #endregion
        #region GRILLA PUNTOS DE VENTA

        protected void gvPuntosVentas_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("IdPuntoVenta", id);

            if (e.CommandName == Gestion.Modificar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/PuntosVentasModificar.aspx"), true);
            else if (e.CommandName == Gestion.Consultar.ToString())
                this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/LavaYa/PuntosVentasConsultar.aspx"), true);

        }

        protected void gvPuntosVentas_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton consultar = (ImageButton)e.Row.FindControl("btnConsultar");
                //ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");

                //modificar.Visible = this.ValidarPermiso("PuntosVentasModificar.aspx");
                //consultar.Visible = this.ValidarPermiso("PuntosVentasConsultar.aspx");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisPuntosVentas.Rows.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        private void CargarListaPuntosVentas(LavEdificios edificio)
        {
            this.MisPuntosVentas = EdificiosF.EdificiosObtenerListaGrillaPuntosVentas(edificio);
            this.gvPuntosVentas.DataSource = this.MisPuntosVentas;
            this.gvPuntosVentas.DataBind();
            AyudaProgramacion.FixGridView(gvPuntosVentas);
        }
        #endregion
    }
}