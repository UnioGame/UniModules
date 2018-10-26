using System.Collections;
using MagicColoring.RuntimeProfiler;
using UnityEngine;

namespace Modules.UnityToolsModule.Tools.UnityTools.DebugUtils.FPSCounter
{
    public sealed class RuntimeProfiler : IRuntimeProfiler
    {
        private readonly Material material;
        private float[] track;
        private bool enable;
        private int lastIndex = 0;
        private long generation;
        private float targetFrameTime;

        /// <param name="transparency">Target framerate bar transparency</param>
        /// <param name="heightNormalize">Target framerate bar height percent from half of screen height</param>
        public RuntimeProfiler(float transparency, float heightNormalize)
        {
            Shader shader = Shader.Find("Custom/Profile/MemoryProfiler");
            material = new Material(shader);
            material.hideFlags = HideFlags.DontSave;
            track = new float[Screen.width];
            material.SetFloat("_Texel", 2f / Screen.width);
            material.SetFloat("_Alpha", transparency);
            material.SetFloat("_Height", heightNormalize);
            enable = true;
            targetFrameTime = 1f / Application.targetFrameRate;
        }

        public IEnumerator StartFrameRateMonitor()
        {
            while (enable)
            {
                yield return new WaitForEndOfFrame();
                track[lastIndex] = targetFrameTime / UnityEngine.Time.unscaledDeltaTime;
                lastIndex++;
                if (lastIndex >= track.Length) lastIndex = 0;

                material.SetPass(0);
                GL.Begin(GL.LINES);
                MapArrayUnsafe();
                GL.End();
            }
        }

        public void Dispose()
        {
            enable = false;
            if (material != null)
            {
                GameObject.DestroyImmediate(material);
            }
        }

        private void MapArrayUnsafe()
        {
            unsafe
            {
                int index = 0;
                fixed (float* itpr = &track[0])
                {
                    while (index < track.Length)
                    {
                        GL.Vertex(new Vector3(index, 0));
                        GL.Vertex(new Vector3(index, (lastIndex == index ? 2 : 1) * *(itpr + index)));
                        index++;
                    }
                }
            }
        }

    }
}
