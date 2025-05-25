using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MicroStack.Core.Repositories;
using MicroStack.UI.Clients;
using MicroStack.UI.ViewModel;

namespace MicroStack.UI.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ProductClient _productClient;
        private readonly AuctionClient _auctionClient;

        public AuctionController(IUserRepository userRepository, ProductClient productClient, AuctionClient auctionClient)
        {
            _userRepository = userRepository;
            _productClient = productClient;
            _auctionClient = auctionClient;
        }

        public async Task<IActionResult> Index()
        {
            var auctionList = await _auctionClient.GetAuctions();
            if (auctionList.IsSuccess)
                return View(auctionList.Data);
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        { 
            var productList = await _productClient.GetProducts();
            if (productList.IsSuccess)
                ViewBag.ProductList = productList.Data;

            var userList = await _userRepository.GetAllAsync();
            ViewBag.UserList = userList;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AuctionViewModel model)
        {
            model.Status = 1;
            model.CreatedAt = DateTime.Now;
            //model.IncludedSellers.Add(model.SellerId);
            var createAuction = await _auctionClient.CreateAuction(model);
            if (createAuction.IsSuccess)
                return RedirectToAction("Index");
            return View(model);
        }

        public IActionResult Detail()
        {
            return View();
        }
    }
}
