using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ApplicationMVC.Models;
using System.Xml.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplicationMVC.Controllers;

public class OperationController : Controller
{
    private readonly ILogger<OperationController> _logger;
    private readonly string? _connection_string;
    private readonly OperationModel? _operationModel;

    public OperationController(ILogger<OperationController> logger, IHttpContextAccessor contextAccessor)
    {
        _logger = logger;
        _connection_string = contextAccessor?.HttpContext?.Session?.GetString(SessionKeys.ConnectionString);
        _operationModel = _connection_string != null ? new(_connection_string) : null;
    }

    public IActionResult Index(string? query)
    {
        if (_operationModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        ViewBag.Operations = query == null ? _operationModel.GetAll() : _operationModel.Search(query);
        ViewBag.Query = query ?? "";
        return View();
    }

    public IActionResult Edit(DateTime date, string codetype, int i_nr, string? error)
    {
        if (_operationModel == null || _connection_string == null)
        {
            return RedirectToAction("Index", "Login");
        }
        ViewBag.Operation = _operationModel.GetOne(date, codetype, i_nr);
        ViewBag.Agents = _operationModel.GetAllAgents(date, codetype, i_nr);
        ViewBag.AllAgents = new FieldAgentModel(_connection_string).GetAll();
        ViewBag.error = error;
        return View();
    }

    public IActionResult Create(int i_nr, string? error)
    {
        if (_operationModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        ViewBag.i_nr = i_nr;
        ViewBag.error = error;
        return View();
    }

    [HttpPost]
    public IActionResult AddAgent(string agent, DateTime date, string codetype, int i_nr)
    {
        if (_operationModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        // Agent name and nr is combined to use the select element that only has one value
        string name = agent[..1];
        int nr = Int32.Parse(agent[1..]);

        string? error = null;
        try
        {
            _operationModel.AddAgent(date, codetype, i_nr, name, nr);
        }
        catch (Exception ex)
        {
            error = ex.Message;
            Console.WriteLine(error);
        }
        return RedirectToAction("Edit", new { date, codetype, i_nr, error });
    }

    [HttpPost]
    public IActionResult RemoveAgent(string name, int nr, DateTime date, string codetype, int i_nr)
    {
        if (_operationModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        string? error = null;
        try
        {
            _operationModel.RemoveAgent(date, codetype, i_nr, name, nr);
        }
        catch (Exception ex)
        {
            error = ex.Message;
            Console.WriteLine(error);
        }
        return RedirectToAction("Edit", new { date, codetype, i_nr, error });
    }

    [HttpPost]
    public IActionResult Complete(DateTime date, string codetype, int i_nr, string success)
    {
        if (_operationModel == null)
        {
            return RedirectToAction("Index", "Login");
        }
        string? error = null;
        try
        {
            _operationModel.Complete(date, codetype, i_nr, success == "on");
        }
        catch (Exception ex)
        {
            error = ex.Message;
            Console.WriteLine(error);
        }
        return RedirectToAction("Edit", new { date, codetype, i_nr, error });
    }

    [HttpPost]
    public IActionResult Create(DateTime date, string codetype, int i_nr, DateTime end_date, string region_name, string region_terrain)
    {
        if (_operationModel == null)
        {
            return RedirectToAction("Index", "Login");
        }

        try
        {
            _operationModel.Create(date, codetype, i_nr, end_date, region_name, region_terrain);
        } catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return RedirectToAction("Create", new {i_nr, error = ex.Message});
        }
        return RedirectToAction("Edit", new { date, codetype, i_nr });
    }
}

