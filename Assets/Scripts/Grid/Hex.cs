using System;
using DefaultNamespace;
using UnityEngine;

namespace Grid
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class Hex : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;
        [SerializeField] private HexData _data;

        private void OnValidate()
        {
            _renderer ??= GetComponent<SpriteRenderer>();
        }

        private void Awake()
        {
            _renderer = GetComponent<SpriteRenderer>();
            // _data = new HexData(_renderer.sprite, _renderer.color);
        }
    
        public void SetColor(Color color)
        {
            _renderer.color = color;
        }
    
        public void SetHex(HexData data)
        {
            _renderer ??= GetComponent<SpriteRenderer>();
            _data = data;
            _renderer.sprite = data.Sprite;
            _renderer.color = data.Color;
        }

        public void ResetHex()
        {
            _renderer.sprite = _data.Sprite;
            _renderer.color = _data.Color;
        }

        public void RemoveHex()
        {
            EventBus.OnHexRemoved?.Invoke(this);
        }
    }
}