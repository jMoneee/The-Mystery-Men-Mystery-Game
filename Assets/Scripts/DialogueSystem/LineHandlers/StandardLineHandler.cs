using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardLineHandler : IHandleLine
{
    public DialogueController dialogueController { get { return DialogueController.instance; } }

    public IEnumerator HandleLine(string rawLine)
    {
        ChaperLineManager.LINE line = ChaperLineManager.Interpret(rawLine);

        dialogueController._next = false;
        int lineProgress = 0;

        while (lineProgress < line.segments.Count)
        {
            dialogueController._next = false;
            ChaperLineManager.LINE.SEGMENT segment = line.segments[lineProgress];

            if (lineProgress != 0)
            {
                if (segment.trigger == ChaperLineManager.LINE.SEGMENT.TRIGGER.autoDelay)
                {
                    for (float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                        if (dialogueController._next)
                        {
                            break;
                        }
                    }
                }
                else
                {
                    while (!dialogueController._next)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            dialogueController._next = false;

            //the segment now needs to build and run
            segment.Run();

            while (segment.isRunning)
            {
                yield return new WaitForEndOfFrame();

                if (dialogueController._next)
                {
                    if (!segment.architect.skip)
                    {
                        segment.architect.skip = true;
                    }
                    else
                    {
                        segment.ForceFinish();
                    }

                    dialogueController._next = false;
                }
            }

            lineProgress++;
        }

        for (int i = 0; i < line.actions.Count; i++)
        {
            CommandHandler.HandleCommand(line.actions[i].Item1, line.actions[i].Item2);
        }

        //This is to give actions (particularly LoadScene) time to execute before saving their states
        yield return new WaitForEndOfFrame();

        dialogueController.chapterProgress++;

        //Debug.Log("INCREMENT CHAPTER PROGRESS AT: " + 430);

        dialogueController.StopHandlingLine();
    }

}
