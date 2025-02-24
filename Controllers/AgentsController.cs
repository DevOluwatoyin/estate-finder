using EstateFInder.Data;
using EstateFInder.Models;
using EstateFInder.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EstateFInder.Controllers
{//localhost:5000/api/agents
    [Route("api/[controller]")]
    [ApiController]
    public class AgentsController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public AgentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllAgents()
        {
            var allAgents = await Task.Run(() => dbContext.Agents.ToList());
            var response = new Response<List<Agent>>(allAgents)
            {
                Succeeded = true,
                Message = "All Agents retrieved successfully"
            };

            return Ok(response);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetAgentById(Guid id) 
        {
            var agent = await Task.Run(() => dbContext.Agents.Find(id));

            if (agent is null){
                return NotFound();
            }

            var response = new Response<Agent>(agent)
            {
                Succeeded = true,
                Message = "Agent data retrieved successfully"
            };

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAgent(AddAgentDetail addAgentDetail)
        {
            var agentEntity = new Agent()
            {
                Name = addAgentDetail.Name,
                Email = addAgentDetail.Email,
                Phone = addAgentDetail.Phone,
                Address = addAgentDetail.Address
            };

            await Task.Run(() => dbContext.Agents.Add(agentEntity));
            dbContext.SaveChanges();
            //return StatusCode(StatusCodes.Status201Created);

            var response = new Response<Agent>(agentEntity)
            {
                Succeeded = true,
                Message = "Agent  createx successfully"
            };

            return Ok(response);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateAgentData(Guid id, Update_agent updateAgentData)
        {
            var agent = await Task.Run(() => dbContext.Agents.Find(id));

            if (agent is null)
            {
                return NotFound();
            }


            agent.Name = updateAgentData.Name;
            agent.Email = updateAgentData.Email;
            agent.Phone = updateAgentData.Phone;
            agent.Address = updateAgentData.Address;


            dbContext.SaveChanges();

            var response = new Response<Agent>(agent)
            {
                Succeeded = true,
                Message = "Agent data modified successfully"
            };

            return Ok(response);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteAgent(Guid id)
        {
            var agent = await Task.Run(() => dbContext.Agents.Find(id));

            if (agent is null)
            {
                return NotFound();
            }

            dbContext.Agents.Remove(agent);

            dbContext.SaveChanges();

            var response = new Response<Agent>(agent)
            {
                Succeeded = true,
                Message = "Agent data deleted successfully"
            };

            return Ok(response);
        }

    }
}
