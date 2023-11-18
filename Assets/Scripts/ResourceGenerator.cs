using System.Collections;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Resource _tempalte;
    [SerializeField] private Transform _parent;

    [Header("Generate positions")]
    [SerializeField] private float _minPositionX;
    [SerializeField] private float _maxPositionX;
    [SerializeField] private float _minPositionZ;
    [SerializeField] private float _maxPositionZ;

    private float _defaultPositionY = 0.5f;
    private float _timeToGenerateResource = 3f;
    private bool _isGenerating = false;


    private void Start()
    {
        _isGenerating = true;
        StartCoroutine(GenerateResource());
    }

    private IEnumerator GenerateResource()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(_timeToGenerateResource);

        while (_isGenerating == true)
        {
            float positionX = Random.Range(_minPositionX, _maxPositionX);
            float positionZ = Random.Range(_minPositionZ, _maxPositionZ);

            Resource resource = Instantiate(_tempalte, new Vector3(positionX, _defaultPositionY, positionZ), Quaternion.identity, _parent);

            yield return waitSeconds;
        }

    }
}
