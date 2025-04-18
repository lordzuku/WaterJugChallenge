using Microsoft.AspNetCore.Mvc;
using WaterJugChallenge.Models;
using WaterJugChallenge.Services;
using System.Text.Json;

namespace WaterJugChallenge.Controllers
{
    /// <summary>
    /// Controller for solving the Water Jug Challenge
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class WaterJugController : ControllerBase
    {
        private readonly IWaterJugService _waterJugService;

        public WaterJugController(IWaterJugService waterJugService)
        {
            _waterJugService = waterJugService;
        }

        /// <summary>
        /// Solves the Water Jug Challenge for given jug capacities and target amount
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/WaterJug/solve
        ///     {
        ///        "x_capacity": 2,
        ///        "y_capacity": 10,
        ///        "z_amount_wanted": 4
        ///     }
        ///
        /// </remarks>
        /// <param name="request">The Water Jug Challenge parameters</param>
        /// <returns>A sequence of steps to solve the challenge</returns>
        /// <response code="200">Returns the solution steps</response>
        /// <response code="400">If the input parameters are invalid</response>
        /// <response code="500">If an unexpected error occurs</response>
        [HttpPost("solve")]
        [ProducesResponseType(typeof(WaterJugResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
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