﻿using UnityEngine;
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
		if(NameManager.nameManagerRef)
		{
			if(playerRef != null)
			{
				// Si el personaje lo controla un jugador y no esta muerto, dibujaremos el nombre encima de el
				if(active && NetworkManager.networkManagerRef.listaJugadores[playerRef.id].activePlayer && !NetworkManager.networkManagerRef.listaJugadores[playerRef.id].player.isDead)
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
		}
	}
}
