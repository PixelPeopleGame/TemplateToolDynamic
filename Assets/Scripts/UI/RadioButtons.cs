using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadioButtons : MonoBehaviour
{
    //public List<Button> Buttons { get; private set; }
    public Button[] Buttons;

    private void Start()
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            int x = new int();
            x = i;
            Buttons[i].GetComponent<Button>().onClick.AddListener(delegate { RadioClick(x); });
        }
    }

    //public void AddRadioButton(Button button)
    //{
    //    Buttons.Add(button);

    //    int x = new int();
    //    x = Buttons.Count - 1;
    //    button.GetComponent<Button>().onClick.AddListener(delegate { RadioClick(x); });
    //}

    /// <summary>
    /// Click event for a custom radio button
    /// </summary>
    /// <param name="id">Id in list</param>
    public void RadioClick(int id)
    {
        for (int i = 0; i < Buttons.Length; i++)
        {
            if (i == id)
            {
                // Enable Button
                Debug.Log("Enabled");
                Buttons[i].gameObject.GetComponent<Image>().color = new Color(0.18823529411f, 0.18823529411f, 0.18823529411f);

                //UIManager._routesUI.SortRoutes();
            }
            else
            {
                // Disable Button
                Debug.Log("Disabled");
                Buttons[i].gameObject.GetComponent<Image>().color = new Color(0.14509803921f, 0.14509803921f, 0.14509803921f);
            }
        }
    }
}
