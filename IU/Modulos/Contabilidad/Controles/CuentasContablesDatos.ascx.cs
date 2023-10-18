using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Contabilidad.Entidades;
using Comunes.Entidades;
using Contabilidad;
using Generales.FachadaNegocio;
using Generales.Entidades;
using System.Data;

namespace IU.Modulos.Contabilidad.Controles
{
    public partial class CuentasContablesDatos : ControlesSeguros
    {
        const int niveles = 2;

        private CtbCuentasContables MiCuentaContable
        {
            get { return (CtbCuentasContables)Session[this.MiSessionPagina + "MiCuentaContable"]; }
            set { Session[this.MiSessionPagina + "MiCuentaContable"] = value; }
        }

        private List<CtbCuentasContables> MisCuentasContables
        {
            get { return (List<CtbCuentasContables>)Session[this.MiSessionPagina + "MisCuentasContables"]; }
            set { Session[this.MiSessionPagina + "MisCuentasContables"] = value; }
        }

        private List<CtbCapitulos> MisCapitulos
        {
            get { return (List<CtbCapitulos>)Session[this.MiSessionPagina + "MisCapitulos"]; }
            set { Session[this.MiSessionPagina + "MisCapitulos"] = value; }
        }

        private List<CtbRubros> MisRubros
        {
            get { return (List<CtbRubros>)Session[this.MiSessionPagina + "MisRubros"]; }
            set { Session[this.MiSessionPagina + "MisRubros"] = value; }
        }

        private List<CtbMonedas> MisMonedas
        {
            get { return (List<CtbMonedas>)Session[this.MiSessionPagina + "MisMonedas"]; }
            set { Session[this.MiSessionPagina + "MisMonedas"] = value; }
        }

        private List<CtbDepartamentos> MisDespartamentos
        {
            get { return (List<CtbDepartamentos>)Session[this.MiSessionPagina + "MisDespartamentos"]; }
            set { Session[this.MiSessionPagina + "MisDespartamentos"] = value; }
        }

        private List<CtbSubRubros> MisSubRubros
        {
            get { return (List<CtbSubRubros>)Session[this.MiSessionPagina + "MisSubRubros"]; }
            set { Session[this.MiSessionPagina + "MisSubRubros"] = value; }
        }

        private List<CtbCuentasContables> MisCuentasRamas
        {
            get { return (List<CtbCuentasContables>)Session[this.MiSessionPagina + "MisCuentasRamas"]; }
            set { Session[this.MiSessionPagina + "MisCuentasRamas"] = value; }
        }

        private List<CtbCuentasContables> MisCuentasRamasYContables
        {
            get { return (List<CtbCuentasContables>)Session[this.MiSessionPagina + "MisCuentasRamasYContables"]; }
            set { Session[this.MiSessionPagina + "MisCuentasRamasYContables"] = value; }
        }

        private bool MiCuentaINAES
        {
            get { return (bool)Session[this.MiSessionPagina + "MiCuentaINAES"]; }
            set { Session[this.MiSessionPagina + "MiCuentaINAES"] = value; }
        }

        //public delegate void CuentaContableDatosAceptarEventHandler(object sender, CtbCuentasContables e);
        //public event CuentaContableDatosAceptarEventHandler CuentaContableDatosAceptar;

        //public delegate void CuentaContableDatosCancelarEventHandler();
        //public event CuentaContableDatosCancelarEventHandler CuentaContableDatosCancelar;
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            //this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            if (!this.IsPostBack)
            {
                BotonesPorDefecto.EstablecerBotonPorDefecto(this.txtBuscar, this.btnFiltrar);
                //this.tvCuentasContables.Attributes.Add("onclick", "OnTreeClick(event)");
                //this.tvCuentasContables.ExpandDepth = 2;
                //if (this.MiCuentaContable == null && this.GestionControl != Gestion.Agregar)
                //{
                //    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/InicioSistema.aspx"), true);
                //}
            }
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de una Cuenta Contable
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(CtbCuentasContables pCuentaContable, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.InterfazContabilidadINAES);
            this.MiCuentaINAES = valor.ParametroValor == string.Empty ? false : Convert.ToBoolean(valor.ParametroValor);

            CtbEjerciciosContables filtro = new CtbEjerciciosContables();
            filtro.Estado.IdEstado = (int)Estados.Activo;
            List<CtbEjerciciosContables> lista = ContabilidadF.EjerciciosContablesObtenerListaFiltro(filtro);
            this.ddlEjercicioContable.DataSource = lista;
            this.ddlEjercicioContable.DataValueField = "IdEjercicioContable";
            this.ddlEjercicioContable.DataTextField = "Descripcion";
            this.ddlEjercicioContable.DataBind();
            if (this.ddlEjercicioContable.Items.Count != 1)
                AyudaProgramacion.AgregarItemSeleccione(this.ddlEjercicioContable, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            else
                this.ddlEjerciciosContables_SelectedIndexChanged(null , EventArgs.Empty);

            this.MostrarCuentaINAES(this.MiCuentaINAES);
            this.pnlGrillaCuentas.Visible = false;
            this.pnlCuentaContable.Visible = false;
        }

        private void MostrarCuentaINAES(bool pValor)
        {
            this.pnlInaes.Visible = pValor;
            //NO OBLIGATORIOS POR AHORA!!!
            this.rfvCapitulo.Enabled = false;
            this.frvMoneda.Enabled = false;
            this.rfvDepartamento.Enabled = false;
            this.rfvRubro.Enabled = false;
            this.frvSubRubro.Enabled = false;
        }

        private void MapearObjetoAControles(CtbCuentasContables pCuentaContable)
        {
            this.txtDescripción.Text = pCuentaContable.Descripcion;
            this.txtNumeroCuenta.Text = pCuentaContable.NumeroCuenta;
            this.ddlRama.SelectedValue = pCuentaContable.IdCuentaContableRama == 0 ? string.Empty : pCuentaContable.IdCuentaContableRama.ToString();
            //this.txtRama.Text = pCuentaContable.DescripcionRama;
            this.chkImputable.Checked = pCuentaContable.Imputable;
            ListItem item = this.ddlCentroCostos.Items.FindByValue(pCuentaContable.CentroCostoProrrateo.IdCentroCostoProrrateo.HasValue ? pCuentaContable.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString() : string.Empty);
            if (item != null)
                this.ddlCentroCostos.Items.Add(new ListItem(pCuentaContable.CentroCostoProrrateo.CentroCostoProrrateo, pCuentaContable.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString()));
            this.ddlCentroCostos.SelectedValue = pCuentaContable.CentroCostoProrrateo.IdCentroCostoProrrateo.HasValue ? pCuentaContable.CentroCostoProrrateo.IdCentroCostoProrrateo.ToString() : string.Empty;
            this.chkCentroCostosObligatorio.Checked = pCuentaContable.CentroCostoObligatorio;
            this.chkMonetaria.Checked = pCuentaContable.Monetaria;
            this.ddlEstado.SelectedValue = pCuentaContable.Estado.IdEstado.ToString();
            if (this.MiCuentaINAES)
            {
                this.ddlCapitulo.SelectedValue = pCuentaContable.Capitulo.IdCapitulo == 0 ? string.Empty : pCuentaContable.Capitulo.IdCapitulo.ToString();
                this.ddlDepartamento.SelectedValue = pCuentaContable.Departamento.IdDepartamento.HasValue ? pCuentaContable.Departamento.IdDepartamento.ToString() : string.Empty;
                this.ddlMoneda.SelectedValue = pCuentaContable.Moneda.IdMoneda == 0 ? string.Empty : pCuentaContable.Moneda.IdMoneda.ToString();
                this.ddlRubro.SelectedValue = pCuentaContable.Rubro.IdRubro == 0 ? string.Empty : pCuentaContable.Rubro.IdRubro.ToString();
                this.ddlSubRubro.SelectedValue = pCuentaContable.SubRubro.IdSubRubro.HasValue ? pCuentaContable.SubRubro.IdSubRubro.ToString() : string.Empty;
                this.txtImputacion.Text = pCuentaContable.Imputacion.ToString();
            }
            this.ctrCamposValores.BorrarControlesParametros();
            this.ctrCamposValores.IniciarControl(pCuentaContable, new Objeto(), this.GestionControl);
        }

        private void MapearControlesAObjeto(CtbCuentasContables pCuentaContable)
        {
            pCuentaContable.Descripcion = this.txtDescripción.Text;
            pCuentaContable.NumeroCuenta = this.txtNumeroCuenta.Text;
            pCuentaContable.IdCuentaContableRama = this.ddlRama.SelectedValue == String.Empty ? 0 : Convert.ToInt32(this.ddlRama.SelectedValue);
            pCuentaContable.Imputable = this.chkImputable.Checked;

            var array = this.txtNumeroCuenta.Text.Split('.');
            string imp = array[array.Length - 1];
            int tmpImp = 0;
            if (Int32.TryParse(imp, out tmpImp))
                pCuentaContable.Imputacion = tmpImp;

            pCuentaContable.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue);
            pCuentaContable.CentroCostoProrrateo.IdCentroCostoProrrateo = this.ddlCentroCostos.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlCentroCostos.SelectedValue);
            pCuentaContable.CentroCostoObligatorio = this.chkCentroCostosObligatorio.Checked;
            pCuentaContable.Monetaria = this.chkMonetaria.Checked;
            pCuentaContable.Estado.IdEstado = Convert.ToInt32(this.ddlEstado.SelectedValue);

            if (this.MiCuentaINAES)
            {
                pCuentaContable.Capitulo.IdCapitulo = this.ddlCapitulo.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlCapitulo.SelectedValue);
                pCuentaContable.Departamento.IdDepartamento = this.ddlDepartamento.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlDepartamento.SelectedValue);
                pCuentaContable.Moneda.IdMoneda = this.ddlMoneda.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlMoneda.SelectedValue);
                pCuentaContable.Rubro.IdRubro = this.ddlRubro.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlRubro.SelectedValue);
                pCuentaContable.SubRubro.IdSubRubro = this.ddlSubRubro.SelectedValue == string.Empty ? default(int?) : Convert.ToInt32(this.ddlSubRubro.SelectedValue);
                pCuentaContable.Imputacion = this.txtImputacion.Text == string.Empty ? 0 : Convert.ToInt32(this.txtImputacion.Text);
            }
            pCuentaContable.Campos = this.ctrCamposValores.ObtenerLista();
        }

        private void CargarCuentasRamas(CtbCuentasContables pCuentaRama, List<CtbCuentasContables> pListaCuentas, TreeNode mi)
        {
            //List<CtbCuentasContables> lista = this.MisCuentasRamas.FindAll(x => x.IdCuentaContableRama == pCuentaRama.IdCuentaContableRama);
            List<CtbCuentasContables> lista = pListaCuentas.FindAll(x => x.IdCuentaContableRama == pCuentaRama.IdCuentaContableRama);
            CtbCuentasContables ctaRama;
            foreach (CtbCuentasContables cta in lista)
            {
                TreeNode miNuevo = new TreeNode();
                //bool bHijos = (bool)men.Hijos;
                ctaRama = new CtbCuentasContables();
                ctaRama.IdCuentaContableRama = cta.IdCuentaContable;
                miNuevo.Text = cta.Descripcion;
                miNuevo.Value = cta.IdCuentaContable.ToString();
                
                if (cta.IdCuentaContableRama == 0)
                    miNuevo.Selected = false;

                if (mi == null)
                    tvCuentasContables.Nodes.Add(miNuevo);
                else
                    mi.ChildNodes.Add(miNuevo);

                if (miNuevo.Depth < niveles)
                    miNuevo.Expand();
                else
                    miNuevo.Collapse();

                this.CargarCuentasRamas(ctaRama, pListaCuentas, miNuevo);
            }
        }

        protected void tvMenu_SelectedNodeChanged(object sender, EventArgs e)
        {
            //CtbCuentasContables cuentaRama = new CtbCuentasContables();
            //cuentaRama.IdCuentaContableRama = Convert.ToInt32(((TreeView)sender).SelectedValue);
            //cuentaRama.Descripcion = ((TreeView)sender).SelectedNode.Text;
            //if (this.pnlCuentaContable.Visible)
            //{
            //    this.pnlCuentaContable.Visible = false;
            //    this.upCuentaContable.Update();
            //}
            //this.CargarLista(cuentaRama);
            CtbCuentasContables cuentaSeleccionada = this.MisCuentasRamasYContables.Find(x => x.IdCuentaContable == Convert.ToInt32(((TreeView)sender).SelectedValue));
            this.MiCuentaContable = ContabilidadF.CuentasContablesObtenerDatosCompletos(cuentaSeleccionada);
            cuentaSeleccionada.Estado.IdEstado = this.MiCuentaContable.Estado.IdEstado;
            CtbCuentasContables cuentaRama = this.MisCuentasRamasYContables.Find(x => x.IdCuentaContable == this.MiCuentaContable.IdCuentaContableRama);
            this.MisCuentasRamas = new List<CtbCuentasContables>();
            //this.MisCuentasRamas.AddRange(this.MisCuentasRamasYContables.FindAll(x=> x.Nivel == cuentaRama.Nivel));
            this.MisCuentasRamas.AddRange(this.MisCuentasRamasYContables.FindAll(x => !x.Imputable).OrderBy(x=>x.NumeroCuenta).ToList());
            this.ddlRama.DataSource = this.MisCuentasRamas;// ContabilidadF.CuentasContablesObtenerListaRama();
            this.ddlRama.DataValueField = "IdCuentaContable";
            this.ddlRama.DataTextField = "Descripcion";
            this.ddlRama.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlRama, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (this.MiCuentaContable.Estado.IdEstado == -1)
            {
                this.pnlCuentaContable.Visible = false;
                this.upCuentaContable.Update();
                this.pnlGrillaCuentas.Visible = true;
                this.upGrillaCuentas.Update();
                return;
            }         

            if (this.MiCuentaContable.Imputable)// || this.MiCuentaContable.Nivel == 4)
                this.pnlGrillaCuentas.Visible = false;
            else
                //this.btnAgregar.Visible = true;
                this.pnlGrillaCuentas.Visible = true;

            this.MapearObjetoAControles(this.MiCuentaContable);
            this.GestionControl = Gestion.Modificar;
            this.lblDatosCuenta.Text = "Modificar Datos de la Cuenta";
            if (this.MisCuentasRamasYContables.Exists(x => x.IdCuentaContableRama == this.MiCuentaContable.IdCuentaContable))
                this.chkImputable.Enabled = false;//this.MiCuentaContable.Nivel == 4;
            else
                this.chkImputable.Enabled = true;
            this.ddlRama.Enabled = true;// !this.MiCuentaContable.TieneMovimientos;
            this.pnlCuentaContable.Visible = true;
            this.upCuentaContable.Update();
            //this.pnlGrillaCuentas.Visible = false;
            this.upGrillaCuentas.Update();
        }

        private void CargarLista(CtbCuentasContables pCuentasContalbes)
        {
            pCuentasContalbes.BusquedaParametros = true;
            this.BusquedaParametrosGuardarValor<CtbCuentasContables>(pCuentasContalbes);
            this.MisCuentasContables = ContabilidadF.CuentasContablesObtenerPorRama(pCuentasContalbes);
            this.gvDatos.DataSource = this.MisCuentasContables;
            this.gvDatos.PageIndex = pCuentasContalbes.IndiceColeccion;
            this.gvDatos.DataBind();
            this.pnlGrillaCuentas.Visible = true;
            this.upGrillaCuentas.Update();
        }

        private void CargarCombos()
        {
            this.ddlEstado.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstado.DataValueField = "IdEstado";
            this.ddlEstado.DataTextField = "Descripcion";
            this.ddlEstado.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlEstado, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            CtbCentrosCostosProrrateos ccp = new CtbCentrosCostosProrrateos();
            ccp.UsuarioLogueado.IdUsuarioEvento = this.UsuarioActivo.IdUsuarioEvento;
            this.ddlCentroCostos.DataSource = ContabilidadF.CentrosCostosProrrateosObtenerCombo(ccp);
            this.ddlCentroCostos.DataValueField = "IdCentroCostoProrrateo";
            this.ddlCentroCostos.DataTextField = "CentroCostoProrrateo";
            this.ddlCentroCostos.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlCentroCostos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            //this.ddlRama.DataSource = this.MisCuentasRamas;// ContabilidadF.CuentasContablesObtenerListaRama();
            //this.ddlRama.DataValueField = "IdCuentaContable";
            //this.ddlRama.DataTextField = "Descripcion";
            //this.ddlRama.DataBind();
            //AyudaProgramacion.AgregarItemSeleccione(this.ddlRama, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            if (this.MiCuentaINAES)
            {
                this.MisCapitulos = ContabilidadF.CapitulosObtenerListar(new CtbCapitulos());
                this.ddlCapitulo.DataSource = this.MisCapitulos;
                this.ddlCapitulo.DataValueField = "IdCapitulo";
                this.ddlCapitulo.DataTextField = "Capitulo";
                this.ddlCapitulo.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlCapitulo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MisDespartamentos = ContabilidadF.DepartamentosObtenerListar(new CtbDepartamentos());
                this.ddlDepartamento.DataSource = this.MisDespartamentos;
                this.ddlDepartamento.DataValueField = "IdDepartamento";
                this.ddlDepartamento.DataTextField = "Departamento";
                this.ddlDepartamento.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlDepartamento, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MisMonedas = ContabilidadF.MonedasObtenerListar(new CtbMonedas());
                this.ddlMoneda.DataSource = this.MisMonedas;
                this.ddlMoneda.DataValueField = "IdMoneda";
                this.ddlMoneda.DataTextField = "Moneda";
                this.ddlMoneda.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlMoneda, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MisRubros = ContabilidadF.RubrosObtenerListar(new CtbRubros());
                this.ddlRubro.DataSource = this.MisRubros;
                this.ddlRubro.DataValueField = "IdRubro";
                this.ddlRubro.DataTextField = "Rubro";
                this.ddlRubro.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlRubro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                this.MisSubRubros = ContabilidadF.SubRubrosObtenerListar(new CtbSubRubros());
                this.ddlSubRubro.DataSource = this.MisSubRubros;
                this.ddlSubRubro.DataValueField = "IdSubRubro";
                this.ddlSubRubro.DataTextField = "SubRubro";
                this.ddlSubRubro.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlSubRubro, this.ObtenerMensajeSistema("SeleccioneOpcion"));
            }
        }

        private void ConcatenarNumeroCuenta(string valor, int posicion)
        {
            //if (valor == String.Empty)
            //{
            //    valor = "00";
            //}
            //var array = this.txtNumeroCuenta.Text.Split('.');
            //array[posicion] = valor;

            //if (!string.IsNullOrEmpty(this.ddlCapitulo.SelectedValue)
            //    && !string.IsNullOrEmpty(this.ddlRubro.SelectedValue)
            //    && !string.IsNullOrEmpty(this.ddlMoneda.SelectedValue)
            //    && !string.IsNullOrEmpty(this.ddlDepartamento.SelectedValue)
            //    && !string.IsNullOrEmpty(this.ddlSubRubro.SelectedValue))
            //{
            //    this.MiCuentaContable.Capitulo.IdCapitulo = this.MisCapitulos[this.ddlCapitulo.SelectedIndex].IdCapitulo;
            //    this.MiCuentaContable.Rubro.IdRubro = this.MisRubros[this.ddlRubro.SelectedIndex].IdRubro;
            //    this.MiCuentaContable.Moneda.IdMoneda = this.MisMonedas[this.ddlMoneda.SelectedIndex].IdMoneda;
            //    this.MiCuentaContable.Departamento.IdDepartamento = this.MisDespartamentos[this.ddlDepartamento.SelectedIndex].IdDepartamento;
            //    this.MiCuentaContable.SubRubro.IdSubRubro = this.MisSubRubros[this.ddlSubRubro.SelectedIndex].IdSubRubro;
            //    if (this.GestionControl == Gestion.Agregar)
            //        this.MiCuentaContable.Imputacion = ContabilidadF.CuentasContablesObtenerImputacion(this.MiCuentaContable).Imputacion;

            //    array[5] = this.MiCuentaContable.Imputacion.ToString().PadLeft(2, '0');
            //}
            //this.txtNumeroCuenta.Text = String.Join(".", array);
        }

        protected void btnFiltrar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ddlEjercicioContable.SelectedValue))
            {
                return;
            }
            //Filtrar Arbol
            bool filtrar = this.txtBuscar.Text.Trim() != string.Empty;
            List<CtbCuentasContables> listaFiltro = filtrar ? this.MisCuentasRamasYContables.Where(x => x.Descripcion.ToLower().Contains(this.txtBuscar.Text.Trim())).ToList()
                                                            : this.MisCuentasRamasYContables;

            if (filtrar)
            {
                bool buscar = true;
                int desde = 0;
                int hasta = listaFiltro.Count;
                while (buscar)
                {
                    buscar = false;
                    for (int i = desde; i < hasta; i++)
                    {
                        if (!listaFiltro.Exists(c => listaFiltro[i].IdCuentaContableRama == c.IdCuentaContable)
                            && this.MisCuentasRamasYContables.Exists(c => listaFiltro[i].IdCuentaContableRama == c.IdCuentaContable))
                        {
                            listaFiltro.Add(this.MisCuentasRamasYContables.Find(c => listaFiltro[i].IdCuentaContableRama == c.IdCuentaContable));
                            buscar = true;
                        }
                    }
                    desde = hasta;
                    hasta = listaFiltro.Count;
                }
                //Agrego nodo RAIZ
                listaFiltro.AddRange(listaFiltro.Where(p => !this.MisCuentasRamasYContables.Any(p2 => p2.IdCuentaContable == p.IdCuentaContable && p2.Estado.IdEstado == -1)));
            }

            this.tvCuentasContables.Nodes.Clear();
            TreeNode nodo = null;
            CtbCuentasContables cuentaRama = new CtbCuentasContables();
            this.CargarCuentasRamas(cuentaRama, listaFiltro, nodo);

            if (filtrar)
                this.tvCuentasContables.ExpandAll();
            AyudaProgramacion.SortTreeNodes(this.tvCuentasContables.Nodes);

            this.upCuentasRamas.Update();
            this.btnCancelar_Click(null, EventArgs.Empty);
        }

        #region Alta Modificacion Cuenta Contable

        protected void btnAgregar_Click(object sender, EventArgs e)
        {
            this.ddlRama.Items.Clear();
            this.ddlRama.SelectedValue = null;
            this.MisCuentasRamas = new List<CtbCuentasContables>();
            this.MisCuentasRamas.Add(this.MisCuentasRamasYContables.Find(x => x.IdCuentaContable == Convert.ToInt32(this.tvCuentasContables.SelectedValue)));
            this.ddlRama.DataSource = this.MisCuentasRamas;// ContabilidadF.CuentasContablesObtenerListaRama();
            this.ddlRama.DataValueField = "IdCuentaContable";
            this.ddlRama.DataTextField = "Descripcion";
            this.ddlRama.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlRama, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.MiCuentaContable = new CtbCuentasContables();
            this.MiCuentaContable.IdCuentaContableRama = Convert.ToInt32(this.tvCuentasContables.SelectedValue);
            this.MiCuentaContable.DescripcionRama = this.tvCuentasContables.SelectedNode.Text;
            this.MiCuentaContable.Estado.IdEstado = (int)Estados.Activo;
            CtbCuentasContables cuentaRama = this.MisCuentasRamasYContables.Find(x => x.IdCuentaContable == this.MiCuentaContable.IdCuentaContableRama);
            if (cuentaRama.Estado.IdEstado != -1)
                this.MiCuentaContable.NumeroCuenta = cuentaRama.NumeroCuenta;
            this.MapearObjetoAControles(this.MiCuentaContable);
            this.chkImputable.Enabled = true;
            this.ddlRama.Enabled = false;
            this.GestionControl = Gestion.Agregar;
            this.lblDatosCuenta.Text = "Agregar Cuenta";
            this.ctrCamposValores.BorrarControlesParametros();
            this.ctrCamposValores.IniciarControl(this.MiCuentaContable, new Objeto(), this.GestionControl);
            this.pnlCuentaContable.Visible = true;
            this.pnlGrillaCuentas.Visible = false;
            this.upCuentaContable.Update();
        }

        private int ObtenerNivelRama(TreeNode node)
        {
            int i = -1;
            // Walk up the tree until we find the
            // root of the tree, keeping count of
            // how many nodes we walk over in
            // the process
            while (node != null)
            {
                i++;
                node = node.Parent;
            }
            return i;
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiCuentaContable = ContabilidadF.CuentasContablesObtenerDatosCompletos( this.MisCuentasContables[indiceColeccion]);

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.MapearObjetoAControles(this.MiCuentaContable);
                this.GestionControl = Gestion.Modificar;
                this.pnlCuentaContable.Visible = true;
                this.upCuentaContable.Update();
                this.pnlGrillaCuentas.Visible = false;
            }
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //CtbCuentasContables cuentaContable = (CtbCuentasContables)e.Row.DataItem;

                //ImageButton modificar = (ImageButton)e.Row.FindControl("btnModificar");
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                int cellCount = e.Row.Cells.Count;
                e.Row.Cells.Clear();
                TableCell tableCell = new TableCell();
                tableCell.ColumnSpan = cellCount;
                tableCell.HorizontalAlign = HorizontalAlign.Right;
                tableCell.Text = string.Format(this.ObtenerMensajeSistema("GrillaTotalRegistros"), this.MisCuentasContables.Count);
                e.Row.Cells.Add(tableCell);
            }
        }

        protected void gvDatos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CtbCuentasContables parametros = this.BusquedaParametrosObtenerValor<CtbCuentasContables>();
            parametros.IndiceColeccion = e.NewPageIndex;
            this.BusquedaParametrosGuardarValor<CtbCuentasContables>(parametros);

            this.gvDatos.PageIndex = e.NewPageIndex;
            this.gvDatos.DataSource = this.MisCuentasContables;
            this.gvDatos.DataBind();
        }

        protected void gvDatos_Sorting(object sender, GridViewSortEventArgs e)
        {
            this.MisCuentasContables = this.OrdenarGrillaDatos<CtbCuentasContables>(this.MisCuentasContables, e);
            this.gvDatos.DataSource = this.MisCuentasContables;
            this.gvDatos.DataBind();
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            if (!this.Page.IsValid)
                return;
            this.MapearControlesAObjeto(this.MiCuentaContable);
            this.MiCuentaContable.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            TreeNode nodo;
            string codigoMsg = string.Empty;
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiCuentaContable.IdCuentaContableRama = Convert.ToInt32(this.tvCuentasContables.SelectedValue);
                    this.MiCuentaContable.IdUsuarioAlta = this.UsuarioActivo.IdUsuario;
                    guardo = ContabilidadF.CuentasContablesAgregar(this.MiCuentaContable);
                    //Actualizo el Arbol con la cuenta nueva
                    if (guardo)
                    {
                        codigoMsg = this.MiCuentaContable.CodigoMensaje;
                        this.MiCuentaContable = ContabilidadF.CuentasContablesObtenerSeleccionarRama(this.MiCuentaContable);
                        this.MisCuentasRamasYContables.Add(this.MiCuentaContable);
                        //Actualizo el Arbol con la cuenta nueva
                        nodo = new TreeNode();
                        nodo.Value = this.MiCuentaContable.IdCuentaContable.ToString();
                        nodo.Text = this.MiCuentaContable.Descripcion;
                        this.tvCuentasContables.SelectedNode.ChildNodes.Add(nodo);
                    }
                    break;
                case Gestion.Modificar:
                    bool reubicarCuenta = false;
                    guardo = ContabilidadF.CuentasContablesModificar(this.MiCuentaContable);
                    //Actualizo el Arbol
                    if (guardo)
                    {
                        codigoMsg = this.MiCuentaContable.CodigoMensaje;
                        //Actualizo el Arbol
                        TreeNode valor = this.tvCuentasContables.SelectedNode;
                        if (this.MiCuentaContable.Estado.IdEstado == (int)Estados.Baja)
                        {
                            this.tvCuentasContables.SelectedNode.Parent.ChildNodes.Remove(valor);
                            this.MisCuentasRamasYContables.Remove( this.MisCuentasRamasYContables.Find(x=>x.IdCuentaContable==this.MiCuentaContable.IdCuentaContable));
                        }
                        else
                        {
                            if (valor.Value == this.MiCuentaContable.IdCuentaContable.ToString())
                            {
                                this.MiCuentaContable = ContabilidadF.CuentasContablesObtenerSeleccionarRama(this.MiCuentaContable);
                                valor.Text = this.MiCuentaContable.Descripcion;
                            }

                            CtbCuentasContables cuenta = this.MisCuentasRamasYContables.Find(x => x.IdCuentaContable == this.MiCuentaContable.IdCuentaContable);
                            if (cuenta.IdCuentaContableRama != this.MiCuentaContable.IdCuentaContableRama)
                                reubicarCuenta = true;
                            AyudaProgramacion.MatchObjectProperties(this.MiCuentaContable, cuenta);
                            if (reubicarCuenta)
                            {
                                TreeNode nodoNuevo = new TreeNode();
                                nodoNuevo.Value = cuenta.IdCuentaContable.ToString();
                                nodoNuevo.Text = string.Concat(cuenta.NumeroCuenta, " ", cuenta.Descripcion);
                                foreach (TreeNode nodoPadre in this.tvCuentasContables.SelectedNode.Parent.Parent.ChildNodes)
                                {
                                    if (nodoPadre.Value == cuenta.IdCuentaContableRama.ToString())
                                    {
                                        nodoPadre.ChildNodes.Add(nodoNuevo);
                                        break;
                                    }
                                }
                                this.tvCuentasContables.SelectedNode.Parent.ChildNodes.Remove(valor);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
            if (guardo)
            {
                //Recargo las Ramas
                this.MisCuentasRamas = this.MisCuentasRamasYContables.Where(x => !x.Imputable).OrderBy(x=>x.NumeroCuenta).ToList();
                this.ddlRama.DataSource = this.MisCuentasRamas;
                this.ddlRama.DataValueField = "IdCuentaContable";
                this.ddlRama.DataTextField = "Descripcion";
                this.ddlRama.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlRama, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                //this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiCuentaContable.CodigoMensaje));
                this.MostrarMensaje(codigoMsg, false);
                this.pnlCuentaContable.Visible = false;
                
                AyudaProgramacion.SortTreeNodes(this.tvCuentasContables.Nodes);
                if (this.tvCuentasContables.SelectedNode != null)
                    this.tvCuentasContables.SelectedNode.Selected = false;
                
                this.upCuentasRamas.Update();
            }
            else
            {
                this.MostrarMensaje(this.MiCuentaContable.CodigoMensaje, true, this.MiCuentaContable.CodigoMensajeArgs);
            }
        }

        //protected void ddlCapitulo_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var valor = this.MisCapitulos[this.ddlCapitulo.SelectedIndex].CodigoCapitulo;
        //    this.ConcatenarNumeroCuenta(valor, 0); 
        //}

        //protected void ddlDepartamento_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var valor = this.MisDespartamentos[this.ddlDepartamento.SelectedIndex].CodigoDepartamento;
        //    this.ConcatenarNumeroCuenta(valor, 3);
        //}

        //protected void ddlMoneda_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var valor = this.MisMonedas[this.ddlMoneda.SelectedIndex].CodigoMoneda;
        //    this.ConcatenarNumeroCuenta(valor, 2);
        //}

        //protected void ddlRubro_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var valor = this.MisRubros[this.ddlRubro.SelectedIndex].CodigoRubro;
        //    this.ConcatenarNumeroCuenta(valor, 1);
        //}

        //protected void ddlSubRubro_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    var valor = this.MisSubRubros[this.ddlSubRubro.SelectedIndex].CodigoSubRubro;
        //    this.ConcatenarNumeroCuenta(valor, 4);
        //}

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            //if (this.CuentaContableDatosCancelar != null)
            //    this.CuentaContableDatosCancelar();
            if(this.tvCuentasContables.SelectedNode !=null)
                this.tvCuentasContables.SelectedNode.Selected = false;
            this.pnlCuentaContable.Visible = false;
            this.upCuentaContable.Update();
            this.pnlGrillaCuentas.Visible = false;
            this.upGrillaCuentas.Update();
        }

        //protected void popUpMensajes_popUpMensajesPostBackAceptar()
        //{
        //    this.pnlCuentaContable.Visible = true;
        //    this.pnlGrillaCuentas.Visible = false;
        //}

        #endregion

        protected void ddlEjerciciosContables_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.pnlGrillaCuentas.Visible)
            {
                this.pnlGrillaCuentas.Visible = false;
                this.upGrillaCuentas.Update();
            }

            if (this.pnlCuentaContable.Visible)
            {
                this.pnlCuentaContable.Visible = false;
                this.upCuentaContable.Update();
            }

            this.tvCuentasContables.Nodes.Clear();

            if (!string.IsNullOrEmpty(this.ddlEjercicioContable.SelectedValue))
            {

                CtbCuentasContables filtro = new CtbCuentasContables();
                filtro.IdEjercicioContable = Convert.ToInt32(this.ddlEjercicioContable.SelectedValue.ToString());

                this.MisCuentasRamasYContables = ContabilidadF.CuentasContablesObtenerListaRamaPorIdEjercicio(filtro);
                if (this.MisCuentasRamasYContables.Count > 0)
                {
                    this.MisCuentasRamas = this.MisCuentasRamasYContables.Where(x => !x.Imputable).ToList();
                    this.CargarCombos();
                    TreeNode nodo = null;
                    CtbCuentasContables cuentaRama = new CtbCuentasContables();
                    this.CargarCuentasRamas(cuentaRama, this.MisCuentasRamasYContables, nodo);
                }
            }
            
            this.upCuentasRamas.Update();
        }
    }
}