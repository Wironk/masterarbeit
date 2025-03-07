using System;
using System.Collections;
using System.Collections.Generic;
using Project.Scripts;
using TMPro;
using UnityEngine;

public class GenerateQR2AI : Generate
{
    private TMP_InputField promptField;
    private TMP_InputField additionalPromptField;
    private TMP_InputField negativePromptField;
    
    protected override string modelUrl { get; set; }
    
    [Serializable]
    public class InputData
    {
        public string image;
        public string prompt;
        public string sampler;
        public string suffix_prompt;
        public string negative_prompt;
    }
        
    [Serializable]
    public class Data
    {
        public string version;
        public InputData input;
    }

    protected override void Start()
    {
        base.Start();
        modelUrl  = "https://api.replicate.com/v1/predictions";
    }
    
    protected override bool CheckParameterContents()
    {
        bool check = true;
        
        promptField = transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
        if (promptField.text == "")
        {
            var placeholder = promptField.placeholder.GetComponent<TMP_Text>();
            placeholder.text = "Input required";
            placeholder.color = Color.red;
            UnityEngine.Debug.Log("No input Prompt");
            check = false;
        }
        
        additionalPromptField = transform.GetChild(1).GetChild(0).GetComponent<TMP_InputField>();
        if (additionalPromptField.text == "")
        {
            var placeholder = additionalPromptField.placeholder.GetComponent<TMP_Text>();
            placeholder.text = "Input required";
            placeholder.color = Color.red;
            UnityEngine.Debug.Log("No additional input Prompt");
            check = false;
        }
        
        negativePromptField = transform.GetChild(2).GetChild(0).GetComponent<TMP_InputField>();
        if (negativePromptField.text == "")
        {
            var placeholder = negativePromptField.placeholder.GetComponent<TMP_Text>();
            placeholder.text = "Input required";
            placeholder.color = Color.red;
            UnityEngine.Debug.Log("No negative input Prompt");
            check = false;
        }

        if (Debugging)
            UnityEngine.Debug.Log("Parameters: " + check);
        
        return check;
    }

    protected override string CreateJsonFromParameters()
    {
        InputData i = new InputData();
        i.image = GetUploadedImageUrl();
        i.prompt = promptField.text;
        i.sampler = "Euler";
        i.suffix_prompt = additionalPromptField.text;
        i.negative_prompt = negativePromptField.text;

        Data d = new Data();
        d.version = "929ace8914560169b34719f45121482241a7ebe53d001b9d66931b2a1e17439d";
        d.input = i;

        if (Debugging)
        {
            UnityEngine.Debug.Log("UploadedImageUrl (child):");
        }

        return JsonUtility.ToJson(d);
    }
}
