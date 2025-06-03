#if UNITY_EDITOR
using System.Threading.Tasks;
using Common;
using UnityEditor;
using Unity.EditorCoroutines.Editor;
#endif

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using EditorExtensions;
using Grid;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    [SerializeField] private int _width = 10, _height = 15;
    [SerializeField] private float _hexWidth = 1f, _hexHeight = 0.866f;
    
#if UNITY_EDITOR

    [SerializeField] private PoolsKeeper _poolsKeeper;
    [SerializeField] private ObjectPool<Hex> _hexPool;
    [SerializeField, Range(0f, 0.1f)] private float _generationDelay = 0.01f;

    [SerializeField] private List<Hex> _cells = new();
    private EditorCoroutine _generationCoroutine;
    private bool _isClearing, _cancelRequested;

    [EditorButton("Get Hex Pool")]
    private void GetPool()
    {
        _hexPool = _poolsKeeper.GetPool<Hex>();
    }
    
    [EditorButton("Generate")]
    private async Task Generate()
    {
        ClearGrid();
        await HexLibrary.InitializeAsync();
        
        if (_generationCoroutine != null)
            EditorCoroutineUtility.StopCoroutine(_generationCoroutine);

        _cancelRequested = false;
        _generationCoroutine = EditorCoroutineUtility.StartCoroutine(GenerateGridStepByStep(), this);
    }

    [EditorButton("Cancel Generation")]
    private void CancelGeneration()
    {
#if UNITY_EDITOR
        if (_generationCoroutine != null)
        {
            _cancelRequested = true;
            EditorCoroutineUtility.StopCoroutine(_generationCoroutine);
            _generationCoroutine = null;
            EditorUtility.ClearProgressBar();
            Debug.Log("Grid generation cancelled.");
        }
#endif
    }
    
    private IEnumerator GenerateGridStepByStep()
    {
        int total = _width * _height;
        int created = 0;

        try
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (_cancelRequested)
                        yield break;

                    float xOffset = y % 2 == 0 ? 0f : _hexWidth / 2f;
                    float xPos = x * _hexWidth + xOffset;
                    float yPos = -y * _hexHeight;
                    Vector2 pos = new Vector2(xPos, yPos);

                    Hex hex = _hexPool.Get();
                    hex.transform.SetParent(transform);
                    hex.transform.localPosition = pos;
                    hex.name = $"Cell-[{x},{y}]";

                    var hexes = HexLibrary.All.ToList();
                    var selected = HexLibrary.ChooseWeighted(hexes);
                    
                    hex.SetHex(selected);
                    
                    _cells.Add(hex);

                    created++;
                    float progress = (float)created / total;
                    EditorUtility.DisplayProgressBar("Generating Grid", 
                        $"Creating cell {created}/{total}", progress);

                    if (_generationDelay > 0f)
                        yield return new EditorWaitForSeconds(_generationDelay);
                    else
                        yield return null;
                }
            }
        }
        finally
        {
            EditorUtility.ClearProgressBar();
        }
    }
    
    [EditorButton("Clear")]
    private void ClearGrid()
    {
        _isClearing = true;
        _cancelRequested = true;

        if (_generationCoroutine != null)
            EditorCoroutineUtility.StopCoroutine(_generationCoroutine);

        foreach (var cell in _cells)
        {
            if (cell != null)
                _hexPool.Return(cell);
        }

        var childCount = transform.childCount;
        if (childCount > 0)
        {
            for (int i = childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(transform.GetChild(i).gameObject);
            }
        }
        
        _cells.Clear();
        _isClearing = false;

        EditorUtility.ClearProgressBar();
    }

    private void OnValidate()
    {
        if (_isClearing) return;

        if (_cells.Count == 0)
        {
            _cells.Clear();
            foreach (var child in transform)
                _cells.Add(child as Hex);
        }

        if (_cells.Count == 0) return;

        int index = 0;
        for (int y = 0; y < _height; y++)
        {
            for (int x = 0; x < _width; x++)
            {
                if (index >= _cells.Count) return;

                float xOffset = (y % 2 == 0) ? 0f : _hexWidth / 2f;
                float xPos = x * _hexWidth + xOffset;
                float yPos = -y * _hexHeight;
                Vector2 pos = new Vector2(xPos, yPos);

                _cells[index].transform.localPosition = pos;
                index++;
            }
        }
    }
#endif
    
    private void OnEnable()
    {
        EventBus.OnHexRemoved.AddListener(RemoveHex);
    }
        
    private void OnDisable()
    {
        EventBus.OnHexRemoved.RemoveListener(RemoveHex);
    }

    private void RemoveHex(Hex hex)
    {
        _cells.Remove(hex);
    }
}
