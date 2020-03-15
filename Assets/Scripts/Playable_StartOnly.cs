using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playable_StartOnly : Playable
{
    public override void InteractBegin()
    {
        instructions.RemovePrompt(key);
        _interacting = true;
        interactAction?.Invoke();
    }

    public override void InteractContinue()
    {
        instructions.RemovePrompt(key);
    }

    public override void InteractEnd()
    {
    }
}
