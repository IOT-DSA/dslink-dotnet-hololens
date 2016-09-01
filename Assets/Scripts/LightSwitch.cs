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
                HoloLensDSLink.Instance.Logger.Error("LightSwitchButton is null.");
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
                needsRotate = true;
            };
#endif
        }

        public override void Update()
        {
            base.Update();

            if (LightSwitchButton != null)
            {
                if (needsRotate)
                {
                    LightSwitchButton.transform.Rotate(0, State ? 180f : -180f, 0, Space.Self);
                    needsRotate = false;
                }
            }
        }

        public override void OnSelect()
        {
            State = !State;
            needsRotate = true;
#if WINDOWS_UWP
            stateNode.Value.Set(State);
#endif
        }
    }
}
