using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HoveringName : MonoBehaviour 
{
	public bool active = false;
	public Text textUI;
	public static RectTransform canvasRect;
	static Camera mainCamera;
	Transform transformRef;
	public int index;

	public void Awake()
	{
		mainCamera = Camera.main;
		transformRef = transform;
	}

	public void LateUpdate()
	{
		if(textUI != null)
		{
			if(active)
			{
				if(NetworkManager.networkManagerRef.listaJugadores[index].activePlayer)
				{
					textUI.text = NetworkManager.networkManagerRef.listaJugadores[index].playerName;

					Vector2 ViewportPosition = mainCamera.WorldToViewportPoint(transformRef.position);

					Vector2 WorldObject_ScreenPosition = new Vector2(
						((ViewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
						((ViewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

					textUI.rectTransform.anchoredPosition = WorldObject_ScreenPosition;
				}
				else
				{
					textUI.text = "";
				}
				textUI.gameObject.SetActive(true);
			}
			else
			{
				textUI.text = "";
				textUI.gameObject.SetActive(false);
			}
		}
	}
}
