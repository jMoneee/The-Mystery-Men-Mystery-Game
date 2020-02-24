using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class TextEffects
{
	private static Tuple<string, string> StyleFormatting(FontStyle fs)
	{
		switch (fs)
		{
			case FontStyle.Normal:
				return new Tuple<string, string>("", "");
			case FontStyle.Bold:
				return new Tuple<string, string>("<b>", "</b>");
			case FontStyle.Italic:
				return new Tuple<string, string>("<i>", "</i>");
			case FontStyle.BoldAndItalic:
				return new Tuple<string, string>("<b><i>", "</i></b>");
			default:
				return null;
		}
	}
	public static string EmphasizeWord(string text, Color color, FontStyle style = FontStyle.Bold)
	{
		Tuple<string, string> styleStart = StyleFormatting(style);
		//" <color=#" + ColorToHex(emphasisColor) + "><b>" + name.Trim() + "</b></color>.";
		return "<color=#" + ColorToHex(color) + ">" + styleStart.Item1 
			+ text.Trim() 
			+ styleStart.Item2 + "</color>";
	}

	// Note that Color32 and Color implictly convert to each other. You may pass a Color object to this method without first casting it.
	static string ColorToHex(Color32 color)
	{
		string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
		return hex;
	}

	static Color HexToColor(string hex)
	{
		byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
		byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
		byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
		return new Color32(r, g, b, 255);
	}
}
