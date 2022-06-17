using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class ApiRoute
{
    // Author
    // Date Editted, etc
    // [JsonProperty("Name")]
    [JsonIgnore] // enable asap
    public string Name { get; set; } = "API2";

    [JsonIgnore]
    public bool IsNew { get; set; } = false;

    [JsonIgnore] // enable asap
    public IList<RouteChanges> RouteChanges { get; set; }

    [JsonIgnore] // enable asap
    public string LatestEditor { get; set; }

    [JsonProperty("Waypoints")]
    public IList<ApiWaypoint> Waypoints { get; set; }

    public ApiRoute()
    {
        this.Name = "API2";
        this.IsNew = false;
        this.RouteChanges = new List<RouteChanges>();
        this.Waypoints = new List<ApiWaypoint>();
    }
}
