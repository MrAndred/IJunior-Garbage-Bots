using UnityEngine;

public class Resource : MonoBehaviour
{
    private bool _isExtracting = false;

    public bool IsExtracting => _isExtracting;

    public void SetIsExtracting(bool isExtracting)
    {
        _isExtracting = isExtracting;
    }
}
