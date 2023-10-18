using Comunes.Entidades;
using System;

namespace Afiliados.Entidades.Entidades
{
    [Serializable]
    public class AfiAfiliadosMatrizRiesgo : Objeto
    {
        int _idAfiliadoMatriz;
        int _idAfiliado;
        int _idMatriz;
        string _matriz;
        string _codigoMatriz;
        int? _valor;

        [PrimaryKey]
        public int IdAfiliadoMatriz { get => _idAfiliadoMatriz; set => _idAfiliadoMatriz = value; }
        public int IdAfiliado { get => _idAfiliado; set => _idAfiliado = value; }
        public int IdMatriz { get => _idMatriz; set => _idMatriz = value; }
        public string Matriz { get => _matriz; set => _matriz = value; }
        public string CodigoMatriz { get => _codigoMatriz; set => _codigoMatriz = value; }
        public int? Valor { get => _valor; set => _valor = value; }


    }
}
