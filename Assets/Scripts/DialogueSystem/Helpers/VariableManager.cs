using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VariableManager
{
    public static Dictionary<string, string> variables = new Dictionary<string, string>();

    public static void Inject(ref string s)
    {
        if (!s.Contains("|"))
        {
            return;
        }

        foreach (string variable in variables.Keys)
        {
            s = s.Replace("|" + variable + "|", variables[variable]);
        }
    }

    public static string Inject(string s)
    {
        string newString = s;

        if (!newString.Contains("|"))
        {
            return newString;
        }

        foreach (string variable in variables.Keys)
        {
            newString = newString.Replace("|" + variable + "|", variables[variable]);
        }

        return newString;
    }

    public static string[] SplitByTags(string targetText)
    {
        return targetText.Split(new char[2] { '<', '>' });
    }
}
