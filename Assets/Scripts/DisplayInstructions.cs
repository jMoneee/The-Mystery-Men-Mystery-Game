using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayInstructions : MonoBehaviour
{
	private Dictionary<KeyCode, Text> prompts = new Dictionary<KeyCode, Text>();
	[SerializeField] RectTransform origin;
	[SerializeField] Font font;
	[SerializeField] int fontSize = 50;
	[SerializeField] Color fontColor = Color.white;
	[SerializeField] FontStyle fontStyle = FontStyle.Normal;
	[SerializeField] HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow;
	[SerializeField] Color emphasisColor = Color.red;
	[SerializeField] int outlineWeight = 1;
	private Dictionary<KeyCode, string> keyNames = new Dictionary<KeyCode, string>();

    void Start()
    {
		foreach (KeyCode k in Enum.GetValues(typeof(KeyCode)))
			if (!keyNames.ContainsKey(k))
				keyNames.Add(k, k.ToString());
		keyNames[KeyCode.Mouse0] = "Left Mouse";
		keyNames[KeyCode.Mouse1] = "Right Mouse";
		keyNames[KeyCode.Mouse2] = "Middle Mouse";

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
		string display = "Press " + keyNames[key] + " to " + trimmed + ".";

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

		origin.sizeDelta = new Vector2(origin.sizeDelta.x, prompts.Count * (fontSize + 10));
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
