using UnityEngine;

public class Add : iQuestion
{
    private int x, y, z;

    public bool Answer(string input)
    {
        if(!int.TryParse(input, out int result))
            return false;
        return result == z;
    }

    public string Question(int difficulty)
    {
        int lower = difficulty / 10;
        int upper = (difficulty + 1) * 5;

        x = Random.Range(lower, upper);
        y = Random.Range(lower, upper);

        z = x + y;

        return x + "\n"
              + "+ " + y;
    }
}
