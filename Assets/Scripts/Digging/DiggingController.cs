using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DiggingController : MonoBehaviour
{
    [SerializeField] private float _radius = 0.5f, _distance = 2f, _digTime = 1.5f;
    [SerializeField] private Transform _player;
    [SerializeField] private List<Hex> _highlighted = new();

    private IDiggingInput _input;
    private Coroutine _digCoroutine;

    private void Start()
    {
        _input = new DiggingInputPC(_player);
    }

    private void Update()
    {
        if (_input.IsDigging && _digCoroutine == null)
        {
            _digCoroutine = StartCoroutine(DigRoutine());
        }
        else if (!_input.IsDigging && _digCoroutine != null)
        {
            StopCoroutine(_digCoroutine);
            _digCoroutine = null;
            ClearHighlights();
        }
    }

    private IEnumerator DigRoutine()
    {
        float timer = 0f;
        while (true)
        {
            Vector2 digPos = GetDigPosition();
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(digPos, _radius);

            var newHighlights = new List<Hex>();
            foreach (var col in overlaps)
            {
                var hex = col.GetComponent<Hex>();
                if (hex != null)
                    newHighlights.Add(hex);
            }

            UpdateHighlights(newHighlights);

            timer += Time.deltaTime;
            if (timer >= _digTime)
            {
                foreach (var hex in _highlighted)
                {
                    if (hex != null)
                        hex.RemoveHex();
                }

                _highlighted.Clear();
                timer = 0f;
            }

            yield return null;
        }
    }

    private void UpdateHighlights(List<Hex> newList)
    {
        foreach (var hex in _highlighted.Except(newList))
            hex?.ResetHex();

        foreach (var hex in newList.Except(_highlighted))
            hex?.SetColor(Color.red);

        _highlighted = newList;
    }

    private void ClearHighlights()
    {
        foreach (var hex in _highlighted)
            hex?.ResetHex();

        _highlighted.Clear();
    }

    private Vector2 GetDigPosition()
    {
        Vector2 origin = _player.position;
        Vector2 direction = _input.GetDirection();
        Vector2 offset = Vector2.ClampMagnitude(direction, _distance);

        return origin + offset;
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !_input?.IsDigging == true) return;

        Vector2 from = _player.position;
        Vector2 to = GetDigPosition();

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(from, to);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(to, _radius);
    }
}
