using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using FireOnWheels.Messages;
using NServiceBus;
using Order = FireOnWheels.Web.Models.Order;

namespace FireOnWheels.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEndpointInstance endpoint;

        public HomeController(IEndpointInstance endpoint)
        {
            this.endpoint = endpoint;
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Index(Order order)
        {
            //Request is an extension method in the NServiceBus.Callbacks NuGet package
            //You also need to assign a unique id to the endpoint
            var priceResponse = await endpoint.Request<PriceResponse>(new PriceRequest {Weight = order.Weight});
            order.Price = priceResponse.Price;
            return View("Review", order);
        }

        public async Task<ActionResult> Confirm(Order order)
        {
            await endpoint.Send(new ProcessOrderCommand
            {
                OrderId = Guid.NewGuid(),
                AddressFrom = order.AddressFrom,
                AddressTo = order.AddressTo,
                Price = order.Price,
                Weight = order.Weight
            }).ConfigureAwait(false);

            return View();
        }
    }
}
