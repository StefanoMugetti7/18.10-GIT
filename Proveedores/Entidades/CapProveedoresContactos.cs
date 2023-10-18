using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Generales.Entidades;
using Comunes.Entidades;

namespace Proveedores.Entidades
{
    class CapProveedoresContactos
    {
        [PrimaryKey]
        public int IdProveedorContacto { get; set; }
        public int IdProveedor { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }

        private TGEOcupaciones _ocupacion;

        [Auditoria]
        public TGEOcupaciones Ocupacion
        {
            get { return _ocupacion == null ? (_ocupacion = new TGEOcupaciones()) : _ocupacion; }
            set { _ocupacion = value; }
        }

        public string TelefonoLaboral { get; set; }
        public string TelefonoMovil { get; set; }
        public string TelefonoParticular { get; set; }

    }
}
