/* * (Greg Brandt) 
 * * (Assignment 6) 
 * * Parent class for signletons
 */
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T instance = null;
    public static T Instance { get { return instance; } }

	private void Awake()
	{
		if(instance) 
		{ 
			Debug.LogError("[Singleton] Trying to instantiate a second instance of a singleton class");
			Destroy(gameObject);
		}
		else { instance = (T)this; DontDestroyOnLoad(this); }
	}

	public static bool ISInitialized { get { return instance != null; } }

	protected virtual void OnDestroy() { if(instance == this) { instance = null; } }

	public void Destroy() { Destroy(gameObject); }
}
