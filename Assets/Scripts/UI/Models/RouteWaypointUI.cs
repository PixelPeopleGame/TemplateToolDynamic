using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RouteWaypointUI : MonoBehaviour
{
    [Header("Game Objects")]
    public Text WaypointIdText;
    public Text WaypointnameText;

    [field: SerializeField, Header("Variables")]
    public ApiWaypoint Waypoint { get; private set; }

    // Private
    private UIManager UIManager;

    public void SetVariables(ApiWaypoint waypoint, UIManager uiManager)
    {
        this.Waypoint = waypoint;
        this.UIManager = uiManager;

        WaypointIdText.text = Waypoint.Id + ".";
        WaypointnameText.text = Waypoint.Name; 
    }

    public void SelectWaypointClick()
    {
        UIManager.LoadWaypointUI(Waypoint);
    }
}
