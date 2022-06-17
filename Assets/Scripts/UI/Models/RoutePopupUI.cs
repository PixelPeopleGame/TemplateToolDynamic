using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

using CurrentRoute;
using System;

public class RoutePopupUI : MonoBehaviour
{
    [Header("Self")]
    public RectTransform rt;

    [Header("Default")]
    public Text PopupIdText;
    public Text PopupTitleText;
    public InputField PopupTitleInput;
    public InputField PopupTriggerTime;

    [field: SerializeField]
    public Dropdown PopupTypeDropdown { get; private set; }

    [Header("Description")]
    public InputField PopupDescriptionInput;

    [Header("Link")]
    public InputField PopupLinkInput;

    [Header("Questions")]
    public GameObject PopupQuestionElement;
    public InputField QuestionTitleInput;

    [Header("Game Objects")]
    public GameObject UnFolded;
    public Image FoldIndicator;

    public RectTransform ToFold;

    private bool isFolded = true;

    private ApiPopup _popup;

    public ApiPopup Popup
    {
        get { return _popup; }
        set
        {
            if (_popup == value) return;
            _popup = value;

            // Save this popup
            RouteController.Instance.SavePopup(_popup);
        }
    }

    // Returns the popup type
    public void SetVariables(ApiPopup popup)
    {
        this.Popup = popup;

        PopupIdText.text = Popup.Id + ".";
        PopupTitleText.text = Popup.Title;
        PopupTitleInput.text = Popup.Title;
        PopupTriggerTime.text = Popup.Timer.ToString();

        // Clear first
        PopupTypeDropdown.options.Clear();

        // Load PopupTypes
        foreach (PopupType value in Enum.GetValues(typeof(PopupType)))
        {
            PopupTypeDropdown.options.Add(new Dropdown.OptionData() { text = value.ToString(), image = null });
        }

        // Select PopupType
        PopupTypeDropdown.value = (int)popup.PopupType;

        // Update popup
        PopupTypeChanged();
    }

    public void SetIndex(int index)
    {
        this.Popup.Id = index;
        PopupIdText.text = Popup.Id + ".";
    }

    public void PopupTypeChanged()
    {
        // Disable all UI
        PopupDescriptionInput.transform.parent.gameObject.SetActive(false);
        PopupQuestionElement.SetActive(false);
        PopupLinkInput.transform.parent.gameObject.SetActive(false);

        switch (Popup.PopupType)
        {
            // Regular
            case PopupType.Information:
                // Information
                PopupDescriptionInput.transform.parent.gameObject.SetActive(true);
                PopupDescriptionInput.text = Popup.Description;
                break;

            // Question
            case PopupType.Question:
                // Questions
                QuestionTitleInput.text = Popup.Description;

                PopupQuestionElement.SetActive(true);
                PopupQuestionElement.GetComponent<QuestionObject>().SetVariables(Popup);
                PopupQuestionElement.GetComponent<QuestionObject>().PopupChanged += RoutePopupUI_QuestionsChanged;
                break;

            // Jackson :: Will soon be removed due to the state of pixelpeople
            case PopupType.Jackson:
                PopupDescriptionInput.transform.parent.gameObject.SetActive(true);
                PopupDescriptionInput.text = Popup.Description;
                break;

            // Nothing
            case PopupType.NoPopup:
                break;

            // Link
            case PopupType.Link:
                // Link
                PopupLinkInput.transform.parent.gameObject.SetActive(true);
                PopupLinkInput.text = Popup.Link;
                break;

            // Video
            case PopupType.Video:
                // Video
                PopupLinkInput.transform.parent.gameObject.SetActive(true);
                PopupLinkInput.text = Popup.Link;
                break;

            // Wut
            default:
                break;
        }
    }

    private void RoutePopupUI_QuestionsChanged(ApiPopup popup)
    {
        Popup = popup;
    }

    #region InspectorEvents
    /// <summary>
    /// Toggle Event for Folding and Unfolding
    /// </summary>
    public void FoldClick()
    {
        // Changed
        if (isFolded)
        {
            // Unfold
            UnFolded.SetActive(true);
            FoldIndicator.rectTransform.Rotate(0.0f, 0.0f, -180.0f);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 890.0f);
            this.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
            this.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
        }
        else
        {
            // Fold
            // Save the content here :)
            UnFolded.SetActive(false);
            FoldIndicator.rectTransform.Rotate(0.0f, 0.0f, 180.0f);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, 100.0f);
            this.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
            this.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
        }

        isFolded = !isFolded;
    }

    /// <summary>
    /// InputChanged event for title input
    /// </summary>
    public void TitleInputTextChanged()
    {
        Popup.Title = PopupTitleInput.text;
        PopupTitleText.text = Popup.Title;
    }

    /// <summary>
    /// InputChanged event for Link input
    /// </summary>
    public void LinkInputOnEndEdit()
    {
        Popup.Link = PopupLinkInput.text;
        PopupLinkInput.text = Popup.Link;
    }

    /// <summary>
    /// TextChanged event for trigger radius
    /// </summary>
    public void TriggerRadiusTextChanged()
    {
        Popup.Timer = int.Parse(PopupTriggerTime.text);
    }

    /// <summary>
    /// Dropdown option changed event for selecting a popup
    /// </summary>
    public void PopupOptionChanged()
    {
        Popup.PopupType = HelperMethods.StringToEnumValue<PopupType>(PopupTypeDropdown.options[PopupTypeDropdown.value].text);

        PopupTypeChanged();
        PopupTypeDropdown.Hide();
    }

    /// <summary>
    /// OnEndEdit for Description
    /// </summary>
    public void DescriptionChanged()
    {
        Popup.Description = PopupDescriptionInput.text;
    }

    /// <summary>
    /// OnEndEdit for Title
    /// </summary>
    public void QuestionTitleChanged()
    {
        Popup.Description = QuestionTitleInput.text;
    }

    /// <summary>
    /// Deletes the popup that got clicked
    /// </summary>
    public void DeletePopupClick()
    {
        // Delete Popup
        RouteController.Instance.DeletePopup(Popup.Id);

        // Destroy GameObject
        Destroy(this.gameObject);
    }
    #endregion InspectorEvents
}
