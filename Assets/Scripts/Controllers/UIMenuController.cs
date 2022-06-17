using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CurrentRoute;

public class UIMenuController : MonoBehaviour
{
    [field: SerializeField, Header("GameObjects")]
    public GameObject UICollection { get; private set; }

    [field: SerializeField, Header("Prefabs")]
    public GameObject UIOptionPrefab { get; private set; }

    private void Start()
    {
        LoadOptions();
        RouteController.Instance.OnWaypointChanged += OpWaypointChanged;
    }

    private void OpWaypointChanged()
    {
        HelperMethods.RemoveChildren(UICollection);
        LoadOptions();
    }

    /// <summary>
    /// Loads the Values of the UI Enum and displays them
    /// </summary>
    public void LoadOptions()
    {
        // To make sure
        HelperMethods.RemoveChildren(UICollection);

        // Fill Collection
        foreach (VisibleUI value in Enum.GetValues(typeof(VisibleUI)))
        {
            // Instantiate
            GameObject prefab = Instantiate(UIOptionPrefab, UICollection.transform);
            prefab.GetComponentInChildren<UIMenuItem>().SetData(value);
            prefab.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate { OnToggleChanged(prefab); });

            foreach (VisibleUI item in RouteController.Instance.CurrentWaypoint.UIVisible)
            {
                if (item == value)
                {
                    prefab.GetComponentInChildren<Toggle>().isOn = true;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// ToggleChanged event for updating the UI enabling
    /// </summary>
    /// <param name="gameObject">Object that got hit</param>
    public void OnToggleChanged(GameObject gameObject)
    {
        UIMenuItem item = gameObject.GetComponentInChildren<UIMenuItem>();

        // Change popup
        if (gameObject.GetComponentInChildren<Toggle>().isOn)
        {
            // Add if not already in there
            for (int i = 0; i < RouteController.Instance.CurrentWaypoint.UIVisible.Count; i++)
            {
                if (RouteController.Instance.CurrentWaypoint.UIVisible[i] == item.Ui)
                {
                    return;
                }
            }

            RouteController.Instance.CurrentWaypoint.UIVisible.Add(item.Ui);
        }
        else
        {
            // Remove if in there
            for (int i = 0; i < RouteController.Instance.CurrentWaypoint.UIVisible.Count; i++)
            {
                if (RouteController.Instance.CurrentWaypoint.UIVisible[i] == item.Ui)
                {
                    RouteController.Instance.CurrentWaypoint.UIVisible.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
