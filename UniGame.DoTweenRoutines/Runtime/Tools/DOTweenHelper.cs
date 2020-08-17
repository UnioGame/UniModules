namespace Helpers
{
    using DG.Tweening;

    public static class DoTweenHelper
    {
        public static void KillSequence(ref Sequence sequence)
        {
            if (sequence != null) {
                sequence.Kill();
                sequence = null;
            }
        }

        public static void KillTween(ref Tween tween)
        {
            if (tween != null) {
                tween.Kill();
                tween = null;
            }
        }
    }
}