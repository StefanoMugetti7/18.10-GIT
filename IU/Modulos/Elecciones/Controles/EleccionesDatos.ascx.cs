using Comunes.Entidades;
using Elecciones;
using Elecciones.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using EnumElecciones = Elecciones.Entidades.EnumElecciones;

namespace IU.Modulos.Elecciones.Controles
{
    public partial class EleccionesDatos : ControlesSeguros
    {

        private EleListasElecciones MiListaEleccion
        {
            get { return (EleListasElecciones)Session[this.MiSessionPagina + "EleccionModificarDatosMiListaEleccion"]; }
            set { Session[this.MiSessionPagina + "EleccionModificarDatosMiListaEleccion"] = value; }
        }
        public List<EleListasEleccionesPostulantes> MisPostulantes
        {
            get { return this.PropiedadObtenerValor<List<EleListasEleccionesPostulantes>>("EleccionModificarDatosMiListaEleccionPostulantes"); }
            set { this.PropiedadGuardarValor("EleccionModificarDatosMiListaEleccionPostulantes", value); }
        }

        public DataTable MisPuestos
        {
            get { return this.PropiedadObtenerValor<DataTable>("EleccionModificarDatosMisPuestos"); }
            set { this.PropiedadGuardarValor("EleccionModificarDatosMisPuestos", value); }
        }
        public List<EleListasEleccionesApoderados> MisApoderados
        {
            get { return this.PropiedadObtenerValor<List<EleListasEleccionesApoderados>>("EleccionModificarDatosMiListaEleccionApoderados"); }
            set { this.PropiedadGuardarValor("EleccionModificarDatosMiListaEleccionApoderados", value); }
        }
        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlModificarDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (this.IsPostBack)
            {
                if (this.MiListaEleccion == null && this.GestionControl != Gestion.Agregar)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                }
            }
            else
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    this.PersistirDatosGrilla();
                    this.PersistirDatosGrillaApoderados();
                    this.PersistirDatosGrillaAvales();
                }
            }
        }
        public void IniciarControl(EleListasElecciones pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MisPuestos = EleccionesF.ListasEleccionesPostulantesObtenerPuestos(pParametro);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiListaEleccion = pParametro;
                    this.txtCantidadAgregarApoderados.Enabled = true;
                    this.txtCantidadAgregarAvales.Enabled = true;
                    this.IniciarGrilla();
                    this.IniciarGrillaAvales();
                    this.IniciarGrillaApoderados();
                    this.ddlEstado.Items.Remove(this.ddlEstado.Items.FindByValue(((int)EnumElecciones.Autorizado).ToString()));
                    this.ddlEstado.Items.Remove(this.ddlEstado.Items.FindByValue(((int)EnumElecciones.Baja).ToString()));
                    this.ddlEstado.Items.Remove(this.ddlEstado.Items.FindByValue(((int)EnumElecciones.Rechazado).ToString()));
                    this.ddlEstado.SelectedValue = ((int)EnumElecciones.Pendiente).ToString();
                    this.ddlEstado.Enabled = false;
                    this.txtCantidadAgregarApoderados.Text = "1";
                    this.txtCantidadAgregarAvales.Text = "1";
                    this.tpHistorial.Visible = false;
                    this.ddlRegion.Enabled = false;
                    this.ctrComentarios.IniciarControl(new EleListasElecciones(), this.GestionControl);
                    this.ctrArchivos.IniciarControl(new EleListasElecciones(), this.GestionControl);
                    this.ddlTipoLista_SelectedIndexChanged(null, new EventArgs());
                    break;
                case Gestion.Modificar:
                    this.MiListaEleccion = EleccionesF.ListasEleccionesObtenerDatosCompletos(pParametro);
                    this.ddlEstado.Items.Remove(this.ddlEstado.Items.FindByValue(((int)EnumElecciones.Autorizado).ToString()));
                    this.MapearObjetoAControles(this.MiListaEleccion);
                    this.txtCantidadAgregarApoderados.Enabled = true;
                    this.txtCantidadAgregarAvales.Enabled = true;
                    this.txtCantidadAgregarApoderados.Text = "1";
                    this.txtCantidadAgregarAvales.Text = "1";
                    this.tpHistorial.Visible = false;
                    this.ddlTipoLista_SelectedIndexChanged(null, new EventArgs());

                    this.ddlEleccion.Enabled = false;
                    this.ddlTipoLista.Enabled = false;
                    this.ddlRegion.Enabled = false;
                    this.ctrArchivos.IniciarControl(MiListaEleccion, this.GestionControl);
                    break;
                case Gestion.Autorizar:
                    this.MiListaEleccion = EleccionesF.ListasEleccionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiListaEleccion);
                    this.txtLista.Enabled = false;
                    this.ddlTipoLista.Enabled = false;
                    this.ddlRegion.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.ddlEleccion.Enabled = false;

                    this.gvApoderados.Columns[this.gvApoderados.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.txtCantidadAgregarApoderados.Visible = false;
                    this.lblCantidadAgregarApoderados.Visible = false;
                    this.btnAgregarItemApoderados.Visible = false;

                    this.gvAvales.Columns[this.gvAvales.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.btnAgregarItemAvales.Visible = false;
                    this.txtCantidadAgregarAvales.Visible = false;
                    this.lblCantidadAgregarAvales.Visible = false;

                    this.btnAceptar.Text = "Autorizar";
                    this.tpHistorial.Visible = true;

                    this.ddlTipoLista_SelectedIndexChanged(null, new EventArgs());
                    this.ctrArchivos.IniciarControl(MiListaEleccion, this.GestionControl);
                    break;
                case Gestion.Consultar:
                    this.MiListaEleccion = EleccionesF.ListasEleccionesObtenerDatosCompletos(pParametro);
                    this.MapearObjetoAControles(this.MiListaEleccion);
                    this.txtLista.Enabled = false;
                    this.ddlTipoLista.Enabled = false;
                    this.ddlRegion.Enabled = false;
                    this.ddlEstado.Enabled = false;
                    this.ddlEleccion.Enabled = false;
                    this.btnAceptar.Visible = false;

                    this.gvApoderados.Columns[this.gvApoderados.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.txtCantidadAgregarApoderados.Visible = false;
                    this.lblCantidadAgregarApoderados.Visible = false;
                    this.btnAgregarItemApoderados.Visible = false;

                    this.gvAvales.Columns[this.gvAvales.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.btnAgregarItemAvales.Visible = false;
                    this.txtCantidadAgregarAvales.Visible = false;
                    this.lblCantidadAgregarAvales.Visible = false;
                    this.tpHistorial.Visible = true;

                    this.ddlEleccion.Enabled = false;
                    this.ddlTipoLista.Enabled = false;
                    this.ddlRegion.Enabled = false;

                    this.ddlTipoLista_SelectedIndexChanged(null, new EventArgs());
                    this.ctrArchivos.IniciarControl(MiListaEleccion, this.GestionControl);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista("ELEListasElecciones");
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlEstado, ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoLista.DataSource = TGEGeneralesF.ListasValoresObtenerListaDetalle(EnumTGEListasValoresCodigos.EleccionesTiposListas);
            this.ddlTipoLista.DataValueField = "IdListaValorDetalle";
            this.ddlTipoLista.DataTextField = "Descripcion";
            this.ddlTipoLista.DataBind();
            AyudaProgramacion.InsertarItemSeleccione(this.ddlTipoLista, ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlEleccion.DataSource = EleccionesF.ListasEleccionesObtenerElecciones();
            this.ddlEleccion.DataValueField = "IdEleccion";
            this.ddlEleccion.DataTextField = "Eleccion";
            this.ddlEleccion.DataBind();

            AyudaProgramacion.InsertarItemSeleccione(this.ddlListaElegir, ObtenerMensajeSistema("SeleccioneOpcion"));
        }

        private void MapearObjetoAControles(EleListasElecciones pParametro)
        {
            this.ddlEstado.SelectedValue = pParametro.Estado.IdEstado.ToString();
            this.txtLista.Text = pParametro.Lista;
            this.txtIdListaEleccion.Text = pParametro.IdListaEleccion.ToString();
            this.txtPorcentajeAval.Text = pParametro.PorcentajeAval.ToString() == string.Empty ? "0" : pParametro.PorcentajeAval.ToString();

            ListItem item3 = ddlEleccion.Items.FindByValue(pParametro.Eleccion.IdEleccion.ToString());
            if (item3 == null)
                ddlEleccion.Items.Add(new ListItem(pParametro.Eleccion.Eleccion, pParametro.Eleccion.IdEleccion.ToString()));

            ddlEleccion.SelectedValue = pParametro.Eleccion.IdEleccion.ToString();

            ListItem item4 = ddlTipoLista.Items.FindByValue(pParametro.IdTipoLista.ToString());
            if (item4 == null)
                ddlTipoLista.Items.Add(new ListItem(pParametro.TipoLista, pParametro.IdTipoLista.ToString()));

            ddlTipoLista.SelectedValue = pParametro.IdTipoLista.ToString();

            ListItem item5 = ddlRegion.Items.FindByValue(pParametro.IdTipoRegion.ToString());
            if (item5 == null)
                ddlRegion.Items.Add(new ListItem(pParametro.TipoRegion, pParametro.IdTipoRegion.ToString()));

            ddlRegion.SelectedValue = pParametro.IdTipoRegion.ToString();

            ListItem item6 = ddlListaElegir.Items.FindByValue(pParametro.IdListaRef.ToString());
            if (item6 == null)
                ddlListaElegir.Items.Add(new ListItem(pParametro.ListaRef, pParametro.IdListaRef.ToString()));

            ddlListaElegir.SelectedValue = pParametro.IdListaRef.ToString();

            if (pParametro.Postulantes.Count > 0)
            {
                pParametro.Postulantes.ForEach(x => x.EstadoColeccion = EstadoColecciones.SinCambio);
                AyudaProgramacion.CargarGrillaListas<EleListasEleccionesPostulantes>(pParametro.Postulantes, false, this.gvPostulantes, true);
                this.upPostulantes.Update();
            }
            else
            {
                IniciarGrilla();
            }

            if (pParametro.Apoderados.Count > 0)
            {
                pParametro.Apoderados.ForEach(x => x.EstadoColeccion = EstadoColecciones.SinCambio);
                AyudaProgramacion.CargarGrillaListas<EleListasEleccionesApoderados>(pParametro.Apoderados, false, this.gvApoderados, true);
                this.upApoderados.Update();
            }
            else
            {
                IniciarGrillaApoderados();
            }

            if (pParametro.Avales.Count > 0)
            {
                pParametro.Avales.ForEach(x => x.EstadoColeccion = EstadoColecciones.SinCambio);
                AyudaProgramacion.CargarGrillaListas<EleListasEleccionesAvales>(pParametro.Avales, false, this.gvAvales, true);
                this.upAvales.Update();
            }
            else
            {
                IniciarGrillaAvales();
            }

            this.ctrAuditoria.IniciarControl(pParametro);
            this.ctrComentarios.IniciarControl(pParametro, GestionControl);
        }
        private void MapearControlesAObjeto(EleListasElecciones pParametro)
        {
            pParametro.Lista = this.txtLista.Text;
            pParametro.Eleccion.IdEleccion = this.ddlEleccion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlEleccion.SelectedValue);
            pParametro.IdTipoLista = this.ddlTipoLista.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlTipoLista.SelectedValue);
            pParametro.IdTipoRegion = this.ddlRegion.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlRegion.SelectedValue);
            pParametro.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);
            pParametro.PorcentajeAval = this.txtPorcentajeAval.Text == string.Empty ? 0 : Convert.ToDecimal(this.txtPorcentajeAval.Text);
            pParametro.IdListaRef = this.ddlListaElegir.SelectedValue == string.Empty ? (Nullable<int>)null : Convert.ToInt32(this.ddlListaElegir.SelectedValue);
            pParametro.Comentarios = ctrComentarios.ObtenerLista();
            pParametro.Archivos = ctrArchivos.ObtenerLista();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptar.Visible = false;
            this.MapearControlesAObjeto(this.MiListaEleccion);
            this.PersistirDatosGrilla();
            this.PersistirDatosGrillaAvales();
            this.PersistirDatosGrillaApoderados();
            this.MiListaEleccion.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    MiListaEleccion.IdListaEleccion = 0;
                    guardo = EleccionesF.ListasEleccionesAgregar(this.MiListaEleccion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiListaEleccion.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Modificar:
                    guardo = EleccionesF.ListasEleccionesModificar(this.MiListaEleccion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiListaEleccion.CodigoMensaje, false);
                    }
                    break;
                case Gestion.Autorizar:
                    guardo = EleccionesF.ListasEleccionesAutorizar(this.MiListaEleccion);
                    if (guardo)
                    {
                        this.MostrarMensaje(this.MiListaEleccion.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(this.MiListaEleccion.CodigoMensaje, true, this.MiListaEleccion.CodigoMensajeArgs);
                if (this.MiListaEleccion.dsResultado != null)
                {
                    this.MiListaEleccion.dsResultado = null;
                }
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ControlModificarDatosCancelar != null)
                this.ControlModificarDatosCancelar();
        }

        protected void ddlTipoLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlTipoLista.SelectedValue))
            {
                this.gvPostulantes.DataSource = null;
                this.gvPostulantes.DataBind();
                this.ddlRegion.Enabled = false;
                this.ddlRegion.Items.Clear();
                AyudaProgramacion.InsertarItemSeleccione(this.ddlRegion, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                if (this.GestionControl == Gestion.Agregar)
                {
                    this.MiListaEleccion.IdTipoLista = Convert.ToInt32(this.ddlTipoLista.SelectedValue);
                    this.MisPuestos = EleccionesF.ListasEleccionesPostulantesObtenerPuestos(this.MiListaEleccion);

                    this.IniciarGrilla();

                    this.ddlRegion.Items.Clear();
                    this.ddlRegion.DataSource = EleccionesF.ListasEleccionesObtenerRegiones(this.MiListaEleccion);
                    this.ddlRegion.DataValueField = "IdListaValorDetalle";
                    this.ddlRegion.DataTextField = "Descripcion";
                    this.ddlRegion.DataBind();

                    if (ddlRegion.Items.Count == 1)
                    {
                        this.ddlRegion.Enabled = false;
                    }
                    else if (ddlRegion.Items.Count > 0)
                    {
                        this.ddlRegion.Enabled = true;
                        this.ddlRegion_SelectedIndexChanged(null, new EventArgs());
                    }

                    if (this.GestionControl == Gestion.Consultar ||
                        this.GestionControl == Gestion.Autorizar)
                    {
                        this.ddlRegion.Enabled = false;
                        AyudaProgramacion.InsertarItemSeleccione(this.ddlRegion, ObtenerMensajeSistema("SeleccioneOpcion"));
                    }
                }
            }
            this.PersistirDatosGrilla();
            this.PersistirDatosGrillaApoderados();
            this.PersistirDatosGrillaAvales();
        }
        protected void ddlRegion_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlRegion.SelectedValue))
            {
                this.ddlListaElegir.Enabled = false;
                this.ddlListaElegir.Items.Clear();
                AyudaProgramacion.InsertarItemSeleccione(this.ddlListaElegir, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
            else
            {
                this.MiListaEleccion.IdTipoLista = Convert.ToInt32(this.ddlTipoLista.SelectedValue);
                this.MiListaEleccion.IdTipoRegion = Convert.ToInt32(this.ddlRegion.SelectedValue);
                this.ddlListaElegir.Items.Clear();
                List<EleListasElecciones> lista = EleccionesF.ListasEleccionesObtenerListasEleccionesRegionalesSinRepresentantes(this.MiListaEleccion);
                this.ddlListaElegir.DataSource = lista;
                this.ddlListaElegir.DataValueField = "IdListaEleccion";
                this.ddlListaElegir.DataTextField = "Lista";
                this.ddlListaElegir.DataBind();
                this.ddlListaElegir.Enabled = ddlListaElegir.Items.Count > 0;
                AyudaProgramacion.InsertarItemSeleccione(this.ddlListaElegir, ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }
        #region GRILLA POSTULANTES
        protected void gvPostulantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (!(e.CommandName == Gestion.Consultar.ToString()
            //    || e.CommandName == Gestion.Anular.ToString()))
            //    return;

            //int index = Convert.ToInt32(e.CommandArgument);
            //int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());

            //this.MisParametrosUrl = new Hashtable();
            //this.MisParametrosUrl.Add("Gestion", e.CommandName);
            //this.MisParametrosUrl.Add("IdListaEleccionPostulante", this.MiListaEleccion.Postulantes[indiceColeccion].IdListaEleccionPostulante);

            //if (e.CommandName == Gestion.Anular.ToString())
            //{
            //    this.MiListaEleccion.Postulantes[indiceColeccion].EstadoColeccion = EstadoColecciones.Borrado;
            //    AyudaProgramacion.CargarGrillaListas<EleListasEleccionesPostulantes>(this.MiListaEleccion.Postulantes, true, this.gvPostulantes, true);
            //}
        }
        protected void gvPostulantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlAfiliado = ((DropDownList)e.Row.FindControl("ddlAfiliado"));
                if (GestionControl == Gestion.Modificar || GestionControl == Gestion.Consultar || GestionControl == Gestion.Autorizar)
                {
                    EleListasEleccionesPostulantes item = (EleListasEleccionesPostulantes)e.Row.DataItem;
                    if (item.IdAfiliado > 0)
                        ddlAfiliado.Items.Add(new ListItem(item.Afiliado, item.IdAfiliado.ToString()));

                    switch (GestionControl)
                    {
                        case Gestion.Modificar:
                            ddlAfiliado.Enabled = true;
                            break;
                        case Gestion.Consultar:
                        case Gestion.Autorizar:
                            ddlAfiliado.Enabled = false;
                            break;
                        default:
                            break;
                    }
                }
                else if (GestionControl == Gestion.Agregar)
                {
                    ddlAfiliado.Enabled = true;
                }
            }
        }
        private void IniciarGrilla()
        {
            this.gvPostulantes.DataSource = this.MisPuestos;
            this.gvPostulantes.DataBind();
            if (this.MiListaEleccion.Postulantes.Count > 0)
            {
                this.MiListaEleccion.Postulantes = new List<EleListasEleccionesPostulantes>();
            }
            foreach (var item in MisPuestos.Rows)
            {
                this.MiListaEleccion.Postulantes.Add(new EleListasEleccionesPostulantes());
            }
        }
        private void PersistirDatosGrilla()
        {
            if (this.MiListaEleccion.Postulantes.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvPostulantes.Rows)
            {
                DropDownList ddlAfiliado = ((DropDownList)fila.FindControl("ddlAfiliado"));
                HiddenField hdfIdAfiliado = (HiddenField)fila.FindControl("hdfIdAfiliado");
                HiddenField hdfIdPuesto = (HiddenField)fila.FindControl("hdfIdPuesto");
                HiddenField hdfAfiliado = (HiddenField)fila.FindControl("hdfAfiliado");
                Label lblPuesto = (Label)fila.FindControl("lblPuesto");

                if (string.IsNullOrEmpty(hdfIdAfiliado.Value))
                    hdfIdAfiliado.Value = "-1";

                this.MiListaEleccion.Postulantes[fila.RowIndex].EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiListaEleccion.Postulantes[fila.RowIndex], GestionControl);
                this.MiListaEleccion.Postulantes[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                this.MiListaEleccion.Postulantes[fila.RowIndex].IdAfiliado = Convert.ToInt32(hdfIdAfiliado.Value);

                if (this.MiListaEleccion.Postulantes[fila.RowIndex].IdListaEleccionPostulante > 0 && this.MiListaEleccion.Postulantes[fila.RowIndex].IdAfiliado != Convert.ToInt32(hdfIdAfiliado.Value))
                {
                    this.MiListaEleccion.Postulantes[fila.RowIndex].EstadoColeccion = EstadoColecciones.Modificado;
                }
                this.MiListaEleccion.Postulantes[fila.RowIndex].Afiliado = hdfAfiliado.Value;
                this.MiListaEleccion.Postulantes[fila.RowIndex].IdPuesto = Convert.ToInt32(hdfIdPuesto.Value);
                this.MiListaEleccion.Postulantes[fila.RowIndex].Puesto = lblPuesto.Text;
            }
        }
        #endregion
        #region GRILLA APODERADOS
        protected void gvApoderados_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            PersistirDatosGrillaApoderados();
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdListaEleccionApoderado", this.MiListaEleccion.Apoderados[indiceColeccion].IdListaEleccionApoderado);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.MiListaEleccion.Apoderados.RemoveAt(indiceColeccion);
                //this.MiListaEleccion.Apoderados[indiceColeccion].EstadoColeccion = EstadoColecciones.Borrado;
                AyudaProgramacion.AcomodarIndices<EleListasEleccionesApoderados>(this.MiListaEleccion.Apoderados);
                AyudaProgramacion.CargarGrillaListas<EleListasEleccionesApoderados>(this.MiListaEleccion.Apoderados, true, this.gvApoderados, true);

            }
        }
        protected void gvApoderados_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                DropDownList ddlAfiliado = ((DropDownList)e.Row.FindControl("ddlAfiliadoApoderado"));
                EleListasEleccionesApoderados item = (EleListasEleccionesApoderados)e.Row.DataItem;
                if ((GestionControl == Gestion.Consultar || GestionControl == Gestion.Modificar || GestionControl == Gestion.Autorizar) || (GestionControl == Gestion.Agregar && item != null))
                {
                    if (item.IdAfiliado > 0)
                        ddlAfiliado.Items.Add(new ListItem(item.Afiliado, item.IdAfiliado.ToString()));

                    switch (GestionControl)
                    {
                        case Gestion.Agregar:
                            ddlAfiliado.Enabled = true;
                            break;
                        case Gestion.Modificar:
                            ddlAfiliado.Enabled = true;
                            break;
                        case Gestion.Consultar:
                        case Gestion.Autorizar:
                            ddlAfiliado.Enabled = false;
                            break;
                        default:
                            break;
                    }
                }
                else if (GestionControl == Gestion.Agregar)
                {
                    ddlAfiliado.Enabled = true;
                }
            }
        }
        private void IniciarGrillaApoderados()
        {
            EleListasEleccionesApoderados item;
            if (this.MiListaEleccion.Apoderados.Count > 0)
            {
                this.MiListaEleccion.Apoderados = new List<EleListasEleccionesApoderados>();
            }
            for (int i = 0; i < 1; i++)
            {
                item = new EleListasEleccionesApoderados();
                item.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiListaEleccion.Apoderados.Add(item);
                item.IndiceColeccion = this.MiListaEleccion.Apoderados.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<EleListasEleccionesApoderados>(this.MiListaEleccion.Apoderados, false, this.gvApoderados, true);
        }
        private void PersistirDatosGrillaApoderados()
        {
            if (this.MiListaEleccion.Apoderados.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvApoderados.Rows)
            {
                HiddenField hdfIdAfiliado = (HiddenField)fila.FindControl("hdfIdAfiliadoApoderado");
                HiddenField hdfAfiliado = (HiddenField)fila.FindControl("hdfAfiliadoApoderado");

                if (hdfIdAfiliado.Value != string.Empty && Convert.ToInt32(hdfIdAfiliado.Value) > 0)
                {
                    this.MiListaEleccion.Apoderados[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                    if (this.MiListaEleccion.Apoderados[fila.RowIndex].IdListaEleccionApoderado > 0 && this.MiListaEleccion.Apoderados[fila.RowIndex].IdAfiliado != Convert.ToInt32(hdfIdAfiliado.Value))
                    {
                        this.MiListaEleccion.Apoderados[fila.RowIndex].EstadoColeccion = EstadoColecciones.Modificado;
                    }
                    this.MiListaEleccion.Apoderados[fila.RowIndex].IdAfiliado = Convert.ToInt32(hdfIdAfiliado.Value);
                    this.MiListaEleccion.Apoderados[fila.RowIndex].Afiliado = hdfAfiliado.Value;
                }
            }
        }
        protected void btnAgregarItemApoderados_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrillaApoderados();
            this.AgregarItemApoderados();
        }
        private void AgregarItemApoderados()
        {
            EleListasEleccionesApoderados item;
            if (this.txtCantidadAgregarApoderados.Text == string.Empty || txtCantidadAgregarApoderados.Text == "0")
            {
                this.txtCantidadAgregarApoderados.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregarApoderados.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new EleListasEleccionesApoderados();
                this.MiListaEleccion.Apoderados.Add(item);
                item.IndiceColeccion = this.MiListaEleccion.Apoderados.IndexOf(item);
                item.IdListaEleccionApoderado = item.IndiceColeccion * -1;
                item.EstadoColeccion = EstadoColecciones.Agregado;
            }
            AyudaProgramacion.CargarGrillaListas<EleListasEleccionesApoderados>(this.MiListaEleccion.Apoderados, true, this.gvApoderados, true);
        }
        #endregion
        #region GRILLA AVALES
        protected void gvAvales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName == Gestion.Anular.ToString()
                || e.CommandName == Gestion.Autorizar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            PersistirDatosGrillaAvales();
            this.MisParametrosUrl = new Hashtable();
            this.MisParametrosUrl.Add("Gestion", e.CommandName);
            this.MisParametrosUrl.Add("IdListaEleccionAval", this.MiListaEleccion.Avales[indiceColeccion].IdListaEleccionAval);

            if (e.CommandName == Gestion.Anular.ToString())
            {
                this.MiListaEleccion.Avales.RemoveAt(indiceColeccion);
                //this.MiListaEleccion.Avales[indiceColeccion].EstadoColeccion = EstadoColecciones.Borrado;
                AyudaProgramacion.AcomodarIndices<EleListasEleccionesAvales>(this.MiListaEleccion.Avales);
                AyudaProgramacion.CargarGrillaListas<EleListasEleccionesAvales>(this.MiListaEleccion.Avales, true, this.gvAvales, true);
                //this.PersistirDatosGrillaAvales();
            }
        }
        protected void gvAvales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DropDownList ddlAfiliado = ((DropDownList)e.Row.FindControl("ddlAfiliadoAval"));
                EleListasEleccionesAvales item = (EleListasEleccionesAvales)e.Row.DataItem;
                if ((GestionControl == Gestion.Consultar || GestionControl == Gestion.Modificar || GestionControl == Gestion.Autorizar) || (GestionControl == Gestion.Agregar && item != null))
                {
                    if (item.IdAfiliado > 0)
                        ddlAfiliado.Items.Add(new ListItem(item.Afiliado, item.IdAfiliado.ToString()));

                    switch (GestionControl)
                    {
                        case Gestion.Agregar:
                            ddlAfiliado.Enabled = true;
                            break;
                        case Gestion.Modificar:
                            ddlAfiliado.Enabled = true;
                            break;
                        case Gestion.Consultar:
                        case Gestion.Autorizar:
                            ddlAfiliado.Enabled = false;
                            break;
                        default:
                            break;
                    }
                }
                else if (GestionControl == Gestion.Agregar)
                {
                    ddlAfiliado.Enabled = true;
                }
            }
        }
        private void IniciarGrillaAvales()
        {
            EleListasEleccionesAvales item;
            if (this.MiListaEleccion.Avales.Count > 0)
            {
                this.MiListaEleccion.Avales = new List<EleListasEleccionesAvales>();
            }
            for (int i = 0; i < 1; i++)
            {
                item = new EleListasEleccionesAvales();
                item.EstadoColeccion = EstadoColecciones.Agregado;
                this.MiListaEleccion.Avales.Add(item);
                item.IndiceColeccion = this.MiListaEleccion.Avales.IndexOf(item);
            }
            AyudaProgramacion.CargarGrillaListas<EleListasEleccionesAvales>(this.MiListaEleccion.Avales, false, this.gvAvales, true);
        }
        private void PersistirDatosGrillaAvales()
        {
            if (this.MiListaEleccion.Avales.Count == 0)
                return;

            foreach (GridViewRow fila in this.gvAvales.Rows)
            {
                HiddenField hdfIdAfiliado = (HiddenField)fila.FindControl("hdfIdAfiliadoAval");
                HiddenField hdfAfiliado = (HiddenField)fila.FindControl("hdfAfiliadoAval");

                if (hdfIdAfiliado.Value != string.Empty && Convert.ToInt32(hdfIdAfiliado.Value) > 0)
                {
                    this.MiListaEleccion.Avales[fila.RowIndex].Estado.IdEstado = (int)Estados.Activo;
                    if (this.MiListaEleccion.Avales[fila.RowIndex].IdListaEleccionAval > 0 && this.MiListaEleccion.Avales[fila.RowIndex].IdAfiliado != Convert.ToInt32(hdfIdAfiliado.Value))
                    {
                        this.MiListaEleccion.Avales[fila.RowIndex].EstadoColeccion = EstadoColecciones.Modificado;
                    }
                    this.MiListaEleccion.Avales[fila.RowIndex].IdAfiliado = Convert.ToInt32(hdfIdAfiliado.Value);
                    this.MiListaEleccion.Avales[fila.RowIndex].Afiliado = hdfAfiliado.Value;
                }
            }
        }
        protected void btnAgregarItemAvales_Click(object sender, EventArgs e)
        {
            this.PersistirDatosGrillaAvales();
            this.AgregarItemAvales();
        }
        private void AgregarItemAvales()
        {
            EleListasEleccionesAvales item;
            if (this.txtCantidadAgregarAvales.Text == string.Empty || txtCantidadAgregarAvales.Text == "0")
            {
                txtCantidadAgregarAvales.Text = "1";
            }
            int cantidad = Convert.ToInt32(this.txtCantidadAgregarAvales.Text);
            for (int i = 0; i < cantidad; i++)
            {
                item = new EleListasEleccionesAvales();
                this.MiListaEleccion.Avales.Add(item);
                item.IndiceColeccion = this.MiListaEleccion.Avales.IndexOf(item);
                item.IdListaEleccionAval = item.IndiceColeccion * -1;
                item.EstadoColeccion = EstadoColecciones.Agregado;
            }
            AyudaProgramacion.CargarGrillaListas<EleListasEleccionesAvales>(this.MiListaEleccion.Avales, true, this.gvAvales, true);
        }
        #endregion
    }
}