namespace UniModules.UniGame.DoTweenRoutines.Runtime
{
    using System.Collections;
    using DG.Tweening;

    public static class DOTweenRoutineExtensions
    {
        public static IEnumerator WaitForCompletion(this Tween t)
        {
            while (t.active && !t.IsComplete()) {
                yield return null;
            }
        }

        public static IEnumerator WaitForRewind(this Tween t)
        {
            while (t.active && (!t.playedOnce || t.position * (t.CompletedLoops() + 1) > 0.0)) {
                yield return null;
            }
        }

        public static IEnumerator WaitForKill(this Tween t)
        {
            while (t.active) {
                yield return null;
            }
        }

        public static IEnumerator WaitForElapsedLoop(this Tween t, int elapsedLoops)
        {
            while (t.active && t.CompletedLoops() < elapsedLoops) {
                yield return null;
            }
        }

        public static IEnumerator WaitForPosition(this Tween t, float position)
        {
            while (t.active && t.position * (t.CompletedLoops() + 1) < position) {
                yield return null;
            }
        }

        public static IEnumerator WaitForStart(this Tween t)
        {
            while (t.active && !t.playedOnce) {
                yield return null;
            }
        }
    }
}