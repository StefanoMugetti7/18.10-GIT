using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Producciones.Entidades
{
    [Serializable]
    public class PrdProduccionesCentrosCostosProrrateos : Objeto
    {
        UsuariosAlta _usuarioAlta;

        [PrimaryKey]
        public Int64 IdProduccionCentroCostoProrrateo { get; set; }
        public Int64 IdProduccion { get; set; }
        public int IdCentroCostoProrrateo { get; set; }
        public DateTime FechaAlta { get; set; }
        public UsuariosAlta UsuarioAlta
        {
            get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
            set { _usuarioAlta = value; }
        }
    }
}
