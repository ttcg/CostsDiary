using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CostsDiary.Web.Logging
{
    public class LogEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent le, ILogEventPropertyFactory lepf)
        {
            le.RemovePropertyIfPresent("RequestId");
            le.RemovePropertyIfPresent("ConnectionId");
            le.RemovePropertyIfPresent("CorrelationId");
            le.RemovePropertyIfPresent("ActionId");
            le.RemovePropertyIfPresent("ActionName");
            
            le.AddPropertyIfAbsent(lepf.CreateProperty("MachineName", Environment.MachineName));
            le.AddPropertyIfAbsent(lepf.CreateProperty("Application", typeof(LogEnricher).Assembly.GetName().Name));
        }
    }
}
