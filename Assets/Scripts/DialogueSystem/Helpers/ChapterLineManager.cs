using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChaperLineManager : MonoBehaviour
{
    public static LINE Interpret(string rawLine)
    {
        return new LINE(rawLine);
    }

    public class LINE
    {
        public string rawLine = "";

        public string speaker = "";

        public List<SEGMENT> segments = new List<SEGMENT>();
        public List<Tuple<string, string>> actions;

        public string lastSegmentsWholeDialogue = "";

        public LINE(string rawLine)
        {
            this.rawLine = rawLine;

            string[] dialogueAndActions = rawLine.Split('"');

            if (dialogueAndActions.Length == 3)
            {
                speaker = dialogueAndActions[0] == "" ? DialogueController.instance.cachedLastSpeaker : dialogueAndActions[0].Trim();

                DialogueController.instance.cachedLastSpeaker = speaker;

                SegmentDialogue(dialogueAndActions[1]);
            }

            actions = GetActions(dialogueAndActions.Length == 3 ? dialogueAndActions[2] : dialogueAndActions[0]);
        }

        void SegmentDialogue(string dialogue)
        {
            segments.Clear();
            string[] parts = dialogue.Split('{', '}');

            for (int i = 0; i < parts.Length; i++)
            {
                SEGMENT segment = new SEGMENT();
                bool isOdd = i % 2 != 0;

                if (isOdd)
                {
                    string[] commandData = parts[i].Split(' ');
                    switch (commandData[0])
                    {
                        case "c":
                            segment.trigger = SEGMENT.TRIGGER.waitClickClear;
                            break;
                        case "a":
                            segment.trigger = SEGMENT.TRIGGER.waitClick;
                            segment.pretext = segments.Count > 0 ? segments[segments.Count - 1].dialogue : "";
                            break;
                        case "w":
                            segment.trigger = SEGMENT.TRIGGER.autoDelay;
                            segment.autoDelay = float.Parse(commandData[1]);
                            break;
                        case "wa":
                            segment.trigger = SEGMENT.TRIGGER.autoDelay;
                            segment.autoDelay = float.Parse(commandData[1]);
                            segment.pretext = segments.Count > 0 ? segments[segments.Count - 1].dialogue : "";
                            break;
                    }
                    i++;
                }
                segment.dialogue = parts[i];
                segment.line = this;

                segments.Add(segment);
            }
        }

        public class SEGMENT
        {
            public LINE line;
            public string dialogue = "";
            public string pretext = "";
            public enum TRIGGER { waitClick, waitClickClear, autoDelay };
            public TRIGGER trigger = TRIGGER.waitClickClear;

            public float autoDelay = 0;

            public void Run()
            {
                if (isRunning)
                {
                    DialogueController.instance.StopCoroutine(running);
                }
                running = DialogueController.instance.StartCoroutine(Running());
            }

            public bool isRunning { get { return running != null; } }
            Coroutine running = null;
            public TextArchitect architect = null;

            List<string> allCurrentlyExecutedEvents = new List<string>();

            IEnumerator Running()
            {
                //VariableManager.Inject(ref dialogue);

                string[] parts = dialogue.Split('[', ']');
                for (int i = 0; i < parts.Length; i++)
                {
                    bool isOdd = i % 2 != 0;
                    if (isOdd)
                    {
                        DialogueEvents.HandleEvent(parts[i], this);
                        allCurrentlyExecutedEvents.Add(parts[i]);
                        i++;
                    }

                    string targDialogue = parts[i];

                    VariableManager.Inject(ref targDialogue);


                    //Debug.Log("SAY THIS-" + targDialogue);
                    DialogueSystem.instance.Say(targDialogue, line.speaker, i > 0 ? true : pretext != "");
                    

                    architect = DialogueSystem.instance.TextArchitect;
                    while (architect.isConstructing)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                running = null;
            }

            public void ForceFinish()
            {
                if (isRunning)
                {
                    DialogueController.instance.StopCoroutine(running);
                }
                running = null;

                if (architect != null)
                {
                    architect.ForceFinish();

                    if (pretext == "")
                    {
                        line.lastSegmentsWholeDialogue = "";
                    }

                    string[] parts = dialogue.Split('[', ']');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        bool isOdd = i % 2 != 0;
                        if (isOdd)
                        {
                            string actions = parts[i];
                            if (allCurrentlyExecutedEvents.Contains(actions))
                            {
                                allCurrentlyExecutedEvents.Remove(actions);
                            }
                            else
                            {
                                DialogueEvents.HandleEvent(actions, this);
                            }
                            i++;
                        }
                        line.lastSegmentsWholeDialogue += parts[i];
                    }

                    architect.ShowText(line.lastSegmentsWholeDialogue);
                }
            }
        }
    }

    /// <summary>
    /// Get actions as a list of [ActionName, actionParameters]
    /// </summary>
    /// <param name="actions"></param>
    /// <returns></returns>
    public static List<Tuple<string, string>> GetActions(string actions)
    {
        List<string> ActionNameAndParameterCouples = actions.Split('(', ')').ToList();

        List<Tuple<string, string>> finalActions = new List<Tuple<string, string>>();

        for (int i = 0; i < ActionNameAndParameterCouples.Count - 1; i += 2)
        {
            ActionNameAndParameterCouples[i] = ActionNameAndParameterCouples[i].Trim();

            if (ActionNameAndParameterCouples[i] == string.Empty)
            {
                ActionNameAndParameterCouples.RemoveAt(i);
            }
            else
            {
                finalActions.Add(new Tuple<string, string>(ActionNameAndParameterCouples[i], ActionNameAndParameterCouples[i + 1]));
            }
        }

        return finalActions;
    }
}
