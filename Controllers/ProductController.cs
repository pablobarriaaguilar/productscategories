using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using productscategories.Models;

namespace productscategories.Controllers;

public class ProductController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private MyContext _context; 
    public ProductController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
       List<Product> AllProducts = _context.Products.ToList();
        MyProductViewModel viewModel = new MyProductViewModel();
         viewModel.AllProducts = AllProducts;

         return View(viewModel);
    }

    [Route("products/{id}")]
      public IActionResult Products(int id)
    {

        LCategoryViewModel _lcategoryviewmodel = new LCategoryViewModel();
        Product ? _product = _context.Products.FirstOrDefault(pro => pro.ProductId == id);
        HttpContext.Session.SetString("ProductName", _product.Name);
        HttpContext.Session.SetInt32("ProductId", _product.ProductId);
       // List<Categorie> AllCategories = _context.Categories.ToList();

       List<Categorie> AllCategories = _context.Products
    .Where(prod => prod.ProductId == id)
    .Include(prod => prod.Asssociations) // Cargar las asociaciones
        .ThenInclude(assoc => assoc.Categorie) // Cargar las categorías relacionadas con las asociaciones
    .SelectMany(prod => prod.Asssociations)
    .Select(assoc => assoc.Categorie)
    .ToList();

    List<Categorie> Allnonecategories = _context.Categories
    .Where(category => !_context.Asssociations
        .Any(assoc => assoc.CategoryId == category.CategoryId && assoc.ProductId == id))
    .ToList();

    _lcategoryviewmodel.AllNoneProductCategories = Allnonecategories ;

    _lcategoryviewmodel.AllProductCategories = AllCategories;


        return View("Products",_lcategoryviewmodel);
    }

    public IActionResult AddCategory([Bind("CategoryId")] int CategoryId){
        int id = (int)HttpContext.Session.GetInt32("ProductId");
        Console.WriteLine("el id de categorie es " +CategoryId);
        Asssociation _association = new Asssociation();
        _association.CategoryId = CategoryId;
        _association.ProductId = id;
        Console.WriteLine("el estado de model state es" +ModelState.IsValid);

           foreach (var key in ModelState.Keys)
        {
            var modelStateEntry = ModelState[key];
            if (modelStateEntry.ValidationState == ModelValidationState.Invalid)
            {
                foreach (var error in modelStateEntry.Errors)
                {
                    Console.WriteLine($"Error in property '{key}': {error.ErrorMessage}");
                }
            }
        }
        if(ModelState.IsValid){
        
        _context.Asssociations.Add(_association);
        _context.SaveChanges();

        return RedirectToAction("Products", new { id = id });
        }else{
        return RedirectToAction("Products", new { id = id });
        }
        
    }

    [HttpPost]
    public IActionResult Create(Product _product){
       List<Product> AllProducts = _context.Products.ToList();
        MyProductViewModel viewModel = new MyProductViewModel();
         viewModel.AllProducts = AllProducts;

        if(ModelState.IsValid){
            Console.WriteLine(" es valido");
            
            viewModel.AllProducts = _context.Products.ToList();
             _context.Add(_product);
             _context.SaveChanges();
        return RedirectToAction("Index",_product);
        }else{
            viewModel.AllProducts = _context.Products.ToList();
            foreach (var modelState in ModelState.Values)
{
    foreach (var error in modelState.Errors)
    {
        Console.WriteLine(error.ErrorMessage);
    }
}           viewModel.Product = _product;
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
