using UnityEngine;
using System.Collections;

[RequireComponent (typeof (BasicMovement))]
public class Player : MonoBehaviour 
{
	public BasicMovement basicMovementRef;
	public PlayerGraphics playerGraphics;

	public string playerName;
	public string characterName;

	public void Awake()
	{
		basicMovementRef = GetComponent<BasicMovement>();
		playerGraphics = GetComponent<PlayerGraphics>();
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
