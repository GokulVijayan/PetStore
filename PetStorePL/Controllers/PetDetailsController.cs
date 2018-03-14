using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PetStoreBL.Services;
using PetStorePL.ViewModel;
using Common;
using System.Net;

namespace PetStorePL.Controllers
{
    public class PetDetailsController : Controller
    {
        private readonly IPetService transRepos;
        public static int petId;
        public PetDetailsController()
        {

        }
        public PetDetailsController(IPetService transactionRepo)
        {
            transRepos = transactionRepo;
        }
        [Authorize]
        public ActionResult Create()
        {

            var xx = GetPetType();
            var petType = new SelectList(xx, "TypeId", "PetType");
            ViewData["pettype"] = petType;
            return View();
        }
        /// <summary>
        /// To list pet type
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PetViewModel> GetPetType()
        {
            IEnumerable<PetDto> petdetails = transRepos.GetType();
            var x = from g in petdetails
                    select new PetViewModel
                    {
                        PetType = g.PetType,
                        TypeId = g.TypeId
                    };
            return x.ToList();
        }
        [HttpPost]
        [Authorize]
        public ActionResult Create([Bind(Include = "Age,ImagePath,BreedType,PetName,Price,PetType,Gender")] PetDetailsViewModel petdetails, HttpPostedFileBase file)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (file != null)
                    {
                        file.SaveAs(HttpContext.Server.MapPath("~/Images/") + file.FileName);
                        var filepath = HttpContext.Server.MapPath("~/Images/") + file.FileName;
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

                        transRepos.Save(pet);
                        return View("Index");
                    }
                    else
                    {
                        return View(petdetails);
                    }
                }
                var xx = GetPetType();
                var petType = new SelectList(xx, "TypeId", "PetType");
                ViewData["pettype"] = petType;
                return View(petdetails);
            }
            catch (Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "PetDetailsViewModel", "Create"));
            }
        }
        [Authorize]
        public ActionResult Index()
        {
            return View(ViewDetails());
        }
        public IEnumerable<PetDetailsViewModel> ViewDetails()
        {
            IEnumerable<PetDetailsDto> petdetails = transRepos.FindAll();
            var pet = GetAllPets(petdetails);
            return pet;
        }
        /// <summary>
        /// To search pet according to pettype,breed
        /// </summary>
        /// <param name="option"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        public ActionResult Search(String option, String search)
        {
            try
            {
                if (option == null)
                {
                    return View(ViewDetails());
                }
                else if (option == "PetType")
                {
                    if (!string.IsNullOrEmpty(search))
                        return View(SortByPetType(search));
                    else
                    {
                        ModelState.AddModelError("Search", "Please enter pet type");
                        return View(ViewDetails());
                    }
                }
                else if (option == "Breed")
                {
                    if (!string.IsNullOrEmpty(search))
                        return View(SortByBreed(search));
                    else
                    {
                        return View(ViewDetails());
                    }

                }
                else if (option == "All")
                {
                    return View(ViewDetails());
                }
                return View();
            }
            catch(Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "PetDetailsViewModel", "Search"));
            }
        }
        /// <summary>
        /// To sort by breed
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<PetDetailsViewModel> SortByBreed(string type)
        {
            IEnumerable<PetDetailsDto> petdetails = transRepos.SortByBreed(type);
            var pet = GetAllPets(petdetails);
            return pet;
        }
        /// <summary>
        /// To list all pets
        /// </summary>
        /// <param name="petdetails"></param>
        /// <returns></returns>
        public IEnumerable<PetDetailsViewModel> GetAllPets(IEnumerable<PetDetailsDto> petdetails)
        {
            IEnumerable<PetDetailsViewModel> pt = from g in petdetails
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
            return pt.ToList();
        }
        /// <summary>
        /// To sort by pet type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<PetDetailsViewModel> SortByPetType(string type)
        {
            IEnumerable<PetDetailsDto> petdetails = transRepos.SortByPetType(type);
            var pet = GetAllPets(petdetails);
            return pet;

        }
        public ActionResult Find(string petName, string breedType, string operation)
        {
            try
            {
                int? id = transRepos.GetPetId(petName, breedType);
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
            catch(Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "PetDetailsViewModel", "Find"));
            }
        }
        [Authorize]
        public ActionResult Delete(int id)
        {
            try
            {
                transRepos.DeletePetRecord(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View("Error", new HandleErrorInfo(ex, "PetDetailsViewModel", "Delete"));
            }
        }
        [Authorize]
        public ActionResult Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else
            {
                PetDetailsViewModel petview = GetPetById(id);
                TempData["imagepath"] = petview.ImagePath;
                var xx = GetPetType();
                petId = id;
                var petType = new SelectList(xx, "TypeId", "PetType", petview.PetType);
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
                    var filepath = HttpContext.Server.MapPath("~/Images/") + file.FileName;

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
                    transRepos.EditPet(pet, petId);
                    return RedirectToAction("Index");
                }
                else if (file == null)
                {
                    PetDetailsDto pet = new PetDetailsDto
                    {
                        Age = petdetails.Age,
                        ImagePath = Convert.ToString(TempData["imagepath"].ToString()),
                        BreedType = petdetails.BreedType,
                        PetName = petdetails.PetName,
                        Price = petdetails.Price,
                        PetType = petdetails.PetType,
                        Gender = petdetails.Gender
                    };
                    transRepos.EditPet(pet, petId);
                    return RedirectToAction("Index");
                }
            }
            var xx = GetPetType();
            var petType = new SelectList(xx, "TypeId", "PetType", petdetails.PetType);
            ViewData["pettype"] = petType;
            petdetails.ImagePath = Convert.ToString(TempData["imagepath"].ToString());
            return View(petdetails);
        }
        /// <summary>
        /// To get pet by their id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PetDetailsViewModel GetPetById(int id)
        {
            PetDetailsDto pet = transRepos.GetPetById(id);
            PetDetailsViewModel pt = new PetDetailsViewModel
            {
                Age = pet.Age,
                BreedType = pet.BreedType,
                PetName = pet.PetName,
                ImagePath = pet.ImagePath,
                Price = pet.Price,
                PetType = pet.PetType,
                Gender = pet.Gender
            };
            return pt;
        }
    }
}
