using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Messages
{
    /// <summary>
    /// Type of MessageBox
    /// </summary>
    public enum MessageBoxType
    {
        OK,
        YesNo,
    }

    public class MessageBox : Singleton<MessageBox>
    {
        [field: SerializeField]
        public GameObject MessageBoxPrefab { get; private set; }

        [field: SerializeField]
        public GameObject _messagesUI { get; private set; }

        /// <summary>
        /// Returns the created MessageBox
        /// </summary>
        /// <param name="title">Title of MessageBox</param>
        /// <param name="message">Message of MessageBox</param>
        /// <param name="type">Type of MessageBox</param>
        /// <returns>The MessageBox GameObject</returns>
        public GameObject Show(string title, string message, MessageBoxType type)
        {
            // Instantiate prefab
            GameObject prefab = Instantiate(MessageBoxPrefab, _messagesUI.transform);
            prefab.GetComponent<MessageBoxUI>().Create(title, message, type);

            return prefab;
        }
    }
}
