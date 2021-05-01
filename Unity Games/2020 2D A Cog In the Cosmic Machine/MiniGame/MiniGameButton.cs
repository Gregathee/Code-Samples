/*
 * MiniGameButton.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/6/2020 (en-US)
 * Description: Custom button script. Works like a toggle and uses sprite swap.
 */

using UnityEngine;
using UnityEngine.UI;

public class MiniGameButton : MonoBehaviour
{
    [HideInInspector] public bool isOn = false;
	[HideInInspector] public int value = 0;
	[SerializeField] Sprite onSprite = null;
	[SerializeField] Sprite offSprite = null;
    public string[] Press;

    public void OnMouseDown() { ChangeValue(); AudioManager.instance.PlaySFX(Press[Random.Range(0, Press.Length - 1)]); }

	public void ChangeValue()
	{
		isOn = !isOn;
		if (isOn) { GetComponent<Image>().sprite = onSprite; value = 1; }
		else { GetComponent<Image>().sprite = offSprite; value = 0; }
	}
}