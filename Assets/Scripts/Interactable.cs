using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
	[SerializeField] bool _pickUp = false;
	[SerializeField] float highlightStrength = 2.5f;
	public bool pickUp { get { return _pickUp; } }
	private Material material;
	private Color ogColor;
	private ParticleSystem interactableSparkle;

	private void Start()
	{
		material = GetComponent<Renderer>().material;
		ogColor = material.color;
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
		interactableSparkle.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	private void OnMouseExit()
	{
		material.color = ogColor;
		interactableSparkle.Play(false);
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