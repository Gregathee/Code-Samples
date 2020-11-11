using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiply : iQuestion
{
    int x, y, z;

    public bool Answer(string input)
    {
        if(!int.TryParse(input, out int result))
            return false;
        return result == z;
    }

    public string Question(int difficulty)
    {
        int lower = 2 + difficulty / 10;
        int upper = 5 + (difficulty / 5);

        x = Random.Range(lower, upper);
        y = Random.Range(lower, upper);

        z = x * y;

        return x + "\n"
              + "* " + y;
    }
}
