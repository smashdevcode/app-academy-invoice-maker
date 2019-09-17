﻿using InvoiceMaker.FormModels;
using InvoiceMaker.Models;
using InvoiceMaker.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InvoiceMaker.Controllers
{
    public class ClientsController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            var repository = new ClientRepository();
            IList<Client> clients = repository.GetClients();
            return View("Index", clients);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var formModel = new CreateClient();
            formModel.IsActivated = true;
            return View("Create", formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateClient formModel)
        {
            var repository = new ClientRepository();

            try
            {
                var client = new Client(0, formModel.Name, formModel.IsActivated);
                repository.Insert(client);
                return RedirectToAction("Index");
            }
            catch (SqlException se)
            {
                if (se.Number == 2627)
                {
                    ModelState.AddModelError("Name", "That name is already taken.");
                }
            }

            return View("Create", formModel);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var repository = new ClientRepository();
            Client client = repository.GetClient(id);

            var formModel = new EditClient();
            formModel.Id = client.Id;
            formModel.IsActivated = client.IsActive;
            formModel.Name = client.Name;

            return View("Edit", formModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, EditClient formModel)
        {
            var repository = new ClientRepository();

            try
            {
                var client = new Client(id, formModel.Name, formModel.IsActivated);
                repository.Update(client);
                return RedirectToAction("Index");
            }
            catch (SqlException se)
            {
                if (se.Number == 2627)
                {
                    ModelState.AddModelError("Name", "That name is already taken.");
                }
            }

            return View("Edit", formModel);
        }
    }
}