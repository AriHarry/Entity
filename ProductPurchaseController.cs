using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LoginInMVC4WithEF.Models;
using System.Data.Entity;
using System.Data;
using System.Data.SqlClient;

namespace LoginInMVC4WithEF.Controllers
{
    public class ProductPurchaseController : Controller
    {
        //
       [OutputCache(CacheProfile = "CacheFor20sec")]
        public ActionResult Index(string sortOrder, string searchString)
        {
            CMPSEntities5 dbcontext = new CMPSEntities5();
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
              
              List<ClsProduct> clsproduct = new List<ClsProduct>();
              var product = (from cus in dbcontext.Product_Details
                                join dep in dbcontext.Products on cus.ProductId equals dep.Id
                                select new ClsProduct
                                {
                                    Id = cus.Id,
                                    ModelPlace = cus.ModelPlace,
                                    Rate = cus.Rate,
                                    Description = dep.Description
                                    //Description =cus.

                                });
              
              if (!String.IsNullOrEmpty(searchString))
              {
                  product = product.Where(s => s.Description.Contains(searchString)
                                         || s.ModelPlace.Contains(searchString) || s.Rate.Contains(searchString));
              }
              switch (sortOrder)
              {
                  case "Description":
                      product = product.OrderByDescending(s => s.Description);
                      break;
                  case "ModelPlace":
                      product = product.OrderBy(s => s.ModelPlace);
                      break;
                  case "Rate":
                      product = product.OrderByDescending(s => s.Rate);
                      break;
                  default:
                      product = product.OrderBy(s => s.Description);
                      break;
              }
              return View(product.ToList());
        }
    
        public ActionResult Create()
        {
            ClsProduct objProject = new ClsProduct();
            using (CMPSEntities5 dbcontext = new CMPSEntities5())
            {
                objProject.lstProduct = dbcontext.Products.Select(a => new Department() { ProductId = a.Id, Description = a.Description }).ToList();
            }

            return View(objProject);
        }
        public JsonResult InsertProducts(List<Product_Details> products)
        {
            using (CMPSEntities5 entities = new CMPSEntities5())
            {
                //Truncate Table to delete all old records.
                //entities.Database.ExecuteSqlCommand("TRUNCATE TABLE [product]");


                //Check for NULL.
                if (products == null)
                {
                    products = new List<Product_Details>();
                }

                //Loop and insert records.
                foreach (Product_Details Product in products)
                {
                    entities.Product_Details.Add(Product);
                }

                int insertedRecords = entities.SaveChanges();
                return Json(insertedRecords);
            }
        }

        [HttpGet]
        public ActionResult CreateCustomer()
        {
            Product_Details objProject = new Product_Details();
             
            CMPSEntities5 dbcontext = new CMPSEntities5();

            return PartialView("InsertCustomers", dbcontext.Product_Details);
        }

        [HttpPost]
        public ActionResult Create(ClsProduct model)
        {
            using (CMPSEntities5 dbcontext = new CMPSEntities5())
            {
                if (model.Id == 0)
                {

                    Product_Details tblCustomer = new Product_Details();
                    tblCustomer.ModelPlace = model.ModelPlace;
                    tblCustomer.Rate = model.Rate;
                    tblCustomer.ProductId = model.ProductId;
                    dbcontext.Product_Details.Add(tblCustomer);
                    dbcontext.SaveChanges();
                }
            }
            return RedirectToAction("Index");
        }
        public ActionResult Edit(int Id)
        {
            ClsProduct objProject = new ClsProduct();
            using (CMPSEntities5 dbcontext = new CMPSEntities5())
            {

                objProject = (from cus in dbcontext.Product_Details
                              where cus.Id == Id
                              select new ClsProduct
                              {
                                  Id = cus.ProductId,
                                  ModelPlace = cus.ModelPlace,
                                  ProductId = cus.ProductId,
                                  Rate = cus.Rate

                              }).FirstOrDefault();
                objProject.lstProduct = dbcontext.Products.Select(a => new Department() { ProductId = a.Id, Description = a.Description }).ToList();
            }
            return View(objProject);
        }


        public ActionResult Update(ClsProduct model)
        {
            using (CMPSEntities5 dbcontext = new CMPSEntities5())
            {
                if (model.Id > 0)
                {

                    Product_Details tblCustomer = dbcontext.Product_Details.Where(a => a.Id == model.Id).FirstOrDefault();
                    if (tblCustomer != null)
                    {
                        tblCustomer.ModelPlace = model.ModelPlace;
                        tblCustomer.Rate = model.Rate;
                        dbcontext.SaveChanges();
                    }
                }
            }
            return RedirectToAction("Index");

        }
        public ActionResult Details(int Id)
        {
            //List<ClsProduct> clsproduct = new List<ClsProduct>();
            //using (CMPSEntities5 dbcontext = new CMPSEntities5())
            //{
            //    clsproduct = (from cus in dbcontext.Product_Details
            //                  join dep in dbcontext.Products on cus.ProductId equals dep.Id
            //                  select new ClsProduct
            //                  {
            //                      Id = cus.Id,
            //                      ModelPlace = cus.ModelPlace,
            //                      Rate = cus.Rate,
            //                  }).ToList();

            CMPSEntities5 dbcontext = new CMPSEntities5();
            //ClsProduct pro = dbcontext.Products.Single(x => x.Id == Id);
            return View();
            }
            
        
    }
}
