using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PathBuilderSlot : MonoBehaviour
{
    public bool towerOverSlot;
	public TowerState State;
	[SerializeField]RawImage towerImage;

	private void Update()
	{
		if(Input.GetMouseButtonDown(0) && towerOverSlot && State)
		{
			PathBuilder.pathBuilder.UnLoadTower(State);
			towerImage.color = new Color(1, 1, 1, 0);
			State = null;
		}
	}

	public void ResetSlot()
	{
		towerImage.color = new Color(1, 1, 1, 0);
		State = null;
	}

	public void Initialize(TowerState state, Texture texture )
	{
		State = state;
		towerImage.texture = texture;
		towerImage.color = new Color(1, 1, 1, 1);
	}

	public void EnterSlot() { towerOverSlot = true; }
    public void ExitSlot() { towerOverSlot = false; }
}
