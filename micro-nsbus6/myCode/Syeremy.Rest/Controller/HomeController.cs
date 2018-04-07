using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NServiceBus;
using Syeremy.Messages;

namespace Syeremy.Rest.Controller
{
    public class HomeController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly IEndpointInstance _endpoint;

        public HomeController(IEndpointInstance endpoint)
        {
            _endpoint = endpoint;
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            var processOrderCommand = new ProcessOrderCommand
            {
                AddressFrom = "My Home",
                AddressTo = "Work",
                Price =  12212,
                Weight =  12
            };
            
            await _endpoint.Send("elcavernas.queue.orders", processOrderCommand).ConfigureAwait(false);
            
            return  Content("processOrderCommand Sent!!!");
        }
    }
}