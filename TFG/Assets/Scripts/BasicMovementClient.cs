using UnityEngine;
using System.Collections;

public class BasicMovementClient : MonoBehaviour 
{
	public Transform transformRef;

	private Vector3 posicionVieja = Vector3.zero;
	protected Vector2 diferenciasPosiciones;
	
	const int mascaraD = 65535;
	const uint mascaraI = 4294901760;


	// Use this for initialization
	public void Awake () 
	{
		transformRef = transform;
	}
	
	void OnSerializeNetworkView (BitStream stream, NetworkMessageInfo info) 
	{
		// Sending
		if (stream.isReading) 
		{
			//DesSerialize2Float(stream, transform);
			DesSerialize1Int(stream, transformRef);
		
			posicionVieja = transformRef.position;	
		} 
	}
	
	void DesSerialize2Float(BitStream stream, Transform transformP)
	{
		float floatPositionX = 0;
		float floatPositionY = 0;
		
		stream.Serialize (ref floatPositionX);
		stream.Serialize (ref floatPositionY);
		
		RecievedNewPosition(new Vector3(floatPositionX, floatPositionY, 0));
	}
	
	void DesSerialize1Int(BitStream stream, Transform transformP)
	{
		int packagePosition = 0;
		stream.Serialize (ref packagePosition);

		RecievedNewPosition(new Vector3(((packagePosition & mascaraI) >> 16) / 100.0f, (packagePosition & mascaraD) / 100.0f, 0));
	}

	public virtual void RecievedNewPosition(Vector3 positionRecieved)
	{
		transformRef.position = positionRecieved;
		diferenciasPosiciones = transformRef.position - posicionVieja;
		if(diferenciasPosiciones.x > 0)
		{
			transformRef.eulerAngles = new Vector3(0, 0, 180);
		}
		else if(diferenciasPosiciones.x < 0)
		{
			transformRef.eulerAngles = Vector3.zero;
		}
		else if(diferenciasPosiciones.y > 0)
		{
			transformRef.eulerAngles = new Vector3(0, 0, -90);
		}
		else if(diferenciasPosiciones.y < 0)
		{
			transformRef.eulerAngles = new Vector3(0, 0, 90);
		}
	}
}
