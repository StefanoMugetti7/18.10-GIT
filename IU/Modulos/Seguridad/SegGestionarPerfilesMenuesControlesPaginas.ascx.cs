using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seguridad.Entidades;
using Comunes.Entidades;
using Seguridad.FachadaNegocio;

namespace IU.Modulos.Seguridad
{
    public partial class SegGestionarPerfilesMenuesControlesPaginas : ControlesSeguros
    {
        private Menues MiMenu
        {
            get { return (Menues)Session[this.MiSessionPagina + "SegGestionarPerfilesMenuesControlesPaginasMiMenu"]; }
            set { Session[this.MiSessionPagina + "SegGestionarPerfilesMenuesControlesPaginasMiMenu"] = value; }
        }

        private List<SegControlesPaginas> MisControlesSeguros
        {
            get { return (List<SegControlesPaginas>)Session[this.MiSessionPagina + "SegGestionarPerfilesMenuesControlesPaginasMisControlesSeguros"]; }
            set { Session[this.MiSessionPagina + "SegGestionarPerfilesMenuesControlesPaginasMisControlesSeguros"] = value; }
        }

        public delegate void SegGestionarPerfilesMenuesControlesPaginasAceptarEventHandler(object sender, Menues e, bool resultado);
        public event SegGestionarPerfilesMenuesControlesPaginasAceptarEventHandler SegGestionarPerfilesMenuesControlesPaginasAceptar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //Limpio las variables de sesion
                this.MiMenu = new Menues();
                this.MisControlesSeguros = new List<SegControlesPaginas>();
            }                
        }

        public void IniciarControl(Menues pMenu)
        {
            this.MiMenu = pMenu;
            this.MisControlesSeguros = SeguridadF.SegControlesPaginaObtenerLista();
           

            this.CargarListasControlesSeguros(this.MiMenu);
            this.mpePopUp.Show();
        }

        protected void btnPopUpAceptar_Click(object sender, EventArgs e)
        {
            this.ObtenerControlesSeleccionados(this.MiMenu);
            this.MiMenu.EstadoColeccion = EstadoColecciones.Modificado;
            this.mpePopUp.Hide();
            if (SegGestionarPerfilesMenuesControlesPaginasAceptar != null)
                SegGestionarPerfilesMenuesControlesPaginasAceptar(this, this.MiMenu, true);
        }

        //private List<SegControlesPaginas> ValidarControlesPagina(List<SegControlesPaginas> controlesPagina)
        //{
        //    List<SegControlesPaginas> listaControlesSeguros = new List<SegControlesPaginas>();
        //    System.Web.UI.Page pagina = new System.Web.UI.Page();
        //    Control controlPagina = new Control();
        //    if (controlesPagina != null)
        //    {
        //        foreach (SegControlesPaginas ctrl in controlesPagina)
        //        {
        //            if (AyudaProgramacion.ValidarControlPagina(controlPagina, ctrl.ControlesPaginas))
        //            {
        //                listaControlesSeguros.Add(ctrl);
        //            }
        //        }
        //    }
        //    return listaControlesSeguros;
        //}

        private void CargarListasControlesSeguros(Menues pMenu)
        {
            this.MisControlesSeguros = SeguridadF.SegControlesPaginaObtenerLista();
            var controles = from t in this.MisControlesSeguros
                            where !(pMenu.ControlesPaginas.Any(controlPagina => controlPagina.IdControlesPaginas == t.IdControlesPaginas))
                           select t;

            this.lbxControlesPaginas.DataSource = AyudaProgramacion.AcomodarIndices<SegControlesPaginas>(controles.ToList());
            this.lbxControlesPaginas.DataTextField = "Descripcion";
            this.lbxControlesPaginas.DataValueField = "IdControlesPaginas";
            this.lbxControlesPaginas.DataBind();

            this.lbxControlesPaginaSeleccionados.DataSource = AyudaProgramacion.AcomodarIndices<SegControlesPaginas>(pMenu.ControlesPaginas);
            this.lbxControlesPaginaSeleccionados.DataTextField = "Descripcion";
            this.lbxControlesPaginaSeleccionados.DataValueField = "IdControlesPaginas";
            this.lbxControlesPaginaSeleccionados.DataBind();
        }

        private void ObtenerControlesSeleccionados(Menues pMenu)
        {
            ListItem item;
            foreach (SegControlesPaginas ctrlSeg in this.MisControlesSeguros)
            {
                item = this.lbxControlesPaginaSeleccionados.Items.FindByValue(ctrlSeg.IdControlesPaginas.ToString());
                if (item == null)
                {
                    ctrlSeg.EstadoColeccion = EstadoColecciones.Borrado;
                    ctrlSeg.Estado.IdEstado = (int)Estados.Baja;
                }
            }

            SegControlesPaginas ctrlSeguro;
            foreach (ListItem itemSeleccionado in this.lbxControlesPaginaSeleccionados.Items)
            {
                ctrlSeguro = pMenu.ControlesPaginas.Find(delegate(SegControlesPaginas segCtrl)
                { return segCtrl.IdControlesPaginas == Convert.ToInt32(itemSeleccionado.Value); });

                if (ctrlSeguro == null)
                {
                    ctrlSeguro = new SegControlesPaginas();
                    ctrlSeguro.IdControlesPaginas = Convert.ToInt32(itemSeleccionado.Value);
                    ctrlSeguro.Descripcion = itemSeleccionado.Text;
                    ctrlSeguro.EstadoColeccion = EstadoColecciones.Agregado;
                    ctrlSeguro.Estado.IdEstado = (int)Estados.Activo;
                    pMenu.ControlesPaginas.Add(ctrlSeguro);
                }
            }
        }

        protected void btnAgregarTodos_Click(object sender, EventArgs e)
        {
            if (this.lbxControlesPaginas.Items.Count > 0)
            {
                lbxControlesPaginas.ClearSelection();
                foreach (ListItem item in this.lbxControlesPaginas.Items)
                {
                    this.lbxControlesPaginaSeleccionados.Items.Add(item);
                }
                this.lbxControlesPaginas.Items.Clear();
                //this.MiTipoDeAccion.TipoDeTramites.AddRange(this.MisTiposDeTramites);
                //this.MisTiposDeTramites.Clear();
            }
            this.mpePopUp.Show();
        }

        protected void btnAgregarUno_Click(object sender, EventArgs e)
        {
            if (this.lbxControlesPaginas.SelectedIndex >= 0)
            {
                this.lbxControlesPaginaSeleccionados.Items.Add(this.lbxControlesPaginas.SelectedItem);
                this.lbxControlesPaginas.Items.RemoveAt(this.lbxControlesPaginas.SelectedIndex);
                this.lbxControlesPaginaSeleccionados.ClearSelection();
            }
            this.mpePopUp.Show();
        }

        protected void btnBorrarUno_Click(object sender, EventArgs e)
        {
            if (this.lbxControlesPaginaSeleccionados.SelectedIndex >= 0)
            {
                this.lbxControlesPaginas.Items.Add(this.lbxControlesPaginaSeleccionados.SelectedItem);
                this.lbxControlesPaginaSeleccionados.Items.RemoveAt(this.lbxControlesPaginaSeleccionados.SelectedIndex);
                this.lbxControlesPaginas.ClearSelection();
            }
            this.mpePopUp.Show();
        }

        protected void btnBorrarTodos_Click(object sender, EventArgs e)
        {
            if (this.lbxControlesPaginaSeleccionados.Items.Count > 0)
            {
                lbxControlesPaginaSeleccionados.ClearSelection();
                foreach (ListItem item in this.lbxControlesPaginaSeleccionados.Items)
                {
                    this.lbxControlesPaginas.Items.Add(item);
                }
                this.lbxControlesPaginaSeleccionados.Items.Clear();
                //this.MisTiposDeTramites.AddRange(this.MiTipoDeAccion.TipoDeTramites);
                //this.MiTipoDeAccion.TipoDeTramites.Clear();
            }
            this.mpePopUp.Show();
        }
    }
}