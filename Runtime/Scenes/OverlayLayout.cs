using UnityEngine;
namespace fwp.gamepad.layout
{

    public class OverlayLayout : MonoBehaviour
    {
        public DataGamepadLayout layout;

        private void OnValidate()
        {
            applyLayout(layout);
        }

        void applyLayout(DataGamepadLayout layout)
        {
            this.layout = layout;
            if (this.layout == null)
                return;


        }
    }

}