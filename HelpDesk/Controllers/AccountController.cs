﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using HelpDesk.Models;
using HelpDesk.Enums;
using System.Data.Entity;

namespace HelpDeskTrain.Controllers
{
    [AllowAnonymous] //Анонимный доступ
    public class AccountController : Controller
    {
        // Авторизация пользователя
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost] // обработка авторизации
        public ActionResult Login(LogViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (ValidateUser(model.UserName, model.Password))
                {
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Request");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Неправильный пароль или логин");
                }
            }
            return View(model);
        }
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Login", "Account");
        }

        private bool ValidateUser(string login, string password)
        {
            bool isValid = false;

            using (HelpdeskContext _db = new HelpdeskContext())
            {
                try
                {
                    User user = (from u in _db.Users
                                 where u.Login == login && u.Password == password
                                 select u).FirstOrDefault();

                    if (user != null)
                    {
                        isValid = true;
                    }
                }
                catch
                {
                    isValid = false;
                }
            }
            return isValid;
            
        }
    }
}

