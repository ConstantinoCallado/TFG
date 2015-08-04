﻿using UnityEngine;
using System.Collections;

public class Robot : Player 
{
	public Sight sightScript;

	public void Start()
	{
		base.playerGraphics.setRobot(GetColor());
	}

	public override void Initialize()
	{
		base.Initialize();
		gameObject.layer = LayerMask.NameToLayer("Robot");
		gameObject.tag = "Robot";
		base.playerGraphics.setRobot(GetColor());
		gameObject.name = "Robot " + getColorString();
		sightScript = gameObject.GetComponentInChildren<Sight>();

		sightScript.SetSight(2.5f, GetColor());
	}

	public override void Kill()
	{
		base.Kill();
		--GameManager.gameManager.robotsAlive;
		sightScript.EnableSight(false);
	}

	public override void Respawn()
	{
		base.Respawn();
		++GameManager.gameManager.robotsAlive;
		sightScript.EnableSight(true);
	}

	public virtual Color GetColor()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
		return Color.black;
	}

	public virtual string getColorString()
	{
		return "";
	}

	public void OnTriggerEnter2D(Collider2D other)
	{
		Debug.Log("He tocado al humano");

		if(other.gameObject.GetComponent<Human>().aggressiveMode)
		{
			GameManager.gameManager.KillPlayerServer(base.id, Human.humanRef.id);
			Kill();
		}
	}

	public override void ActivatePower()
	{
		Debug.Log("ROBOT ACTIVANDO HABILIDAD");
	}
}
