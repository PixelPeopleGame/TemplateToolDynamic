using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CurrentRoute;

public class PopupManager : MonoBehaviour
{
    [field: SerializeField, Header("UI Manager")]
    public UIManager UIManager { get; private set; }

    [Header("UI")]
    public GameObject PopupCollection;

    [field: SerializeField, Header("Prefabs")]
    public GameObject PopupPrefab { get; private set; }

    /// <summary>
    /// Clears current popups and loads given popups
    /// </summary>
    /// <param name="popups">Popups to load</param>
    public void LoadPopups()
    {
        // Clear popups
        HelperMethods.RemoveChildren(PopupCollection);

        // Load popups
        for (int i = 0; i < RouteController.Instance.CurrentWaypoint.Popups.Count; i++)
        {
            RouteController.Instance.CurrentWaypoint.Popups[i].Id = i;
            InstantiatePopup(RouteController.Instance.CurrentWaypoint.Popups[i]);
        }
    }

    /// <summary>
    /// Adds a new popup to the list
    /// </summary>
    /// <param name="popup">Popup to add</param>
    public void AddPopup(ApiPopup popup)
    {
        popup.Id = RouteController.Instance.CurrentWaypoint.Popups.Count;
        RouteController.Instance.CurrentWaypoint.Popups.Add(popup);
        InstantiatePopup(popup);
    }

    public void InstantiatePopup(ApiPopup popup)
    {
        GameObject prefab = Instantiate(PopupPrefab, PopupCollection.transform);
        prefab.GetComponent<RoutePopupUI>().SetVariables(popup);
    }
}
