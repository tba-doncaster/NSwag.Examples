using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using NSwagAspNetCoreWithExamples.Models;
using NSwagAspNetCoreWithExamples.Models.Zoo;

namespace NSwagAspNetCoreWithExamples.Controllers;

[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(CustomInternalError))]
[ApiController]
[Route("api/v1/zoo")]
public class ZooController : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Animal>))]
    public IActionResult GetAnimals() => Ok(Enumerable.Empty<Animal>());

    [HttpGet("{name}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Animal))]
    public IActionResult GetAnimals([FromRoute][BindRequired] string name) => Ok(default(Animal));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Adopt([FromBody][BindRequired] Animal animal) => Ok();
}