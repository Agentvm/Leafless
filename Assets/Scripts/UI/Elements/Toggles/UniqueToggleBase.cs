using UnityEngine;
using UnityEngine.UI;

namespace Leafless.UI.Elements.Toggles
{
    public abstract class UniqueToggleBase : MonoBehaviourSingleton<UniqueToggleBase>
    {
        protected static Toggle _toggle;
        public static bool State { get => _toggle.isOn;}


        protected virtual void Start()
        {
            _toggle = this.GetComponent<Toggle>();
            _toggle.onValueChanged.AddListener(ValueChanged);
        }

        protected virtual void ValueChanged(bool value)
        {
            // Optional implementation in derived class
        }
    }
}
