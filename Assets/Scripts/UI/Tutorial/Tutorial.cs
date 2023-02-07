using Leafless.UI;
using UnityEngine;

namespace Leafless.UI.Tutorial
{
    public class Tutorial : MonoBehaviourSingleton<Tutorial>
    {
        [SerializeField] GameObject move_text;
        [SerializeField] GameObject leaf_text;
        [SerializeField] GameObject shoot_text;
        [SerializeField] GameObject world_text;

        Transform player;

        Vector3 origin;
        Vector3 text_offset;

        // Start is called before the first frame update
        void Start()
        {
            player = GameObject.FindObjectOfType<Movement>().transform;
            origin = player.position;
            text_offset = move_text.transform.position - player.position;

            if (!MainMenu.Instance.TutorialToggleActive)
                this.gameObject.SetActive(false);
            else this.gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {


            // tell the player to move
            if (player && Vector3.Distance(player.position, origin) > 6f)
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
                if (Vector3.Distance(player.position, leaf_position) > 18f)
                    leaf_text.transform.position = player.position + (leaf_position - player.position) * 0.35f + text_offset;
                else
                    leaf_text.transform.position = leaf_position + text_offset;

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

                if (Vector3.Distance(player.position, enemy_position) > 20f)
                    shoot_text.transform.position = player.position + (enemy_position - player.position) * 0.35f + text_offset;
                else
                    shoot_text.transform.position = enemy_position + text_offset;

                // Stop if score rises
                if (GameState.Instance.Award > 7)
                {
                    shoot_text.SetActive(false);
                    world_text.SetActive(true);
                    world_text.transform.position = player.transform.position + text_offset;
                }
            }

            // End the Tutorial if world exends
            if (world_text.activeSelf && world_text.activeInHierarchy && GameObject.FindGameObjectsWithTag("Panel").Length > 1)
            {
                this.gameObject.SetActive(false);
                GameState.Instance.tutorial_completed = true;
            }
        }

        Vector3 getNearestTagPosition(string tag)
        {
            GameObject[] leaf_array = GameObject.FindGameObjectsWithTag(tag);
            Vector3 nearest_pos = player.position;
            float shortest_distance = 100f;

            foreach (GameObject leaf in leaf_array)
            {
                float current_distance = Vector3.Distance(leaf.transform.position, player.position);

                if (current_distance < shortest_distance)
                {
                    shortest_distance = current_distance;
                    nearest_pos = leaf.transform.position;
                }

            }

            return nearest_pos;
        }
    }
}
