using UnityEngine;

public class ResetStaticDataManager : MonoBehaviour
{
    private void Awake()
    {
        // Reset static data for all static classes
        CuttingCounter.ResetStaticData();
        TrashCounter.ResetStaticData();
        BaseCounter.ResetStaticData();
    }
}
