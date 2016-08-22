using UnityEngine;

#if WINDOWS_UWP
using DSLink.Nodes;
using Newtonsoft.Json.Linq;
#endif

namespace DSHoloLens
{
    public class RotateCube : MonoBehaviour
    {
#if WINDOWS_UWP
        private Node rotX;
        private Node rotY;
        private Node rotZ;

        public void Start()
        {
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
        }
#endif

        // Update is called once per frame
        public void Update()
        {
            transform.Rotate(Vector3.up, 25f * Time.deltaTime);
            transform.Rotate(Vector3.right, 25f * Time.deltaTime);

#if WINDOWS_UWP
            rotX.Value.Set(transform.rotation.x);
            rotY.Value.Set(transform.rotation.y);
            rotZ.Value.Set(transform.rotation.z);
#endif
        }
    }
}
