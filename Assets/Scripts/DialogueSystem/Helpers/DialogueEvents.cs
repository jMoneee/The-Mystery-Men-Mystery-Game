using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DialogueEvents
{
    public static void HandleEvent(string _event, ChaperLineManager.LINE.SEGMENT segment)
    {
        if (_event.Contains("("))
        {
            List<Tuple<string, string>> actions = ChaperLineManager.GetActions(_event);

            foreach (Tuple<string, string> action in actions)
            {
                CommandHandler.HandleCommand(action.Item1, action.Item2);
            }
        }

        string[] eventData = _event.Split(new char[1] { ' ' }, 2);

        string[] parameters = eventData.Length > 1 ? eventData[1].Split(',') : null;

        switch (eventData[0])
        {
            case "txtSpd":
                EVENT_TxtSpd(parameters, segment);
                break;
            case "/txtSpd":
                segment.architect.speed = 1;
                segment.architect.charactersPerFrame = 1;
                break;
        }
    }

    static void EVENT_TxtSpd(string[] parameters, ChaperLineManager.LINE.SEGMENT segment)
    {

        float delay = float.Parse(parameters[0].Trim());
        int charactersPerFrame = int.Parse(parameters[1].Trim());
        segment.architect.speed = delay;
        segment.architect.charactersPerFrame = charactersPerFrame;
    }
}
