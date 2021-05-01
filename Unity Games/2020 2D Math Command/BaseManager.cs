using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager : MonoBehaviour
{
    public static BaseManager Inst;

    private Base[] bases;
    private List<Base> activeBases = new List<Base>();

    private void Awake() => Inst = this;

    private void Start()
    {
        bases = FindObjectsOfType<Base>();
        activeBases.AddRange(bases);
    }

    public Base GetRandomActiveBase()
    {
        if(CheckLose())
            return null;

        var @base = activeBases[Random.Range(0, activeBases.Count)];
        return @base;
    }

    public void BaseDestroyed(Base @base)
    {
        if(activeBases.Contains(@base))
            activeBases.Remove(@base);

        CheckLose();
    }

    public bool CheckLose()
    {
        if(activeBases.Count == 0)
        {
            ScoreManager.Inst.PostScore();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            return true;
        }

        return false;
    }

    public void RestoreBases()
    {
        foreach(var @base in bases)
        {
            if(!activeBases.Contains(@base))
            {
                @base.Restore();
                BaseRestored(@base);
            }
        }
    }

    public void ShieldBases()
    {
        foreach(var @base in activeBases)
        {
            @base.Shield();
        }
    }

    public void BaseRestored(Base @base)
    {
        if(!activeBases.Contains(@base))
            activeBases.Add(@base);
    }
}
