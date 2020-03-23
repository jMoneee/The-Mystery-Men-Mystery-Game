using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinigameStation : Interactable
{
	public LookMinigameHandler thisStationMinigame;
	public MinigameListing[] minigames;

	private void OnTriggerEnter(Collider other)
	{
		foreach (MinigameListing item in minigames)
		{
			if (item.triggerObject == other.gameObject)
				thisStationMinigame.StartMatchingGame(item);
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