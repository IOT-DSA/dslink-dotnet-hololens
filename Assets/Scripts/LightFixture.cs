using System;
using UnityEngine;
#if WINDOWS_UWP
using DSLink.Nodes;
using ValueType = DSLink.Nodes.ValueType;
#endif

namespace DSHoloLens
{
    public class LightFixture : ConnectedObject
    {
        public GameObject Bulb;
        private Material BulbMaterial;
        private Color _colorWithoutIntensity;
        private float _intensity;
        private int? _newColor;
        private float? _newIntensity;

#if WINDOWS_UWP
        private Node _colorNode;
        private Node _intensityNode;
#endif

        public override void Start()
        {
            base.Start();

            try
            {
                BulbMaterial = Bulb.GetComponent<Renderer>().material;
                BulbMaterial.EnableKeyword("_EMISSION");
                _colorWithoutIntensity = BulbMaterial.GetColor("_EmissionColor");
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Debug.Log(e.StackTrace);
            }
            

#if WINDOWS_UWP
            rootNode.DisplayName = "Light Fixture";
            
            _colorNode = rootNode.CreateChild("color")
                .SetType(ValueType.Number)
                .SetConfig("editor", new Value("color"))
                .SetWritable(Permission.Write)
                .BuildNode();
            _colorNode.Value.OnRemoteSet += value =>
            {
                _newColor = value.Int;
            };

            _intensityNode = rootNode.CreateChild("intensity")
                .SetType(ValueType.Number)
                .SetValue(1.0f)
                .SetWritable(Permission.Write)
                .BuildNode();
            _intensityNode.Value.OnRemoteSet += value =>
            {
                _newIntensity = value.Float;
            };
#endif
        }

        public override void Update()
        {
            base.Update();

            if (_newColor.HasValue)
            {
                Color color;
                ColorUtility.TryParseHtmlString("#" + _newColor.Value.ToString("X6"), out color);
                _colorWithoutIntensity = color;
                BulbMaterial.SetColor("_EmissionColor", color * _intensity);
                _newColor = null;
            }

            if (_newIntensity.HasValue)
            {
                _intensity = _newIntensity.Value;
                BulbMaterial.SetColor("_EmissionColor", _colorWithoutIntensity * _intensity);
                _newIntensity = null;
            }
        }

        public void OnVoiceActivate()
        {
            _newIntensity = 1.0f;
        }

        public void OnVoiceDeactivate()
        {
            _newIntensity = 0.0f;
        }
    }
}
