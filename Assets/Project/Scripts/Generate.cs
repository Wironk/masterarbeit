using System;
using System.Collections;
using System.IO;
using System.Text;
using Newtonsoft.Json.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

namespace Project.Scripts
{
    public abstract class Generate : MonoBehaviour
    {
        [SerializeField]
        private GameObject whiteboardObject;
        private Whiteboard _whiteboard;
        
        [SerializeField]
        private TextMeshProUGUI statusField;
        [SerializeField]
        private TextMeshProUGUI noInputImageText;

        protected abstract string modelUrl { get; set; }
        private const string Authorization = "Bearer r8_cGXVOim9GQn3uj2eS504W7yCzPbSaNH03yvmi";
        private string _uploadedImageUrl = "";
        private string _generatedImageUrl = "";
        private string _generateRequestUrl = "";
        private Texture2D _generatedTexture;
        
        private bool _finished;

        protected const bool Debugging = false;

        protected virtual void Start()
        {
            noInputImageText.enabled = false;
            _whiteboard = whiteboardObject.GetComponent<Whiteboard>();
        }

        private void ResetVariables()
        {
            _uploadedImageUrl = "";
            _generatedImageUrl = "";
            _generateRequestUrl = "";
            _generatedTexture = null;
            _finished = false;
        }

        private bool CheckWhiteboardContent()
        {
            noInputImageText.enabled = false;
            
            if (_whiteboard.CheckIfTextureEmpty())
            {
                noInputImageText.enabled = true;
                if (Debugging)
                    UnityEngine.Debug.Log("No input image");
                return false;
            } 
            return true;
        }

        protected abstract bool CheckParameterContents();
        
        protected abstract string CreateJsonFromParameters();
        
        public void StartGenerate()
        {
            statusField.text = "checking parameters";
            if (!CheckWhiteboardContent() | !CheckParameterContents())
            {
                statusField.text = "parameter check failed";
                return;
            }
            statusField.text = "saving texture";
            _whiteboard.SaveDrawnTexture(Path.Combine(Application.persistentDataPath, "image.png"));
            
            StartCoroutine(GenerateImageCoroutine());
        }
        
        private IEnumerator GenerateImageCoroutine()
        {
            statusField.text = "uploading input image";
            yield return SendPostUploadImage();
            if (_uploadedImageUrl == "")
            {
                yield break;
            }
            statusField.text = "sending generate request";
            yield return SendPostGenerate();
            if (_generateRequestUrl == "")
            {
                yield break;
            }
            while (!_finished)
            {
                yield return GetRequestStatus();
                yield return new WaitForSeconds(1);
            }
            if (_generatedImageUrl == "")
            {
                yield break;
            }
            yield return DownloadGeneratedImage();
        }

        private IEnumerator SendPostUploadImage()
        {
            WWWForm form = new WWWForm();
            form.AddBinaryData("content", File.ReadAllBytes(Path.Combine(Application.persistentDataPath, "image.png")), "image.png", "image/png");
            form.AddField("type", "application/octet-stream");
            form.AddField("filename", "./image.png");
        
            using (var w = UnityWebRequest.Post("https://api.replicate.com/v1/files", form))
            {
                w.SetRequestHeader("Authorization", Authorization);
                yield return w.SendWebRequest();
                if (w.result != UnityWebRequest.Result.Success) 
                {
                    print(w.error);
                    statusField.text = "input image upload failed";
                }
                else 
                {
                    _uploadedImageUrl = (string)JToken.Parse(w.downloadHandler.text).SelectToken("urls.get");
                    if (Debugging)
                    {
                        Debug.Log("PostImage: " + w.downloadHandler.text);
                        Debug.Log("UploadedImageUrl: " + _uploadedImageUrl);
                    }
                }
            }
        }

        private IEnumerator SendPostGenerate()
        {
            string json = CreateJsonFromParameters();
            if (Debugging)
                Debug.Log(json);
            
            using (var w = UnityWebRequest.PostWwwForm(modelUrl, ""))
            {
                w.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
                w.SetRequestHeader("Authorization", Authorization);
                w.SetRequestHeader("Content-Type", "application/json");
                yield return w.SendWebRequest();
                if (w.result != UnityWebRequest.Result.Success) 
                {
                    print(w.error);
                    statusField.text = "generate request failed";
                }
                else {
                    _generateRequestUrl = (string)JToken.Parse(w.downloadHandler.text).SelectToken("urls.get");
                    if (Debugging)
                        Debug.Log("PostGenerate: " + w.downloadHandler.text);
                }
            }
        }

        private IEnumerator GetRequestStatus()
        {
            using (var w = UnityWebRequest.Get(_generateRequestUrl))
            {
                w.SetRequestHeader("Authorization", Authorization);
                yield return w.SendWebRequest();
                if (w.result != UnityWebRequest.Result.Success) {
                    print(w.error);
                    statusField.text = "status request failed";
                    _finished = true;
                }
                else {
                    string status = (string)JToken.Parse(w.downloadHandler.text).SelectToken("status");
                    statusField.text = status;
                    if (status == "succeeded")
                    {
                        _generatedImageUrl = (string)JToken.Parse(w.downloadHandler.text).SelectToken("output[0]");
                        if (Debugging)
                        {
                            Debug.Log("GetStatus: " + w.downloadHandler.text);
                            Debug.Log("GenImageURL: " + _generatedImageUrl);
                        }
                        _finished = true;
                    }
                }
            }
        }
        private IEnumerator DownloadGeneratedImage()
        {
            using (var w = UnityWebRequestTexture.GetTexture(_generatedImageUrl))
            {
                w.SetRequestHeader("Authorization", Authorization);
                yield return w.SendWebRequest();
                if (w.result != UnityWebRequest.Result.Success) {
                    print(w.error);
                    statusField.text = "output image download failed";
                }
                else {
                    _generatedTexture = ((DownloadHandlerTexture) w.downloadHandler).texture;
                    if (Debugging)
                        Debug.Log(w.downloadHandler.text);
                    _whiteboard.SetGeneratedTexture(_generatedTexture);
                    ResetVariables();
                }
            }
        }
        
        private IEnumerator DownloadGeneratedImageAsFile(string authorization) {
            using (var w = UnityWebRequest.Get(_generatedImageUrl))
            {
                w.SetRequestHeader("Authorization", authorization);
                string path = Path.Combine(Application.persistentDataPath, "genimage.webp");
                w.downloadHandler = new DownloadHandlerFile(path);
                yield return w.SendWebRequest();
                if (w.result != UnityWebRequest.Result.Success) {
                    print(w.error);
                }
                else
                {
                    Debug.Log("File successfully downloaded and saved to " + path);
                }
            }
        }

        public string GetUploadedImageUrl()
        {
            return _uploadedImageUrl;
        }
    }
}
