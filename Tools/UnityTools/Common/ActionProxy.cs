using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Tools.Utils;

namespace Assets.Scripts.Modules.Tools.Common
{
    public class ActionProxy<T> : IPoolable
    {
        private Action _action;

        public void Initialize(Action action) {
            _action = action;
        }

        public virtual void Release() {
            _action?.Invoke();
        }
    }
}
