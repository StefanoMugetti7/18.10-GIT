using Comunes.Entidades;
using Comunes.LogicaNegocio;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace Hoteles.Entidades
{
    [Serializable]
    public class HTLReservas : Objeto
    {
        List<HTLReservasDetalles> _reservasDetalles;
        List<HTLReservasOcupantes> _reservasocupantes;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        List<TGECampos> _campos;
        XmlDocumentSerializationWrapper _loteCamposValores;
        XmlDocumentSerializationWrapper _loteCamposValoresOcupantes;

        [PrimaryKey]
        public Int64 IdReserva { get; set; }
        public DateTime FechaAlta { get; set; }
        [Auditoria]
        public DateTime? FechaIngreso { get; set; }

        public DateTime? FechaIngresoHasta { get; set; }
        [Auditoria]
        public DateTime? FechaEgreso { get; set; }

        public DateTime? FechaEgresoHasta { get; set; }

        public int IdHotel { get; set; }
        [Auditoria]
        public int IdListaPrecio { get; set; }
        public string ListaPrecio { get; set; }    
        [Auditoria]
        public int? Permanencia { get; set; }
        [Auditoria]
        public Int64? IdAfiliado { get; set; }
        [Auditoria]
        public string Apellido { get; set; }
        [Auditoria]
        public string Nombre { get; set; }
        [Auditoria]
        public int? IdTipoDocumento { get; set; }
        [Auditoria]
        public string NumeroDocumento { get; set; }
        [Auditoria]
        public string CorreoElectronico { get; set; }
        [Auditoria]
        public int? IdAfiliadoTipo { get; set; }

        [Auditoria]
        public int IdCondicionFiscal { get; set; }

        [Auditoria]
        public decimal PrecioTotal { get; set; }

        [Auditoria]
        public int CantidadPersonas { get; set; }
        public List<HTLReservasDetalles> ReservasDetalles {
            get { return _reservasDetalles == null ? (_reservasDetalles = new List<HTLReservasDetalles>()) : _reservasDetalles; }
            set { _reservasDetalles = value; }
        }

        public List<HTLReservasOcupantes> ReservasOcupantes
        {
            get { return _reservasocupantes == null ? (_reservasocupantes = new List<HTLReservasOcupantes>()) : _reservasocupantes; }
            set { _reservasocupantes = value; }
        }

        public List<TGEArchivos> Archivos
        {
            get { return _archivos == null ? (_archivos = new List<TGEArchivos>()) : _archivos; }
            set { _archivos = value; }
        }

        public List<TGEComentarios> Comentarios
        {
            get { return _comentarios == null ? (_comentarios = new List<TGEComentarios>()) : _comentarios; }
            set { _comentarios = value; }
        }

        public List<TGECampos> Campos
        {
            get { return _campos == null ? (_campos = new List<TGECampos>()) : _campos; }
            set { _campos = value; }
        }

        public XmlDocument LoteCamposValoresOcupantes
        {
            get { return _loteCamposValoresOcupantes; }
            set { _loteCamposValoresOcupantes = value; }
        }

        public void CargarLoteCamposValoresOcupantes()
        {
            this.LoteCamposValoresOcupantes = new XmlDocument();
            XmlNode nodos = this.LoteCamposValoresOcupantes.CreateElement("ReservasOcupantes");
            this.LoteCamposValoresOcupantes.AppendChild(nodos);

            XmlNode itemNodo;
            XmlAttribute itemAttribute;
            foreach (HTLReservasOcupantes item in this.ReservasOcupantes)
            {
                if (item.IdHabitacion.HasValue)
                {
                    itemNodo = this.LoteCamposValoresOcupantes.CreateElement("ReservaOcupante");
                    itemAttribute = this.LoteCamposValoresOcupantes.CreateAttribute("IdHabitacion");
                    itemAttribute.Value = item.IdHabitacion.ToString();
                    itemNodo.Attributes.Append(itemAttribute);

                    itemAttribute = this.LoteCamposValoresOcupantes.CreateAttribute("IdAfiliado");
                    itemAttribute.Value = item.IdAfiliado.ToString();
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = this.LoteCamposValoresOcupantes.CreateAttribute("IdEstado");
                    itemAttribute.Value = item.Estado.IdEstado.ToString();
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);


                }
            }
        }


        public XmlDocument LoteCamposValores
        {
            get { return _loteCamposValores; }
            set { _loteCamposValores = value; }
        }
        public void CargarLoteCamposValores()
        {
            this.LoteCamposValores = new XmlDocument();
            XmlNode nodos = this.LoteCamposValores.CreateElement("ReservasDetalles");
            this.LoteCamposValores.AppendChild(nodos);

            XmlNode itemNodo;
            XmlAttribute itemAttribute;
            foreach (HTLReservasDetalles item in this.ReservasDetalles)
            {
                if (item.IdHabitacion.HasValue)
                {
                    itemNodo = this.LoteCamposValores.CreateElement("ReservaDetalle");
                    itemAttribute = this.LoteCamposValores.CreateAttribute("IdHabitacion");
                    itemAttribute.Value = item.IdHabitacion.ToString();
                    itemNodo.Attributes.Append(itemAttribute);

                    itemAttribute = this.LoteCamposValores.CreateAttribute("FechaIngreso");
                    itemAttribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(item.FechaIngreso.Value);
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = this.LoteCamposValores.CreateAttribute("FechaEgreso");
                    itemAttribute.Value = AyudaProgramacionLN.ObtenerFechaAAAAMMDD(item.FechaEgreso.Value);
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = this.LoteCamposValores.CreateAttribute("Compartida");
                    itemAttribute.Value = item.Compartida.ToString();
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = this.LoteCamposValores.CreateAttribute("IdHabitacionDetalle");
                    itemAttribute.Value = item.HabitacionDetalle.IdHabitacionDetalle.ToString();
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = this.LoteCamposValores.CreateAttribute("CantidadPersonas");
                    itemAttribute.Value = item.CantidadPersonas.ToString();
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = this.LoteCamposValores.CreateAttribute("CantidadPersonasOpciones");
                    itemAttribute.Value = item.CantidadPersonasOpciones.HasValue ? item.CantidadPersonasOpciones.Value.ToString() : string.Empty;
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = this.LoteCamposValores.CreateAttribute("LateCheckOut");
                    itemAttribute.Value = item.LateCheckOut.ToString();
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);

                    itemAttribute = this.LoteCamposValores.CreateAttribute("Detalle");
                    itemAttribute.Value = item.Detalle;
                    itemNodo.Attributes.Append(itemAttribute);
                    nodos.AppendChild(itemNodo);
                }
            }
        }

        public string EstadoSocio { get; set; }

        public string CategoriaSocio { get; set; }
    }
}
