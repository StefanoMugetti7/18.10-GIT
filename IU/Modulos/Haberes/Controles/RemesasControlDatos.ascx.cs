using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Haberes.Entidades;
using Comunes.Entidades;
using Haberes;
using Afiliados.Entidades;
using Generales.FachadaNegocio;

namespace IU.Modulos.Haberes.Controles
{
    public partial class RemesasControlDatos : ControlesSeguros
    {
        private HabRemesas MiHabRemesas
        {
            get { return (HabRemesas)Session[this.MiSessionPagina + "RemesasControlDatosMiHabRemesas"]; }
            set { Session[this.MiSessionPagina + "RemesasControlDatosMiHabRemesas"] = value; }
        }

        private int MiIndiceDetalle
        {
            get { return (int)Session[this.MiSessionPagina + "RemesasControlDatosMiIndiceDetalle"]; }
            set { Session[this.MiSessionPagina + "RemesasControlDatosMiIndiceDetalle"] = value; }
        }

        public delegate void RemesasDatosAceptarEventHandler(object sender, HabRemesas e);
        public event RemesasDatosAceptarEventHandler RemesasModificarDatosAceptar;

        public delegate void RemesasDatosCancelarEventHandler();
        public event RemesasDatosCancelarEventHandler RemesasModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrAfiliados.AfiliadosBuscarSeleccionar += new IU.Modulos.Afiliados.Controles.AfiliadosBuscarPopUp.AfiliadosBuscarEventHandler(ctrAfiliados_AfiliadosBuscarSeleccionar);
            this.ctrEnviarMails.ArmarMail += new Comunes.EnviarMails.ArmarMailEventHandler(ctrEnviarMails_ArmarMail);
            this.ctrEnviarMails.IniciarProceso += new Comunes.EnviarMails.IniciarProcesoEventHandler(ctrEnviarMails_IniciarProceso);
            if (this.IsPostBack)
            {
                if (this.MiHabRemesas == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Solicitud Material
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(HabRemesas pRemesa, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiHabRemesas = HaberesF.RemesasObtenerDatosCompletos(pRemesa);

            this.MapearObjetoAControles(this.MiHabRemesas);

            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    break;
                case Gestion.Modificar:
                    string mensaje = this.ObtenerMensajeSistema("RemesasConfirmarDepositos");
                    mensaje = string.Format(mensaje, this.MiHabRemesas.CantidadDepositar.ToString(), this.MiHabRemesas.ImporteDepositar.ToString("C2"));
                    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    this.btnAceptar.Attributes.Add("OnClick", funcion);
                    this.btnAceptar.Visible = this.ValidarPermiso("RemesasProcesarDepositos.aspx");

                    MostrarExcel(this.MiHabRemesas);

                    mensaje = this.ObtenerMensajeSistema("RemesasConfirmarCierre");
                    mensaje = string.Format(mensaje, (this.MiHabRemesas.CantidadRegistros - this.MiHabRemesas.CantidadDepositada).ToString(), (this.MiHabRemesas.ImporteTotal-this.MiHabRemesas.ImporteDepositado).ToString("C2"));
                    funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                    this.btnCerrarRemesa.Attributes.Add("OnClick", funcion);
                    this.btnCerrarRemesa.Visible = this.ValidarPermiso("RemesasProcesarCierre.aspx");
                    break;
                case Gestion.Consultar:
                    this.btnAceptar.Visible = false;

                    MostrarExcel(this.MiHabRemesas);

                    this.btnCerrarRemesa.Visible = false;
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            List<TGEEstados> lista = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosAfiliados));
            lista.AddRange(TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosRemesasAfiliados)));
            this.ddlEstadosAfiliados.DataSource = lista;
            this.ddlEstadosAfiliados.DataValueField = "IdEstado";
            this.ddlEstadosAfiliados.DataTextField = "Descripcion";
            this.ddlEstadosAfiliados.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstadosAfiliados, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(EstadosRemesasDetalles));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstados, this.ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles(HabRemesas pRemesa)
        {
            this.txtPeriodo.Text = pRemesa.Periodo.ToString();
            hdfIdRemesaTipo.Value = pRemesa.RemesaTipo.IdRemesaTipo.ToString();
            this.txtCantidadRegistros.Text = pRemesa.CantidadRegistros.ToString();
            this.txtImporteTotal.Text = pRemesa.ImporteTotal.ToString("C2");
            this.txtCantidadDepositar.Text = pRemesa.CantidadDepositar.ToString();
            this.txtImporteDepositar.Text = pRemesa.ImporteDepositar.ToString("C2");

            AyudaProgramacion.CargarGrillaListas<HabRemesasDetalles>(this.MiHabRemesas.RemesasDetalles, false, this.gvDatos, true);
        }

        private void MapearControlesAObjeto(HabRemesas pCuenta)
        {
            
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            HabRemesasDetalles filtro = new HabRemesasDetalles();
            filtro.IdRemesa = this.MiHabRemesas.IdRemesa;
            filtro.Periodo = this.MiHabRemesas.Periodo;
            filtro.RemesaTipo.IdRemesaTipo = this.MiHabRemesas.RemesaTipo.IdRemesaTipo;
            filtro.NumeroDocumentoIAF = txtNumeroDocumento.Text == string.Empty ? 0 : Convert.ToInt64(this.txtNumeroDocumento.Text);
            filtro.IdEstadoAfiliado = this.ddlEstadosAfiliados.SelectedValue == string.Empty ? (int)EstadosTodos.Todos : Convert.ToInt32(this.ddlEstadosAfiliados.SelectedValue);
            filtro.Estado.IdEstado = this.ddlEstados.SelectedValue == string.Empty ? (int)EstadosTodos.Todos : Convert.ToInt32(this.ddlEstados.SelectedValue);
            this.MiHabRemesas.RemesasDetalles = HaberesF.RemesasDetallesObtenerListaFiltro(filtro);
            AyudaProgramacion.CargarGrillaListas<HabRemesasDetalles>(this.MiHabRemesas.RemesasDetalles, false, this.gvDatos, true);
        }

        //protected void btnEnviarMail_Click(object sender, EventArgs e)
        //{
        //    this.pnlEnviarMails.Visible = true;
        //}

        void ctrEnviarMails_IniciarProceso(ref List<Objeto> listaEnvio)
        {
            HabRemesasDetalles filtro = new HabRemesasDetalles();
            //filtro.IdRemesa = this.MiHabRemesas.IdRemesa;
            filtro.Periodo = Convert.ToInt32(this.txtPeriodo.Text); //this.MiHabRemesas.Periodo;
            filtro.RemesaTipo.IdRemesaTipo = Convert.ToInt32(hdfIdRemesaTipo.Value);// this.MiHabRemesas.RemesaTipo.IdRemesaTipo;
            filtro.IdEstadoAfiliado = (int)EstadosTodos.Todos;
            filtro.Estado.IdEstado = (int)EstadosRemesasDetalles.Depositado;
            //this.MiListaEnvio = HaberesF.RemesasDetallesObtenerPendienteEnvio(filtro);
            listaEnvio = HaberesF.RemesasDetallesObtenerPendienteEnvio(filtro).Cast<Objeto>().ToList();
            //cantidad = this.MiListaEnvio.Count;
        }

        bool ctrEnviarMails_ArmarMail(Objeto item, System.Net.Mail.MailMessage mail)
        {
            ((HabRemesasDetalles)item).Periodo = Convert.ToInt32(this.txtPeriodo.Text);
            item.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //HaberesF.HabRecibosArmarMail(this.MiListaEnvio[posicion], mail);
            return HaberesF.HabRecibosArmarMail( (HabRemesasDetalles)item, mail);
        }
        
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //if (!this.Page.IsValid)
            //    return;

            //this.MapearControlesAObjeto(this.MiHabRemesas);
            this.MiHabRemesas.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            guardo = HaberesF.RemesasProcesarDepostio(this.MiHabRemesas);

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiHabRemesas.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiHabRemesas.CodigoMensaje, true, this.MiHabRemesas.CodigoMensajeArgs);
            }
        }

        protected void btnCerrarRemesa_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //if (!this.Page.IsValid)
            //    return;

            //this.MapearControlesAObjeto(this.MiHabRemesas);
            this.MiHabRemesas.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            guardo = HaberesF.RemesasProcesarCierre(this.MiHabRemesas);

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiHabRemesas.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiHabRemesas.CodigoMensaje, true, this.MiHabRemesas.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.RemesasModificarDatosCancelar != null)
                this.RemesasModificarDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.RemesasModificarDatosAceptar != null)
                this.RemesasModificarDatosAceptar(null, this.MiHabRemesas);
        }

        #region HaberesDetalles

        void ctrAfiliados_AfiliadosBuscarSeleccionar(AfiAfiliados e)
        {
            HabRemesasDetalles detalle = this.MiHabRemesas.RemesasDetalles[this.MiIndiceDetalle];
            AyudaProgramacion.MatchObjectProperties(e, detalle.Afiliado);
            detalle.IdEstadoAfiliado = e.Estado.IdEstado;
            detalle.EstadoAfiliado = e.Estado.Descripcion;
            detalle.IdCategoriaAfiliado = e.Categoria.IdCategoria.Value;
            detalle.CategoriaAfiliado = e.Categoria.Categoria;
            detalle.Estado.IdEstado = (int)EstadosRemesasDetalles.Activo;
            detalle.Estado = TGEGeneralesF.TGEEstadosObtener(detalle.Estado);
            detalle.EstadoColeccion = EstadoColecciones.Modificado;
            detalle.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

            if (HaberesF.RemesasDetallesModificar(detalle))
            {
                this.MiHabRemesas.CantidadDepositar += 1;
                this.MiHabRemesas.ImporteDepositar += detalle.NetoIAF;
                this.MapearObjetoAControles(this.MiHabRemesas);
                this.MostrarMensaje(detalle.CodigoMensaje, false);
                upDetalles.Update();
            }
            else
            {
                this.MostrarMensaje(detalle.CodigoMensaje, true);
            }
            
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == "Modificar" || e.CommandName == "Borrar" || e.CommandName == "Agregar" ))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            this.MiIndiceDetalle = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            HabRemesasDetalles detalle = this.MiHabRemesas.RemesasDetalles[this.MiIndiceDetalle];

            if (e.CommandName == "Modificar")
            {
                switch (detalle.Estado.IdEstado)
                {
                    case (int)EstadosRemesasDetalles.Activo:
                    case (int)EstadosRemesasDetalles.SinValidar:
                        this.ctrAfiliados.IniciarControl(true);
                        break;
                    default:
                        break;
                }
            }
            else if (e.CommandName == "Borrar")
            {
                if (detalle.Estado.IdEstado == (int)EstadosRemesasDetalles.Activo)
                {
                    detalle.Estado.IdEstado = (int)EstadosRemesasDetalles.Baja;
                    detalle.Estado = TGEGeneralesF.TGEEstadosObtener(detalle.Estado);
                    detalle.EstadoColeccion = EstadoColecciones.Modificado;
                    detalle.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                    if (HaberesF.RemesasDetallesModificar(detalle))
                    {
                        this.MiHabRemesas.CantidadDepositar -= 1;
                        this.MiHabRemesas.ImporteDepositar -= detalle.NetoIAF;
                        this.MapearObjetoAControles(this.MiHabRemesas);
                        this.MostrarMensaje(detalle.CodigoMensaje, false);
                        upDetalles.Update();
                    }
                    else
                    {
                        this.MostrarMensaje(detalle.CodigoMensaje, true);
                    }
                }
            }
            else if (e.CommandName == "Agregar")
            {
                if (detalle.Estado.IdEstado == (int)EstadosRemesasDetalles.Baja)
                {
                    detalle.Estado.IdEstado = (int)EstadosRemesasDetalles.Activo;
                    detalle.Estado = TGEGeneralesF.TGEEstadosObtener(detalle.Estado);
                    detalle.EstadoColeccion = EstadoColecciones.Modificado;
                    detalle.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);

                    if (HaberesF.RemesasDetallesModificar(detalle))
                    {
                        this.MiHabRemesas.CantidadDepositar += 1;
                        this.MiHabRemesas.ImporteDepositar += detalle.NetoIAF;
                        this.MapearObjetoAControles(this.MiHabRemesas);
                        this.MostrarMensaje(detalle.CodigoMensaje, false);
                        upDetalles.Update();
                    }
                    else
                    {
                        this.MostrarMensaje(detalle.CodigoMensaje, true);
                    }
                }
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HabRemesasDetalles item = (HabRemesasDetalles)e.Row.DataItem;

                switch (this.GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton ibtn, ibtnEliminar, ibtnAgregar;
                        bool permisoModificar = true;// this.ValidarPermiso("RemesasDetallesModificar.aspx");

                        if (item.Estado.IdEstado == (int)EstadosRemesasDetalles.SinValidar)
                        {
                            ibtn = (ImageButton)e.Row.FindControl("btnModificar");
                            ibtn.Visible = permisoModificar;
                        }
                        else if (item.Estado.IdEstado == (int)EstadosRemesasDetalles.Activo)
                        {
                            ibtn = (ImageButton)e.Row.FindControl("btnModificar");
                            ibtn.Visible = permisoModificar;

                            ibtnEliminar = (ImageButton)e.Row.FindControl("btnEliminar");
                            ibtnEliminar.Visible = permisoModificar;

                            string mensaje = this.ObtenerMensajeSistema("RemesasDetallesConfirmarBaja");
                            mensaje = string.Format(mensaje, item.ApellidoNombre);
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtnEliminar.Attributes.Add("OnClick", funcion);
                        }
                        else if (item.Estado.IdEstado == (int)EstadosRemesasDetalles.Baja)
                        {
                            ibtn = (ImageButton)e.Row.FindControl("btnModificar");
                            ibtn.Visible = permisoModificar;

                            ibtnAgregar = (ImageButton)e.Row.FindControl("btnAgregar");
                            ibtnAgregar.Visible = permisoModificar;

                            string mensaje = this.ObtenerMensajeSistema("RemesasDetallesConfirmarAgregar");
                            mensaje = string.Format(mensaje, item.ApellidoNombre);
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtnAgregar.Attributes.Add("OnClick", funcion);
                        }
                        
                        break;
                    default:
                        break;
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblNetoIAF = (Label)e.Row.FindControl("lblNetoIAF");
                lblNetoIAF.Text = this.MiHabRemesas.RemesasDetalles.Sum(x => x.NetoIAF).ToString("C2");
                Label lblRegistros = (Label)e.Row.FindControl("lblGrillaTotalRegistros");
                lblRegistros.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiHabRemesas.RemesasDetalles.Count);
                
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvDatos.PageIndex = e.NewPageIndex;
            AyudaProgramacion.CargarGrillaListas<HabRemesasDetalles>(this.MiHabRemesas.RemesasDetalles, false, this.gvDatos, true);
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiHabRemesas.RemesasDetalles = this.OrdenarGrillaDatos<HabRemesasDetalles>(this.MiHabRemesas.RemesasDetalles, e);
            AyudaProgramacion.CargarGrillaListas<HabRemesasDetalles>(this.MiHabRemesas.RemesasDetalles, false, this.gvDatos, true);
        }

        #endregion

        #region Excel
        private void MostrarExcel(HabRemesas pRemesa)
        {
            if (pRemesa.RemesasDetalles.Count > 0)
                btnExportarExcel.Visible = true;
            else
                btnExportarExcel.Visible = false;
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvDatos.AllowPaging = false;
            this.gvDatos.DataSource = this.MiHabRemesas.RemesasDetalles;
            this.gvDatos.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvDatos);
        }

        #endregion

    }
}
