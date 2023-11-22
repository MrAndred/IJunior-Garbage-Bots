using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private Unit[] _baseUnits;
    [SerializeField] private Transform _resourcesInProcess;
    [SerializeField] private Transform _resourcesParent;

    private Queue<Resource> _resources;
    private Queue<Unit> _units;

    private float _extractPeriod = 0.5f;
    private bool _isWorking = false;
    private int _maxResourcesCount = 10;
    private int _resourcesCount = 0;

    public void Start()
    {
        _resources = new Queue<Resource>();
        _units = new Queue<Unit>();
        _isWorking = true;
        InitUnits();
        StartCoroutine(SendResourceCoordinatesByPeriod());
    }

    private void InitUnits()
    {
        foreach (var unit in _baseUnits)
        {
            unit.Init(transform.position);
            _units.Enqueue(unit);
        }
    }

    private IEnumerator SendResourceCoordinatesByPeriod()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(_extractPeriod);

        while (_isWorking == true && _resourcesCount < _maxResourcesCount)
        {
            SendCoordinates();
            yield return waitSeconds;
        }
    }

    private void SendCoordinates()
    {
        Resource resource = GetResource();

        if (resource == null)
        {
            UpdateResources();
            return;
        }

        Unit unit = GetFreeUnit();

        if (unit == null)
        {
            return;
        }

        StartCoroutine(Extract(unit, resource));
    }

    private IEnumerator Extract(Unit unit, Resource resource)
    {
        yield return StartCoroutine(unit.StartExtraction(resource));
        _units.Enqueue(unit);
        _resourcesCount++;
    }

    private void UpdateResources()
    {
        if (_resources.Count > 0)
        {
            return;
        }

        Resource[] resources = _resourcesParent.GetComponentsInChildren<Resource>();

        foreach (Resource resource in resources)
        {
            _resources.Enqueue(resource);
        }
    }

    private Resource GetResource()
    {
        if (_resources.Count == 0)
        {
            return null;
        }

        Resource resource = _resources.Dequeue();
        resource.transform.SetParent(_resourcesInProcess);

        return resource;
    }

    private Unit GetFreeUnit()
    {
        if (_units.Count == 0)
        {
            return null;
        }

        return _units.Dequeue();
    }
}
