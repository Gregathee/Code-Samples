/*
 * DestroyAfterTime.cs
 * Author(s): #Greg Brandt#
 * Created on: 11/6/2020 (en-US)
 * Description: Destroy object after set time starting when the object is instantiated
 */

using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour
{
    [Tooltip("Time after instatiation that object is destroyed")]
    [SerializeField] float time;

    void Start() { StartCoroutine(DestroyThisAfterTime()); }

    IEnumerator DestroyThisAfterTime()
	{
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
	}
}