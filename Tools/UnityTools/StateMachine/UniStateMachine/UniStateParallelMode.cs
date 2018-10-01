using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools.UniRoutineTask;

namespace UniStateMachine
{
    [Serializable]
    public class UniStateParallelMode
    {
        public UniStateBehaviour StateBehaviour;
        public RoutineType RoutineType = RoutineType.UpdateStep;
    }
}
