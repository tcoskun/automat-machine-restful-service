using System;

namespace AutomatMachine.Common.Response
{
    public class StartProcessResponse
    {
        public Guid ProcessId { get; }

        public StartProcessResponse(Guid processId)
        {
            ProcessId = processId;
        }
    }
}
