using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modules.UnityToolsModule.Tools.UnityTools.Interfaces;

namespace Assets.Scripts.Tools
{
    public class CompletionSource : ICompletionSource
    {
        public bool IsComplete { get; protected set; }

        public void Complete()
        {
            IsComplete = true;
        }

        public void Dispose()
        {
            IsComplete = false;
        }
    }
}
