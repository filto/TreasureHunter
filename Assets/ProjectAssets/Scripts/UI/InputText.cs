
using System;
using UnityEngine;
using TMPro;

public class InputText : MonoBehaviour
{
    public TMP_Text targetText; 
    public TMP_InputField inputField;
    public TMP_Text outputText;
    public TMP_Text addTextText;

    void Start()
    {
        setText(outputText.text);
    }

    public void startInputText()
    {
        if (targetText != null)
        {   
            inputField.gameObject.SetActive(true); 
            inputField.text = outputText.text;
            inputField.ActivateInputField();
            inputField.onEndEdit.AddListener(setText);
        }
    }
    private void setText(string userInput)
    {
        if (string.IsNullOrWhiteSpace(outputText.text))
        {
            addTextText.gameObject.SetActive(true);
        }
        
        else
        {
            addTextText.gameObject.SetActive(false); 
        }
        
        outputText.text = userInput;
        inputField.gameObject.SetActive(false); 
        inputField.onEndEdit.RemoveListener(setText);
    }
}
