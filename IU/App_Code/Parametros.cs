using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Web.UI;

namespace IU
{
    [Serializable]
    public class Parametros
    {
        string _idReferencia;
        object _valor;

        public string IdReferencia
        {
            get { return _idReferencia == null ? string.Empty : _idReferencia; }
            set { _idReferencia = value; }
        }

        public object Valor
        {
            get { return _valor; }
            set { _valor = value; }
        }
    }

    public class ListaParametros : PaginaSegura
    {
        string _sessionPagina = string.Empty;
        public ListaParametros(string SessionPagina)
        {
            _sessionPagina = SessionPagina;
        }
        /// <summary>
        /// Agrega un Valor en Session
        /// </summary>
        /// <param name="p"></param>
        public void Agregar(Parametros p)
        {
            if (MisParametros.Exists(x => x.IdReferencia == p.IdReferencia))
                MisParametros[MisParametros.FindIndex(x => x.IdReferencia == p.IdReferencia)].Valor = p.Valor;
            else
                MisParametros.Add(p);
        }

        /// <summary>
        /// Agrega un Valor en Session
        /// </summary>
        /// <param name="p"></param>
        public void Agregar(string id, object valor)
        {
            Parametros p = new Parametros();
            p.IdReferencia = id;
            p.Valor = valor;
            Agregar(p);
        }

        /// <summary>
        /// Obtiene el Valor (INT64)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Int32 ObtenerValor(Parametros p)
        {
            Int32 id = 0;
            Parametros valor = MisParametros.Find(x => x.IdReferencia == p.IdReferencia);
            if (valor == null)
                return id;
            Int32.TryParse(valor.Valor.ToString(), out id);

            return id;
        }

        /// <summary>
        /// Obtiene el Valor (INT64)
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public Int32 ObtenerValor(string id)
        {
            Parametros p = new Parametros();
            p.IdReferencia = id;
            return ObtenerValor(p);
        }

        public object Obtener(Parametros p)
        {
            return MisParametros.Find(x => x.IdReferencia == p.IdReferencia).Valor;
        }

        public void Limpiar(string id)
        {
            if(MisParametros.Exists(x=>x.IdReferencia==id))
                MisParametros.RemoveAt(MisParametros.FindIndex(x => x.IdReferencia == id));
        }

        public bool Existe(string id)
        { 
            return MisParametros.Exists(x=>x.IdReferencia==id);
        }

        private List<Parametros> MisParametros
        {
            get
            {
                if (Session[this._sessionPagina + "ListaParametrosMisParametros"] == null)
                    Session[this._sessionPagina + "ListaParametrosMisParametros"] = new List<Parametros>();

                return (List<Parametros>)Session[this._sessionPagina + "ListaParametrosMisParametros"];
            }
            set { Session[this._sessionPagina + "ListaParametrosMisParametros"] = value; }
        }
    }

}