using System.Collections;
using UnityEngine;

public class UnitSender
{
    private float _sendSpeed;

    public UnitSender(float sendSpeed)
    {
        _sendSpeed = sendSpeed;
    }

    public IEnumerator UnloadResource(Resource resource, Vector3 targetPosition)
    {
        while (resource.transform.position != targetPosition)
        {
            resource.transform.position = Vector3.MoveTowards(resource.transform.position, targetPosition, _sendSpeed * Time.deltaTime);
            yield return null;
        }

        resource.transform.parent = null;
    }
}
