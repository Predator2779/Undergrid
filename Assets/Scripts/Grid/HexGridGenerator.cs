#if UNITY_EDITOR
using UnityEditor;
using Unity.EditorCoroutines.Editor;
#endif

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public GameObject hexPrefab;
    public int width = 10;
    public int height = 15;
    public float hexWidth = 1f;
    public float hexHeight = 0.866f;

#if UNITY_EDITOR
    [Range(0f, 0.1f)]
    public float generationDelay = 0.01f;

    private List<GameObject> _cells = new();
    private bool _isClearing;
    private bool _cancelRequested;
    private EditorCoroutine _generationCoroutine;

    [EditorButton("Generate")]
    private void Generate()
    {
        ClearGrid();

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
        int total = width * height;
        int created = 0;

        try
        {
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (_cancelRequested)
                        yield break;

                    float xOffset = y % 2 == 0 ? 0f : hexWidth / 2f;
                    float xPos = x * hexWidth + xOffset;
                    float yPos = -y * hexHeight;
                    Vector2 pos = new Vector2(xPos, yPos);

                    GameObject hex = PrefabUtility.InstantiatePrefab(hexPrefab, transform) as GameObject;
                    hex.AddComponent<Hex>();
                    hex.transform.localPosition = pos;
                    hex.name = $"Cell-[{x},{y}]";

                    _cells.Add(hex);

                    created++;
                    float progress = (float)created / total;
                    EditorUtility.DisplayProgressBar("Generating Grid", $"Creating cell {created}/{total}", progress);

                    if (generationDelay > 0f)
                        yield return new EditorWaitForSeconds(generationDelay);
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

        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            Undo.DestroyObjectImmediate(child.gameObject);
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
            foreach (Transform child in transform)
                _cells.Add(child.gameObject);
        }

        if (_cells.Count == 0) return;

        int index = 0;
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (index >= _cells.Count) return;

                float xOffset = (y % 2 == 0) ? 0f : hexWidth / 2f;
                float xPos = x * hexWidth + xOffset;
                float yPos = -y * hexHeight;
                Vector2 pos = new Vector2(xPos, yPos);

                _cells[index].transform.localPosition = pos;
                index++;
            }
        }
    }
#endif
}
