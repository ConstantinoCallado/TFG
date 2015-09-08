using UnityEngine;	
using System.Collections;

public class Human : Player 
{
	public bool aggressiveMode = false;
	float aggressiveTimeEnd = 0;
	const float aggressiveTime = 8;
	//const float aggressiveTime = 20;
	public SightableHuman sightable;
	
	public static Human humanRef;

	public void Awake()
	{
		base.Awake();
		humanRef = this;
	}

	public override void Initialize()
	{
		base.Initialize();
		gameObject.layer = LayerMask.NameToLayer("Human");
		gameObject.tag = "Human";
		speed = 3.85f;
		base.playerGraphics.setHuman();
		gameObject.name = "Human";
		sightable = gameObject.GetComponentInChildren<SightableHuman>();
		sightable.humanRef = this;

		//Debug.Log("inicializado Humano");
	}


	public void OnTriggerEnter2D(Collider2D other)
	{
		// Si estamos en el servidor comprobamos la colision con los robots y las armas
		if(Network.isServer)
		{
			if(other.tag == "Weapon")
			{
				pickUpAggressive();
				Destroy(other.gameObject);
			}
		}
		else
		{
			if(other.tag == "Weapon")
			{
				Destroy(other.gameObject);
			}
		}
	}

	public void pickUpAggressive()
	{
		aggr();
		base.networkView.RPC("aggr", RPCMode.Others);
	}

	// Funcion que envia el servidor a los clientes para notificar que el humano pasa a estado agresivo 
	[RPC]
	void aggr()
	{
		aggressiveTimeEnd = Time.time + aggressiveTime;
		base.playerGraphics.SetAggressive(true);

		aggressiveMode = true;
		StartCoroutine(coroutineAggressive());
	}

	IEnumerator coroutineAggressive()
	{
		while(Time.time < aggressiveTimeEnd)
		{
			//Debug.Log("COMPROBANDO");
			if(Time.time < aggressiveTimeEnd - 3)
			{
				yield return new WaitForSeconds(0.1f);
			}
			else if(Time.time < aggressiveTimeEnd - 1.5)
			{
				base.playerGraphics.SetAggressive(false);
				yield return new WaitForSeconds(0.2f);
				base.playerGraphics.SetAggressive(true);
				yield return new WaitForSeconds(0.2f);
			}
			else
			{
				base.playerGraphics.SetAggressive(false);
				yield return new WaitForSeconds(0.1f);
				base.playerGraphics.SetAggressive(true);
				yield return new WaitForSeconds(0.1f);
			}		
		}

		base.playerGraphics.SetAggressive(false);
		
		aggressiveMode = false;
	}
	
	public override float GetCoolDownTime()
	{
		return 40;
	}

	public override void ActivatePower()
	{
		Debug.Log("HUMANO ACTIVANDO HABILIDAD");
	}
}
