using UnityEngine;

namespace Game.Scripts.Mono
{
    public class Cell : MonoBehaviour
    {
        #region Serialized Fields

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private SpriteRenderer xSpriteRenderer;

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

        #endregion
    }
}