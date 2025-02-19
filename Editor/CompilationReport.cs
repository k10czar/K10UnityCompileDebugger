using System;
using System.Collections.Generic;

namespace UnityCompilationDebugger
{
    [Serializable]
    internal class CompilationReport
    {
        public double compilationTotalTime;
        public long reloadEventTimes;
		public bool assemblyEvaluated;
		public double assemblyReloadTotalTime;
    }
}