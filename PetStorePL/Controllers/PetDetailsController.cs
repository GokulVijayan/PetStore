using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetStoreBL.Services;
using Common.ViewModel;
using Common;
using System.Net;
using System.IO;
using PagedList;
using PetStorePL.ViewModel;

namespace Common.Controllers
{
    public class PetDetailsController : Controller
    {
        private readonly IPetService petService;
        public static int petId;
        public static string imagepath;
        public PetDetailsController()
        {

        }
        public PetDetailsController(IPetService petServ)
        {
            petService = petServ;
        }
        [Authorize]
        public ActionResult Create()
        {

            var pet = GetPetType();
            var petType = new SelectList(pet, "TypeId", "PetType");
            ViewData["pettype"] = petType;
            return View();
        }
        /// <summary>
        /// To list pet type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PetViewModel> GetPetType()
        {
            IEnumerable<PetDto> petdetails = petService.GetType();
            var petType = from g in petdetails
                    select new PetViewModel
                    {
                        PetType = g.PetType,
                        TypeId = g.TypeId
                    };
            return petType.ToList();
        }
        /// <summary>
        /// To create a pet
        /// </summary>
        /// <param name="petdetails"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "Age,ImagePath,BreedType,PetName,Price,PetType,Gender")] PetDetailsViewModel petdetails, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + file.FileName);
                    var filepath = HttpContext.Server.MapPath("~/Images/") + file.FileName;
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".JPG", ".JPEG" };
                    var checkextension = Path.GetExtension(file.FileName).ToLower();
                    if (!allowedExtensions.Contains(checkextension))
                    {
                        
                        ViewBag.ErrorMessage = "Select jpeg or png Image with height and width less than 600";
                    }
                    else
                    {
                        using (System.Drawing.Image image = System.Drawing.Image.FromStream(file.InputStream, true, true))
                        {
                            if (image.Width <= 600 && image.Height <= 600)
                            {
                                PetDetailsDto pett = ConvertToDto(petdetails, file);
                                petService.Save(pett);
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Please Select jpeg or png Image with height and width less than 600";
                    return View(petdetails);
                }
            }
            var pet = GetPetType();
            var petType = new SelectList(pet, "TypeId", "PetType");
            ViewData["pettype"] = petType;
            return View(petdetails);
        }
        /// <summary>
        /// To convert to dto
        /// </summary>
        /// <param name="petdetails"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        PetDetailsDto ConvertToDto(PetDetailsViewModel petdetails, HttpPostedFileBase file)
        {
            PetDetailsDto pet = new PetDetailsDto
            {
                Age = petdetails.Age,
                ImagePath = file.FileName,
                BreedType = petdetails.BreedType,
                PetName = petdetails.PetName,
                Price = petdetails.Price,
                PetType = petdetails.PetType,
                Gender = petdetails.Gender
            };
            return pet;
        }
        [Authorize]
        public ActionResult Index()
        {
            return View(ViewDetails());
        }
        /// <summary>
        /// To View all pet details
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PetDetailsViewModel> ViewDetails()
        {
            IEnumerable<PetDetailsDto> petdetails = petService.FindAll();
            var pet = GetAllPets(petdetails);
            return pet;
        }
        /// <summary>
        /// To search pet according to pettype,breed,age,price
        /// </summary>
        /// <param name="option"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult Search(string pettype, string breedtype, string age, string price, int? PageNo)
        {
            var type = GetPetType();
            var petType = new SelectList(type, "TypeId", "PetType");
            ViewData["pettype"] = petType;

            var pageNumber = (PageNo ?? 1) - 1;
            var totalCount = 0;
            Page p = new Page();
            p.PageNumber = pageNumber;
            p.PageSize = 3;

            p.TotalCount = totalCount;
            ViewBag.pettypes = pettype;
            ViewBag.breedtype = breedtype;
            ViewBag.age = age;
            ViewBag.price = price;
            if ((string.IsNullOrEmpty(breedtype) && (string.IsNullOrEmpty(age) && (string.IsNullOrEmpty(price)))))
            {

                List<PetDetailsViewModel> petdetails = SortByPetType(pettype,p, out totalCount).ToList();
                if (petdetails == null)
                    return View();
                else
                {
                    IPagedList<PetDetailsViewModel> pageOrders = new StaticPagedList<PetDetailsViewModel>(petdetails, pageNumber + 1, 3, totalCount);
                    return View(pageOrders);
                }
            }
            else
            {
                var pet = petService.GetPetDetails(pettype, breedtype, age, price, p, out totalCount);
                var petDetails = GetAllPets(pet).ToList();
                IPagedList<PetDetailsViewModel> pageOrders = new StaticPagedList<PetDetailsViewModel>(petDetails, pageNumber + 1, 3, totalCount);
                return View(pageOrders);

            }
        }
        /// <summary>
        /// To list all pets
        /// </summary>
        /// <param name="petdetails"></param>
        /// <returns></returns>
        public IEnumerable<PetDetailsViewModel> GetAllPets(IEnumerable<PetDetailsDto> petdetails)
        {
            IEnumerable<PetDetailsViewModel> pet = from g in petdetails
                                                  select new PetDetailsViewModel
                                                  {
                                                      Age = g.Age,
                                                      BreedType = g.BreedType,
                                                      PetName = g.PetName,
                                                      ImagePath = g.ImagePath,
                                                      Price = g.Price,
                                                      PetType = g.PetType,
                                                      Gender = g.Gender
                                                  };
            return pet.ToList();
        }
        /// <summary>
        /// To sort by pet type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<PetDetailsViewModel> SortByPetType(string type,Page page, out int totalCount)
        {

            IEnumerable<PetDetailsDto> petDetails = petService.SortByPetType(type, page, out totalCount);
            var pet = GetAllPets(petDetails);
            return pet;

        }
        public ActionResult Find(string petName, string breedType, string operation)
        {
            try
            {
                int? id = petService.GetPetId(petName, breedType);
                if (id == null)
                {
                    return RedirectToAction("Index", "PetDetails");
                }
                else if (operation == "Delete")
                    return RedirectToAction("Delete", "PetDetails", new { id = id });
                else if (operation == "Edit")
                    return RedirectToAction("Edit", "PetDetails", new { id = id });
                else
                    return View();
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "PetDetailsViewModel", "Find"));
            }
        }
        /// <summary>
        /// To delete particular pet
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                petService.DeletePetRecord(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View("Index", new HandleErrorInfo(ex, "PetDetailsViewModel", "Delete"));
            }
        }
        /// <summary>
        /// To edit a pet record
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                var petID = Convert.ToInt32(id);
                PetDetailsViewModel petview = GetPetById(petID);
                imagepath= petview.ImagePath;
                var type = GetPetType();
                petId = petID;
                var petType = new SelectList(type, "TypeId", "PetType", petview.PetType);
                ViewData["pettype"] = petType;
                return View(petview);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PetDetailsViewModel petdetails, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    file.SaveAs(HttpContext.Server.MapPath("~/Images/") + file.FileName);
                    var filePath = HttpContext.Server.MapPath("~/Images/") + file.FileName;
                    PetDetailsDto pet = new PetDetailsDto
                    {
                        Age = petdetails.Age,
                        ImagePath = file.FileName,
                        BreedType = petdetails.BreedType,
                        PetName = petdetails.PetName,
                        Price = petdetails.Price,
                        PetType = petdetails.PetType,
                        Gender = petdetails.Gender
                    };
                    petService.EditPet(pet, petId);
                    return RedirectToAction("Index");
                }
                else if (file == null)
                {
                    PetDetailsDto pet = new PetDetailsDto
                    {
                        Age = petdetails.Age,
                        ImagePath = imagepath,
                        BreedType = petdetails.BreedType,
                        PetName = petdetails.PetName,
                        Price = petdetails.Price,
                        PetType = petdetails.PetType,
                        Gender = petdetails.Gender
                    };
                    petService.EditPet(pet, petId);
                    return RedirectToAction("Index");
                }
            }
            var type = GetPetType();
            var petType = new SelectList(type, "TypeId", "PetType", petdetails.PetType);
            ViewData["pettype"] = petType;
            petdetails.ImagePath = imagepath;
            return View(petdetails);
        }
        /// <summary>
        /// To get pet by their id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PetDetailsViewModel GetPetById(int id)
        {
            PetDetailsDto pet = petService.GetPetById(id);
            PetDetailsViewModel petView = new PetDetailsViewModel
            {
                Age = pet.Age,
                BreedType = pet.BreedType,
                PetName = pet.PetName,
                ImagePath = pet.ImagePath,
                Price = pet.Price,
                PetType = pet.PetType,
                Gender = pet.Gender
            };
            return petView;
        }
    }
}
