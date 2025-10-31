using System;
using UnityEngine;

public class StoveCounterSound : MonoBehaviour
{
    [SerializeField] private StoveCounter stoveCounter;

    private AudioSource audioSource;
    private float warning_sound_timer;
    private bool playWarningSound;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
        stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
    }

    private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        float burn_show_progress_amount = 0.5f;
        playWarningSound = stoveCounter.IsFried() && e.progress_normalized >= burn_show_progress_amount;
    }

    private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
    {
        bool play_sound = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;

        if (play_sound)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Pause();
        }
    }

    private void Update()
    {
        if (playWarningSound)
        {
            warning_sound_timer -= Time.deltaTime;

            if (warning_sound_timer <= 0f && stoveCounter.IsFried())
            {
                float warning_sound_timer_max = 0.2f; // Play sound every second
                warning_sound_timer = warning_sound_timer_max;

                SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
            }
        }
    }
}
