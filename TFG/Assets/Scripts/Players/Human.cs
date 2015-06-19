using UnityEngine;	
using System.Collections;

public class Human : Player 
{
	public bool aggressiveMode = false;
	float aggressiveTimeEnd = 0;
	const float aggressiveTime = 8;

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
		speed = 3f;
		base.playerGraphics.setHuman();
		gameObject.name = "Human";
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
			Destroy(other.gameObject);
		}
		// Si estamos en el servidor comprobamos la colision con los robots y las armas
		else if(Network.isServer)
		{
			if(other.tag == "Robot")
			{
				Debug.Log("He tocado un robot");
				
				if(!aggressiveMode)
				{
					if(!isDead)
					{
						base.Kill();
						
						GameManager.gameManager.KillPlayerServer(base.id);
					}
				}
			}
			else if(other.tag == "Weapon")
			{
				pickUpAggressive();
				
				//TODO: No destruir sin mas... hay que notificarlo antes a los clientes
				Destroy(other.gameObject);
			}
		}
		else
		{
			if(other.tag == "Weapon")
			{
				
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

		while(Time.time < aggressiveTimeEnd)
		{
			yield return new WaitForSeconds(0.1f);
		}

		base.playerGraphics.SetAggressive(false);

		aggressiveMode = false;
	}
}
