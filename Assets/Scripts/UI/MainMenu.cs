using Leafless.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Leafless.UI
{
    public class MainMenu : MonoBehaviourSingleton<MainMenu>
    {

        // Cached Properties
        #region CachedProperties
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
        #endregion

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
            SceneLoader.SceneLoaded += SceneLoaded;
            SoundToggle.onValueChanged.AddListener(SoundToggled);
            Tutorial.Completed += TutorialCompleted;
        }

        private void SceneLoaded(SceneIdentifiers newScene)
        {
            if (newScene == SceneIdentifiers.Menu) this.gameObject.SetActive(true);
            else if (newScene == SceneIdentifiers.Game) this.gameObject.SetActive(false);

            SoundToggled(SoundToggleActive);
        }

        private void SoundToggled (bool value)
        {
            AudioListener.volume = value ? 1f : 0f;
        }

        private void TutorialCompleted()
        {
            this.TutorialToggle.isOn = false;
        }
    }
}
