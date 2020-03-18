using UniGreenModules.UniCore.Runtime.ProfilerTools;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UniGreenModules.UniCore.EditorTools.Editor.BuildTools
{
    using global::UniCore.Runtime.ProfilerTools;

    public class BuildReportUtils  {


        public static void PrintBuildReport(BuildReport buildReport)
        {

            PrintSummary(buildReport.summary);
            PrintBuildSteps(buildReport.steps);

        }

        private static void PrintSummary(BuildSummary summary)
        {

            Debug.Log("SUMMARY=========================");

            Debug.LogFormat("RESULT : {0}", summary.result);
            Debug.LogFormat("OUTPUT : {0} \n[start time: {1} at end time:{2}]",
                summary.outputPath, summary.buildStartedAt, summary.buildEndedAt);
            Debug.LogFormat("DURATION {0}", summary.totalTime.TotalSeconds);
            Debug.LogFormat("SIZE : {0}", summary.totalSize);

            Debug.LogFormat("ERRORS : {0} WARNINGS {1}", summary.totalErrors, summary.totalWarnings);

            Debug.Log("================================");
        }

        public static void PrintBuildSteps(BuildStep[] steps) {
            Debug.Log("BUILD STEPS=========================");
            for (int i = 0; i < steps.Length; i++) {
                var step     = steps[i];
                var messages = step.messages;
                Debug.LogFormat("\tbuild step {0} duration {1}",step.name,step.duration);
                for (int j = 0; j < messages.Length; j++) {
                    var message = messages[j];
                    GameLog.EditorLogFormat(message.type,"[{0}] content: {1}",message.type,message.content);
                }
            }
            Debug.Log("================================");
        }

        public static void PrintBuildErrors(BuildReport buildReport) {

        }

    }
}
