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

	//public static string ToString(this KeyCode key)
	//{
	//	if (key == KeyCode.Mouse0)
	//		return "left mouse";
	//	else if (key == KeyCode.Mouse1)
	//		return "right mouse";
	//	else if (key == KeyCode.Mouse2)
	//		return "middle mouse";
	//	else
	//		return key.ToString();
	//}

    public static void ChangeCanvasGroupVisibility(this CanvasGroup canvasGroup, bool visible)
    {
        canvasGroup.alpha = visible ? 1 : 0;
        canvasGroup.interactable = visible;
        canvasGroup.blocksRaycasts = visible;
    }

    /// <summary>
    /// This returns the component on the given object, and creates the component if it does not exist
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static T EnsureComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component == null)
        {
            component = gameObject.AddComponent<T>();
        }
        return component;
    }

    /// <summary>
    /// This returns the component on the given component, and creates the component if it does not exist
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="comp"></param>
    /// <returns></returns>
    public static T EnsureComponent<T>(this Component comp) where T : Component
    {
        T component = comp.gameObject.EnsureComponent<T>();
        return component;
    }

	/// <summary>
	/// Functions identically to TryGetComponent but instead returns an array of all components attached to the object.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="comp"></param>
	/// <param name="components">True if at least one component was found. False otherwise.</param>
	/// <returns></returns>
	public static bool TryGetComponents<T>(this Component comp, out T[] components) where T : Component
	{
		components = comp.GetComponents<T>();

		if (components.Length == 0)
			return false;

		return true;
	}
}