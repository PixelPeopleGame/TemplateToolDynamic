using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class MessageBoxUI : MonoBehaviour
{
    [field: SerializeField, Header("Text Components")]
    public Text TitleText { get; private set; }

    [field: SerializeField]
    public Text MessageText { get; private set; }

    [field: SerializeField, Header("Button Components")]
    public Button OkButton { get; private set; }

    [field: SerializeField]
    public Button YesButton { get; private set; }

    [field: SerializeField]
    public Button NoButton { get; private set; }

    private string _returnValue = "";
    public string ReturnValue
    {
        get { return _returnValue; }
        set
        {
            if (_returnValue == value) return;
            _returnValue = value;

            if (OnVariableChange != null)
                OnVariableChange(_returnValue, this.gameObject);
        }
    }

    public delegate void OnVariableChangeDelegate(string value, GameObject messageBox);
    public event OnVariableChangeDelegate OnVariableChange;

    public void Create(string title, string message, MessageBoxType type)
    {
        // Set Variables
        this.TitleText.text = title;
        this.MessageText.text = message;

        switch (type)
        {
            case MessageBoxType.OK:
                OkButton.gameObject.SetActive(true);
                break;
            case MessageBoxType.YesNo:
                YesButton.gameObject.SetActive(true);
                NoButton.gameObject.SetActive(true);
                break;
        }
    }

    public void CloseMessageBoxClick()
    {
        ReturnValue = "Cancel";
        this.Close();
    }

    #region ButtonClicks
    /// <summary>
    /// Click event for OK button
    /// </summary>
    public void OkClick()
    {
        ReturnValue = "OK";
        this.Close();
    }

    /// <summary>
    /// Click event for Yes button
    /// </summary>
    public void YesClick()
    {
        // Notify the creator that Yes has been pressed
        ReturnValue = "Yes";
        //this.Close();
    }

    /// <summary>
    /// Click event for No button
    /// </summary>
    public void NoClick()
    {
        ReturnValue = "No";
        //this.Close();
    }
    #endregion ButtonClicks

    /// <summary>
    /// Closes the MessageBox
    /// </summary>
    public void Close()
    {
        Destroy(this.gameObject);
    }
}
