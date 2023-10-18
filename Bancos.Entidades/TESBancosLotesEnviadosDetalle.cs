using Comunes.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bancos
{
    [Serializable]
    public class TESBancosLotesEnviadosDetalle:Objeto
    {
        [PrimaryKey]
        public int IdBancoLoteEnviadoDetalle { get; set; }
        public int IdBancoLoteEnviado{ get; set; }
        public DateTime FechaComprobante { get; set; }
        public string Tabla { get; set; }
        public int IdTipoOperacion{ get; set; }
        public string TipoOperacion { get; set; }
        public string Detalle { get; set; }
        public int IdRefTipoOperacion{ get; set; }
        public int Orden{ get; set; }
        public int IdEntidad{ get; set; }
        public int IdRefEntidad { get; set; }
        public decimal ImporteTotal{ get; set; }
        public string RegistroEnviado { get; set; }
        /// <summary>
        /// 0 = PROVEEDOR
        /// </summary>
        public int IdAfiliado { get; set; }
        bool _incluirEnLote;
        public bool IncluirEnLote
        {
            get { return _incluirEnLote; }
            set { _incluirEnLote = value; }
        }


        public string CBUDetalle { get; set; }
        public string CBU { get; set; }
        public string Cuit { get; set; }

    }
}
