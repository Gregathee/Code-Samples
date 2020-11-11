public interface iQuestion
{
    // Difficulty currently (8/8/20 10:30PM CST) scales in 2's through 8.
    string Question(int difficulty);
    bool Answer(string input);
}
