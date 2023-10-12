using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApplicationMVC.Controllers;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost] 
    public IActionResult Index(string username, string password)
    {
        Console.WriteLine($"Login attempt: {username} {password}");
        MySqlConnectionStringBuilder conn_string = new()
        {
            Database = "a22teoka",
            Server = "localhost",
            Port = 3308,
            Pooling = false,
            //SslMode = MySqlSslMode.Disabled,
            ConvertZeroDateTime = true,
            UserID = username,
            Password = password
        };
        Console.WriteLine($"connection string: {conn_string}");

        try
        {
            MySqlConnection conn = new(conn_string.ToString());
            conn.Open();
            HttpContext.Session.SetString(SessionKeys.Username, username);
            HttpContext.Session.SetString(SessionKeys.ConnectionString, conn_string.ToString());
        } catch (MySqlException ex)
        {
            Console.WriteLine(ex);
            ViewBag.error = "Username or password is incorrect\n" + ex.Message;
            return View();
        }
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }
}

