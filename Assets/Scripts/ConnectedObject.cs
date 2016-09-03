using System;
using UnityEngine;
using Debug = System.Diagnostics.Debug;
#if WINDOWS_UWP
using DSLink.Nodes;
using ValueType = DSLink.Nodes.ValueType;
#endif

namespace DSHoloLens
{
    public class ConnectedObject : MonoBehaviour
    {
        public TextMesh TextMesh;

#if WINDOWS_UWP
        private static int objNumber = 1;

        protected Node rootNode;
        protected Node doRotateDemoNode;
        protected Node positionX;
        protected Node positionY;
        protected Node positionZ;
        protected Node rotX;
        protected Node rotY;
        protected Node rotZ;
        protected Node scaleX;
        protected Node scaleY;
        protected Node scaleZ;
        protected Node textNode;
        protected Node textHiddenNode;

        private bool doRotateDemo;
        private float? pX;
        private float? pY;
        private float? pZ;
        private float? rX;
        private float? rY;
        private float? rZ;
        private float? sX;
        private float? sY;
        private float? sZ;
        private string newLabelText = "Label";
        private bool? newLabelHidden;

        private float nextActionTime = 0.0f;
        private float period = 0.01f;

        public virtual void Start()
        {
            rootNode = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("Object-" + objNumber).BuildNode();
            objNumber++;

            rootNode.CreateChild("unityPath")
                .SetType(ValueType.String)
                .SetValue(GetObjectPath(transform))
                .BuildNode();

            doRotateDemoNode = rootNode.CreateChild("doRotateDemo")
                .SetType(ValueType.Boolean)
                .SetValue(false)
                .SetWritable(Permission.Config)
                .BuildNode();
            doRotateDemoNode.Value.OnRemoteSet += value =>
            {
                doRotateDemo = value.Boolean;
            };

            positionX = rootNode.CreateChild("positionX")
                .SetType(ValueType.Number)
                .SetValue(transform.position.x)
                .SetWritable(Permission.Write)
                .BuildNode();
            positionX.Value.OnRemoteSet += value =>
            {
                pX = value.Float;
            };

            positionY = rootNode.CreateChild("positionY")
                .SetType(ValueType.Number)
                .SetValue(transform.position.y)
                .SetWritable(Permission.Write)
                .BuildNode();
            positionY.Value.OnRemoteSet += value =>
            {
                pY = value.Float;
            };

            positionZ = rootNode.CreateChild("positionZ")
                .SetType(ValueType.Number)
                .SetValue(transform.position.z)
                .SetWritable(Permission.Write)
                .BuildNode();
            positionZ.Value.OnRemoteSet += value =>
            {
                pZ = value.Float;
            };

            rotX = rootNode.CreateChild("rotX")
                .SetType(ValueType.Number)
                .SetValue(transform.rotation.x)
                .SetWritable(Permission.Write)
                .BuildNode();
            rotX.Value.OnRemoteSet += value =>
            {
                rX = value.Float;
            };

            rotY = rootNode.CreateChild("rotY")
                .SetType(ValueType.Number)
                .SetValue(transform.rotation.y)
                .SetWritable(Permission.Write)
                .BuildNode();
            rotY.Value.OnRemoteSet += value =>
            {
                rY = value.Float;
            };

            rotZ = rootNode.CreateChild("rotZ")
                .SetType(ValueType.Number)
                .SetValue(transform.rotation.z)
                .SetWritable(Permission.Write)
                .BuildNode();
            rotZ.Value.OnRemoteSet += value =>
            {
                rZ = value.Float;
            };

            scaleX = rootNode.CreateChild("scaleX")
                .SetType(ValueType.Number)
                .SetValue(transform.localScale.x)
                .SetWritable(Permission.Write)
                .BuildNode();
            scaleX.Value.OnRemoteSet += value =>
            {
                sX = value.Float;
            };

            scaleY = rootNode.CreateChild("scaleY")
                .SetType(ValueType.Number)
                .SetValue(transform.localScale.y)
                .SetWritable(Permission.Write)
                .BuildNode();
            scaleY.Value.OnRemoteSet += value =>
            {
                sY = value.Float;
            };

            scaleZ = rootNode.CreateChild("scaleZ")
                .SetType(ValueType.Number)
                .SetValue(transform.localScale.z)
                .SetWritable(Permission.Write)
                .BuildNode();
            scaleZ.Value.OnRemoteSet += value =>
            {
                sZ = value.Float;
            };

            if (TextMesh != null)
            {
                textNode = rootNode.CreateChild("labelText")
                    .SetType(ValueType.String)
                    .SetValue(TextMesh.text)
                    .SetWritable(Permission.Write)
                    .BuildNode();
                textNode.Value.OnRemoteSet += value =>
                {
                    newLabelText = value.String;
                };

                textHiddenNode = rootNode.CreateChild("labelHidden")
                    .SetType(ValueType.Boolean)
                    .SetValue(false)
                    .SetWritable(Permission.Write)
                    .BuildNode();
                textHiddenNode.Value.OnRemoteSet += value =>
                {
                    newLabelHidden = value.Boolean;
                };
            }
        }
        
        public virtual void Update()
        {
            if (doRotateDemo)
            {
                transform.Rotate(Vector3.up, 25f * Time.deltaTime);
                transform.Rotate(Vector3.right, 25f * Time.deltaTime);
            }

            if (Time.time > nextActionTime)
            {
                nextActionTime = Time.time + period;
                try
                {
                    var position = transform.position;
                    var rotation = transform.rotation;
                    var localScale = transform.localScale;

                    if (pX.HasValue || pY.HasValue || pZ.HasValue)
                    {
                        SetPosition();
                        pX = null;
                        pY = null;
                        pZ = null;
                    }
                    else
                    {
                        positionX.Value.Set(position.x);
                        positionY.Value.Set(position.y);
                        positionZ.Value.Set(position.z);
                    }

                    if (rX.HasValue || rY.HasValue || rZ.HasValue)
                    {
                        SetRotation();
                        rX = null;
                        rY = null;
                        rZ = null;
                    }
                    else
                    {
                        rotX.Value.Set(rotation.x);
                        rotY.Value.Set(rotation.y);
                        rotZ.Value.Set(rotation.z);
                    }

                    if (sX.HasValue || sY.HasValue || sZ.HasValue)
                    {
                        SetScale();
                        sX = null;
                        sY = null;
                        sZ = null;
                    }
                    else
                    {
                        scaleX.Value.Set(localScale.x);
                        scaleY.Value.Set(localScale.y);
                        scaleZ.Value.Set(localScale.z);
                    }
                }
                catch (Exception e)
                {
                    HoloLensDSLink.Instance.Logger.Error("Exception occurred\n" + e.Message + "\n" + e.StackTrace);
                }
                if (TextMesh != null && newLabelText != null && !newLabelText.Equals(TextMesh.text))
                {
                    TextMesh.text = newLabelText;
                    newLabelText = null;
                }
                if (newLabelHidden.HasValue && TextMesh != null)
                {
                    TextMesh.GetComponent<Renderer>().enabled = !newLabelHidden.Value;
                    newLabelHidden = null;
                }
            }
        }
        
        public void SetPosition()
        {
            var position = transform.position;
            if (pX.HasValue) position.x = pX.Value;
            if (pY.HasValue) position.y = pY.Value;
            if (pZ.HasValue) position.z = pZ.Value;
            transform.position = position;
        }

        public void SetRotation()
        {
            var rotation = transform.rotation;
            if (rX.HasValue) rotation.x = rX.Value;
            if (rY.HasValue) rotation.y = rY.Value;
            if (rZ.HasValue) rotation.z = rZ.Value;
            transform.rotation = rotation;
        }

        public void SetScale()
        {
            var scale = transform.localScale;
            if (sX.HasValue) scale.x = sX.Value;
            if (sY.HasValue) scale.y = sY.Value;
            if (sZ.HasValue) scale.z = sZ.Value;
            transform.localScale = scale;
        }

        public static string GetObjectPath(Transform transform)
        {
            var path = "/" + transform.name;
            if (transform.parent != null)
            {
                path = GetObjectPath(transform.parent) + path;
            }
            return path;
        }
#else
        public virtual void Start()
        {
        }

        public virtual void Update()
        {
        }
#endif

        public virtual void OnSelect()
        {
        }

        public virtual void OnHoldStart()
        {
        }

        public virtual void OnHoldStop()
        {
        }

        public virtual void OnVoiceRemove()
        {
            Debug.WriteLine("Removed object via voice.");
#if WINDOWS_UWP
            rootNode.RemoveFromParent();
#endif
            Destroy(gameObject);
        }
    }
}
