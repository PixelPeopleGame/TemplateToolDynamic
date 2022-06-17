using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Messages;

public class InlogManager : MonoBehaviour
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] private InputField UsernameInput;
    [SerializeField] private Text PasswordInput;

    /// <summary>
    /// Click event for logging in
    /// </summary>
    public async void LoginClick()
    {
        // Default password
        if (PasswordInput.text.Equals("123"))
        {
            // Regular login
            UIManager.ChangeUIState(UIManager.UIMenus.INLOG_UI, false);
            await UIManager.LoadRoutesUI();
        }
        else if (PasswordInput.text.Equals("root"))
        {
            // Admin login
            UIManager.ChangeUIState(UIManager.UIMenus.INLOG_UI, false);
            await UIManager.LoadRoutesUI();
        }
        else
        {
            // Tell user that the password is incorrect
            MessageBox.Instance.Show("Incorrect", "Het ingevulde wachtwoord is fout!", MessageBoxType.OK);
        }
    }

    /// <summary>
    /// Click event for exiting the application with exit code 0
    /// </summary>
    public void CloseAppClick()
    {
        Application.Quit(0);
    }
}
