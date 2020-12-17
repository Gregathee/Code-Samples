/*
 * Cannon.cs
 * Author(s): #Greg Brandt#
 * Created on: 10/20/2020 (en-US)
 * Description: Point astroid mini game cannons at mouse position and fire with left and right click
 */

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Cannon : MonoBehaviour
{
	[Tooltip("Indicate whether this cannon is on the left.")]
	[SerializeField] bool left = false;
	[Tooltip("Object that is instatiated when cannon is fired.")]
	[SerializeField] GameObject projectilePrefab = null;
	[Tooltip("Transform who's position is used as the point at which projectile is instantiated.")]
	[SerializeField] Transform barrel;
	[Tooltip("Time at which cannon cannot fire after firing.")]
	[SerializeField] float coolDown;
	[Tooltip("Object with 2D mask used to mask projectiles that go out of bounds.")]
	[SerializeField] GameObject projectileParent;
	[Tooltip("The image that of the bar that shrinks to indicate the cool down status.")]
	[SerializeField] Image coolDownBarIndicator;
	float coolDownBarWidth;
	bool canFire = true;
	Camera cam;
	[Tooltip("SFK for firing projectiles.")]
    public string[] fireCannon;

    private void Start()
	{
		coolDownBarWidth = coolDownBarIndicator.rectTransform.rect.width;
		cam = Camera.main;
	}
	private void Update()
	{
		// convert mouse position into world coordinates
		Vector2 mouseScreenPosition = cam.ScreenToWorldPoint(Input.mousePosition);

		// get direction you want to point at
		Vector2 direction = (mouseScreenPosition - (Vector2)transform.position).normalized;

		// set vector of transform directly
		transform.up = direction;
		if (canFire)
		{
			if ((left && Input.GetMouseButtonDown(1)) || (!left && Input.GetMouseButtonDown(0)))
			{
				Instantiate(projectilePrefab, barrel.position, transform.rotation, projectileParent.transform);
                AudioManager.instance.PlaySFX(fireCannon[Random.Range(0, fireCannon.Length - 1)]);
                StartCoroutine(CoolDown());
			}
		}
	}
	
	IEnumerator CoolDown()
	{
		float timeElapsed = 0;
		canFire = false;
		float height = coolDownBarIndicator.rectTransform.rect.height;

		//adjust width of cool down bar to indicate cool down status
		while (timeElapsed < coolDown)
		{
			coolDownBarIndicator.rectTransform.sizeDelta = new Vector2(coolDownBarWidth * (timeElapsed / coolDown), height);
			yield return new WaitForSeconds(0.01f);
			timeElapsed += 0.01f;
		}
		coolDownBarIndicator.rectTransform.sizeDelta = new Vector2(coolDownBarWidth, height);
        AudioManager.instance.PlaySFX("Ready to Fire");
        canFire = true;
	}
}