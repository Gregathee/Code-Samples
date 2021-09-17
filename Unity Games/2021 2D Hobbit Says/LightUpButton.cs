using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpButton : MonoBehaviour
{
    [SerializeField] Color lightOnColor = new Color();
    [SerializeField] Color lightOffColor = new Color();
    [SerializeField] AudioClip clip = null;
    [SerializeField] float playClipTime = 0.25f;
    List<ButtonSound> sounds = new List<ButtonSound>();
    SimonSays simon = null;
    SpriteRenderer spriteRenderer = null;
    int index = 0;

    void Update()
    {
        for(int i = 0; i < sounds.Count;)
        {
            ButtonSound sound = sounds[i];
            sound.source.volume -= Time.deltaTime;
            if (sounds[i].source.volume <= 0)
            {
                sounds.Remove(sound);
                Destroy(sound.gameObject);
            }
            else { i++; }
        }
    }

    /// <summary>
    /// Play sound and change to color to light up
    /// </summary>
    public void TurnOn()
    {
        spriteRenderer.color = lightOnColor;
        GameObject go = new GameObject();
        ButtonSound sound = go.AddComponent<ButtonSound>();
        sound.source = sound.gameObject.AddComponent<AudioSource>();
        sound.source.PlayOneShot(clip);
        sounds.Add(sound);
    }
    
    // Change to light off color
    public void TurnOff()
    {
        spriteRenderer.color = lightOffColor;
    }
    public void Initialize(SimonSays simonIn, int indexIn)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = lightOffColor;
        index = indexIn;
        simon = simonIn;
    }

    /// <summary>
    /// Play clip for playClipTime seconds
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayClip()
    {
        TurnOn();
        yield return new WaitForSeconds(playClipTime);
        TurnOff();
    }

    void OnMouseDown()
    {
        if (SimonSays.playingSequence) { return;}
        simon.Input(index);
        StopCoroutine(PlayClip());
        StartCoroutine(PlayClip());
    }
}
