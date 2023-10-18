using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Medicina.Entidades;
using Comunes.Entidades;
using Medicina;
using Generales.FachadaNegocio;
using Afiliados;
using Generales.Entidades;
using System.IO;
using System.Drawing;
using Comunes.LogicaNegocio;

namespace IU.Modulos.Medicina.Controles
{
    public partial class PrestadoresModificarDatos : ControlesSeguros
    {
        private MedPrestadores MiPrestador
        {
            get { return (MedPrestadores)Session[this.MiSessionPagina + "PrestadoresModificarDatosMiPrestador"]; }
            set { Session[this.MiSessionPagina + "PrestadoresModificarDatosMiPrestador"] = value; }
        }

        private byte[] Firma
        {
            get { return (byte[])Session[this.MiSessionPagina + "PrestadorFirmaModificar"]; }
            set { Session[this.MiSessionPagina + "PrestadorFirmaModificar"] = value; }
        }

        private TGEArchivos MitmpArchivo
        {
            get
            {
                return (Session[this.MiSessionPagina + "ArchivosMitmpArchivo"] == null ?
                    (TGEArchivos)(Session[this.MiSessionPagina + "ArchivosMitmpArchivo"] = new TGEArchivos()) : (TGEArchivos)Session[this.MiSessionPagina + "ArchivosMitmpArchivo"]);
            }
            set { Session[this.MiSessionPagina + "ArchivosMitmpArchivo"] = value; }
        }

        private int MiIndiceDetalleModificar
        {
            get { return (int)Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"]; }
            set { Session[this.MiSessionPagina + "AfiliadoModificarDatosMiIndiceDetalleModificar"] = value; }
        }

        public delegate void PrestadoresModificarDatosAceptarEventHandler(MedPrestadores e);
        public event PrestadoresModificarDatosAceptarEventHandler PrestadoresModificarDatosAceptar;
        public delegate void PrestadoresModificarDatosCancelarEventHandler();
        public event PrestadoresModificarDatosCancelarEventHandler PrestadoresModificarDatosCancelar;

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            this.popUpMensajes.popUpMensajesPostBackAceptar += new IU.Modulos.Comunes.popUpMensajesPostBack.popUpMensajesPostBackAceptarEventHandler(popUpMensajes_popUpMensajesPostBackAceptar);
            this.ctrEspecialidades.EspecializacionesModificarDatosAceptar += new EspecializacionesModificarDatosPopUp.EspecializacionesModificarDatosEventHandler(ctrEspecialidades_EspecializacionesModificarDatosAceptar);
            this.ctrDiasHoras.DiasHorasModificarDatosAceptar += new DiasHorasModificarDatosPopUp.DiasHorasModificarDatosEventHandler(ctrDiasHoras_DiasHorasModificarDatosAceptar);
            base.PageLoadEvent(sender, e);
            if (this.IsPostBack)
            {
                //this.paginaSegura.scriptManager.Scripts.Add(new ScriptReference("~/Recursos/jquery.ptTimeSelect.js"));

                if (this.MiPrestador == null)
                {
                    this.Response.Redirect(AyudaProgramacion.ObtenerUrlParametros("~/ControlSesion.aspx"), true);
                }
            }
        }



        void popUpMensajes_popUpMensajesPostBackAceptar()
        {
            if (this.PrestadoresModificarDatosAceptar != null)
                this.PrestadoresModificarDatosAceptar(this.MiPrestador);
        }

        /// <summary>
        /// Inicializa el control de Alta y Modificación de un Afiliado
        /// </summary>
        /// <param name="pRequisicion"></param>
        public void IniciarControl(MedPrestadores pPrestador, Gestion pGestion)
        {
            this.GestionControl = pGestion;
            this.MiPrestador = pPrestador;
            this.btnAceptar.Visible = true;
            this.btnAgregarEspecialidad.Visible = this.ValidarPermiso("EspecializacionesModificar.aspx");
            this.btnAgregarDiasHoras.Visible = this.ValidarPermiso("DiasHorasModificar.aspx");
            this.CargarCombos();


            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    dvFirma.Visible = false;
                    dvFirmaCrear.Visible = true;
                    this.ddlEstados.SelectedValue = ((int)Estados.Activo).ToString();
                    this.ddlEstados.Enabled = false;
                    this.txtFechaIngreso.Text = DateTime.Now.ToShortDateString();
                     this.ctrArchivos.IniciarControl(this.MiPrestador, this.GestionControl);
                     this.ctrCamposValores.IniciarControl(this.MiPrestador, new Objeto(), this.GestionControl);
                    break;
                case Gestion.Modificar:
                    this.MiPrestador = MedicinaF.PrestadoresObtenerDatosCompletos(pPrestador);
                    this.MapearObjetoAControles(this.MiPrestador);
                    break;
                case Gestion.Consultar:
                    btnEliminarFirma.Visible = false;
                    btnAgregarFirma.Visible = false;
                    btnClean.Visible = false;
                    this.MiPrestador = MedicinaF.PrestadoresObtenerDatosCompletos(pPrestador);
                    this.MapearObjetoAControles(this.MiPrestador);
                    AyudaProgramacion.HabilitarControles(this, false, this.paginaSegura);
                    this.btnAceptar.Visible = false;
                    this.btnAgregarEspecialidad.Visible = false;
                    this.btnAgregarDiasHoras.Visible = false;
                    this.btnAgregarDiasHoras.Visible = false;
                    break;
                default:
                    break;
            }
            
        }

       
        /// <summary>
        /// Mapea la Entidad SolicitudesMateriales a los controles de Pantalla
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearObjetoAControles(MedPrestadores pPrestador)
        {
            this.ddlEstados.SelectedValue = pPrestador.Estado.IdEstado.ToString();
            this.txtApellido.Text = pPrestador.Apellido;
            this.txtNombre.Text = pPrestador.Nombre;
            this.ddlTipoDocumento.SelectedValue = pPrestador.TipoDocumento.IdTipoDocumento.ToString();
            this.txtNumeroDocumento.Text = pPrestador.NumeroDocumento.ToString();
            this.txtCUIL.Text = pPrestador.CUIL.ToString();
            this.ddlSexo.SelectedValue = pPrestador.Sexo.IdSexo==0? string.Empty : pPrestador.Sexo.IdSexo.ToString();
            this.txtFechaNacimiento.Text = AyudaProgramacion.MostrarFechaPantalla(pPrestador.FechaNacimiento);
            this.txtFechaIngreso.Text = AyudaProgramacion.MostrarFechaPantalla(pPrestador.FechaIngreso);
            this.ddlEstadoCivil.SelectedValue = pPrestador.EstadoCivil.IdEstadoCivil.ToString();
            this.txtMatricula.Text = pPrestador.Matricula.ToString();
            this.txtCorreoElectronico.Text = pPrestador.CorreoElectronico;
            //this.ddlGrupoSanquineo.SelectedValue = pAfiliado.GrupoSanguieno.IdGrupoSanguienio.ToString();     
            this.txtFechaBaja.Text = AyudaProgramacion.MostrarFechaPantalla(pPrestador.FechaBaja);

            if (MiPrestador.Firma != null)
            {
                Firma = MiPrestador.Firma;
            }
                MostrarFirma(null, EventArgs.Empty);

            ListItem sexo = this.ddlSexo.Items.FindByValue(pPrestador.Sexo.IdSexo.ToString());
            if (sexo == null && pPrestador.Sexo.IdSexo > 0)
                this.ddlSexo.Items.Add(new ListItem(pPrestador.Sexo.Descripcion, pPrestador.Sexo.IdSexo.ToString()));
            this.ddlSexo.SelectedValue = pPrestador.Sexo.IdSexo == 0 ? string.Empty : pPrestador.Sexo.IdSexo.ToString();

            // FOTO
            // FIRMA
            
            AyudaProgramacion.CargarGrillaListas(pPrestador.PrestadoresEspecializaciones, true, this.gvEspecialidades, true);
            AyudaProgramacion.CargarGrillaListas(pPrestador.PrestadoresDiasHoras,true, this.gvDiasHoras, true);

            this.ctrComentarios.IniciarControl(pPrestador, this.GestionControl);
            this.ctrArchivos.IniciarControl(pPrestador, this.GestionControl);
            this.ctrAuditoria.IniciarControl(pPrestador);
            this.ctrCamposValores.IniciarControl(this.MiPrestador, new Objeto(), this.GestionControl);
        }

        /// <summary>
        /// Mapea los controles de Pantalla con la Entidad SolicitudesMateriales.
        /// </summary>
        /// <param name="pRequisicion"></param>
        private void MapearControlesAObjeto(MedPrestadores pPrestador)
        {
            pPrestador.Estado.IdEstado = Convert.ToInt32(this.ddlEstados.SelectedValue);
            pPrestador.Estado.Descripcion = this.ddlEstados.SelectedItem.Text;
            pPrestador.Apellido = this.txtApellido.Text;
            pPrestador.Nombre = this.txtNombre.Text;
            pPrestador.TipoDocumento.IdTipoDocumento = Convert.ToInt32( this.ddlTipoDocumento.SelectedValue);
            pPrestador.NumeroDocumento = Convert.ToInt32( this.txtNumeroDocumento.Text);
            pPrestador.CUIL = this.txtCUIL.Text == string.Empty ? 0 : Convert.ToInt64(this.txtCUIL.Text);
            pPrestador.Sexo.IdSexo = Convert.ToInt32(this.ddlSexo.SelectedValue);
            pPrestador.FechaNacimiento = Convert.ToDateTime( this.txtFechaNacimiento.Text);
            pPrestador.FechaIngreso = Convert.ToDateTime(this.txtFechaIngreso.Text);
            pPrestador.EstadoCivil.IdEstadoCivil = Convert.ToInt32( this.ddlEstadoCivil.SelectedValue);
            pPrestador.Matricula = this.txtMatricula.Text;
            pPrestador.CorreoElectronico = this.txtCorreoElectronico.Text;
            //pAfiliado.GrupoSanguieno.IdGrupoSanguienio = Convert.ToInt32( this.ddlGrupoSanquineo.SelectedValue );
            pPrestador.FechaBaja = this.txtFechaBaja.Text == string.Empty ? default(DateTime?) : Convert.ToDateTime(this.txtFechaBaja.Text);
            // FOTO
            // FIRMA

            if (Firma != MiPrestador.Firma)
                MiPrestador.Firma = Firma;

            pPrestador.Comentarios = ctrComentarios.ObtenerLista();
            pPrestador.Archivos = ctrArchivos.ObtenerLista();
            pPrestador.Campos = this.ctrCamposValores.ObtenerLista();
        }

        /// <summary>
        /// Llena los DropDownList de la Pantalla con los datos
        /// </summary>
        private void CargarCombos()
        {
            this.ddlEstados.DataSource = TGEGeneralesF.TGEEstadosObtenerLista(typeof(Estados));
            this.ddlEstados.DataValueField = "IdEstado";
            this.ddlEstados.DataTextField = "Descripcion";
            this.ddlEstados.DataBind();

            this.ddlEstadoCivil.DataSource = AfiliadosF.EstadosCivilesObtenerLista();
            this.ddlEstadoCivil.DataValueField = "IdEstadoCivil";
            this.ddlEstadoCivil.DataTextField = "EstadoCivil";
            this.ddlEstadoCivil.DataBind();

            //this.ddlGrupoSanquineo.DataSource = "";
            //this.ddlGrupoSanquineo.DataValueField = "IdCategoria";
            //this.ddlGrupoSanquineo.DataTextField = "Categoria";
            //this.ddlGrupoSanquineo.DataBind();

            this.ddlSexo.DataSource = TGEGeneralesF.ListasValoresSistemasObtenerListaDetalle(EnumTGEListasValoresSistemas.Sexos);
            this.ddlSexo.DataValueField = "IdListaValorSistemaDetalle";
            this.ddlSexo.DataTextField = "Descripcion";
            this.ddlSexo.DataBind();
            AyudaProgramacion.AgregarItemSeleccione(this.ddlSexo, this.ObtenerMensajeSistema("SeleccioneOpcion"));

            this.ddlTipoDocumento.DataSource = AfiliadosF.TipoDocumentosObtenerLista();
            this.ddlTipoDocumento.DataValueField = "IdTipoDocumento";
            this.ddlTipoDocumento.DataTextField = "TipoDocumento";
            this.ddlTipoDocumento.DataBind();

        }


        protected void btnEliminarFirma_Click(object sender, EventArgs e)
        {
            dvFirmaCrear.Visible = true;
            dvFirma.Visible = false;
            Firma = null;

            if (Firma != MiPrestador.Firma)
            {

                string mensaje = this.ObtenerMensajeSistema("FirmaModificarPrestadorCancelar");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnCancelar.Attributes.Add("OnClick", funcion);

            }
        }

        protected void MostrarFirma(object sender, EventArgs e)
        {

            if (Firma != null)
            {
                dvFirmaCrear.Visible = false;
                dvFirma.Visible = true;
                System.Drawing.Bitmap img = new System.Drawing.Bitmap(new MemoryStream(Firma));

                string base64String = Convert.ToBase64String((byte[])new System.Drawing.ImageConverter().ConvertTo(img, typeof(byte[])));
                if (base64String != string.Empty)
                {
                    this.imgLogo.ImageUrl = "data:image/png;base64," + base64String;
                    this.imgLogo.Visible = true;

                }
            }
            else
            {
                dvFirmaCrear.Visible = true;
                dvFirma.Visible = false;
            }
        }

        protected void btnAgregarFirma_Click(object sender, EventArgs e)
        {
            string[] valores = this.hiddenSigData.Value.Split(',');

            if (valores.Length > 1)
            {
                //MiFirmarDocumento.Firma = Convert.FromBase64String(valores[1]);
                System.Drawing.Image img = Base64ToImage(valores[1]);
                //Bitmap bmp = new Bitmap(img);
                //bmp = Crop(bmp);
                //img = (System.Drawing.Image)bmp;
                //img = ScaleImage(img, 150, 50);
                //MiPrestador.Firma = ImageToByteArray(img);

                img = ScaleImage(img, 150, 50);
                ImageConverter imgCon = new ImageConverter();
                Firma = (byte[])imgCon.ConvertTo(img, typeof(byte[]));

                string mensaje = this.ObtenerMensajeSistema("FirmaModificarPrestadorCancelar");
                string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                btnCancelar.Attributes.Add("OnClick", funcion);

                MostrarFirma(null, EventArgs.Empty);
            }

        }

        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            this.MitmpArchivo.TipoArchivo = this.afuArchivo.ContentType;
            this.MitmpArchivo.Archivo = this.StreamToByteArray(this.afuArchivo.FileContent);
            
        }

        protected void btnPostBack(object sender, EventArgs e)
        {
            if (MitmpArchivo.TipoArchivo == "image/png" || MitmpArchivo.TipoArchivo == "image/jpeg")
            {
                string imgBase64 = Convert.ToBase64String(MitmpArchivo.Archivo, 0, MitmpArchivo.Archivo.Length);

                System.Drawing.Image img = Base64ToImage(imgBase64);
                img = ScaleImage(img, 300, 150);
                ImageConverter imgCon = new ImageConverter();
                Firma = (byte[])imgCon.ConvertTo(img, typeof(byte[]));

                MostrarFirma(null, EventArgs.Empty);
            }
            else
            {
                MostrarMensaje("ArchivoIngresadoIncorrecto", true);
            }
        }

        public System.Drawing.Image byteArrayToImage(byte[] source)
        {
            MemoryStream ms = new MemoryStream(source);
            System.Drawing.Image ret = System.Drawing.Image.FromStream(ms);
            return ret;
        }

        private byte[] StreamToByteArray(Stream inputStream)
        {
            if (!inputStream.CanRead)
            {
                throw new ArgumentException();
            }

            // This is optional
            if (inputStream.CanSeek)
            {
                inputStream.Seek(0, SeekOrigin.Begin);
            }

            byte[] output = new byte[inputStream.Length];
            int bytesRead = inputStream.Read(output, 0, output.Length);
            return output;
        }

        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            //this.PersistirDatos();
            this.Page.Validate("AfiliadosModificarDatosAceptar");
            if (!this.Page.IsValid)
                return;

            bool guardo = false;

            this.MapearControlesAObjeto(this.MiPrestador);

            //Firma

         


            //Firma Fin

            //this.ActualizarGrilla();
            this.MiPrestador.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(this.UsuarioActivo);
            switch (this.GestionControl)
            {
                case Gestion.Agregar:
                    this.MiPrestador.IdusuarioAlta = this.UsuarioActivo.IdUsuarioEvento;
                    this.MiPrestador.FechaAlta = DateTime.Now;
                    guardo = MedicinaF.PrestadoresAgregar(this.MiPrestador);
                    break;
                case Gestion.Modificar:
                    guardo = MedicinaF.PrestadoresModificar(this.MiPrestador);
                    break;
                default:
                    break;
            }

            if (guardo)
            {
                this.popUpMensajes.MostrarMensaje(this.ObtenerMensajeSistema(this.MiPrestador.CodigoMensaje));
            }
            else
            {
                this.MostrarMensaje(this.MiPrestador.CodigoMensaje, true, this.MiPrestador.CodigoMensajeArgs);
            }
        }

        protected void btnCancelar_Click(object sender, EventArgs e)
        {  
            if (this.PrestadoresModificarDatosCancelar != null)
                this.PrestadoresModificarDatosCancelar();
        }

        #region PrestadoresEspecializaciones

        protected void btnAgregarEspecialidad_Click(object sender, EventArgs e)
        {
            MedPrestadoresEspecializaciones especialidad = new MedPrestadoresEspecializaciones();
            especialidad.IdPrestador = this.MiPrestador.IdPrestador;

            ctrEspecialidades.IniciarControl(especialidad, ObtenerEspecializacionesPrestador(), Gestion.Agregar);
        }

        void ctrEspecialidades_EspecializacionesModificarDatosAceptar(MedPrestadoresEspecializaciones e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiPrestador.PrestadoresEspecializaciones.Add(e);
                    e.IndiceColeccion = this.MiPrestador.PrestadoresEspecializaciones.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiPrestador.PrestadoresEspecializaciones[this.MiIndiceDetalleModificar] = e;

                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiPrestador.PrestadoresEspecializaciones, true, this.gvEspecialidades, true);
            this.upEspecialidades.Update();

        }

        protected void gvEspecialidades_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;

            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrEspecialidades.IniciarControl(this.MiPrestador.PrestadoresEspecializaciones[indiceColeccion], this.ObtenerEspecializacionesPrestador(), Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrEspecialidades.IniciarControl(this.MiPrestador.PrestadoresEspecializaciones[indiceColeccion], this.ObtenerEspecializacionesPrestador(), Gestion.Consultar);
            }
        }

        protected void gvEspecialidades_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MedPrestadoresEspecializaciones item = (MedPrestadoresEspecializaciones)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("EspecialidadesConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region DiasHoras

        protected void gvDiasHoras_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            int indiceColeccion = Convert.ToInt32(((GridView)sender).DataKeys[index].Value.ToString());
            this.MiIndiceDetalleModificar = indiceColeccion;
            if (e.CommandName == Gestion.Modificar.ToString())
            {
                this.ctrDiasHoras.IniciarControl(this.MiPrestador.PrestadoresDiasHoras[indiceColeccion], this.ObtenerEspecializacionesPrestador(), Gestion.Modificar);
            }
            else if (e.CommandName == Gestion.Consultar.ToString())
            {
                this.ctrDiasHoras.IniciarControl(this.MiPrestador.PrestadoresDiasHoras[indiceColeccion], this.ObtenerEspecializacionesPrestador(), Gestion.Consultar);
            }
        }

        protected void gvDiasHoras_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                MedPrestadoresDiasHoras item = (MedPrestadoresDiasHoras)e.Row.DataItem;

                switch (GestionControl)
                {
                    case Gestion.Modificar:
                        ImageButton btnModificar = (ImageButton)e.Row.FindControl("btnModificar");
                        btnModificar.Visible = true;

                        if (item.EstadoColeccion == EstadoColecciones.Agregado
                            || item.EstadoColeccion == EstadoColecciones.AgregadoPrevio)
                        {
                            ImageButton ibtn = (ImageButton)e.Row.Cells[4].FindControl("btnEliminar");
                            string mensaje = this.ObtenerMensajeSistema("DiasHorasConfirmarBaja");
                            mensaje = string.Format(mensaje, string.Concat(e.Row.Cells[0].Text, " - ", e.Row.Cells[0].Text));
                            string funcion = string.Format("showConfirm(this,'{0}'); return false;", mensaje);
                            ibtn.Attributes.Add("OnClick", funcion);
                            ibtn.Visible = true;
                        }
                        break;
                    case Gestion.Consultar:
                        ImageButton btnConsultar = (ImageButton)e.Row.FindControl("btnConsultar");
                        btnConsultar.Visible = true;
                        break;
                    default:
                        break;
                }
            }
        }

        void ctrDiasHoras_DiasHorasModificarDatosAceptar(MedPrestadoresDiasHoras e, Gestion pGestion)
        {
            switch (pGestion)
            {
                case Gestion.Agregar:
                    this.MiPrestador.PrestadoresDiasHoras.Add(e);
                    e.IndiceColeccion = this.MiPrestador.PrestadoresDiasHoras.IndexOf(e);
                    break;
                case Gestion.Modificar:
                case Gestion.Anular:
                    this.MiPrestador.PrestadoresDiasHoras[this.MiIndiceDetalleModificar] = e;
                    break;
            }
            AyudaProgramacion.CargarGrillaListas(this.MiPrestador.PrestadoresDiasHoras, true, this.gvDiasHoras, true);
            this.upDiasHoras.Update();
        }

        protected void btnAgregarDiasHoras_Click(object sender, EventArgs e)
        {
            MedPrestadoresDiasHoras diasHoras = new MedPrestadoresDiasHoras();
            diasHoras.IdPrestador = this.MiPrestador.IdPrestador;
            this.ctrDiasHoras.IniciarControl(diasHoras, this.ObtenerEspecializacionesPrestador(), Gestion.Agregar);
        }

        private List<MedEspecializaciones> ObtenerEspecializacionesPrestador()
        {
            List<MedEspecializaciones> resultado = new List<MedEspecializaciones>();
            foreach (MedPrestadoresEspecializaciones item in this.MiPrestador.PrestadoresEspecializaciones)
                resultado.Add(item.Especializacion);
            return resultado;
        }

        #endregion


        #region Firma

        private static Bitmap Crop(Bitmap bmp)
        {
            int w = bmp.Width;
            int h = bmp.Height;

            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
                    if (bmp.GetPixel(i, row).R != 255)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                    if (bmp.GetPixel(col, i).R != 255)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            try
            {
                var target = new Bitmap(croppedWidth, croppedHeight);
                using (Graphics g = Graphics.FromImage(target))
                {
                    g.DrawImage(bmp,
                      new RectangleF(0, 0, croppedWidth, croppedHeight),
                      new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                      GraphicsUnit.Pixel);
                }
                return target;
            }
            catch (Exception ex)
            {
                throw new Exception(
                  string.Format("Values are topmost={0} btm={1} left={2} right={3} croppedWidth={4} croppedHeight={5}", topmost, bottommost, leftmost, rightmost, croppedWidth, croppedHeight),
                  ex);
            }
        }

        public System.Drawing.Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
                return image;
            }
        }

        public string ImageToBase64(System.Drawing.Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to base 64 string
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static System.Drawing.Image ScaleImage(System.Drawing.Image image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        //protected string ObtenerAppPath()
        //{
        //    return this.Page.Request.ApplicationPath.EndsWith("/") ? this.Page.Request.ApplicationPath : string.Concat(this.Page.Request.ApplicationPath, "/");
        //}

        #endregion

    }
}