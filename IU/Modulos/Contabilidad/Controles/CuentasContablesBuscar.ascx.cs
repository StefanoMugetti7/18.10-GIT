using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Contabilidad;
using System.ComponentModel;
using Comunes.Entidades;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class CuentasContablesBuscar : ControlesSeguros
    {      
        public CtbCuentasContables MiCuentaContable
        {
            get { return (CtbCuentasContables)Session[this.MiSessionPagina + "CuentaContableBuscarMiCuentaContable"]; }
            set { Session[this.MiSessionPagina + "CuentaContableBuscarMiCuentaContable"] = value; }
        }

        //[Browsable(true)]
        //[Category("Behavior")]
        //public int MiIndiceColeccion
        //{
        //    get { return (int)Session[this.MiSessionPagina + this.ClientID + "CuentaContableBuscarMiIndiceColeccion"]; }
        //    set { Session[this.MiSessionPagina + this.ClientID + "CuentaContableBuscarMiIndiceColeccion"] = value; }
        //}

        public delegate void CuentasContablesBuscarEventHandler(CtbCuentasContables e, int indiceColeccion);
        public event CuentasContablesBuscarEventHandler CuentasContablesBuscarSeleccionar;
        public delegate void CuentasContablesBuscarIniciarEventHandler(CtbEjerciciosContables ejercicio);
        public event CuentasContablesBuscarIniciarEventHandler CuentasContablesBuscarIniciar;
        public delegate void CuentasContablesEliminarEventHandler(int e);
        public event CuentasContablesEliminarEventHandler CuentasContablesBuscarEliminar;

        [Browsable(true)]
        [Category("Behavior")]
        public bool MostrarEtiquetas
        {
            get;
            set;
        }

        [Browsable(true)]
        [Category("Behavior")]
        public Unit AnchoTextBoxs
        {
            get;
            set;
        }

        [Browsable(true)]
        [Category("Behavior")]
        public bool MostrarEliminar
        {
            get;
            set;
        }

        [Browsable(true)]
        [Category("Behavior")]
        public string TextoBoton
        {
            get;
            set;
        }

        [Browsable(true)]
        [Category("Behavior")]
        public string LabelNumeroCuenta
        {
            get;
            set;
        }

        [Browsable(true)]
        [Category("Behavior")]
        public bool Validation
        {
            get { return this.rfvNumeroCuenta.Enabled; }
            set { this.rfvNumeroCuenta.Enabled = value; }
        }

        [Browsable(true)]
        [Category("Behavior")]
        public string ValidationGroup
        {
            get { return this.rfvNumeroCuenta.ValidationGroup; }
            set { this.rfvNumeroCuenta.ValidationGroup=value;}
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            this.puCuentasContables.CuentasContablesBuscarSeleccionarPopUp += new CuentasContablesBuscarPopUp.CuentasContablesBuscarPopUpEventHandler(puCuentasContables_CuentasContablesBuscarSeleccionarPopUp);
            this.lblNumeroCuenta.Visible = this.MostrarEtiquetas;
            this.lblDescripcion.Visible = this.MostrarEtiquetas;
            this.txtNumeroCuenta.Width = this.AnchoTextBoxs;
            this.txtDescripcion.Width = this.AnchoTextBoxs;
            if (this.LabelNumeroCuenta != null)
                this.lblNumeroCuenta.Text = this.LabelNumeroCuenta;

            if (!this.IsPostBack)
            {
                //this.MiIndiceColeccion = 0;
                this.MiCuentaContable = new CtbCuentasContables();
            }
        }

        protected void puCuentasContables_CuentasContablesBuscarSeleccionarPopUp(CtbCuentasContables e)
        {
            this.MapearObjetoControles(e);
            if (this.CuentasContablesBuscarSeleccionar != null)
            {
                string val = this.hdfIdIndiceColeccion.Value;
                this.CuentasContablesBuscarSeleccionar(e, val==string.Empty ? 0 : Convert.ToInt32(this.hdfIdIndiceColeccion.Value));
            }
        }

        protected void txtNumeroCuenta_TextChanged(object sender, EventArgs e)
        {
            if (this.txtNumeroCuenta.Text.Trim() == string.Empty)
            {
                this.txtDescripcion.Text = string.Empty;
                this.hfIdCuentaContable.Value = string.Empty;
                if (this.CuentasContablesBuscarSeleccionar != null)
                    this.CuentasContablesBuscarSeleccionar(new CtbCuentasContables(), this.hdfIdIndiceColeccion.Value==string.Empty ? 0 : Convert.ToInt32(this.hdfIdIndiceColeccion.Value));
                return;
            }

            CtbEjerciciosContables ejercicio = new CtbEjerciciosContables();
            this.CuentasContablesBuscarIniciar(ejercicio);

            List<CtbCuentasContables> cuentasContablesBuscar = new List<CtbCuentasContables>();
            CtbCuentasContables cuentaContable = new CtbCuentasContables();
            cuentaContable.Estado.IdEstado = (int)Estados.Activo;
            cuentaContable.NumeroCuenta = this.txtNumeroCuenta.Text;
            cuentaContable.Imputable = true;
            //CtbEjerciciosContables ejercicioActual = ContabilidadF.EjerciciosContablesObtenerActivo();
            //cuentaContable.IdEjercicioContable = ejercicioActual.IdEjercicioContable;
            cuentaContable.IdEjercicioContable = ejercicio.IdEjercicioContable.Value;
            cuentasContablesBuscar = ContabilidadF.CuentasContablesObtenerListaFiltro(cuentaContable);
            if (cuentasContablesBuscar.Count == 1)
            {
                this.txtDescripcion.Text = cuentasContablesBuscar[0].Descripcion;
                this.hfIdCuentaContable.Value = cuentasContablesBuscar[0].IdCuentaContable.ToString();
                //Devuele la CuentaContable
                if (this.CuentasContablesBuscarSeleccionar != null)
                    this.CuentasContablesBuscarSeleccionar(cuentasContablesBuscar[0], Convert.ToInt32(this.hdfIdIndiceColeccion.Value));
            }
            else if (cuentasContablesBuscar.Count > 1)
            {
                this.puCuentasContables.IniciarControl(true, cuentaContable, cuentasContablesBuscar);
            }
            else
            {
                this.puCuentasContables.IniciarControl(true, cuentaContable, new List<CtbCuentasContables>());
            }
        }

        protected void btnBuscar_Click(object sender, EventArgs e)
        {
            CtbEjerciciosContables ejercicio = new CtbEjerciciosContables();
            if (this.CuentasContablesBuscarIniciar != null)
                this.CuentasContablesBuscarIniciar(ejercicio);
            else
                ejercicio = ContabilidadF.EjerciciosContablesObtenerActivo();

            this.MiCuentaContable.IdEjercicioContable=ejercicio.IdEjercicioContable.Value;
            this.puCuentasContables.IniciarControl(true, this.MiCuentaContable, new List<CtbCuentasContables>());
        }

        protected void btnEliminar_Click(object sender, EventArgs e)
        {
            if (this.hdfIdIndiceColeccion.Value != string.Empty)
            {
                if (this.CuentasContablesBuscarEliminar != null)
                {
                    this.btnEliminar.Visible = false;
                    this.CuentasContablesBuscarEliminar(Convert.ToInt32(this.hdfIdIndiceColeccion.Value));
                }
            }
        }

        public void Enable(bool habilitado)
        {
            this.txtNumeroCuenta.Enabled = habilitado;
            this.txtDescripcion.Enabled = habilitado;
            this.btnBuscar.Visible = habilitado;
            this.btnEliminar.Visible = habilitado;
        }

        private void MapearObjetoControles(CtbCuentasContables pCuentaContable)
        {
            this.txtNumeroCuenta.Text = pCuentaContable.NumeroCuenta;
            this.txtDescripcion.Text = pCuentaContable.Descripcion;
            this.hfIdCuentaContable.Value = pCuentaContable.IdCuentaContable.ToString();
            if (pCuentaContable.IdCuentaContable != 0)
                this.btnEliminar.Visible = this.MostrarEliminar;

            this.UpdatePanel1.Update();
        }

        public void MapearObjetoControles(CtbCuentasContables pCuentaContable, Gestion pGestion, int indiceColeccion)
        {
            this.txtNumeroCuenta.Text = pCuentaContable.NumeroCuenta;
            this.txtDescripcion.Text = pCuentaContable.Descripcion;
            this.hfIdCuentaContable.Value = pCuentaContable.IdCuentaContable.ToString();
            this.hdfIdIndiceColeccion.Value = indiceColeccion.ToString();
            if (pCuentaContable.IdCuentaContable != 0 && pGestion != Gestion.Consultar)
                this.btnEliminar.Visible = this.MostrarEliminar;

            if (pGestion == Gestion.Consultar)
            {
                txtNumeroCuenta.Enabled = false;
                btnBuscar.Visible = false;
            }
            this.UpdatePanel1.Update();
        }

        public void CambiarTextoBoton(string texto)
        {
            //this.btnBuscar.Text = texto;
        }
    }
}