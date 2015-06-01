using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PingScript : MonoBehaviour 
{
	public Text pingText;
	public int ping;
	// Use this for initialization
	void Start () 
	{
		if(Network.isClient)
		{
			pingText.gameObject.SetActive(true);
			StartCoroutine(coroutinePing());
		}
	}

	IEnumerator coroutinePing()
	{
		while(true)
		{
			yield return new WaitForSeconds(2);
			ping = Network.GetAveragePing(NetworkManager.networkManagerRef.networkPlayerServer);

			if(ping != -1)
			{
				pingText.text = ping + "ms";
			}
		}
	}
}
