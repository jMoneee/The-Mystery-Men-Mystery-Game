﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DisplayInstructions : MonoBehaviour
{
	private Dictionary<KeyCode, Text> prompts;
	[SerializeField] RectTransform origin;
	[SerializeField] Font font;
	[SerializeField] int fontSize = 50;
	[SerializeField] Color fontColor = Color.white;
	[SerializeField] FontStyle fontStyle = FontStyle.Normal;
	[SerializeField] HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow;
	[SerializeField] Color emphasisColor = Color.red;
	[SerializeField] int outlineWeight = 1;

    void Start()
    {
		prompts = new Dictionary<KeyCode, Text>();
    }

    void Update()
    {
        
    }

	/// <summary>
	/// Display a message to the player about what to do with an interactable.
	/// The format goes "Press [key] to [text]." 
	/// It is recommended to include a name for the item in the text.
	/// <para></para>
	/// WARNING: Will replace existing prompts if called for the same KeyCode!
	/// </summary>
	/// <param name="key">Input key required to interact.</param>
	/// <param name="text">The display message.</param>
	public void SetPrompt(KeyCode key, string action)
	{
		string trimmed = action.Trim();
		string display = "Press " + key.ToString() + " to " + trimmed + ".";

		if (!prompts.ContainsKey(key))
		{
			GameObject g = new GameObject(key + " prompt");
			g.transform.parent = origin.transform;
			Text tex = g.AddComponent<Text>();
			tex.font = font;
			tex.fontSize = fontSize;
			tex.color = fontColor;
			tex.fontStyle = fontStyle;
			tex.horizontalOverflow = horizontalWrapMode;
			tex.rectTransform.localScale = Vector3.one;
			g.AddComponent<Outline>().effectDistance = new Vector2(outlineWeight, outlineWeight);
			tex.text = display;

			//tex.GetGenerationSettings()
			prompts.Add(key, tex);
		}
		else
		{
			prompts[key].text = display;
		}
	}

	/// <summary>
	/// Remove the prompt from display.
	/// </summary>
	/// <param name="key">Input key prompt to remove.</param>
	public void RemovePrompt(KeyCode key)
	{
		if (prompts.ContainsKey(key))
		{
			Destroy(prompts[key].gameObject);
			prompts.Remove(key);
		}
	}

	private void UpdatePositions()
	{

	}
}
