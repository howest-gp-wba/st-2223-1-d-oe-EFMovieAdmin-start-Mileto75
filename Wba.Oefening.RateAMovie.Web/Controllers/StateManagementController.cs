using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Wba.Oefening.RateAMovie.Web.Controllers
{
    public class StateManagementController : Controller
    {
        public IActionResult Index()
        {
            //create a cookie
            Response.Cookies.Append("username", "test@test.com",
                new CookieOptions
                {
                    Expires = DateTime.Now.AddMinutes(5)
                });
            //create a session
            HttpContext.Session.SetString("username", "test@com.be");
            //create tempdata
            TempData["message"] = "This is a tempdata message!";
            return View();
        }
        public IActionResult ShowSession()
        {
            var username = HttpContext.Session.GetString("username");
            return View("ShowSession",username);
        }
        public IActionResult ShowCookies()
        {
            //get the ophalen
            var username = Request.Cookies["username"];
            return View("showcookies",username);
        }
        public IActionResult DeleteCookie()
        {
            //delete the username cookie
            Response.Cookies.Delete("username");
            return View();
        }
        public IActionResult DeleteSession()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("ShowSession");
        }
        public IActionResult ShowTempMessage()
        {
            return View();
        }
        public IActionResult ComplexSession()
        {
            List<SelectListItem> items
                = new List<SelectListItem>
                { 
                    new SelectListItem{Value = "1",Text="één" },
                    new SelectListItem{Value = "2",Text="twee" },
                    new SelectListItem{Value = "3",Text="drie" },
                    new SelectListItem{Value = "4",Text="vier" },
                };
            var serializedItems = JsonConvert.SerializeObject(items);
            HttpContext.Session.SetString("items", serializedItems);
            return View();
        }
        public IActionResult ReadComplexSession()
        {
            var serializedItems = HttpContext.Session
                .GetString("items");
            var items = JsonConvert
                .DeserializeObject<List<SelectListItem>>(serializedItems);
            return View();
        }
    }
}
