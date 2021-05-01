/*
 * CodeBlock.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/22/2020 (en-US)
 * Description: Used to display and input random code characters for security mini game
 */

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CodeBlock : MonoBehaviour
{
	public TMP_Text codeText;
	SecurityMiniGame miniGameManager;
	[SerializeField] Sprite unselectedSprite;
	[SerializeField] Sprite selectedSprite;

	private void Start()
	{
		miniGameManager = FindObjectOfType<SecurityMiniGame>();
		GetComponent<Image>().sprite = unselectedSprite;
	}

	public void RestetInput() { GetComponent<Image>().sprite = unselectedSprite; }

	public void InputCode()
	{
		miniGameManager.InputCode(codeText.text);
		GetComponent<Image>().sprite = selectedSprite;
	}
}