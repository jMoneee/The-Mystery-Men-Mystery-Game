using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractableDisplay : MonoBehaviour
{
	private Material[] materials;
	private Color[] ogColors;
	private ParticleSystem interactableSparkle;
	private float highlightStrength = 1.5f;
	public bool sparkle = true;

	// Start is called before the first frame update
	void Start()
    {
		materials = GetComponent<Renderer>().materials;
		ogColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++)
		{
			ogColors[i] = materials[i].color;
		}

		if (GetComponentInChildren<ParticleSystem>() || !sparkle)
			return;

		interactableSparkle = Instantiate(Resources.Load("Prefabs/Interactable Sparkle") as GameObject, transform.position, Quaternion.identity, transform).GetComponent<ParticleSystem>();
		//why the fuck does this allow me to assign to .mesh with var but not 
		ParticleSystem.ShapeModule s = interactableSparkle.shape;
		//s.shapeType = ParticleSystemShapeType.MeshRenderer;
		//s.meshRenderer = GetComponent<MeshRenderer>();
		//interactableSparkle.transform.localScale *= 1.1f;
		s.shapeType = ParticleSystemShapeType.Mesh;
		s.mesh = new Mesh();
		Mesh m = GetComponent<MeshFilter>().sharedMesh;
		//TODO: make this scale by the same amount in world space
		s.mesh.vertices = m.vertices.times(1.1f).Select(x => x = Vector3.Scale(x, transform.localScale)).ToArray();
		s.mesh.triangles = m.triangles;
		s.mesh.uv = m.uv;
		s.mesh.RecalculateNormals();
	}

	/// <summary>
	/// Activates on cursor hover regardless of distance. Called by Unity Events.
	/// </summary>
	private void OnMouseEnter()
	{
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].color = ogColors[i] * highlightStrength;
		}

		interactableSparkle?.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	/// <summary>
	/// Activates on cursor exit regardless of distance. Called by Unity Events.
	/// </summary>
	private void OnMouseExit()
	{
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].color = ogColors[i];
		}
		interactableSparkle?.Play(false);
	}
}
