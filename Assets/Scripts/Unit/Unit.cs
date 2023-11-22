using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _extractionSpeed;
    [SerializeField] private float _stopDistance;

    private UnitMover _unitMover;
    private UnitExtractor _unitExtractor;
    private UnitSender _unitSender;

    private Vector3 _basePosition;

    public void Init(Vector3 basePosition)
    {
        _basePosition = basePosition;
        _unitMover = new UnitMover(this, _speed, _stopDistance);
        _unitExtractor = new UnitExtractor(this, _extractionSpeed);
        _unitSender = new UnitSender(_extractionSpeed);
    }

    public IEnumerator StartExtraction(Resource resource)
    {
        Vector3 originPosition = transform.position;

        yield return MoveTo(resource.transform.position);
        yield return ExtractResource(resource);
        yield return MoveTo(originPosition);
        yield return UnloadResource(resource, _basePosition);

        Destroy(resource.gameObject);
    }

    private Coroutine MoveTo(Vector3 targetPosition)
    {
        return StartCoroutine(_unitMover.MoveTo(targetPosition));
    }

    private Coroutine ExtractResource(Resource resource)
    {
        return StartCoroutine(_unitExtractor.ExtractResource(resource));
    }

    private Coroutine UnloadResource(Resource resource, Vector3 targetPosition)
    {
        return StartCoroutine(_unitSender.UnloadResource(resource, targetPosition));
    }
}
