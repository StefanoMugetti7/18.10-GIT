using Compras.Entidades;
using Comunes.Entidades;
using Generales.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Producciones.Entidades
{
    [Serializable]
    public class PrdProducciones : Objeto
    {
        //UsuariosAlta _usuarioAlta;
        TGEFiliales _filial;
        CMPProductos _producto;
        List<PrdProduccionesDetalles> _produccionesDetalles;
        List<PrdProduccionesCentrosCostosProrrateos > _produccionesCentrosCostosProrrateo;
        List<TGEArchivos> _archivos;
        List<TGEComentarios> _comentarios;
        List<TGECampos> _campos;

        [PrimaryKey]
        public Int64 IdProduccion { get; set; }
        [Auditoria]
        public string Descripcion { get; set; }
        [Auditoria]
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaInicioHasta { get; set; }
        [Auditoria]
        public DateTime? FechaFin { get; set; }
        [Auditoria]
        public decimal CantidadProducida { get; set; }
        public DateTime FechaAlta { get; set; }

        public int IdUsuarioAlta { get; set; }

        public TGEFiliales Filial
        {
            get { return _filial == null ? (_filial = new TGEFiliales()) : _filial; }
            set { _filial = value; }
        }
        public CMPProductos Producto
        {
            get { return _producto == null ? (_producto = new CMPProductos()) : _producto; }
            set { _producto = value; }
        }
        //public UsuariosAlta UsuarioAlta
        //{
        //    get { return _usuarioAlta == null ? (_usuarioAlta = new UsuariosAlta()) : _usuarioAlta; }
        //    set { _usuarioAlta = value; }
        //}
        public List<PrdProduccionesDetalles> ProduccionesDetalles
        {
            get { return _produccionesDetalles == null ? (_produccionesDetalles = new List<PrdProduccionesDetalles>()) : _produccionesDetalles; }
            set { _produccionesDetalles = value; }
        }
        public List<PrdProduccionesCentrosCostosProrrateos> ProduccionesCentrosCostosProrrateo
        {
            get { return _produccionesCentrosCostosProrrateo == null ? (_produccionesCentrosCostosProrrateo = new List<PrdProduccionesCentrosCostosProrrateos>()) : _produccionesCentrosCostosProrrateo; }
            set { _produccionesCentrosCostosProrrateo = value; }
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
    }
}
