using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Cargos.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Cargos;
using Generales.Entidades;
using Contabilidad;
using Afiliados.Entidades;
using Afiliados;
using System.Collections.Generic;
using Evol.Controls;

namespace IU.Modulos.Cargos.Controles
{
    public partial class TipoCargoModificarDatos : ControlesSeguros
    {
        private CarTiposCargos MiCarTiposCargos
        {
            get { return (CarTiposCargos)Session[this.MiSessionPagina + "CargoAgregarMiCarTiposCargos"]; }
            set { Session[this.MiSessionPagina + "CargoAgregarMiCarTiposCargos"] = value; }
        }

        private TGEEstados MiEstadoBaja
        {
            get { return (TGEEstados)Session[this.MiSessionPagina + "CargoAgregarMiEstadoBaja"]; }
            set { Session[this.MiSessionPagina + "CargoAgregarMiEstadoBaja"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "TipoCargoModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "TipoCargoModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        private List<TGEListasValoresSistemasDetalles> MisCarTiposCargosRangos
        {
            get { return (List<TGEListasValoresSistemasDetalles>)Session[this.MiSessionPagina + "TipoCargoModificarDatosMisCarTiposCargosRangos"]; }
            set { Session[this.MiSessionPagina + "TipoCargoModificarDatosMisCarTiposCargosRangos"] = value; }
        }

        private List<TGEEstados> MisEstados
        {
            get { return (List<TGEEstados>)Session[this.MiSessionPagina + "TipoCargoModificarDatosMisEstados"]; }
            set { Session[this.MiSessionPagina + "TipoCargoModificarDatosMisEstados"] = value; }
        }

        public delegate void AfiliadoDatosAceptarEventHandler(object sender, CarTiposCargos e);
        public event AfiliadoDatosAceptarEventHandler TipoCargoModificarDatosAceptar;
        public delegate void AfiliadoDatosCancelarEventHandler();
        public event AfiliadoDatosCancelarEventHandler TipoCargoModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            //this.ctrCuentasContables.CuentasContablesBuscarSeleccionar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarEventHandler(ctrCuentasContables_CuentasContablesBuscarSeleccionar);
            //this.ctrCuentasContables.CuentasContablesBuscarIniciar += new Contabilidad.Controles.CuentasContablesBuscar.CuentasContablesBuscarIniciarEventHandler(ctrCuentasContables_CuentasContablesBuscarIniciar);
            if (this.IsPostBack)
            {
                if (this.MiCarTiposCargos == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }

                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtCodigo, this.btnAceptar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtTipoCargo, this.btnAceptar);
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtImporte, this.btnAceptar);
                
                if(this.MiCarTiposCargos.TiposCargosCategorias.Count() > 0)
                    this.PersistirDatosGrilla();
            }
            
        }

        void ctrCuentasContables_CuentasContablesBuscarIniciar(global::Contabilidad.Entidades.CtbEjerciciosContables ejercicio)
        {
            AyudaProgramacion.MatchObjectProperties(ContabilidadF.EjerciciosContablesObtenerUltimoActivo(), ejercicio);
        }

        void ctrCuentasContables_CuentasContablesBuscarSeleccionar(global::Contabilidad.Entidades.CtbCuentasContables e, int indiceColeccion)
        {
            this.MiCarTiposCargos.CuentaContable = e;
           // this.upCuentasContables.Update();
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CarTiposCargos pTipoCargo, Gestion pGestion)
        {
            this.MiCarTiposCargos = pTipoCargo;
            this.GestionControl = pGestion;
            this.CargarCombos();
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCarTiposCargos = pTipoCargo;
                    this.ddlEstado.SelectedValue = ((int)Estados.Activo).ToString();
                    this.txtUsuarioAlta.Text = this.UsuarioActivo.ApellidoNombre;
                    this.txtFechaAlta.Text = DateTime.Today.ToString();
                    this.ddlEstado.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.txtUsuarioAlta.Enabled = false;
                    this.lblCategorias.Visible = true;
                    this.ddlCategorias.Visible = true;
                    this.btnAgregar.Visible = true;
                    this.btnAgregarRangos.Visible = true;
                    this.CargarCategorias();
                    this.ctrArchivos.IniciarControl(this.MiCarTiposCargos, this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiCarTiposCargos, new Objeto(), this.GestionControl);
                    this.ctrCamposValores.IniciarControl(this.MiCarTiposCargos, this.MiCarTiposCargos, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiCarTiposCargos = CargosF.TiposCargosObtenerDatosCompletos(pTipoCargo);
                    this.MapearObjetoAControles(this.MiCarTiposCargos);
                    this.txtFechaAlta.Enabled = false;
                    this.txtUsuarioAlta.Enabled = false;
                    this.lblCategorias.Visible = true;
                    this.ddlCategorias.Visible = true;
                    this.btnAgregar.Visible = true;
                    this.btnAgregarRangos.Visible = true;
                    this.ddlTipoCargoProceso.Enabled = UsuarioActivo.EsAdministradorGeneral;
                    this.CargarCategorias();
                    break;
                case Gestion.Consultar:
                    this.MiCarTiposCargos = CargosF.TiposCargosObtenerDatosCompletos(pTipoCargo);
                    this.MapearObjetoAControles(this.MiCarTiposCargos);
                    this.txtCodigo.Enabled = false;
                    this.txtTipoCargo.Enabled = false;
                    this.txtImporte.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.txtFechaAlta.Enabled = false;
                    this.txtUsuarioAlta.Enabled = false;
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void MapearObjetoAControles(CarTiposCargos pTipoCargo)
        {
            this.txtCodigo.Text = pTipoCargo.CodigoCargo.ToString();
            this.txtTipoCargo.Text = pTipoCargo.TipoCargo;
            this.txtImporte.Text = pTipoCargo.Importe.ToString("C2");
            this.ddlEstado.SelectedValue = pTipoCargo.Estado.IdEstado.ToString();
            //this.ddlTiposOperaciones.SelectedValue = pTipoCargo.TipoOperacion.IdTipoOperacion == 0 ? string.Empty : pTipoCargo.TipoOperacion.IdTipoOperacion.ToString();
            this.txtFechaAlta.Text = pTipoCargo.FechaAlta.ToString();
            this.txtUsuarioAlta.Text = pTipoCargo.UsuarioAlta.Apellido;
            //this.chkCargoAutomatico.Checked = pTipoCargo.;
            this.txtPrioridad.Text = pTipoCargo.Prioridad.ToString();
            this.chkPermiteCuotas.Checked = pTipoCargo.PermiteCuotas;
            this.txtCantidadMaximaCuotas.Text = pTipoCargo.CantidadMaximaCuotas.ToString();
            this.txtPorcentaje.Text = pTipoCargo.Porcentaje.ToString();
            
            ListItem item;
            if (pTipoCargo.TipoOperacion.IdTipoOperacion > 0)
            {
                item = this.ddlTiposOperaciones.Items.FindByValue(pTipoCargo.TipoOperacion.IdTipoOperacion.ToString());
                if (item == null)
                    this.ddlTiposOperaciones.Items.Add(new ListItem(pTipoCargo.TipoOperacion.TipoOperacion, pTipoCargo.TipoOperacion.IdTipoOperacion.ToString()));
                this.ddlTiposOperaciones.SelectedValue = pTipoCargo.TipoOperacion.IdTipoOperacion.ToString();
            }

            //if (pTipoCargo.ConceptoContable.IdConceptoContable > 0)
            //{
            //    item = this.ddlConceptosContables.Items.FindByValue(pTipoCargo.ConceptoContable.IdConceptoContable.ToString());
            //    if (item == null && pTipoCargo.ConceptoContable.IdConceptoContable > 0)
            //        this.ddlConceptosContables.Items.Add(new ListItem(pTipoCargo.ConceptoContable.ConceptoContable, pTipoCargo.ConceptoContable.IdConceptoContable.ToString()));
            //    this.ddlConceptosContables.SelectedValue = pTipoCargo.ConceptoContable.IdConceptoContable.ToString();
            //}
            if (pTipoCargo.SumarizaConIdTipoCargo > 0)
            {
                item = this.ddlSumarizaConCargo.Items.FindByValue(pTipoCargo.SumarizaConIdTipoCargo.ToString());
                if (item == null)
                    this.ddlSumarizaConCargo.Items.Add(new ListItem(pTipoCargo.SumarizaConTipoCargo, pTipoCargo.SumarizaConIdTipoCargo.ToString()));
                this.ddlSumarizaConCargo.SelectedValue = pTipoCargo.SumarizaConIdTipoCargo.ToString();
            }
            hdfIdProducto.Value = pTipoCargo.Producto.IdProducto.ToString();
            hdfProductoDetalle.Value = pTipoCargo.Producto.Descripcion.ToString();
            this.chkAplicaSiTieneParticipantes.Checked = pTipoCargo.AplicaSiTieneParticipantes;
            this.chkAplicaPorCantidadParticipantes.Checked = pTipoCargo.AplicaPorCantidadParticipantes;
            this.chkDepositoCajaAhorro.Checked = pTipoCargo.DepositoCajaAhorro;
            //this.txtTipoCargoProceso.Text = pTipoCargo.TipoCargoProceso.Descripcion;
            item = this.ddlTipoCargoProceso.Items.FindByValue(pTipoCargo.TipoCargoProceso.IdTipoCargoProceso.ToString());
            if (item == null)
                this.ddlTipoCargoProceso.Items.Add(new ListItem(pTipoCargo.TipoCargoProceso.Descripcion, pTipoCargo.TipoCargoProceso.IdTipoCargoProceso.ToString()));
            this.ddlTipoCargoProceso.SelectedValue = pTipoCargo.TipoCargoProceso.IdTipoCargoProceso.ToString();


            if (pTipoCargo.Producto.IdProducto > 0)
            {
                this.ddlProducto.Items.Add(new ListItem(pTipoCargo.Producto.Descripcion, pTipoCargo.Producto.IdProducto.ToString()));
                this.ddlProducto.SelectedValue = pTipoCargo.Producto.IdProducto.ToString();
            }


            if (pTipoCargo.CuentaContable.IdCuentaContable > 0)
            {
                this.hdfIdCuentaContable.Value = pTipoCargo.CuentaContable.IdCuentaContable.ToString();
                this.ddlCuentaContable.Items.Clear();
                this.ddlCuentaContable.Items.Add(new ListItem(pTipoCargo.CuentaContable.Descripcion, pTipoCargo.CuentaContable.IdCuentaContable.ToString()));
                this.ddlCuentaContable.SelectedValue = pTipoCargo.CuentaContable.IdCuentaContable.ToString();
            }

            this.chkCargoIrregular.Checked = pTipoCargo.CargoIrregular;

            foreach (TGEFormasCobros fp in pTipoCargo.FormasCobros)
            {
                item = this.chkFormasCobros.Items.FindByValue(fp.IdFormaCobro.ToString());
                if (item != null)
                    item.Selected = true;
            }
            AyudaProgramacion.CargarGrillaListas<CarTiposCargosCategorias>(pTipoCargo.TiposCargosCategorias, false, this.gvDatos, true);
            AyudaProgramacion.CargarGrillaListas<CarTiposCargosRangos>(pTipoCargo.TiposCargosRangos, false, this.gvRangos, true);
            //this.ctrCuentasContables.MapearObjetoControles(pTipoCargo.CuentaContable, this.GestionControl, 0);
            this.ctrComentarios.IniciarControl(pTipoCargo, this.GestionControl);
            this.ctrArchivos.IniciarControl(pTipoCargo, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pTipoCargo);
            this.ctrCamposValores.IniciarControl(pTipoCargo, new Objeto(), this.GestionControl);
            this.ctrCamposValores.IniciarControl(pTipoCargo, pTipoCargo, this.GestionControl);
            this.ctrCamposValores.IniciarControl(pTipoCargo, pTipoCargo.TipoOperacion, this.GestionControl);
            ddlTipoCargoProceso_SelectedIndexChanged(null, EventArgs.Empty);
        }

        private void MapearControlesAObjeto(CarTiposCargos pTipoCargo)
        {
            pTipoCargo.CodigoCargo = this.txtCodigo.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCodigo.Text.Trim());
            pTipoCargo.TipoCargo = this.txtTipoCargo.Text.Trim();
            pTipoCargo.Importe = this.txtImporte.Decimal;
            pTipoCargo.Porcentaje = this.txtPorcentaje.Decimal;
            pTipoCargo.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pTipoCargo.Estado.Descripcion = this.ddlEstado.SelectedItem.Text;
            pTipoCargo.Producto.IdProducto = this.hdfIdProducto.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdProducto.Value);
            pTipoCargo.Producto.Descripcion = hdfProductoDetalle.Value;

            pTipoCargo.CuentaContable.IdCuentaContable = this.hdfIdCuentaContable.Value == string.Empty ? 0 : Convert.ToInt32(this.hdfIdCuentaContable.Value);
            if (!string.IsNullOrEmpty(this.ddlTiposOperaciones.SelectedValue))
            {
                pTipoCargo.TipoOperacion.IdTipoOperacion = Convert.ToInt32(this.ddlTiposOperaciones.SelectedValue);
                pTipoCargo.TipoOperacion.TipoOperacion = this.ddlTiposOperaciones.SelectedItem.Text;
            }
            //pTipoCargo.CargoAutomatico = this.chkCargoAutomatico.Checked;
            pTipoCargo.PermiteCuotas = this.chkPermiteCuotas.Checked;
            pTipoCargo.CantidadMaximaCuotas = this.txtCantidadMaximaCuotas.Text == string.Empty ? 0 : Convert.ToInt32(this.txtCantidadMaximaCuotas.Text);
            //pTipoCargo.ConceptoContable.IdConceptoContable = Convert.ToInt32(this.ddlConceptosContables.SelectedValue);
            //pTipoCargo.ConceptoContable.ConceptoContable = this.ddlConceptosContables.SelectedItem.Text;
            pTipoCargo.Prioridad = this.txtPrioridad.Text == string.Empty ? 0 : Convert.ToInt32(this.txtPrioridad.Text);
            pTipoCargo.AplicaSiTieneParticipantes = this.chkAplicaSiTieneParticipantes.Checked;
            pTipoCargo.AplicaPorCantidadParticipantes = this.chkAplicaPorCantidadParticipantes.Checked;
            pTipoCargo.DepositoCajaAhorro = this.chkDepositoCajaAhorro.Checked;
            pTipoCargo.SumarizaConIdTipoCargo = this.ddlSumarizaConCargo.SelectedValue==string.Empty? 0 : Convert.ToInt32(this.ddlSumarizaConCargo.SelectedValue);
            pTipoCargo.SumarizaConTipoCargo = this.ddlSumarizaConCargo.SelectedValue==string.Empty? string.Empty : this.ddlSumarizaConCargo.SelectedItem.Text;
            
            pTipoCargo.CargoIrregular = this.chkCargoIrregular.Checked;

            pTipoCargo.TipoCargoProceso.IdTipoCargoProceso = Convert.ToInt32(this.ddlTipoCargoProceso.SelectedValue);
            pTipoCargo.TipoCargoProceso.Descripcion = this.ddlTipoCargoProceso.SelectedItem.Text;

            TGEFormasCobros formaCobro;
            foreach (ListItem lst in this.chkFormasCobros.Items)
            {
                formaCobro = pTipoCargo.FormasCobros.Find(x => x.IdFormaCobro == Convert.ToInt32(lst.Value));
                if (formaCobro == null && lst.Selected)
                {
                    formaCobro = new TGEFormasCobros();
                    formaCobro.IdFormaCobro = Convert.ToInt32(lst.Value);
                    formaCobro.FormaCobro = lst.Text;
                    pTipoCargo.FormasCobros.Add(formaCobro);
                    formaCobro.EstadoColeccion = EstadoColecciones.Agregado;
                    formaCobro.IndiceColeccion = pTipoCargo.FormasCobros.IndexOf(formaCobro);
                }
                else if (formaCobro != null && !lst.Selected)
                    formaCobro.EstadoColeccion = EstadoColecciones.Borrado;
            }

            pTipoCargo.Comentarios = ctrComentarios.ObtenerLista();
            pTipoCargo.Archivos = ctrArchivos.ObtenerLista();
            pTipoCargo.Campos = ctrCamposValores.ObtenerLista();
        }

        private void CargarCombos()
        {
            this.MisCarTiposCargosRangos = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposRangos);
            this.MisEstados = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataSource = this.MisEstados;
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();

            this.chkFormasCobros.DataSource = TGEGeneralesF.FormasCobrosObtenerLista();
            this.chkFormasCobros.DataValueField = "IdFormaCobro";
            this.chkFormasCobros.DataTextField = "FormaCobro";
            this.chkFormasCobros.DataBind();
            //this.chkFormasCobros.Items.Insert(0, new ListItem(this.ObtenerMensajeSistema("Todos"), ((int)EstadosTodos.Todos).ToString()));

            TGETiposOperaciones operacion = new TGETiposOperaciones();
            operacion.Estado.IdEstado = (int)EstadosTodos.Todos;
            this.ddlTiposOperaciones.DataSource = TGEGeneralesF.TiposOperacionesObtenerListaFiltro(operacion);
            this.ddlTiposOperaciones.DataValueField = "IdTipoOperacion";
            this.ddlTiposOperaciones.DataTextField = "TipoOperacion";
            this.ddlTiposOperaciones.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTiposOperaciones, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            ////Validar Conceptos por Funcionalidad para Tipos de Cargos.
            //TGETiposOperaciones opeFiltro = new TGETiposOperaciones();
            ////funcion.IdTipoFuncionalidad = (int)EnumTGETiposFuncionalidades.Cobros;
            //opeFiltro.Estado.IdEstado = (int)Estados.Activo;
            //this.ddlConceptosContables.DataSource = ContabilidadF.ConceptosContablesObtenerListaFiltro(opeFiltro);
            //this.ddlConceptosContables.DataValueField = "IdConceptoContable";
            //this.ddlConceptosContables.DataTextField = "ConceptoContable";
            //this.ddlConceptosContables.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlConceptosContables, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoCargoProceso.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.TiposCargosProcesos);
            this.ddlTipoCargoProceso.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlTipoCargoProceso.DataTextField = "Descripcion";
            this.ddlTipoCargoProceso.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoCargoProceso, this.ObtenerMensajeSistema("SeleccioneOpcion"));


            CarTiposCargos filtro =new CarTiposCargos();
            filtro.Estado.IdEstado=(int)EstadosCargos.Activo;
            List<CarTiposCargos> cargosSumariza=CargosF.TiposCargosObtenerListaFiltro(filtro);
            if(cargosSumariza.Exists(x=>x.IdTipoCargo==this.MiCarTiposCargos.IdTipoCargo))
                cargosSumariza.Remove(cargosSumariza.Find(x=>x.IdTipoCargo==this.MiCarTiposCargos.IdTipoCargo));
            this.ddlSumarizaConCargo.DataSource = cargosSumariza;
            this.ddlSumarizaConCargo.DataValueField = "IdTipoCargo";
            this.ddlSumarizaConCargo.DataTextField = "TipoCargo";
            this.ddlSumarizaConCargo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlSumarizaConCargo, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiCarTiposCargos);
           
            this.PersistirDatosRangosGrilla();
            this.MiCarTiposCargos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCarTiposCargos.UsuarioAlta.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = CargosF.TiposCargosAgregar(this.MiCarTiposCargos);                    
                    break;
                case Gestion.Modificar:
                    this.MiCarTiposCargos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    guardo = CargosF.TiposCargosModificar(this.MiCarTiposCargos);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCarTiposCargos.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiCarTiposCargos.CodigoMensaje, true, this.MiCarTiposCargos.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.TipoCargoModificarDatosCancelar != null)
                this.TipoCargoModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.TipoCargoModificarDatosAceptar != null)
                this.TipoCargoModificarDatosAceptar(null, this.MiCarTiposCargos);
        }

        protected void ddlTipoCargoProceso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlTipoCargoProceso.SelectedValue))
            {
                this.MiCarTiposCargos.TipoCargoProceso.IdTipoCargoProceso = Convert.ToInt32(this.ddlTipoCargoProceso.SelectedValue);
                rfvPorcentaje.Enabled = false;

                if (this.MiCarTiposCargos.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.Bonificacion)
                {
                    this.lblCategorias.Visible = false;
                    this.ddlCategorias.Visible = false;
                    this.btnAgregar.Visible = false;
                    this.btnAgregarRangos.Visible = false;
                    this.txtPorcentaje.Enabled = true;

                }
                else if (this.MiCarTiposCargos.TipoCargoProceso.IdTipoCargoProceso == (int)EnumTiposCargosProcesos.AdministrablePorcentaje)
                {
                    txtImporte.Enabled = false;
                    rfvImporte.Enabled = false;
                    this.lblCategorias.Visible = true;
                    this.ddlCategorias.Visible = true;
                    this.btnAgregar.Visible = true;
                    this.btnAgregarRangos.Visible = true;
                    this.txtPorcentaje.Enabled = true;
                    txtImporte.Text = "$0.00";
                    rfvPorcentaje.Enabled = true;
                }
                else
                {
                    txtPorcentaje.Text = "0";
                    txtImporte.Enabled = true;
                    rfvPorcentaje.Enabled = false;
                    rfvImporte.Enabled = true;
                    this.lblCategorias.Visible = true;
                    this.ddlCategorias.Visible = true;
                    this.btnAgregar.Visible = true;
                    this.btnAgregarRangos.Visible = true;
                    this.txtPorcentaje.Enabled = false;
                }
            }

        }

        protected void ddlTiposOperaciones_SelectedIndexChanged(object sender, EventArgs e)
        {
            MiCarTiposCargos.TipoOperacion.IdTipoOperacion = ddlTiposOperaciones.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTiposOperaciones.SelectedValue);
            this.ctrCamposValores.IniciarControl(this.MiCarTiposCargos, this.MiCarTiposCargos.TipoOperacion, this.GestionControl);
            
        }

            #region Tipos Cargos Categorias
            private void PersistirDatosGrilla()
        {
            if (this.MiCarTiposCargos.TiposCargosCategorias.Count > 0)
            {
                foreach (GridViewRow fila in this.gvDatos.Rows)
                {
                    decimal importe = ((Evol.Controls.CurrencyTextBox)fila.FindControl("txtImporte")).Decimal;
                    DateTime fechaAltaGrilla;
                    DateTime fechaVigenciaDesde;

                    TextBox txtFechaAltaGrilla = (TextBox)fila.FindControl("txtfechaAltaGrilla");
                    DateTime.TryParse(txtFechaAltaGrilla.Text, out fechaAltaGrilla);
                    TextBox txtFechaVigenciaDesde = (TextBox)fila.FindControl("txtFechaVigenciaDesde");
                    DateTime.TryParse(txtFechaVigenciaDesde.Text, out fechaVigenciaDesde);
                    DropDownList ddlEstados = (DropDownList)fila.FindControl("ddlEstados");

                    this.MiCarTiposCargos.TiposCargosCategorias[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiCarTiposCargos.TiposCargosCategorias[fila.RowIndex], GestionControl);

                    if (this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].Importe != importe)
                    {
                        this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex], this.GestionControl);
                    }
                    this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].Importe = importe;
                    if (this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].FechaAlta != fechaAltaGrilla)
                    {
                        this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex], this.GestionControl);
                    }
                    this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].FechaAlta = fechaAltaGrilla;
                    if (this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].FechaVigenciaDesde != fechaVigenciaDesde)
                    {
                        this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex], this.GestionControl);
                    }
                    this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].FechaVigenciaDesde = fechaVigenciaDesde;

                    if (this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].Estado.IdEstado != Convert.ToInt32(ddlEstados.SelectedValue))
                    {
                        this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].Estado.IdEstado = ddlEstados.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlEstados.SelectedValue);
                        this.MiCarTiposCargos.TiposCargosCategorias[fila.DataItemIndex].Estado.Descripcion = ddlEstados.SelectedValue == string.Empty ? string.Empty : ddlEstados.SelectedItem.Text;
                    }

                }
            }
        }
        
        protected void CargarCategorias()
        {
            this.ddlCategorias.DataSource = CargosF.TiposCargosObtenerCategoriasSinAsociar(this.MiCarTiposCargos);
            this.ddlCategorias.DataValueField = "IdCategoria";
            this.ddlCategorias.DataTextField = "Categoria";
            this.ddlCategorias.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCategorias, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;
            int indice = Convert.ToInt32(e.CommandArgument);
            
            this.MiIndiceDetalleModificar = Convert.ToInt32(((GridView)sender).DataKeys[indice].Value.ToString());
            if (e.CommandName == "Baja")
            {
                if (this.MiEstadoBaja == null)
                {
                    TGEEstados estadoBaja = new TGEEstados();
                    estadoBaja.IdEstado = (int)Estados.Baja;
                    this.MiEstadoBaja = TGEGeneralesF.TGEEstadosObtener(estadoBaja);
                }
                this.MiCarTiposCargos.TiposCargosCategorias.ElementAt(this.MiIndiceDetalleModificar).Estado = this.MiEstadoBaja;
                //MODIFICO estado coleccion para luego saber a que item actualizarle el estado
                this.MiCarTiposCargos.TiposCargosCategorias.ElementAt(this.MiIndiceDetalleModificar).EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiCarTiposCargos.TiposCargosCategorias.ElementAt(this.MiIndiceDetalleModificar), Gestion.Modificar);
                //this.MiCarTiposCargos.TiposCargosCategorias.ElementAt(this.MiIndiceDetalleModificar).Importe = 0;
                AyudaProgramacion.CargarGrillaListas<CarTiposCargosCategorias>(this.MiCarTiposCargos.TiposCargosCategorias, false, this.gvDatos, true);
                this.pnlCategorias.Update();
            }
           
            //if ((e.CommandName == "Sort"))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            //CarTiposCargos tipoCargo = this.MisTiposCargos[indiceColeccion];
            //string parametros = string.Format("?IdCargo={0}", tipoCargo.IdTipoCargo);
            //if (e.CommandName == Gestion.Modificar.ToString())
            //{
            //    string url = string.Concat("~/Modulos/Cargos/TiposCargosModificar.aspx", parametros);
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            //}
            //else if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    string url = string.Concat("~/Modulos/Cargos/TiposCargosConsultar.aspx", parametros);
            //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros(url), true);
            //}
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CarTiposCargosCategorias carTiposCargosCategorias =(CarTiposCargosCategorias)e.Row.DataItem;
             
                Evol.Controls.CurrencyTextBox importe = (Evol.Controls.CurrencyTextBox)e.Row.FindControl("txtImporte");
                TextBox fechaAlta = (TextBox)e.Row.FindControl("txtFechaAltaGrilla");
                TextBox fechaVigenciaDesde = (TextBox)e.Row.FindControl("txtFechaVigenciaDesde");
                DropDownList ddlEstado = (DropDownList)e.Row.FindControl("ddlEstados");

                ddlEstado.DataSource = this.MisEstados;
                ddlEstado.DataValueField = "IdEstado";
                ddlEstado.DataTextField = "Descripcion";
                ddlEstado.DataBind();
                if (carTiposCargosCategorias.Estado.IdEstado >= 0)
                {
                    ddlEstado.SelectedValue = carTiposCargosCategorias.Estado.IdEstado.ToString();
                }
                else
                {
                    ddlEstado.SelectedValue = Estados.Activo.ToString();
                }

                switch (GestionControl)
                {
                    case Gestion.Agregar:
                     
                        ddlEstado.Enabled = true;
                        break;
                    case Gestion.Modificar:
                    
                        if (carTiposCargosCategorias.Estado.IdEstado == (int)Estados.Baja)
                        {
                            fechaVigenciaDesde.Enabled = false;
                            importe.Enabled = false;
                           
                        }
                        ddlEstado.Enabled = true;
                        break;
                    case Gestion.Consultar:
                        importe.Enabled = false;
                        ddlEstado.Enabled = false;

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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiCarTiposCargos.TiposCargosCategorias.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosCategorias parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosCategorias>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CarTiposCargosCategorias>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MiCarTiposCargos.TiposCargosCategorias;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MisTiposCargos = this.OrdenarGrillaDatos<CarTiposCargos>(this.MisTiposCargos, e);
            //this.gvDatos.DataSource = this.MisTiposCargos;
            //this.gvDatos.DataBind();
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            CarTiposCargosCategorias item = new CarTiposCargosCategorias();
            item.Categoria.IdCategoria = Convert.ToInt32(this.ddlCategorias.SelectedValue);
            item.Categoria = AfiliadosF.CategoriaObtenerDatosCompletos(item.Categoria);
            item.IdTipoCargo = this.MiCarTiposCargos.IdTipoCargo;
            item.FechaAlta = DateTime.Now;
            item.FechaVigenciaDesde = DateTime.Now;
            //pongo una descripcion ficticia en el estado, luego se levanta la original al cargar la pagina
            item.Estado.Descripcion = "Activo";
            
            //Modifico el estado para darlo de alta en la LN
            item.EstadoColeccion = EstadoColecciones.Agregado;
            //cargo el item agregado en la grilla para borrarlo del selector
            ListItem aBorrar = this.ddlCategorias.Items.FindByValue(this.ddlCategorias.SelectedValue);
            if (aBorrar != null)
            {
                this.ddlCategorias.Items.Remove(aBorrar);
            }
            //cargo el item en la grilla
            this.MiCarTiposCargos.TiposCargosCategorias.Add(item);
            item.IndiceColeccion = this.MiCarTiposCargos.TiposCargosCategorias.IndexOf(item);
            AyudaProgramacion.CargarGrillaListas<CarTiposCargosCategorias>(this.MiCarTiposCargos.TiposCargosCategorias, false, this.gvDatos, true);
            //actualizo la pagina
            this.pnlCategorias.Update();
            
        }

        #endregion




        #region Tipos Cargos Rangos
        private void PersistirDatosRangosGrilla()
        {
            if (this.MiCarTiposCargos.TiposCargosRangos.Count > 0)
            {
                CarTiposCargosRangos item;
                bool modifica;
                foreach (GridViewRow fila in this.gvRangos.Rows)
                {
                    modifica = false;
                    item = this.MiCarTiposCargos.TiposCargosRangos[fila.DataItemIndex];
                    DropDownList ddlTiposRangos = (DropDownList)fila.FindControl("ddlTiposRangos");
                    decimal desde = ((CurrencyTextBox)fila.FindControl("txtDesde")).Decimal;
                    decimal hasta = ((CurrencyTextBox)fila.FindControl("txtHasta")).Decimal;
                    decimal importe = ((CurrencyTextBox)fila.FindControl("txtImporte")).Decimal;
                    string sfechaVigenciaDesde = ((TextBox)fila.FindControl("txtFechaVigenciaDesde")).Text;
                    DropDownList ddlEstados = (DropDownList)fila.FindControl("ddlEstadosRangos");

                    if (item.TipoRango.IdTipoRango.ToString() != ddlTiposRangos.SelectedValue)
                    {
                        item.TipoRango.IdTipoRango = ddlTiposRangos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlTiposRangos.SelectedValue);
                        item.TipoRango.Descripcion = ddlTiposRangos.SelectedValue == string.Empty ? string.Empty : ddlTiposRangos.SelectedItem.Text;
                        modifica = true;
                    }
                    if (item.Desde != desde)
                    {
                        item.Desde = desde;
                        modifica = true;
                    }
                    if (item.Hasta != hasta)
                    {
                        item.Hasta = hasta;
                        modifica = true;
                    }
                    if (item.Importe != importe)
                    {
                        item.Importe = importe;
                        modifica = true;
                    }
                    if (item.FechaVigenciaDesde.ToShortDateString() != sfechaVigenciaDesde)
                    {
                        item.FechaVigenciaDesde = Convert.ToDateTime(sfechaVigenciaDesde);
                        modifica = true;
                    }
                    if (item.Estado.IdEstado.ToString() != ddlEstados.SelectedValue)
                    {
                        item.Estado.IdEstado = ddlEstados.SelectedValue == string.Empty ? 0 : Convert.ToInt32(ddlEstados.SelectedValue);
                        item.Estado.Descripcion = ddlEstados.SelectedValue == string.Empty ? string.Empty : ddlEstados.SelectedItem.Text;
                        modifica = true;
                    }



                    if (modifica)
                        item.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(item, this.GestionControl);
                }
            }
        }
        protected void gvRangos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CarTiposCargosRangos rango = (CarTiposCargosRangos)e.Row.DataItem;
                DropDownList ddlTiposRangos = (DropDownList)e.Row.FindControl("ddlTiposRangos");
                CurrencyTextBox txtDesde = (CurrencyTextBox)e.Row.FindControl("txtDesde");
                CurrencyTextBox txtHasta = (CurrencyTextBox)e.Row.FindControl("txtHasta");
                CurrencyTextBox txtImporte = (CurrencyTextBox)e.Row.FindControl("txtImporte");
                TextBox txtFechaVigenciaDesde = (TextBox)e.Row.FindControl("txtFechaVigenciaDesde");
                DropDownList ddlEstadoRango = (DropDownList)e.Row.FindControl("ddlEstadosRangos");

                ddlTiposRangos.DataSource = this.MisCarTiposCargosRangos;
                ddlTiposRangos.DataValueField = "IdListaValorSistemaDetalle";
                ddlTiposRangos.DataTextField = "Descripcion";
                ddlTiposRangos.DataBind();
                ddlTiposRangos.Items.Insert(0, new ListItem(this.ObtenerMensajeSistema("SeleccioneOpcion"), ""));
                ListItem item;
                if (rango.TipoRango.IdTipoRango > 0)
                {
                    item = ddlTiposRangos.Items.FindByValue(rango.TipoRango.IdTipoRango.ToString());
                    if (item == null)
                        ddlTiposRangos.Items.Add(new ListItem(rango.TipoRango.Descripcion, rango.TipoRango.IdTipoRango.ToString()));
                    ddlTiposRangos.SelectedValue = rango.TipoRango.IdTipoRango.ToString();
                }

                ddlEstadoRango.DataSource = this.MisEstados;
                ddlEstadoRango.DataValueField = "IdEstado";
                ddlEstadoRango.DataTextField = "Descripcion";
                ddlEstadoRango.DataBind();
                if (rango.Estado.IdEstado >= 0)
                {
                    ddlEstadoRango.SelectedValue = rango.Estado.IdEstado.ToString();
                }
                else
                {
                    ddlEstadoRango.SelectedValue = Estados.Activo.ToString();
                }


                switch (GestionControl)
                {
                    case Gestion.Agregar:
                        ddlTiposRangos.Enabled = true;
                        txtDesde.Enabled = true;
                        txtHasta.Enabled = true;
                        txtImporte.Enabled = true;
                        txtFechaVigenciaDesde.Enabled = true;
                        ddlEstadoRango.Enabled = true;
                        break;
                    case Gestion.Modificar:
                        ddlTiposRangos.Enabled = true;
                        txtDesde.Enabled = true;
                        txtHasta.Enabled = true;
                        txtImporte.Enabled = true;
                        txtFechaVigenciaDesde.Enabled = true;
                        ddlEstadoRango.Enabled = true;
                        break;
                    case Gestion.Consultar:
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
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiCarTiposCargos.TiposCargosRangos.Count);
                e.Row.Cells.Add(tableCell);
            }
        }
        protected void gvRangos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;
            int indice = Convert.ToInt32(e.CommandArgument);

            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[indice].Value.ToString());
            if (e.CommandName == "Baja")
            {
                this.PersistirDatosRangosGrilla();
                this.MiCarTiposCargos.TiposCargosRangos.RemoveAt(indiceColeccion);
                this.MiCarTiposCargos.TiposCargosRangos = AyudaProgramacion.AcomodarIndices<CarTiposCargosRangos>(this.MiCarTiposCargos.TiposCargosRangos);
                AyudaProgramacion.CargarGrillaListas<CarTiposCargosRangos>(this.MiCarTiposCargos.TiposCargosRangos, false, this.gvRangos, true);
                ScriptManager.RegisterStartupScript(this.upRangos, this.upRangos.GetType(), "scriptInitRangos", "InitRangos();", true);
                this.upRangos.Update();
            }
        }
        protected void gvRangos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CarTiposCargosRangos parametros = this.BusquedaParametrosObtenerValor<CarTiposCargosRangos>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CarTiposCargosRangos>(parametros);

            this.gvRangos.PageIndex = e.NewPageIndex;
            this.gvRangos.DataSource = this.MiCarTiposCargos.TiposCargosRangos;
            this.gvRangos.DataBind();
            this.upRangos.Update();
        }
        protected void gvRangos_Sorting(object sender, GridViewSortEventArgs e)
        {
            //this.MisTiposCargos = this.OrdenarGrillaDatos<CarTiposCargos>(this.MisTiposCargos, e);
            //this.gvDatos.DataSource = this.MisTiposCargos;
            //this.gvDatos.DataBind();
        }
        protected void btnAgregarRangos_Click(object sender, EventArgs e)
        {
            this.PersistirDatosRangosGrilla();
            CarTiposCargosRangos item = new CarTiposCargosRangos();
            item.EstadoColeccion = EstadoColecciones.Agregado;
            item.Estado.IdEstado = (int)Estados.Activo;
            item.FechaVigenciaDesde = DateTime.Now;
            this.MiCarTiposCargos.TiposCargosRangos.Add(item);
            item.IndiceColeccion = this.MiCarTiposCargos.TiposCargosRangos.IndexOf(item);
            AyudaProgramacion.CargarGrillaListas<CarTiposCargosRangos>(this.MiCarTiposCargos.TiposCargosRangos, false, this.gvRangos, true);
            ScriptManager.RegisterStartupScript(this.upRangos, this.upRangos.GetType(), "scriptInitRangos", "InitRangos();", true);
            this.upRangos.Update();
        }
        #endregion
    }
}