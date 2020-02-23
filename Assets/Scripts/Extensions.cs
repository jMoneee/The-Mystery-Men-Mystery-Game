using UnityEngine;

public static class Extensions
{
	public static Vector3[] times(this Vector3[] v, float f)
	{
		Vector3[] v2 = new Vector3[v.Length];
		for (int i = 0; i < v.Length; i++)
		{
			v2[i] = v[i] * f;
		}

		return v2;
	}

	public static string ToString(this KeyCode key)
	{
		if (key == KeyCode.Mouse0)
			return "left mouse";
		else if (key == KeyCode.Mouse1)
			return "right mouse";
		else if (key == KeyCode.Mouse2)
			return "middle mouse";
		else
			return key.ToString();
	}
}