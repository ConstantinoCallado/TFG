using UnityEngine;
using System.Collections;

public class PurpleRobot : Robot
{
	Color colorRobot = new Color(0.91f, 0.19f, 1f);
	const float skillDuration = 6;

	public override void Initialize()
	{
		base.Initialize();

		Debug.Log("Inicializando robot morado");
	}

	public override void ActivatePower()
	{
		GameObject barreraInstanciada = (GameObject)GameObject.Instantiate(playerGraphics.robotGraphics.prefabBarrier);
		barreraInstanciada.GetComponent<Barrier>().TurnOn(skillDuration, new Vector3((int)transform.position.x, (int)transform.position.y, 0));
	}

	public override Color GetColor()
	{
		return colorRobot;
	}
	
	public override string getColorString()
	{
		return "purple";
	}
}
