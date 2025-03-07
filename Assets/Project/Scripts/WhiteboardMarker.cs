using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Simulation;


namespace Project.Scripts
{
    public class WhiteboardMarker : MonoBehaviour
    {
        private Transform _tip;
        private int _penSize = 10;
        private Color[] _color;

        private Renderer _renderer;
        private float _tipHeight;

        private RaycastHit _touch;
        public LayerMask layerMask;
        private bool _touchedLastFrame;
        private Vector2 _touchPos, _lastTouchPos;
        
        private Whiteboard _whiteboard;
        private ControllerPhysics _controllerPhysics;
        private Transform _previewCube;
        
        private void Start()
        {
            _previewCube = transform.GetChild(2);
            _previewCube.localScale = new Vector3((float) 0.0005 * _penSize, (float) 0.0008, (float) 0.0005 * _penSize);
            
            _tip = transform.GetChild(1);
            
            _renderer = _tip.GetComponent<Renderer>();
            _color = Enumerable.Repeat(_renderer.material.color, _penSize * _penSize).ToArray();
            _tipHeight = _tip.localScale.y;
            
            _controllerPhysics = transform.parent.GetComponent<ControllerPhysics>();
        }
        
        private void Update()
        {
            Draw();
        }
        
        public void PenSizeChangeCheck(float newValue)
        {
            _penSize = (int) newValue;
            ChangeColor(_renderer.material.color);
            _previewCube.localScale = new Vector3((float) 0.0005 * _penSize, (float) 0.0008, (float) 0.0005 * _penSize);
        }
        
        public void ChangeMaterial(Material newMaterial)
        {
            _renderer.material = newMaterial;
            _color = Enumerable.Repeat(newMaterial.color, _penSize * _penSize).ToArray();
        }
        
        public void ChangeColor(Color newColor)
        {
            _renderer.material.color = newColor;
            _color = Enumerable.Repeat(newColor, _penSize * _penSize).ToArray();
        }
        
        private void Draw()
        {
            if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight, layerMask))
            {
                if (_touch.transform.CompareTag("Whiteboard"))
                {
                    if (_whiteboard == null) 
                        _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                    
                    if (_controllerPhysics.rotationLocked)
                    {
                        _touchPos = new Vector2(
                            _touch.textureCoord.x, _touch.textureCoord.y);

                        var x = (int)(_touchPos.x * _whiteboard.GetTextureSize().x - _penSize / 2);
                        var y = (int)(_touchPos.y * _whiteboard.GetTextureSize().y - _penSize / 2);

                        if (y < 0 || y > _whiteboard.GetTextureSize().y ||
                            x < 0 || _whiteboard.GetTextureSize().x > 0)

                            if (_touchedLastFrame)
                            {
                                _whiteboard.texture.SetPixels(x, y, _penSize, _penSize, _color);

                                for (var f = 0.01f; f < 1.00f; f += 0.03f)
                                {
                                    var lerpX = (int)Mathf.Lerp(_lastTouchPos.x, x, f);
                                    var lerpY = (int)Mathf.Lerp(_lastTouchPos.y, y, f);
                                    
                                    _whiteboard.texture.SetPixels(lerpX, lerpY, _penSize, _penSize, _color);
                                }

                                _whiteboard.texture.Apply();
                            }

                        _lastTouchPos = new Vector2(x, y);
                        _touchedLastFrame = true;
                        return;
                    }

                    _controllerPhysics.rotationLocked = true;
                    transform.parent.rotation = _whiteboard.transform.rotation * Quaternion.Euler(-55, 0, 0);
                    return;
                }
            } 
            else if (Physics.Raycast(_tip.position, transform.up, out _touch, _tipHeight + (float)0.1, layerMask))
            {
                if (_touch.transform.CompareTag("Whiteboard"))
                {
                    if (_whiteboard == null) _whiteboard = _touch.transform.GetComponent<Whiteboard>();
                    
                    _controllerPhysics.rotationLocked = true;
                    transform.parent.rotation = _whiteboard.transform.rotation * Quaternion.Euler(-55, 0, 0);
                }
            }

            if (_touchedLastFrame)
            {
                _controllerPhysics.ResetPos();
            }

            _whiteboard = null;
            _touchedLastFrame = false;
            _controllerPhysics.rotationLocked = false;
        }
    }
}