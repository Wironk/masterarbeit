using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Scripts
{
    public class GenerateBFL : Generate
    {
        private TMP_InputField _promptField;
        private ShowSliderValue _promptStrengthSlider;
        private ShowSliderValue _guidanceSlider;

        protected override string modelUrl { get; set; }            
    
        [Serializable]
        public class InputData
        {
            public string prompt;
            public string image;
            public float prompt_strength;
            public float guidance;
            public string output_format;
        }
        
        [Serializable]
        public class Data
        {
            public InputData input;
        }

        protected override void Start()
        {
            base.Start();
            modelUrl = "https://api.replicate.com/v1/models/black-forest-labs/flux-dev/predictions";
            
            _promptField = transform.GetChild(0).GetChild(0).GetComponent<TMP_InputField>();
            _promptStrengthSlider = transform.GetChild(1).GetChild(1).GetComponent<ShowSliderValue>();
            _guidanceSlider = transform.GetChild(2).GetChild(1).GetComponent<ShowSliderValue>();
        }
        
        protected override bool CheckParameterContents()
        {
            bool check = true;

            if (_promptField.text == "")
            {
                var placeholder = _promptField.placeholder.GetComponent<TMP_Text>();
                placeholder.text = "Input required";
                placeholder.color = Color.red;
                Debug.Log("No input Prompt");
                check = false;
            }
            
            if (Debugging)
                Debug.Log("Parameters: " + check);
        
            return check;
        }

        protected override string CreateJsonFromParameters()
        {
            InputData i = new InputData();
            i.image = GetUploadedImageUrl();
            i.prompt = _promptField.text;
            i.prompt_strength = _promptStrengthSlider.GetValue();
            i.guidance = _guidanceSlider.GetValue();
            i.output_format = "png";
                
            Data d = new Data();
            d.input = i;

            return JsonUtility.ToJson(d);
        }
    }
}
