using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailing.Entidades
{
    [Serializable]
    public partial class TGEMailingProcesos : Objeto
    {
        #region "Private Members"
        int _idMailingProceso;
        List<TGEMailingParametros> _parametros;
        #endregion

        #region "Constructors"
        public TGEMailingProcesos()
        {
        }

        #endregion

        #region "Public Properties"
        [PrimaryKey()]
        public int IdMailingProceso
        {
            get { return _idMailingProceso; }
            set { _idMailingProceso = value; }
        }

        public string Descripcion { get; set; }

        public string StoredProcedure { get; set; }

        public bool PruebaEnvio { get; set; }

        public string CodigoProceso { get; set; }

        public List<TGEMailingParametros> Parametros
        {
            get { return _parametros == null ? (_parametros = new List<TGEMailingParametros>()) : _parametros; }
            set { _parametros = value; }
        }

        #endregion
    }
}
