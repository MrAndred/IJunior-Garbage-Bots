using UnityEngine;
using UnityEngine.EventSystems;

public class Map : MonoBehaviour, IPointerClickHandler
{
    private BaseBuilder _baseBuilder;
    private Base _baseToExpand;

    public void Init(Base baseTemplate)
    {
        _baseToExpand = null;
    }

    public void SetBase(Base baseToExpand)
    {
        _baseToExpand = baseToExpand;
    }

    private void SetExtendPosition(Vector3 position)
    {
        _baseToExpand.SetExtendPosition(position);
        _baseToExpand = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_baseToExpand == null)
        {
            return;
        }

        Vector3 clickPosition = eventData.pointerCurrentRaycast.worldPosition;
        SetExtendPosition(clickPosition);
    }
}