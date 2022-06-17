using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CurrentRoute;
using System.Linq;

public class SpecialUIMenuController : MonoBehaviour
{
    [field: SerializeField, Header("GameObjects")]
    public GameObject UICollection { get; private set; }

    [field: SerializeField, Header("Prefabs")]
    public GameObject UIOptionPrefab { get; private set; }

    [field: SerializeField]
    public ToggleGroup ToggleGroup { get; private set; }

    void Start()
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
        foreach (SpecialUI value in Enum.GetValues(typeof(SpecialUI)))
        {
            // Instantiate
            GameObject prefab = Instantiate(UIOptionPrefab, UICollection.transform);
            prefab.GetComponentInChildren<SpecialUIMenuItem>().SetData(value);
            prefab.GetComponentInChildren<Toggle>().group = ToggleGroup;
            prefab.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate { OnToggleChanged(prefab); });

            if (value == RouteController.Instance.CurrentWaypoint.SpecialUI)
            {
                prefab.GetComponentInChildren<Toggle>().isOn = true;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void OnToggleChanged(GameObject gameObject)
    {
        SpecialUIMenuItem item = gameObject.GetComponentInChildren<SpecialUIMenuItem>();

        // Set SpecialUI
        RouteController.Instance.CurrentWaypoint.SpecialUI = item.Ui;

        // Get selected item in the group
        //SpecialUIMenuItem item2 = ToggleGroup.ActiveToggles().FirstOrDefault().gameObject.GetComponentInChildren<SpecialUIMenuItem>();

        //if (item2 != null)
        //{
        //    // Set SpecialUI
        //    RouteController.Instance.CurrentWaypoint.SpecialUI = item2.Ui;
        //}
    }
}
