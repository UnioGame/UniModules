using System.Collections;
using System.Collections.Generic;
using UniRx.Async;
using UnityEngine;

public interface IUniTaskExecutor
{

    UniTask Execute(UniTask actionTask);

    void Stop();

}
