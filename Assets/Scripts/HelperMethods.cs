using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class HelperMethods
{
    #region string APIKEY
    private static string _apiKey = "16bccc3749e5a2a0de4e9a733242b9c2";
    #endregion string APIKEY

    /// <summary>
    /// C# 8 Version of string.Split, custom made
    /// </summary>
    /// <param name="original">Text to split on</param>
    /// <param name="splitOn">To split on</param>
    /// <returns></returns>
    public static string[] Split(string original, string splitOn)
    {
        List<string> items = new List<string>();
        string temp = "";
        int index = 0;

        for (int i = 0; i < original.Length; i++)
        {
            temp += original[i];

            if (original[i] == splitOn[index])
            {
                index++;

                if (index == splitOn.Length)
                {
                    temp = temp.Substring(0, temp.Length - splitOn.Length);
                    items.Add(temp);
                    temp = "";
                    index = 0;
                }
            }
        }

        if (temp != "")
            items.Add(temp);

        return items.ToArray();
    }

    /// <summary>
    /// Removes all children from parent
    /// </summary>
    /// <param name="parent">GameObject in which the children are going to be cleared</param>
    public static void RemoveChildren(GameObject parent)
    {
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            MonoBehaviour.Destroy(parent.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Converts an Adress to a longitude latitude Vector2
    /// </summary>
    /// <param name="adress">Adress, format: Street HouseNumber, City</param>
    /// <returns></returns>
    public static async Task<Vector2> AdressToLonLat(string adress)
    {
        try
        {
            PositionStackApi data = JsonHelper<PositionStackApi>.FromJSON(await ApiHandler.GetRequest("http://api.positionstack.com/v1/forward?access_key=" + _apiKey + "&query=" + adress));
            return new Vector2(float.Parse(data.Data[0].Latitude.ToString()), float.Parse(data.Data[0].Longitude.ToString()));
        }
        catch (System.Exception ex)
        {
            Debug.Log("ERROR: HelperMethods::AdressToLonLat" + ex);
            return new Vector2();
        }
    }

    /// <summary>
    /// Turns a string into an enum value
    /// </summary>
    /// <typeparam name="T">enum</typeparam>
    /// <param name="str">name of enum value</param>
    /// <returns></returns>
    public static T StringToEnumValue<T>(string str)
    {
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            if (str == value.ToString())
            {
                return value;
            }
        }

        Debug.Log("ERROR: PopupType didnt exist!");
        return default;
    }
}
