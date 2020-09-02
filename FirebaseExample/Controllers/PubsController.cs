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
    public class PubsController : Controller
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
            FirebaseResponse response = cliente.Get("Publishers");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<publishers>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<publishers>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(publishers publisher)
        {
            try
            {
                AddPublishertoFirebase(publisher);
                ModelState.AddModelError(string.Empty,"Añadido");
                return RedirectToAction("Index");
                ;
            }
            catch(Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
            return View();
        }


        private void AddPublishertoFirebase(publishers publisher)
        {
            cliente = new FireSharp.FirebaseClient(config);
            var data = publisher;
            PushResponse responde = cliente.Push("Publishers/",data);
            data.pub_id = responde.Result.name;
            SetResponse setResponse = cliente.Set("Publishers/"+ data.pub_id,data);
        }

        [HttpGet]
        public ActionResult Detail(string id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Publishers/" + id);
            publishers data = JsonConvert.DeserializeObject<publishers>(response.Body);
            return View(data);
        }
        public ActionResult Edit(string id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Publishers/" + id);
            publishers data = JsonConvert.DeserializeObject<publishers>(response.Body);
            return View(data);
        }

        [HttpPost]
        public ActionResult Edit(publishers publishers)
        {
            cliente = new FireSharp.FirebaseClient(config);
            SetResponse response = cliente.Set("Publishers/" + publishers.pub_id,publishers);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            cliente = new FireSharp.FirebaseClient(config);
            FirebaseResponse response = cliente.Get("Publishers/" + id);
            return RedirectToAction("Index");
        }

    }
}