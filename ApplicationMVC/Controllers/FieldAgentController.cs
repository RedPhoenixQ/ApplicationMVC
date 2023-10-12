using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ApplicationMVC.Models;

namespace ApplicationMVC.Controllers;

public class FieldAgentController : Controller
{
    private readonly ILogger<FieldAgentController> _logger;
    private readonly string? _connection_string;
    private readonly FieldAgentModel? _fieldAgentModel;

    public FieldAgentController(ILogger<FieldAgentController> logger, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _connection_string = contextAccessor?.HttpContext?.Session?.GetString(SessionKeys.ConnectionString);
        _fieldAgentModel = _connection_string != null ? new(_connection_string) : null;
    }

    public IActionResult Index()
    {
        if (_fieldAgentModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        ViewBag.Agents = _fieldAgentModel.GetAll();
        return View();
    }

    public IActionResult Details(string name, int nr)
    {
        if (_fieldAgentModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        ViewBag.Agent = _fieldAgentModel.GetOne(name, nr);
        ViewBag.Operations = _fieldAgentModel.GetOperations(name, nr);
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

