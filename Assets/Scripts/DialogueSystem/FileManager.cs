using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileManager : MonoBehaviour
{
    /// <summary>
    /// Converts an array of strings into a list of the same values.
    /// </summary>
    /// <returns>The to list.</returns>
    /// <param name="array">Array.</param>
    /// <param name="removeBlankLines">If set to <c>true</c> remove blank lines.</param>
    public static List<string> ArrayToList(string[] array, bool removeBlankLines = true)
    {
        List<string> list = new List<string>();
        for (int i = 0; i < array.Length; i++)
        {
            string s = array[i];
            if (s.Length > 0 || !removeBlankLines)
            {
                list.Add(s);
            }
        }
        return list;
    }

    /// <summary>
	/// Read a text asset and return a list of lines
	/// </summary>
	/// <returns>The text asset.</returns>
	/// <param name="txt">Text.</param>
	public static List<string> ReadTextAsset(TextAsset txt)
    {
        string[] lines = txt.text.Split('\n', '\r');

        return ArrayToList(lines);
    }
}