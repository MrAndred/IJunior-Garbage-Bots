using System.Collections.Generic;
using UnityEngine;

public class GameRoot : MonoBehaviour
{
    [SerializeField] private List<Base> _bases;
    [SerializeField] private Map _map;
    [SerializeField] private Transform _resources;
    [SerializeField] private ResourceGenerator _resourceGenerator;

    [Header("Prefabs")]
    [SerializeField] private Base _baseTemplate;
    [SerializeField] private Unit _unitTemplate;

    private void Start()
    {
        _resourceGenerator.Init();
        InitBases();
    }

    private void InitBases()
    {
        foreach (var baseObject in _bases)
        {
            baseObject.Init(_resources, _map, _baseTemplate, _unitTemplate);
        }
    }
}
