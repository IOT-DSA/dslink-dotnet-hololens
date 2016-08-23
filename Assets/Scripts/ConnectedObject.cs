using UnityEngine;

#if WINDOWS_UWP
using DSLink.Nodes;
#endif

namespace DSHoloLens
{
    public class ConnectedObject : MonoBehaviour
    {
#if WINDOWS_UWP
        private Node positionX;
        private Node positionY;
        private Node positionZ;
        private Node rotX;
        private Node rotY;
        private Node rotZ;
        private Node scaleX;
        private Node scaleY;
        private Node scaleZ;

        public void Start()
        {
            positionX = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("positionX")
                .SetType(ValueType.Number)
                .SetValue(transform.position.x)
                .BuildNode();

            positionY = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("positionY")
                .SetType(ValueType.Number)
                .SetValue(transform.position.y)
                .BuildNode();

            positionZ = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("positionZ")
                .SetType(ValueType.Number)
                .SetValue(transform.position.z)
                .BuildNode();

            rotX = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("rotX")
                .SetType(ValueType.Number)
                .SetValue(transform.rotation.x)
                .BuildNode();

            rotY = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("rotY")
                .SetType(ValueType.Number)
                .SetValue(transform.rotation.y)
                .BuildNode();

            rotZ = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("rotZ")
                .SetType(ValueType.Number)
                .SetValue(transform.rotation.z)
                .BuildNode();

            scaleX = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("scaleX")
                .SetType(ValueType.Number)
                .SetValue(transform.localScale.x)
                .BuildNode();

            scaleY = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("scaleY")
                .SetType(ValueType.Number)
                .SetValue(transform.localScale.y)
                .BuildNode();

            scaleZ = HoloLensDSLink.Instance.Responder.SuperRoot.CreateChild("scaleZ")
                .SetType(ValueType.Number)
                .SetValue(transform.localScale.z)
                .BuildNode();
        }
#endif
        
        public void Update()
        {
            transform.Rotate(Vector3.up, 25f * Time.deltaTime);
            transform.Rotate(Vector3.right, 25f * Time.deltaTime);

#if WINDOWS_UWP
            positionX.Value.Set(transform.position.x);
            positionY.Value.Set(transform.position.y);
            positionZ.Value.Set(transform.position.x);
            rotX.Value.Set(transform.rotation.x);
            rotY.Value.Set(transform.rotation.y);
            rotZ.Value.Set(transform.rotation.z);
            scaleX.Value.Set(transform.localScale.x);
            scaleY.Value.Set(transform.localScale.y);
            scaleZ.Value.Set(transform.localScale.z);
#endif
        }
    }
}
