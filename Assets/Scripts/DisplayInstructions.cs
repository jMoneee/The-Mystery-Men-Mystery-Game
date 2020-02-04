using System.Collections;
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
	[SerializeField] Color fontColor;
	[SerializeField] FontStyle fontStyle = FontStyle.Normal;
	[SerializeField] HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow;
	[SerializeField] Color emphasisColor;
	[SerializeField] int outlineWeight;

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
	public void SetPrompt(KeyCode key, string action, string name)
	{
		string trimmed = action.Trim() + " <color=#" + ColorToHex(emphasisColor) + "><b>" + name.Trim() + "</b></color>.";
		string display = "Press " + key.ToString() + " to " + trimmed;

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

	// Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
	string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}

	Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, 255);
	}
}
