using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mypassionproject.Models;
using mypassionproject.Models.ViewModels;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Diagnostics;
using System.Web.Script.Serialization;


namespace mypassionproject.Controllers
{
    public class ClientController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static ClientController()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AllowAutoRedirect = false
            };
            client = new HttpClient(handler);
            //change this to match your own local port number
            client.BaseAddress = new Uri("https://localhost:44357/api/");
            client.DefaultRequestHeaders.Accept.Add(
            new MediaTypeWithQualityHeaderValue("application/json"));


            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ACCESS_TOKEN);

        }

        // GET: Client/List
        public ActionResult List()
        {

            string url = "clientdata/getclients";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<ClientDto> SelectedClients = response.Content.ReadAsAsync<IEnumerable<ClientDto>>().Result;
                return View(SelectedClients);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Client/Details/5
        public ActionResult Details(int id)
        {
            ShowClient ViewModel = new ShowClient();
            string url = "clientdata/findclient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Client data transfer object
                ClientDto SelectedClient = response.Content.ReadAsAsync<ClientDto>().Result;
                ViewModel.client = SelectedClient;

                //We don't need to throw any errors if this is null
                //A team not having any pets is not an issue.
                url = "clientdata/getpetsforclient/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                IEnumerable<PetDto> SelectedPets = response.Content.ReadAsAsync<IEnumerable<PetDto>>().Result;
                ViewModel.clientpets = SelectedPets;


                url = "clientata/getbookingsforclient/" + id;
                response = client.GetAsync(url).Result;
                //Can catch the status code (200 OK, 301 REDIRECT), etc.
                //Debug.WriteLine(response.StatusCode);
                //Put data into client data transfer object
                //problem with the code below Json array
                IEnumerable<BookingDto> SelectedBookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;
                ViewModel.clientbookings = SelectedBookings;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Client/Create

        public ActionResult Create()
        {
            return View();
        }

        // POST: Client/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Client ClientInfo)
        {
            Debug.WriteLine(ClientInfo.ClientFirstName);
            string url = "Clientdata/addClient";
            Debug.WriteLine(jss.Serialize(ClientInfo));
            HttpContent content = new StringContent(jss.Serialize(ClientInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int Clientid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = Clientid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Client/Edit/5
        public ActionResult Edit(int id)
        {
            string url = "clientdata/findclient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into Client data transfer object
                ClientDto SelectedClient = response.Content.ReadAsAsync<ClientDto>().Result;
                return View(SelectedClient);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Client/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Client ClientInfo)
        {
            Debug.WriteLine(ClientInfo.ClientFirstName);
            string url = "clientdata/updateclient/" + id;
            Debug.WriteLine(jss.Serialize(ClientInfo));
            HttpContent content = new StringContent(jss.Serialize(ClientInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Client/Delete/5
        [HttpGet]
        public ActionResult DeleteConfirmation(int id)
        {
            string url = "clientdata/findclient/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into client data transfer object
                ClientDto SelectedClient = response.Content.ReadAsAsync<ClientDto>().Result;
                return View(SelectedClient);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Client/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "clientdata/deleteclient/" + id;
            //post body is empty
            HttpContent content = new StringContent("");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }
        public ActionResult Error()
        {
            return View();
        }
    }
}
