using Microsoft.AspNetCore.Mvc;
using WaterJugChallenge.Models;
using WaterJugChallenge.Services;
using System.Text.Json;

namespace WaterJugChallenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WaterJugController : ControllerBase
    {
        private readonly IWaterJugService _waterJugService;

        public WaterJugController(IWaterJugService waterJugService)
        {
            _waterJugService = waterJugService;
        }

        [HttpPost("solve")]
        public IActionResult Solve([FromBody] WaterJugRequest request)
        {
            if (!ModelState.IsValid)
            {
                if (ModelState.ContainsKey("$") && ModelState["$"]?.Errors.FirstOrDefault()?.ErrorMessage is string error && error.Contains("y_capacity"))
                {
                    return BadRequest(new { error = "the provided y_capacity is not a positive integer" });
                }

                var firstError = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .FirstOrDefault();

                if (firstError != null)
                {
                    return BadRequest(new { error = firstError });
                }

                return BadRequest(new { error = "Invalid request format" });
            }

            try
            {
                var response = _waterJugService.SolveWaterJugProblem(request);
                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (JsonException)
            {
                return BadRequest(new { error = "Invalid JSON format" });
            }
            catch
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });
            }
        }
    }
} 