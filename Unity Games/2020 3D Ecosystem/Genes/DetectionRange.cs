using UnityEngine;

public class DetectionRange : Gene
{
	public static bool showRange = false;
	[SerializeField] GameObject rangeIndicator = null;

	private void Update()
	{
		if(Input.GetKey(KeyCode.Space)) { showRange = true; }
		else { showRange = false; }
		rangeIndicator.SetActive(showRange);
		Vector3 radius = new Vector3(Stat*2, 0.1f, Stat*2);
		if(showRange)rangeIndicator.transform.localScale = radius;
	}
}
