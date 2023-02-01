using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Leafless.UI
{
    public class MainMenu : MonoBehaviourSingleton<MainMenu>
    {
        public bool TutorialToggleActive { get => TutorialToggle.isOn; }
        private Toggle _tutorialToggle;
        private Toggle TutorialToggle
        {
            get
            {
                if (_tutorialToggle == null)
                    _tutorialToggle = this.transform.Find(nameof(TutorialToggle)).GetComponent<Toggle>();

                return _tutorialToggle;
            }
        }

        public bool SoundToggleActive { get => SoundToggle.isOn; }
        private Toggle _soundToggle;
        private Toggle SoundToggle
        {
            get
            {
                if (_soundToggle == null)
                    _soundToggle = this.transform.Find(nameof(SoundToggle)).GetComponent<Toggle>();

                return _soundToggle;
            }
        }

        void Start ()
        {
            // Don't destroy on load
            if (this.transform.parent == null)
            {
                Debug.LogError("Main Menu has no Canvas parent!", this);
                return;
            }
            GameObject.DontDestroyOnLoad(this.transform.parent.gameObject);

            // React to start of game
            SceneLoader.SceneLoaded += SceneLoader_SceneLoaded;
        }

        private void SceneLoader_SceneLoaded(SceneIdentifiers newScene)
        {
            bool isMenu = newScene == SceneIdentifiers.Menu;
            if (isMenu) this.gameObject.SetActive(true);
            else this.gameObject.SetActive(false);
        }
    }
}
