using Afiliados;
using Afiliados.Entidades;
using Comunes.Entidades;
using Elecciones;
using Elecciones.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;
using Reportes.FachadaNegocio;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Elecciones.Controles
{
    public partial class EleccionesVotacionDatos : ControlesSeguros
    {
        private DataTable MisListasNacionales
        {
            get { return (DataTable)Session[this.MiSessionPagina + "EleccionesListarMisListasNacionales"]; }
            set { Session[this.MiSessionPagina + "EleccionesListarMisListasNacionales"] = value; }
        }
        private DataTable MisListasRegionales
        {
            get { return (DataTable)Session[this.MiSessionPagina + "EleccionesListarMisListasRegionales"]; }
            set { Session[this.MiSessionPagina + "EleccionesListarMisListasRegionales"] = value; }
        }
        private DataTable MisListasRepresentantes
        {
            get { return (DataTable)Session[this.MiSessionPagina + "EleccionesMisListasRepresentantes"]; }
            set { Session[this.MiSessionPagina + "EleccionesMisListasRepresentantes"] = value; }
        }
        private EleElecciones MiEleccionVotos
        {
            get { return (EleElecciones)Session[this.MiSessionPagina + "EleccionesListarMisEleccionesVotos"]; }
            set { Session[this.MiSessionPagina + "EleccionesListarMisEleccionesVotos"] = value; }
        }

        private AfiAfiliados MiVotante
        {
            get { return (AfiAfiliados)Session[this.MiSessionPagina + "EleccionesMiVotante"]; }
            set { Session[this.MiSessionPagina + "EleccionesMiVotante"] = value; }
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
        public void IniciarControl(EleElecciones pParametro, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiEleccionVotos = pParametro;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiEleccionVotos = EleccionesF.EleccionesObtenerEleccionVigente(this.MiEleccionVotos);
                    if (this.MiEleccionVotos.IdEleccion > 0)
                    {
                        ScriptManager.RegisterStartupScript(this.upAfiliado, this.upAfiliado.GetType(), "MostrarTituloEleccionVigente", string.Format("MostrarTituloEleccionVigente('{0}');", this.MiEleccionVotos.Eleccion), true);
                        this.upAfiliado.Update();
                    }
                    break;
                case Gestion.ConfirmarAgregar:
                    this.btnConfirmar.Visible = false;
                    this.btnAceptarConfirmar.Visible = true;
                    this.gvListasNacionales.Columns[this.gvListasNacionales.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.gvListasRegionales.Columns[this.gvListasRegionales.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.gvListasRepresentantes.Columns[this.gvListasRepresentantes.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    this.gvListasRepresentantes.Columns[this.gvListasRepresentantes.Columns.Count - 1].Visible = false; //ELIMINO COLUMNA ACCIONES
                    CargarListaNacionalVotada(this.MisListasNacionales);//EN ESTA GESTION LA LISTA YA QUEDO CON LO QUE SELECCIONO ANTERIORMENTE
                    CargarListaRegionalVotada(this.MisListasRegionales);//EN ESTA GESTION LA LISTA YA QUEDO CON LO QUE SELECCIONO ANTERIORMENTE
                    CargarListaRepresentantesVotada(this.MisListasRepresentantes);//EN ESTA GESTION LA LISTA YA QUEDO CON LO QUE SELECCIONO ANTERIORMENTE
                    break;
                default:
                    break;
            }
        }
        private void MapearControlesAObjeto()
        {
            this.PersistirDatosGrillaNacional();
            this.PersistirDatosGrillaRegional();
            this.PersistirDatosGrillaRepresentantes();
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Elecciones/EleccionesVotar.aspx"), true);
        }
        protected void btnAceptarConfirmar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            this.btnAceptarConfirmar.Visible = false;
            this.MiEleccionVotos.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.ConfirmarAgregar:
                    guardo = EleccionesF.EleccionesVotosAgregar(this.MiEleccionVotos);
                    if (guardo)
                    {
                        this.btnImprimir.Visible = true;
                        this.MostrarMensaje(this.MiEleccionVotos.CodigoMensaje, false);
                    }
                    break;
                default:
                    break;
            }
            if (!guardo)
            {
                this.btnAceptarConfirmar.Visible = true;
                this.MostrarMensaje(this.MiEleccionVotos.CodigoMensaje, true, this.MiEleccionVotos.CodigoMensajeArgs);
                if (this.MiEleccionVotos.dsResultado != null)
                {
                    this.MiEleccionVotos.dsResultado = null;
                }
            }
        }
        /// <summary>
        /// "BOTON CONFIRMAR" llamara a un sp de validacion que valide los votos del usuario y llamara nuevamente al IniciarControl con la gestion ConfirmarAgregar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnConfirmar_Click(object sender, EventArgs e)
        {
            MapearControlesAObjeto();
            bool ok = false;
            this.btnConfirmar.Visible = false;
            ok = EleccionesF.EleccionesValidarVotacion(this.MiEleccionVotos);

            if (ok)
            {
                this.IniciarControl(this.MiEleccionVotos, Gestion.ConfirmarAgregar);
                this.ddlAfiliado.Enabled = false;
                this.upAfiliado.Update();
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "MostrarTituloVotacion", "MostrarTituloVotacion();", true);
            }
            else
            {
                this.MostrarMensaje(this.MiEleccionVotos.CodigoMensaje, false);
                this.btnConfirmar.Visible = true;
            }
        }
        #region GRILLA NACIONALES
        protected void gvListasNacionales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            EleListasElecciones aux = new EleListasElecciones();

            aux.IdAfiliado = idAfiliado;
            List<EleListasElecciones> representantes = EleccionesF.ListasEleccionesObtenerListasNacionales(aux);
            aux = representantes.Find(x => x.IdListaEleccion == id);
            if (aux != null)
            {
                DataTable rep = EleccionesF.ListasEleccionesObtenerRepresentantesPopUp(aux);
                aux.dsResultado = new DataSet();
                aux.dsResultado.Tables.Add(rep);
                this.ctrPopUpGrilla.IniciarControl(aux, this.MiEleccionVotos.Eleccion, aux.Lista);
            }
            this.upListaGrilla.Update();
        }
        protected void gvListasNacionales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                DataRowView dr = (DataRowView)e.Row.DataItem;
                string idListaEleccion = gvListasNacionales.DataKeys[e.Row.DataItemIndex].Value.ToString();
                ImageButton ibtnExpandirLista = (ImageButton)e.Row.FindControl("btnExpandirLista");
                ibtnExpandirLista.Visible = true;
                if (GestionControl == Gestion.Agregar)
                {
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Checked = false;
                }
                else
                {
                    ibtnConsultar.Visible = false;
                }
                if (this.MisListasNacionales.AsEnumerable()
                            .Where(row => row.Field<int>("IdListaEleccion") == Convert.ToInt32(idListaEleccion)).Count() > 0)
                {
                    DataTable tblFiltered = this.MisListasNacionales.AsEnumerable()
                    .Where(row => row.Field<int>("IdListaEleccion") == Convert.ToInt32(idListaEleccion))
                    .CopyToDataTable();

                    string codigo = "";
                    Literal detalles = e.Row.FindControl("ltlDetalleCampos") as Literal;

                    codigo = string.Concat(codigo, "<table>");
                    foreach (DataRow r in tblFiltered.Rows)
                    {
                        codigo = string.Concat(codigo, string.Format("<tr><td>{0}</td></tr><tr><td>{1}</td></tr>", r["Detalle"].ToString(), r["Detalle2"].ToString()));
                    }
                    codigo = string.Concat(codigo, "</table>");
                    detalles.Text = codigo;
                }
            }
        }
        private void CargarListaNacional(EleListasElecciones pParametro)
        {
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<EleListasElecciones>(pParametro);

            this.MisListasNacionales = EleccionesF.ListasEleccionesObtenerListasNacionalesDT(pParametro);
            this.gvListasNacionales.DataSource = this.MisListasNacionales;
            this.gvListasNacionales.DataBind();
            AyudaProgramacion.FixGridView(gvListasNacionales);
        }
        private void CargarListaNacionalVotada(DataTable pParametro)
        {
            this.MisListasNacionales = pParametro;
            this.gvListasNacionales.DataSource = this.MisListasNacionales;
            this.gvListasNacionales.DataBind();
            AyudaProgramacion.FixGridView(gvListasNacionales);
            this.upListasNacionales.Update();
        }
        private void PersistirDatosGrillaNacional()
        {
            int index = 0;
            if (this.MisListasNacionales.Rows.Count == 0)
                return;

            List<EleListasElecciones> aux = EleccionesF.ListasEleccionesObtenerListasNacionales(new EleListasElecciones());

            foreach (GridViewRow fila in this.gvListasNacionales.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                if (chkIncluir.Checked)
                {
                    int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
                    AfiAfiliados votante = new AfiAfiliados();
                    votante.IdAfiliado = idAfiliado;
                    votante = AfiliadosF.AfiliadosObtenerDatos(votante);

                    EleEleccionesVotos voto = new EleEleccionesVotos();
                    voto.IdAfiliado = idAfiliado;
                    voto.IdListaEleccion = aux[index].IdListaEleccion;
                    voto.IdEstado = (int)Estados.Activo;
                    voto.IdRegion = aux[index].IdTipoRegion;

                    MiEleccionVotos.Votos.Add(voto);
                }
                else
                {
                    this.MisListasNacionales.Rows.RemoveAt(index);
                    aux.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }
        #endregion
        #region GRILLA Regionales
        protected void gvListasRegionales_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            EleListasElecciones aux = new EleListasElecciones();

            aux.IdAfiliado = idAfiliado;
            List<EleListasElecciones> representantes = EleccionesF.ListasEleccionesObtenerListasRegionales(aux);
            aux = representantes.Find(x => x.IdListaEleccion == id);
            if (aux != null)
            {
                DataTable rep = EleccionesF.ListasEleccionesObtenerRepresentantesPopUp(aux);
                aux.dsResultado = new DataSet();
                aux.dsResultado.Tables.Add(rep);
                this.ctrPopUpGrilla.IniciarControl(aux, this.MiEleccionVotos.Eleccion, aux.Lista);
            }
            this.upListaGrilla.Update();
        }
        protected void gvListasRegionales_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                DataRowView dr = (DataRowView)e.Row.DataItem;
                string idListaEleccion = gvListasRegionales.DataKeys[e.Row.DataItemIndex].Value.ToString();
                ImageButton ibtnExpandirLista = (ImageButton)e.Row.FindControl("btnExpandirLista");
                ibtnExpandirLista.Visible = true;
                if (GestionControl == Gestion.Agregar)
                {
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Checked = false;
                }
                else
                {
                    ibtnConsultar.Visible = false;
                }

                if (this.MisListasRegionales.AsEnumerable()
                    .Where(row => row.Field<int>("IdListaEleccion") == Convert.ToInt32(idListaEleccion)).Count() > 0)
                {
                    DataTable tblFiltered = this.MisListasRegionales.AsEnumerable()
                      .Where(row => row.Field<int>("IdListaEleccion") == Convert.ToInt32(idListaEleccion))
                    .CopyToDataTable();

                    string codigo = "";
                    Literal detalles = e.Row.FindControl("ltlDetalleCampos") as Literal;

                    codigo = string.Concat(codigo, "<table>");
                    foreach (DataRow r in tblFiltered.Rows)
                    {
                        codigo = string.Concat(codigo, string.Format("<tr><td>{0}</td></tr><tr><td>{1}</td></tr>", r["Detalle"].ToString(), r["Detalle2"].ToString()));
                        if (!string.IsNullOrEmpty(r["Detalle3"].ToString()))//SIGNIFICA QUE TIENE LA LISTA ASOCIADA Y VA A TENER DETALLE3 Y DETALLE4 (PRESIDENTE TITULAR 1 Y 2)
                        {
                            codigo = codigo + ("<tr><td></td></tr>");
                            codigo = string.Concat(codigo, string.Format("<tr><td>{0}</td></tr><tr><td>{1}</td></tr>", r["Detalle3"].ToString(), r["Detalle4"].ToString()));
                        }
                    }
                    codigo = string.Concat(codigo, "</table>");
                    detalles.Text = codigo;
                }
            }
        }
        private void CargarListaRegional(EleListasElecciones pParametro)
        {
            int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            pParametro.IdAfiliado = idAfiliado;

            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<EleListasElecciones>(pParametro);

            this.MisListasRegionales = EleccionesF.ListasEleccionesObtenerListasRegionalesDT(pParametro);
            this.gvListasRegionales.DataSource = this.MisListasRegionales;
            this.gvListasRegionales.DataBind();
            AyudaProgramacion.FixGridView(gvListasRegionales);
        }
        private void CargarListaRegionalVotada(DataTable pParametro)
        {
            this.MisListasRegionales = pParametro;
            this.gvListasRegionales.DataSource = this.MisListasRegionales;
            this.gvListasRegionales.DataBind();
            AyudaProgramacion.FixGridView(gvListasRegionales);
            this.upListasRegionales.Update();
        }
        private void PersistirDatosGrillaRegional()
        {
            int index = 0;
            if (this.MisListasRegionales.Rows.Count == 0)
                return;

            EleListasElecciones lista = new EleListasElecciones();
            lista.IdAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            List<EleListasElecciones> aux = EleccionesF.ListasEleccionesObtenerListasRegionales(lista);

            foreach (GridViewRow fila in this.gvListasRegionales.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                if (chkIncluir.Checked)
                {
                    int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
                    AfiAfiliados votante = new AfiAfiliados();
                    votante.IdAfiliado = idAfiliado;
                    votante = AfiliadosF.AfiliadosObtenerDatos(votante);

                    EleEleccionesVotos voto = new EleEleccionesVotos();
                    voto.IdAfiliado = idAfiliado;
                    voto.IdListaEleccion = aux[index].IdListaEleccion;
                    voto.IdEstado = (int)Estados.Activo;
                    voto.IdRegion = aux[index].IdTipoRegion;

                    MiEleccionVotos.Votos.Add(voto);
                }
                else
                {
                    this.MisListasRegionales.Rows.RemoveAt(index);
                    aux.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }
        #endregion  
        #region GRILLA  REPRESENTANTES
        protected void gvListasRepresentantes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int id = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            EleListasElecciones aux = new EleListasElecciones();

            aux.IdAfiliado = idAfiliado;
            List<EleListasElecciones> representantes = EleccionesF.ListasEleccionesObtenerListasRepresentantes(aux);
            aux = representantes.Find(x => x.IdListaEleccion == id);
            if (aux != null)
            {
                DataTable rep = EleccionesF.ListasEleccionesObtenerRepresentantesPopUp(aux);
                aux.dsResultado = new DataSet();
                aux.dsResultado.Tables.Add(rep);
                this.ctrPopUpGrilla.IniciarControl(aux,this.MiEleccionVotos.Eleccion,aux.Lista);
            }
            this.upListaGrilla.Update();
        }
        protected void gvListasRepresentantes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox ibtnConsultar = (CheckBox)e.Row.FindControl("chkIncluir");
                string idListaEleccion = gvListasRepresentantes.DataKeys[e.Row.DataItemIndex].Value.ToString();
                DataRowView dr = (DataRowView)e.Row.DataItem;
                ImageButton ibtnExpandirLista = (ImageButton)e.Row.FindControl("btnExpandirLista");
                ibtnExpandirLista.Visible = true;
                if (GestionControl == Gestion.Agregar)
                {
                    ibtnConsultar.Visible = true;
                    ibtnConsultar.Checked = false;
                }
                else
                {
                    ibtnConsultar.Visible = false;
                }
                if (this.MisListasRepresentantes.AsEnumerable()
                   .Where(row => row.Field<int>("IdListaEleccion") == Convert.ToInt32(idListaEleccion)).Count() > 0)
                {
                    DataTable tblFiltered = this.MisListasRepresentantes.AsEnumerable()
                      .Where(row => row.Field<int>("IdListaEleccion") == Convert.ToInt32(idListaEleccion))
                    .CopyToDataTable();

                    string codigo = "";
                    Literal detalles = e.Row.FindControl("ltlDetalleCampos") as Literal;

                    codigo = string.Concat(codigo, "<table>");
                    foreach (DataRow r in tblFiltered.Rows)
                    {
                        codigo = string.Concat(codigo, string.Format("<tr><td>{0}</td></tr><tr><td>{1}</td></tr>", r["Detalle"].ToString(), r["Detalle2"].ToString()));
                    }
                    codigo = string.Concat(codigo, "</table>");
                    detalles.Text = codigo;
                }
            }
        }
        private void CargarListaRepresentantes(EleListasElecciones pParametro)
        {
            int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            pParametro.IdAfiliado = idAfiliado;
            pParametro.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            pParametro.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<EleListasElecciones>(pParametro);

            this.MisListasRepresentantes = EleccionesF.ListasEleccionesObtenerListasRepresentantesDT(pParametro);
            this.gvListasRepresentantes.DataSource = this.MisListasRepresentantes;
            this.gvListasRepresentantes.DataBind();
            AyudaProgramacion.FixGridView(gvListasRepresentantes);
        }
        private void CargarListaRepresentantesVotada(DataTable pParametro)
        {
            this.MisListasRepresentantes = pParametro;
            this.gvListasRepresentantes.DataSource = this.MisListasRepresentantes;
            this.gvListasRepresentantes.DataBind();
            AyudaProgramacion.FixGridView(gvListasRepresentantes);
            this.upListasRepresentantes.Update();
        }
        private void PersistirDatosGrillaRepresentantes()
        {
            int index = 0;
            if (this.MisListasRepresentantes.Rows.Count == 0)
                return;

            EleListasElecciones lista = new EleListasElecciones();
            lista.IdAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
            List<EleListasElecciones> aux = EleccionesF.ListasEleccionesObtenerListasRepresentantes(lista);

            foreach (GridViewRow fila in this.gvListasRepresentantes.Rows)
            {
                CheckBox chkIncluir = (CheckBox)fila.FindControl("chkIncluir");
                if (chkIncluir.Checked)
                {
                    int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
                    AfiAfiliados votante = new AfiAfiliados();
                    votante.IdAfiliado = idAfiliado;
                    votante = AfiliadosF.AfiliadosObtenerDatos(votante);

                    EleEleccionesVotos voto = new EleEleccionesVotos();
                    voto.IdAfiliado = idAfiliado;
                    voto.IdListaEleccion = aux[index].IdListaEleccion;
                    voto.IdEstado = (int)Estados.Activo;
                    voto.IdRegion = aux[index].IdTipoRegion;

                    MiEleccionVotos.Votos.Add(voto);
                }
                else
                {
                    this.MisListasRepresentantes.Rows.RemoveAt(index);
                    aux.RemoveAt(index);
                    index--;
                }
                index++;
            }
        }
        #endregion
        protected void button_Click(object sender, EventArgs e)
        {
            if (this.MiEleccionVotos.IdEleccion > 0)
            {
                ScriptManager.RegisterStartupScript(this.upAfiliado, this.upAfiliado.GetType(), "MostrarTituloEleccionVigente", string.Format("MostrarTituloEleccionVigente('{0}');", this.MiEleccionVotos.Eleccion), true);
                this.upAfiliado.Update();
            }
            if (!string.IsNullOrEmpty(this.hdfIdAfiliado.Value))
            {
                bool ok = true;
                int idAfiliado = Convert.ToInt32(this.hdfIdAfiliado.Value);
                AfiAfiliados aux = new AfiAfiliados();
                aux.IdAfiliado = idAfiliado;
                aux = AfiliadosF.AfiliadosObtenerDatos(aux);
                if (aux != null)
                {
                    this.MiVotante = aux;
                    this.CargarDatosAfiliado();
                }
                this.MiEleccionVotos.IdVotante = idAfiliado;
                if (GestionControl == Gestion.Agregar)
                    ok = EleccionesF.EleccionesValidarVotacion(this.MiEleccionVotos);
                else
                    ok = EleccionesF.EleccionesValidarVotacionConfirmar(this.MiEleccionVotos);

                if (ok)
                {
                    CargarListaNacional(new EleListasElecciones());
                    CargarListaRegional(new EleListasElecciones());
                    CargarListaRepresentantes(new EleListasElecciones());

                    this.ddlAfiliado.Items.Add(new ListItem(this.hdfAfiliado.Value, this.hdfIdAfiliado.Value));
                    this.ddlAfiliado.SelectedValue = this.hdfIdAfiliado.Value;
                    this.upAfiliado.Update();

                    this.upListasNacionales.Update();
                    this.upListasRegionales.Update();
                    this.upListasRepresentantes.Update();
                    this.upListasRepresentantes.Update();
                    this.btnConfirmar.Visible = true;
                    this.upBotones.Update();
                }
                else
                {
                    this.MostrarMensaje(this.MiEleccionVotos.CodigoMensaje, true);
                    this.ddlAfiliado.Items.Add(new ListItem(this.hdfAfiliado.Value, this.hdfIdAfiliado.Value));
                    this.ddlAfiliado.SelectedValue = this.hdfIdAfiliado.Value;
                    this.upAfiliado.Update();
                    this.btnConfirmar.Visible = false;
                    this.upBotones.Update();
                }
            }
            else
            {
                this.LimpiarGrillas();
                this.LimpiarDatosAfiliado();
                this.ddlAfiliado.Items.Add(new ListItem("Ingrese el apellido, DNI o nro. de socio", ""));
                this.ddlAfiliado.SelectedValue = "";

            }
        }
        private void LimpiarDatosAfiliado()
        {
            this.txtIdAfiliado.Text = "";
            this.txtTipoDocumento.Text = "";
            this.txtNumeroDocumento.Text = "";
            this.txtNumeroSocio.Text = "";
            this.txtCategoria.Text = "";
            this.txtEstado.Text = "";
            this.txtFilial.Text = "";
            this.txtFechaNacimiento.Text = "";
            this.upAfiliado.Update();
        }
        private void LimpiarGrillas()
        {
            this.gvListasNacionales.DataSource = null;
            this.gvListasRegionales.DataSource = null;
            this.gvListasRepresentantes.DataSource = null;

            this.gvListasNacionales.DataBind();
            this.gvListasRegionales.DataBind();
            this.gvListasRepresentantes.DataBind();

            this.upListasNacionales.Update();
            this.upListasRegionales.Update();
            this.upListasRepresentantes.Update();
        }
        protected void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                RepReportes reporte = new RepReportes();
                string parametro = this.hdfIdAfiliado.Value;

                RepParametros param = new RepParametros();
                param.TipoParametro.IdTipoParametro = (int)EnumRepTipoParametros.TextBox;
                param.Parametro = "IdAfiliado";
                param.ValorParametro = parametro;


                TGEPlantillas plantilla = new TGEPlantillas();
                plantilla.Codigo = "EleccionesVotos";
                plantilla = TGEGeneralesF.PlantillasObtenerDatosCompletosPorCodigo(plantilla);

                reporte.StoredProcedure = plantilla.NombreSP;
                reporte.Parametros.Add(param);

                DataSet ds = ReportesF.ReportesObtenerDatos(reporte);

                byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdEleccionVoto", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                //byte[] pdf = ReportesF.ExportPDFGenerarReportePDF(plantilla, ds, "IdAfiliado", AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo));
                ExportPDF.ExportarPDF(pdf, this.Page, "EleccionVotos_" + this.UsuarioActivo.ApellidoNombre, this.UsuarioActivo);
                //upComprobantes.Update();
            }
            catch (Exception ex)
            {
                this.MostrarMensaje("No se pudo imprimir el comprobante.", true);
            }
        }
        private void CargarDatosAfiliado()
        {
            this.txtIdAfiliado.Text = this.MiVotante.IdAfiliado.ToString();
            this.txtTipoDocumento.Text = this.MiVotante.TipoDocumento.TipoDocumento.ToString();
            this.txtNumeroDocumento.Text = this.MiVotante.NumeroDocumento.ToString();
            this.txtNumeroSocio.Text = this.MiVotante.NumeroSocio.ToString();
            this.txtCategoria.Text = this.MiVotante.Categoria.Categoria.ToString();
            this.txtEstado.Text = this.MiVotante.Estado.Descripcion.ToString();
            this.txtFilial.Text = this.MiVotante.Filial.Filial.ToString();
            this.txtFechaNacimiento.Text = this.MiVotante.FechaNacimiento.HasValue == true ? Convert.ToDateTime(this.MiVotante.FechaNacimiento).ToShortDateString() : "";
            this.upAfiliado.Update();
        }
    }
}