using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using productscategories.Models;

namespace productscategories.Controllers;

public class CategorieController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context; 

    public CategorieController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }
    [HttpGet]
    [Route("categories")]
    public IActionResult Index()
    {
        List<Categorie> AllCategories = _context.Categories.ToList();
        MyCategoryViewModel viewModel = new MyCategoryViewModel();
        viewModel.AllCategories  = AllCategories;
        return View(viewModel);
    }
    [Route("categories/{id}")]
      public IActionResult Categories(int id)
    {   Categorie ? _categorie = _context.Categories.FirstOrDefault( cat => cat.CategoryId == id);
        HttpContext.Session.SetString("CategoryName", _categorie.Name);
        HttpContext.Session.SetInt32("CategoryId",_categorie.CategoryId);

        LProductViewModel _lproductviewmodel = new LProductViewModel();


        List<Product>  Allproducts = _context.Categories
    .Where(cat => cat.CategoryId == id).Include (cat => cat.Asssociations).ThenInclude(asso => asso.Product).SelectMany(prod => prod.Asssociations)
    .Select(assoc => assoc.Product)
    .ToList();
        
        List<Product>  Allnoneproducts = _context.Products
    .Where(product => !_context.Asssociations
        .Any(assoc => assoc.CategoryId == id && assoc.ProductId == product.ProductId))
    .ToList();
        
        
        _lproductviewmodel.AllCategoriesProduct = Allproducts;
        _lproductviewmodel.AllNoneCategoriesProduct = Allnoneproducts;



        return View("Categories", _lproductviewmodel);
    }

    public IActionResult Addproduct([Bind("ProductId")] int ProductId){
        int id = (int)HttpContext.Session.GetInt32("CategoryId");
         LProductViewModel _lproductviewmodel = new LProductViewModel();
     
        if(ModelState.IsValid){
        Asssociation _association = new Asssociation();
        _association.CategoryId = id;
        _association.ProductId = ProductId;
        _context.Add(_association);
        _context.SaveChanges();
           List<Product>  Allproducts = _context.Categories
        .Where(cat => cat.CategoryId == id).Include (cat => cat.Asssociations).ThenInclude(asso => asso.Product).SelectMany(prod => prod.Asssociations)
        .Select(assoc => assoc.Product)
        .ToList();
        
        List<Product>  Allnoneproducts = _context.Products
        .Where(product => !_context.Asssociations
        .Any(assoc => assoc.CategoryId == id && assoc.ProductId == product.ProductId))
        .ToList();
        
        _lproductviewmodel.AllCategoriesProduct = Allproducts;
        _lproductviewmodel.AllNoneCategoriesProduct = Allnoneproducts;
        Console.WriteLine("el estado de model state es" +ModelState.IsValid);
       
        return View("Categories", _lproductviewmodel);
        }
        
        else{
            return View("Categories", _lproductviewmodel);
        }
        
         


    }
    [HttpPost]
    public IActionResult Create(Categorie _categorie){
        
        

        List<Categorie> AllCategories = _context.Categories.ToList();
        MyCategoryViewModel viewModel = new MyCategoryViewModel();
        viewModel.AllCategories  = AllCategories;
        viewModel.Categorie = _categorie;
        if(ModelState.IsValid){
            _context.Add(_categorie);
            _context.SaveChanges();
           return RedirectToAction("Index");
            
        }else{
            return View("Index", viewModel);
        }
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
