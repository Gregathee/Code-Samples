using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Subscriber : IComparable<Subscriber>
{
    public int priority = 0;
    public IEscapeKeySubscriber subscriber = null;

    public Subscriber(int _priority, IEscapeKeySubscriber _subscriber)
    {
        priority = _priority;
        subscriber = _subscriber;
    }
    public int CompareTo(Subscriber other)
    {
        if (this.priority < other.priority) { return -1; }
        if (this.priority == other.priority) { return 0; }
        return 1;
    }
}
public class GameManager : MonoBehaviour
{
    public static bool paused = false;
    public static bool ignorePause = false;
    static bool skipPause = false;
    static bool skipNext = false;
    bool escapePressed = false;
    [SerializeField] GameObject pauseMenu = null;
    static List<Subscriber> subscribers = new List<Subscriber>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Joystick1Button9) || Input.GetKeyDown(KeyCode.Joystick1Button2))
        {
            StartCoroutine(NotifySubscribers());
        }
        //Ignore pause is used to prevent in game menus from displaying the pause menu
        if (paused && ignorePause) { ignorePause = false;}

        if (skipPause) { escapePressed = false;}
        if (escapePressed && !skipPause)
        {
            if (!paused && !ignorePause) { paused = true; }
            else if (paused && !ignorePause) { paused = false;}
            pauseMenu.SetActive(paused);
            escapePressed = false;
        }
        //Prevent game from pausing when exiting a menu using escape
        if (skipPause) { skipPause = false; }
    }
    public static void RegisterEscapeSubscriber(IEscapeKeySubscriber subscriber, int priority)
    {
        subscribers.Add(new Subscriber(priority, subscriber));
    }

    public static void SkipNextPause() { skipNext = true;}

    IEnumerator NotifySubscribers()
    {
        subscribers.Sort();
        foreach (Subscriber subscriber in subscribers)
        {
            subscriber.subscriber.EscapeKeyPressed();
            yield return new WaitForEndOfFrame();
        }
        skipPause = skipNext;
        skipNext = false;
        escapePressed = true;
    }
}
