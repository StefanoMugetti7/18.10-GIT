using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Comunes.Entidades;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using System.IO;
using System.Text;
using System.Data;
using Servicio.AccesoDatos;
using System.Web.Services;
using Prestamos.Entidades;
using Prestamos;
using Bancos;
using Bancos.Entidades;
using Reportes.Entidades;
using System.Web.Script.Services;

namespace IU
{
    public partial class Dashboard : PaginaSegura
    {
        private List<REPGraficos> MisGraficos
        {
            get { return (List<REPGraficos>)Session["InicioSistemaMisGraficos"]; }
            set { Session["InicioSistemaMisGraficos"] = value; }
        }

        private List<REPGraficos> MisCards
        {
            get { return (List<REPGraficos>)Session["InicioSistemaMisCards"]; }
            set { Session["InicioSistemaMisCards"] = value; }
        }

        protected override void PageLoadEvent(object sender, EventArgs e)
        {
            base.PageLoadEvent(sender, e);
            if (!this.IsPostBack)
            {
                MisGraficos = new List<REPGraficos>();
                PaginaSegura pagina = new PaginaSegura();
                Objeto obj = new Objeto();
                obj.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(pagina.UsuarioActivo);
                List<REPGraficos> lista = BaseDatos.ObtenerBaseDatos().ObtenerLista<REPGraficos>("RepGraficosSeleccionar", obj);
                MisCards = new List<REPGraficos>();
                MisGraficos = new List<REPGraficos>();
                MisCards.AddRange(lista.Where(x => x.TipoGrafico == "Cards").ToList());
                MisGraficos.AddRange(lista.Where(x => x.TipoGrafico != "Cards").ToList());

            }
        }

        [WebMethod(true)]
        public static List<REPGraficos> ObteneCards()
        {
            return (List<REPGraficos>)HttpContext.Current.Session["InicioSistemaMisCards"];
        }

        [WebMethod(true)]
        public static List<REPGraficos> ObteneGraficos()
        {
            return (List<REPGraficos>)HttpContext.Current.Session["InicioSistemaMisGraficos"];
        }

        [WebMethod(true)]
        public static REPGraficos ObtenerGraficoDatos(REPGraficos grafico)
        {
            PaginaSegura pagina = new PaginaSegura();
            Objeto obj = new Objeto();
            obj.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(pagina.UsuarioActivo);
            DataTable datos = BaseDatos.ObtenerBaseDatos().ObtenerLista(grafico.StoreProcedure, obj);

            grafico.Columns = new List<List<string>>();
            grafico.Rows = new List<List<object>>();

            int cantidadColumnas = datos.Columns.Count;
            List<string> columns;

            foreach (DataColumn column in datos.Columns)
            {
                columns = new List<string>();

                columns.Add(column.DataType.ToString());
                columns.Add(column.ColumnName);

                grafico.Columns.Add(columns);
            }
            List<object> rows;
            foreach (DataRow row in datos.Rows)
            {
                rows = new List<object>();

                for (int i = 0; i < cantidadColumnas; i++)
                {
                    rows.Add(row[i]);
                }

                grafico.Rows.Add(rows);
            }
            return grafico;
        }

        [WebMethod(true)]
        public static List<REPGraficos> ObtenerDatos()
        {
            PaginaSegura pagina = new PaginaSegura();
            Objeto obj = new Objeto();
            obj.UsuarioLogueado = AyudaProgramacion.ObtenerDatosUsuario(pagina.UsuarioActivo);
            List<REPGraficos> graficos = BaseDatos.ObtenerBaseDatos().ObtenerLista<REPGraficos>("RepGraficosSeleccionar", obj);
            List<DataTable> graficosLista = new List<DataTable>();

            int contador = 0;

            foreach (REPGraficos grafico in graficos)
            {
                DataTable datos = BaseDatos.ObtenerBaseDatos().ObtenerLista(grafico.StoreProcedure, obj);

                grafico.Columns = new List<List<string>>();
                grafico.Rows = new List<List<object>>();
                
                int cantidadColumnas = datos.Columns.Count;

                foreach (DataColumn column in datos.Columns)
                {
                    List<string> columns = new List<string>();
    
                    columns.Add(column.DataType.ToString());
                    columns.Add(column.ColumnName);

                    grafico.Columns.Add(columns);
                }

                foreach (DataRow row in datos.Rows)
                {
                    List<object> rows = new List<object>();

                    for(int i = 0; i<cantidadColumnas; i++)
                    {
                        rows.Add(row[i]);
                    }

                    grafico.Rows.Add(rows);
                }

                contador++;
            }    

            return graficos;
        }

    }
}


