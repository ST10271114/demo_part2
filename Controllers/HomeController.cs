using demo_part2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace demo_part2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Check the connection
            try
            {
                connection conn = new connection();
                using (SqlConnection connect = new SqlConnection(conn.connecting()))
                {
                    connect.Open();
                    Console.WriteLine("Connected");
                    connect.Close();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error: " + error.Message);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // User Registration
        [HttpPost]
        public IActionResult Register_user(register add_user)
        {
            string name = add_user.name;
            string email = add_user.email;
            string password = add_user.password;
            string role = add_user.role;

            string message = add_user.insert_user(name, email, role, password);

            if (message == "done")
            {
                Console.WriteLine(message);
                return RedirectToAction("Login", "Home");
            }
            else
            {
                Console.WriteLine(message);
                return RedirectToAction("Index", "Home");
            }
        }

        // Login
        public IActionResult Login()
        {
            return View();
        }

        // Program Coordinator Login
        [HttpGet("login_program_coordinator")]
        public IActionResult login_program_coordinator()
        {
            return View(new check_login());
        }

        // User Login
        [HttpPost]
        public IActionResult login_user(check_login user)
        {
            string email = user.email;
            string role = user.role;
            string password = user.password;

            string message = user.login_user(email, role, password);
            if (message == "found")
            {
                return RedirectToAction("Dashboard", "Home"); 
            }
            else
            {
                return View("Login"); 
            }
        }


        // Program Dashboard
        public IActionResult program_dashboard()
        {
            
            return View(); 
        }

        // Submit Claim
        [HttpPost]
        public IActionResult ClaimSub(IFormFile file, claim insert)
        {
            string module_name = insert.user_email;
            string hour_work = insert.hours_worked;
            string hour_rate = insert.rate;
            string description = insert.description;

            // File info
            string filename = "no file";
            if (file != null && file.Length > 0)
            {
                filename = Path.GetFileName(file.FileName);
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/pdf");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                string filePath = Path.Combine(folderPath, filename);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }

            string message = insert.insert_claim(module_name, hour_work, hour_rate, description, filename);

            if (message == "done")
            {
                Console.WriteLine(message);
                return RedirectToAction("program_dashboard"); 
            }
            else
            {
                Console.WriteLine(message);
                return RedirectToAction("program_dashboard");
            }
        }

        // View Claims
        public IActionResult view_claims()
        {
            get_claims collect = new get_claims();
            return View(collect);
        }

        // Approve Claims View
        public IActionResult ApproveClaims()
        {
            get_claims collect = new get_claims();
            return View(collect);
        }

        // Approve Claim
        [HttpPost]
        public IActionResult ApproveClaim(string claimId)
        {
            using (SqlConnection conn = new SqlConnection(new connection().connecting()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE claiming SET status = 'Approved' WHERE user_id = @ClaimId", conn))
                {
                    cmd.Parameters.AddWithValue("@ClaimId", claimId);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("ApproveClaims");
        }
        public IActionResult Dashboard()
        {
            return View(); 
        }

        // Reject Claim
        [HttpPost]
        public IActionResult RejectClaim(string claimId)
        {
            using (SqlConnection conn = new SqlConnection(new connection().connecting()))
            {
                conn.Open();
                using (SqlCommand cmd = new SqlCommand("UPDATE claiming SET status = 'Rejected' WHERE user_id = @ClaimId", conn))
                {
                    cmd.Parameters.AddWithValue("@ClaimId", claimId);
                    cmd.ExecuteNonQuery();
                }
            }
            return RedirectToAction("ApproveClaims");
        }
    }
}
