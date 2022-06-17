using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionStackApi
{
    /*
     * Location to lon, lat
    http://api.positionstack.com/v1/forward?access_key=16bccc3749e5a2a0de4e9a733242b9c2&query=Von%20Flotowlaan%201a,%20Eindhoven

     * Image
    https://open.mapquestapi.com/staticmap/v5/map?key=SBEtfEnKF9W47C6Aysa38QMkxBch0iIq&center=Von%20Flotowlaan%201a,%20Eindhoven&size=500,500&zoom=15
     */

    [JsonProperty("data")]
    public IList<PositionData> Data { get; set; }
}
