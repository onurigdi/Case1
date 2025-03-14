using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.Mono
{
    public class Cell : MonoBehaviour, IPointerClickHandler
    {
        #region Serialized Fields

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer xSpriteRenderer;

        #endregion

        #region Private Fields

        private bool _isMarked;

        #endregion

        #region Properties

        public bool IsMarked => _isMarked;

        #endregion

        #region Public Methods

        public void SetSize(Vector2 size)
        {
            spriteRenderer.size = size;
            xSpriteRenderer.size = size;
            spriteRenderer.transform.localScale = Vector3.one;
            xSpriteRenderer.transform.localScale = Vector3.one;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void ChangeMarked(bool isMarked)
        {
            _isMarked = isMarked;
            xSpriteRenderer.gameObject.SetActive(_isMarked);
        }

        #endregion

        #region Click Detection

        public void OnPointerClick(PointerEventData eventData)
        {
            Debug.Log($"Cell clicked at position: {transform.position}");
            ChangeMarked(true);
        }

        #endregion
    }
}