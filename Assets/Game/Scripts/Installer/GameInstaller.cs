using Game.Scripts.Enums;
using Game.Scripts.Mono;
using Game.Scripts.Pools;
using MessagePipe;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Installer
{
    public class GameInstaller : MonoInstaller
    {
        public Cell cellPrefab;

        public override void InstallBindings()
        {
            #region Pool Bindings

            Container.BindMemoryPool<Cell, CellPool>()
                .WithInitialSize(25) 
                .FromComponentInNewPrefab(cellPrefab) 
                .UnderTransformGroup("CellPool");             

            #endregion
            
            #region MessagePipeOptions
            var options = Container.BindMessagePipe();
            Container.BindMessageBroker<GeneralEvents,object>(options);
            #endregion 
        }
    }
}
