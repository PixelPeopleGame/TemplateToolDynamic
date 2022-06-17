using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class RoutesManager : MonoBehaviour
{
    [field: SerializeField]
    public UIManager UIManager { get; private set; }

    [field: SerializeField, Header("Prefabs")]
    public GameObject RoutesRoutePrefab { get; private set; }

    [Header("UI")]
    public GameObject RouteCollection;
    public List<GameObject> Routes;

    private List<string> Files = new List<string>();

    [Header("Action Controls")]
    public Text SearchText;

    /// <summary>
    /// Loads all routes
    /// </summary>
    public async Task LoadRoutes()
    {
        // Define list
        List<ApiRoute> routes = new List<ApiRoute>();

        // Clear existing lists
        HelperMethods.RemoveChildren(RouteCollection);

        // Call api data
        await GetApiData();

        // Loop through results
        for (int i = 0; i < Files.Count; i++)
        {
            // Instantiate prefab
            GameObject prefab = Instantiate(RoutesRoutePrefab, RouteCollection.transform);
            prefab.GetComponent<RoutesRouteUI>().SetVariables(Files[i], UIManager);
        }
    }

    /// <summary>
    /// Gets all data and puts it inside the Files List<string>()
    /// </summary>
    private async Task GetApiData()
    {
        try
        {
            // Fill Data
            string[] tempSplit = HelperMethods.Split(await ApiHandler.GetRequest("https://pixelpeople.nl/PixelPeopleAPI/RouteFolder/"), "a href=\"");
            //string[] tempSplit = HelperMethods.Split(StartCoroutine(ApiHandler.IGetRequest("https://pixelpeople.nl/PixelPeopleAPI/RouteFolder/")).ToString(), "a href=\"");

            // Clear Files
            Files.Clear();

            for (int i = 2; i < tempSplit.Length - 1; i++)
            {
                Files.Add(HelperMethods.Split(tempSplit[i], "\">")[0]);
            }
        }
        catch (System.Exception ex)
        {
            Debug.Log("ERROR: " + ex);
        }
    }

    #region Actions
    /// <summary>
    /// Click event for searching a route
    /// </summary>
    public void SearchClick()
    {
        // Clear existing items
        HelperMethods.RemoveChildren(RouteCollection);

        // Instantiate all files as a gameobject prefab
        for (int i = 0; i < Files.Count; i++)
        {
            if (Files[i].ToLower().Contains(SearchText.text.ToLower()))
            {
                GameObject prefab = Instantiate(RoutesRoutePrefab, RouteCollection.transform);
                prefab.GetComponent<RoutesRouteUI>().SetVariables(Files[i], UIManager);
            }
        }
    }

    /// <summary>
    /// Click event for refetching the api data
    /// </summary>
    public async void RefreshClick()
    {
        // Refresh data
        await GetApiData();

        // Call search
        SearchClick();
    }

    /// <summary>
    /// Click event for creating a new route
    /// </summary>
    public void NewRouteClick()
    {
        // New Route
        UIManager.LoadRouteUI(new ApiRoute());
        UIManager.ChangeUIState(UIManager.UIMenus.ROUTES_UI, false);
    }

    /// <summary>
    /// Click event for deleting the selected routes
    /// </summary>
    public void DeleteSelectionClick()
    {
        GameObject messageBox = MessageBox.Instance.Show("Confirm", "Wilt u de geslecteerde items verwijderen?", MessageBoxType.YesNo);
        messageBox.GetComponent<MessageBoxUI>().OnVariableChange += MessageBoxReturnListener;
    }

    private void MessageBoxReturnListener(string value, GameObject messageBox)
    {
        messageBox.GetComponent<MessageBoxUI>().Close();

        if (value == "Yes")
        {
            for (int i = 0; i < RouteCollection.transform.childCount; i++)
            {
                if (RouteCollection.transform.GetChild(i).GetComponent<RoutesRouteUI>().DeleteToggle.isOn)
                {
                    // DELETE request to Api


                    // For now just remove them from the list
                    Destroy(RouteCollection.transform.GetChild(i).gameObject);
                }
            }

            MessageBox.Instance.Show("Melding", "De geselecteerde items zijn zogenaamd verwijdered, ", MessageBoxType.OK);
        }
    }

    /// <summary>
    /// Click event for logging out
    /// </summary>
    public void LogoutClick()
    {
        UIManager.LoadLoginUI();
        UIManager.ChangeUIState(UIManager.UIMenus.ROUTES_UI, false);
    }
    #endregion Actions

    #region Sorting
    public enum Sorting
    {
        AZ,
        ZA
    }

    public void SortRoutes(Sorting sorting)
    {
        List<string> newFiles = new List<string>();

        for (int i = 0; i < Files.Count; i++)
        {

        }
    }
    #endregion Sorting
}
