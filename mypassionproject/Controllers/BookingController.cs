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
    public class BookingController : Controller
    {
        //Http Client is the proper way to connect to a webapi
        //https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclient?view=net-5.0

        private JavaScriptSerializer jss = new JavaScriptSerializer();
        private static readonly HttpClient client;


        static BookingController()
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
        // GET: Booking/list
        public ActionResult List()
        {
            string url = "bookingdata/getbookings";
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                IEnumerable<BookingDto> SelectedBookings = response.Content.ReadAsAsync<IEnumerable<BookingDto>>().Result;
                return View(SelectedBookings);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Booking/Details/5
        public ActionResult Details(int id)
        {
            ShowBooking ViewModel = new ShowBooking();
            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into booking data transfer object
                BookingDto SelectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
                ViewModel.booking = SelectedBooking;


                url = "bookingdata/findclientforbooking/" + id;
                response = client.GetAsync(url).Result;
                ClientDto SelectedClient = response.Content.ReadAsAsync<ClientDto>().Result;
                ViewModel.client = SelectedClient;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Booking/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Booking/Create
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Create(Booking BookingInfo)
        {
            Debug.WriteLine(BookingInfo.BookingDes);
            string url = "bookingdata/addbooking";
            Debug.WriteLine(jss.Serialize(BookingInfo));
            HttpContent content = new StringContent(jss.Serialize(BookingInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {

                int bookingid = response.Content.ReadAsAsync<int>().Result;
                return RedirectToAction("Details", new { id = bookingid });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Booking/Edit/5
        public ActionResult Edit(int id)
        {

            UpdateBooking ViewModel = new UpdateBooking();

            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into booking data transfer object
                BookingDto SelectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
                ViewModel.booking = SelectedBooking;

                //get information about clients this booking could belong to.
                url = "clientata/getclients";
                response = client.GetAsync(url).Result;
                //have problems with the code below Jason Array
                IEnumerable<ClientDto> PotentialClients = response.Content.ReadAsAsync<IEnumerable<ClientDto>>().Result;
                ViewModel.allclients = PotentialClients;

                return View(ViewModel);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Booking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(int id, Booking BookingInfo)
        {
            Debug.WriteLine(BookingInfo.BookingDes);
            string url = "bookingdata/updatebooking/" + id;
            Debug.WriteLine(jss.Serialize(BookingInfo));
            HttpContent content = new StringContent(jss.Serialize(BookingInfo));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {

                return RedirectToAction("Details", new { id = id });
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Booking/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "bookingdata/findbooking/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Can catch the status code (200 OK, 301 REDIRECT), etc.
            //Debug.WriteLine(response.StatusCode);
            if (response.IsSuccessStatusCode)
            {
                //Put data into player data transfer object
                BookingDto SelectedBooking = response.Content.ReadAsAsync<BookingDto>().Result;
                return View(SelectedBooking);
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // POST: Booking/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Delete(int id)
        {
            string url = "bookingdata/deletebooking/" + id;
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
