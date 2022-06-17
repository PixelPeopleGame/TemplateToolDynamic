using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialUIMenuItem : MonoBehaviour
{
    [field: SerializeField]
    public Text NameText { get; private set; }

    public SpecialUI Ui { get; private set; }

    public void SetData(SpecialUI ui)
    {
        Ui = ui;
        NameText.text = Ui.ToString();
    }
}
