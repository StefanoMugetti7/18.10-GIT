namespace Afiliados.Entidades.Entidades
{
    public partial class CuentasCorrientesFiltro : AfiAfiliados
    {
        #region "Private Members"	
        int _idMoneda;
        #endregion
        #region "Constructors"
        #endregion
        #region "Public Properties"

        public int IdMoneda
        {
            get { return _idMoneda; }
            set { _idMoneda = value; }
        }

        #endregion
    }
}
