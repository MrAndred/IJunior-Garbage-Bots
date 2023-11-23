using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Base : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Transform _baseObject;
    [SerializeField] private List<Unit> _baseUnits;
    [SerializeField] private Transform _baseUnitsParent;
    [SerializeField] private int _newUnitCost;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private ExtendPoint _extendPointTemplate;

    [SerializeField] private Transform _resourcesInProcess;

    private Transform _resourcesParent;
    private Queue<Resource> _resources;
    private Queue<Unit> _units;

    private float _extractPeriod = 0.5f;
    private bool _isWorking = false;
    private int _maxResourcesCount = 10;
    private int _resourcesCount = 0;

    private bool _hasChildBase;

    private Vector3 _extendPosition;
    private ExtendPoint _extendPoint = null;
    private bool _isExtending = false;

    private BaseUnitCreator _baseUnitCreator;
    private BaseBuilder _baseBuilder;
    private int _newBaseCost = 5;

    private Map _map;
    private Base _baseTemplate;
    private Unit _unitTemplate;

    public void Init(Transform resourcesParent, Map map, Base baseTemplate, Unit unitTemplate)
    {
        DefaultInit(resourcesParent, map, baseTemplate, unitTemplate);
    }

    public void Init(Transform resourcesParent, Map map, Base baseTemplate, Unit unitTemplate, List<Unit> units)
    {
        _baseUnits = units;
        DefaultInit(resourcesParent, map, baseTemplate, unitTemplate);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount == 1)
        {
            LoadCreateBaseQuery();
        }
        else
        {
            CreateNewUnit();
        }
    }

    private void InitUnits()
    {
        foreach (var unit in _baseUnits)
        {
            unit.Init(_baseObject.position);
            _units.Enqueue(unit);
            unit.transform.SetParent(_baseUnitsParent);
        }
    }

    private IEnumerator SendResourceCoordinatesByPeriod()
    {
        WaitForSeconds waitSeconds = new WaitForSeconds(_extractPeriod);

        while (_isWorking == true && _resourcesCount < _maxResourcesCount)
        {
            if (_isExtending == true && _resourcesCount >= _newBaseCost)
            {
                BuildNewBase();
            }
            else
            {
                SendCoordinates();
            }

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
            _resources.Enqueue(resource);
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

        if (resource.IsExtracting == true)
        {
            return null;
        }

        resource.transform.SetParent(_resourcesInProcess);
        resource.SetIsExtracting(true);

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

    private void BuildNewBase()
    {
        Unit unit = GetFreeUnit();

        if (unit == null)
        {
            return;
        }

        StartCoroutine(StartBuildingNewBase(unit));
    }

    private IEnumerator StartBuildingNewBase(Unit unit)
    {
        _hasChildBase = true;
        _isExtending = false;
        _resourcesCount -= _newBaseCost;

        yield return StartCoroutine(unit.BuildNewBase(_extendPosition));
        Base newBase = _baseBuilder.BuildBase(_extendPosition);
        newBase.Init(_resourcesParent, _map, _baseTemplate, _unitTemplate, new List<Unit>() { unit });
    }

    private void LoadCreateBaseQuery()
    {
        if (_hasChildBase == true)
        {
            return;
        }

        _map.SetBase(this);
    }

    public void SetExtendPosition(Vector3 extendPosition)
    {
        _extendPosition = extendPosition;

        if (_extendPoint == null)
        {
            ExtendPoint extendPoint = Instantiate(_extendPointTemplate, _extendPosition, Quaternion.identity);
            _extendPoint = extendPoint;
        }

        _extendPoint.transform.position = _extendPosition;
        _isExtending = true;
    }

    private void CreateNewUnit()
    {
        if (_isExtending == true || _resourcesCount < _newUnitCost)
        {
            return;
        }

        Vector2 randomCirclePosition = Random.insideUnitCircle * _spawnRadius;
        Vector3 unitPosition = new Vector3(_baseObject.position.x + randomCirclePosition.x, _baseObject.position.y, _baseObject.position.z + randomCirclePosition.y);

        Unit newUnit = _baseUnitCreator.CreateUnit(unitPosition);
        newUnit.Init(_baseObject.position);
        _units.Enqueue(newUnit);
        _resourcesCount -= _newUnitCost;
    }

    private void DefaultInit(Transform resourcesParent, Map map, Base baseTemplate, Unit unitTemplate)
    {
        _resourcesParent = resourcesParent;
        _map = map;
        _baseTemplate = baseTemplate;
        _unitTemplate = unitTemplate;
        _baseUnitCreator = new BaseUnitCreator(unitTemplate, _baseUnitsParent);
        _baseBuilder = new BaseBuilder(baseTemplate);

        _resources = new Queue<Resource>();
        _units = new Queue<Unit>();
        _isWorking = true;
        _hasChildBase = false;

        InitUnits();
        StartCoroutine(SendResourceCoordinatesByPeriod());
    }
}
