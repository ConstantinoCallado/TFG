using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BasicMovement))]
public class Player : MonoBehaviour 
{
	public BasicMovement basicMovementRef;

	public void Awake()
	{
		basicMovementRef = GetComponent<BasicMovement>();
	}

	public void Start()
	{
		Initialize();
	}

	public virtual void Initialize()
	{
		Debug.Log("inicializado Player");
	}

	public virtual void Kill()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
	}
}
