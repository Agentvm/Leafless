using System;
using UnityEngine;

namespace Leafless.UI
{
    public class Tutorial : MonoBehaviourSingleton<Tutorial>
    {
        [SerializeField] GameObject move_text;
        [SerializeField] GameObject leaf_text;
        [SerializeField] GameObject shoot_text;
        [SerializeField] GameObject world_text;

        // Properties
        private bool tutorialActive;
        public bool TutorialActive
        {
            get => tutorialActive; private set
            {
                tutorialActive = value;
                if (!value)
                {
                    this.gameObject.SetActive(false);
                }
            }
        }

        // Cached Properties
        #region CachedProperties
        private Transform _player;
        public Transform Player
        {
            get
            {
                if (_player == null) _player = GameObject.FindObjectOfType<Movement>().transform;
                return _player;
            }
        }

        private Vector3 _origin = Vector3.zero;
        public Vector3 Origin
        {
            get
            {
                if (_origin == Vector3.zero) _origin = Player.position;
                return _origin;
            }
        }

        private Vector3 _textOffset;
        public Vector3 TextOffset
        {
            get
            {
                if (_textOffset == Vector3.zero) _textOffset = move_text.transform.position - Player.position;
                return _textOffset;
            }
        }
        #endregion

        // Events
        public static event Action Completed;

        void Start()
        {
            SceneLoader.SceneLoaded += ActiveSceneChanged;
            TutorialActive = MainMenu.Instance.TutorialToggleActive;
        }

        private void ActiveSceneChanged(SceneIdentifiers scene)
        {
            if (scene == SceneIdentifiers.Game) TutorialActive = MainMenu.Instance.TutorialToggleActive;
            else if (scene == SceneIdentifiers.Menu) TutorialActive = false;
        }

        void Update()
        {
            if (!TutorialActive) return;

            // tell the player to move
            if (Player && Vector3.Distance(Player.position, Origin) > 6f)
            {
                move_text.SetActive(false);
                leaf_text.SetActive(true);
            }

            // tell the player to eat
            if (leaf_text.activeSelf && leaf_text.activeInHierarchy)
            {
                // get the nearest leaf and place the text above it
                Vector3 leaf_position = getNearestTagPosition("Interactable");

                // if it is too far away, direct the player to it
                if (Vector3.Distance(Player.position, leaf_position) > 18f)
                    leaf_text.transform.position = Player.position + (leaf_position - Player.position) * 0.35f + TextOffset;
                else
                    leaf_text.transform.position = leaf_position + TextOffset;

                // stop if score rises
                if (GameState.Instance.Award > 2)
                {
                    leaf_text.SetActive(false);
                    shoot_text.SetActive(true);
                }

            }

            // tell the player to shoot
            if (shoot_text.activeSelf && shoot_text.activeInHierarchy)
            {
                // get the nearest enemy and place the text above it
                Vector3 enemy_position = getNearestTagPosition("Enemy");

                if (Vector3.Distance(Player.position, enemy_position) > 20f)
                    shoot_text.transform.position = Player.position + (enemy_position - Player.position) * 0.35f + TextOffset;
                else
                    shoot_text.transform.position = enemy_position + TextOffset;

                // Stop if score rises
                if (GameState.Instance.Award > 7)
                {
                    shoot_text.SetActive(false);
                    world_text.SetActive(true);
                    world_text.transform.position = Player.transform.position + TextOffset;
                }
            }

            // End the Tutorial if world exends
            if (world_text.activeSelf && world_text.activeInHierarchy && GameObject.FindGameObjectsWithTag("Panel").Length > 1)
            {
                this.gameObject.SetActive(false);
                OnTutorialCompleted();
            }
        }

        Vector3 getNearestTagPosition(string tag)
        {
            GameObject[] leaf_array = GameObject.FindGameObjectsWithTag(tag);
            Vector3 nearest_pos = Player.position;
            float shortest_distance = 100f;

            foreach (GameObject leaf in leaf_array)
            {
                float current_distance = Vector3.Distance(leaf.transform.position, Player.position);

                if (current_distance < shortest_distance)
                {
                    shortest_distance = current_distance;
                    nearest_pos = leaf.transform.position;
                }

            }

            return nearest_pos;
        }

        private static void OnTutorialCompleted()
        {
            Completed?.Invoke();
        }
    }
}
