using UnityEngine;

public class BaseUnitCreator
{
    private Unit _unitTemplate;
    private Transform _unitsParent;

    public BaseUnitCreator(Unit unitTemplate, Transform unitsParent)
    {
        _unitTemplate = unitTemplate;
        _unitsParent = unitsParent;
    }

    public Unit CreateUnit(Vector3 position)
    {
        Unit unit = Object.Instantiate(_unitTemplate, position, Quaternion.identity, _unitsParent);
        return unit;
    }
}
