using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Comunes.Entidades;
using GoogleApi;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Maps.Geocoding.Address.Request;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.AutoComplete.Request.Enums;
using GoogleApi.Entities.Places.Common;
using GoogleApi.Entities.Places.Details.Request;
using Nominatim.API.Geocoders;
using Nominatim.API.Models;
using Servicio.AccesoDatos;

namespace IU.Modulos.Comunes
{
    /// <summary>
    /// Descripción breve de OpenStreetWS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    [System.Web.Script.Services.ScriptService]
    public class OpenStreetWS : System.Web.Services.WebService
    {

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<GeolocalizacionDTO> BuscarLocalizacion(string filtro)
        //{
        //    if (!filtro.ToLower().Trim().Contains("argentina"))
        //        filtro = filtro + " Argentina";

        //    List<GeolocalizacionDTO> lista = new List<GeolocalizacionDTO>();
        //    try
        //    {
        //        var x = new ForwardGeocoder();
        //        System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //        var r = x.Geocode(new ForwardGeocodeRequest
        //        {
        //            queryString = filtro,//filtro

        //            BreakdownAddressElements = true,
        //            ShowExtraTags = true,
        //            ShowAlternativeNames = true,
        //            ShowGeoJSON = true
        //        });
        //        r.Wait();

        //        string localidad = "";
        //        foreach (GeocodeResponse i in r.Result.ToList<GeocodeResponse>())
        //        {
        //            if (!string.IsNullOrEmpty(i.Address.Town))
        //                localidad = i.Address.Town;
        //            else
        //                localidad = i.Address.City;

        //            lista.Add(new GeolocalizacionDTO()
        //            {
        //                id = i.PlaceID,
        //                text = i.DisplayName,
        //                PostCode = i.Address.PostCode,
        //                Latitude = i.Latitude,
        //                Longitude = i.Longitude,
        //                HouseNumber = i.Address.HouseNumber,
        //                District = i.Address.District,
        //                State = i.Address.State,
        //                Road = i.Address.Road,
        //                Town = localidad,
        //            });
        //        }
        //    }
        //    catch (AggregateException ex)
        //    {
        //        throw ex.InnerException;
        //    }

        //    return lista;
        //}



        //GoogleSigned ApiKey = new GoogleSigned("AIzaSyBq6iLEN_gnzfCnDqJ5bCb2Q8op3Is_9iM");
        //GeocodingService CreateService()
        //{
        //    var svc = new GeocodingService(ApiKey);
        //    return svc;
        //}

        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public List<GeolocalizacionDTO> BuscarLocalizacionGoogleMaps(string filtro)
        //{
        //     if (!filtro.ToLower().Trim().Contains("argentina"))
        //        filtro = filtro + " Argentina";

        //    List<GeolocalizacionDTO> lista = new List<GeolocalizacionDTO>();
        //    try
        //    {
        //        var request = new PlacesAutoCompleteRequest
        //        {
        //            Key = "AIzaSyBq6iLEN_gnzfCnDqJ5bCb2Q8op3Is_9iM",
        //            Input = filtro,
        //            Types = new List<RestrictPlaceType> { RestrictPlaceType.Address }
        //        };

        //        var response = GooglePlaces.AutoComplete.Query(request);
        //        if (response != null)
        //        {
        //            foreach (Prediction i in response.Predictions.ToList())
        //            {
        //                GeolocalizacionDTO aux = new GeolocalizacionDTO();
        //                aux.PlaceID = i.PlaceId;
        //                aux.text = i.Description;
                        
        //                lista.Add(aux);
        //                //lista.Add(new GeolocalizacionDTO()
        //                //{
        //                //    PlaceID = i.PlaceId,
        //                //    text = i.Description,
        //                //    //PostCode = i.Address.PostCode,
        //                //    //Latitude = i.Latitude,
        //                //    //Longitude = i.Longitude,
        //                //    //HouseNumber = i.Address.HouseNumber,
        //                //    //District = i.Address.District,
        //                //    //State = i.Address.State,
        //                //    //Road = i.Address.Road,
        //                //    //Town = localidad,
        //                //});
        //            }
        //        }
        //    }
        //    catch (AggregateException ex)
        //    {
        //        throw ex.InnerException;
        //    }

        //    return lista;
        //}


        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public List<GeolocalizacionDTO> ListarLocalizaciones(string filtro)
        {
            if (!filtro.ToLower().Trim().Contains("argentina"))
                       filtro = filtro + " Argentina";

            List<GeolocalizacionDTO> lista = new List<GeolocalizacionDTO>();
            var request = new PlacesAutoCompleteRequest
            {
                Key = "AIzaSyBq6iLEN_gnzfCnDqJ5bCb2Q8op3Is_9iM",
                Input = filtro,
                Types = new List<RestrictPlaceType> { RestrictPlaceType.Address }
            };
            var response = GooglePlaces.AutoComplete.Query(request);
            if (response != null)
            {
                foreach (Prediction i in response.Predictions.ToList())
                {
                    GeolocalizacionDTO aux = new GeolocalizacionDTO();
                    aux.PlaceID = i.PlaceId;
                    aux.text = i.Description;
                    lista.Add(aux);
                }
            }
            return lista;
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public GeolocalizacionDTO ObtenerDatosCompletos(string PlaceID)
        {
            GeolocalizacionDTO retorno = new GeolocalizacionDTO();
            var request = new PlacesDetailsRequest
            {
                Key = "AIzaSyBq6iLEN_gnzfCnDqJ5bCb2Q8op3Is_9iM",
                PlaceId = PlaceID
            };
            var response = GooglePlaces.Details.Query(request);
            if(response != null)
            {
                retorno.Latitude = response.Result.Geometry.Location.Latitude;
                retorno.Longitude = response.Result.Geometry.Location.Longitude;

                foreach (var item in response.Result.AddressComponents)
                {
                        switch (item.Types.First())
                        {
                            case AddressComponentType.Street_Address:
                            case AddressComponentType.Route:
                            retorno.Road = item.LongName;
                                break;
                            case AddressComponentType.Political:
                                break;
                            case AddressComponentType.Administrative_Area_Level_1:
                            case AddressComponentType.Administrative_Area_Level_2:
                            case AddressComponentType.Administrative_Area_Level_3:
                            case AddressComponentType.Administrative_Area_Level_4:
                            case AddressComponentType.Administrative_Area_Level_5:
                            retorno.State = item.LongName;
                                break;
                            case AddressComponentType.Locality:
                            case AddressComponentType.Sublocality:
                            case AddressComponentType.Sublocality_Level_1:
                            if(string.IsNullOrEmpty(retorno.Town))
                            {
                                retorno.Town = item.LongName;
                            }
                                break;
                            case AddressComponentType.Postal_Code:
                            retorno.PostCode = item.LongName;
                                break;
                            case AddressComponentType.Street_Number:
                            retorno.HouseNumber = item.LongName;
                                break;
                            default:
                                break;
                        }
                }
               // this.ObtenerDatosCompletosLatLon(retorno);
            }
            return retorno;
        }

        //public GeolocalizacionDTO ObtenerDatosCompletosLatLon(GeolocalizacionDTO data)
        //{
        //    System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        //    var z = new ReverseGeocoder();

        //    var r3 = z.ReverseGeocode(new ReverseGeocodeRequest
        //    {
        //        Longitude = Convert.ToDouble(data.Longitude) ,
        //        Latitude = Convert.ToDouble(data.Latitude),

        //        BreakdownAddressElements = true,
        //        ShowExtraTags = true,
        //        ShowAlternativeNames = true,
        //        ShowGeoJSON = true
        //    });
        //    r3.Wait();
        //    if(r3.Status == System.Threading.Tasks.TaskStatus.RanToCompletion)
        //    {
        //        data.Town = r3.Result.Address.Town == null ? r3.Result.Address.Suburb : r3.Result.Address.Town;
        //        if (string.IsNullOrEmpty(data.Town))
        //            data.Town = r3.Result.Address.Village;

        //        if (string.IsNullOrEmpty(data.Town))
        //            data.Town = r3.Result.Address.City;

        //        data.State = r3.Result.Address.State == null  ? r3.Result.Address.City : r3.Result.Address.State;
        //        data.HouseNumber = r3.Result.Address.HouseNumber == null ? "0" : r3.Result.Address.HouseNumber;
        //        data.Road = r3.Result.Address.Road == null ? "" : r3.Result.Address.Road;
        //    }
        //    return data;
        //}
    }
}
