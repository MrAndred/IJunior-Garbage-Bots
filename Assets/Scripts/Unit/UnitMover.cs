using System.Collections;
using UnityEngine;

public class UnitMover
{
    private float _stopDistance;
    private float _speed;
    private Unit _unit;

    public UnitMover(Unit unit, float speed, float stopDistance)
    {
        _unit = unit;
        _speed = speed;
        _stopDistance = stopDistance;
    }

    public IEnumerator MoveTo(Vector3 targetPosition)
    {
        while (Vector3.Distance(_unit.transform.position, targetPosition) > _stopDistance)
        {
            _unit.transform.position = Vector3.MoveTowards(_unit.transform.position, targetPosition, _speed * Time.deltaTime);

            yield return null;
        }
    }
}
