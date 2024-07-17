using TMPro;
using UnityEngine;

namespace Game.View
{
    /// <summary>
    /// This is a class from a board game project. It represents a token on the board.
    /// I added it to showcase usage of MaterialPropertyBlock and some basic code structure.
    /// IHoverable interface is added to the bottom of the file for clarity.
    /// </summary>
    public class TokenView : MonoBehaviour, IHoverable
    {
        [SerializeField] MeshRenderer _textureMesh;
        [SerializeField] MeshRenderer _highlightMesh;
        [SerializeField] Canvas _amountCanvas;
        [SerializeField] TextMeshProUGUI _amountText;

        public Token Token { get; private set; }
        public Player Owner { get; private set; }
        public RegionView Region { get; private set; }
        public bool IsPointerOver { get; private set; }
        public bool IsHoveredOver { get; private set; }

        void Awake()
        {
            _gameObject = gameObject;
            _transform = transform;
            _materialPropertyBlock = new MaterialPropertyBlock();
            _highlightable = _highlightMesh != null;
        }

        public void Init(Token token)
        {
            Token = token;
            Owner = token is PlayerToken pt ? pt.Owner : null;
            Region = token.Region.Value;
        }

        public void Place(RegionView region, Vector3 position, bool active)
        {
            Region = region;
            _transform.position = position;
            _gameObject.SetActive(active);
        }

        public void SetPointerOver(bool value)
        {
            IsPointerOver = value;

            if (_highlightable)
            {
                var color = value ? _highlightColor : Color.clear;
                _highlightMesh.GetPropertyBlock(_materialPropertyBlock);
                _materialPropertyBlock.SetColor(_materialColorId, color);
                _highlightMesh.SetPropertyBlock(_materialPropertyBlock);
            }
        }

        public void SetHover(bool value)
        {
            IsHoveredOver = value;
        }

        public void SetTexture(Texture texture)
        {
            _textureMesh.GetPropertyBlock(_materialPropertyBlock, 1);
            _materialPropertyBlock.SetTexture(_materialTextureId, texture);
            _textureMesh.SetPropertyBlock(_materialPropertyBlock, 1);
        }

        public void SetBackTexture(Texture texture)
        {
            _textureMesh.GetPropertyBlock(_materialPropertyBlock, 2);
            _materialPropertyBlock.SetTexture(_materialTextureId, texture);
            _textureMesh.SetPropertyBlock(_materialPropertyBlock, 2);
        }

        public void SetAmount(int amount)
        {
            if (amount < 2)
            {
                _amountCanvas.enabled = false;
                return;
            }
            _amountCanvas.enabled = true;
            _amountText.text = amount.ToString();
        }

        public void Flip()
        {
            _textureMesh.transform.Rotate(180, 0, 180);
        }

        bool _highlightable;
        GameObject _gameObject;
        Transform _transform;
        MaterialPropertyBlock _materialPropertyBlock;
        readonly Color _highlightColor = new Color(1, 1, 1, 0.2f);
        readonly int _materialColorId = Shader.PropertyToID("_BaseColor");
        readonly int _materialTextureId = Shader.PropertyToID("_BaseMap");
    }

    public interface IHoverable
    {
        bool IsHoveredOver { get; }
        void SetPointerOver(bool value);
        void SetHover(bool value);
    }
}