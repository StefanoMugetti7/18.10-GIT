using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.Data;
using Comunes.LogicaNegocio;
using System.IO;

namespace IU.Modulos.Comunes
{
    public partial class ImportarArchivo : ControlesSeguros
    {
        public delegate void ImportarArchivoAceptarEventHandler(object sender, DataTable e);
        public event ImportarArchivoAceptarEventHandler ImportarArchivoDatosAceptar;

        //public delegate void ImportarArchivoCancelarEventHandler();
        //public event ImportarArchivoCancelarEventHandler ImportarArchivoDatosCancelar;

        private DataTable Datos
        {
            get { return (DataTable)Session[this.MiSessionPagina + "ImportarArchivo"]; }
            set { Session[this.MiSessionPagina + "ImportarArchivo"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);

            if (!this.IsPostBack)
            {
                this.Datos = new DataTable();
            }
        }

        /// <summary>
        /// Nombres de columnas del excel a importar separadas por ,
        /// </summary>
        /// <param name="pColumnasArchivos"></param>
        public void IniciarControl(string pColumnasArchivos)
        {
            this.lblColumnasDetalles.Text = pColumnasArchivos;
            this.lblCantidad.Visible = false;
        }

        protected void uploadComplete_Action(object sender, EventArgs e)
        {
            var excel = new ExcelPackage(new MemoryStream(this.StreamToByteArray(this.afuArchivo.FileContent)));
            this.Datos = ExcelPackageExtensions.ToDataTable(excel);

            if (this.Datos.Rows.Count == 0)
            {
                this.MostrarMensaje("ValidarArchivo", true);
                return;
            }

            List<string> columnas = this.lblColumnasDetalles.Text.Trim().Split(',').ToList();
            string val;
            foreach (string col in columnas)
            {
                val = col.Trim();
                if (!this.Datos.Columns.Contains(val))
                {
                    this.MostrarMensaje("ValidarArchivo", true, new List<string>() { val });
                    return;
                }
            }
            this.afuArchivo.FailedValidation = false;
            this.afuArchivo.ClearAllFilesFromPersistedStore();
        }

        protected void button_Click(object sender, EventArgs e)
        {
            if (this.ImportarArchivoDatosAceptar != null)
                this.ImportarArchivoDatosAceptar(sender, this.Datos);
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