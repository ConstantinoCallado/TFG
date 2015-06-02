using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoveringName : MonoBehaviour 
{
	public bool active = false;
	public static RectTransform canvasRect;
	static Camera mainCamera;
	Transform transformRef;
	public Player playerRef;

	public void Awake()
	{
		mainCamera = Camera.main;
		transformRef = transform;
	}

	public void LateUpdate()
	{
		if(playerRef != null)
		{
			if(NetworkManager.networkManagerRef.listaJugadores[playerRef.id].activePlayer)
			{
				Vector2 ViewportPosition = mainCamera.WorldToViewportPoint(transformRef.position);
				Vector2 WorldObject_ScreenPosition = new Vector2(
					((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
					((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

				NameManager.nameManagerRef.listaTextos[playerRef.id].rectTransform.anchoredPosition = WorldObject_ScreenPosition;
				NameManager.nameManagerRef.listaTextos[playerRef.id].text = NetworkManager.networkManagerRef.listaJugadores[playerRef.id].playerName;
			}
			else
			{
				NameManager.nameManagerRef.listaTextos[playerRef.id].text = "";
			}
			NameManager.nameManagerRef.listaTextos[playerRef.id].gameObject.SetActive(true);
		}
		else
		{
			NameManager.nameManagerRef.listaTextos[playerRef.id].text = "";
			NameManager.nameManagerRef.listaTextos[playerRef.id].gameObject.SetActive(false);
		}
	}
}
