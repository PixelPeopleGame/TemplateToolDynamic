using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Messages;

public class UIManager : MonoBehaviour
{
    public enum UIMenus
    {
        INLOG_UI,
        ROUTES_UI,
        ROUTE_UI,
        WAYPOINT_UI
    }

    [Header("Objects")]
    public RectTransform BackgroundRt;

    [Header("UI Menus")]
    [SerializeField] public GameObject _inlogUI;
    [SerializeField] public GameObject _routesUI;
    [SerializeField] public GameObject _routeUI;
    [SerializeField] public GameObject _messagesUI;

    [Header("Prefabs")]
    public GameObject MessageBoxPrefab;

    private void Awake()
    {
        _inlogUI.SetActive(true);
        _routesUI.SetActive(false);
        _routeUI.SetActive(false); //LoadRouteUI(new APIRoute()); // Disable if SetActive is false
    }
    void Start()
    {
        BackgroundRt.anchorMin = new Vector2(0, 0);
        BackgroundRt.anchorMax = new Vector2(1, 1);
        BackgroundRt.pivot = new Vector2(0.5f, 0.5f);
    }

    /// <summary>
    /// Changes the UI state of an UI
    /// </summary>
    /// <param name="ui">UI To change</param>
    /// <param name="active">Change to</param>
    public void ChangeUIState(UIMenus ui, bool active)
    {
        // Enable UI
        switch (ui)
        {
            case UIMenus.INLOG_UI:
                _inlogUI.SetActive(active);
                break;
            case UIMenus.ROUTES_UI:
                _routesUI.SetActive(active);
                break;
            case UIMenus.ROUTE_UI:
                _routeUI.SetActive(active);
                break;
            default:
                // Show error message
                Debug.Log("ERROR: UI doesn't exist! Keeping current UI open as a result.");
                break;
        }
    }

    /// <summary>
    /// Loads the UI for the login page
    /// </summary>
    public void LoadLoginUI()
    {
        _inlogUI.SetActive(true);
    }

    /// <summary>
    /// Loads the UI for the routes page
    /// </summary>
    public async Task LoadRoutesUI()
    {
        _routesUI.SetActive(true);
        await _routesUI.GetComponent<RoutesManager>().LoadRoutes();
    }

    /// <summary>
    /// Loads the UI for the Route page
    /// </summary>
    /// <param name="route">Route to load</param>
    public void LoadRouteUI(ApiRoute route)
    {
        _routeUI.SetActive(true);
        _routeUI.GetComponent<RouteManager>().LoadRoute(route);
    }

    /// <summary>
    /// Loads the waypoint ui
    /// </summary>
    /// <param name="waypoint">Waypoint to load</param>
    public void LoadWaypointUI(ApiWaypoint waypoint)
    {
        _routeUI.GetComponent<RouteManager>().LoadWaypoint(waypoint);
    }
    
    // Not in use atm
    public void UpdateWaypointPopups()
    {
        _routeUI.GetComponent<RouteManager>();
    }
}
