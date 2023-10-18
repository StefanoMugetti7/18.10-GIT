using Comunes.Entidades;
using ProcesosDatos.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mailing.Entidades
{
    [Serializable]
    public partial class TGEMailingParametros : SisParametros
    {
        #region "Private Members"
       // int _idParametro;
        int _idMailingProceso;
        //string _parametro;
        //int _orden;
        //string _cnn;
        //string _storedProcedure;
        //string _nombreParametro;
        //SisTiposParametros _tipoParametro;
        //string _paramDependiente;
        //object _valorParametro;
        //bool _parametroDibujado;
        //string _valorParametroDescripcion;
        //int _idProcesoProcesamientoParametroValor;
        //int _idProcesoProcesamiento;
        #endregion

        #region "Constructors"
        public TGEMailingParametros()
        {
        }
        #endregion

        #region "Public Properties"

        //[PrimaryKey()]
        //public int IdParametro
        //{
        //    get { return _idParametro; }
        //    set { _idParametro = value; }
        //}
        public int IdMailingProceso
        {
            get { return _idMailingProceso; }
            set { _idMailingProceso = value; }
        }
        //[Auditoria()]
        //public string Parametro
        //{
        //    get { return _parametro == null ? string.Empty : _parametro; }
        //    set { _parametro = value; }
        //}

        //[Auditoria()]
        //public int Orden
        //{
        //    get { return _orden; }
        //    set { _orden = value; }
        //}

        //public string Cnn
        //{
        //    get { return _cnn == null ? string.Empty : _cnn; }
        //    set { _cnn = value; }
        //}
        //[Auditoria()]
        //public string StoredProcedure
        //{
        //    get { return _storedProcedure == null ? string.Empty : _storedProcedure; }
        //    set { _storedProcedure = value; }
        //}
        //[Auditoria()]
        //public string NombreParametro
        //{
        //    get { return _nombreParametro == null ? string.Empty : _nombreParametro; }
        //    set { _nombreParametro = value; }
        //}
        //[Auditoria()]
        //public SisTiposParametros TipoParametro
        //{
        //    get { return _tipoParametro == null ? (_tipoParametro = new SisTiposParametros()) : _tipoParametro; }
        //    set { _tipoParametro = value; }
        //}
        //[Auditoria()]
        //public string ParamDependiente
        //{
        //    get { return _paramDependiente == null ? string.Empty : _paramDependiente; }
        //    set { _paramDependiente = value; }
        //}

        //public bool ParametroDibujado
        //{
        //    get { return _parametroDibujado; }
        //    set { _parametroDibujado = value; }
        //}

        //public object ValorParametro
        //{
        //    get { return _valorParametro == null ? string.Empty : _valorParametro; }
        //    set { _valorParametro = value; }
        //}

        //public string ValorParametroDescripcion
        //{
        //    get { return _valorParametroDescripcion == null ? string.Empty : _valorParametroDescripcion; }
        //    set { _valorParametroDescripcion = value; }
        //}

        //public int IdProcesoProcesamientoParametroValor
        //{
        //    get { return _idProcesoProcesamientoParametroValor; }
        //    set { _idProcesoProcesamientoParametroValor = value; }
        //}

        //public int IdMailingProcesoProcesamiento
        //{
        //    get { return _idProcesoProcesamiento; }
        //    set { _idProcesoProcesamiento = value; }
        //}

        #endregion
    }
}
