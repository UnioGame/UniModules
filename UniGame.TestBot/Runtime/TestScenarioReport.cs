using System.Collections.Generic;

namespace UniModules.UniGame.TestBot.Runtime
{
    using System;
    
    [Serializable]
    public class TestScenarioReport
    {
        public bool complete;
    }


    [Serializable]
    public class TestCase
    {
        public string name = string.Empty;
        public List<TestResult> results = new List<TestResult>();
    }

    [Serializable]
    public class TestResult
    {
        public string exception;
        public bool success;
        public string result;
        public string stacktrace;
    }
}
