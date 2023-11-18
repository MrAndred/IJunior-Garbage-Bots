using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _extractingTime;
    [SerializeField] private float _stopDistance;

    private bool _isBusy;
    private bool _hasExtractedResource;

    public bool IsBusy => _isBusy;

    public void SetIsBusy(bool isBusy)
    {
        _isBusy = isBusy;
    }

    public IEnumerator MoveTo(Vector3 targetPosition)
    {
        _isBusy = true;

        while (Vector3.Distance(transform.position, targetPosition) > _stopDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            yield return null;
        }
    }

    public IEnumerator ExtractResource(Resource resource)
    {
        while (_hasExtractedResource == false)
        {
            if (resource.transform.position == transform.position)
            {
                _hasExtractedResource = true;
            }

            resource.transform.position = Vector3.MoveTowards(resource.transform.position, transform.position, _speed * Time.deltaTime);
            yield return null;
        }

        resource.transform.parent = transform;
    }

    public IEnumerator UnloadResource(Resource resource, Vector3 targetPosition)
    {
        while (_hasExtractedResource == true)
        {
            if (resource.transform.position == targetPosition)
            {
                _hasExtractedResource = false;
            }

            resource.transform.position = Vector3.MoveTowards(resource.transform.position, targetPosition, _speed * Time.deltaTime);
            yield return null;
        }

        resource.transform.parent = null;
    }
}
