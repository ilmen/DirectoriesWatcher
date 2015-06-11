using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DirectoriesWatcher.Classes
{
    #region Fake UI container
    public class FakeContainer : IContainer
    {
        ComponentCollection components;

        public FakeContainer()
        {
            components = new ComponentCollection(new IComponent[] { });
        }

        public void Add(IComponent component, string name) { }

        public void Add(IComponent component) { }

        public ComponentCollection Components
        {
            get
            {
                return components;
            }
        }

        public void Remove(IComponent component) { }

        public void Dispose() { }
    }
    #endregion
}
