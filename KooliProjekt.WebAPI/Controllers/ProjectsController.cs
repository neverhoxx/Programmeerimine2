using Azure;
using KooliProjekt.Application.Features.Projects;
using KooliProjekt.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjects([FromQuery] ProjectsQuery query)
        {
            var result = await _mediator.Send(query);

            return Result(result);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProjectsQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }


        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveProjectCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteProjectCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }
}