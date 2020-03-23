using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class MinigameStation : Interactable
{
	public LookMinigameHandler thisStationMinigame;
	public MinigameListing[] minigames;
    Dictionary<MinigameListing, bool> minigamesCompleted = new Dictionary<MinigameListing, bool>();
    public bool completelyDoneWithMinigames { get { return minigamesCompleted.Values.Where(s => s == false).Count() == 0; } }

    private void Start()
    {
        foreach (MinigameListing minigame in minigames)
        {
            minigamesCompleted.Add(minigame, false);
        }
    }

    private void OnTriggerStay(Collider other)
	{
        foreach (MinigameListing item in minigames)
        {
            if (item.triggerObject == other.gameObject && minigamesCompleted[item] == false)
            {
                gameObject.layer = 2;


                if (other.GetComponent<Pickupable>().interacting == false)
                {
                    StartCoroutine(StartGame(item));
                    minigamesCompleted[item] = true;
                }
                else
                {
                    other.GetComponent<Pickupable>().endVerb = "start fingerprinting";
                }
            }
        }
	}

    private IEnumerator StartGame(MinigameListing item)
    {
        yield return new WaitForSeconds(0.1f);
        thisStationMinigame.StartMatchingGame(item);
        instructions.RemovePrompt(KeyCode.Numlock);
        item.triggerObject.GetComponent<Pickupable>().endVerb = "drop";
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (MinigameListing item in minigames)
        {
            if (item.triggerObject == other.gameObject)
            {
                gameObject.layer = 1;
                instructions.RemovePrompt(KeyCode.Numlock);
            }
        }
    }

    public override void HoverBegin()
	{
		instructions.SetPromptNoKey(KeyCode.Numlock, "Place evidence here to " + TextEffects.EmphasizeWord(startVerb, Color.red) + ".");
	}

	public override void HoverContinue()
	{
		
	}

	public override void HoverEnd()
	{
		instructions.RemovePrompt(KeyCode.Numlock);
	}

	public override void InteractBegin()
	{
		
	}

	public override void InteractContinue()
	{
		
	}

	public override void InteractEnd()
	{
		
	}
}

[System.Serializable]
public class MinigameListing
{
	public GameObject triggerObject;
	public TextAsset correct;
	public TextAsset incorrect;
	public Sprite objectMatch;
	public int correctIndex;
	public bool success = false;
}