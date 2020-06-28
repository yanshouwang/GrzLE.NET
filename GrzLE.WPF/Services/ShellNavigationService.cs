using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrzLE.WPF.Services
{
    class ShellNavigationService : INavigationService
    {
        protected IRegionManager RM { get; }

        public ShellNavigationService(IRegionManager rm)
        {
            RM = rm;
        }

        public void Navigate(string uri)
            => RM.RequestNavigate("Shell", uri);
    }
}
