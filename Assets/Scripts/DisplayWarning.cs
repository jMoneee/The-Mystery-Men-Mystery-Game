using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayWarning : MonoBehaviour
{
	private List<Text> prompts;
	[SerializeField] RectTransform origin;
	[SerializeField] Font font;
	[SerializeField] int fontSize = 50;
	[SerializeField] Color fontColor = Color.white;
	[SerializeField] FontStyle fontStyle = FontStyle.Normal;
	[SerializeField] HorizontalWrapMode horizontalWrapMode = HorizontalWrapMode.Overflow;
	[SerializeField] int outlineWeight = 1;
	[SerializeField] float textLifetime = 3f;
	public int maxWarningsToShow = 4;

	// Start is called before the first frame update
	void Start()
    {
		prompts = new List<Text>();
	}

	public void Warning(string text)
	{
		GameObject g = new GameObject("warning " + prompts.Count);
		g.transform.parent = origin.transform;
		Text tex = g.AddComponent<Text>();
		tex.font = font;
		tex.fontSize = fontSize;
		tex.color = fontColor;
		tex.fontStyle = fontStyle;
		tex.horizontalOverflow = horizontalWrapMode;
		tex.rectTransform.localScale = Vector3.one;
		tex.alignment = TextAnchor.LowerCenter;
		g.AddComponent<Outline>().effectDistance = new Vector2(outlineWeight, outlineWeight);
		tex.text = text;
		prompts.Add(tex);

		StartCoroutine(TextLifetime(tex));

		CheckOriginSize();
	}

	private void CheckOriginSize()
	{
		origin.sizeDelta = new Vector2(origin.sizeDelta.x, prompts.Count * (fontSize + 5));
	}

	private IEnumerator TextLifetime(Text text)
	{
		float time = Time.time;
		yield return new WaitUntil(() => ((Time.time - time) >= textLifetime) || ((prompts.Count > maxWarningsToShow) && (text == prompts[0])));
		prompts.Remove(text);
		Destroy(text.gameObject);
		CheckOriginSize();
	}
}
