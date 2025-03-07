using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Experimental.Rendering.RenderGraphModule;
using UnityEngine.UI;

namespace Project.Scripts
{
    public class Whiteboard : MonoBehaviour
    {
        [SerializeField]
        private Vector2 textureSize = new(2048, 2048);
        
        private Renderer _renderer;
        public Texture2D texture { get; private set; }
        private Texture2D _originalTexture;
        private Texture2D _generatedTexture;

        private Button _switchTextureButton;
        private TextMeshProUGUI _switchTextureButtonText;
        private bool _showingResultTexture;
    
        private void Start()
        {
            _renderer = GetComponent<Renderer>();
            _originalTexture = new Texture2D((int)textureSize.x, (int)textureSize.y);
            texture = new Texture2D((int)textureSize.x, (int)textureSize.y);
            for (int i = 0; i < textureSize.x; i++)
            {
                for (int j = 0; j < textureSize.y; j++)
                {
                    texture.SetPixel(i, j, Color.white);
                    _originalTexture.SetPixel(i, j, Color.white);

                }
            }

            //texture.LoadImage(File.ReadAllBytes(Path.Combine(Application.persistentDataPath, "image.png")));
            _renderer.material.mainTexture = texture;
            texture.Apply();

            _switchTextureButton = transform.parent.GetChild(1).GetChild(5).GetComponent<Button>();
            _switchTextureButtonText = _switchTextureButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            _switchTextureButton.interactable = false;
        }

        public bool CheckIfTextureEmpty()
        {
            Texture2D t = texture;
            return (Hash128.Compute(t.EncodeToJPG(1)) ==
                    Hash128.Compute(_originalTexture.EncodeToJPG(1)));
        }

        public void SaveDrawnTexture(string path)
        {
            Debug.Log("Saving Image to " + path);
            File.WriteAllBytes(path, texture.EncodeToPNG());
        }
        
        public void SaveGeneratedTexture(string path)
        {
            Debug.Log("Saving Image to " + path);
            File.WriteAllBytes(path, _generatedTexture.EncodeToPNG());
        }
    
        public void SetGeneratedTexture(Texture2D t)
        {
            _generatedTexture = t;
            _renderer.material.mainTexture = t; 
            _switchTextureButtonText.text = "Show input";
            _showingResultTexture = true;
            _switchTextureButton.interactable = true;
            
            SaveGeneratedTexture(Path.Combine(Application.persistentDataPath, "genimage.png"));
        }

        public void SwitchTexture()
        {
            if (_showingResultTexture)
            {
                _renderer.material.mainTexture = texture;
                _switchTextureButtonText.text = "Show result";
                _showingResultTexture = false;
            }
            else
            {
                _renderer.material.mainTexture = _generatedTexture;
                _switchTextureButtonText.text = "Show input";
                _showingResultTexture = true;
            }
        }

        public Vector2 GetTextureSize()
        {
            return textureSize;
        }
    }
}