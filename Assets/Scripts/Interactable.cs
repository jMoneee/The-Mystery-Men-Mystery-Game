using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public abstract class Interactable : MonoBehaviour
{
	[SerializeField] float highlightStrength = 2.5f;
	private Material material;
	private Color ogColor;
	private ParticleSystem interactableSparkle;
	protected DisplayInstructions instructions;
	[SerializeField] protected string startVerb = "interact with";
	[SerializeField] protected string endVerb = "stop interacting with";
	[SerializeField] protected KeyCode _key;
	public KeyCode key { get { return _key; } }
	protected bool _interacting = false;
	public bool interacting { get { return _interacting; } }
	public UnityEvent interactAction;

	protected virtual void Start()
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
		material.color = ogColor * highlightStrength;
		interactableSparkle?.Stop(false, ParticleSystemStopBehavior.StopEmittingAndClear);
	}

	/// <summary>
	/// Activates on cursor exit regardless of distance. Called by Unity Events.
	/// </summary>
	private void OnMouseExit()
	{
		material.color = ogColor;
		interactableSparkle?.Play(false);
	}

	/// <summary>
	/// Called by Interactor.cs on the first frame that it hovers over the object <b>while in range of it</b>.
	/// </summary>
	/// <param name="key"></param>
	/// <param name="obj"></param>
	public abstract void StartHover();

	public abstract void DuringHover();

	public abstract void EndHover();

	public abstract void StartInteract();

	public abstract void DuringInteract();

	public abstract void EndInteract();
}