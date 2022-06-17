using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionData
{
    [JsonProperty("latitude")]
    public double Latitude { get; set; }

    [JsonProperty("longitude")]
    public double Longitude { get; set; }

    [JsonProperty("type")]
    public string Type { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("number")]
    public string Number { get; set; }

    [JsonProperty("postal_code")]
    public string PostalCode { get; set; }

    [JsonProperty("street")]
    public string Street { get; set; }

    [JsonProperty("confidence")]
    public int Confidence { get; set; }

    [JsonProperty("region")]
    public string Region { get; set; }

    [JsonProperty("region_code")]
    public string RegionCode { get; set; }

    [JsonProperty("county")]
    public object County { get; set; }

    [JsonProperty("locality")]
    public string Locality { get; set; }

    [JsonProperty("administrative_area")]
    public object AdministrativeArea { get; set; }

    [JsonProperty("neighbourhood")]
    public string Neighbourhood { get; set; }

    [JsonProperty("country")]
    public string Country { get; set; }

    [JsonProperty("country_code")]
    public string CountryCode { get; set; }

    [JsonProperty("continent")]
    public string Continent { get; set; }

    [JsonProperty("label")]
    public string Label { get; set; }
}
