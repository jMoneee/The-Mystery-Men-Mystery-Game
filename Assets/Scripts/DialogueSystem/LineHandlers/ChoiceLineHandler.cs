using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoiceLineHandler : IHandleLine
{
    public DialogueController dialogueController { get { return DialogueController.instance; } }

    public IEnumerator HandleLine(string line)
    {
        string title = line.Split('"')[1];

        List<string> choices = new List<string>();
        Dictionary<string /*choice*/, List<string>>/*line / action list*/ actions = new Dictionary<string, List<string>>();
        bool gatheringChoices = true;
        bool gatheringActions = false;

        int cProgress = dialogueController.chapterProgress;

        while (gatheringChoices || gatheringActions)
        {
            cProgress++;
            line = dialogueController.data[cProgress];

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

                choices.Add(line.Split('"')[1]);
                actions.Add(line.Split('"')[1], new List<string>());
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
            ChoiceScreen.Show(title, choices.ToArray());


            ///TODO: 
            ///BLOCK OFF CHOICES THAT ARE NOT SELECTED if this is the past.

            yield return new WaitForEndOfFrame();

            Debug.Log("BEGIN WAITING FOR CHOICE");

            while (ChoiceScreen.isWaitingForChoiceToBeMade)
            {
                yield return new WaitForEndOfFrame();
            }

            Debug.Log("DONE WAITING FOR CHOICE");

            List<string> actionList = actions[choices[ChoiceScreen.lastChoiceMade.index]];

            foreach (Tuple<int, string> addedData in actionList.Select(s => new Tuple<int, string>(actionList.IndexOf(s) + 1 + cProgress, s)))
            {
                if (dialogueController.data[addedData.Item1] != addedData.Item2)
                {
                    dialogueController.data.Insert(addedData.Item1, addedData.Item2);
                }
            }
        }
        else
        {
            Debug.LogError("Invalid choice operation, no choices are found");
        }

        dialogueController.chapterProgress = cProgress;
        dialogueController.chapterProgress++;
        //Debug.Log("cProgress at: " + cProgress + " chaoterProgessAt: " + dialogueController.chapterProgress);

        dialogueController._next = true;
        dialogueController.StopHandlingLine();
    }
}
