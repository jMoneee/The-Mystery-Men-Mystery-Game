using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class SwitchLineHandler : IHandleLine
{
    public DialogueController dialogueController { get { return DialogueController.instance; } }

    List<string> conditionalChecks = new List<string>()
        {
            { "==" },
            { "!="},
            { ">" },
            { "<" },
            { "else" }
        };

    public IEnumerator HandleLine(string line)
    {
        string[] separators = new string[] { " " };

        string variableValue = VariableManager.Inject(line.Split(separators, StringSplitOptions.RemoveEmptyEntries)[1].Trim());

        List<string> choices = new List<string>();
        Dictionary<string /*choice*/, List<string>>/*line / action list*/ actions = new Dictionary<string, List<string>>();
        bool gatheringChoices = true;
        bool gatheringActions = false;

        int cProgress = dialogueController.chapterProgress;

        while (gatheringChoices || gatheringActions)
        {
            cProgress++;
            line = dialogueController.data[cProgress].Trim();

            if (line == "{")
            {
                continue;
            }
            if (line == "}")
            {
                gatheringChoices = false;
                gatheringActions = false;
                continue;
            }

            line = line.Trim();

            if (gatheringChoices)
            {
                if (line.ToLower().StartsWith("["))
                {
                    gatheringChoices = false;
                    gatheringActions = true;
                    continue;
                }

                choices.Add(line);
                actions.Add(line, new List<string>());
            }
            else
            {
                if (line.ToLower().StartsWith("]"))
                {
                    gatheringChoices = true;
                    gatheringActions = false;
                    continue;
                }

                actions[choices.Last()].Add(line);
            }

        }

        if (choices.Count > 0)
        {
            int index = -1;
            for(int i = 0; i < choices.Count; i++)
            {
                string[] split = choices[i].Split(separators, 2, StringSplitOptions.RemoveEmptyEntries);

                Debug.Log("SPLIT VALUES");
                foreach(string s in split)
                {
                    Debug.Log(s);
                }

                string conditional = split[0].Trim();
                string conditionalValue = split.Length >= 2 ? split[1].Trim() : "";

                bool validOption = ValidateConditional(variableValue, conditional, conditionalValue);

                if (validOption)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1) 
            {
                List<string> actionList = actions[choices[index]];

                foreach (Tuple<int, string> addedData in actionList.Select(s => new Tuple<int, string>(actionList.IndexOf(s) + 1 + cProgress, s)))
                {
                    if (dialogueController.data[addedData.Item1] != addedData.Item2)
                    {
                        dialogueController.data.Insert(addedData.Item1, addedData.Item2);
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Invalid choice operation, no choices are found");
        }

        dialogueController.chapterProgress = cProgress;
        dialogueController.chapterProgress++;
        Debug.Log("cProgress at: " + cProgress + " chaoterProgessAt: " + dialogueController.chapterProgress);

        dialogueController._next = true;
        dialogueController.StopHandlingLine();

        yield break;
    }

    private bool ValidateConditional(string variableValue, string conditional, string conditionalCheck)
    {
        Debug.Log("Validate Conditional: " + variableValue + " " + conditional + " " + conditionalCheck);

        bool valid = false;
        switch (conditional)
        {
            case "==":
                valid = variableValue == conditionalCheck;
                break;
            case "!=":
                valid = variableValue != conditionalCheck;
                break;
            case "else":
                valid = true;
                break;
            case ">":
                valid = float.Parse(variableValue) > float.Parse(conditionalCheck);
                break;
            case "<":
                valid = float.Parse(variableValue) < float.Parse(conditionalCheck);
                break;
        }

        return valid;
    }
}
