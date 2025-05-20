using Microsoft.AspNetCore.Mvc;
using TestAzureApp.Models;
using System.Threading.Tasks;
using System.Linq;

namespace TestAzureApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var data = await Task.Run(() => _context.TestData.OrderByDescending(d => d.InsertedAt).ToList());
            ViewBag.MachineName = Environment.MachineName; // Server name
            ViewBag.Region = Environment.GetEnvironmentVariable("Region") ?? "Unknown"; // Web app region, an environment variable has to be set in Azure
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> InsertData()
        {
            var testRecord = new TestData
            {
                Message = "Test record inserted at " + DateTime.UtcNow.ToString("u"),
                InsertedAt = DateTime.UtcNow
            };

            _context.TestData.Add(testRecord);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        // New API endpoint for AJAX
        [HttpPost]
        public async Task<IActionResult> InsertDataApi()
        {
            var testRecord = new TestData
            {
                Message = "Test record inserted at " + DateTime.UtcNow.ToString("u"),
                InsertedAt = DateTime.UtcNow
            };

            _context.TestData.Add(testRecord);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAll()
        {
            var allRecords = _context.TestData.ToList();
            _context.TestData.RemoveRange(allRecords);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var record = await _context.TestData.FindAsync(id);
            if (record != null)
            {
                _context.TestData.Remove(record);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
        public IActionResult TableRows()
        {
            var data = _context.TestData.OrderByDescending(d => d.InsertedAt).ToList();
            return PartialView("TableRows", data);
        }

        [HttpPost]
        public async Task<IActionResult> BurstInsert(int count = 100)
        {
            var cetZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, cetZone);

            var records = Enumerable.Range(0, count).Select(i => new TestData
            {
                Message = $"Burst record {i + 1} inserted at {now:u}",
                InsertedAt = now
            }).ToList();

            _context.TestData.AddRange(records);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



    }
}
