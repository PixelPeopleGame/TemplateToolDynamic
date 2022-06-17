using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoutesRouteUI : MonoBehaviour
{
    [Header("Game Objects")]
    public Text RouteNameText;
    public Text RouteDateText;
    public Toggle DeleteToggle;

    //[field: SerializeField, Header("Variables")]
    //public APIRoute Route { get; private set; }
    private string FileName;

    // Privates
    private UIManager UIManager;

    public void SetVariables(/*APIRoute route, */string fileName, UIManager uiManager)
    {
        // Set data
        this.FileName = fileName;
        this.UIManager = uiManager;
        //this.Route = route;

        RouteNameText.text = FileName; //Route.Name;
        RouteDateText.text = "Test Route Date | API1"; //Route.RouteChanges[Route.RouteChanges.Count - 1].Date.ToString();
    }

    public async void SelectRouteClick()
    {
        // Get file
        ApiRoute route = JsonHelper<ApiRoute>.FromJSON(await ApiHandler.GetRequest("https://pixelpeople.nl/PixelPeopleAPI/RouteFolder/" + FileName));
        route.Name = HelperMethods.Split(FileName, ".")[0];

        UIManager.LoadRouteUI(route);
        UIManager.ChangeUIState(UIManager.UIMenus.ROUTES_UI, false);
    }
}
