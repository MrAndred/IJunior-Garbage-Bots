using System.Collections;
using UnityEngine;

public class UnitExtractor
{
    private Unit _unit;
    private float _speed;

    public UnitExtractor(Unit unit, float speed)
    {
        _unit = unit;
        _speed = speed;
    }

    public IEnumerator ExtractResource(Resource resource)
    {
        while (resource.transform.position != _unit.transform.position)
        {
            resource.transform.position = Vector3.MoveTowards(resource.transform.position, _unit.transform.position, _speed * Time.deltaTime);
            yield return null;
        }

        resource.transform.parent = _unit.transform;
    }
}
