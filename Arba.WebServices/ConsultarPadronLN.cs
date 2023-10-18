using Generales.Entidades;
using Generales.FachadaNegocio;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using RestSharp;
using Servicio.AccesoDatos;
using Servicio.Encriptacion;
using System;
using System.Data.Common;
using System.Xml;


namespace Arba.WebServices
{
    public class ConsultarPadronLN
    {

        public void ConsultarTest()
        {
            var client = new RestClient("dfe.arba.gov.ar/DomicilioElectronico/SeguridadCliente/dfeServicioConsulta.do?user=30714930083&password=sALTO2021");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Cookie", "JSESSIONID=0000aW57ivic0jEuNKBaMDcn0xc:-1");
            request.AddFile("file", "/C:/Users/Agula/Downloads/DFEServicioConsulta_368fff3b5567c31e9b47df4e448a270c.xml");
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);
        }

        public bool ConsultarPadron(ConsultarPadronEntidad pParametro)
        {

            /*
            1) Armar un XML a mano
            2) Generar el hash para el XML
            3) Guardar el archivo en disco /temp
            4) Armar ConsultarTest con los datos nuevos
            5) resultado
            */
            bool resultado = true;

            //validar que exista en Padron ARBA erp_comun



            if (!ValidarPadron(pParametro))
            {
                #region Consultar ARBA WebService
                try
                {
                    XmlDocument doc = new XmlDocument();
                    //XmlDeclaration xmlDeclaration = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
                    //XmlElement root = doc.DocumentElement;
                    //doc.InsertBefore(xmlDeclaration, root);
                    XmlElement Consulta = doc.CreateElement(string.Empty, "CONSULTA-ALICUOTA", string.Empty);
                    doc.AppendChild(Consulta);

                    XmlElement FechaDesde = doc.CreateElement(string.Empty, "fechaDesde", string.Empty);

                    XmlText text3 = doc.CreateTextNode(pParametro.FechaVigenciaDesde.ToString("yyyyMMdd"));
                    FechaDesde.AppendChild(text3);
                    Consulta.AppendChild(FechaDesde);

                    XmlElement FechaHasta = doc.CreateElement(string.Empty, "fechaHasta", string.Empty);

                    XmlText text4 = doc.CreateTextNode(pParametro.FechaVigenciaHasta.ToString("yyyMMdd"));
                    FechaHasta.AppendChild(text4);
                    Consulta.AppendChild(FechaHasta);


                    XmlElement cantidadContribuyentes = doc.CreateElement(string.Empty, "cantidadContribuyentes", string.Empty);
                    XmlText text5 = doc.CreateTextNode("1");
                    cantidadContribuyentes.AppendChild(text5);
                    Consulta.AppendChild(cantidadContribuyentes);

                    XmlElement contribuyentes = doc.CreateElement(string.Empty, "contribuyentes", string.Empty);
                    Consulta.AppendChild(contribuyentes);
                    XmlAttribute att = doc.CreateAttribute("class");
                    att.Value = "list";
                    contribuyentes.Attributes.Append(att);

                    XmlElement contribuyente = doc.CreateElement(string.Empty, "contribuyente", string.Empty);
                    //XmlText text1 = doc.CreateTextNode("texto");
                    //element3.AppendChild(text1);
                    contribuyentes.AppendChild(contribuyente);
                    XmlElement cuitContribuyentes = doc.CreateElement(string.Empty, "cuitContribuyente", string.Empty);
                    XmlText text2 = doc.CreateTextNode(pParametro.NumeroCUIT.ToString());
                    cuitContribuyentes.AppendChild(text2);
                    contribuyente.AppendChild(cuitContribuyentes);
                    //string hash = Encriptar.EncriptarHashMD5(doc.OuterXml);

                    string nombreArchivo = string.Concat("DFEServicioConsulta_", "EvolTemp", ".xml");
                    string fileSystemPath = string.Concat(AppDomain.CurrentDomain.BaseDirectory, "tempPDF\\", nombreArchivo);
                    doc.Save(fileSystemPath);
                    string text = System.IO.File.ReadAllText(fileSystemPath);
                    string hash = Encriptar.EncriptarHashMD5(text);
                    string hashFileSystemPath = fileSystemPath.Replace("EvolTemp", hash);
                    if (System.IO.File.Exists(hashFileSystemPath))
                        System.IO.File.Delete(hashFileSystemPath);
                    System.IO.File.Move(fileSystemPath, hashFileSystemPath);
                    //string nombreArchivo = string.Concat("DFEServicioConsulta_c0868dc044e2ece359396a5bb854491a", ".xml");
                    TGEParametrosValores valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSArbaUrl);
                    //bool Pvalor = !string.IsNullOrEmpty(valor.ParametroValor) ? Convert.ToBoolean(valor.ParametroValor) : false;

                    var client = new RestClient(valor.ParametroValor);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSArbaUsuario);
                    request.AddParameter("user", valor.ParametroValor);
                    valor = TGEGeneralesF.ParametrosObtenerValorActual(EnumTGEParametros.WSArbaContrasenia);
                    request.AddParameter("password", valor.ParametroValor);
                    request.AddFile("file", hashFileSystemPath);
                    IRestResponse response = client.Execute(request);
                    if (response.Content.Length == 0)
                    {
                        pParametro.CodigoMensaje = "ErrorConexionARBA";
                        pParametro.Mensaje = response.ErrorMessage;
                        return false;
                    }
                    pParametro.Respuesta = new XmlDocument();
                    pParametro.Respuesta.LoadXml(response.Content);
                    if (pParametro.Respuesta.FirstChild.NodeType == XmlNodeType.XmlDeclaration)
                        pParametro.Respuesta.RemoveChild(pParametro.Respuesta.FirstChild);
                }
                catch (Exception ex)
                {
                    pParametro.CodigoMensaje = ex.Message;
                    return false;
                }
                #endregion

                //guardar los datos del webservice en la tabla del padron
                DbTransaction tran;
                DatabaseProviderFactory factory = new DatabaseProviderFactory(); Database bd = factory.Create(BaseDatos.conexionErpComun);
                
                using (DbConnection con = bd.CreateConnection())
                {
                    con.Open();
                    tran = con.BeginTransaction();

                    try
                    {
                        pParametro.IdPadronPorSujeto = BaseDatos.ObtenerBaseDatos().Agregar(pParametro, bd, tran, "ARBAPadronPorSujetoInsertar");
                        if (pParametro.IdPadronPorSujeto == 0) {
                            resultado = false;
                                }

                        if (resultado)
                        {
                            tran.Commit();
                            pParametro.CodigoMensaje = "ResultadoTransaccion";
                        }
                        else
                        {
                            tran.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        ExceptionHandler.HandleException(ex, "LogicaNegocio");
                        tran.Rollback();
                        pParametro.CodigoMensaje = "ResultadoTransaccionIncorrecto";
                        pParametro.CodigoMensajeArgs.Add(ex.Message);
                        return resultado;
                    }
                }

            }
            
            pParametro = BaseDatos.ObtenerBaseDatos().Obtener<ConsultarPadronEntidad>("ARBAPadronPorSujetoSeleccionar", pParametro, BaseDatos.conexionErpComun);

            return resultado;
        }

        private bool ValidarPadron(ConsultarPadronEntidad pParametro)
        {
            return BaseDatos.ObtenerBaseDatos().EjecutarSPValidacion(pParametro, "ARBAPadronPorSujetoValidaciones", BaseDatos.conexionErpComun);
        }
    }
}
