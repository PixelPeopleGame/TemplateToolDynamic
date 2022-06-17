using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuItem : MonoBehaviour
{
    [field: SerializeField]
    public Text NameText { get; private set; }

    public VisibleUI Ui { get; private set; }

    public void SetData(VisibleUI ui)
    {
        Ui = ui;
        NameText.text = Ui.ToString();
    }
}
