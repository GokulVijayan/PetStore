using Common;
using PetStoreBL.Services;
using Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetStorePL.ViewModel;

namespace Common.Controllers
{
    public class DiscountsController : Controller
    {
        // GET: Discounts
        private readonly IDiscountService discountService;
        public DiscountsController()
        {

        }
        public DiscountsController(IDiscountService discount)
        {
            discountService = discount;
        }
        [Authorize]
        public ActionResult Create()
        {
            var pet = GetPetType();
            var petType = new SelectList(pet, "TypeId", "PetType");
            ViewData["pettype"] = petType;
            return View();
        }
        public IEnumerable<PetViewModel> GetPetType()
        {
            IEnumerable<PetDto> petdetails = discountService.GetType();
            var petType = from g in petdetails
                          select new PetViewModel
                          {
                              PetType = g.PetType,
                              TypeId = g.TypeId
                          };
            return petType.ToList();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DiscountViewModel discountdetails)
        {
            if (ModelState.IsValid)
            {
                DiscountDto discount = ConvertToDto(discountdetails);
                discountService.Save(discount);
                return RedirectToAction("Index");
            }
            var pet = GetPetType();
            var petType = new SelectList(pet, "TypeId", "PetType");
            ViewData["pettype"] = petType;
            return View(discountdetails);

        }
        DiscountDto ConvertToDto(DiscountViewModel discountdetails)
        {
            DiscountDto disc = new DiscountDto
            {
                DiscountRate = discountdetails.DiscountRate,
                StartDate = discountdetails.StartDate,
                EndDate = discountdetails.EndDate,
                PetType = discountdetails.PetType
            };
            return disc;
        }
        public ActionResult Index()
        {
            return View(ViewAllDiscount());
        }
        IEnumerable<DiscountViewModel> ViewAllDiscount()
        {
            IEnumerable<DiscountDto> discount = discountService.FindAll();
            var pet = GetAllDiscount(discount);
            return pet;
        }
        IEnumerable<DiscountViewModel> GetAllDiscount(IEnumerable<DiscountDto> discount)
        {
            IEnumerable<DiscountViewModel> pt = from g in discount
                                                select new DiscountViewModel
                                                {
                                                    DiscountRate = g.DiscountRate,
                                                    PetType = g.PetType,
                                                    StartDate = g.StartDate,
                                                    EndDate = g.EndDate
                                                };
            return pt.ToList();
        }
        [Authorize]
        public ActionResult Delete(string pettype, string discount)
        {
            try
            {
                discountService.DeleteDiscount(pettype, discount);
                return RedirectToAction("Index", "Discounts");
            }
            catch (Exception ex)
            {
                return View("Index", new HandleErrorInfo(ex, "Discounts", "Index"));
            }
        }
        public ActionResult Details()
        {
            return View(ViewAllDiscount());
        }
    }
}