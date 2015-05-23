using UnityEngine;
using System.Collections.Generic;

public class AvatarManager : MonoBehaviour 
{
	public static AvatarManager avatarManager;

	public List<Sprite> listaTexturasAvatares = new List<Sprite>();

	void Awake()
	{
		avatarManager = this;
	}

	public Sprite getAvatar(EnumPersonaje enumPersonaje)
	{
		return listaTexturasAvatares[(int)(enumPersonaje)-1];
	}
}
