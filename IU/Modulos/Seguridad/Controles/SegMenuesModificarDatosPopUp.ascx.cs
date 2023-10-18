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
using Comunes.Entidades;
using System.Collections.Generic;
using Seguridad.FachadaNegocio;
using Generales.FachadaNegocio;
using System.IO;

namespace IU.Modulos.Seguridad.Controles
{
    public partial class SegMenuesModificarDatosPopUp : ControlesSeguros
    {
        private Menues MiMenuItem
        {
            get { return (Menues)Session[this.MiSessionPagina + "SegMenuesModificarDatosPopUpMiMenu"]; }
            set { Session[this.MiSessionPagina + "SegMenuesModificarDatosPopUpMiMenu"] = value; }
        }

        private List<Menues> MisMenues
        {
            get { return (List<Menues>)Session[this.MiSessionPagina + "SegMenuesModificarDatosMisMenues"]; }
            set { Session[this.MiSessionPagina + "SegMenuesModificarDatosMisMenues"] = value; }
        }

        public delegate void MenuesModificarDatosPopUpEventHandler(object sender, Menues e, Gestion pGestion);
        public event MenuesModificarDatosPopUpEventHandler MenuesModificarDatosAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtMenu, btnAceptar);
            }
        }

        public void IniciarControl(Menues pMenuItem, Gestion pGestion, bool menuPrincipal, int pIdMenuPrincipal)
        {
            this.MiMenuItem = pMenuItem;
            this.CargarCombos();
            this.CargarComboMenu(menuPrincipal, pIdMenuPrincipal);
            this.GestionControl = pGestion;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    pMenuItem.BajaLogica = false;
                    break;
                case Gestion.Modificar:
                    this.MiMenuItem = SeguridadF.MenuesControlesOcultarObtenerPorIdMenu(MiMenuItem);
                    this.MapearObjetoAControles(this.MiMenuItem);
                    break;
                case Gestion.Consultar:
                    this.MiMenuItem = SeguridadF.MenuesControlesOcultarObtenerPorIdMenu(MiMenuItem);
                    this.MapearObjetoAControles(this.MiMenuItem);
                    this.btnAceptar.Visible = false;
                    break;
                default:
                    break;
            }
            if (menuPrincipal)
                this.chkMostrar.Enabled = false;
            else
                this.chkMostrar.Enabled = true;

            this.Visible = true;

            if (!UsuarioActivo.EsAdministradorGeneral)
            {
                txtURL.Enabled = false;
                txtOrden.Enabled = false;
                ddlMenuPadre.Enabled = false;
                chkMostrar.Enabled = false;
                chkBajaLogica.Enabled = false;
                dvAgregarControl.Visible = false;
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            this.Page.Validate("MenuPopUp");
            if (!this.Page.IsValid)
                return;

            this.MapearControlesAObjeto(this.MiMenuItem);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiMenuItem.EstadoColeccion = EstadoColecciones.Agregado;
                    MostrarGrillaDetalles();
                    break;
                case Gestion.Modificar:
                    this.MiMenuItem.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(this.MiMenuItem, this.GestionControl);
                    MostrarGrillaDetalles();
                    break;
                default:
                    break;
            }
            if (SeguridadF.MenuesActualizar(this.MiMenuItem))
            {
                if (this.MenuesModificarDatosAceptar != null)
                    this.MenuesModificarDatosAceptar(sender, this.MiMenuItem, this.GestionControl);
                AyudaProgramacion.LimpiarControles(this, true);
                this.Visible = false;
                //MostrarGrillaDetalles();
            }
            else
            {
                this.MostrarMensaje("ResultadoTransaccionIncorrecto", true);
                this.Visible = true;
            }
        }

        private void MapearControlesAObjeto(Menues pParametro)
        {
            pParametro.Menu = this.txtMenu.Text;
            pParametro.Orden = this.txtOrden.Text==string.Empty? 0 : Convert.ToInt32( this.txtOrden.Text);
            pParametro.URL = this.txtURL.Text;
            pParametro.BajaLogica = this.chkBajaLogica.Checked;
            pParametro.Mostrar = this.chkMostrar.Checked;
            pParametro.IdMenuPadre = Convert.ToInt32(this.ddlMenuPadre.SelectedValue);
            pParametro.IdTipoFuncionalidad = this.ddlFuncionalidades.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlFuncionalidades.SelectedValue);
            MostrarGrillaDetalles();
            //AyudaProgramacion.CargarGrillaListas<MenuesControlesOcultar>(pParametro.ControlesOcultar, false, this.gvDatos, true);
        }

        private void MapearObjetoAControles(Menues pParametro)
        {
            this.txtMenu.Text = pParametro.Menu;
            this.txtOrden.Text = pParametro.Orden.ToString();
            this.txtURL.Text = pParametro.URL;
            this.chkBajaLogica.Checked = pParametro.BajaLogica;
            this.chkHijos.Checked = pParametro.Hijos;
            this.chkMostrar.Checked = pParametro.Mostrar;
            this.ddlMenuPadre.SelectedValue = pParametro.IdMenuPadre.ToString();
            this.ddlFuncionalidades.SelectedValue = pParametro.IdTipoFuncionalidad == 0 ? string.Empty : pParametro.IdTipoFuncionalidad.ToString();
            MostrarGrillaDetalles();
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            AyudaProgramacion.LimpiarControles(this, true);
            this.Visible = false;
            //this.upGrillaControles.Visible = false;
            //this.pnlGrillaControles.Visible = false;
        }

        private void CargarCombos()
        {
            if (this.ddlFuncionalidades.Items.Count > 0)
                this.ddlFuncionalidades.Items.Clear();

            //TGEEstados estado = new TGEEstados();
            //estado.IdEstado = (int)EstadosTodos.Todos;
            this.ddlFuncionalidades.DataSource = TGEGeneralesF.TGETiposFuncionalidadesObtenerLista();
            this.ddlFuncionalidades.DataValueField = "IdTipoFuncionalidad";
            this.ddlFuncionalidades.DataTextField = "TipoFuncionalidad";
            this.ddlFuncionalidades.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlFuncionalidades, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //Control control = (Control)Page.LoadControl(string.Concat("~", "/Modulos/Afiliados/Controles/AfiliadoModificarDato.ascx"));
            //List<string> lista = AyudaProgramacion.FindControlRecursiveAttributes(control, "data-ocultar");

        }

        private void CargarComboMenu(bool menuPrincipal, int pIdMenuPrincipal)
        {
            if (this.ddlMenuPadre.Items.Count > 0)
                this.ddlMenuPadre.Items.Clear();

            if (menuPrincipal)
            {
                this.ddlMenuPadre.Items.Add(new ListItem("No aplica", "-1"));
            }
            else
            {
                Menues menPpal = this.MisMenues.First(x=>x.IdMenu==pIdMenuPrincipal);
                this.ddlMenuPadre.Items.Add(new ListItem(menPpal.Menu, menPpal.IdMenu.ToString()));
                List<Menues> menuesPrincipales = this.MisMenues.Where(x => x.IdMenuPadre == pIdMenuPrincipal).OrderBy(x => x.Orden).ToList();
                foreach (Menues men in menuesPrincipales)
                {
                    this.CargarComboMenu(men, ">>");
                }
            }
        }

        private void CargarComboMenu(Menues pMenu, string tabulador)
        {
            this.ddlMenuPadre.Items.Add(new ListItem(string.Concat(tabulador, pMenu.Menu), pMenu.IdMenu.ToString()));            
            List<Menues> menuesHijos = this.MisMenues.Where(x => x.IdMenuPadre == pMenu.IdMenu).OrderBy(x => x.Orden).ToList();
            tabulador = tabulador + ">>";
            foreach (Menues men in menuesHijos)
            {
                this.CargarComboMenu(men, tabulador);
            }
            tabulador = tabulador.Substring(0,tabulador.Length-2);
        }

        #region Grilla
        
        protected void btnAgregarGrilla_Click(object sender, EventArgs e)
        {
            if (!this.MiMenuItem.ControlesOcultar.Exists(x => x.Control == txtControl.Text))
            {
                MenuesControlesOcultar item;
                item = new MenuesControlesOcultar();
                item.IdMenu = this.MiMenuItem.IdMenu;
                item.Control = txtControl.Text;
                this.MiMenuItem.ControlesOcultar.Add(item);
                AyudaProgramacion.CargarGrillaListas<MenuesControlesOcultar>(this.MiMenuItem.ControlesOcultar, false, this.gvDatos, true);
                txtControl.Text = string.Empty;
            }
            else
            {
                MostrarMensaje("El control ya existe", true);
            }
            //MostrarGrillaDetalles();
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            string indiceColeccion = ((GridView)sender).DataKeys[index].Values["Control"].ToString();

            if (e.CommandName == "Borrar")
            {
                MenuesControlesOcultar item = this.MiMenuItem.ControlesOcultar.FirstOrDefault(x => x.Control == (indiceColeccion));
                this.MiMenuItem.ControlesOcultar.Remove(item);
                MostrarGrillaDetalles();
            }

            if (e.CommandName == Gestion.Agregar.ToString())
            {
                this.MapearObjetoAControles(this.MiMenuItem);
                this.GestionControl = Gestion.Agregar;
                this.upGrillaControles.Update();
                this.pnlGrillaControles.Visible = true;
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //MenuesControlesOcultar item = (MenuesControlesOcultar)e.Row.DataItem;
            }

            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisMenues.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        private void MostrarGrillaDetalles()
        {
            //List<MenuesControlesOcultar> listaMenues;
            AyudaProgramacion.CargarGrillaListas<MenuesControlesOcultar>(this.MiMenuItem.ControlesOcultar, false, this.gvDatos, true);
            this.upGrillaControles.Update();
        }
        #endregion

    }
}