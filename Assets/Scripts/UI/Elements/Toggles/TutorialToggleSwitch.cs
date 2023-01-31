using Leafless.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Leafless.UI.Elements.Toggles
{
    public class TutorialToggleSwitch : UniqueToggleBase
    {
        protected override void Start ()
        {
            base.Start();
            _toggle.isOn = GameState.Instance.tutorial_completed;
        }
    }
}

