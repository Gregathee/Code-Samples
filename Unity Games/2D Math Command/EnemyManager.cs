using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Inst;

    [SerializeField]
    private GameObject[] enemies = null,
                         specialEnemies = null;

    [SerializeField]
    private float highSpawnTime = 4f, lowSpawnTime = 2.5f;

    [SerializeField]
    private GameObject friendlyMissile = null;
    [SerializeField]
    private Transform friendlySpawnPos = null;

    private Dictionary<char, Problem> activeEnemies = new Dictionary<char, Problem>();

    private int difficulty = 0, numSpawns = 0;
    private float spawnXRange = 6; // make dynamic with camera
    private char? activeEnemyLetter = null;
    private bool tutorial = false;

    private void Awake() => Inst = this;
    private void Start() => Invoke("SpawnEnemy", 0);

    private void Update()
    {
        bool acceptButton = Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter);
        if(acceptButton && activeEnemyLetter.HasValue)
        {
            Deselect();
            activeEnemyLetter = null;
        }

        for(char letter = 'a'; letter <= 'z'; ++letter)
        {
            if(Input.GetKeyDown(letter.ToString()) && activeEnemies.ContainsKey(letter))
            {
                if(activeEnemyLetter.HasValue && activeEnemyLetter.Value != letter)
                    Deselect();

                Select(letter);
            }
        }
    }

    private void Deselect()
    {
        activeEnemies[activeEnemyLetter.Value].Deselect();
        activeEnemyLetter = null;
    }

    public void ForceDeselect(char variable)
    {
        if(activeEnemyLetter == variable)
            activeEnemyLetter = null;
    }

    private void Select(char variable)
    {
        activeEnemies[variable].Select();
        activeEnemyLetter = variable;
    }

    public void ForceSelect(char variable)
    {
        if(activeEnemyLetter.HasValue)
            Deselect();

        if(activeEnemies.ContainsKey(variable))
        {
            activeEnemies[variable].Select();
            activeEnemyLetter = variable;
        }
    }

    private void SpawnEnemy()
    {
        var spawnPoint = new Vector2((Random.Range(-spawnXRange, spawnXRange)), 7f);
        var enemy = enemies[Random.Range(0, enemies.Length)];

        bool spawnSpecial = numSpawns % 25 == 24;

        if(spawnSpecial) // spawn a special enemy
        {
            spawnPoint = new Vector2(10, Random.Range(-1f, 3f));
            enemy = specialEnemies[Random.Range(0, specialEnemies.Length)];
        }

        char variable = (char)Random.Range('a', 'z' + 1);
        for(int attempt = 0; attempt < 15 && activeEnemies.ContainsKey(variable); ++attempt)
            variable = (char)Random.Range('a', 'z' + 1);

        // If we run out of attempts to find a nonused, randomized variable -> STOP, try again later
        if(activeEnemies.ContainsKey(variable))
        {
            Invoke("SpawnEnemy", Random.Range(lowSpawnTime, highSpawnTime));
            return;
        }

        var newQuestion = GenerateQuestion();
        var newDifficulty = (spawnSpecial) ? difficulty + 5 : difficulty;
        var newEnemy = Instantiate(enemy, spawnPoint, Quaternion.identity, transform);
        newEnemy.GetComponent<Problem>().AssignProperties(variable, newQuestion, newDifficulty);

        activeEnemies.Add(variable, newEnemy.GetComponent<Problem>());

        Invoke("SpawnEnemy", Random.Range(lowSpawnTime, highSpawnTime));
    }

    private iQuestion GenerateQuestion()
    {
        difficulty = numSpawns++ / 3;

        switch(Random.Range(0, 4))
        {
            case 0: return new Add();
            case 1: return new Subtract();
            case 2: return new Multiply();
            case 3: return new Divide();
            default: return new Add();
        }
    }

    public void ProblemAccepted(char variable)
    {
        var missile =
            Instantiate(friendlyMissile, friendlySpawnPos.position, Quaternion.identity)
            .GetComponent<FriendlyMissile>();

        missile.SetTarget(activeEnemies[variable].GetComponent<Enemy>());

        RemoveProblem(variable);
    }

    public void RemoveProblem(char variable)
    {
        if(activeEnemies.ContainsKey(variable))
            activeEnemies.Remove(variable);
    }
}