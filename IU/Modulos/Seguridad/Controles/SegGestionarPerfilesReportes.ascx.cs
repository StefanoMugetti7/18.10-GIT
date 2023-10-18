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
using Reportes.FachadaNegocio;
using System.Collections.Generic;
using Generales.Entidades;
using Generales.FachadaNegocio;
using Reportes.Entidades;

namespace IU.Modulos.Seguridad.Controles
{
    public partial class SegGestionarPerfilesReportes : ControlesSeguros
    {
        private Perfiles MiPerfil
        {
            get { return (Perfiles)Session[this.MiSessionPagina + "SegGestionarPerfilesReportesMiPerfil"]; }
            set { Session[this.MiSessionPagina + "SegGestionarPerfilesReportesMiPerfil"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(Perfiles pPerfil)
        {
            this.MiPerfil = pPerfil;
            this.CargarReportes();
            this.CargarPerfilReportes();
        }

        private void CargarReportes()
        {
            this.tvReportes.Nodes.Clear();

            List<TGEModulosSistema> modulos = TGEGeneralesF.ModulosSistemaObtenerLista();
            List<RepReportes> reportes = new List<RepReportes>();
            TreeNode nodoModulo;
            TreeNode nodoReporte;
            foreach (TGEModulosSistema mod in modulos)
            {
                mod.IdPerfil = MiPerfil.IdPerfil;
                reportes = ReportesF.ReportesObtenerPorModulo(mod);
                nodoModulo = new TreeNode();
                //nodoModulo.Checked = true;
                nodoModulo.Text = mod.ModuloSistema;
                nodoModulo.Value = mod.IdModuloSistema.ToString();
                nodoModulo.ShowCheckBox = false;
                this.tvReportes.Nodes.Add(nodoModulo);
                foreach (RepReportes rep in reportes)
                {
                    nodoReporte = new TreeNode();
                    nodoReporte.Value = rep.IdReporte.ToString();
                    nodoReporte.Text = rep.Descripcion;
                    nodoModulo.ChildNodes.Add(nodoReporte);
                }
            }
        }

        private void CargarPerfilReportes()
        {
            foreach (TreeNode n in this.tvReportes.Nodes)
            {
                if (n.ChildNodes.Count > 0)
                    foreach (TreeNode nodoRep in n.ChildNodes)
                    {
                        if (this.MiPerfil.Reportes.Exists(x => x.IdReporte == Convert.ToInt32(nodoRep.Value)))
                            nodoRep.Checked = true;
                    }
            }
        }

        public List<RepReportes> ObtenerPerfilReportes()
        {
            RepReportes reporte;
            foreach (TreeNode n in this.tvReportes.Nodes)
            {
                if (n.ChildNodes.Count > 0)
                {
                    foreach (TreeNode nodoRep in n.ChildNodes)
                    {
                        reporte = this.MiPerfil.Reportes.Find(x => x.IdReporte == Convert.ToInt32(nodoRep.Value));
                        if (reporte != null)
                        {
                            if (!nodoRep.Checked)
                            {
                                reporte.EstadoColeccion = EstadoColecciones.Borrado;
                            }
                        }
                        else
                        {
                            if (nodoRep.Checked)
                            {
                                reporte = new RepReportes();
                                reporte.IdReporte = Convert.ToInt32(nodoRep.Value);
                                reporte.EstadoColeccion = EstadoColecciones.Agregado;
                                this.MiPerfil.Reportes.Add(reporte);
                            }
                        }
                    }
                }
            }

            return this.MiPerfil.Reportes;
        }
    }
}