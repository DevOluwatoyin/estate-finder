using EstateFInder.Data;
using EstateFInder.Models;
using EstateFInder.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Numerics;

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

        [HttpGet]
        public IActionResult GetAllAgents()
        {

            var allAgents = dbContext.Agents.ToList();

            return Ok(allAgents);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public IActionResult GetAgentById(Guid id) {
            var agent = dbContext.Agents.Find(id);

            if (agent is null){
                return NotFound();
            }

            return Ok(agent);
        }

        

        [HttpPost]
        public IActionResult CreateAgent(AddAgentDetail addAgentDetail)
        {
            var agentEntity = new Agent()
            {
                Name = addAgentDetail.Name,
                Email = addAgentDetail.Email,
                Phone = addAgentDetail.Phone,
                Address = addAgentDetail.Address
            };

            dbContext.Agents.Add(agentEntity);
            dbContext.SaveChanges();
            //return StatusCode(StatusCodes.Status201Created);
            return Ok(agentEntity);
        }


        [HttpPut]
        [Route("{id:guid}")]
        public IActionResult UpdateAgentData(Guid id, Update_agent updateAgentData)
        {
            var agent = dbContext.Agents.Find(id);

            if (agent is null)
            {
                return NotFound();
            }


            agent.Name = updateAgentData.Name;
            agent.Email = updateAgentData.Email;
            agent.Phone = updateAgentData.Phone;
            agent.Address = updateAgentData.Address;


            dbContext.SaveChanges();
            return Ok(agent);
        }


        [HttpDelete]
        [Route("{id:guid}")]
        public IActionResult DeleteAgent(Guid id)
        {
            var agent = dbContext.Agents.Find(id);

            if (agent is null)
            {
                return NotFound();
            }


            dbContext.Agents.Remove(agent);


            dbContext.SaveChanges();
            return Ok(agent);
        }
    }
}
