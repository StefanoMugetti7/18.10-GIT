using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using Generales.FachadaNegocio;
using ProcesosDatos.Entidades;
using ProcesosDatos;
using Seguridad.Entidades;


namespace IU.Modulos.Seguridad.Controles
{
    public partial class SegGestionarPerfilesProcesos : ControlesSeguros
    {

        private Perfiles MiPerfil
        {
            get { return (Perfiles)Session[this.MiSessionPagina + "SegGestionarPerfilesProcesosMiPerfil"]; }
            set { Session[this.MiSessionPagina + "SegGestionarPerfilesProcesosMiPerfil"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(Perfiles pPerfil)
        {
            this.MiPerfil = pPerfil;
            this.CargarProcesos();
            this.CargarPerfilProcesos();
        }

        private void CargarProcesos()
        {
            this.tvProcesos.Nodes.Clear();

            //List<TGEModulosSistema> modulos = TGEGeneralesF.ModulosSistemaObtenerLista();
            List<SisProcesos> procesos = ProcesosDatosF.ProcesosObtenerLista(MiPerfil);
            //TreeNode nodoModulo;
            TreeNode nodoProceso;
            //foreach (TGEModulosSistema mod in modulos)
            //{
            //   // procesos = ReportesF.ReportesObtenerPorModulo(mod); //Obtener procesos por modulo (HACER) 
            //    nodoModulo = new TreeNode();

            //    nodoModulo.Text = mod.ModuloSistema;
            //    nodoModulo.Value = mod.IdModuloSistema.ToString();
            //    nodoModulo.ShowCheckBox = false;
            //    this.tvProcesos.Nodes.Add(nodoModulo);
            foreach (SisProcesos pro in procesos)
            {
                nodoProceso = new TreeNode();
                nodoProceso.Value = pro.IdProceso.ToString();
                nodoProceso.Text = pro.Descripcion;
                this.tvProcesos.Nodes.Add(nodoProceso);
            }

        }

        private void CargarPerfilProcesos()
        {
            foreach (TreeNode n in this.tvProcesos.Nodes)
            {
               
                        if (this.MiPerfil.Procesos.Exists(x => x.IdProceso == Convert.ToInt32(n.Value)))
                            n.Checked = true;
                    
            }
        }

        public List<SegPerfilesProcesosDatos> ObtenerPerfilProcesos()
        {
            SegPerfilesProcesosDatos proceso;
            foreach (TreeNode n in this.tvProcesos.Nodes)
            {
                
                        proceso = this.MiPerfil.Procesos.Find(x => x.IdProceso == Convert.ToInt32(n.Value));
                        if (proceso != null)
                        {
                            if (!n.Checked)
                            {
                                proceso.EstadoColeccion = EstadoColecciones.Borrado;
                            }
                        }
                        else
                        {
                            if (n.Checked)
                            {
                                proceso = new SegPerfilesProcesosDatos();
                                proceso.IdProceso = Convert.ToInt32(n.Value);
                                proceso.EstadoColeccion = EstadoColecciones.Agregado;
                                this.MiPerfil.Procesos.Add(proceso);
                            }
                        }
                    
                }
            return this.MiPerfil.Procesos;
            }

            
        
    }
}