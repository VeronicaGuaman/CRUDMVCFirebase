using FirebaseExample.Models;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FirebaseExample.Controllers
{
    public class JobsController : Controller
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "ILmIHUGlrUDwgsJaWzYE2OtuejYbjbUJRg0dlHeU",
            BasePath = "https://pruebamvc-5045f.firebaseio.com/",
        };
        IFirebaseClient cliente;

        // GET: Pubs
        public ActionResult Index()
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Jobs");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<jobs>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<jobs>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(jobs jobs)
        {
            try
            {
                AddJobsFirebase(jobs);
                ModelState.AddModelError(string.Empty, "Añadido");
                return RedirectToAction("Index");
                ;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }


        private void AddJobsFirebase(jobs jobs)
        {
            cliente = new FireSharp.FirebaseClient(config);
            var data = jobs;
            PushResponse responde = cliente.Push("Jobs/", data);
            data.job_id = Convert.ToInt16(responde.Result.name);
            SetResponse setResponse = cliente.Set("Jobs/" + data.job_id, data);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Jobs/" + id);
            publishers data = JsonConvert.DeserializeObject<publishers>(response.Body);
            return View(data);
        }
        public ActionResult Edit(string id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Jobs/" + id);
            publishers data = JsonConvert.DeserializeObject<publishers>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(jobs jobs)
        {
            cliente = new FireSharp.FirebaseClient(config);
            SetResponse response = cliente.Set("Jobs/" + jobs.job_id, jobs);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Jobs/" + id);
            return RedirectToAction("Index");
        }
    }
}