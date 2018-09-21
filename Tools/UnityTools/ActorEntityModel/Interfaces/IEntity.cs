using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Common;
using UnityEngine;

namespace Tools.ActorModel
{
    public interface IEntity : IUpdatable, IContextProvider
    {
        int Id { get; }

        void SetState(bool state);
    }
}