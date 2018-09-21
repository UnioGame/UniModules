using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContextProvider
{

    TData GetContext<TData>() where TData : class;

}
