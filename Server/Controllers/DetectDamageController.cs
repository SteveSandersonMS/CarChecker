using System;
using CarChecker.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarChecker.Server.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class DetectDamageController : ControllerBase
    {
        [HttpPost]
        public DamageDetectionResult PerformDamageDetection()
        {
            // Here we could read the uploaded image data from Request.Body,
            // and then pass that through to a pretrained ML model for
            // damage detection.
            //
            // However in this repo, we're not able to distribute the ML.NET
            // model data used in the demo, so the actual response here is
            // simply random.

            var rng = new Random();
            return new DamageDetectionResult
            {
                IsDamaged = rng.Next(2) == 0,
                Score = 0.5 + rng.NextDouble() / 2,
            };
        }
    }
}
