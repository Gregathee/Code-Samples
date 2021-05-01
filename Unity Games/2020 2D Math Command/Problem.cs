using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class Problem : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI equationTF = null,
                            variableTF = null,
                            inputTF    = null;

    [SerializeField]
    private SpriteRenderer targetGraphic = null;

    private iQuestion question;
    public char Variable { get; private set; }

    private bool listening = false, acceptingLetters = false;
    private StringBuilder inputString = new StringBuilder();

    private void Update()
    {
        if(listening)
            ReadInput();
    }

    private void ReadInput()
    {
        if(Input.GetKeyDown(KeyCode.Backspace) && inputString.Length > 0)
        {
            inputString.Length = inputString.Length - 1;
            UpdateInputText();
        }

        if(acceptingLetters)
            for(char letter = 'a'; letter <= 'z'; ++letter)
            {
                if(Input.GetKeyDown(letter.ToString()))
                {
                    inputString.Append(letter);
                    UpdateInputText();
                }
            }

        for(char letter = '0'; letter <= '9'; ++letter)
        {
            var numString = letter.ToString();
            var keyNumString = "[" + numString + "]";

            if(Input.GetKeyDown(numString) || Input.GetKeyDown(keyNumString))
            {
                inputString.Append(letter);
                UpdateInputText();
            }
        }

        if(Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            inputString.Append('-');
            UpdateInputText();
        }
    }

    private void UpdateInputText()
    => inputTF.text = inputString.ToString();

    public void AssignProperties(char variable, iQuestion question, int difficulty)
    {
        this.question = question;
        this.Variable = variable;
        variableTF.text = char.ToUpper(variable) + " =";
        equationTF.text = question.Question(difficulty);

        SanitizeInput(question);
    }

    private void SanitizeInput(iQuestion question)
    {
        switch(question)
        {
            case Add a:
            case Subtract b:
            case Multiply c:
            case Divide d:
                acceptingLetters = false;
                break;
            default:
                break;
        }
    }

    public void Answer()
    {
        bool accepted = question.Answer(inputTF.text);

        if(accepted)
        {
            targetGraphic.enabled = false;
            EnemyManager.Inst.ProblemAccepted(Variable);
            equationTF.text = variableTF.text = "";
            SendMessageUpwards("ShipDestroyed", SendMessageOptions.RequireReceiver);
            Destroy(this);
        }
    }
    public void Select()
    {
        listening = true;
        targetGraphic.enabled = true;
        inputTF.text = "";
        inputString.Clear();
    }

    public void Deselect()
    {
        listening = false;
        Answer();
        inputString.Clear();
        inputTF.text = "";
        targetGraphic.enabled = false;
    }

    private void OnMouseDown()
    {
        EnemyManager.Inst.ForceSelect(Variable);
        Select();
    }
}
