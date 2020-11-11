using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkTutorial : TutorialPrompt
{
    [SerializeField] GameObject wasd;
    
    protected override IEnumerator ManageTutorial()
    {
        wasd.SetActive(wasd);
        bool w = false;
        bool a = false;
        bool s = false;
        bool d = false;
        while (!w || !a || !s || !d)
        {
            if (Input.GetKeyDown(KeyCode.W)) w = true;
            if (Input.GetKeyDown(KeyCode.A)) a = true;
            if (Input.GetKeyDown(KeyCode.S)) s = true;
            if (Input.GetKeyDown(KeyCode.D)) d = true;
            yield return null;
        }
        wasd.SetActive(false);
        DeactivateTutorial();
    }


}
