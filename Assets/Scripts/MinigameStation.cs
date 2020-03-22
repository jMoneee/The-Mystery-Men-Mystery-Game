using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MinigameStation : MonoBehaviour
{
	public LookMinigameHandler thisStationMinigame;
	public MinigameListing[] minigames;

    void Start()
    {
        
    }

	private void OnTriggerEnter(Collider other)
	{
		foreach (MinigameListing item in minigames)
		{
			if (item.triggerObject == other.gameObject)
				thisStationMinigame.StartMatchingGame(item);
		}
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