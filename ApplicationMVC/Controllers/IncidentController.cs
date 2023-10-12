using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationMVC.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplicationMVC.Controllers;

public class IncidentController : Controller
{
    private readonly ILogger<IncidentController> _logger;
    private readonly string? _connection_string;
    private readonly IncidentModel? _incidentModel;

    public IncidentController(ILogger<IncidentController> logger, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _connection_string = contextAccessor?.HttpContext?.Session?.GetString(SessionKeys.ConnectionString);
        _incidentModel = _connection_string != null ? new IncidentModel(_connection_string) : null;
    }

    public IActionResult Index(string? query)
    {
        if (_incidentModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        ViewBag.Incidents = query == null ? _incidentModel.GetAll() : _incidentModel.Search(query);
        return View();
    }

    public IActionResult Details(int nr)
    {
        if (_incidentModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        ViewBag.Incident = _incidentModel.GetOne(nr);
        ViewBag.Operations = _incidentModel.GetOperations(nr);
        return View();
    }
}

