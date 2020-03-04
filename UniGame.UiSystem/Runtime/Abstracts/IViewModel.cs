using System;
using System.Collections.Generic;

namespace Taktika.MVVM.Abstracts
{
    using UniGreenModules.UniCore.Runtime.Interfaces;
    using UniRx;

    public interface IViewModel: IDisposable, ILifeTimeContext
    {
        IReadOnlyReactiveProperty<bool> IsActive { get; }
        
    }
}