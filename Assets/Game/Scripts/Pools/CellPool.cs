using Game.Scripts.Mono;
using Zenject;

namespace Game.Scripts.Pools
{
    public class CellPool : MonoMemoryPool<Cell>
    {
        protected override void OnCreated(Cell item)
        {
            item.gameObject.SetActive(false);
        }

        protected override void OnSpawned(Cell item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Cell item)
        {
            item.gameObject.SetActive(false);
        }
    }
}