using UnityEngine;
using System.Collections;

public class PlayerLocal : MonoBehaviour 
{
	void Awake()
	{
		if(!Network.isClient)
		{
			DestroyImmediate(this);
		}
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{


	}
}
