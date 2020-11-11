using UnityEngine;

public class Divide : iQuestion
{
    int x = 0, y = 0, z;

    public bool Answer(string input)
    {
        if(!int.TryParse(input, out int result))
            return false;
        return result == x;
    }

    public string Question(int difficulty)
    {
        int lower = 1 + difficulty / 10;
        int upper = 5 + (difficulty / 5);

        x = Random.Range(lower, upper);
        y = Random.Range(lower, upper);

        z = x * y;

        return z + "\n"
              + "/ " + y;
    }
}
