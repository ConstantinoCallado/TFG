using UnityEngine;
using System.Collections;

public class Robot : Player 
{
	const float speed = 3.5f;

	public override void Initialize()
	{
		base.Initialize();
		base.basicMovementRef.speed = speed;
		gameObject.layer = LayerMask.NameToLayer("Robot");
		base.playerGraphics.setRobot(GetColor());
		Debug.Log("inicializado Robot con color " + GetColor());
	}

	public virtual void ActivatePower()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
	}

	public override void Kill()
	{
		Debug.Log("Matando robot");
	}

	public virtual Color GetColor()
	{
		Debug.Log("La clase hija deberia sobreescribir este metodo");
		return Color.black;
	}
}
