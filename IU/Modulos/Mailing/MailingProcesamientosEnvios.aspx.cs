using AjaxControlToolkit;
using Comunes.Entidades;
using Generales.Entidades;
using Generales.FachadaNegocio;
using IU.Modulos.ProcesosDatos.Controles;
using Mailing;
using Mailing.Entidades;
using ProcesosDatos;
using ProcesosDatos.Entidades;
using Reportes.Entidades;
using Servicio.AccesoDatos;
using SharpCompress.Compressor.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IU.Modulos.Mailing
{
    public partial class MailingProcesamientosEnvios : PaginaSegura
    {
        private DataTable MisDatosMailing
        {
            get { return (DataTable)Session[this.MiSessionPagina + "MailingProcesamientosEnviosMisDatosMailing"]; }
            set { Session[this.MiSessionPagina + "MailingProcesamientosEnviosMisDatosMailing"] = value; }
        }
        private List<TGEMailingProcesos> MiMailingProceso
        {
            get { return (List<TGEMailingProcesos>)Session[this.MiSessionPagina + "TGEMailingDatosMiMailingProceso"]; }
            set { Session[this.MiSessionPagina + "TGEMailingDatosMiMailingProceso"] = value; }
        }

        private TGEMailing MiMailing
        {
            get { return (TGEMailing)Session[this.MiSessionPagina + "TGEMailingDatosMiMailing"]; }
            set { Session[this.MiSessionPagina + "TGEMailingDatosMiMailing"] = value; }
        }

        //protected TGEMailingProcesamiento MiMailing.MailingProcesamiento
        //{
        //    get { return (TGEMailingProcesamiento)Session[this.MiSessionPagina + "MailingProcesamientoModificarDatosMiMailing.MailingProcesamiento"]; }
        //    set { Session[this.MiSessionPagina + "MailingProcesamientoModificarDatosMiMailing.MailingProcesamiento"] = value; }
        //}

        private List<TGEMailingProcesamientosPlantillas> MisPlantillas
        {
            get { return (List<TGEMailingProcesamientosPlantillas>)Session[this.MiSessionPagina + "TGEMailingProcesamientosPlantillasMisPlantillas"]; }
            set { Session[this.MiSessionPagina + "TGEMailingProcesamientosPlantillasMisPlantillas"] = value; }
        }
       
        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                btnAceptar.Visible = false;

                MiMailing = new TGEMailing();
                if (this.MisParametrosUrl.Contains("IdMailingProcesamiento"))
                { MiMailing.MailingProcesamiento.IdMailingProcesamiento = Convert.ToInt32(MisParametrosUrl["IdMailingProcesamiento"]); }

                TGEPlantillas filtro = new TGEPlantillas();
        

                MiMailingProceso = MailingF.TGEMailingObtenerListaMailingProceso();
                this.ddlProceso.DataSource = MiMailingProceso;
                this.ddlProceso.DataValueField = "IdMailingProceso";
                this.ddlProceso.DataTextField = "Descripcion";
                this.ddlProceso.DataBind();
                if (this.ddlProceso.Items.Count != 1)
                    AyudaProgramacion.InsertarItemSeleccione(this.ddlProceso, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                else
                    ddlProceso_OnSelectedIndexChanged(null, EventArgs.Empty);

                filtro.Filtro = "Comprobantes";
                this.ddlAdjuntos.DataSource = MailingF.PlantillasObtenerLista(filtro);
                this.ddlAdjuntos.DataValueField = "IdPlantilla";
                this.ddlAdjuntos.DataTextField = "NombrePlantilla";
                this.ddlAdjuntos.DataBind();
                AyudaProgramacion.AgregarItemSeleccione(this.ddlAdjuntos, this.ObtenerMensajeSistema("SeleccioneOpcion"));

                TGEMailing parametros = this.BusquedaParametrosObtenerValor<TGEMailing>();
                if (parametros.BusquedaParametros)
                {
                    this.ddlProceso.SelectedValue = parametros.IdMailing == 0 ? string.Empty : parametros.IdMailing.ToString();
                    this.ddlPlantilla.SelectedValue = parametros.Plantillas.IdPlantilla == 0 ? string.Empty : parametros.Plantillas.IdPlantilla.ToString();
                    this.txtAsunto.Text = parametros.Asunto;
                    MiMailing.BusquedaParametros = false;
                    this.BusquedaParametrosGuardarValor<TGEMailing>(MiMailing);
                    // this.CargarLista(parametros);

                }

                

            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.IsPostBack)
            {
                if (this.MiMailing.MailingProcesamiento != null && this.MiMailing.MailingProcesos.IdMailingProceso > 0)
                {
                    this.ObtenerValoresRequestForm(this.MiMailing.MailingProcesos);
                    this.ArmarTablaParametros(this.MiMailing.MailingProcesos);
                }
            }
        }

        private void MapearControlesAObjeto(TGEMailing pMailing)
        {
            List<TGEMailingParametros> parametros = new List<TGEMailingParametros>();
            parametros = pMailing.MailingProcesos.Parametros;

            pMailing.Asunto = txtAsunto.Text;
            pMailing.MailingProcesos = MiMailingProceso.FirstOrDefault(x => x.IdMailingProceso == Convert.ToInt32(this.ddlProceso.SelectedValue));// == string.Empty ? 0 : Convert.ToInt32(this.ddlProceso.SelectedValue);
            pMailing.MailingProcesos.Parametros = parametros;
            pMailing.Plantillas.IdPlantilla = this.ddlPlantilla.SelectedValue == string.Empty ? 0 : Convert.ToInt32(this.ddlPlantilla.SelectedValue);
            pMailing.Asunto = txtAsunto.Text;

            /*POR AHORA UN SOLO ADJUNTO*/
            if (!string.IsNullOrEmpty(ddlAdjuntos.SelectedValue))
            {
                TGEMailingProcesamientosAdjuntos mailingAdjuntos;
                if (MiMailing.MailingProcesamientosAdjuntos.Exists(x => x.Plantilla.IdPlantilla == Convert.ToInt32(ddlAdjuntos.SelectedValue)))
                {
                    mailingAdjuntos = MiMailing.MailingProcesamientosAdjuntos.First(x => x.Plantilla.IdPlantilla == Convert.ToInt32(ddlAdjuntos.SelectedValue));
                    mailingAdjuntos.EstadoColeccion = EstadoColecciones.SinCambio;
                }
                else
                {
                    mailingAdjuntos = new TGEMailingProcesamientosAdjuntos();
                    mailingAdjuntos.Estado.IdEstado = (int)Estados.Activo;
                    mailingAdjuntos.EstadoColeccion = EstadoColecciones.Agregado;
                    mailingAdjuntos.Plantilla.IdPlantilla = Convert.ToInt32(ddlAdjuntos.SelectedValue);
                    mailingAdjuntos.Plantilla.NombrePlantilla = ddlAdjuntos.SelectedItem.Text;
                    MiMailing.MailingProcesamientosAdjuntos.Add(mailingAdjuntos);
                }
            }
            else
            {
                MiMailing.MailingProcesamientosAdjuntos = new List<TGEMailingProcesamientosAdjuntos>();
            }

        }
        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //bool guardo = true;
            //if (!this.Page.IsValid)
            //    return;
            
            //this.MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //this.MiMailing.FechaAlta = DateTime.Now;
            //guardo = MailingF.TGEMailingModificar(this.MiMailing);
            //if (guardo)
            //{
            //    //this.MostrarMensaje(this.MiMailing.CodigoMensaje, false);
            //}
            //else
            //{
            //    this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
            //}
        }

        protected void gvDatos_RowCreated(object sender, GridViewRowEventArgs e)
        {

            GridViewRow row = e.Row;
            // Intitialize TableCell list
            List<TableCell> columns = new List<TableCell>();
            foreach (DataControlField column in gvDatos.Columns)
            {
                //Get the first Cell /Column
                TableCell cell = row.Cells[0];
                // Then Remove it after
                row.Cells.Remove(cell);
                //And Add it to the List Collections
                columns.Add(cell);
            }

            // Add cells
            row.Cells.AddRange(columns.ToArray());
        }

        protected void gvDatos_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                CheckBox chkIncluir = (CheckBox)e.Row.FindControl("chkIncluir");
                chkIncluir.Checked = true;
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            AudMailsEnvios mailsEnvios = new AudMailsEnvios();
            mailsEnvios.IdMailEnvio = Convert.ToInt32(((GridView)sender).DataKeys[index]["IdMailEnvio"].ToString());

            if (e.CommandName == Gestion.Consultar.ToString())
            {
                ctrVistaPrevia.IniciarControl(mailsEnvios);
            }
        }
        protected void btnPrepararEnvio_Click(object sender, EventArgs e)
        {
            //bool guardo = true;
            ////this.MapearControlesAObjeto(this.MiMailing);
            //this.btnAceptar_Click(sender, e);
            //MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            //MiMailing.MailingProcesamiento.EnvioManual = true;
            //MiMailing.MailingProcesos.PruebaEnvio = false;
            bool guardo = true;
            //guardo = MailingF.TGEMailingEnviarMails(MiMailing);
            this.btnAceptar_Click(sender, e);
            
            MiMailing.Asunto = txtAsunto.Text;
            MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            ForEachParametros();

            this.MapearControlesAObjeto(this.MiMailing);
            //MiMailing.MailingProcesamiento = MiMailing.MailingProcesamiento;
            MiMailing.MailingProcesamiento.EnvioManual = true; 
            MiMailing.MailingProcesos.PruebaEnvio = false;
            MiMailing.MailingProcesos.IdMailingProceso = Convert.ToInt32(ddlProceso.SelectedValue);
            

            guardo = MailingF.TGEMailingEnviarMailsV2(MiMailing);
            if (guardo)
            {
                TGEMailing pParametro = new TGEMailing();
                pParametro.MailingProcesamiento.IdMailingProcesamiento = MiMailing.MailingProcesamiento.IdMailingProcesamiento;
              //  this.ArmarTablaParametros(this.MiMailing.MailingProcesamiento.Proceso);
                this.CargarLista(pParametro);
            }
            else
            {
                this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
                return;
            }
            //if (guardo)
            //{

            //}
            //else
            //{
            //    this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
            //    return;
            //}



           

        }

        protected void btnPruebaEnvio_Click(object sender, EventArgs e)
        {

            bool guardo = true;

            MiMailing.Asunto = txtAsunto.Text;
            MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            ForEachParametros();

            this.MapearControlesAObjeto(this.MiMailing);
            //MiMailing.MailingProcesamiento = MiMailing.MailingProcesamiento;
            MiMailing.MailingProcesamiento.EnvioManual = true;
            MiMailing.MailingProcesos.PruebaEnvio = true;
            MiMailing.IdMailing = Convert.ToInt32(ddlProceso.SelectedValue);


            guardo = MailingF.TGEMailingPruebaEnvio(MiMailing);
           
            if (guardo)
            {
                this.MostrarMensaje(this.MiMailing.CodigoMensaje, false, this.MiMailing.CodigoMensajeArgs);
            }
            else
            {
                this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
            }
        }


        protected void btnModificarCopiaPlantilla_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(ddlPlantilla.SelectedValue))
            {
                MiMailing.Plantillas.IdPlantilla = Convert.ToInt32(ddlPlantilla.SelectedValue);
                MiMailing.IdMailing = Convert.ToInt32(ddlProceso.SelectedValue);
                MiMailing.Asunto = txtAsunto.Text;
                MiMailing.BusquedaParametros = true;
                this.BusquedaParametrosGuardarValor<TGEMailing>(MiMailing);
                this.MisParametrosUrl = new Hashtable();
                this.MisParametrosUrl.Add("IdPlantilla", Convert.ToInt32(ddlPlantilla.SelectedValue));
                this.MisParametrosUrl.Add("IdMailingProcesamiento", Convert.ToInt32(MiMailing.MailingProcesamiento.IdMailingProcesamiento));
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingProcesamientosPlantillas.aspx"), true);
            }

        }
        //protected void PlantillaModificarAceptar(object sender, EventArgs e)
        //{

        //    MiMailing.IdMailing = Convert.ToInt32(ddlProceso.SelectedValue);
        //    MiMailing = MailingF.TGEMailingObtenerDatosCompletos(MiMailing);
        //    ddlProceso.SelectedValue = MiMailing.IdMailing.ToString();
        //    ddlPlantilla.SelectedValue = MiMailing.Plantillas.IdPlantilla.ToString();



        //}
        protected void ddlProceso_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            tablaParametros.Controls.Clear();
            MiMailing = new TGEMailing();
   
            if (!string.IsNullOrEmpty(ddlProceso.SelectedValue))
            {

                bool guardo = true;
                MiMailing = new TGEMailing();
                MiMailing.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                MiMailing.MailingProcesos.IdMailingProceso = Convert.ToInt32(ddlProceso.SelectedValue);
                MiMailing.IdEstado = 38;

                TGEPlantillas miPlantilla = new TGEPlantillas();
                miPlantilla.IdTipoProceso = Convert.ToInt32(ddlProceso.SelectedValue);
                miPlantilla.Filtro = "Mailing";
                MisPlantillas = MailingF.PlantillasObtenerLista(miPlantilla);
                this.ddlPlantilla.DataSource = MisPlantillas;
                this.ddlPlantilla.DataValueField = "IdPlantilla";
                this.ddlPlantilla.DataTextField = "NombrePlantilla";
                this.ddlPlantilla.DataBind();
                if (this.ddlPlantilla.Items.Count != 1)
                {
                    AyudaProgramacion.AgregarItemSeleccione(this.ddlPlantilla, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    ddlPlantilla.Enabled = true;
                }

                guardo = MailingF.TGEMailingAgregarMailingProcesamiento(MiMailing);
                if (!guardo)
                {
                    this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
                    return;
                }

                //TGEMailing mailingPlantilla = new TGEMailing();
                //mailingPlantilla.MailingProcesos.IdMailingProceso = Convert.ToInt32(ddlProceso.SelectedValue);
                //mailingPlantilla = MailingF.TGEMailingProcesosObtenerPlantilla(mailingPlantilla);
                //if (mailingPlantilla.Plantillas.IdPlantilla > 0)
                //    ddlPlantilla.SelectedValue = mailingPlantilla.Plantillas.IdPlantilla.ToString();
                //else
                //ddlPlantilla.SelectedValue = "";
                //    ddlPlantilla.Enabled = true;
             


                MiMailing.MailingProcesos = MailingF.MailingProcesosObtenerDatosCompletos(MiMailing);
                ddlPlantilla_OnSelectedIndexChanged(null, EventArgs.Empty);
                if (MiMailing.MailingProcesos.Parametros.Count > 0)
                {
                    this.ArmarTablaParametros(this.MiMailing.MailingProcesos);
                }

            }
            else
            {
                ddlPlantilla.SelectedValue = "";

            }

            btnPruebaEnvio.Visible = false;


        }

        protected void ddlPlantilla_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            bool guardo = true;
            bool GuardoPlantilla = true;
            if (!string.IsNullOrEmpty(ddlPlantilla.SelectedValue))
            {

                TGEMailingProcesamientosPlantillas plantilla = new TGEMailingProcesamientosPlantillas();                  
                plantilla = MisPlantillas.FirstOrDefault(x => x.IdPlantilla == Convert.ToInt32(this.ddlPlantilla.SelectedValue));
                plantilla.IdMailingProcesamiento = MiMailing.MailingProcesamiento.IdMailingProcesamiento;
                GuardoPlantilla = MailingF.PlantillasAgregar(plantilla);
          

                if (GuardoPlantilla)
                {
                    //TGEMailing pParametro = new TGEMailing();
                    //pParametro.MailingProcesamiento.IdMailingProcesamiento = MiMailing.MailingProcesamiento.IdMailingProcesamiento;
                    //this.CargarLista(pParametro);
                }
                else
                {
                    this.MostrarMensaje(this.MiMailing.CodigoMensaje, true, this.MiMailing.CodigoMensajeArgs);
                    return;
                }



            }

        }

        protected void ObtenerValoresRequestForm(TGEMailingProcesos proceso)
        {
            List<string> keys = this.Request.Form.AllKeys.Where(x => !string.IsNullOrEmpty(x) && x.Contains("$ModifDatos$")).ToList();
            foreach (TGEMailingParametros parametro in proceso.Parametros)
            {
                foreach (string k in keys)
                {
                    if (k.EndsWith(parametro.Parametro))
                    {
                        switch (parametro.TipoParametro.IdTipoParametro)
                        {
                            case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                                if (k.EndsWith("select2HdfValue" + parametro.Parametro))
                                    parametro.ValorParametro = this.Request.Form[k];
                                else if (k.EndsWith("select2HdfText" + parametro.Parametro))
                                    parametro.ValorParametroDescripcion = this.Request.Form[k];
                                break;
                        }
                    }
                }
            }
        }

        protected void ForEachParametros()
        {
            Control ctrl;
            DateTime fechaHasta;


            foreach (TGEMailingParametros parametro in this.MiMailing.MailingProcesos.Parametros)
            {
                foreach (Control tr in tablaParametros.Controls)
                {
                    ctrl = tr.FindControl(parametro.Parametro);
                    if (ctrl != null)
                    {
                        if (ctrl is TextBox)
                        {
                            if (parametro.TipoParametro.IdTipoParametro == (int)EnumRepTipoParametros.DateTime)
                            {
                                if(ctrl.ID.Trim().ToLower() == "fechadesde")
                                {
                                    fechaHasta = ((TextBox)ctrl).Text == string.Empty ? Convert.ToDateTime("01/01/1975") : Convert.ToDateTime(((TextBox)ctrl).Text);
                                    parametro.ValorParametro = fechaHasta.ToShortDateString();
                                }
                                else if(ctrl.ID.Trim().ToLower() == "fechahasta")
                                {
                                    fechaHasta = ((TextBox)ctrl).Text == string.Empty ? DateTime.MaxValue : Convert.ToDateTime(((TextBox)ctrl).Text);
                                    parametro.ValorParametro = fechaHasta.ToShortDateString();
                                }
                                
                                //parametro.ValorParametroDescripcion = fechaHasta.ToShortDateString();
                            }
                            else
                            {
                                parametro.ValorParametro = ((TextBox)ctrl).Text;
                                //parametro.ValorParametroDescripcion = ((TextBox)ctrl).Text;
                            }
                            ctrl = null;
                            break;
                        }
                        else if (ctrl is YearCombo)
                        {
                            parametro.ValorParametro = ((YearCombo)ctrl).SelectedValue;
                            parametro.ValorParametroDescripcion = ((YearCombo)ctrl).SelectedItem.Text;
                            //MiProceso.Anio = ((YearCombo)ctrl).SelectedValue;
                            break;
                        }
                        else if (ctrl is MonthCombo)
                        {
                            parametro.ValorParametro = ((MonthCombo)ctrl).SelectedValue;
                            parametro.ValorParametroDescripcion = ((MonthCombo)ctrl).SelectedItem.Text;
                            //MiProceso.Mes = ((MonthCombo)ctrl).SelectedValue;
                            break;
                        }
                        else if (ctrl is DropDownList)
                        {
                            parametro.ValorParametro = ((DropDownList)ctrl).SelectedValue;
                            parametro.ValorParametroDescripcion = ((DropDownList)ctrl).SelectedValue == string.Empty ? string.Empty : ((DropDownList)ctrl).SelectedItem.Text;
                            break;
                        }
                        //else if (ctrl is AsyncFileUpload)
                        //{
                        //    parametro.ValorParametro = ((AsyncFileUpload)ctrl).FileBytes;
                        //    parametro.ValorParametroDescripcion = ((AsyncFileUpload)ctrl).FileName;
                        //    break;
                        //}
                        else if (ctrl is CheckBoxList)
                        {
                            parametro.ValorParametro = string.Empty;
                            CheckBoxList chkList = (CheckBoxList)ctrl;
                            string separador = "";
                            foreach (ListItem item in chkList.Items)
                            {
                                if (item.Selected)
                                {
                                    parametro.ValorParametro = string.Concat(parametro.ValorParametro, separador, item.Value);
                                    parametro.ValorParametroDescripcion = string.Concat(parametro.ValorParametroDescripcion, separador, item.Text);
                                    separador = ",";
                                }
                            }
                            break;
                        }
                        else if (ctrl is GridViewCheckFecha)
                        {
                            GridViewCheckFecha gvCheckFecha = (GridViewCheckFecha)ctrl;
                            if (!gvCheckFecha.ObtenerValores(parametro))
                                return;
                            break;
                        }
                        else if (ctrl is GridViewCheckDinamico)
                        {
                            GridViewCheckDinamico gvCheckDinamico = (GridViewCheckDinamico)ctrl;
                            if (!gvCheckDinamico.ObtenerValores(parametro))
                                return;
                            break;
                        }
                    }
                }
            }
        }
        #region "ArmarControlesParametros"
        const string cssLabel = "col-sm-1 col-form-label";
        const string cssRow = "form-group row";
        const string cssCol12 = "col-sm-12";
        const string cssCol9 = "col-sm-9";
        const string cssCol8 = "col-sm-8";
        const string cssCol6 = "col-sm-6";
        const string cssCol4 = "col-sm-4";
        const string cssCol3 = "col-sm-3";
        const string cssCol2 = "col-sm-2";
        const string cssCol1 = "col-sm-1";
        private void ArmarTablaParametros(TGEMailingProcesos pProceso)
        {
            //this.tablaParametros = new Table();
            //this.pnlArchivo.Visible = pProceso.ProcesoArchivo.ProcesaArchivo;
            //this.Accordion1.Visible = pProceso.ProcesoArchivo.ProcesaArchivo;
            this.tablaParametros.Controls.Clear();

            int countControl = 0;
            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssRow;
            foreach (TGEMailingParametros parametro in pProceso.Parametros)
            {
                DataSet dsParametros = new DataSet();
                TGEMailingProcesos proc;
                TGEMailingParametros param;

                if (!string.IsNullOrEmpty(parametro.StoredProcedure)
                    && parametro.ParamDependiente.Trim() == string.Empty
                    && parametro.TipoParametro.IdTipoParametro != (int)EnumSisTipoParametros.DropDownListSPAutoComplete)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    proc = new TGEMailingProcesos();
                    proc.StoredProcedure = parametro.StoredProcedure;
                    if (parametro.ParamDependiente.Trim() == "#TodasAnteriores#")
                    {
                        foreach (TGEMailingParametros p in pProceso.Parametros.Where(x => x.Orden < parametro.Orden))
                        {
                            proc.Parametros.Add(p);
                        }
                    }
                    else
                    {
                        param = new TGEMailingParametros();
                        param = pProceso.Parametros.Find(x => x.Parametro == parametro.ParamDependiente);
                        if (param != null)
                        {
                            proc.Parametros.Add(param);
                        }
                    }
                    proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    dsParametros = MailingF.ProcesosObtenerDatosParametro(proc);
                }

                switch (parametro.TipoParametro.IdTipoParametro)
                {
                    case (int)EnumSisTipoParametros.Int:
                        pnlRow.Controls.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.CheckBoxList:
                        pnlRow.Controls.Add(AddCheckBoxListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.DropDownListSPAutoComplete:
                        pnlRow.Controls.Add(AddListBoxAutocompleteRow(parametro));
                        break;
                    case (int)EnumSisTipoParametros.GridViewCheckFecha:
                        pnlRow.Controls.Add(AddGridViewCheckFechaBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.DateTime:
                        pnlRow.Controls.Add(AddDateBoxRow(parametro.NombreParametro, parametro.Parametro));
                        //AddDateControl(parametro.NombreParametro, counter, parametro.Parametro);
                        break;
                    case (int)EnumSisTipoParametros.TextBox:
                        pnlRow.Controls.Add(AddTextBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.IntNumericInput:
                        pnlRow.Controls.Add(AddNumericInputBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.DateTimeRange:
                        pnlRow.Controls.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
                        break;
                    case (int)EnumSisTipoParametros.YearMonthCombo:
                        break;
                    case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                        pnlRow.Controls.Add(AddGridViewCheckDinamicoBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                        break;
                    default:
                        break;
                }
                parametro.ParametroDibujado = true;
                countControl++;

                if (countControl == 1)
                {
                    this.tablaParametros.Controls.Add(pnlRow);
                    countControl = 0;
                    //pnlRow = new Panel();
                    //pnlRow.CssClass = cssRow;
                }
            }
        }

        private void CargarValoresParametros(TGEMailingParametros parametroDatos, TGEMailingParametros parametroLlenar)
        {
            DataSet dsParametros = new DataSet();
            TGEMailingProcesos proc;
            ForEachParametros();
            if (!string.IsNullOrEmpty(parametroDatos.StoredProcedure)
                && parametroDatos.TipoParametro.IdTipoParametro != (int)EnumSisTipoParametros.DropDownListSPAutoComplete)
            {
                //Obtengo los valores para llenar las opciones del parametro
                proc = new TGEMailingProcesos();
               // proc.ProcesoArchivo.StoredProcedure = parametroDatos.StoredProcedure;
                proc.Parametros.Add(parametroDatos);
                dsParametros = MailingF.ProcesosObtenerDatosParametro(proc);
            }

            this.CargarValoresParametrosControl(parametroLlenar, parametroLlenar.Parametro, dsParametros);
        }

        private void CargarValoresParametrosControl(TGEMailingParametros parametroLlenar, string pNombreControl, DataSet dsParametros)
        {
            Control control;
            switch (parametroLlenar.TipoParametro.IdTipoParametro)
            {
                case (int)EnumSisTipoParametros.Int:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        DropDownList ddl = (DropDownList)control;
                        ddl.Items.Clear();
                        ddl.SelectedValue = null;
                        ddl.DataSource = dsParametros;
                        ddl.DataValueField = parametroLlenar.Parametro;
                        ddl.DataTextField = "Descripcion";
                        ddl.DataBind();
                        AyudaProgramacion.InsertarItemSeleccione(ddl, this.ObtenerMensajeSistema("SeleccioneOpcion"));
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));

                    break;
                case (int)EnumSisTipoParametros.CheckBoxList:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        CheckBoxList ddl = (CheckBoxList)control;
                        ddl.Items.Clear();
                        ddl.SelectedValue = null;
                        ddl.DataSource = dsParametros;
                        ddl.DataValueField = parametroLlenar.Parametro;
                        ddl.DataTextField = "Descripcion";
                        ddl.RepeatColumns = 1;
                        ddl.RepeatDirection = System.Web.UI.WebControls.RepeatDirection.Horizontal;
                        ddl.RepeatLayout = RepeatLayout.Flow;
                        ddl.AutoPostBack = true;
                        ddl.SelectedIndexChanged += new EventHandler(cheFiltro_SelectedIndexChanged);
                        ddl.DataBind();
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.GridViewCheckFecha:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        GridViewCheckFecha gv = (GridViewCheckFecha)control;
                        gv.IniciarControl(dsParametros, string.Empty, string.Empty);
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.GridViewCheckDinamico:
                    control = this.BuscarControlRecursivo(this.tablaParametros, parametroLlenar.Parametro);
                    if (control != null)
                    {
                        GridViewCheckDinamico gv = (GridViewCheckDinamico)control;
                        gv.IniciarControl(dsParametros, parametroLlenar.Parametro, parametroLlenar.NombreParametro);
                    }
                    //tablaParametros.Rows.Add(AddListBoxRow(parametro.NombreParametro, dsParametros, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.DateTime:
                    //tablaParametros.Rows.Add(AddDateBoxRow(parametro.NombreParametro, parametro.Parametro));

                    break;
                case (int)EnumSisTipoParametros.TextBox:
                    break;
                case (int)EnumSisTipoParametros.IntNumericInput:
                    break;
                case (int)EnumSisTipoParametros.DateTimeRange:
                    //tablaParametros.Rows.Add(AddDateRangeBoxRow(parametro.NombreParametro, parametro.Parametro));
                    break;
                case (int)EnumSisTipoParametros.YearMonthCombo:
                    break;
                default:
                    break;
            }
        }

        private PlaceHolder AddListBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;

            DropDownList drpFiltro = new DropDownList();
            drpFiltro.CssClass = "form-control select2";

            drpFiltro.DataValueField = Parametro;
            drpFiltro.DataTextField = "Descripcion";
            if (dsDatos.Tables.Count > 0)
            {
                drpFiltro.DataSource = dsDatos;
                drpFiltro.DataBind();
            }
            AyudaProgramacion.InsertarItemSeleccione(drpFiltro, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            drpFiltro.ID = Parametro;
            drpFiltro.SelectedIndexChanged += new EventHandler(drpFiltro_SelectedIndexChanged);
            drpFiltro.AutoPostBack = this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == Parametro)
                || this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");

            TextBox Text = new TextBox();
            Text.CssClass = "textbox";
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;

            pnlRow.Controls.Add(drpFiltro);
            pnlRow.Controls.Add(Text);
            miPanel.Controls.Add(pnlRow);

            return miPanel;
        }

        void drpFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TGEMailingParametros paramValor = this.MiMailing.MailingProcesos.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID);
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesos.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID);
            this.RecargarControles(paramValor, ((DropDownList)sender).SelectedValue);
        }   

        private void RecargarControles(TGEMailingParametros paramValor, string valor)
        {

            TGEMailingParametros paramLlenar = this.MiMailing.MailingProcesos.Parametros.Find(x => x.ParamDependiente == paramValor.Parametro);
            paramValor.ValorParametro = valor;
            if (paramLlenar != null)
            {
                TGEMailingParametros nuevo = new TGEMailingParametros();
                nuevo.Parametro = paramValor.Parametro;
                nuevo.TipoParametro = paramValor.TipoParametro;
                nuevo.StoredProcedure = paramLlenar.StoredProcedure;
                nuevo.ValorParametro = paramValor.ValorParametro;
                this.CargarValoresParametros(nuevo, paramLlenar);
            }

            if (this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden))
            {
                List<TGEMailingParametros> lstParamLlenar = this.MiMailing.MailingProcesos.Parametros.FindAll(x => x.ParamDependiente == "#TodasAnteriores#" && x.Orden > paramValor.Orden);
                TGEMailingProcesos proc;
                foreach (TGEMailingParametros p in lstParamLlenar)
                {
                    //Obtengo los valores para llenar las opciones del parametro
                    proc = new TGEMailingProcesos();
                   // proc.ProcesoArchivo.StoredProcedure = p.StoredProcedure;
                    foreach (TGEMailingParametros depParam in this.MiMailing.MailingProcesos.Parametros.Where(x => x.Orden < p.Orden))
                    {
                        //this.CargarParametroValor(depParam);
                        proc.Parametros.Add(depParam);
                    }
                    ForEachParametros();
                    proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                    DataSet dsParametros = MailingF.ProcesosObtenerDatosParametro(proc);
                    this.CargarValoresParametrosControl(p, p.Parametro, dsParametros);
                }
            }
        }

        private PlaceHolder AddCheckBoxListBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel panelCheckboxList = new Panel();
            panelCheckboxList.CssClass = cssCol3 + " overflow-auto";
            panelCheckboxList.Style.Add("max-height", "200px");
            CheckBoxList cheFiltro = new CheckBoxList();
            cheFiltro.RepeatLayout = RepeatLayout.Flow;
            cheFiltro.CssClass = "form-check";
            cheFiltro.DataValueField = Parametro;
            cheFiltro.DataTextField = "Descripcion";
            if (dsDatos.Tables.Count > 0)
            {
                cheFiltro.DataSource = dsDatos;
                cheFiltro.RepeatLayout = RepeatLayout.Flow;
                cheFiltro.AutoPostBack = true;
                cheFiltro.SelectedIndexChanged += new EventHandler(cheFiltro_SelectedIndexChanged);
                cheFiltro.DataBind();
            }
            cheFiltro.ID = Parametro;

            panelCheckboxList.Controls.Add(cheFiltro);
            miPanel.Controls.Add(panelCheckboxList);

            TextBox Text = new TextBox();
            Text.CssClass = "textbox";
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;
            panelCheckboxList.Controls.Add(Text);

            return miPanel;
        }

        void cheFiltro_SelectedIndexChanged(object sender, EventArgs e)
        {
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesos.Parametros.Find(x => x.Parametro == ((CheckBoxList)sender).ID);

            paramValor.ValorParametro = string.Empty;
            CheckBoxList chkList = (CheckBoxList)sender;
            string separador = "";
            foreach (ListItem item in chkList.Items)
            {
                if (item.Selected)
                {
                    paramValor.ValorParametro = string.Concat(paramValor.ValorParametro, separador, item.Value);
                    paramValor.ValorParametroDescripcion = string.Concat(paramValor.ValorParametroDescripcion, separador, item.Text);
                    separador = ",";
                }
            }
            this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
        }

        void btnCheckbox_Click(object sender, EventArgs e)
        {
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesos.Parametros.Find(x => "btnChk" + x.Parametro == ((Button)sender).ID);
            this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
        }

        private PlaceHolder AddListBoxAutocompleteRow(TGEMailingParametros parametro)
        {
            PlaceHolder panel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //panel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + parametro.Parametro;
            lbl.Text = parametro.NombreParametro;
            panel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            DropDownList ddlListaOpciones = new DropDownList();
            ddlListaOpciones.ID = parametro.Parametro;
            ddlListaOpciones.CssClass = "form-control select2";
            ddlListaOpciones.DataValueField = parametro.Parametro;
            ddlListaOpciones.DataTextField = "Descripcion";

            ddlListaOpciones.SelectedIndexChanged += new EventHandler(drpFiltro_SelectedIndexChanged);
            ddlListaOpciones.AutoPostBack = this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == parametro.Parametro)
                || this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");

            //Busco el detalle en la BD
            if (parametro.ValorParametro.ToString().Trim().Length > 0)
            {
                TGECamposValores campoValor = new TGECamposValores();
                campoValor.Valor = parametro.ValorParametro.ToString().Trim();
                DataSet ds = BaseDatos.ObtenerBaseDatos().ObtenerDataSet(parametro.StoredProcedure, parametro);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    parametro.ValorParametroDescripcion = ds.Tables[0].Rows[0]["Descripcion"].ToString();

                ListItem item = new ListItem(parametro.ValorParametroDescripcion, parametro.ValorParametro.ToString());
                ddlListaOpciones.Items.Add(item);
            }

            HiddenField hdfValue = new HiddenField();
            hdfValue.ID = "select2HdfValue" + parametro.Parametro;
            hdfValue.Value = parametro.ValorParametro.ToString();
            HiddenField hdfText = new HiddenField();
            hdfText.ID = "select2HdfText" + parametro.Parametro;
            hdfText.Value = parametro.ValorParametroDescripcion;

            pnlRow.Controls.Add(ddlListaOpciones);
            pnlRow.Controls.Add(hdfValue);
            pnlRow.Controls.Add(hdfText);
            panel.Controls.Add(pnlRow);

            StringBuilder script = new StringBuilder();
            script.AppendLine(" $(document).ready(function () {");
            //start function
            script.AppendLine(" function InitControlSelect2|CTRLID|() {");
            script.AppendFormat("var control = $('select[name$={0}]');", ddlListaOpciones.ID);

            //select2 start
            script.AppendLine("control.select2({");
            script.AppendFormat("placeholder: '{0}',", parametro.NombreParametro);
            script.AppendLine("minimumInputLength: 4,");
            script.AppendLine("language: 'es',");
            script.AppendLine("allowClear: true,");
            script.AppendLine("ajax: {");
            script.AppendLine("type: 'POST',");
            script.AppendLine("contentType: 'application/json; charset=utf-8',");
            script.AppendLine("dataType: 'json',");
            script.AppendFormat("url: '{0}',", ResolveUrl("~/Modulos/Comunes/CamposValoresWS.asmx/ListaGenerica"));
            script.AppendLine("delay: 250,");
            script.AppendLine("data: function (params) {");
            script.AppendLine("return JSON.stringify( {");
            script.AppendLine("value: control.val(), // search term");
            script.AppendLine("filtro: params.term, // search term");
            script.AppendFormat("sp: '{0}',", parametro.StoredProcedure);
            //script.AppendFormat("idRefTablaValor: '{0}',", this.MiIdRefTablaValor);
            script.AppendLine(" });");
            script.AppendLine("},");
            script.AppendLine("processResults: function (data, params) {");
            script.AppendLine("return {");
            script.AppendLine("results: data.d,");
            script.AppendLine("};");
            script.AppendLine(" cache: true");
            script.AppendLine("},");
            script.AppendLine("}");
            script.AppendLine("});");
            //end select2
            //select2 ON Select
            script.AppendLine("control.on('select2:select', function(e) { ");
            script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
            script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
            script.AppendLine("hdfValue.val(e.params.data.id);");
            script.AppendLine("hdfText.val(e.params.data.text);");
            script.AppendLine("});");
            //end select2 ON
            //select2 ON unselect
            script.AppendLine("control.on('select2:unselect', function(e) { ");
            script.AppendFormat("var hdfValue = $('input[name$={0}]');", hdfValue.ID);
            script.AppendFormat("var hdfText = $('input[name$={0}]');", hdfText.ID);
            //script.AppendLine("control.val(null);");
            script.AppendLine("hdfValue.val(null);");
            script.AppendLine("hdfText.val(null);");
            script.AppendLine("});");
            //end select2 unselect

            script.AppendLine("};");
            //end function
            script.AppendLine("Sys.WebForms.PageRequestManager.getInstance().add_endRequest(InitControlSelect2|CTRLID|);");
            script.AppendLine("InitControlSelect2|CTRLID|();");
            script.AppendLine("});");

            script.Replace("|CTRLID|", ddlListaOpciones.ID);

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Script" + ddlListaOpciones.ID.ToString(), script.ToString(), true);

            return panel;
        }

        private PlaceHolder AddGridViewCheckFechaBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder panel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //panel.Controls.Add(pnlCol);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol9;
            GridViewCheckFecha gvCheckFecha = (GridViewCheckFecha)LoadControl("~/Modulos/ProcesosDatos/Controles/GridviewCheckFecha.ascx");
            gvCheckFecha.ID = Parametro;
            gvCheckFecha.IniciarControl(dsDatos, Parametro, NombreParametro);
            pnlRow.Controls.Add(gvCheckFecha);

            panel.Controls.Add(pnlRow);
            return panel;
        }

        private PlaceHolder AddGridViewCheckDinamicoBoxRow(string NombreParametro, DataSet dsDatos, string Parametro)
        {
            PlaceHolder panel = new PlaceHolder();
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesos.Parametros.Find(x => x.Parametro == Parametro);
            if (paramValor.ParamDependiente.Trim() == "#BotonBuscar#")
            {
                Panel pnlButtonLeft = new Panel();
                pnlButtonLeft.CssClass = cssCol4;
                panel.Controls.Add(pnlButtonLeft);

                Panel pnlButton = new Panel();
                pnlButton.CssClass = cssCol8;
                Button btnGVDinamico = new Button();
                btnGVDinamico.ID = "btnGVDinamico" + Parametro;
                btnGVDinamico.CssClass = "botonesEvol";
                btnGVDinamico.Text = NombreParametro;
                btnGVDinamico.Click += BtnGVDinamico_Click;
                pnlButton.Controls.Add(btnGVDinamico);
                panel.Controls.Add(pnlButton);
            }
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //panel.Controls.Add(pnlCol);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol12;
            GridViewCheckDinamico gvCheck = (GridViewCheckDinamico)LoadControl("~/Modulos/ProcesosDatos/Controles/GridViewCheckDinamico.ascx");
            gvCheck.ID = Parametro;
            gvCheck.IniciarControl(dsDatos, Parametro, NombreParametro);
            pnlRow.Controls.Add(gvCheck);
            panel.Controls.Add(pnlRow);
            return panel;
        }

        private void BtnGVDinamico_Click(object sender, EventArgs e)
        {
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesos.Parametros.Find(x => "btnGVDinamico" + x.Parametro == ((Button)sender).ID);
            Control gv = BuscarControlRecursivo(((Button)sender).Parent.Parent, paramValor.Parametro);
            if (gv != null)
            {
                List<TGEMailingParametros> lstParamLlenar = this.MiMailing.MailingProcesos.Parametros.FindAll(x => x.Orden < paramValor.Orden);
                TGEMailingProcesos proc;
                //Obtengo los valores para llenar las opciones del parametro
                proc = new TGEMailingProcesos();
               // proc.ProcesoArchivo.StoredProcedure = paramValor.StoredProcedure;
                proc.Parametros.AddRange(lstParamLlenar);
                ForEachParametros();
                proc.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
                DataSet dsParametros = MailingF.ProcesosObtenerDatosParametro(proc);
                GridViewCheckDinamico _gv = (GridViewCheckDinamico)gv;
                _gv.IniciarControl(dsParametros, paramValor.Parametro, paramValor.NombreParametro);
            }
        }

        private PlaceHolder AddMultipleListBoxRow(string NombreParametro, DataSet dsDatos, int Counter, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;

            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            ListBox lstMultipleList = new ListBox();
            lstMultipleList.CssClass = "form-control";
            lstMultipleList.ID = "MultipleListBox" + Counter.ToString();
            lstMultipleList.SelectionMode = ListSelectionMode.Multiple;
            lstMultipleList.Rows = 8;

            pnlRow.Controls.Add(lstMultipleList);
            //tc2.CssClass = "TableCell";

            TextBox Text = new TextBox();
            Text.CssClass = "textbox";
            Text.ID = "txt" + Parametro;
            Text.Visible = false;
            Text.Text = Parametro;
            pnlRow.Controls.Add(Text);

            miPanel.Controls.Add(pnlRow);
            return miPanel;
        }

        private PlaceHolder AddTextBoxRow(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            TextBox Text = new TextBox();
            Text.CssClass = "form-control";
            Text.ID = "txt1" + Parametro;
            TextBox Text2 = new TextBox();
            Text2.CssClass = "textbox";
            Text2.ID = "txt2" + Parametro;
            Text2.Visible = false;
            Text2.Text = Parametro;
            pnlRow.Controls.Add(Text);

            pnlRow.Controls.Add(Text2);
            miPanel.Controls.Add(pnlRow);

            return miPanel;
        }

        private PlaceHolder AddNumericInputBoxRow(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            TextBox txtNumerico = new TextBox();
            txtNumerico.ID = Parametro;
            FilteredTextBoxExtender fte = new FilteredTextBoxExtender();
            fte.TargetControlID = Parametro;
            fte.FilterType = FilterTypes.Numbers;
            txtNumerico.CssClass = "form-control";

            if (this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == Parametro)
               || this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#"))
            {
                txtNumerico.AutoPostBack = true;
                txtNumerico.TextChanged += TxtNumerico_TextChanged;
            }

            pnlRow.Controls.Add(txtNumerico);

            miPanel.Controls.Add(pnlRow);
            return miPanel;
        }

        private void TxtNumerico_TextChanged(object sender, EventArgs e)
        {
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesos.Parametros.Find(x => x.Parametro == ((TextBox)sender).ID);
            this.RecargarControles(paramValor, ((TextBox)sender).Text);
        }

        private PlaceHolder AddDateRangeBoxRow(string NombreParametro, string Parametro)
        {
            return new PlaceHolder();
            //Panel tr = new Panel();

            //Label lbl = new Label();
            //lbl.CssClass = "label";
            //lbl.ID = "lbl" + Parametro;
            //lbl.Text = NombreParametro;
            //tr.Controls.Add(lbl);

            //// RANGO DESDE
            //TextBox dateSelectorDesde = new TextBox();
            //// TODO:Cambiar año
            //dateSelectorDesde.Text = DateTime.Now.AddYears(-1).ToShortDateString();
            //dateSelectorDesde.CssClass = "textbox";
            //dateSelectorDesde.Columns = 10;
            //dateSelectorDesde.ID = "dtDesde" + Parametro;
            //tr.Controls.Add(dateSelectorDesde);

            //TextBox txtBoxDesde = new TextBox();
            //txtBoxDesde.Text = "@Rango" + Parametro.Replace("@", "") + "Inicial";
            //txtBoxDesde.CssClass = "textbox";
            //txtBoxDesde.ID = "TxtBoxDesde" + Parametro;
            //txtBoxDesde.Visible = false;
            //tr.Controls.Add(txtBoxDesde);

            //Literal ltrl = new Literal();
            //ltrl.Text = "  ";
            //tr.Controls.Add(ltrl);

            //Image imgCalendarDesde = new Image();
            //imgCalendarDesde.ImageUrl = "~/Imagenes/Calendario.png";
            //imgCalendarDesde.ID = "imgCalendarDesde" + Parametro;
            //tr.Controls.Add(imgCalendarDesde);

            //AjaxControlToolkit.CalendarExtender fechaCalendarDesde = new AjaxControlToolkit.CalendarExtender();
            //fechaCalendarDesde.Format = "dd/MM/yyyy";
            //fechaCalendarDesde.ID = "fechaCalendarDesde" + Parametro;
            //fechaCalendarDesde.TargetControlID = dateSelectorDesde.ID;
            //fechaCalendarDesde.PopupButtonID = imgCalendarDesde.ClientID;
            //tr.Controls.Add(fechaCalendarDesde);

            //Literal ltrl2 = new Literal();
            //ltrl2.Text = "  ";
            //tr.Controls.Add(ltrl2);

            //// RANGO HASTA
            //TextBox dateSelectorHasta = new TextBox();
            //dateSelectorHasta.Text = DateTime.Now.AddYears(1).ToShortDateString();
            //dateSelectorHasta.Columns = 10;
            //dateSelectorHasta.CssClass = "textbox";
            //dateSelectorHasta.ID = "dtHasta" + Parametro;
            //tr.Controls.Add(dateSelectorHasta);

            //TextBox txtBoxHasta = new TextBox();
            //txtBoxHasta.Text = "@Rango" + Parametro.Replace("@", "") + "Final";
            //txtBoxHasta.CssClass = "TextBox";
            //txtBoxHasta.ID = "TxtBoxHasta" + Parametro;
            //txtBoxHasta.Visible = false;
            //tr.Controls.Add(txtBoxHasta);

            //Literal ltrl1 = new Literal();
            //ltrl1.Text = "  ";
            //tr.Controls.Add(ltrl1);

            //Image imgCalendarHasta = new Image();
            //imgCalendarHasta.ImageUrl = "~/Imagenes/Calendario.png";
            //imgCalendarHasta.ID = "imgCalendarHasta" + Parametro;
            //tr.Controls.Add(imgCalendarHasta);

            //AjaxControlToolkit.CalendarExtender fechaCalendarHasta = new AjaxControlToolkit.CalendarExtender();
            //fechaCalendarHasta.Format = "dd/MM/yyyy";
            //fechaCalendarHasta.ID = "fechaCalendarHasta" + Parametro;
            //fechaCalendarHasta.TargetControlID = dateSelectorHasta.ID;
            //fechaCalendarHasta.PopupButtonID = imgCalendarHasta.ID;
            //tr.Controls.Add(fechaCalendarHasta);

            //AjaxControlToolkit.MaskedEditExtender fechaExtenderDesde = new AjaxControlToolkit.MaskedEditExtender();
            //fechaExtenderDesde.Mask = "99/99/9999";
            //fechaExtenderDesde.ID = "fechaExtenderDesde" + Parametro;
            //fechaExtenderDesde.MaskType = AjaxControlToolkit.MaskedEditType.Date;
            //fechaExtenderDesde.TargetControlID = dateSelectorDesde.ID;
            ////fechaExtenderDesde.CultureName = "es-AR";
            //fechaExtenderDesde.UserDateFormat = MaskedEditUserDateFormat.DayMonthYear;
            //tr.Controls.Add(fechaExtenderDesde);

            //AjaxControlToolkit.MaskedEditExtender fechaExtenderHasta = new AjaxControlToolkit.MaskedEditExtender();
            //fechaExtenderHasta.Mask = "99/99/9999";
            //fechaExtenderHasta.ID = "fechaExtenderHasta" + Parametro;
            //fechaExtenderHasta.MaskType = AjaxControlToolkit.MaskedEditType.Date;
            //fechaExtenderHasta.TargetControlID = dateSelectorHasta.ID;
            ////fechaExtenderHasta.CultureName = "es-AR";
            //fechaExtenderHasta.UserDateFormat = MaskedEditUserDateFormat.DayMonthYear;
            //tr.Controls.Add(fechaExtenderHasta);

            //AjaxControlToolkit.MaskedEditValidator fechaValidatorDesde = new AjaxControlToolkit.MaskedEditValidator();
            //fechaValidatorDesde.ID = "fechaValidatorDesde" + Parametro;
            //fechaValidatorDesde.MaximumValue = "31/12/2100";
            //fechaValidatorDesde.MinimumValue = "01/01/1950";
            //fechaValidatorDesde.Display = ValidatorDisplay.Dynamic;
            //fechaValidatorDesde.EmptyValueMessage = "&nbsp;Fechas obligatorias";
            //fechaValidatorDesde.IsValidEmpty = false;
            //fechaValidatorDesde.ControlExtender = fechaExtenderDesde.ID;
            //fechaValidatorDesde.ControlToValidate = dateSelectorDesde.ID;
            //fechaValidatorDesde.InvalidValueMessage = "&nbsp;Formato inválido";
            //fechaValidatorDesde.MinimumValueMessage = "&nbsp;El valor mínimo es 01/01/1950";
            //fechaValidatorDesde.MaximumValueMessage = "&nbsp;El valor máximo es 31/12/2100";
            //tr.Controls.Add(fechaValidatorDesde);

            //AjaxControlToolkit.MaskedEditValidator fechaValidatorHasta = new AjaxControlToolkit.MaskedEditValidator();
            //fechaValidatorHasta.ID = "fechaValidatorHasta" + Parametro;
            //fechaValidatorHasta.MaximumValue = "31/12/2100";
            //fechaValidatorHasta.MinimumValue = "01/01/1950";
            //fechaValidatorHasta.Display = ValidatorDisplay.Dynamic;
            //fechaValidatorHasta.EmptyValueMessage = "&nbsp;Fechas obligatorias";
            //fechaValidatorHasta.IsValidEmpty = false;
            //fechaValidatorHasta.ControlExtender = fechaExtenderHasta.ID;
            //fechaValidatorHasta.ControlToValidate = dateSelectorHasta.ID;
            //fechaValidatorHasta.InvalidValueMessage = "&nbsp;Formato inválido";
            //fechaValidatorHasta.MinimumValueMessage = "&nbsp;El valor mínimo es 01/01/1950";
            //fechaValidatorHasta.MaximumValueMessage = "&nbsp;El valor máximo es 31/12/2100";
            //tr.Controls.Add(fechaValidatorHasta);

            //return tr;
        }

        void BtnPostBack_Click(object sender, EventArgs e)
        {
            string id = ((Button)sender).ID.Replace("btnPostBack", string.Empty);
            TGEMailingParametros paramValor = this.MiMailing.MailingProcesos.Parametros.Find(x => x.Parametro == id);
            Control txt = BuscarControlRecursivo(((Button)sender).Parent, id);
            if (txt != null && txt is TextBox)
                paramValor.ValorParametro = ((TextBox)txt).Text;
            List<TGEMailingParametros> parametrosLlenar = this.MiMailing.MailingProcesos.Parametros.FindAll(x => x.ParamDependiente == paramValor.Parametro);
            if (parametrosLlenar != null)
            {
                //TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((DropDownList)sender).ID);
                this.RecargarControles(paramValor, ((TextBox)txt).Text);
            }


        }

        private PlaceHolder AddDateBoxRow(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            //Panel pnlCol = new Panel();
            //pnlCol.CssClass = cssCol1;
            //miPanel.Controls.Add(pnlCol);

            Label lbl = new Label();
            lbl.CssClass = cssLabel;
            lbl.ID = "lbl" + Parametro;
            lbl.Text = NombreParametro;
            miPanel.Controls.Add(lbl);

            Panel pnlRow = new Panel();
            pnlRow.CssClass = cssCol3;
            TextBox dateSelector = new TextBox();
            //if (Parametro == "FechaDesde")
            //    dateSelector.Text = DateTime.Now.AddDays(-DateTime.Now.Day + 1).ToShortDateString();
            //else
            //    dateSelector.Text = DateTime.Now.ToShortDateString();
            dateSelector.CssClass = "form-control datepicker";
            dateSelector.Columns = 10;
            dateSelector.ID = Parametro;
            //dateSelector.AutoPostBack = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == Parametro)
            //    || this.MiMailing.MailingProcesamiento.Proceso.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#");
            //if (dateSelector.AutoPostBack)
            //{
            //    dateSelector.TextChanged += DateSelector_TextChanged;
            //}
            pnlRow.Controls.Add(dateSelector);
            miPanel.Controls.Add(pnlRow);
            Panel pnlCol4 = new Panel();
            pnlCol4.CssClass = cssCol3;
            pnlRow.Controls.Add(pnlCol4);

            if (this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == Parametro)
               || this.MiMailing.MailingProcesos.Parametros.Exists(x => x.ParamDependiente == "#TodasAnteriores#"))
            {
                Button btn = new Button();
                btn.ID = dateSelector.ID + "btnPostBack";
                btn.Style.Add("visibility", "hidden");
                btn.Click += BtnPostBack_Click;
                HiddenField hdfDate = new HiddenField();
                hdfDate.Value = dateSelector.Text;
                hdfDate.ID = dateSelector.ID + "hdfDate";
                pnlRow.Controls.Add(btn);
                pnlRow.Controls.Add(hdfDate);
                dateSelector.Attributes.Add("onchange", "return btnPostBackClick" + dateSelector.ID + "();");

                StringBuilder scriptBtn = new StringBuilder();
                scriptBtn.AppendLine("function btnPostBackClick|CTRLID|(ctrl) {");
                scriptBtn.AppendFormat("var hdfDate = $('input[id$={0}]');", hdfDate.ID);
                scriptBtn.AppendFormat("var txtFecha = $('input[id$={0}]');", dateSelector.ID);
                scriptBtn.AppendLine(" if (txtFecha.val()!=hdfDate.val()) {");
                scriptBtn.AppendLine(" hdfDate.val(txtFecha.val());");
                scriptBtn.AppendFormat("$(\"[id$='{0}']\").click();", btn.ID);
                //scriptBtn.AppendLine("__doPostBack(btn.name, '');");
                scriptBtn.AppendLine("}");
                scriptBtn.AppendLine("}");
                scriptBtn.Replace("|CTRLID|", dateSelector.ID);
                ScriptManager.RegisterClientScriptBlock(this.UpdatePanel2, this.UpdatePanel2.GetType(), "ScriptAgregar" + btn.ID, scriptBtn.ToString(), true);
            }


            return miPanel;
        }

        //private void DateSelector_TextChanged(object sender, EventArgs e)
        //{
        //    TGEMailingParametros paramValor = this.MiMailing.MailingProcesamiento.Proceso.Parametros.Find(x => x.Parametro == ((TextBox)sender).ID);

        //    paramValor.ValorParametro = string.Empty;
        //    TextBox chkList = (TextBox)sender;
        //    paramValor.ValorParametro = chkList.Text;

        //    this.RecargarControles(paramValor, paramValor.ValorParametro.ToString());
        //}

        private PlaceHolder AddYearMonthCombo(string NombreParametro, string Parametro)
        {
            PlaceHolder miPanel = new PlaceHolder();
            return miPanel;
            //Panel tr = new Panel();

            //Label lbl = new Label();
            //lbl.CssClass = "label";
            //lbl.ID = "lbl" + Parametro;
            //lbl.Text = NombreParametro;
            //tr.Controls.Add(lbl);

            //YearMonthCombo YMCYear = new YearMonthCombo();
            //YMCYear.ID = "Year" + Parametro;

            //for (int i = 1900; i < 2100; i++)
            //{
            //    YMCYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //}

            //YearMonthCombo YMCMonth = new YearMonthCombo();
            //YMCMonth.ID = "Month" + Parametro;

            //YMCMonth.Items.Add(new ListItem("Enero", "01"));
            //YMCMonth.Items.Add(new ListItem("Febrero", "02"));
            //YMCMonth.Items.Add(new ListItem("Marzo", "03"));
            //YMCMonth.Items.Add(new ListItem("Abril", "04"));
            //YMCMonth.Items.Add(new ListItem("Mayo", "05"));
            //YMCMonth.Items.Add(new ListItem("Junio", "06"));
            //YMCMonth.Items.Add(new ListItem("Julio", "07"));
            //YMCMonth.Items.Add(new ListItem("Agosto", "08"));
            //YMCMonth.Items.Add(new ListItem("Septiembre", "09"));
            //YMCMonth.Items.Add(new ListItem("Octubre", "10"));
            //YMCMonth.Items.Add(new ListItem("Noviembre", "11"));
            //YMCMonth.Items.Add(new ListItem("Diciembre", "12"));

            //YMCYear.Text = DateTime.Now.Year.ToString();
            //YMCMonth.Text = (DateTime.Now.Month < 10 ? ("0" + DateTime.Now.Month.ToString()) : DateTime.Now.Month.ToString());

            //TextBox Text = new TextBox();
            //Text.CssClass = "textbox";
            //Text.ID = "TxtBox" + Parametro;
            //Text.Visible = false;
            //Text.Text = Parametro;

            //tr.Controls.Add(YMCYear);
            //tr.Controls.Add(YMCMonth);
            //tr.Controls.Add(Text);

            //return tr;
        }

        #endregion

        protected void btnProcesarEnvio_Click(object sender, EventArgs e)
        {
            bool guardo = true;
            //if (!this.Page.IsValid)
            //    return;
            this.btnAceptar.Visible = false;

            TGEMailing mailingEnvioManual = new TGEMailing();

            mailingEnvioManual.MailingProcesamiento.IdMailingProcesamiento = MiMailing.MailingProcesamiento.IdMailingProcesamiento;

            //mailingEnvioManual.LoteMailingEnvioManual = new XmlDocument();
            //XmlNode mailingNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("MailingEnvioManual");
            //mailingEnvioManual.LoteMailingEnvioManual.AppendChild(mailingNode);
            //DataTable enviar = MisDatosMailing.Copy();

            int idTable;
            DataRow dr;
            foreach (GridViewRow pre in this.gvDatos.Rows)
            {
                if (pre.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkIncluir = (CheckBox)pre.FindControl("chkIncluir");
                    if (!chkIncluir.Checked)
                    {
                        idTable = Convert.ToInt32(this.gvDatos.DataKeys[pre.DataItemIndex]["IdMailEnvio"].ToString());
                        dr = this.MisDatosMailing.AsEnumerable().First(x => x.Field<Int32>("IdMailEnvio") == idTable);
                        //dr.Delete();
                        MisDatosMailing.Rows.Remove(dr);
                        MisDatosMailing.AcceptChanges();
                    }
                }
            }

            #region Grilla
            //XmlNode mNode;
            //XmlNode ValorNode;
            //int cantidad = 0;
            //            cantidad++;
            //            idTable = Convert.ToInt32(this.gvDatos.DataKeys[pre.DataItemIndex]["IdMailEnvio"].ToString());
            //            dr = this.MisDatosMailing.AsEnumerable().First(x => x.Field<Int32>("IdMailEnvio") == idTable);
            //            mNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("EnvioManual");

            //            ValorNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("Para");
            //            ValorNode.InnerText = dr["Para"].ToString();
            //            mNode.AppendChild(ValorNode);
            //            ValorNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("Nombre");
            //            ValorNode.InnerText = dr["Nombre"].ToString();
            //            mNode.AppendChild(ValorNode);
            //            ValorNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("Asunto");
            //            ValorNode.InnerText = dr["Asunto"].ToString();
            //            mNode.AppendChild(ValorNode);
            //            ValorNode = mailingEnvioManual.LoteMailingEnvioManual.CreateElement("Cuerpo");
            //            ValorNode.InnerText = dr["Cuerpo"].ToString();
            //            mNode.AppendChild(ValorNode);

            //            mailingNode.AppendChild(mNode);
            //        }
            //    }
            //}
            //if (cantidad == 0)
            //{
            //    this.btnAceptar.Visible = true;
            //    this.MostrarMensaje("ValidarCantidadItems", true);
            //    return;
            //}
            #endregion

            guardo = MailingF.TGEMailingEnviarMailsSeleccionados(MisDatosMailing, mailingEnvioManual);

            if (guardo)
            {
                this.MostrarMensaje(mailingEnvioManual.CodigoMensaje, false, mailingEnvioManual.CodigoMensajeArgs);
            }
            else
            {
                this.btnAceptar.Visible = true;
                this.MostrarMensaje(mailingEnvioManual.CodigoMensaje, true, mailingEnvioManual.CodigoMensajeArgs);

            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            if (this.ViewState["UrlReferrer"] != null)
                this.Response.Redirect(this.ViewState["UrlReferrer"].ToString(), true);
            else
                Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/Modulos/Mailing/MailingListar.aspx"), true);
        }

        private void CargarLista(TGEMailing pParametro)
        {
            MisDatosMailing = MailingF.TGEMailingObtenerMailsAEnviar(pParametro);
            this.gvDatos.DataSource = this.MisDatosMailing;
            this.gvDatos.DataBind();
            if (MisDatosMailing.Rows.Count > 0)
            {
                btnAceptar.Visible = true;
                btnPruebaEnvio.Visible = true;
            }
            else
            {

                btnAceptar.Visible = false;
                btnPruebaEnvio.Visible = false;
            }
        }

    }
}