using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FirebaseExample.Models;
using Newtonsoft.Json.Linq;

namespace FirebaseExample.Controllers
{
    public class EmployeesController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "your Key",
            BasePath = "Your url",
        };
        IFirebaseClient cliente;

        public ActionResult Index()
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Employees");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<employee>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<employee>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }


        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(employee employee)
        {
            try
            {
                AddEmployeeFirebase(employee);
                ModelState.AddModelError(string.Empty, "Empleado Añadido");
                return RedirectToAction("Index");
                ;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }


        private void AddEmployeeFirebase(employee employee)
        {
            cliente = new FireSharp.FirebaseClient(config);
            var data = employee;
            PushResponse responde = cliente.Push("Employees/", data);
            data.emp_id = responde.Result.name;
            SetResponse setResponse = cliente.Set("Employees/" + data.emp_id, data);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Employees/" + id);
            employee data = JsonConvert.DeserializeObject<employee>(response.Body);
            return View(data);
        }
        public ActionResult Edit(string id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Employees/" + id);
            employee data = JsonConvert.DeserializeObject<employee>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(employee employee)
        {
            cliente = new FireSharp.FirebaseClient(config);
            SetResponse response = cliente.Set("Employees/" + employee.emp_id, employee);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Employees/" + id);
            return RedirectToAction("Index");
        }

    }
}
