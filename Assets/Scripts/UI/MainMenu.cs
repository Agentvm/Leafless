using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Leafless.UI
{
    public class MainMenu : MonoBehaviourSingleton<MainMenu>
    {
        public bool TutorialToggleActive { get => _tutorialToggle.isOn; }
        [SerializeField] private Toggle _tutorialToggle;
        /*private Toggle TutorialToggle
        {
            get
            {
                if (_tutorialToggle == null)
                    _tutorialToggle = this.transform.Find(nameof(TutorialToggle)).GetComponent<Toggle>();

                return _tutorialToggle;
            }
        }*/

        public bool SoundToggleActive { get => _soundToggle.isOn; }
        [SerializeField] private Toggle _soundToggle;
        /*private Toggle SoundToggle
        {
            get
            {
                if (_soundToggle == null)
                    _soundToggle = this.transform.Find(nameof(SoundToggle)).GetComponent<Toggle>();

                return _soundToggle;
            }
        }*/

        void Start ()
        {

        }
    }
}
