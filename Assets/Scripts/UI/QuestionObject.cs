using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestionObject : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject QuestionCollection;
    public List<GameObject> QuestionObjects;

    [Header("Prefab")]
    public GameObject QuestionPrefab;

    // List changed event
    private ApiPopup _popup;

    public ApiPopup Popup
    {
        get { return _popup; }
        set
        {
            if (_popup == value) return;
            _popup = value;

            if (PopupChanged != null)
                PopupChanged(_popup);
        }
    }

    public delegate void OnVariableChangeDelegate(ApiPopup popup);
    public event OnVariableChangeDelegate PopupChanged;

    /// <summary>
    /// Constrcutor for loading the questions
    /// </summary>
    /// <param name="popup"></param>
    public void SetVariables(ApiPopup popup)
    {
        // Set popup
        this.Popup = popup;
        HelperMethods.RemoveChildren(QuestionCollection);

        // Loop through queestions and instantiate them
        for (int i = 0; i < Popup.QuestionAnswers.Count; i++)
        {
            GameObject prefab = Instantiate(QuestionPrefab, QuestionCollection.transform);
            prefab.GetComponentInChildren<InputField>().text = Popup.QuestionAnswers[i];
            prefab.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate { OnQuestionTextChanged(); });

            prefab.GetComponentInChildren<Toggle>().isOn = Popup.QuestionAnswers[i] == Popup.CorrectAnswer;
            prefab.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate { OnToggleChanged(prefab); });
            prefab.GetComponentInChildren<Toggle>().group = QuestionCollection.GetComponent<ToggleGroup>();

            prefab.GetComponentInChildren<Button>().onClick.AddListener(delegate { DeleteAnswerClick(prefab); });
        }
    }

    /// <summary>
    /// Saves the Popup
    /// </summary>
    public void SaveCurrentPopup()
    {
        // If you know a different way, implement it
        ApiPopup popup = new ApiPopup()
        {
            Id = Popup.Id,
            IndexInList = Popup.IndexInList,
            Title = Popup.Title,
            Description = Popup.Description,
            Timer = Popup.Timer,
            PopupType = Popup.PopupType,
            QuestionAnswers = Popup.QuestionAnswers,
            Link = Popup.Link,
            CorrectAnswer = Popup.CorrectAnswer
        };

        // Change popup
        Popup = popup;
    }

    #region ClickEvents
    /// <summary>
    /// Click event for deleting an answer
    /// </summary>
    /// <param name="gameobject"></param>
    public void DeleteAnswerClick(GameObject gameobject)
    {
        // Destroy GameObject
        Destroy(gameobject);

        // Update popup
        OnQuestionTextChanged();
    }

    /// <summary>
    /// Click event for adding a question
    /// </summary>
    public void AddQuestionClick()
    {
        // Don't save yet
        GameObject prefab = Instantiate(QuestionPrefab, QuestionCollection.transform);
        prefab.GetComponentInChildren<InputField>().onEndEdit.AddListener(delegate { OnQuestionTextChanged(); });
        prefab.GetComponentInChildren<Toggle>().onValueChanged.AddListener(delegate { OnToggleChanged(prefab); });
        prefab.GetComponentInChildren<Toggle>().group = QuestionCollection.GetComponent<ToggleGroup>();
        prefab.GetComponentInChildren<Button>().onClick.AddListener(delegate { DeleteAnswerClick(prefab); });

        // Set first answer to correct answer by default
        if (QuestionCollection.transform.childCount == 1)
        {
            Popup.QuestionAnswers.Add("");
            prefab.GetComponentInChildren<Toggle>().isOn = true;
        }

        // Save
        
    }
    
    /// <summary>
    /// InputTextChanged event for updating a question change
    /// </summary>
    public void OnQuestionTextChanged()
    {
        // Temporary new List
        List<string> newQuestions = new List<string>();

        // Get Input values
        for (int i = 0; i < QuestionCollection.transform.childCount; i++)
        {
            newQuestions.Add(QuestionCollection.transform.GetChild(i).GetComponentInChildren<InputField>().text);
        }

        // Save popup
        Popup.QuestionAnswers = newQuestions;
        SaveCurrentPopup();
    }

    /// <summary>
    /// ToggleChanged event for updating the correct answer
    /// </summary>
    /// <param name="gameObject">Object that got hit</param>
    public void OnToggleChanged(GameObject gameObject)
    {
        // Change popup
        Popup.CorrectAnswer = gameObject.GetComponentInChildren<Toggle>().isOn ? gameObject.GetComponentInChildren<InputField>().text : Popup.CorrectAnswer;
        SaveCurrentPopup();
    }
    #endregion ClickEvents
}
