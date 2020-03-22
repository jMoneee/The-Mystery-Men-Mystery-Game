using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public static class CommandHandler
{
    public static void HandleCommand(string actionName, string actionParameters)
    {
        string[] parameters = actionParameters.Split(',');

        switch (actionName)
        {
            case "playSound":
                Command_PlaySound(parameters);
                break;
            case "showScene":
                Command_ShowScene(parameters);
                break;
            case "Load":
                Command_Load(parameters);
                break;
            case "AddToJournal":
                Command_AddJournal(parameters);
                break;
            case "SetVariable":
                Command_SetVariable(parameters);
                break;
            default:
                Debug.LogWarning("COMMAND " + actionName + " NOT FOUND. YOU ARE USING A COMMAND THAT DOES NOT EXIST (YET) OR HAVE A SPELLING ERROR");
                break;
        }
    }

    private static void Command_SetVariable(string[] parameters)
    {
        string var = parameters[0].Trim();
        string text = parameters[1].Trim();
        if (DialogueController.instance.savedVariables.ContainsKey(var) == false)
        {
            DialogueController.instance.savedVariables.Add(var, text);
        }
        else
        {
            DialogueController.instance.savedVariables[var] = text;
        }
    }

    private static void Command_AddJournal(string[] parameters)
    {
        string text = parameters[0].Trim();
        VariableManager.Inject(ref text);
        JournalManager2.AddTextToJournal(text);
    }

    private static void Command_Load(string[] parameters)
    {
        string chapterName = parameters[0].Trim();
        //NovelController.instance.chapterProgress = 0;
        DialogueController.instance.StartNewText(chapterName);
    }

    private static void Command_ShowScene(string[] parameters)
    {
        //bool show = bool.Parse(parameters[0].Trim());
        //string transitionName = parameters[1].Trim();
        //Texture2D transition = Resources.Load(NovelController.TransitionFilePath + transitionName) as Texture2D;

        //float speed = parameters.Length > 2 ? float.Parse(parameters[2].Trim()) : 2f;
        //bool smooth = parameters.Length > 3 ? bool.Parse(parameters[3].Trim()) : false;

        //TransitionMaster.ShowScene(show, transition, speed, smooth);
    }

    private static void Command_PlaySound(string[] parameters)
    {
        //string soundEffectName = parameters[0].Trim();
        //AudioClip clip = soundEffectName == "null" ? null : Resources.Load(NovelController.SFXFilePath + soundEffectName) as AudioClip;

        //float volume = parameters.Length > 2 ? float.Parse(parameters[2].Trim()) : 1f;
        //float pitch = parameters.Length > 3 ? float.Parse(parameters[3].Trim()) : 1f;
        //bool loop = parameters.Length > 4 ? bool.Parse(parameters[4].Trim()) : false;

        //AudioManager.instance.PlaySFX(clip, volume, pitch, loop);
    }
}
