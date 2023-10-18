using Afiliados.Entidades;
using Comunes.Entidades;
using Cargos.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace Haberes.Entidades
{
    [Serializable]
    public class HabFondoSuplementario : Objeto
    {
        #region "Private Members"
        XmlDocumentSerializationWrapper _loteAportesJubilatorios;
        List<CarCuentasCorrientes> _cargosPendientesCancelar;
        CarTiposCargos _tipoCargo;
        AfiAfiliados _afiliado;
        #endregion

        #region "Constructors"
        public HabFondoSuplementario()
        {
            SueldosHaberes = new List<HabSueldoHaberes>();
            Afiliado = new AfiAfiliados();
            Archivos = new List<TGEArchivos>();
        }
        #endregion

        #region "Public Properties"
        [PrimaryKey]
        public int IdFondoSuplementario { get; set; }

        public int PeriodoInicio { get; set; }

        public DateTime ? FechaJubilacion { get; set; }

        public bool TieneRecibo { get; set; }
        public decimal AporteInicial { get; set; }

        public decimal AporteTotal { get; set; }
        public decimal AporteInicialPorcentual { get; set; }

        public decimal AporteTotalPorcentual { get; set; }
        public decimal SueldoPromedio { get; set; }
        public decimal SueldoSuplementario { get; set; }
        public decimal SueldoBruto{ get; set; }
        

        public List<HabSueldoHaberes> SueldosHaberes;

        public AfiAfiliados Afiliado
        {
            get { return _afiliado == null ? (_afiliado = new AfiAfiliados()) : _afiliado; }
            set { _afiliado = value; }
        }


        public List<TGEArchivos> Archivos;

        public XmlDocument LoteAportesJubilatorios
        {
            get { return _loteAportesJubilatorios; }
            set { _loteAportesJubilatorios = value; }
        }

        public List<CarCuentasCorrientes> CargosPendientesCancelar
        {
            get { return _cargosPendientesCancelar == null ? (_cargosPendientesCancelar = new List<CarCuentasCorrientes>()) : _cargosPendientesCancelar; }
            set { _cargosPendientesCancelar = value; }
        }

        [Auditoria()]
        public CarTiposCargos TipoCargo
        {
            get { return _tipoCargo == null ? (_tipoCargo = new CarTiposCargos()) : _tipoCargo; }
            set { _tipoCargo = value; }
        }

        public void CargarLoteAportesJubilatoriosDetalles()
        {
            this.LoteAportesJubilatorios = new XmlDocument();

            XmlNode items = this.LoteAportesJubilatorios.CreateElement("HabSueldoHaberes");
            this.LoteAportesJubilatorios.AppendChild(items);

            XmlNode item;
            XmlAttribute attribute;
            foreach (HabSueldoHaberes dato in this.SueldosHaberes)
            {
                if(dato.Coeficiente > 0)
                {
                    item = this.LoteAportesJubilatorios.CreateElement("HabSueldoHaber");

                    attribute = this.LoteAportesJubilatorios.CreateAttribute("IdCategoriaHaber");
                    attribute.Value = dato.CategoriaHaber.IdCategoriaHaber.ToString();
                    item.Attributes.Append(attribute);
                    items.AppendChild(item); 

                    attribute = this.LoteAportesJubilatorios.CreateAttribute("Cantidad");
                    attribute.Value = dato.CantidadMeses.ToString();
                    item.Attributes.Append(attribute);
                    items.AppendChild(item);

                    attribute = this.LoteAportesJubilatorios.CreateAttribute("Coeficiente");
                    attribute.Value = dato.Coeficiente.ToString().Replace(',','.');
                    item.Attributes.Append(attribute);
                    items.AppendChild(item);
                }
            }
        }
        #endregion
    }
}
