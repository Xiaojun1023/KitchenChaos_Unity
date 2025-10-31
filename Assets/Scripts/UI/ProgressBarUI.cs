using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;

    private IHasProgress hasProgress;

    private void Start()
    {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        if (hasProgress == null)
        {
            Debug.LogError("ProgressBarUI requires a GameObject with IHasProgress interface.");
            return;
        }

        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
        barImage.fillAmount = 0f; // Initialize the progress bar to 0
        Hide(); // Hide the progress bar initially
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progress_normalized;

        if (e.progress_normalized == 0f || e.progress_normalized == 1f)
        {
            Hide(); // Hide the progress bar when there is no progress yet or the progress is complete
        }
        else
        {
            Show(); // Show the progress bar when there is progress
        }
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
       gameObject.SetActive(false);
    }
}
