using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Haberes.Entidades
{
    [Serializable]
    public class HabSueldoHaberes : Objeto
    {
        #region "Private Members"
        HabCategoriasHaberes _categoriaHaber;
        #endregion

        #region "Constructors"
        public HabSueldoHaberes() { }
       
        #endregion

        #region "Public Properties"
        [PrimaryKey]
        public int IdSueldoSaltoGrande { get; set; }

        public int IdFondoSuplementario { get; set; }
        public int IdAfiliado { get; set; }
        public int CantidadMeses { get; set; }
        public decimal Coeficiente { get; set; }

        //public HabCategoriasHaberes CategoriaHaber = new HabCategoriasHaberes();

        public HabCategoriasHaberes CategoriaHaber
        {
            get { return _categoriaHaber == null ? (_categoriaHaber = new HabCategoriasHaberes()) : _categoriaHaber; }
            set { _categoriaHaber = value; }
        }
        #endregion

    }
}
