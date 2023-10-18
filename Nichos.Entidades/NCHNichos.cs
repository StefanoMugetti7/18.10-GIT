using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nichos.Entidades
{
    [Serializable]
    public class NCHNichos : Objeto
    {
        [PrimaryKey]
        public int IdNicho { get; set; }
        public string Codigo { get; set; }


        NCHPanteones _panteon;
        NCHTiposNichos _tipoNicho;
        NCHNichoCapacidad _nichoCapacidad;
        NCHNichosUbicacion _nichoUbicacion;
        NCHNichosSubUbicacion _nichoSubUbicacion;

        public NCHPanteones Panteon { get { return _panteon == null ? (_panteon = new NCHPanteones()) : _panteon; } set { _panteon = value; } }
        public NCHTiposNichos TipoNicho { get { return _tipoNicho == null ? (_tipoNicho = new NCHTiposNichos()) : _tipoNicho; } set { _tipoNicho = value; } }
        public NCHNichoCapacidad NichoCapacidad { get { return _nichoCapacidad == null ? (_nichoCapacidad = new NCHNichoCapacidad()) : _nichoCapacidad; } set { _nichoCapacidad = value; } }
        public NCHNichosUbicacion NichoUbicacion { get { return _nichoUbicacion == null ? (_nichoUbicacion = new NCHNichosUbicacion()) : _nichoUbicacion; } set { _nichoUbicacion = value; } }
        public NCHNichosSubUbicacion NichoSubUbicacion { get { return _nichoSubUbicacion == null ? (_nichoSubUbicacion = new NCHNichosSubUbicacion()) : _nichoSubUbicacion; } set { _nichoSubUbicacion = value; } }
    }

}
