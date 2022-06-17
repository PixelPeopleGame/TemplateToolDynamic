using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointEditModes : MonoBehaviour
{
    [field: SerializeField, Header("Objects")]
    public GameObject PopupsMenu { get; private set; }

    [field: SerializeField]
    public GameObject VisibleUIMenu { get; private set; }

    [field: SerializeField]
    public GameObject VisibleSpecialUIMenu { get; private set; }

    /// <summary>
    /// Click event for LoadPopupMenu
    /// </summary>
    public void LoadPopupMenuClick()
    {
        EnableUI(PopupsMenu);
    }

    /// <summary>
    /// Click event for LoadUIMenu
    /// </summary>
    public void LoadUIMenuClick()
    {
        EnableUI(VisibleUIMenu);
    }

    /// <summary>
    /// Click event for LoadSpecialUIMenu
    /// </summary>
    public void LoadSpecialUIMenuClick()
    {
        EnableUI(VisibleSpecialUIMenu);
    }

    /// <summary>
    /// Also disabled all other UI
    /// </summary>
    /// <param name="ui">UI to Enable</param>
    private void EnableUI(GameObject ui)
    {
        VisibleUIMenu.SetActive(false);
        VisibleSpecialUIMenu.SetActive(false);
        PopupsMenu.SetActive(false);

        ui.SetActive(true);
    }
}
