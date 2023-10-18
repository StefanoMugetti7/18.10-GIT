using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Acopios.Entidades
{
    [Serializable]
    public class AcpAcopios : Objeto
    {
        [PrimaryKey]
        public int IdAcopio { get; set; }
        public int IdModuloSistema { get; set; }

        public Int64 IdRefTabla { get; set; }

        public string Tabla { get; set; }
        public string RazonSocial { get; set; }
        [Auditoria]
        public decimal ImporteTotal { get; set; }
        [Auditoria]
        public string Descripcion { get; set; }

        public int? IdListaPrecio { get; set; }
        public string ListaPrecio { get; set; }

        List<AcpAcopiosImportes> _acopiosImportes;
        public List<AcpAcopiosImportes> AcopiosImportes
        {
            get { return _acopiosImportes == null ? (_acopiosImportes = new List<AcpAcopiosImportes>()) : _acopiosImportes; }
            set { _acopiosImportes = value; }
        }
    }
}
