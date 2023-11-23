using UnityEngine;

public class BaseBuilder
{
    private Base _template;

    public BaseBuilder(Base template)
    {
        _template = template;
    }

    public Base BuildBase(Vector3 position)
    {
        Base newBase = Object.Instantiate(_template, position, Quaternion.identity);
        return newBase;
    }
}
