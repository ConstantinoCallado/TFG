using UnityEngine;	
using System.Collections;

public class Human : Player 
{
	public bool aggressiveMode = false;
	float aggressiveTimeEnd = 0;
	const float aggressiveTime = 8;
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
		speed = 4.25f;
		base.playerGraphics.setHuman();
		gameObject.name = "Human";
		sightable = gameObject.GetComponentInChildren<SightableHuman>();
		sightable.humanRef = this;

		Debug.Log("inicializado Humano");
	}


	public void OnTriggerEnter2D(Collider2D other)
	{
		// Comprobamos si ha cogido una pieza en todos los juegos
		if(other.tag == "Piece")
		{
			//TODO: No destruir sin mas... hay que notificarlo antes a los clientes
			//TODO: Hacer un objeto que cada 5 segundos envie de Servidor a cliente el estado de todas las bolitas
				// SI hay 32 bolitas o menos se pueden serializar en un solo entero de 32 bits
			GameManager.gameManager.RecogerPieza();
			Destroy(other.gameObject);
		}
		// Si estamos en el servidor comprobamos la colision con los robots y las armas
		else if(Network.isServer)
		{
			if(other.tag == "Weapon")
			{
				pickUpAggressive();
				//GameManager.gameManager.RecogerPieza();

				//TODO: No destruir sin mas... hay que notificarlo antes a los clientes
				Destroy(other.gameObject);
			}
		}
		else
		{
			if(other.tag == "Weapon")
			{
				//GameManager.gameManager.RecogerPieza();

				//TODO: No destruir sin mas... hay que notificarlo antes a los clientes
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
		aggressiveMode = true;
		aggressiveTimeEnd = Time.time + aggressiveTime;
		StartCoroutine(coroutineAggressive());
	}

	IEnumerator coroutineAggressive()
	{
		base.playerGraphics.SetAggressive(true);

		while(Time.time < aggressiveTimeEnd - 3)
		{
			yield return new WaitForSeconds(0.1f);
		}
		while(Time.time < aggressiveTimeEnd - 1.5)
		{
			base.playerGraphics.SetAggressive(false);
			yield return new WaitForSeconds(0.2f);
			base.playerGraphics.SetAggressive(true);
			yield return new WaitForSeconds(0.2f);
		}
		while(Time.time < aggressiveTimeEnd)
		{
			base.playerGraphics.SetAggressive(false);
			yield return new WaitForSeconds(0.1f);
			base.playerGraphics.SetAggressive(true);
			yield return new WaitForSeconds(0.1f);
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
