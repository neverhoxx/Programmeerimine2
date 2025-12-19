using Azure;
using KooliProjekt.Application.Features.Projects;
using KooliProjekt.Application.Features.ProjectTeams;
using KooliProjekt.WebAPI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace KooliProjekt.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectTeamController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public ProjectTeamController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetProjectTasks([FromQuery] ProjectTeamQuery query)
        {
            var result = await _mediator.Send(query);

            return Result(result);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetProjectTeamQuery { Id = id };
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveProjectTeamCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(DeleteProjectTeamCommand command)
        {
            var response = await _mediator.Send(command);

            return Result(response);
        }
    }

}