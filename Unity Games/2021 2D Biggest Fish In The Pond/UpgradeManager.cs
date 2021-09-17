using System.Collections.Generic;
using System.IO;
using HobbitUtilz;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Manages the state of player upgrades
/// </summary>
public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;
    [SerializeField] TMP_Text _coinsText;
    [SerializeField] TMP_Text _speedCostText;
    [SerializeField] TMP_Text _startingSizeCostText;
    [SerializeField] TMP_Text _heartsCostText;
    [SerializeField] TMP_Text _speedUpgradesText;
    [SerializeField] TMP_Text _startingSizeUpgradesText;
    [SerializeField] TMP_Text _heartsUpgradesText;
    [SerializeField] Button _speedButton;
    [SerializeField] Button _startSizeButton;
    [SerializeField] Button _heartsButton;
    [SerializeField] float _speedUpgradeAmount = 1;
    [SerializeField] float _startingSizeUpgradeAmount = 0.1f;
    [SerializeField] float _speedCostModifier = 0.25f;
    [SerializeField] float _sizeCostModifier = 0.25f;
    [SerializeField] float _heartCostModifier = 0.25f;
    [SerializeField] int _speedCost = 20;
    [SerializeField] int _sizeCost = 20;
    [SerializeField] int _heartCost = 20;
    
    
    const int MAX_LEVEL = 100;
    
    int _hpUpgrade = 1;
    int _speedLevel = 0;
    int _startSizeLevel = 0;
    int _coins = 0;
    float _startingSize = 0f;
    float _speedUpgrade = 0;
    float _baseSize = 0.5f;
    string FILE_PATH;

    void Awake()
    {
        if (!Instance)
        {
            FILE_PATH = Application.persistentDataPath + "/saveFile.json";
            Instance = this;
            Load();
        }
        else {Destroy(gameObject);}
    }

    /// <summary>
    /// Saves current upgrade status in json file
    /// </summary>
    public void Save()
    {
        Dictionary<string, string> saveDict = new Dictionary<string, string>()
        {
            {"coins", _coins.ToString()},
            {"speed", _speedLevel.ToString()},
            {"hp", _hpUpgrade.ToString()},
            {"size", _startSizeLevel.ToString()}
        };
        StreamWriter writer = new StreamWriter(FILE_PATH);
        writer.Write(HU_Functions.Dict_To_JSON(saveDict));
        writer.Dispose();
    }

    /// <summary>
    /// Loads upgrade status from json file
    /// </summary>
    public void Load()
    {
        if (!File.Exists(FILE_PATH)) { return;}
        StreamReaderPro reader = new StreamReaderPro(FILE_PATH);
        Dictionary<string, string> saveDict = HobbitUtilz.HU_Functions.JSON_To_Dict(reader.ToString());
        _coins = int.Parse(saveDict["coins"]);
        _hpUpgrade = int.Parse(saveDict["hp"]);
        _startSizeLevel = int.Parse(saveDict["size"]);
        _speedLevel = int.Parse(saveDict["speed"]);

        _speedUpgrade = (_speedLevel) * _speedUpgradeAmount;
        _startingSize = (_startSizeLevel) * _startingSizeUpgradeAmount;
    }

    /// <summary>
    /// Resets all upgrades to and saves.
    /// </summary>
    public void ResetProgress()
    {
        _coins = 0;
        _startSizeLevel = 0;
        _speedLevel = 0;
        _hpUpgrade = 1;
        
        _speedUpgrade = (_speedLevel) * _speedUpgradeAmount;
        _startingSize = (_startSizeLevel) * _startingSizeUpgradeAmount;
        
        Save();
        DisplayButtons();
    }

    /// <summary>
    /// Determines if upgrade buttons are interactable based on cost and make level and updates texts to reflect costs and upgrade levels.
    /// </summary>
    public void DisplayButtons()
    {
        bool canAffordSpeed = _coins >= CalculateSpeedCost();
        bool canAffordSize = _coins >= CalculateSizeCost();
        bool canAffordHearts = _coins >= CalculateHPCost();
        bool canLevelSpeed = _speedLevel < MAX_LEVEL;
        bool canLevelSize = _startSizeLevel < MAX_LEVEL;
        bool canLevelHeart = _hpUpgrade < MAX_LEVEL;

        _speedButton.interactable = canAffordSpeed && canLevelSpeed;
        _startSizeButton.interactable = canAffordSize && canLevelSize;
        _heartsButton.interactable = canAffordHearts && canLevelHeart;

        _speedUpgradesText.text = _speedLevel.ToString();
        _startingSizeUpgradesText.text = _startSizeLevel.ToString();
        _heartsUpgradesText.text = _hpUpgrade.ToString();
        _speedCostText.text = CalculateSpeedCost().ToString();
        _startingSizeCostText.text = CalculateSizeCost().ToString();
        _heartsCostText.text = CalculateHPCost().ToString();
        _coinsText.text = _coins.ToString();
    }

    /// <summary>
    /// Adds to players available coins for upgrades
    /// </summary>
    /// <param name="amount"></param>
    public void AddCoins(int amount) { _coins += amount;}

    /// <summary>
    /// Deducts the cost of speed upgrade and applies 1 level to speed
    /// </summary>
    public void UpgradeSpeed()
    {
        _coins -= CalculateSpeedCost();
        _speedUpgrade += _speedUpgradeAmount;
        _speedLevel++;
        DisplayButtons();
    }

    /// <summary>
    /// Deducts the cost of size upgrade and applies 1 level to size
    /// </summary>
    public void UpgradeStartingSize()
    {
        _coins -= CalculateSizeCost();
        _startingSize += _startingSizeUpgradeAmount;
        _startSizeLevel++;
        DisplayButtons();
    }

    /// <summary>
    /// Deducts the cost of hp upgrade and applies 1 level to hp
    /// </summary>
    public void UpgradeHP()
    {
        _coins -= CalculateHPCost();
        _hpUpgrade++;
        DisplayButtons();
    }

    public float GetSpeed() { return _speedUpgrade; }
    public float GetSize() { return _baseSize + _startingSize; }
    public int GetHP() { return _hpUpgrade; }

    int CalculateSpeedCost() { return Mathf.RoundToInt(_speedCost * (_speedLevel + 1) * _speedCostModifier); }
    int CalculateSizeCost() { return Mathf.RoundToInt(_sizeCost * (_startSizeLevel + 1) * _sizeCostModifier); }
    int CalculateHPCost() { return Mathf.RoundToInt(_heartCost * (_hpUpgrade + 1) * _heartCostModifier); }
}
