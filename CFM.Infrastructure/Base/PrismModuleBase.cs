using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;

namespace CFM.Infrastructure.Base
{
    public class PrismModuleBase: IModule
    {
        public IRegionManager RegionManager { get; private set; }
        public IUnityContainer UnityContainer { get; private set; }

        public virtual void Initialize()
        {
        }

        protected virtual void RegisterViews()
        {
        }

        protected virtual void RegisterViewModels()
        {
        }

        public PrismModuleBase(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            RegionManager = regionManager;
            UnityContainer = unityContainer;
        }

    }
}
