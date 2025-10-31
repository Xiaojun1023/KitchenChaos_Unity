using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private Player player;
    private float footstep_timer;
    private float footstep_timer_max = 0.1f; // Adjust this value to change the frequency of footstep sounds

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        footstep_timer -= Time.deltaTime;

        if (footstep_timer <= 0f)
        {
            footstep_timer = footstep_timer_max;

            if (player.IsWalking())
            {
                float volume = 1f; // You can adjust the volume as needed

                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume);
            }
        }
    }
}
