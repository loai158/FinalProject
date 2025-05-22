using FinalProject.Data.Models.Medical;
using FinalProject.Services.Abstracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace FinalProject.App.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class InteractionController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IAppointmentServices _appointmentServices;
        private readonly IPreviousMedicineServices _previousMedicineServices;

        public InteractionController(IHttpClientFactory httpClientFactory, IAppointmentServices appointmentServices, IPreviousMedicineServices previousMedicineServices)
        {
            _httpClient = httpClientFactory.CreateClient();
            this._appointmentServices = appointmentServices;
            this._previousMedicineServices = previousMedicineServices;
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

        [HttpGet]
        public async Task<IActionResult> CheckInteractions(string medicineName, int appointmentId)
        {
            if (string.IsNullOrEmpty(medicineName) || appointmentId <= 0)
            {
                return Content("<p class='text-danger text-center'>بيانات غير صالحة.</p>");
            }

            var patientId = _appointmentServices.GetAll().Where(a => a.Id == appointmentId)
                                         .Select(p => p.PatientId).FirstOrDefault();
            if (patientId == 0)
            {
                return Content("<p class='text-danger text-center'>لم يتم العثور على موعد بالمعرف المحدد.</p>");
            }

            var currentMedicines = await _previousMedicineServices.GetAll()
                .Where(x => x.PatientId == patientId)
                .Select(m => m.Name.Trim())
                .ToListAsync();

            Console.WriteLine("Current Medicines (normalized): " + string.Join(", ", currentMedicines));

            if (!currentMedicines.Any())
            {
                return Content("<p class='text-muted text-center'>لا توجد أدوية مسجلة للمريض.</p>");
            }

            var interactionDetails = new List<string>();
            using var client = new HttpClient();

            // جلب بيانات الدواء الجديد
            var newMedQuery = Uri.EscapeDataString($"openfda.brand_name:\"{medicineName}\" OR openfda.generic_name:\"{medicineName}\"");
            var newMedUrl = $"https://api.fda.gov/drug/label.json?search={newMedQuery}&limit=1";
            List<string> newMedReactions = new List<string>();

            try
            {
                var newMedResult = await client.GetAsync(newMedUrl);
                if (newMedResult.IsSuccessStatusCode)
                {
                    var newMedJson = await newMedResult.Content.ReadAsStringAsync();
                    var newMedData = JsonSerializer.Deserialize<DrugLabelResponse>(newMedJson);

                    if (newMedData?.results != null && newMedData.results.Any())
                    {
                        newMedReactions = newMedData.results
                            .SelectMany(e => e.warnings?.Select(w => w) ?? Enumerable.Empty<string>())
                            .Where(w => !string.IsNullOrEmpty(w))
                            .Distinct()
                            .ToList();
                    }
                    else
                    {
                        Console.WriteLine($"No data found for new medicine: {medicineName}");
                    }
                }
                else
                {
                    Console.WriteLine($"FDA API Error for {medicineName}: {newMedResult.StatusCode} - {newMedResult.ReasonPhrase}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error fetching new medicine {medicineName}: {e.Message}");
            }

            // مقارنة الدواء الجديد مع الأدوية الحالية
            foreach (var currentMed in currentMedicines)
            {
                var currentMedQuery = Uri.EscapeDataString($"openfda.brand_name:\"{currentMed}\" OR openfda.generic_name:\"{currentMed}\"");
                var currentMedUrl = $"https://api.fda.gov/drug/label.json?search={currentMedQuery}&limit=1";

                try
                {
                    var currentMedResult = await client.GetAsync(currentMedUrl);
                    if (currentMedResult.IsSuccessStatusCode)
                    {
                        var currentMedJson = await currentMedResult.Content.ReadAsStringAsync();
                        var currentMedData = JsonSerializer.Deserialize<DrugLabelResponse>(currentMedJson);

                        if (currentMedData?.results != null && currentMedData.results.Any())
                        {
                            var currentMedReactions = currentMedData.results
                                  .SelectMany(e => e.warnings?.Select(w => w) ?? Enumerable.Empty<string>())
                            .Where(w => !string.IsNullOrEmpty(w))
                            .Distinct()
                            .ToList();

                            // مقارنة التفاعلات بين الدواء الجديد والدواء الحالي
                            var commonReactions = newMedReactions.Intersect(currentMedReactions).ToList();
                            if (commonReactions.Any())
                            {
                                interactionDetails.Add($"تفاعل محتمل بين <strong>{medicineName}</strong> و <strong>{currentMed}</strong>. الأعراض المشتركة: {string.Join(", ", commonReactions)}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"FDA API Error for {currentMed}: {currentMedResult.StatusCode} - {currentMedResult.ReasonPhrase}");
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"HTTP Request Error for {currentMed}: {e.Message}");
                }
                catch (JsonException e)
                {
                    Console.WriteLine($"JSON Deserialization Error for {currentMed}: {e.Message}");
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Unexpected error for {currentMed}: {e.Message}");
                }
            }

            // عرض النتائج النهائية
            if (!interactionDetails.Any())
            {
                return Content("<p class='text-muted text-center'>لا توجد تقارير أحداث سلبية تشير إلى تفاعلات بين الدواء الجديد والأدوية الحالية.</p>");
            }

            var html = "<div class='alert alert-warning' role='alert'><h4><i class='fas fa-exclamation-triangle'></i> تنبيه: تفاعلات دوائية محتملة (بناءً على تقارير FDA)</h4><ul>";
            foreach (var detail in interactionDetails)
            {
                html += $"<li>{detail}</li>";
            }
            html += "</ul><p class='small text-danger'>هذه المعلومات مبنية على تقارير الأحداث السلبية. يرجى استشارة الطبيب لتقييم المخاطر.</p></div>";

            return Content(html);
        }
    }
}


