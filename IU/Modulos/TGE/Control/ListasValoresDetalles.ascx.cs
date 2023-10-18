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
using Generales.Entidades;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using Seguridad.Entidades;

namespace IU.Modulos.TGE.Control
{
    public partial class ListasValoresDetalles : ControlesSeguros
    {
        private TGEListasValores MiListaValor
        {
            get
            {
                return (Session[this.MiSessionPagina + "ListasValoresDetallesMiListaValor"] == null ?
                    (TGEListasValores)(Session[this.MiSessionPagina + "ListasValoresDetallesMiListaValor"] = new TGEListasValores()) : (TGEListasValores)Session[this.MiSessionPagina + "ListasValoresDetallesMiListaValor"]);
            }
            set { Session[this.MiSessionPagina + "ListasValoresDetallesMiListaValor"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "ListasValoresDetallesMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "ListasValoresDetallesMiIndiceDetalleModificar"] = value; }
        }

        public delegate void ListasValoresDatosAceptarEventHandler(object sender, TGEListasValores e);
        public event ListasValoresDatosAceptarEventHandler ListasValoresDatosAceptar;
        public delegate void ListasValoresDatosCancelarEventHandler();
        public event ListasValoresDatosCancelarEventHandler ListasValoresDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            ctrDatosPopUp.ListasValoresDetallesPopUpAceptar += new ListasValoresDetallesDatosPopUp.ListasValoresDetallesDatosPopUpAceptarEventHandler(ctrDatosPopUp_ListasValoresDetallesPopUpAceptar);
            ctrDatosPopUp.ListasValoresDetallesPopUpCancelar += CtrDatosPopUp_ListasValoresDetallesPopUpCancelar;
            popUpMensajes.popUpMensajesPostBackAceptar +=new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
        }

        private void CtrDatosPopUp_ListasValoresDetallesPopUpCancelar()
        {
            MultiView1.SetActiveView(vwLista);
            this.txtListaValor.Enabled = true;
            this.txtCodigoValor.Enabled = true;
            this.ddlDepende.Enabled = true;
        }

        void ctrDatosPopUp_ListasValoresDetallesPopUpAceptar(TGEListasValoresDetalles e, Gestion pGestion)
        {
            
            bool guardo = false;
            switch (pGestion)
            {
                case Gestion.Agregar:
                    guardo = TGEGeneralesF.ListasValoresItemAgregar(e);
                    this.MiListaValor.ListasValoresDetalles.Add(e);
                    e.IndiceColeccion = this.MiListaValor.ListasValoresDetalles.IndexOf(e);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.ListasValoresItemModificar(e);
                    this.MiListaValor.ListasValoresDetalles[this.MiIndiceDetalleModificar] = e;
            break;
            }

            AyudaProgramacion.CargarGrillaListas(this.MiListaValor.ListasValoresDetalles, true, this.gvParametrosValores, true);
            this.upParametrosValores.Update();
            MultiView1.SetActiveView(vwLista);
        }

        public void IniciarControl(TGEListasValores pParametro, Gestion pGestion)
        {
            
            this.MiListaValor = pParametro;
            this.GestionControl = pGestion;
            this.CargarCombos();
            this.MiListaValor.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.btnAgregar.Visible = this.ValidarPermiso("ListasValoresDetallesAgregar.aspx");
                    this.txtListaValor.Enabled = true;
                    this.txtCodigoValor.Enabled = this.UsuarioActivo.EsAdministradorGeneral;
                    this.btnAgregar.Visible = false;
                    this.btnExportarExcel.Visible = false;
                    this.ddlDepende.Enabled = true;
                    MultiView1.SetActiveView(vwLista);
                    break;
                case Gestion.Modificar:
                    this.MiListaValor = TGEGeneralesF.ListasValoresObtenerDatosCompletos(pParametro);
                    this.MapearObjetoControles();
                    this.txtListaValor.Enabled = true;
                    this.txtCodigoValor.Enabled =  this.UsuarioActivo.EsAdministradorGeneral;
                    this.ddlDepende.Enabled = true;
                    this.btnAgregar.Visible = this.ValidarPermiso("ListasValoresDetallesAgregar.aspx");
                    MultiView1.SetActiveView(vwLista);
                    break;
                case Gestion.Consultar:
                    this.MiListaValor = TGEGeneralesF.ListasValoresObtenerDatosCompletos(pParametro);
                    this.MapearObjetoControles();
                    this.btnAceptar.Visible = false;
                    this.btnAgregar.Visible = false;
                    MultiView1.SetActiveView(vwLista);
                    break;
                default:
                    break;
            }
        }

        private void CargarCombos()
        {
            this.ddlDepende.DataSource = TGEGeneralesF.ListasValoresObtenerListaRecursiva();
            this.ddlDepende.DataValueField = "IdListaValor";
            this.ddlDepende.DataTextField = "ListaValor";
            this.ddlDepende.DataBind();
            if (this.ddlDepende.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlDepende, this.ObtenerMensajeSistema("SeleccioneOpcion"));

        }

        private void MapearObjetoControles()
        {
            this.txtListaValor.Text = this.MiListaValor.ListaValor;
            this.txtCodigoValor.Text = this.MiListaValor.CodigoValor;

            this.ddlDepende.SelectedValue = this.MiListaValor.IdRefListaValor.ToString();
            ListItem item = this.ddlDepende.Items.FindByValue(this.MiListaValor.IdRefListaValor.ToString());
            if (item == null)
                this.ddlDepende.Items.Add(new ListItem(this.MiListaValor.DescripcionRef, this.MiListaValor.IdRefListaValor.ToString()));
            this.ddlDepende.SelectedValue = this.MiListaValor.IdRefListaValor.ToString();

            AyudaProgramacion.CargarGrillaListas(this.MiListaValor.ListasValoresDetalles, true, this.gvParametrosValores, true);
        }

        private void MapearControlesObjeto()
        {
            this.MiListaValor.ListaValor = this.txtListaValor.Text;
            this.MiListaValor.CodigoValor = this.txtCodigoValor.Text;
            this.MiListaValor.IdRefListaValor = ddlDepende.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlDepende.SelectedValue); 
            this.MiListaValor.DescripcionRef = this.ddlDepende.SelectedItem.Text;
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            TGEListasValoresDetalles param = new TGEListasValoresDetalles();
            this.MiListaValor.ListaValor = this.txtListaValor.Text;
            param.ListaValor = this.MiListaValor;
            this.ctrDatosPopUp.IniciarControl(param, MiListaValor, Gestion.Agregar);
            MultiView1.SetActiveView(vwValoresItem);
            this.txtListaValor.Enabled = false;
            this.txtCodigoValor.Enabled = false;
            this.ddlDepende.Enabled = false;
        }

        protected void btnExportarExcel_Click(object sender, ImageClickEventArgs e)
        {
            this.gvParametrosValores.AllowPaging = false;
            this.gvParametrosValores.DataSource = this.MiListaValor.ListasValoresDetalles;
            this.gvParametrosValores.DataBind();
            GridViewExportUtil.Export("Datos.xls", this.gvParametrosValores);
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("ListasValoresDatosAceptar");
            if (!this.Page.IsValid)
                return;

            bool guardo = false;
            this.MapearControlesObjeto();
            this.MiListaValor.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    guardo = TGEGeneralesF.ListasValoresAgregar(this.MiListaValor);
                    break;
                case Gestion.Modificar:
                    guardo = TGEGeneralesF.ListasValoresModificar(this.MiListaValor);
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                this.btnAceptar.Visible = false;
                this.btnAgregar.Visible = false;
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiListaValor.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiListaValor.CodigoMensaje, true, this.MiListaValor.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ListasValoresDatosCancelar != null)
                this.ListasValoresDatosCancelar();
        }

        protected void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.ListasValoresDatosAceptar != null)
                this.ListasValoresDatosAceptar(null, this.MiListaValor);
        }

        protected void gvParametrosValores_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (!(e.CommandName == Gestion.Consultar.ToString()
                || e.CommandName==Gestion.Modificar.ToString()))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;
            this.MiListaValor.ListasValoresDetalles[indiceColeccion].ListaValor = this.MiListaValor;

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrDatosPopUp.IniciarControl(this.MiListaValor.ListasValoresDetalles[indiceColeccion], MiListaValor, Gestion.Modificar);
                MultiView1.SetActiveView(vwValoresItem);
                this.txtListaValor.Enabled = false;
                this.txtCodigoValor.Enabled = false;
                this.ddlDepende.Enabled = false;
            }
            //else if (e.CommandName == Gestion.Consultar.ToString())
            //{
            //    this.ctrDatosPopUp.IniciarControl(this.MiListaValor.ListasValoresDetalles[indiceColeccion], Gestion.Consultar);
            //}
        }

        protected void gvParametrosValores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                //string mensaje = this.ObtenerMensajeSistema("ParametroValorConfirmarBaja");
                //string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                //ibtn.Attributes.Add("OnClick", funcion);
                //ibtn.Visible = true;
                if (this.GestionControl == Gestion.Agregar
                    || this.GestionControl == Gestion.Modificar)
                {
                    ImageButton ibtnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                    ibtnModificar.Visible = this.ValidarPermiso("ListasValoresDetallesModificar.aspx");
                }

                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    int cellCount = e.Row.Cells.Count;
                    e.Row.Cells.Clear();
                    TableCell tableCell = new TableCell();
                    tableCell.ColumnSpan = cellCount;
                    tableCell.HorizontalAlign = HorizontalAlign.Right;
                    tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MiListaValor.ListasValoresDetalles.Count);
                    e.Row.Cells.Add(tableCell);
                }
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            TGEListasValoresDetalles parametros = this.BusquedaParametrosObtenerValor<TGEListasValoresDetalles>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<TGEListasValoresDetalles>(parametros);

            this.gvParametrosValores.PageIndex = e.NewPageIndex;
            this.gvParametrosValores.DataSource = this.MiListaValor.ListasValoresDetalles;
            this.gvParametrosValores.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MiListaValor.ListasValoresDetalles = this.OrdenarGrillaDatos<TGEListasValoresDetalles>(this.MiListaValor.ListasValoresDetalles, e);
            this.gvParametrosValores.DataSource = this.MiListaValor.ListasValoresDetalles;
            this.gvParametrosValores.DataBind();
        }
    }
}