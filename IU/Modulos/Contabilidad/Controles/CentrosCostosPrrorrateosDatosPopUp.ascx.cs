using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class CentrosCostosPrrorrateosDatosPopUp : ControlesSeguros
    {
        private bool MostrarPopUp
        {
            get { return (bool)Session[this.MiSessionPagina + "CentrosCostosPrrorrateosDatosPopUpMostrarPopUp"]; }
            set { Session[this.MiSessionPagina + "CentrosCostosPrrorrateosDatosPopUpMostrarPopUp"] = value; }
        }

        public delegate void AsientoContableDatosAceptarEventHandler(object sender, CtbCentrosCostosProrrateos e);
        public event AsientoContableDatosAceptarEventHandler ControlDatosAceptar;

        public delegate void ControlDatosCancelarEventHandler();
        public event ControlDatosCancelarEventHandler ControlDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.ctrDatos.ControlDatosAceptar += new CentrosCostosProrrateosDatos.AsientoContableDatosAceptarEventHandler(ctrDatos_ControlDatosAceptar);
            this.ctrDatos.ControlDatosCancelar += new CentrosCostosProrrateosDatos.ControlDatosCancelarEventHandler(ctrDatos_ControlDatosCancelar);
            if (!this.IsPostBack)
                this.MostrarPopUp = false;
            else
            {
                if (this.MostrarPopUp)
                    this.mpePopUp.Show();
            }
        }

        void ctrDatos_ControlDatosCancelar()
        {
            this.mpePopUp.Hide();
            this.MostrarPopUp = false;
            if (this.ControlDatosCancelar != null)
                this.ControlDatosCancelar();
        }

        void ctrDatos_ControlDatosAceptar(object sender, CtbCentrosCostosProrrateos e)
        {
            this.mpePopUp.Hide();
            this.MostrarPopUp = false;
            if (this.ControlDatosAceptar != null)
                this.ControlDatosAceptar(sender, e);
        }

        public void IniciarControl(CtbCentrosCostosProrrateos pCentroCostoProrrateo, Gestion pGestion)
        {
            this.ctrDatos.IniciarControl(pCentroCostoProrrateo, pGestion);
            this.mpePopUp.Show();
            this.MostrarPopUp = true;
        }


    }
}