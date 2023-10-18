using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Seguridad.FachadaNegocio;
using Generales.FachadaNegocio;

namespace IU.Modulos.Seguridad.Controles
{
    public partial class SegUsuariosSectoresDatos : ControlesSeguros
    {
        public Usuarios MiUsuario
        {
            get { return (Usuarios)Session[this.MiSessionPagina + "SegGestionarUsuariosDatosMiUsuario"]; }
            set { this.Session[this.MiSessionPagina + "SegGestionarUsuariosDatosMiUsuario"] = value; }
        }

        public delegate void SegUsuariosSectoresAgregarEventHandler(object sender, TGESectores e);
        public event SegUsuariosSectoresAgregarEventHandler UsuariosSectoresAgregar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //if (!this.IsPostBack)
            //{
            //    this.CargarSectores();
            //}
        }

        public void IniciarControl(Gestion pGestion)
        {
            //this.MiUsuario = pUsuario;
            this.chkSectores.Items.Clear();

            this.CargarFilialesUsuario(this.MiUsuario);
            this.GestionControl = pGestion;
            switch (pGestion)
            {
                case Gestion.Modificar:
                    break;
                case Gestion.Consultar:
                    this.ddlFiliales.Enabled = false;
                    this.chkSectores.Enabled = false;
                    break;
                default:
                    break;
            }
        }

        private void CargarSectores(TGEFiliales pFilial)
        {
            TGESectores filtro = new TGESectores();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            filtro.Filial.IdFilial = pFilial.IdFilial;
            List<TGESectores> sectores = TGEGeneralesF.SectoresObtenerListaFiltro(filtro);

            this.chkSectores.DataSource = sectores;
            this.chkSectores.DataValueField = "IdSector";
            this.chkSectores.DataTextField = "Sector";
            this.chkSectores.DataBind();
        }

        private void CargarFilialesUsuario(Usuarios pUsuario)
        {
            this.ddlFiliales.DataSource = pUsuario.Filiales;
            this.ddlFiliales.DataValueField = "IdFilial";
            this.ddlFiliales.DataTextField = "Filial";
            this.ddlFiliales.DataBind();
            if (this.ddlFiliales.Items.Count == 0)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            else if (this.ddlFiliales.Items.Count > 1)
            {
                AyudaProgramacion.AgregarItemSeleccione(this.ddlFiliales, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                this.ddlFiliales_SelectedIndexChanged(null, EventArgs.Empty);
            }
            else if (this.ddlFiliales.Items.Count ==1)
                this.ddlFiliales_SelectedIndexChanged(null, EventArgs.Empty);
        }

        protected void ddlFiliales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlFiliales.SelectedValue))
                return;

            TGEFiliales filial = this.MiUsuario.Filiales.Find(x => x.IdFilial == Convert.ToInt32(this.ddlFiliales.SelectedValue));
            this.CargarSectores(filial);
            foreach (TGESectores per in filial.Sectores)
            {
                foreach (ListItem item in chkSectores.Items)
                {
                    if (Convert.ToInt32(item.Value) == per.IdSector)
                        item.Selected = true;
                }
            }
        }

        protected void chkSectores_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.ddlFiliales.SelectedValue))
                return;
            
            TGESectores cc;
            TGEFiliales filial = this.MiUsuario.Filiales.Find(x => x.IdFilial == Convert.ToInt32(this.ddlFiliales.SelectedValue));
            foreach (ListItem lst in this.chkSectores.Items)
            {
                if (!filial.Sectores.Exists(x => x.IdSector == Convert.ToInt32(lst.Value))
                    && lst.Selected)
                {
                    cc = new TGESectores();
                    cc.IdSector = Convert.ToInt32(lst.Value);
                    cc.Sector = lst.Text;
                    cc.Filial.IdFilial = filial.IdFilial;
                    cc.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(cc, Gestion.Agregar);
                    filial.Sectores.Add(cc);
                    cc.IndiceColeccion = filial.Sectores.IndexOf(cc);
                    if (this.UsuariosSectoresAgregar != null)
                        this.UsuariosSectoresAgregar(null,cc);
                }
                else if (filial.Sectores.Exists(x => x.IdSector== Convert.ToInt32(lst.Value)))
                {
                    cc = filial.Sectores.Single(x => x.IdSector == Convert.ToInt32(lst.Value));
                    if (!lst.Selected)
                        cc.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(cc, Gestion.Anular);
                    else
                        cc.EstadoColeccion = AyudaProgramacion.ObtenerEstadoColeccion(cc, Gestion.Modificar);
                }
            }
        }

    }
}