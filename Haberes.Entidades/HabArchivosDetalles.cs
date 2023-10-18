using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Haberes.Entidades
{
    [Serializable]
    public partial class HabArchivosDetalles : Objeto
    {

        #region "Private Members"
        int _idArchivoDetalle;
        HabArchivosCabeceras _archivoCabecera;
        int _idAfiliado;
        byte[] _archivo;
        string _nombreArchivo;
        string _tipoArchivo;
        long _tamanio;

        #endregion

        #region "Constructors"
        public HabArchivosDetalles()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdArchivoDetalle
        {
            get { return _idArchivoDetalle; }
            set { _idArchivoDetalle = value; }
        }

        public HabArchivosCabeceras ArchivoCabecera
        {
            get { return _archivoCabecera == null ? (_archivoCabecera = new HabArchivosCabeceras()) : _archivoCabecera; }
            set { _archivoCabecera = value; }
        }

        public int IdAfiliado
        {
            get { return _idAfiliado; }
            set { _idAfiliado = value; }
        }

        public byte[] Archivo
        {
            get { return _archivo; }
            set { _archivo = value; }
        }

        public string NombreArchivo
        {
            get { return _nombreArchivo == null ? string.Empty : _nombreArchivo; }
            set { _nombreArchivo = value; }
        }

        public string TipoArchivo
        {
            get { return _tipoArchivo == null ? string.Empty : _tipoArchivo; }
            set { _tipoArchivo = value; }
        }

        public long Tamanio
        {
            get { return _tamanio; }
            set { _tamanio = value; }
        }

        public string TamanioFormateado
        {
            get
            {
                string[] suf = { "B", "KB", "MB", "GB", "TB", "PB" };
                int place = this.Tamanio == 0 ? 0 : Convert.ToInt32(Math.Floor(Math.Log(this.Tamanio, 1024)));
                double num = Math.Round(this.Tamanio / Math.Pow(1024, place), 1);
                return num.ToString() + " " + suf[place];
            }
            set { ;}
        }

        #endregion

    }
}
