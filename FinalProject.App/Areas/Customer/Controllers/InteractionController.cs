using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class InteractionController : Controller
    {
        private readonly HttpClient _httpClient;

        public InteractionController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }


        public IActionResult Create()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SearchMedicines(string term)
        {
            if (string.IsNullOrEmpty(term))
            {
                return Json(new List<object>());
            }

            string apiUrl = $"https://api.fda.gov/drug/label.json?search=openfda.brand_name:{term}*+OR+openfda.generic_name:{term}*&limit=10";

            try
            {
                var response = await _httpClient.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                var jsonString = await response.Content.ReadAsStringAsync();
                using var document = JsonDocument.Parse(jsonString);

                var results = document.RootElement.GetProperty("results");
                var medicines = new List<object>();

                foreach (var item in results.EnumerateArray())
                {
                    var openfda = item.GetProperty("openfda");
                    string brandName = openfda.TryGetProperty("brand_name", out var brand) ? brand[0].GetString() : null;
                    string genericName = openfda.TryGetProperty("generic_name", out var generic) ? generic[0].GetString() : null;

                    if (!string.IsNullOrEmpty(brandName) || !string.IsNullOrEmpty(genericName))
                    {
                        medicines.Add(new
                        {
                            label = brandName ?? genericName,
                            value = brandName ?? genericName
                        });
                    }
                }

                return Json(medicines);
            }
            catch
            {
                return Json(new List<object>());
            }
        }
    }

}
