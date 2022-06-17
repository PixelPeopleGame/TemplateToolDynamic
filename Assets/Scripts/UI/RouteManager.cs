using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

using CurrentRoute;
using Messages;

public class RouteManager : MonoBehaviour
{
    [field: SerializeField, Header("UI Manager")]
    public UIManager UIManager { get; private set; }
    public PopupManager PopupManager;

    [field: SerializeField, Header("Prefabs")]
    public GameObject RouteWaypointPrefab { get; private set; }

    [field: SerializeField]
    public GameObject WaypointPopupPrefab { get; private set; }

    [Header("UI Objects")]
    public GameObject WaypointCollection;
    public List<GameObject> Waypoints;
    public GameObject PopupCollection;

    public RectTransform MapRt;
    public RectTransform toMove;
    public RectTransform rtRadius;

    [Header("Location")]
    public InputField WaypointNameInput;
    public InputField TriggerRadiusInput;
    public InputField LatitudeInput;
    public InputField LongitudeInput;
    public InputField AdressInput;

    public RawImage rawImage;

    private double MapLatCenter;
    private double MapLonCenter;
    #region private string APIKEY
    private string _apiKey = "16bccc3749e5a2a0de4e9a733242b9c2";
    #endregion private string APIKEY

    private void Update()
    {
        MapClick();
    }

    #region LoadUI
    /// <summary>
    /// Loads the provided route
    /// </summary>
    /// <param name="route">Route to load</param>
    public void LoadRoute(ApiRoute route)
    {
        // Set Route
        RouteController.Instance.SetCurrentRoute(route);

        if (RouteController.Instance.Route != null)
        {
            // Add one if none are there
            if (RouteController.Instance.Route.Waypoints.Count == 0)
            {
                RouteController.Instance.Route.Waypoints.Add(new ApiWaypoint());
            }

            // Load waypoints
            LoadWaypoints();

            // Set Current waypoint
            RouteController.Instance.CurrentWaypoint = RouteController.Instance.Route.Waypoints[0];
            LoadWaypoint(RouteController.Instance.CurrentWaypoint);
        }
        else
        {
            // Display error message
            Debug.Log("ERROR Routemanager.cs::LoadRoate: Route could not be loaded because Route is null!");
        }
    }

    /// <summary>
    /// Loads the UI for the Waypoints on the left
    /// </summary>
    public void LoadWaypoints()
    {
        // Remove current waypoints
        HelperMethods.RemoveChildren(WaypointCollection);

        // Load Waypoints
        for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
        {
            // Instantiate prefab
            GameObject prefab = Instantiate(RouteWaypointPrefab, WaypointCollection.transform);
            prefab.GetComponent<RouteWaypointUI>().SetVariables(RouteController.Instance.Route.Waypoints[i], UIManager);
        }
    }

    /// <summary>
    /// Loads the selected waypoint
    /// </summary>
    /// <param name="waypoint">Waypoint to load</param>
    public void LoadWaypoint(ApiWaypoint waypoint)
    {
        RouteController.Instance.CurrentWaypoint = waypoint;

        // Show Map
        //UpdateMap(CurrentWaypoint.Latitude, CurrentWaypoint.Longitude);

        // Set values
        WaypointNameInput.text = RouteController.Instance.CurrentWaypoint.Name;
        TriggerRadiusInput.text = RouteController.Instance.CurrentWaypoint.Radius.ToString();
        LatitudeInput.text = RouteController.Instance.CurrentWaypoint.Latitude.ToString();
        LongitudeInput.text = RouteController.Instance.CurrentWaypoint.Longitude.ToString();
        AdressInput.text = "";

        PopupManager.LoadPopups();
    }
    #endregion LoadUI

    #region MapSelecting
    /// <summary>
    /// Click event for moving the marker on the map and re-centering the map
    /// </summary>
    public void MapClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 positionOnMap = new Vector2(
                Input.mousePosition.x - MapRt.position.x + MapRt.rect.width / 2.0f,
                Input.mousePosition.y - MapRt.position.y + MapRt.rect.height / 2.0f - MapRt.sizeDelta.y
                );

            Vector2 newPositionOnMap = new Vector2(
                        positionOnMap.x - MapRt.sizeDelta.x / 2.0f,
                        positionOnMap.y + MapRt.sizeDelta.y / 2.0f
                        );

            // Check if click was within map boundaries
            if (positionOnMap.y <= 0 && positionOnMap.y >= -Mathf.Abs(MapRt.rect.height))
            {
                if (positionOnMap.x >= 0 && positionOnMap.x <= MapRt.rect.width)
                {
                    // Move object
                    toMove.localPosition = newPositionOnMap;

                    //
                    // Research about this code has bin done with the default location of MAD
                    // This code will NOT work close to the north or south pole
                    // Nor will this work with values of lonitude or latotitdue...
                    // That are above 180 or below -180
                    //

                    // Meters per pixel
                    // https://wiki.openstreetmap.org/wiki/Zoom_levels
                    // getal * cos(lat in degrees) / 2 ^ (zoom + 8);
                    // (40075016.686 * cos(51.428708 degrees) / 2 ^ (16 + 8));
                    // (40075016.686f * Mathf.Cos(float.Parse((Mathf.Deg2Rad * latitude).ToString()))) / Mathf.Pow(2, 16 + 8) * pixels;
                    float metersPerPixel = (40075016.686f * Mathf.Cos(float.Parse((Mathf.Deg2Rad * MapLatCenter).ToString()))) / Mathf.Pow(2, 16 + 8);
                    float xMeter = metersPerPixel * newPositionOnMap.x;
                    float yMeter = metersPerPixel * newPositionOnMap.y;

                    // Calculate lat and lon
                    // https://stackoverflow.com/questions/7477003/calculating-new-longitude-latitude-from-old-n-meters
                    // Lat = y == 51.428708 + (50meter * 0.0000089)
                    // Lon = x == 5.457311 + (100meter * 0.0000089) / cos(51.428708 * 0.018)
                    double newLat = MapLatCenter + yMeter * 0.0000089f;
                    double newLon = MapLonCenter + xMeter * 0.0000089f / Math.Cos(MapLatCenter * 0.018);

                    // Set Text Inputs
                    LatitudeInput.text = newLat.ToString();
                    LongitudeInput.text = newLon.ToString();

                    // New Map
                    UpdateMap(newLat, newLon);
                }
            }
        }
    }

    public async void UpdateMap(double latitude, double longitude)
    {
        rawImage.gameObject.SetActive(true);

        // Set values
        LatitudeInput.text = latitude.ToString();
        LongitudeInput.text = longitude.ToString();

//#if DEBUG
        string MapQuestApiKey = "SBEtfEnKF9W47C6Aysa38QMkxBch0iIq";
//#endif

        string url = "https://open.mapquestapi.com/staticmap/v5/map" +
            $"?key={MapQuestApiKey}" +
            $"&center={latitude.ToString().Replace(',', '.')},{longitude.ToString().Replace(',', '.')}" +
            $"&size=500,400&zoom=16";

        Debug.Log(url);

        Texture2D texture = await ApiHandler.GetRemoteTexture(
            url
            //"https://open.mapquestapi.com/staticmap/v5/map?key=SBEtfEnKF9W47C6Aysa38QMkxBch0iIq&center=51.428708,5.457311&size=500,500&zoom=16"
            );

        // Set Texture
        rawImage.texture = texture;

        // Applies to all textures but this should be "better" appearently
        //rawImage.material.mainTexture = texture;

        toMove.localPosition = new Vector2(0.0f, 0.0f);

        // Set private
        MapLatCenter = latitude;
        MapLonCenter = longitude;

        // Update Route
        for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
        {
            if (RouteController.Instance.Route.Waypoints[i].Id == RouteController.Instance.CurrentWaypoint.Id)
            {
                RouteController.Instance.Route.Waypoints[i].Latitude = latitude;
                RouteController.Instance.Route.Waypoints[i].Longitude = longitude;
            }
        }
    }
    #endregion MapSelecting

    #region ClickEvents
    /// <summary>
    /// Click event for going to the previous waypoint
    /// </summary>
    public void PreviousWaypointClick()
    {
        for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
        {
            if (RouteController.Instance.Route.Waypoints[i].Id == RouteController.Instance.CurrentWaypoint.Id)
            {
                if (i > 0)
                {
                    RouteController.Instance.SaveCurrentRoute();
                    RouteController.Instance.CurrentWaypoint = RouteController.Instance.Route.Waypoints[i - 1];
                    LoadWaypoint(RouteController.Instance.CurrentWaypoint);
                }
            }
        }
    }

    /// <summary>
    /// Click event for going to the next waypoint
    /// </summary>
    public void NextWaypointClick()
    {
        int waypointId = RouteController.Instance.Route.Waypoints.IndexOf(RouteController.Instance.Route.Waypoints.Where(x => x.Id == RouteController.Instance.CurrentWaypoint.Id).FirstOrDefault());

        if (waypointId < RouteController.Instance.Route.Waypoints.Count - 1)
        {
            RouteController.Instance.SaveCurrentWaypoint();
            RouteController.Instance.CurrentWaypoint = RouteController.Instance.Route.Waypoints[waypointId + 1];
            LoadWaypoint(RouteController.Instance.CurrentWaypoint);
        }
    }

    /// <summary>
    /// Click event for going to the clicked waypoint
    /// </summary>
    public void NewWaypointClick()
    {
        RouteController.Instance.SaveCurrentWaypoint();

        ApiWaypoint waypoint = new ApiWaypoint();
        int waypointId = 0;

        for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
        {
            if (RouteController.Instance.Route.Waypoints[i].Id == waypointId)
            {
                waypointId++;
                i = 0;
            }
        }

        waypoint.Id = waypointId;

        // Add new waypoint
        RouteController.Instance.Route.Waypoints.Add(waypoint);

        // Instantiate prefab
        GameObject prefab = Instantiate(RouteWaypointPrefab, WaypointCollection.transform);
        prefab.GetComponent<RouteWaypointUI>().SetVariables(waypoint, UIManager);

        // Show waypoint
        LoadWaypoint(waypoint);
    }

    /// <summary>
    /// Click event for deleting the currect waypoint
    /// </summary>
    public void DeleteWaypointClick()
    {
        GameObject messageBox = MessageBox.Instance.Show("Confirm", "Wilt u de waypoint verwijderen?", MessageBoxType.YesNo);
        messageBox.GetComponent<MessageBoxUI>().OnVariableChange += ReturnListenerRemoveWaypoint;
    }

    /// <summary>
    /// Click event for saving the route to the database
    /// </summary>
    public void SaveRouteClick()
    {
        // Save Route
        RouteController.Instance.SaveCurrentRoute();
    }

    /// <summary>
    /// Click event for stopping the editing of the route 
    /// </summary>
    public void StopEdittingClick()
    {
        GameObject messageBox = MessageBox.Instance.Show("Confirm", "Wilt u stoppen? \nGegevens worden NIET opgeslagen", MessageBoxType.YesNo);
        messageBox.GetComponent<MessageBoxUI>().OnVariableChange += ReturnListenerStopEditting;
    }

    /// <summary>
    /// Click event for creating a new popup
    /// </summary>
    public void NewWaypointPopupClick()
    {
        ApiPopup popup = new ApiPopup();
        PopupManager.AddPopup(popup);
    }

    /// <summary>
    /// InputFieldChanged event for changing the radius indicator on the map
    /// </summary>
    public void RadiusChanged()
    {
        int radius = int.Parse(TriggerRadiusInput.text);

        for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
        {
            if (RouteController.Instance.Route.Waypoints[i].Id == RouteController.Instance.CurrentWaypoint.Id)
            {
                RouteController.Instance.Route.Waypoints[i].Radius = radius;
            }
        }

        // Update Marker
        if (MapLatCenter != 0.0)
        {
            float metersPerPixel = (40075016.686f * Mathf.Cos(float.Parse((Mathf.Deg2Rad * MapLatCenter).ToString()))) / Mathf.Pow(2, 16 + 8);
            rtRadius.sizeDelta = new Vector2(radius * 2.0f / metersPerPixel, radius * 2.0f / metersPerPixel);
        }
    }

    /// <summary>
    /// InputFieldChanged event for chaning the waypoint name
    /// </summary>
    public void WaypointNameChanged()
    {
        RouteController.Instance.CurrentWaypoint.Name = WaypointNameInput.text;
        LoadWaypoints();
    }

    /// <summary>
    /// Click event for searching a location based on an Adres
    /// </summary>
    public async void SearchAdressClick()
    {
        // Fill data
        string text = AdressInput.text;

        if (!string.IsNullOrEmpty(text))
        {
            PositionStackApi data = JsonHelper<PositionStackApi>.FromJSON(await ApiHandler.GetRequest("http://api.positionstack.com/v1/forward?access_key=" + _apiKey + "&query=" + text));
            
            if (data != null)
            {
                // Maak aanpassingen aan UI
                RouteController.Instance.CurrentWaypoint.Latitude = data.Data[0].Latitude;
                RouteController.Instance.CurrentWaypoint.Longitude = data.Data[0].Longitude;

                UpdateMap(data.Data[0].Latitude, data.Data[0].Longitude);
            }
            else
            {
                // geef fout melding weer
                MessageBox.Instance.Show("Fout", "Ingegeven adres bestaat niet!", MessageBoxType.OK);
            }
        }
    }

    /// <summary>
    /// Click event for searching a location based on latitude and longitude
    /// </summary>
    public void SearchLatLonClick()
    {
        UpdateMap(double.Parse(LatitudeInput.text), double.Parse(LongitudeInput.text));
    }

    /// <summary>
    /// Makes sure that dots are converted to kommas
    /// </summary>
    public void LatitudeInputChanged()
    {
        LatitudeInput.text = LatitudeInput.text.Replace('.', ',');
    }

    /// <summary>
    /// Makes sure that dots are converted to kommas
    /// </summary>
    public void LongitudeInputChanged()
    {
        LongitudeInput.text = LongitudeInput.text.Replace('.', ',');
    }

    /// <summary>
    /// Update whenever the user finished edditing
    /// </summary>
    public void LatitudeOnEndEdit()
    {
        RouteController.Instance.CurrentWaypoint.Latitude = double.Parse(LatitudeInput.text);
    }

    /// <summary>
    /// Update whenever the user finished edditing
    /// </summary>
    public void LongitudeOnEndEdit()
    {
        RouteController.Instance.CurrentWaypoint.Longitude = double.Parse(LongitudeInput.text);
    }
    #endregion ClickEvents

    #region EventListeners
    private void ReturnListenerRemoveWaypoint(string value, GameObject messageBox)
    {
        messageBox.GetComponent<MessageBoxUI>().Close();

        if (value == "Yes")
        {
            // Delete waypoint
            for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
            {
                if (RouteController.Instance.Route.Waypoints[i].Id == RouteController.Instance.CurrentWaypoint.Id)
                {
                    RouteController.Instance.Route.Waypoints.RemoveAt(i);
                }
            }

            // Update ID's
            for (int i = 0; i < RouteController.Instance.Route.Waypoints.Count; i++)
            {
                RouteController.Instance.Route.Waypoints[i].Id = i;
            }

            // Update UI
            LoadRoute(RouteController.Instance.Route);
        }
    }

    private async void ReturnListenerStopEditting(string value, GameObject messageBox)
    {
        messageBox.GetComponent<MessageBoxUI>().Close();

        if (value == "Yes")
        {
            UIManager.ChangeUIState(UIManager.UIMenus.ROUTE_UI, false);
            await UIManager.LoadRoutesUI();
        }
    }
    #endregion EventListeners
}
