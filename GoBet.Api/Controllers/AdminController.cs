<<<<<<< HEAD
﻿using GoBet.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoBet.Api.Controllers
{
    // =====================
    // System & Monitoring
    // =====================
    [ApiController]
    [Route("api/admin/system")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminSystemController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetStatus() => Ok(new { status = "Operational" });

        [HttpGet("metrics")]
        public IActionResult GetSystemMetrics() => Ok(new { metrics = new string[] { "Metric1", "Metric2" } });

        [HttpGet("database")]
        public IActionResult GetDatabaseStatus() => Ok(new { database = "Running smoothly" });

        [HttpGet("server")]
        public IActionResult GetServerStatus() => Ok(new { server = "Operational" });

        [HttpGet("cache")]
        public IActionResult GetCacheStatus() => Ok(new { cache = "Functioning properly" });

        [HttpGet("dependencies")]
        public IActionResult GetDependencyStatus() => Ok(new { dependencies = "All up to date" });

        [HttpGet("version")]
        public IActionResult GetVersionInfo() => Ok(new { version = "1.0.0" });

        [HttpGet("build")]
        public IActionResult GetBuildInfo() => Ok(new { build = "Build 1001" });

        [HttpGet("release-notes")]
        public IActionResult GetReleaseNotes() => Ok(new { releaseNotes = "Initial release" });
    }

    // =====================
    // Users & Drivers
    // =====================
    [ApiController]
    [Route("api/admin/users")]
    public class AdminUsersController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetUsers() => Ok(new { users = new string[] { "User1", "User2" } });

        [HttpGet("drivers")]
        public IActionResult GetDrivers() => Ok(new { drivers = new string[] { "Driver1", "Driver2" } });

        [HttpGet("activity")]
        public IActionResult GetUserActivity() => Ok(new { activities = new string[] { "Activity1", "Activity2" } });
    }

    // =====================
    // Reports, Analytics & Feedback
    // =====================
    [ApiController]
    [Route("api/admin/reports")]
    public class AdminReportsController : ControllerBase
    {
        [HttpGet("reports")]
        public IActionResult GetReports() => Ok(new { reports = new string[] { "Report1", "Report2" } });

        [HttpGet("analytics")]
        public IActionResult GetAnalytics() => Ok(new { analytics = new string[] { "Analytics1", "Analytics2" } });

        [HttpGet("rides")]
        public IActionResult GetRides() => Ok(new { rides = new string[] { "Ride1", "Ride2" } });

        [HttpGet("payments")]
        public IActionResult GetPayments() => Ok(new { payments = new string[] { "Payment1", "Payment2" } });

        [HttpGet("feedback")]
        public IActionResult GetFeedback() => Ok(new { feedback = new string[] { "Feedback1", "Feedback2" } });

        [HttpGet("promotions")]
        public IActionResult GetPromotions() => Ok(new { promotions = new string[] { "Promotion1", "Promotion2" } });
    }

    // =====================
    // Support, Legal & Info
    // =====================
    [ApiController]
    [Route("api/admin/info")]
    public class AdminInfoController : ControllerBase
    {
        [HttpGet("support-tickets")]
        public IActionResult GetSupportTickets() => Ok(new { tickets = new string[] { "Ticket1", "Ticket2" } });

        [HttpGet("team")]
        public IActionResult GetTeamInfo() => Ok(new { team = new string[] { "Member1", "Member2" } });

        [HttpGet("contact")]
        public IActionResult GetContactInfo() => Ok(new { contact = "0987654321" });

        [HttpGet("legal")]
        public IActionResult GetLegalInfo() => Ok(new { legal = "All legal information is up to date" });

        [HttpGet("privacy-policy")]
        public IActionResult GetPrivacyPolicy() => Ok(new { privacyPolicy = "Compliant" });

        [HttpGet("terms-of-service")]
        public IActionResult GetTermsOfService() => Ok(new { termsOfService = "Accepted" });

        [HttpGet("faq")]
        public IActionResult GetFAQ() => Ok(new { faq = new string[] { "FAQ1", "FAQ2" } });

        [HttpGet("help")]
        public IActionResult GetHelp() => Ok(new { help = "Help section available" });

        [HttpGet("roadmap")]
        public IActionResult GetRoadmap() => Ok(new { roadmap = new string[] { "Feature1", "Feature2" } });

        [HttpGet("changelog")]
        public IActionResult GetChangelog() => Ok(new { changelog = new string[] { "Changelog1", "Changelog2" } });
=======
﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using GoBet.Domain.Constants;
using GoBet.Application.Services.Interfaces;

namespace GoBet.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = Roles.Admin)]
    public class AdminController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("approve-driver/{userId}")]
        public async Task<IActionResult> ApproveDriver(string userId)
        {
            await _userService.ApproveDriverAsync(userId);
            return Ok(new { message = "User has been approved as a driver." });
        }
>>>>>>> c234042cdbe0294b25c4f396f7dd3818bdcd29e4
    }
}
