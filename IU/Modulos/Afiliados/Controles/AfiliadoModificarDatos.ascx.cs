using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Tesorerias.Entidades;

namespace IU.Modulos.Afiliados.Controles
{
    public partial class AfiliadoModificarDatos : ControlesSeguros
    {
        private AfiAfiliados MiAfiliado
        {
            get { return (AfiAfiliados)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiAfiliado"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiAfiliado"] = value; }
        }

        private List<TGEListasValoresDetalles> TiposPersonas
        {
            get { return (List<TGEListasValoresDetalles>)Session[this.MiSessionPagina + "AfiliadosTiposPersonasTiposPersonas"]; }
            set { Session[this.MiSessionPagina + "AfiliadosTiposPersonasTiposPersonas"] = value; }
        }

        private int MiIdAfiliadoImportarCliente
        {
            get { return (int)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIdAfiliadoImportarCliente"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIdAfiliadoImportarCliente"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        bool AceptarContinuar = false;

        public delegate void AfiliadoDatosAceptarEventHandler(object sender, AfiAfiliados e);
        public event AfiliadoDatosAceptarEventHandler AfiliadosModificarDatosAceptar;
        public delegate void AfiliadoDatosCancelarEventHandler();
        public event AfiliadoDatosCancelarEventHandler AfiliadosModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrDomicilios.AfiliadosModificarDatosAceptar += new AfiliadoModificarDatosDomicilioPopUp.AfiliadoModificarDatosDomicilioEventHandler(ctrDomicilios_AfiliadosModificarDatosAceptar);
            this.ctrTelefonos.AfiliadosModificarDatosAceptar += new AfiliadoModificarDatosTelefonoPopUp.AfiliadoModificarDatosTelefonoEventHandler(ctrTelefonos_AfiliadosModificarDatosAceptar);
            this.ctrBuscarPadronTXT.AfiliadosBuscarSeleccionar += CtrBuscarPadronTXT_AfiliadosBuscarSeleccionar;
            //this.ctrAfiliados.AfiliadosBuscarSeleccionar += new AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            //this.ctrAfiFamiliares.AfiliadosBuscarSeleccionar += new AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiFamiliares_AfiliadosBuscarSeleccionar);
            base.PageLoadEvent(sender, e);
            if (this.IsPostBack)
            {
                if (this.MiAfiliado == null)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlSesion.aspx"), true);
                }
            }
        }

        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.MiAfiliado.ConfirmarAccion)
            {
                this.MiAfiliado.ConfirmarBajaFamiliares = true;
                this.btnAceptar_Click(null, EventArgs.Empty);
            }
            else
            {
                if (this.AfiliadosModificarDatosAceptar != null)
                    this.AfiliadosModificarDatosAceptar(null, this.MiAfiliado);
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de un Afiliado
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(AfiAfiliados pAfiliado, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiAfiliado = pAfiliado;
            this.AceptarContinuar = false;
            this.btnAceptarContinuar.Visible = true;
            this.btnAceptar.Visible = true;
            this.CargarCombos();

            switch (pAfiliado.AfiliadoTipo.IdAfiliadoTipo)
            {
                case (int)EnumAfiliadosTipos.Socios:
                    this.MostrarCampos(true);
                    break;
                case (int)EnumAfiliadosTipos.Familiares:
                    this.btnAceptarContinuar.Visible = true;
                    this.MostrarCampos(false);
                    this.ddlTipoPersona.Enabled = false;
                    this.dvSexo.Visible = true;
                    //this.lblParentesco.Visible = true;
                    //this.ddlParentesco.Visible = true;
                    break;
                case (int)EnumAfiliadosTipos.Apoderados:
                    this.MostrarCampos(false);
                    this.dvParentesco.Visible = false;
                    this.lblCategoria.Visible = false;
                    this.ddlCategoria.Visible = false;
                    //this.lblSexo.Visible = false;
                    //this.ddlSexo.Visible = false;
                    //this.rfvSexo.Enabled = false;
                    this.lblEstadoCivil.Visible = false;
                    this.ddlEstadoCivil.Visible = false;
                    this.dvTipoApoderado.Visible = true;
                    this.rfvTipoApoderado.Enabled = true;
                    this.ddlTipoApoderado.DataSource = AfiliadosF.TiposApoderadosObtenerLista();
                    this.ddlTipoApoderado.DataValueField = "IdTipoApoderado";
                    this.ddlTipoApoderado.DataTextField = "TipoApoderado";
                    this.ddlTipoApoderado.DataBind();
                    break;
                default:
                    break;
            }
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.ctrComentarios.IniciarControl(new AfiAfiliados(), this.GestionControl);
                    this.ctrArchivos.IniciarControl(new AfiAfiliados(), this.GestionControl);
                    this.ctrAuditoria.IniciarControl(new AfiAfiliados());
                    this.ctrAuditoriaEstados.IniciarControl(new AfiAfiliados(), "Estado -> Descripcion");
                    this.txtNumeroSocio.Enabled = true;
                    TGEParametrosValores paramValor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.EstadoAfiliadoPorDefecto);
                    ListItem item = this.ddlEstados.Items.FindByValue(paramValor.ParametroValor);
                    if (item != null)
                        this.ddlEstados.SelectedValue = item.Value;
                    else
                        this.ddlEstados.SelectedValue = ((int)EstadosAfiliados.Normal).ToString();

                    this.txtFechaIngreso.Text = DateTime.Now.ToShortDateString();

                    this.MiAfiliado.Categoria.IdCategoria = Convert.ToInt32(this.ddlCategoria.SelectedValue);
                    this.MiAfiliado.Filial.IdFilial = Convert.ToInt32(this.ddlFilial.SelectedValue);
                    if (this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                    {
                        this.ddlTipoSocio.Enabled = false;
                        this.ddlTipoSocio.SelectedValue = 1.ToString();
                        this.dvFechaFallecimiento.Visible = false;
                        this.dvTipoApoderado.Visible = false;
                        this.dvFechaBaja.Visible = false;
                        AfiAfiliados afi = new AfiAfiliados();
                        afi.Categoria.IdCategoria = MiAfiliado.Categoria.IdCategoria;
                        afi.Filial.IdFilial = MiAfiliado.Filial.IdFilial;
                        if (!AfiliadosF.AfiliadosObtenerProximoNumeroSocio(afi))
                        {
                            this.MostrarMensaje(afi.CodigoMensaje, true, afi.CodigoMensajeArgs);
                        }
                        else
                            this.MiAfiliado.NumeroSocio = afi.NumeroSocio;
                        this.ctrCamposValores.IniciarControl(this.MiAfiliado, new Objeto(), this.GestionControl);
                    }
                    if (this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Familiares)
                    {
                        this.dvNumeroSocio.Visible = false;
                        this.dvFechaFallecimiento.Visible = false;
                        this.dvTipoApoderado.Visible = false;
                        this.dvFechaBaja.Visible = false;
                        AfiFamiliares familiar = new AfiFamiliares();
                        familiar.IdAfiliado = MiAfiliado.IdAfiliado;
                        this.ctrCamposValores.IniciarControl(familiar, new Objeto(), this.GestionControl);
                    }
                    this.txtNumeroSocio.Text = this.MiAfiliado.NumeroSocio;
                    //this.MiAfiliado.Estado.IdEstado = (int)EstadosAfiliados.Normal;
                    //this.MiAfiliado.Estado = TGEGeneralesF.TGEEstadosObtener(this.MiAfiliado.Estado);
                    //this.imgImprimir.Visible = false;
                    this.btnImprimir.Visible = false;
                    this.btnAgregarFamiliar.Visible = false;
                    this.btnAgregarApoderado.Visible = false;
                    this.btnAceptarContinuar.Visible = false;
                    this.ctrArchivos.IniciarControl(this.MiAfiliado, this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.txtNumeroSocio.Enabled = true;
                    this.MiAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(pAfiliado);
                    if (TiposPersonas.FirstOrDefault(x => x.IdListaValorDetalle == this.MiAfiliado.IdTipoPersona).CodigoValor == ((char)EnumTiposPersonas.Fisica).ToString())
                    {
                        this.dvSexo.Visible = false;
                        this.rfvSexo.Enabled = false;
                    }
                    if (this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                    {
                        this.ddlTipoSocio.Enabled = false;
                        if (this.MiAfiliado.IdAfiliadoRef > 0)
                        {
                            AfiAfiliados afi = AfiliadosF.AfiliadosObtenerDatosSocioTitular(this.MiAfiliado);
                            afi.CodigoMensajeArgs.Add(afi.ApellidoNombre);
                            string mensaje = this.ObtenerMensajeSistema("ConfirmarDesvincularCuenta", afi.CodigoMensajeArgs);
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            this.btnDesvincularSocio.Attributes.Add("OnClick", funcion);
                            this.btnDesvincularSocio.Visible = true;
                        }
                    }
                    this.MapearObjetoAControles(this.MiAfiliado);
                    this.HabilitarFechaBaja();
                    if (this.MiAfiliado.IdAfiliadoRef == 0)
                    {
                        this.btnAgregarFamiliar.Visible = this.ValidarPermiso("FamiliaresAgregar.aspx");
                        this.btnAgregarApoderado.Visible = this.ValidarPermiso("ApoderadosAgregar.aspx");
                    }
                    else
                    {
                        this.tpFamiliares.Visible = false;
                        this.tpApoderados.Visible = false;
                    }
                    break;
                case Gestion.Consultar:
                    this.MiAfiliado = AfiliadosF.AfiliadosObtenerDatosCompletos(pAfiliado);
                    this.MapearObjetoAControles(this.MiAfiliado);
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnAceptar.Visible = false;
                    this.btnAceptarContinuar.Visible = false;
                    this.btnAgregarFamiliar.Visible = false;
                    this.btnAgregarApoderado.Visible = false;
                    this.btnAgregarDomicilio.Visible = false;
                    this.btnAgregarTelefono.Visible = false;
                    break;
                default:
                    break;
            }
            //if (this.txtFechaIngreso.Text != string.Empty)
            //{
            //    this.cvFechaIngreso.Enabled = true;
            //    this.cvFechaIngreso.Type = ValidationDataType.Date;
            //    this.cvFechaIngreso.ValueToCompare = this.txtFechaIngreso.Text;
            //    this.cvFechaIngreso.ErrorMessage = this.ObtenerMensajeSistema("ValidarFechaIngreso");
            //}
            //else
            //    this.cvFechaIngreso.Enabled = false;
        }
        private void MostrarCampos(bool pEstado)
        {
            this.lblNumeroSocio.Enabled = pEstado;
            this.txtNumeroSocio.Enabled = pEstado;
            this.dvSexo.Visible = pEstado;
            this.dvFechaRetiro.Visible = pEstado;
            //this.lblFechaRetiro.Visible = pEstado;
            //this.txtFechaRetiro.Visible = pEstado;
            //this.imgFechaRetiro.Visible = pEstado;
            //this.lblMatriculaIAF.Visible = pEstado;
            //this.txtMatriculaIAF.Visible = pEstado;
            this.dvMatriculaIAF.Visible = pEstado;
            //this.lblGrado.Visible = pEstado;
            //this.ddlGrado.Visible = pEstado;
            this.dvGrado.Visible = pEstado;
            this.tpFamiliares.Visible = pEstado;
            //this.tpEstados.Visible = pEstado;
            this.tpApoderados.Visible = pEstado;
            this.dvParentesco.Visible = !pEstado;
            this.pnlAlertasTipos.Visible = pEstado;
            //this.lblFilial.Visible = pEstado;
            //this.ddlFilial.Visible = pEstado;
            this.dvFilial.Visible = pEstado;
            this.rfvFilial.Enabled = pEstado;
            //this.lblFechaSupervivencia.Visible = pEstado;
            //this.txtFechaSupervivencia.Visible = pEstado;
            this.dvFechaSupervivencia.Visible = pEstado;
            //this.lblZonaGrupo.Visible = pEstado;
            //this.ddlZonasGrupos.Visible = pEstado;
            this.dvZonasGrupos.Visible = pEstado;
            //this.lblCondicionesFiscales.Visible = pEstado;
            //this.ddlCondicionFiscal.Visible = pEstado;
            this.dvCondicionesFiscales.Visible = pEstado;
        }
        /// <summary>
        /// Mapea la Entidad SolicitudesMateriales a los controles de Pantalla
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearObjetoAControles(AfiAfiliados pAfiliado)
        {
            this.ddlEstados.SelectedValue = pAfiliado.Estado.IdEstado.ToString();
            this.txtApellido.Text = pAfiliado.Apellido;
            this.txtNombre.Text = pAfiliado.Nombre;
            this.ddlTipoDocumento.SelectedValue = pAfiliado.TipoDocumento.IdTipoDocumento.HasValue ? pAfiliado.TipoDocumento.IdTipoDocumento.Value.ToString() : string.Empty;
            this.txtNumeroDocumento.Text = pAfiliado.NumeroDocumento.ToString();
            this.txtCUIL.Text = pAfiliado.CUILFormateado;
            this.txtNumeroSocio.Text = pAfiliado.NumeroSocio;
            this.ddlSexo.SelectedValue = pAfiliado.Sexo.IdSexo.HasValue ? pAfiliado.Sexo.IdSexo.ToString() : string.Empty;
            this.txtFechaNacimiento.Text = AyudaProgramacion.MostrarFechaPantalla(pAfiliado.FechaNacimiento);
            this.txtFechaIngreso.Text = AyudaProgramacion.MostrarFechaPantalla(pAfiliado.FechaIngreso);
            this.ddlEstadoCivil.SelectedValue = pAfiliado.EstadoCivil.IdEstadoCivil.HasValue ? pAfiliado.EstadoCivil.IdEstadoCivil.Value.ToString() : string.Empty;
            this.txtMatriculaIAF.Text = pAfiliado.MatriculaIAF.ToString();
            this.txtCorreoElectronico.Text = pAfiliado.CorreoElectronico;
            this.ddlTipoSocio.SelectedValue = pAfiliado.AfiliadoTipo.IdAfiliadoTipo.ToString();
            this.ddlTipoPersona.SelectedValue = pAfiliado.IdTipoPersona.ToString();
            this.txtFechaRetiro.Text = AyudaProgramacion.MostrarFechaPantalla(pAfiliado.FechaRetiro);
            this.txtFechaFallecimiento.Text = AyudaProgramacion.MostrarFechaPantalla(pAfiliado.FechaFallecimiento);
            this.ddlParentesco.SelectedValue = pAfiliado.Parentesco.IdParentesco.HasValue ? pAfiliado.Parentesco.IdParentesco.Value.ToString() : string.Empty;
            this.txtFechaBaja.Text = AyudaProgramacion.MostrarFechaPantalla(pAfiliado.FechaBaja);
            this.txtFechaSupervivencia.Text = AyudaProgramacion.MostrarFechaPantalla(pAfiliado.FechaSupervivencia);

            ListItem item;
            item = ddlGrupoSanguineo.Items.FindByValue(pAfiliado.GrupoSanguieno.IdGrupoSanguieno.ToString());
            if (item == null && pAfiliado.GrupoSanguieno.IdGrupoSanguieno > 0)
                ddlGrupoSanguineo.Items.Add(new ListItem(pAfiliado.GrupoSanguieno.GrupoSanguineo, pAfiliado.GrupoSanguieno.IdGrupoSanguieno.ToString()));
            this.ddlGrupoSanguineo.SelectedValue = pAfiliado.GrupoSanguieno.IdGrupoSanguieno > 0 ? pAfiliado.GrupoSanguieno.IdGrupoSanguieno.ToString() : string.Empty;

            if (pAfiliado.Categoria.IdCategoria.HasValue)
            {
                item = this.ddlCategoria.Items.FindByValue(pAfiliado.Categoria.IdCategoria.ToString());
                if (item == null && pAfiliado.Categoria.IdCategoria > 0)
                    this.ddlCategoria.Items.Add(new ListItem(pAfiliado.Categoria.Categoria, pAfiliado.Categoria.IdCategoria.Value.ToString()));
                this.ddlCategoria.SelectedValue = pAfiliado.Categoria.IdCategoria.Value.ToString();
            }

            if (pAfiliado.Grado.IdGrado.HasValue)
            {
                item = this.ddlGrado.Items.FindByValue(pAfiliado.Grado.IdGrado.Value.ToString());
                if (item == null && pAfiliado.Grado.IdGrado > 0)
                    this.ddlGrado.Items.Add(new ListItem(pAfiliado.Grado.Grado, pAfiliado.Grado.IdGrado.Value.ToString()));
                this.ddlGrado.SelectedValue = pAfiliado.Grado.IdGrado.Value.ToString();
            }

            ListItem filial = this.ddlFilial.Items.FindByValue(pAfiliado.Filial.IdFilial.ToString());
            if (filial == null && pAfiliado.Filial.IdFilial > 0)
                this.ddlFilial.Items.Add(new ListItem(pAfiliado.Filial.Filial, pAfiliado.Filial.IdFilial.ToString()));
            this.ddlFilial.SelectedValue = pAfiliado.Filial.IdFilial == 0 ? string.Empty : pAfiliado.Filial.IdFilial.ToString();

            if (pAfiliado.Sexo.IdSexo.HasValue)
            {
                ListItem sexo = this.ddlSexo.Items.FindByValue(pAfiliado.Sexo.IdSexo.Value.ToString());
                if (sexo == null && pAfiliado.Sexo.IdSexo > 0)
                    this.ddlSexo.Items.Add(new ListItem(pAfiliado.Sexo.Sexo, pAfiliado.Sexo.IdSexo.Value.ToString()));
                this.ddlSexo.SelectedValue = pAfiliado.Sexo.IdSexo == 0 ? string.Empty : pAfiliado.Sexo.IdSexo.Value.ToString();
            }

            ListItem zonaGrupo = this.ddlZonasGrupos.Items.FindByValue(pAfiliado.ZonaGrupo.IdZonaGrupo.ToString());
            if (zonaGrupo == null && pAfiliado.ZonaGrupo.IdZonaGrupo > 0)
                this.ddlZonasGrupos.Items.Add(new ListItem(pAfiliado.ZonaGrupo.Descripcion, pAfiliado.ZonaGrupo.IdZonaGrupo.ToString()));
            this.ddlZonasGrupos.SelectedValue = pAfiliado.ZonaGrupo.IdZonaGrupo == 0 ? string.Empty : pAfiliado.ZonaGrupo.IdZonaGrupo.ToString();

            ListItem condicionFiscal = this.ddlCondicionFiscal.Items.FindByValue(pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString());
            if (condicionFiscal == null && pAfiliado.CondicionFiscal.IdCondicionFiscal > 0)
                this.ddlCondicionFiscal.Items.Add(new ListItem(pAfiliado.CondicionFiscal.Descripcion, pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString()));
            this.ddlCondicionFiscal.SelectedValue = pAfiliado.CondicionFiscal.IdCondicionFiscal == 0 ? string.Empty : pAfiliado.CondicionFiscal.IdCondicionFiscal.ToString();
            // FOTO
            // FIRMA
            if (pAfiliado.IdAfiliadoRef == 0)
            {
                AyudaProgramacion.CargarGrillaListas(pAfiliado.Familiares, true, this.gvDatos, true);
                AyudaProgramacion.CargarGrillaListas(pAfiliado.Apoderados, true, this.gvApoderado, true);
            }
            AyudaProgramacion.CargarGrillaListas(pAfiliado.Domicilios, true, this.gvDomicilios, true);
            AyudaProgramacion.CargarGrillaListas(pAfiliado.Telefonos, true, this.gvTelefonos, true);

            this.CargarAlertasTiposAfiliado(pAfiliado);

            this.ddlTipoPersona_SelectedIndexChanged(null, EventArgs.Empty);
            this.ctrComentarios.IniciarControl(pAfiliado, this.GestionControl);
            this.ctrArchivos.IniciarControl(pAfiliado, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pAfiliado);
            this.ctrAuditoriaEstados.IniciarControl(pAfiliado, "Estado -> Descripcion");

            if (pAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
            {
                this.ctrCamposValores.IniciarControl(pAfiliado, new Objeto(), this.GestionControl);
            }
            else if (pAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Familiares)
            {
                AfiFamiliares familiar = new AfiFamiliares();
                familiar.IdAfiliado = pAfiliado.IdAfiliado;
                familiar.Campos.AddRange(pAfiliado.Campos);
                this.ctrCamposValores.IniciarControl(familiar, new Objeto(), this.GestionControl);
            }
            else if (pAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Apoderados)
            {
                item = this.ddlTipoApoderado.Items.FindByValue(pAfiliado.TipoApoderado.IdTipoApoderado.ToString());
                if (item == null)
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoApoderado, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                else
                    this.ddlTipoApoderado.SelectedValue = pAfiliado.TipoApoderado.IdTipoApoderado.ToString();
            }
        }
        /// <summary>
        /// Mapea los controles de Pantalla con la Entidad SolicitudesMateriales.
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearControlesAObjeto(AfiAfiliados pAfiliado)
        {
            pAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pAfiliado.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pAfiliado.Apellido = this.txtApellido.Text;
            pAfiliado.Nombre = this.txtNombre.Text;
            pAfiliado.TipoDocumento.IdTipoDocumento = this.ddlTipoDocumento.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            pAfiliado.TipoDocumento.TipoDocumento = this.ddlTipoDocumento.SelectedValue == string.Empty ? string.Empty : this.ddlTipoDocumento.SelectedItem.Text;
            pAfiliado.NumeroDocumento = this.txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text.Trim());
            pAfiliado.CUIL = this.txtCUIL.Text == string.Empty ? 0 : Convert.ToInt64(this.txtCUIL.Text.Trim());
            pAfiliado.NumeroSocio = this.txtNumeroSocio.Text;
            pAfiliado.IdTipoPersona = Convert.ToInt32(this.ddlTipoPersona.SelectedValue);
            pAfiliado.Sexo.IdSexo = this.ddlSexo.SelectedValue == string.Empty ? 1 : Convert.ToInt32(this.ddlSexo.SelectedValue);
            pAfiliado.FechaNacimiento = this.txtFechaNacimiento.Text == string.Empty ? default(DateTime) : Convert.ToDateTime(this.txtFechaNacimiento.Text);
            pAfiliado.FechaIngreso = this.txtFechaIngreso.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaIngreso.Text);
            pAfiliado.EstadoCivil.IdEstadoCivil = this.ddlEstadoCivil.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlEstadoCivil.SelectedValue);
            pAfiliado.MatriculaIAF = this.txtMatriculaIAF.Text == string.Empty ? 0 : Convert.ToInt64(this.txtMatriculaIAF.Text);
            pAfiliado.CorreoElectronico = this.txtCorreoElectronico.Text;
            pAfiliado.Categoria.IdCategoria = this.ddlCategoria.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlCategoria.SelectedValue);
            pAfiliado.Categoria.Categoria = this.ddlCategoria.SelectedValue == string.Empty ? string.Empty : this.ddlCategoria.SelectedItem.Text;
            pAfiliado.GrupoSanguieno.IdGrupoSanguieno = this.ddlGrupoSanguineo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlGrupoSanguineo.SelectedValue);
            pAfiliado.Grado.IdGrado = this.ddlGrado.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlGrado.SelectedValue);
            pAfiliado.FechaBaja = this.txtFechaBaja.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaBaja.Text);
            pAfiliado.FechaRetiro = this.txtFechaRetiro.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaRetiro.Text);
            pAfiliado.FechaSupervivencia = this.txtFechaSupervivencia.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaSupervivencia.Text);
            pAfiliado.FechaFallecimiento = this.txtFechaFallecimiento.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaFallecimiento.Text);
            pAfiliado.Parentesco.IdParentesco = this.ddlParentesco.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlParentesco.SelectedValue);
            pAfiliado.Filial.IdFilial = this.ddlFilial.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFilial.SelectedValue);
            pAfiliado.Filial.Filial = this.ddlFilial.SelectedValue == string.Empty ? string.Empty : this.ddlFilial.SelectedItem.Text;
            pAfiliado.ZonaGrupo.IdZonaGrupo = this.ddlZonasGrupos.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlZonasGrupos.SelectedValue);
            pAfiliado.ZonaGrupo.Descripcion = this.ddlZonasGrupos.SelectedValue == string.Empty ? string.Empty : this.ddlZonasGrupos.SelectedItem.Text;
            pAfiliado.CondicionFiscal.IdCondicionFiscal = this.ddlCondicionFiscal.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCondicionFiscal.SelectedValue);
            pAfiliado.CondicionFiscal.Descripcion = this.ddlCondicionFiscal.SelectedValue == string.Empty ? string.Empty : this.ddlCondicionFiscal.SelectedItem.Text;
            pAfiliado.AfiliadoTipo.IdAfiliadoTipo = Convert.ToInt32(ddlTipoSocio.SelectedValue);

            //pAfiliado.ComprobanteExento = true;// this.chkComprobanteExento.Checked;
            // FOTO
            // FIRMA
            this.ObtenerAlertasTipos(pAfiliado);
            if (pAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Apoderados)
                pAfiliado.TipoApoderado.IdTipoApoderado = Convert.ToInt32(this.ddlTipoApoderado.SelectedValue);

            pAfiliado.Comentarios = ctrComentarios.ObtenerLista();
            pAfiliado.Archivos = ctrArchivos.ObtenerLista();
            pAfiliado.Campos = this.ctrCamposValores.ObtenerLista();
            pAfiliado.LoteCamposValores = this.ctrCamposValores.ObtenerListaCamposValores();
        }

        protected void btnTxtCuitBlur_Click(object sender, EventArgs e)
        {
            this.MapearControlesAObjeto(this.MiAfiliado);
            if (this.txtNumeroDocumento.Text.Trim().Length > 0)
            {
                this.txtApellido.Enabled = true;
                //MiAfiliado.CUIL = Convert.ToInt32(txtCuit.Text);
                if (!AfiliadosF.AfiliadosObtenerDatosAFIP(this.MiAfiliado))
                {
                    //this.txtCuit.Decimal = 0;
                    this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true);
                }
            }
            this.MapearObjetoAControles(this.MiAfiliado);
        }
        /// <summary>
        /// Llena los DropDownList de la Pantalla con los datos
        /// </summary>
        private void CargarCombos()
        {
            string parametro = "AfiAfiliados";
            List<TGEEstados> estados = TGEGeneralesF.TGEEstadosObtenerLista(parametro);
            if (estados.Exists(x => x.IdEstado == (int)EstadosAfiliados.Vigente))
                estados.Remove(estados.Find(x => x.IdEstado == (int)EstadosAfiliados.Vigente));
            this.ddlEstados.DataSource = AyudaProgramacion.AcomodarIndices<TGEEstados>(estados);
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            AfiAfiliados afiFiltro = new AfiAfiliados();
            afiFiltro.Estado.IdEstado = (int)Estados.Activo;
            this.ddlCategoria.DataSource = AfiliadosF.CategoriasObtenerListaActiva(afiFiltro);
            this.ddlCategoria.DataValueField = "IdCategoria";
            this.ddlCategoria.DataTextField = "Categoria";
            this.ddlCategoria.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlCategoria, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            AfiAfiliados AfiTipos = new AfiAfiliados();
            this.ddlTipoSocio.DataSource = AfiliadosF.AFiliadosObtenerTiposAfiliados(AfiTipos);
            this.ddlTipoSocio.DataValueField = "IdAfiliadoTipo";
            this.ddlTipoSocio.DataTextField = "AfiliadoTipo";
            this.ddlTipoSocio.DataBind();
            this.ddlTipoSocio.SelectedValue = "2";


            this.ddlEstadoCivil.DataSource = AfiliadosF.EstadosCivilesObtenerLista();
            this.ddlEstadoCivil.DataValueField = "IdEstadoCivil";
            this.ddlEstadoCivil.DataTextField = "EstadoCivil";
            this.ddlEstadoCivil.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstadoCivil, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlGrado.DataSource = AfiliadosF.GradosObtenerListaActiva();
            this.ddlGrado.DataValueField = "IdGrado";
            this.ddlGrado.DataTextField = "Grado";
            this.ddlGrado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlGrado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlGrupoSanguineo.DataSource = AfiliadosF.GruposSanguineosObtenerListar();
            this.ddlGrupoSanguineo.DataValueField = "IdGrupoSanguieno";
            this.ddlGrupoSanguineo.DataTextField = "GrupoSanguineo";
            this.ddlGrupoSanguineo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlGrupoSanguineo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlSexo.DataSource = AfiliadosF.SexoObtenerLista();
            this.ddlSexo.DataValueField = "IdSexo";
            this.ddlSexo.DataTextField = "Sexo";
            this.ddlSexo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlSexo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            TiposPersonas = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.TipoPersona);
            int? IdPersonaFisica = TiposPersonas.FirstOrDefault(x => x.CodigoValor == "F")?.IdListaValorDetalle;
            if (IdPersonaFisica.HasValue)
            {
                this.ddlTipoPersona.DataSource = TiposPersonas;
                this.ddlTipoPersona.DataValueField = "IdListaValorDetalle";
                this.ddlTipoPersona.DataTextField = "Descripcion";
                this.ddlTipoPersona.SelectedValue = IdPersonaFisica.ToString();
                this.ddlTipoPersona.DataBind();
            }
            else
            {
                this.ddlTipoPersona.DataSource = TiposPersonas;
                this.ddlTipoPersona.DataValueField = "IdListaValorDetalle";
                this.ddlTipoPersona.DataTextField = "Descripcion";
                this.ddlTipoPersona.DataBind();
            }

            List<AfiTiposDocumentos> tiposDocs = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataSource = tiposDocs;
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlTipoDocumento, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            if (tiposDocs.Exists(x => x.IdTipoDocumento == (int)EnumTiposDocumentos.DNI))
                this.ddlTipoDocumento.SelectedValue = ((int)EnumTiposDocumentos.DNI).ToString();

            this.ddlParentesco.DataSource = AfiliadosF.ParentescoObtenerListaActiva();
            this.ddlParentesco.DataValueField = "IdParentesco";
            this.ddlParentesco.DataTextField = "Parentesco";
            this.ddlParentesco.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlParentesco, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.chkAlertasTipos.DataSource = AfiliadosF.AlertasTiposObtenerListaFiltro(new AfiAlertasTipos());
            this.chkAlertasTipos.DataTextField = "AlertaTipo";
            this.chkAlertasTipos.DataValueField = "IdAlertaTipo";
            this.chkAlertasTipos.DataBind();

            this.ddlFilial.DataSource = this.UsuarioActivo.Filiales; //TGEGeneralesF.FilialesObenerLista();
            this.ddlFilial.DataValueField = "IdFilial";
            this.ddlFilial.DataTextField = "Filial";
            this.ddlFilial.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFilial, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            this.ddlFilial.SelectedValue = this.UsuarioActivo.FilialPredeterminada.IdFilial.ToString();

            this.ddlZonasGrupos.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresCodigos.ZonasGrupos);
            this.ddlZonasGrupos.DataValueField = "IdListaValorDetalle";
            this.ddlZonasGrupos.DataTextField = "Descripcion";
            this.ddlZonasGrupos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlZonasGrupos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlCondicionFiscal.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(Generales.Entidades.EnumTGEListasValoresSistemas.CondicionesFiscales);
            this.ddlCondicionFiscal.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlCondicionFiscal.DataTextField = "Descripcion";
            this.ddlCondicionFiscal.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCondicionFiscal, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }
        protected void ddlTipoDocumento_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ValidarTipoNumeroDocumento();
        }
        protected void ddlTipoPersona_SelectedIndexChanged(object sender, EventArgs e)
        {
            //falta validar
            if (this.TiposPersonas.FirstOrDefault(x => x.IdListaValorDetalle == Convert.ToInt32(this.ddlTipoPersona.SelectedValue)).CodigoValor == ((char)EnumTiposPersonas.Juridica).ToString())
            {
                this.dvSexo.Visible = false;
                this.rfvSexo.Enabled = false;
                this.txtNumeroDocumento.AutoPostBack = false;

                this.btnTxtCuitBlur.Visible = true;
                this.txtCUIL.Enabled = false;

                this.ddlTipoDocumento.SelectedValue = ((int)EnumTiposDocumentos.CUIT).ToString();
                this.ddlTipoDocumento.Enabled = false;
                this.rfvNombre.Enabled = false;
                //upImportarDatos.Update();
            }
            else
            {
                this.dvSexo.Visible = true;
                this.rfvSexo.Enabled = true;
                this.rfvNombre.Enabled = true;
                this.txtNumeroDocumento.AutoPostBack = true;
                this.btnTxtCuitBlur.Visible = false;
                this.txtCUIL.Enabled = true;
                this.ddlTipoDocumento.Enabled = true;
                //upImportarDatos.Update();
            }
        }
        protected void txtNumeroDocumento_TextChanged(object sender, EventArgs e)
        {
            this.ValidarTipoNumeroDocumento();
        }
        private void ValidarTipoNumeroDocumento()
        {
            if (string.IsNullOrEmpty(this.ddlTipoDocumento.SelectedValue))
                return;
            if (this.txtNumeroDocumento.Text.Trim() == string.Empty)
                return;

            AfiAfiliados afiValidar = new AfiAfiliados();
            afiValidar.TipoDocumento.IdTipoDocumento = Convert.ToInt32(this.ddlTipoDocumento.SelectedValue);
            afiValidar.NumeroDocumento = Convert.ToInt64(this.txtNumeroDocumento.Text);
            afiValidar.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Clientes;
            List<AfiAfiliados> afiliados = AfiliadosF.AfiliadosObtenerListaFiltro(afiValidar);
            afiliados = afiliados.Where(x => x.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Clientes
                || x.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.PacientesExternos).ToList();
            if (afiliados.Count > 0)
            {
                this.MiIdAfiliadoImportarCliente = afiliados[0].IdAfiliado;
                List<string> msgArgs = new List<string>();
                msgArgs.Add(afiliados[0].RazonSocial);
                switch (this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo)
                {
                    case (int)EnumAfiliadosTipos.Socios:
                        msgArgs.Add("Socio");
                        break;
                    case (int)EnumAfiliadosTipos.Familiares:
                        msgArgs.Add("Familiar");
                        break;
                    default:
                        break;
                }
                string codigoMsg = afiliados[0].AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.PacientesExternos ? "ValidarTipoNumeroDocumentoPaciente" : "ValidarTipoNumeroDocumentoCliente";
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", this.ObtenerMensajeSistema(codigoMsg, msgArgs));
                this.btnImportarCliente.Attributes.Add("OnClick", funcion);
                ScriptManager.RegisterStartupScript(this.upNumeroSocio, this.upNumeroSocio.GetType(), "ImportarClienteScript", "ImportarClienteComoSocio();", true);
                //this.upImportarDatos.Update();
            }
            else
            {
                if (!(afiValidar.TipoDocumento.IdTipoDocumento == (int)EnumTiposDocumentos.CUIL
                    || afiValidar.TipoDocumento.IdTipoDocumento == (int)EnumTiposDocumentos.CUIT)
                    && afiValidar.NumeroDocumento > 0)
                {
                    afiliados = AfiliadosF.AfiliadosObtenerDatosAFIPTxtPorDNI(afiValidar);
                    if (afiliados.Count == 1)
                    {
                        this.txtCUIL.Text = afiliados[0].CUIL.ToString();
                        if (this.GestionControl == Gestion.Agregar)
                        {
                            this.txtNombre.Text = afiliados[0].Nombre.ToString();
                            this.txtApellido.Text = afiliados[0].Apellido.ToString();
                        }
                    }
                    else if (afiliados.Count > 1)
                    {
                        this.ctrBuscarPadronTXT.IniciarControl(afiliados);
                    }
                    else
                    {
                        this.MostrarMensaje("ApiRenaperDNINoEncontrado", true);
                    }
                }
            }
        }
        private void CtrBuscarPadronTXT_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        {
            if (e.CUIL > 0)
            {
                txtCUIL.Text = e.CUIL.ToString();
                if (this.GestionControl == Gestion.Agregar)
                {
                    txtNombre.Text = e.Nombre.ToString();
                    txtApellido.Text = e.Apellido.ToString();
                }
            }
            upNumeroSocio.Update();
        }


        protected void btnImportarCliente_Click(object sender, EventArgs e)
        {
            AfiAfiliados afi = new AfiAfiliados();
            afi.IdAfiliado = this.MiIdAfiliadoImportarCliente;
            afi = AfiliadosF.AfiliadosObtenerDatos(afi);
            afi.NumeroSocio = this.txtNumeroSocio.Text;
            afi.AfiliadoTipo.IdAfiliadoTipo = this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo;
            afi.FechaIngreso = DateTime.Now;
            if (AfiliadosF.AfiliadosModificar(afi))
            {
                switch (afi.AfiliadoTipo.IdAfiliadoTipo)
                {
                    case (int)EnumAfiliadosTipos.Socios:
                        PaginaAfiliados paginaAfi = new PaginaAfiliados();
                        paginaAfi.Guardar(this.MiSessionPagina, afi);
                        Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/AfiliadosModificar.aspx"), true);
                        break;
                    case (int)EnumAfiliadosTipos.Familiares:
                        this.MisParametrosUrl = new Hashtable();
                        this.MisParametrosUrl.Add("Gestion", Gestion.Modificar);
                        this.MisParametrosUrl.Add("IdFamiliar", afi.IdAfiliado);
                        Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/FamiliaresModificar.aspx"), true);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                this.MostrarMensaje(afi.CodigoMensaje, true, afi.CodigoMensajeArgs);
            }
        }
        protected void btnObtenerNumeroSocio_Click(object sender, EventArgs e)
        {
            AfiAfiliados filtro = new AfiAfiliados();
            filtro.Filtro = "Recalcular";
            ObtenerNumeroSocio(filtro);
        }

        private void ObtenerNumeroSocio(AfiAfiliados filtro)
        {
            bool numerodesocio;
            filtro = MiAfiliado;
            filtro.Estado.IdEstado = Convert.ToInt32(ddlEstados.SelectedValue);
            filtro.Estado.Descripcion = ddlEstados.SelectedItem.Text;
            filtro.Filial.IdFilial = Convert.ToInt32(ddlFilial.SelectedValue);

            numerodesocio = AfiliadosF.AfiliadosObtenerProximoNumeroSocio(filtro);
            if (numerodesocio)
                txtNumeroSocio.Text = MiAfiliado.NumeroSocio.ToString();
            else
                MostrarMensaje("No se ha podido obtener el siguiente numero de socio", true);
        }

        protected void btnRenaper_Click(object sender, EventArgs e)
        {
            if (ddlTipoDocumento.SelectedValue == string.Empty)
            {
                MostrarMensaje("Debe ingresar el tipo de documento", true);
                return;
            }
            if (txtNumeroDocumento.Text.Trim() == string.Empty)
            {
                MostrarMensaje("Debe ingresar el numero de documento", true);
                return;
            }
            if (ddlSexo.SelectedValue == string.Empty)
            {
                MostrarMensaje("Debe ingresar el sexo", true);
                return;
            }

            MiAfiliado.TipoDocumento.IdTipoDocumento = Convert.ToInt32(ddlTipoDocumento.SelectedValue);
            MiAfiliado.NumeroDocumento = Convert.ToInt64(txtNumeroDocumento.Text);
            MiAfiliado.Sexo.IdSexo = Convert.ToInt32(ddlSexo.SelectedValue);
            //MiAfiliado.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            //MiAfiliado.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            //MiAfiliado.NumeroSocio = this.txtNumeroSocio.Text;
            //MiAfiliado.Categoria.IdCategoria = this.ddlCategoria.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlCategoria.SelectedValue);
            //MiAfiliado.Categoria.Categoria = this.ddlCategoria.SelectedValue == string.Empty ? string.Empty : this.ddlCategoria.SelectedItem.Text;

            if (!AfiliadosF.RenaperValidarObtenerDatos(MiAfiliado))
            {
                //this.txtApellido.Text = string.Empty;
                //this.txtNombre.Text = string.Empty;
                //this.txtCUIL.Text = string.Empty;
                //this.txtFechaNacimiento.Text = string.Empty;

                MostrarMensaje(MiAfiliado.CodigoMensaje, true);
            }
            else
            {
                this.txtApellido.Text = MiAfiliado.Apellido;
                this.txtNombre.Text = MiAfiliado.Nombre;
                this.txtCUIL.Text = MiAfiliado.CUILFormateado;
                this.txtFechaNacimiento.Text = AyudaProgramacion.MostrarFechaPantalla(MiAfiliado.FechaNacimiento);
                if (MiAfiliado.Domicilios.Count > 0)
                    AyudaProgramacion.CargarGrillaListas(MiAfiliado.Domicilios, true, this.gvDomicilios, true);

                MostrarMensaje(MiAfiliado.CodigoMensaje, false);
            }
        }

        /// <summary>
        /// Obtiene una lista de Alertas Seleccionadas para el Afiliado
        /// </summary>
        /// <param name="pUsuario"></param>
        private void ObtenerAlertasTipos(AfiAfiliados pAfiliado)
        {
            AfiAlertasTipos alerta;
            //pUsuario.AlertasTipos = new List<AlertasTipos>();

            foreach (ListItem lst in this.chkAlertasTipos.Items)
            {
                alerta = pAfiliado.AlertasTipos.Find(delegate (AfiAlertasTipos per)
                { return per.IdAlertaTipo == Convert.ToInt32(lst.Value); });

                if (alerta == null && lst.Selected)
                {
                    alerta = new AfiAlertasTipos();
                    alerta.IdAlertaTipo = Convert.ToInt32(lst.Value);
                    alerta.AlertaTipo = lst.Text;
                    pAfiliado.AlertasTipos.Add(alerta);
                    alerta.EstadoColeccion = EstadoColecciones.Agregado;
                }
                else if (alerta != null && !lst.Selected)
                    alerta.EstadoColeccion = EstadoColecciones.Borrado;

            }
        }

        /// <summary>
        /// Marca en Pantalla las Alertas Tipos que tiene el Afiliado
        /// </summary>
        /// <param name="pAfiliado"></param>
        private void CargarAlertasTiposAfiliado(AfiAfiliados pAfiliado)
        {
            foreach (AfiAlertasTipos alerta in pAfiliado.AlertasTipos)
            {
                foreach (ListItem item in chkAlertasTipos.Items)
                {
                    if (Convert.ToInt32(item.Value) == alerta.IdAlertaTipo)
                    {
                        item.Selected = true;
                    }
                }
            }
        }

        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            TGEPlantillas plantilla = new TGEPlantillas();
            if (this.MiAfiliado.Estado.IdEstado == (int)EstadosAfiliados.Baja)
                plantilla.Codigo = "AfiliadoSolicitudBaja";
            else
                plantilla.Codigo = "AfiliadoSolicitudIngreso";

            TESCajasMovimientos movimiento = new TESCajasMovimientos();
            movimiento.IdRefTipoOperacion = MiAfiliado.IdAfiliado;
            movimiento.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(UsuarioActivo);
            movimiento.TipoOperacion.IdTipoOperacion = (int)EnumTGETiposOperaciones.Afiliados;
            TGEPlantillas miPlantilla = TGEGeneralesF.PlantillasObtenerDatosPorTipoOperacion(movimiento);

            // miPlantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(miPlantilla);

            byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(EnumTGEComprobantes.AltaAfiliado, miPlantilla.Codigo, this.MiAfiliado, AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
            ExportPDF.ExportarPDF(pdf, this.UpdatePanel1, string.Concat("Afiliado_", this.MiAfiliado.IdAfiliado.ToString().PadLeft(10, '0')), this.UsuarioActivo);

        }

        protected void btnAceptarContinuar_Click(object sender, EventArgs e)
        {
            this.AceptarContinuar = true;
            AfiAfiliados parametros = this.BusquedaParametrosObtenerValor<AfiAfiliados>();
            parametros.HashTransaccion = this.tcDatos.ActiveTabIndex;
            this.BusquedaParametrosGuardarValor<AfiAfiliados>(parametros);
            this.btnAceptar_Click(sender, e);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.PersistirDatos();
            this.Page.Validate("AfiliadosModificarDatosAceptar");
            if (!this.Page.IsValid)
            {
                //this.upImportarDatos.Update();
                return;
            }
            this.btnAceptar.Visible = false;
            this.btnAceptarContinuar.Visible = false;

            this.MapearControlesAObjeto(this.MiAfiliado);

            //this.ActualizarGrilla();
            this.MiAfiliado.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    AyudaProgramacion.MatchObjectProperties(this.UsuarioActivo, this.MiAfiliado.UsuarioAlta);
                    this.MiAfiliado.UsuarioAlta.IdUsuarioAlta = this.MiAfiliado.UsuarioAlta.IdUsuario;
                    this.MiAfiliado.FechaAlta = DateTime.Now;

                    if (txtNumeroSocio.Enabled == false)
                    {
                        MiAfiliado.CalculaNumeroSocio = true;
                    }
                    else
                    {
                        MiAfiliado.CalculaNumeroSocio = false;
                    }

                    if (AfiliadosF.AfiliadosAgregar(this.MiAfiliado))
                    {
                        if (this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                        {
                            PaginaAfiliados paginaAfi = new PaginaAfiliados();
                            paginaAfi.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatos(this.MiAfiliado));
                        }
                        //this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, false);
                        this.btnImprimir.Visible = true;
                        if (this.AceptarContinuar)
                        {
                            if (this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                            {
                                PaginaAfiliados paginaAfi = new PaginaAfiliados();
                                paginaAfi.Guardar(this.MiSessionPagina, AfiliadosF.AfiliadosObtenerDatos(this.MiAfiliado));
                            }
                            this.IniciarControl(this.MiAfiliado, Gestion.Modificar);
                            this.upFamiliares.Update();
                            this.upApoderados.Update();
                            this.upDomicilios.Update();
                            this.upTelefonos.Update();
                        }
                        this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje));
                    }
                    else
                    {
                        this.btnAceptar.Visible = true;
                        //this.btnAceptarContinuar.Visible = true;
                        this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true, this.MiAfiliado.CodigoMensajeArgs);
                    }
                    break;
                case Gestion.Modificar:
                    if (AfiliadosF.AfiliadosModificar(this.MiAfiliado))
                    {
                        PaginaAfiliados pagina = new PaginaAfiliados();
                        AfiAfiliados afi = pagina.Obtener(this.MiSessionPagina);
                        if (afi.IdAfiliado == this.MiAfiliado.IdAfiliado)
                            pagina.Guardar(this.MiSessionPagina, this.MiAfiliado);
                        //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje));
                        //this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, false);
                        if (this.AceptarContinuar)
                        {
                            //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje));
                            this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, false);
                            this.IniciarControl(this.MiAfiliado, Gestion.Modificar);
                        }
                        else
                        {
                            this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje));
                        }
                    }
                    else
                    {
                        if (!this.MiAfiliado.ErrorAccesoDatos && this.MiAfiliado.ConfirmarAccion)
                        {
                            this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiAfiliado.CodigoMensaje, this.MiAfiliado.CodigoMensajeArgs), true);
                        }
                        else
                        {

                            this.btnAceptar.Visible = true;

                            this.btnAceptarContinuar.Visible = true;
                            this.MostrarMensaje(this.MiAfiliado.CodigoMensaje, true, this.MiAfiliado.CodigoMensajeArgs);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.AfiliadosModificarDatosCancelar != null)
                this.AfiliadosModificarDatosCancelar();
        }

        protected void ddlEstados_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlEstados.SelectedValue))
            {
                this.HabilitarFechaBaja();
                if ((MiAfiliado.Estado.IdEstado == (int)EstadosAfiliados.Baja
               || MiAfiliado.Estado.IdEstado == (int)EstadosAfiliados.Renuncia
               || MiAfiliado.Estado.IdEstado == (int)EstadosAfiliados.Expulsado
               || MiAfiliado.Estado.IdEstado == (int)EstadosAfiliados.Fallecido
               || MiAfiliado.Estado.IdEstado == (int)EstadosAfiliados.BajaArt171B)
               && Convert.ToInt32(ddlEstados.SelectedValue) == (int)EstadosAfiliados.Normal)
                {
                    txtFechaIngreso.Text = DateTime.Now.ToShortDateString();
                }

                AfiAfiliados filtro = new AfiAfiliados();
                ObtenerNumeroSocio(filtro);
            }
        }

        private void HabilitarFechaBaja()
        {
            int idEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            if (idEstado == (int)EstadosAfiliados.Baja
                || idEstado == (int)EstadosAfiliados.Renuncia
                || idEstado == (int)EstadosAfiliados.Expulsado
                || idEstado == (int)EstadosAfiliados.Fallecido
                || idEstado == (int)EstadosAfiliados.BajaArt171B
                )
            {
                this.txtFechaBaja.Enabled = true;
                this.rfvFechaBaja.Enabled = true;

                if (idEstado == (int)EstadosAfiliados.Fallecido)
                {
                    this.txtFechaFallecimiento.Enabled = true;
                }

            }
            else
            {
                this.txtFechaBaja.Enabled = false;
                this.rfvFechaBaja.Enabled = false;
                this.txtFechaBaja.Text = string.Empty;
                this.txtFechaFallecimiento.Enabled = false;
                this.txtFechaFallecimiento.Text = string.Empty;


            }
        }

        protected void ddlCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlCategoria.SelectedValue))
            {
                //int idCategoria = Convert.ToInt32(this.ddlCategoria.SelectedValue);
                //AfiCategorias categoria = new AfiCategorias();
                //categoria.IdCategoria = idCategoria;
                //this.txtNumeroSocio.Text = AfiliadosF.AfiliadosObtenerProximoNumeroSocio(categoria);
                this.MiAfiliado.Categoria.IdCategoria = Convert.ToInt32(this.ddlCategoria.SelectedValue);
                this.MiAfiliado.Filial.IdFilial = this.ddlFilial.SelectedValue == string.Empty ? this.MiAfiliado.Filial.IdFilial : Convert.ToInt32(this.ddlFilial.SelectedValue);
                if (MiAfiliado.AfiliadoTipo.IdAfiliadoTipo == (int)EnumAfiliadosTipos.Socios)
                {
                    AfiAfiliados afi = new AfiAfiliados();
                    afi.Categoria.IdCategoria = MiAfiliado.Categoria.IdCategoria;
                    afi.Filial.IdFilial = MiAfiliado.Filial.IdFilial;
                    afi.IdAfiliado = MiAfiliado.IdAfiliado;
                    if (!AfiliadosF.AfiliadosObtenerProximoNumeroSocio(afi))
                    {
                        this.MostrarMensaje(afi.CodigoMensaje, true, afi.CodigoMensajeArgs);
                    }
                    else
                        this.MiAfiliado.NumeroSocio = afi.NumeroSocio;
                }
                this.txtNumeroSocio.Text = this.MiAfiliado.NumeroSocio;
                //if (idCategoria == 2)
                //{
                //    //this.pnlAfiliadoFallecido.Visible = true;
                //    //AyudaProgramacion.CargarGrillaListas<AfiAfiliados>(new List<AfiAfiliados>(), false, this.gvAfiliado, true);
                //}
                //else
                //{
                //    //this.pnlAfiliadoFallecido.Visible = false;
                //    this.txtNumeroSocio.Enabled = false;
                //    this.MiAfiliado.NumeroSocioFallecido = string.Empty;
                //    this.MiAfiliado.IdAfiliadoFallecido = 0;
                //}
                //this.upNumeroSocio.Update();

                AfiAfiliados filtro = new AfiAfiliados();
                ObtenerNumeroSocio(filtro);
            }
        }

        protected void ddlFilial_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(this.ddlFilial.SelectedValue))
            {

                AfiAfiliados filtro = new AfiAfiliados();
                ObtenerNumeroSocio(filtro);
            }
        }

        #region Afiliados Categoria Adherente 1

        //protected void btnBuscar_Click(object sender, EventArgs e)
        //{
        //    this.ctrAfiliados.IniciarControl(true);
        //}

        //protected void btnBuscarFamiliar_Click(object sender, EventArgs e)
        //{
        //    AfiAfiliados afi = new AfiAfiliados();
        //    afi.IdAfiliado = this.MiAfiliado.IdAfiliadoFallecido;
        //    this.ctrAfiFamiliares.IniciarControl(afi, false, EnumAfiliadosTipos.Familiares, false);
        //}

        //void ctrAfiliados_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        //{
        //    this.MiAfiliado.NumeroSocio = e.NumeroSocio;
        //    this.MiAfiliado.NumeroSocioFallecido = e.NumeroSocio;
        //    this.MiAfiliado.IdAfiliadoFallecido = e.IdAfiliado;
        //    this.txtNumeroSocio.Text = e.NumeroSocio; // string.Concat(e.NumeroSocio, "/");
        //    this.txtNumeroSocio.Enabled = true;
        //    List<AfiAfiliados> lista = new List<AfiAfiliados>();
        //    lista.Add(e);
        //    AyudaProgramacion.CargarGrillaListas<AfiAfiliados>(lista, false, this.gvAfiliado, true);
        //    this.btnBuscarFamiliar.Visible = true;
        //    this.upNumeroSocio.Update();
        //}

        //void ctrAfiFamiliares_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        //{
        //    e.IdAfiliadoFallecido = this.MiAfiliado.IdAfiliadoFallecido;
        //    e.NumeroSocioFallecido = this.MiAfiliado.NumeroSocioFallecido;
        //    e.NumeroSocio = this.txtNumeroSocio.Text;
        //    e.IdAfiliadoRef = 0;
        //    e.Categoria.IdCategoria = Convert.ToInt32(this.ddlCategoria.SelectedValue);
        //    AyudaProgramacion.MatchObjectProperties(e, this.MiAfiliado);
        //    this.MiAfiliado.AfiliadoTipo.IdAfiliadoTipo = (int)EnumAfiliadosTipos.Socios;
        //    this.MapearObjetoAControles(this.MiAfiliado);
        //    this.upImportarDatos.Update();
        //}

        //protected void gvAfiliado_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    if (!(e.CommandName == "Borrar"))
        //        return;

        //    this.txtNumeroSocio.Enabled = false;
        //    this.MiAfiliado.NumeroSocioFallecido = string.Empty;
        //    this.MiAfiliado.IdAfiliadoFallecido = 0;
        //    this.btnBuscarFamiliar.Visible = false;
        //    this.ddlCategoria_SelectedIndexChanged(null, EventArgs.Empty);
        //}

        #endregion


        #region Familiares

        protected void btnAgregarFamiliar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/FamiliaresAgregar.aspx"), true);
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()
                || e.CommandName == Gestion.Impresion.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdFamiliar", this.MiAfiliado.Familiares[indiceColeccion].IdAfiliado);
            this.MisParametrosUrl.Add("IdAfiliado", this.MiAfiliado.IdAfiliado);
            if (e.CommandName == Gestion.Modificar.ToString())
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/FamiliaresModificar.aspx"), true);

            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.MiAfiliado.Familiares.RemoveAt(indiceColeccion);
                AyudaProgramacion.AcomodarIndices<AfiAfiliados>(this.MiAfiliado.Familiares);
                this.gvDatos.DataSource = this.MiAfiliado.Familiares;
                this.gvDatos.DataBind();
            }
            else if (e.CommandName == Gestion.Impresion.ToString())
            {
                AfiAfiliados familiar = AfiliadosF.AfiliadosObtenerDatosCompletos(this.MiAfiliado.Familiares[indiceColeccion]);
                this.PopUpComprobantes1.CargarReporte(familiar, EnumTGEComprobantes.AltaAfiliado);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/FamiliaresConsultar.aspx"), true);
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiAfiliados item = (AfiAfiliados)e.Row.DataItem;

                switch (this.GestionControl)
                {
                    case Gestion.Modificar:
                        bool permisoModificar = this.ValidarPermiso("FamiliaresModificar.aspx");
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = permisoModificar;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.Cells[6].FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("FamiliaresConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        bool permisoConsultar = this.ValidarPermiso("FamiliaresConsultar.aspx");
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = permisoConsultar;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Domicilios

        protected void btnAgregarDomicilio_Click(object sender, EventArgs e)
        {
            this.ctrDomicilios.IniciarControl(new AfiDomicilios(), Gestion.Agregar);
        }

        void ctrDomicilios_AfiliadosModificarDatosAceptar(object sender, AfiDomicilios e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiAfiliado.Domicilios.Add(e);
                    e.IndiceColeccion = this.MiAfiliado.Domicilios.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiAfiliado.Domicilios[this.MiIndiceDetalleModificar] = e;

                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiAfiliado.Domicilios, true, this.gvDomicilios, true);
            this.upDomicilios.Update();
        }

        protected void gvDomicilios_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrDomicilios.IniciarControl(this.MiAfiliado.Domicilios[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrDomicilios.IniciarControl(this.MiAfiliado.Domicilios[indiceColeccion], Gestion.Consultar);
            }
        }

        protected void gvDomicilios_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiDomicilios item = (AfiDomicilios)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("DomiciliosConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Telefonos

        protected void gvTelefonos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrTelefonos.IniciarControl(this.MiAfiliado.Telefonos[indiceColeccion], Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrTelefonos.IniciarControl(this.MiAfiliado.Telefonos[indiceColeccion], Gestion.Consultar);
            }
            else if (e.CommandName == "WSP")
            {
                string text = "";
                AfiTelefonos cel = AfiliadosF.AfiliadosObtenerTelefonoCelular(this.MiAfiliado);
                string numero = /*cel.Prefijo.ToString() +*/ cel.Numero.ToString();
                string urlwa = string.Format("https://api.whatsapp.com/send?phone={0}&text={1}", numero, HttpUtility.UrlEncode(text));
                ScriptManager.RegisterStartupScript(this.upTelefonos, this.upTelefonos.GetType(), "scriptWa", string.Format("EnviarWhatsApp('{0}');", urlwa), true);
            }
        }

        protected void gvTelefonos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiTelefonos item = (AfiTelefonos)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        ImageButton btnWSP = (ImageButton)e.Row.FindControl("btnWSP");
                        btnModificar.Visible = true;

                        if (item.TelefonoTipo.IdTelefonoTipo == 3)
                            btnWSP.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.Cells[4].FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("TelefonosConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        void ctrTelefonos_AfiliadosModificarDatosAceptar(object sender, AfiTelefonos e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiAfiliado.Telefonos.Add(e);
                    e.IndiceColeccion = this.MiAfiliado.Telefonos.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiAfiliado.Telefonos[this.MiIndiceDetalleModificar] = e;
                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiAfiliado.Telefonos, true, this.gvTelefonos, true);
            this.upTelefonos.Update();
        }

        protected void btnAgregarTelefono_Click(object sender, EventArgs e)
        {
            AfiTelefonos telefono = new AfiTelefonos();
            telefono.IdAfiliado = this.MiAfiliado.IdAfiliado;
            this.ctrTelefonos.IniciarControl(telefono, Gestion.Agregar);
        }

        #endregion

        #region Apoderados

        protected void btnAgregarApoderado_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ApoderadosAgregar.aspx"), true);
        }

        protected void gvApoderados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdApoderado", this.MiAfiliado.Apoderados[indiceColeccion].IdAfiliado);

            if (e.CommandName == Gestion.Modificar.ToString())
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ApoderadosModificar.aspx"), true);

            else if (e.CommandName == Gestion.Anular.ToString())
            {
                this.MiAfiliado.Apoderados.RemoveAt(indiceColeccion);
                AyudaProgramacion.AcomodarIndices<AfiAfiliados>(this.MiAfiliado.Apoderados);
                this.gvApoderado.DataSource = this.MiAfiliado.Apoderados;
                this.gvApoderado.DataBind();
            }

            else if (e.CommandName == Gestion.Consultar.ToString())
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Afiliados/ApoderadosConsultar.aspx"), true);
        }

        protected void gvApoderado_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                AfiAfiliados item = (AfiAfiliados)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        bool permisoModificar = this.ValidarPermiso("ApoderadosModificar.aspx");
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = permisoModificar;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("ApoderadosConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        bool permisoConsultar = this.ValidarPermiso("ApoderadosConsultar.aspx");
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = permisoConsultar;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
        protected void btnDesvincularSocio_Click(object sender, EventArgs e)
        {
            if (AfiliadosF.AfiliadosDesvincularSocio(this.MiAfiliado))
            {
                this.MostrarMensaje("El Socio fue desvinculado correctamente.", false);
                this.btnDesvincularSocio.Visible = false;
            }
            else
            {
                this.MostrarMensaje("Error al intentar desvincular al Socio.", true);
            }
            this.UpdatePanel1.Update();
        }
    }
}
