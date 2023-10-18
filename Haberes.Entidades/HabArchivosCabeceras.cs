using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;
using Generales.Entidades;

namespace Haberes.Entidades
{
    [Serializable]
    public partial class HabArchivosCabeceras : Objeto
    {

        #region "Private Members"
        int _idArchivoCabecera;
        int _anio;
        int _mes;
        List<HabArchivosDetalles> _archivosDetalles;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        string _appPath;
        HabRemesasTipos _remesaTipo;
        #endregion

        #region "Constructors"
        public HabArchivosCabeceras()
        {
        }
        #endregion

        #region "Public Properties"

        [PrimaryKey()]
        public int IdArchivoCabecera
        {
            get { return _idArchivoCabecera; }
            set { _idArchivoCabecera = value; }
        }

        public int Anio
        {
            get { return _anio; }
            set { _anio = value; }
        }

        public int Mes
        {
            get { return _mes; }
            set { _mes = value; }
        }

        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        public List<TGEComentarios> Comentarios
        {
            get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
            set { _comentarios = value; }
        }

        public List<HabArchivosDetalles> ArchivosDetalles
        {
            get { return _archivosDetalles == null ? (_archivosDetalles = new List<HabArchivosDetalles>()) : _archivosDetalles; }
            set { _archivosDetalles = value; }
        }

        public string AppPath
        {
            get { return _appPath==null? string.Empty : _appPath; }
            set { _appPath = value; }
        }

        public HabRemesasTipos RemesaTipo
        {
            get { return _remesaTipo == null ? (_remesaTipo = new HabRemesasTipos()) : _remesaTipo; }
            set { _remesaTipo = value; }
        }
        #endregion

    }
}
