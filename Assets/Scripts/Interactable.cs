using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
	[SerializeField] float highlightStrength = 2.5f;
	private Material material;
	private Color ogColor;
	private ParticleSystem interactableSparkle;
	public UnityEvent interactAction;
	protected DisplayInstructions instructions;

	private void Start()
	{
		instructions = FindObjectOfType<DisplayInstructions>();
		material = GetComponent<Renderer>().material;
		ogColor = material.color;

		if (TryGetComponent(out ParticleSystem ps))
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
		s.mesh.vertices = m.vertices.times(1.1f);
		s.mesh.triangles = m.triangles;
		s.mesh.uv = m.uv;
		s.mesh.RecalculateNormals();
	}

	private void OnMouseEnter()
	{
		material.color = ogColor * highlightStrength;
		interactableSparkle?.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	private void OnMouseExit()
	{
		material.color = ogColor;
		interactableSparkle?.Play(false);
	}

	public virtual void StartHover(KeyCode key, GameObject obj)
	{

	}

	public virtual void ActiveHover(KeyCode key, GameObject obj)
	{

	}

	public void EndHover(KeyCode key)
	{
		instructions.RemovePrompt(key);
	}
}

public static class extension
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
}