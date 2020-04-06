using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taggable : Interactable
{
	public GameObject marker;
	public AudioClip placementSound;
	public static int markerCount = 0;
	public Material[] markerMaterials;

	private void Start()
	{
		_key = KeyCode.X;
	}

	public override void HoverBegin()
	{
		instructions.SetPrompt(key, startVerb + " " + TextEffects.EmphasizeWord(name, Color.red));
	}

	public override void HoverContinue()
	{

	}

	public override void HoverEnd()
	{
		instructions.RemovePrompt(key);
	}

	public override void InteractBegin()
    {
        instructions.RemovePrompt(key);
        _interacting = true;
		interactAction?.Invoke();
		if (marker)
		{
			marker.SetActive(true);
			gameObject.AddComponent<AudioSource>().clip = placementSound;
			GetComponent<AudioSource>().Play();
			marker.GetComponent<Renderer>().material = markerMaterials[markerCount];
			markerCount++;
		}
    }

    public override void InteractContinue()
    {
        instructions.RemovePrompt(key);
    }

    public override void InteractEnd()
    {

    }

	private void OnValidate()
	{
		Start();
	}
}
