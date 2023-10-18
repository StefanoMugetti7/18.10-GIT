using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Prestamos.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Prestamos.Controles
{
    public partial class PlanesIpsDatos : ControlesSeguros
    {
        private PrePrestamosPlanes MiPlan
        {
            get
            {
                return (Session[this.MiSessionPagina + "PlanesIpsDatosMiPlan"] == null ?
                    (PrePrestamosPlanes)(Session[this.MiSessionPagina + "PlanesIpsDatosMiPlan"] = new PrePrestamosPlanes()) : (PrePrestamosPlanes)Session[this.MiSessionPagina + "PlanesDatosMiPlan"]);
            }
            set { Session[this.MiSessionPagina + "PlanesIpsDatosMiPlan"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrParametrosPopUp.PrePrestamosPlanesIpsAceptar += new PlanesIpsDatosPopUp.PlanesIpsDatosPopUpAceptarEventHandler(ctrParametrosPopUp_PrePrestamosPlanesIpsAceptar);
            if (!this.IsPostBack)
            {
            }
        }

        void ctrParametrosPopUp_PrePrestamosPlanesIpsAceptar(PrePrestamosIpsPlanes e)
        {
            List<PrePrestamosIpsPlanes> lista = new List<PrePrestamosIpsPlanes>();
            lista.AddRange(this.MiPlan.PrestamosIpsPlanes);
            this.MiPlan.PrestamosIpsPlanes = new List<PrePrestamosIpsPlanes>();
            this.MiPlan.PrestamosIpsPlanes.Add(e);
            //e.IndiceColeccion = this.MiPlan.PrestamosPlanesTasas.IndexOf(e);
            this.MiPlan.PrestamosIpsPlanes.AddRange(lista);
            AyudaProgramacion.AcomodarIndices<PrePrestamosIpsPlanes>(this.MiPlan.PrestamosIpsPlanes);
            this.CargarGrilla();
        }

        public void IniciarControl(PrePrestamosPlanes pParametro, Gestion pGestion)
        {
            this.MiPlan = pParametro;
            this.GestionControl = pGestion;
            this.CargarGrilla();
            switch (pGestion)
            {
                case Gestion.Agregar:
                case Gestion.Modificar:
                    this.btnAgregar.Visible = true;
                    break;
                case Gestion.Consultar:
                    break;
            }
        }

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            PrePrestamosIpsPlanes param = new PrePrestamosIpsPlanes();
            param.IdPlan = this.MiPlan.IdPrestamoPlan;
            this.ctrParametrosPopUp.IniciarControl(param);
        }

        protected void gvParametrosValores_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
            //    string mensaje = this.ObtenerMensajeSistema("ParametroValorConfirmarBaja");
            //    string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
            //    ibtn.Attributes.Add("OnClick", funcion);
            //    ibtn.Visible = true;
            //}
        }

        protected void gvParametrosValores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvParametrosValores.PageIndex = e.NewPageIndex;
            this.gvParametrosValores.DataSource = this.MiPlan.PrestamosIpsPlanes;
            this.gvParametrosValores.DataBind();
        }

        private void CargarGrilla()
        {
            AyudaProgramacion.CargarGrillaListas(this.MiPlan.PrestamosIpsPlanes, true, this.gvParametrosValores, true);
        }

        internal List<PrePrestamosIpsPlanes> ObtenerLista()
        {
            return this.MiPlan.PrestamosIpsPlanes;
        }
    }
}