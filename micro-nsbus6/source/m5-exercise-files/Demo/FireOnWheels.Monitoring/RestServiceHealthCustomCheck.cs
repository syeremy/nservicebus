using System;
using System.Threading.Tasks;
using ServiceControl.Plugin.CustomChecks;

namespace FireOnWheels.Monitoring
{
    public class RestServiceHealthCustomCheck: CustomCheck
    {
        public RestServiceHealthCustomCheck(): base("RestServiceHealth", "RestService", TimeSpan.FromSeconds(5))
        {
            
        }

        public override Task<CheckResult> PerformCheck()
        {
            //Ping service
            return CheckResult.Failed("REST service not reachable");
        }
    }
}