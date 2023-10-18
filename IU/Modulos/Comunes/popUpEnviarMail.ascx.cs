using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using Generales.Entidades;
using System.IO;
using Generales.FachadaNegocio;
using Comunes.Entidades;
using Mail.Serialization;
using Comunes.LogicaNegocio;
using Auditoria;
using Servicio.Conectividad;
using System.Text;
using Reportes.FachadaNegocio;

namespace IU.Modulos.Comunes
{
    public partial class popUpEnviarMail : ControlesSeguros
    {
        private SerializeableMailMessage MiMail
        {
            get { return (SerializeableMailMessage)Session[this.MiSessionPagina + "popUpEnviarMailMiMail"]; }
            set { Session[this.MiSessionPagina + "popUpEnviarMailMiMail"] = value; }
        }

        private Objeto MiRefOjeto
        {
            get { return (Objeto)Session[this.MiSessionPagina + "popUpEnviarMailMiRefOjeto"]; }
            set { Session[this.MiSessionPagina + "popUpEnviarMailMiRefOjeto"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
        }

        public void IniciarControl(MailMessage mail, Objeto pRefObj)
        {
            this.txtEnviarA.Text = string.Empty;
            string separador = string.Empty;
            foreach (MailAddress ma in mail.To.ToList())
            {
                this.txtEnviarA.Text = string.Concat(this.txtEnviarA.Text, separador, ma.Address);
                separador = "; ";
            }

            this.gvDatos.DataSource = mail.Attachments;
            this.gvDatos.DataBind();

            this.txtAsunto.Text = mail.Subject;
            this.txtMensaje.Text = mail.Body;

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalEnviarMailShow", "ShowModalEnviarMail();", true);
            this.MiMail = new SerializeableMailMessage(mail);
            this.MiRefOjeto = pRefObj;
            //this.MiMail = new MailMessageWrapper(mail);
            //this.MiMail = mail;
        }

        protected void btnAceptar_Click(object sender, EventArgs e)
        {
            try
            {
                TGEEmpresas empresa = TGEGeneralesF.EmpresasSeleccionar();
                MailMessage mail = this.MiMail.GetMailMessage();

                string[] direcciones = this.txtEnviarA.Text.Trim().Replace(',', ';').Split(';');
                for (int i = 0; i < mail.To.Count; i++)
                    if (!direcciones.ToList().Exists(x => x.Trim() == mail.To[i].Address.Trim()))
                        mail.To.RemoveAt(i);

                foreach(string a in direcciones.ToList())
                    if(!mail.To.ToList().Exists(x=>x.Address.Trim()==a.Trim()))
                        mail.To.Add(new MailAddress(a));

                mail.Subject = this.txtAsunto.Text.Trim();
                mail.Body = this.txtMensaje.Text;

                //bool resultado = AuditoriaF.MailsEnviosAgregar(mail, this.MiRefOjeto);
                bool resultado = new Email().Enviar(mail, this.MiRefOjeto);

                if (resultado)
                {
                    this.MostrarMensaje("EnviarMailOk", false);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalEnviarMailHide", "HideModalEnviarMail();", true);
                }
                else
                {
                    this.MostrarMensaje(this.MiRefOjeto.CodigoMensaje, true, this.MiRefOjeto.CodigoMensajeArgs);
                    AyudaProgramacionLN.LimpiarMensajesError(this.MiRefOjeto);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "popUpModalEnviarMailRemove", "RemoveModalBackground(); ShowModalEnviarMail();", true);
                }
            }
            catch (Exception ex)
            {
                this.MostrarMensaje(ex.Message, true);
            }
            finally
            {
            }
        }

        protected void gvDatos_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if ((e.CommandName == "Sort" || e.CommandName == "Page"))
                return;

            int index = Convert.ToInt32(e.CommandArgument);
            string name = ((GridView)sender).DataKeys[index].Value.ToString();
            MailMessage mail = this.MiMail.GetMailMessage();
            Attachment attach = mail.Attachments.First(x => x.Name == name);
                        
            if (e.CommandName == Gestion.Consultar.ToString())
            {
                List<TGEArchivos> listaArchivos = new List<TGEArchivos>();
                TGEArchivos archivo = new TGEArchivos();
                archivo.Archivo = StreamToByteArray(attach.ContentStream);
                listaArchivos.Add(archivo);
                ExportPDF.ExportarPDF(archivo.Archivo, this.upComprobante, "VTAResumenCuenta", this.UsuarioActivo);

            }
            this.MiMail = new SerializeableMailMessage(mail);
        }
        protected void btnCancelar_Click(object sender, EventArgs e)
        {
            StringBuilder script = new StringBuilder();
            script.Append("$('body').removeClass('modal-open');");
            script.AppendLine("$('.modal-backdrop').remove();");
            script.AppendLine("$(\"[id$='modalEnviarMail']\").modal('hide');");


            ScriptManager.RegisterStartupScript(this, this.GetType(), "popUpModalParametrosEnviarMail", script.ToString(), true);


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
    }
}