using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using System.Linq;
using System.Linq.Expressions;
using Comunes.Entidades;

namespace IU
{
    public class Ordenar<T> : IComparer<T>
    {
        //public List<T> OrdenarLista(List<T> pLista, SortDirection pDir, string pNombrePropiedad)
        //{
        //    _sortDirection = pDir;
        //    _propertyName = pNombrePropiedad;
        //    pLista.Sort(Compare);
        //    return AyudaProgramacion.AcomodarIndices<T>(pLista);
        //}

        #region Propiedades
        private SortDirection _sortDirection = SortDirection.Ascending;
        private string _propertyName = string.Empty;

        #endregion

        #region ICompdarer<T> Members

        public int Compare(T x, T y)
        {
            if (typeof(T).GetProperty(_propertyName) == null)
                throw new Exception(String.Format
                ("Given property is not part of the type {0}", _propertyName));

            object objX = typeof(T).GetProperty(_propertyName).GetValue(x, null);
            object objY = typeof(T).GetProperty(_propertyName).GetValue(y, null);

            int retVal = default(int);
            if (_sortDirection == SortDirection.Ascending)
                retVal = ((IComparable)objX).CompareTo((IComparable)objY);
            else
                retVal = ((IComparable)objY).CompareTo((IComparable)objX);

            return retVal;
        }

        #endregion

        public List<T> OrdenarListaGenerica(List<T> pLista, SortDirection pDir, string pNombrePropiedad)
        {
            if (SortDirection.Ascending == pDir)
            {
                var litaOrdenada = from p in pLista
                                   orderby pNombrePropiedad ascending
                                   select p;
                return litaOrdenada.ToList<T>();
            }
            else
            {
                var litaOrdenada = from p in pLista
                                   orderby pNombrePropiedad descending
                                   select p;
                return litaOrdenada.ToList<T>();
            }
        }
    }
}

