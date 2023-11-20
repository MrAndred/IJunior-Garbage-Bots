using System.Collections;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit[] _units;
    [SerializeField] private Transform _resourcesParent;

    private float _extractPeriod = 0.5f;
    private bool _isWorking = false;
    private int _maxResourcesCount = 10;
    private int _resourcesCount = 0;

    private void Start()
    {
        _isWorking = true;
        InitUnits();
        StartCoroutine(ExtractByPeriod());
    }

    private void InitUnits()
    {
        foreach (var unit in _units)
        {
            unit.Init();
        }
    }

    private IEnumerator ExtractByPeriod()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(_extractPeriod);

        while (_isWorking == true && _resourcesCount < _maxResourcesCount)
        {
            StartExtraction();
            yield return waitSeconds;
        }
    }

    private void StartExtraction()
    {
        Unit unit = GetFreeUnit();

        if (unit == null)
        {
            return;
        }

        Resource resource = GetResource();

        if (resource == null)
        {
            return;
        }

        StartCoroutine(Extract(unit, resource));
    }

    private IEnumerator Extract(Unit unit, Resource resource)
    {
        Vector3 unitOriginPosition = unit.transform.position;
        unit.SetIsBusy(true);
        resource.SetIsExtracting(true);

        yield return unit.MoveTo(resource.transform.position);
        yield return unit.ExtractResource(resource);
        yield return unit.MoveTo(unitOriginPosition);
        yield return unit.UnloadResource(resource, transform.position);

        _resourcesCount++;
        Destroy(resource.gameObject);
        unit.SetIsBusy(false);
    }

    private Resource GetResource()
    {
        if (_resourcesParent == null || _resourcesParent.childCount == 0)
        {
            return null;
        }

        Resource[] resources = _resourcesParent.GetComponentsInChildren<Resource>();

        foreach (Resource resource in resources)
        {
            if (resource.IsExtracting == false)
            {
                return resource;
            }
        }

        return null;
    }

    private Unit GetFreeUnit()
    {
        foreach (var unit in _units)
        {
            if (unit.IsBusy == false)
            {
                return unit;
            }
        }

        return null;
    }
}
