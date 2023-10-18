using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Comunes.Entidades;

namespace Tesorerias.Entidades
{
    [Serializable]
    public partial class TESFiltroMovimientosPendientes : Objeto
    {
        #region Private Members
        
        int _idFilial;
        int _idTipoOperacion;
        int _idRefTipoOperacion;
        #endregion


        #region Constructors
        public TESFiltroMovimientosPendientes()
        {
        }
        
        #endregion


        #region Public PRoperties
        //public int IdUsuarioEvento
        //{
        //    get { return _idUsuarioEvento; }
        //    set { _idUsuarioEvento = value; }
        //}

        public int IdFilial
        {
            get { return _idFilial; }
            set { _idFilial = value; }
        }

        public int IdTipoOperacion
        {
            get { return _idTipoOperacion; }
            set { _idTipoOperacion = value; }
        }

        public int IdRefTipoOperacion
        {
            get { return _idRefTipoOperacion; }
            set { _idRefTipoOperacion = value; }
        }
        #endregion
    }
}
