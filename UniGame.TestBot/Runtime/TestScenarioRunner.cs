using System;
using UniModules.UniGame.Core.Runtime.Interfaces;

namespace UniModules.UniGame.TestBot.Runtime
{
    using System.Collections.Generic;
    using Cysharp.Threading.Tasks;
    using UnityEngine;

    [CreateAssetMenu(menuName = "UniGame/TestBot/" + nameof(TestScenarioRunner), fileName = nameof(TestScenarioRunner))]
    public class TestScenarioRunner : ScriptableObject
    {
        [SerializeReference]
        public List<ITestScenario> scenarios = new List<ITestScenario>();

        [SerializeReference]
        public List<IReportProcessor> reportProcessors = new List<IReportProcessor>();
        
        public async UniTask Execute()
        {
            var report = await CreateTestReport();
            
            //send report
            await ProcessReport(report);
        }

        private async UniTask<TestReport> CreateTestReport()
        {
            var report = new TestReport();

            //execute scenarios
            foreach (var scenario in scenarios)
            {
                var scenarioReport = await scenario.ExecuteAsync();
                report.Add(scenarioReport);
            }

            return report;
        }

        private async UniTask ProcessReport(TestReport report)
        {
            //send report
            foreach (var processor in reportProcessors)
            {
                await processor.ProcessReport(report);
            }
        }
        
    }
    
    public interface IReportProcessor
    {
        public UniTask ProcessReport(TestReport report);
    }

    [Serializable]
    public class TestScenario : ITestScenario
    {
        public UniTask<TestScenarioReport> ExecuteAsync()
        {
            return new UniTask<TestScenarioReport>();
        }
    }


    public interface ITestScenario : IAsyncCommand<TestScenarioReport>
    {
        
    }

    public interface ITestReport
    {
        public void Add(TestScenarioReport report);
    }

    [Serializable]
    public class TestReport : ITestReport
    {
        public List<TestScenarioReport> scenarioReports = new List<TestScenarioReport>();
        
        public void Add(TestScenarioReport report)
        {
            scenarioReports.Add(report);
        }
    }
}
