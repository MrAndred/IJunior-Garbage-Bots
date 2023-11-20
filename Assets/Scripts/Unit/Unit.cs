using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _extractionSpeed;
    [SerializeField] private float _stopDistance;

    private UnitMover _unitMover;
    private UnitExtractor _unitExtractor;
    private UnitSender _unitSender;

    private bool _isBusy;
    private bool _hasExtractedResource;

    public bool IsBusy => _isBusy;

    public void Init()
    {
        _isBusy = false;
        _unitMover = new UnitMover(this, _speed, _stopDistance);
        _unitExtractor = new UnitExtractor(this, _extractionSpeed);
        _unitSender = new UnitSender(_extractionSpeed);
    }

    public void SetIsBusy(bool isBusy)
    {
        _isBusy = isBusy;
    }

    public Coroutine MoveTo(Vector3 targetPosition)
    {
        return StartCoroutine(_unitMover.MoveTo(targetPosition));
    }

    public Coroutine ExtractResource(Resource resource)
    {
        return StartCoroutine(_unitExtractor.ExtractResource(resource));
    }

    public Coroutine UnloadResource(Resource resource, Vector3 targetPosition)
    {
        return StartCoroutine(_unitSender.UnloadResource(resource, targetPosition));
    }
}
