using UnityEngine;

#if WINDOWS_UWP
using DSLink.Nodes;
#endif

namespace DSHoloLens
{
    public class LightSwitch : ConnectedObject
    {
        public GameObject LightSwitchButton;
        public bool State;
        private bool needsRotate;

#if WINDOWS_UWP
        private Node stateNode;
#endif

        public override void Start()
        {
            base.Start();

            if (LightSwitchButton == null)
            {
                Debug.LogError("LightSwitchButton is null.");
            }

#if WINDOWS_UWP
            rootNode.DisplayName = "Light Switch";

            stateNode = rootNode.CreateChild("state")
                .SetType(ValueType.Boolean)
                .SetValue(State)
                .SetWritable(Permission.Write)
                .BuildNode();
            stateNode.Value.OnRemoteSet += value =>
            {
                State = value.Boolean;
            };
#endif
        }

        public override void Update()
        {
            base.Update();

            if (LightSwitchButton != null)
            {
                LightSwitchButton.transform.localRotation = new Quaternion(0, State ? 270f : 90f, 0, 0);
            }
        }

        public override void OnSelect()
        {
            State = !State;
#if WINDOWS_UWP
            stateNode.Value.Set(State);
#endif
        }
    }
}
