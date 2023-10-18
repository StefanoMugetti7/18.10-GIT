using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;
using System.Collections;
using Generales.Entidades;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class AsientosModelosDatos : ControlesSeguros
    {
        private CtbAsientosModelos MiAsientoModelo
        {
            get { return (CtbAsientosModelos)Session[this.MiSessionPagina + "MiAsientoModelo"]; }
            set { Session[this.MiSessionPagina + "MiAsientoModelo"] = value; }
        }

        private int MiPosicion
        {
            get { return (int)Session[this.MiSessionPagina + "MiPosicion"]; }
            set { Session[this.MiSessionPagina + "MiPosicion"] = value; }
        }

        //private CtbCuentasContables MiCuentaContable
        //{
        //    get { return (CtbCuentasContables)Session[this.MiSessionPagina + "MiCuentaContable"]; }
        //    set { Session[this.MiSessionPagina + "MiCuentaContable"] = value; }
        //}

        private List<TGEListasValoresSistemasDetalles> MisCodigosAMD
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "MisCodigosAMD"]; }
            set { Session[this.MiSessionPagina + "MisCodigosAMD"] = value; }
        }

        public delegate void AsientoModeloDatosAceptarEventHandler(object sender, CtbAsientosModelos e);
        public event AsientoModeloDatosAceptarEventHandler AsientoModeloDatosAceptar;

        public delegate void AsientoModeloDatosCancelarEventHandler();
        public event AsientoModeloDatosCancelarEventHandler AsientoModeloDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.puCuentasContables.CuentasContablesBuscarSeleccionarPopUp += PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp;
            if (!this.IsPostBack)
            {
                if (this.MiAsientoModelo == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            else
            {
                this.PersistirDatosGrilla();
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Cuenta Modelo
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbAsientosModelos pAsientoModelo, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MisCodigosAMD = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.CodigosAsientosModelos);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiAsientoModelo = pAsientoModelo;
                    this.CargarItemsPorDefecto();
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ddlEstado.Enabled = false;
                    this.ddlTipoAsiento.Enabled = true;
                    AyudaProgramacion.CargarGrillaListas(this.MiAsientoModelo.AsientosModelosDetalles, true, this.gvDatos, true);
                    break;
                case Gestion.Modificar:
                    this.MiAsientoModelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(pAsientoModelo);
                    this.MapearObjetoAControles(this.MiAsientoModelo);
                    this.ddlTipoAsiento.Enabled = false;
                    if (EnumTiposAsientos.Manuales.ToString() == this.ddlTipoAsiento.SelectedItem.Text)
                    {
                        this.gvDatos.Columns[2].Visible = false;
                        this.ddlTiposOperaciones.Enabled = false;
                    }
                    
                    break;
                case Gestion.Consultar:
                    this.MiAsientoModelo = ContabilidadF.AsientosModelosObtenerDatosCompletos(pAsientoModelo);
                    this.MapearObjetoAControles(this.MiAsientoModelo);
                    this.ddlTipoAsiento.Enabled = false;
                    this.btnAgregarCuenta.Visible = false;
                    this.txtDetalle.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.btnAceptar.Visible = false;
                    this.gvDatos.Columns[3].Visible = false;
                    
                    if (EnumTiposAsientos.Manuales.ToString() == this.ddlTipoAsiento.SelectedItem.Text)
                    {
                        this.gvDatos.Columns[2].Visible = false;
                        this.ddlTiposOperaciones.Enabled = false;
                    }
                    break;
                default:
                    break;
            }
        }

        private void CargarItemsPorDefecto()
        {
            CtbAsientosModelosDetalles detalle;
            for (int i = 0; i < 2; i++)
            {
                detalle = new CtbAsientosModelosDetalles();
                this.MiAsientoModelo.AsientosModelosDetalles.Add(detalle);
                detalle.IndiceColeccion = this.MiAsientoModelo.AsientosModelosDetalles.IndexOf(detalle);
            }
        }

        private void MapearObjetoAControles(CtbAsientosModelos pAsientoModelo)
        {
            this.txtDetalle.Text = pAsientoModelo.Detalle;
            this.ddlTipoAsiento.SelectedValue = pAsientoModelo.TipoAsiento.IdTipoAsiento.ToString();
            ListItem item = this.ddlTiposOperaciones.Items.FindByValue(pAsientoModelo.TipoOperacion.IdTipoOperacion.ToString());
            if (item == null && pAsientoModelo.TipoOperacion.IdTipoOperacion > 0)
                this.ddlTiposOperaciones.Items.Add(new ListItem(pAsientoModelo.TipoOperacion.TipoOperacion, pAsientoModelo.TipoOperacion.IdTipoOperacion.ToString()));
            this.ddlTiposOperaciones.SelectedValue = pAsientoModelo.TipoOperacion.IdTipoOperacion > 0 ? pAsientoModelo.TipoOperacion.IdTipoOperacion.ToString() : string.Empty;
            this.ddlEstado.SelectedValue = pAsientoModelo.Estado.IdEstado.ToString();

            item = this.ddlEjercicioContable.Items.FindByValue(pAsientoModelo.EjercicioContable.IdEjercicioContable.Value.ToString());
            if (item == null)
                this.ddlEjercicioContable.Items.Add(new ListItem(pAsientoModelo.EjercicioContable.Descripcion, pAsientoModelo.EjercicioContable.IdEjercicioContable.Value.ToString()));
            this.ddlEjercicioContable.SelectedValue = pAsientoModelo.EjercicioContable.IdEjercicioContable.Value.ToString();

            AyudaProgramacion.CargarGrillaListas(this.MiAsientoModelo.AsientosModelosDetalles, true, this.gvDatos, true);
            //if (this.MiAsientoModelo.AsientosModelosDetalles.Count > 0)
            //    this.MapearObjetoAGrilla();        

            //this.cdFechaAsiento.StartDate = pAsientoModelo.EjercicioContable.FechaInicio;
            //this.cdFechaAsiento.EndDate = pAsientoModelo.EjercicioContable.FechaFin;
            this.txtFechaAsiento.Text = pAsientoModelo.EjercicioContable.FechaInicio.ToShortDateString();
        }

        private void MapearControlesAObjeto(CtbAsientosModelos pAsientoModelo)
        {
            pAsientoModelo.EjercicioContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            pAsientoModelo.Detalle = this.txtDetalle.Text;
            pAsientoModelo.TipoAsiento.IdTipoAsiento = Convert.ToInt32(this.ddlTipoAsiento.SelectedValue);
            pAsientoModelo.TipoOperacion.IdTipoOperacion = this.ddlTiposOperaciones.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTiposOperaciones.SelectedValue);
            pAsientoModelo.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pAsientoModelo.ModificarAsientos = this.chkModificarAsientos.Checked;
            pAsientoModelo.FechaAsientoDesde = this.txtFechaAsiento.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaAsiento.Text);
            this.MapearGrillaAObjeto();
        }

        private void CargarCombos()
        {
            //Cargar Ejercicios contables
            CtbEjerciciosContables filtroAsiento = new CtbEjerciciosContables();
            filtroAsiento.Estado.IdEstado = (int)Estados.Activo;
            List<CtbEjerciciosContables> lista = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtroAsiento);
            this.ddlEjercicioContable.DataSource = lista;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();

            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            List<TGEListasValoresSistemasDetalles> tiposAsiento = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposAsientos);
            tiposAsiento = tiposAsiento.Where(x => x.IdListaValorSistemaDetalle == (int)EnumTiposAsientos.Automaticos
                                    || x.IdListaValorSistemaDetalle == (int)EnumTiposAsientos.Manuales).ToList();
            tiposAsiento = AyudaProgramacion.AcomodarIndices<TGEListasValoresSistemasDetalles>(tiposAsiento);
            this.ddlTipoAsiento.DataSource = tiposAsiento;
            this.ddlTipoAsiento.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoAsiento.DataTextField = "Descripcion";
            this.ddlTipoAsiento.DataBind();

            TGETiposOperaciones filtro = new TGETiposOperaciones();
            filtro.TipoFuncionalidad.IdTipoFuncionalidad = this.paginaSegura.paginaActual.IdTipoFuncionalidad;
            this.ddlTiposOperaciones.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(filtro);
            this.ddlTiposOperaciones.DataValueField = "IdTipoOperacion";
            this.ddlTiposOperaciones.DataTextField = "TipoOperacion";
            this.ddlTiposOperaciones.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposOperaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void ddlEjercicioContable_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MiAsientoModelo.AsientosModelosDetalles = new List<CtbAsientosModelosDetalles>();
            this.CargarItemsPorDefecto();
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoModelo.AsientosModelosDetalles, false, this.gvDatos, true);
            this.upCuentasContables.Update();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiAsientoModelo);
            this.MiAsientoModelo.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = ContabilidadF.AsientosModelosAgregar(this.MiAsientoModelo);
                    break;
                case Gestion.Modificar:
                    guardo = ContabilidadF.AsientosModelosModificar(this.MiAsientoModelo);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAsientoModelo.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiAsientoModelo.CodigoMensaje, true, this.MiAsientoModelo.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.AsientoModeloDatosCancelar != null)
                this.AsientoModeloDatosCancelar();
        }

        protected void btnAgregarCuenta_Click(object sender, EventArgs e)
        {
            //this.MapearGrillaAObjeto();
            CtbAsientosModelosDetalles item = new CtbAsientosModelosDetalles();
            item.EstadoColeccion=EstadoColecciones.Agregado;
            this.MiAsientoModelo.AsientosModelosDetalles.Add(item);
            item.IndiceColeccion = this.MiAsientoModelo.AsientosModelosDetalles.IndexOf(item);
            //this.MiAsientoModelo.AsientosModelosDetalles.Add(new CtbAsientosModelosDetalles());
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoModelo.AsientosModelosDetalles, true, this.gvDatos, true);
            //this.MapearObjetoAGrilla();
            this.upCuentasContables.Update();
            this.MostrarOcultarColumnas();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.AsientoModeloDatosAceptar != null)
                this.AsientoModeloDatosAceptar(null, this.MiAsientoModelo);
        }

        protected void txtNumeroCuenta_TextChanged(object sender, EventArgs e)
        {
            TextBox txtNumeroCuenta = (TextBox)sender;
            GridViewRow row = txtNumeroCuenta.NamingContainer as GridViewRow;
            this.MiPosicion = Convert.ToInt32(this.gvDatos.DataKeys[row.RowIndex].Value);
            this.MapearGrillaAObjeto();
            if (string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
            {
                this.MostrarMensaje("ValidarSeleccionarEjercicioContable", true);
                this.PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp(new CtbCuentasContables());
                return;
            }
            if (txtNumeroCuenta.Text.Trim() == string.Empty)
            {
                this.PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp(new CtbCuentasContables());
                return;
            }

            List<CtbCuentasContables> cuentasContablesBuscar = new List<CtbCuentasContables>();
            CtbCuentasContables cuentaContable = new CtbCuentasContables();
            cuentaContable.Estado.IdEstado = (int)Estados.Activo;
            cuentaContable.NumeroCuenta = ((TextBox)sender).Text.Trim();
            cuentaContable.Imputable = true;
            cuentaContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            cuentasContablesBuscar = ContabilidadF.CuentasContablesObtenerListaFiltro(cuentaContable);
            if (cuentasContablesBuscar.Count == 1)
            {
                this.PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp(cuentasContablesBuscar[0]);
            }
            else if (cuentasContablesBuscar.Count > 1)
            {
                this.puCuentasContables.IniciarControl(true, cuentaContable, cuentasContablesBuscar);
            }
            else
            {
                this.puCuentasContables.IniciarControl(true, cuentaContable, new List<CtbCuentasContables>());
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            if (e.CommandArgument.ToString() == string.Empty)
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            //this.MiPosicion = index;
            this.MiPosicion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            if (e.CommandName == "BuscarCuentaContable")
            {
                this.MapearGrillaAObjeto();
                if (string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
                {
                    this.MostrarMensaje("ValidarSeleccionarEjercicioContable", true);
                    return;
                }
                CtbCuentasContables cta = new CtbCuentasContables();
                cta.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
                this.puCuentasContables.IniciarControl(true, cta, new List<CtbCuentasContables>());
            }
            else if (e.CommandName == "EliminarCuentaContable")
            {
                this.PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp(new CtbCuentasContables());
            }
            else if (e.CommandName == "Borrar")
            {
                this.MiAsientoModelo.Detalle = this.txtDetalle.Text;
                this.MiAsientoModelo.TipoAsiento.IdTipoAsiento = Convert.ToInt32(this.ddlTipoAsiento.SelectedValue);
                this.MiAsientoModelo.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
                this.MiAsientoModelo.AsientosModelosDetalles.RemoveAt(this.MiPosicion);
                AyudaProgramacion.CargarGrillaListas(this.MiAsientoModelo.AsientosModelosDetalles, true, this.gvDatos, true);
                this.MapearObjetoAControles(this.MiAsientoModelo);
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CtbAsientosModelosDetalles detalle = (CtbAsientosModelosDetalles)e.Row.DataItem;

                ImageButton btnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                ImageButton btnEliminarCuenta = (ImageButton)e.Row.FindControl("btnEliminarCuenta");
                string mensaje = this.ObtenerMensajeSistema("CuentasContablesConfirmarBaja");
                mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnEliminar.Attributes.Add("OnClick", funcion);
                this.MiPosicion = e.Row.RowIndex;
                DropDownList ddlTipoImputacion = ((DropDownList)e.Row.FindControl("ddlTipoImputacion"));
                ddlTipoImputacion.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TipoImputacion);
                ddlTipoImputacion.DataValueField = "IdListaValorSistemaDetalle";
                ddlTipoImputacion.DataTextField = "Descripcion";
                ddlTipoImputacion.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlTipoImputacion, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                
                DropDownList ddlCodigoAMD = ((DropDownList)e.Row.FindControl("ddlCodigoAMD"));
                ddlCodigoAMD.DataSource = this.MisCodigosAMD;
                ddlCodigoAMD.DataTextField = "Descripcion";
                ddlCodigoAMD.DataValueField = "IdListaValorSistemaDetalle";
                ddlCodigoAMD.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(ddlCodigoAMD, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                RequiredFieldValidator rfv = ((RequiredFieldValidator)e.Row.FindControl("rfvTipoImputacion"));
                if (Convert.ToInt32(this.ddlTipoAsiento.SelectedValue) == (int)EnumTiposAsientos.Automaticos)
                    rfv.Enabled = true;
                else
                    rfv.Enabled = false;

                //CuentasContablesBuscar cc = (CuentasContablesBuscar)e.Row.FindControl("buscarCuenta");
                TextBox txtNumeroCuenta = (TextBox)e.Row.FindControl("txtNumeroCuenta");
                TextBox txtDescripcion = (TextBox)e.Row.FindControl("txtDescripcion");
                txtNumeroCuenta.Text = detalle.CuentaContable.NumeroCuenta;
                txtDescripcion.Text = detalle.CuentaContable.Descripcion;
                ListItem item;
                switch (this.GestionControl)
                {
                    case Gestion.Agregar:
                        btnEliminar.Visible = true;
                        btnEliminarCuenta.Visible = detalle.CuentaContable.IdCuentaContable>0;
                        //cc.MapearObjetoControles(detalle.CuentaContable, this.GestionControl, detalle.IndiceColeccion);
                        ddlCodigoAMD.SelectedValue = detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo == 0 ? string.Empty : detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo.ToString();
                        ddlTipoImputacion.SelectedValue = detalle.TipoImputacion.IdTipoImputacion==0 ? string.Empty : detalle.TipoImputacion.IdTipoImputacion.ToString();
                        break;
                    case Gestion.Consultar:
                        btnEliminar.Visible = false;
                        btnEliminarCuenta.Visible = false;
                        //cc.Enable(false);
                        //cc.MapearObjetoControles(detalle.CuentaContable, this.GestionControl, detalle.IndiceColeccion);
                        ddlTipoImputacion.Enabled = false;
                        ddlTipoImputacion.SelectedValue = detalle.TipoImputacion.IdTipoImputacion.ToString();
                        ddlCodigoAMD.Enabled = false;
                        item = ddlCodigoAMD.Items.FindByValue(detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo.ToString());
                        if (item == null && detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo>0)
                            ddlCodigoAMD.Items.Add(new ListItem(detalle.AsientoModeloDetalleCodigo.Descripcion, detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo.ToString()));
                        ddlCodigoAMD.SelectedValue = detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo==0? string.Empty : detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo.ToString();
                        break;
                    case Gestion.Modificar:
                        btnEliminar.Visible = true;
                        btnEliminarCuenta.Visible = detalle.CuentaContable.IdCuentaContable > 0;
                        //cc.MapearObjetoControles(detalle.CuentaContable, this.GestionControl, detalle.IndiceColeccion);
                        ddlTipoImputacion.SelectedValue = detalle.TipoImputacion.IdTipoImputacion.ToString();
                        item = ddlCodigoAMD.Items.FindByValue(detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo.ToString());
                        if (item == null && detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo>0)
                            ddlCodigoAMD.Items.Add(new ListItem(detalle.AsientoModeloDetalleCodigo.Descripcion, detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo.ToString()));
                        ddlCodigoAMD.SelectedValue = detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo==0? string.Empty : detalle.AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo.ToString();
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiAsientoModelo.AsientosModelosDetalles.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //this.MapearGrillaAObjeto();
            CtbAsientosModelos parametros = this.BusquedaParametrosObtenerValor<CtbAsientosModelos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbAsientosModelos>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiAsientoModelo.AsientosModelosDetalles;
            this.gvDatos.DataBind();
            //this.MapearObjetoAGrilla();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MapearGrillaAObjeto();
            this.MiAsientoModelo.AsientosModelosDetalles = this.OrdenarGrillaDatos<CtbAsientosModelosDetalles>(this.MiAsientoModelo.AsientosModelosDetalles, e);
            this.gvDatos.DataSource = this.MiAsientoModelo.AsientosModelosDetalles;
            this.gvDatos.DataBind();
            //this.MapearObjetoAGrilla();
        }

        //protected void buscarCuenta_CuentasContablesBuscarIniciar(CtbEjerciciosContables ejercicio)
        //{
        //    if (string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
        //    {
        //        this.MostrarMensaje("ValidarSeleccionarEjercicioContable", true);
        //        return;
        //    }
        //    ejercicio.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
        //}

        protected void PuCuentasContables_CuentasContablesBuscarSeleccionarPopUp(CtbCuentasContables e)
        {
            this.MiAsientoModelo.AsientosModelosDetalles[this.MiPosicion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAsientoModelo.AsientosModelosDetalles[this.MiPosicion], this.GestionControl);
            this.MiAsientoModelo.AsientosModelosDetalles[this.MiPosicion].CuentaContable = e;

            this.CargarLista();
            this.upCuentasContables.Update();
            this.MostrarOcultarColumnas();

            if (this.MiAsientoModelo.AsientosModelosDetalles.Exists(x => x.EstadoColeccion == EstadoColecciones.Modificado))
            {
                this.pnlModificarAsientos.Visible = true;
                this.txtFechaAsiento.Text = this.MiAsientoModelo.EjercicioContable.FechaInicio.ToShortDateString();
                double desde = this.MiAsientoModelo.EjercicioContable.FechaInicio.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                double hasta = this.MiAsientoModelo.EjercicioContable.FechaFin.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
                string script = string.Format("InitControlFecha('{0}', '{1}');", desde, hasta);
                ScriptManager.RegisterStartupScript(this.upModificarAsientos, this.upModificarAsientos.GetType(), "InitControlFechaScript", script, true);
            }
            else
            {
                this.pnlModificarAsientos.Visible = false;
            }
            this.upModificarAsientos.Update();
        }

        //protected void buscarCuenta_CuentasContablesBuscarEliminar(int pIndiceColeecion)
        //{
        //    var asientoModeloDetalle = this.MiAsientoModelo.AsientosModelosDetalles[pIndiceColeecion];
        //    asientoModeloDetalle.CuentaContable = null;
        //    //this.MapearObjetoAGrilla();
        //    this.CargarLista();
        //    this.upCuentasContables.Update();
        //    this.MostrarOcultarColumnas();
        //}

        protected void ddlTipoAsiento_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Convert.ToInt32(this.ddlTipoAsiento.SelectedValue) == (int)EnumTiposAsientos.Automaticos)
            {
                this.ddlTiposOperaciones.Enabled = true;
                this.gvDatos.Columns[5].Visible = true;
                this.gvDatos.Columns[6].Visible = true;
                this.gvDatos.Columns[7].Visible = true;
            }
            else
            {
                this.ddlTiposOperaciones.Enabled = false;
                this.gvDatos.Columns[5].Visible = false;
                this.gvDatos.Columns[6].Visible = false;
                this.gvDatos.Columns[7].Visible = false;
            }
            this.CargarLista();
            upCuentasContables.Update();
        }

        private void MostrarOcultarColumnas()
        {
            //ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "MyFun1", "show_hide_column();", true);
        }

        private void CargarLista()
        {
            AyudaProgramacion.CargarGrillaListas(this.MiAsientoModelo.AsientosModelosDetalles, true, this.gvDatos, true);
            //this.gvDatos.DataSource = this.MiAsientoModelo.AsientosModelosDetalles;
            //this.gvDatos.DataBind();
        }

        private void PersistirDatosGrilla()
        {
            this.MapearGrillaAObjeto();
        }

        private void MapearGrillaAObjeto()
        {
            foreach (GridViewRow fila in this.gvDatos.Rows)
            {
                int indiceColeccion = fila.DataItemIndex;
                DropDownList ddlTipoImputacion = ((DropDownList)fila.FindControl("ddlTipoImputacion"));
                DropDownList ddlCodigoAMD = (DropDownList)fila.FindControl("ddlCodigoAMD");
                //CuentasContablesBuscar cc = (CuentasContablesBuscar)fila.FindControl("buscarCuenta");
                //HiddenField hfId = (HiddenField)cc.FindControl("hfIdCuentaContable");
                HiddenField hfId = (HiddenField)fila.FindControl("hdfIdCuentaContable");
                if (ddlTipoImputacion.SelectedValue != "")
                {
                    this.MiAsientoModelo.AsientosModelosDetalles[indiceColeccion].TipoImputacion.IdTipoImputacion = Convert.ToInt32(ddlTipoImputacion.SelectedValue);
                }
                if ((hfId.Value != string.Empty && hfId.Value != "0"
                    && this.MiAsientoModelo.AsientosModelosDetalles[indiceColeccion].CuentaContable.IdCuentaContable.ToString() != hfId.Value)
                    || ddlCodigoAMD.SelectedValue != string.Empty
                    )
                {
                    if (this.MiAsientoModelo.AsientosModelosDetalles[indiceColeccion].EstadoColeccion != EstadoColecciones.Agregado)
                        this.MiAsientoModelo.AsientosModelosDetalles[indiceColeccion].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiAsientoModelo.AsientosModelosDetalles[indiceColeccion], this.GestionControl);
                    string codigoAMD = ((DropDownList)fila.FindControl("ddlCodigoAMD")).SelectedValue;
                    if (!string.IsNullOrEmpty(codigoAMD))
                    {
                        int indiceCodigoAMD = ((DropDownList)fila.FindControl("ddlCodigoAMD")).SelectedIndex;
                        this.MiAsientoModelo.AsientosModelosDetalles[indiceColeccion].AsientoModeloDetalleCodigo.IdAsientoModeloDetalleCodigo = this.MisCodigosAMD[indiceCodigoAMD].IdListaValorSistemaDetalle;
                        this.MiAsientoModelo.AsientosModelosDetalles[indiceColeccion].AsientoModeloDetalleCodigo.CodigoValor = this.MisCodigosAMD[indiceCodigoAMD].CodigoValor;
                        this.MiAsientoModelo.AsientosModelosDetalles[indiceColeccion].AsientoModeloDetalleCodigo.Descripcion = this.MisCodigosAMD[indiceCodigoAMD].Descripcion;
                    }
                }
            }
        }
    }
}