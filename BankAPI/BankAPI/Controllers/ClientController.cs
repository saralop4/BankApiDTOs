using BankAPI.Data;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Runtime.CompilerServices;

namespace BankAPI.Controllers
{


    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : Controller
    {
        private readonly ClientService service;

        public ClientController(ClientService service)
        {
            this.service = service;
        }

        [HttpGet("getAll")]
        public async Task< IEnumerable<Client>> Get()
        {
            return await service.GetAll();
        }

        [HttpGet("{id}")]
        public async Task< ActionResult<Client>> GetById( int id)
        {
            var client = await service.GetById(id);    
            if(client is null)
            {
                return ClientNotFound(id);  
            }

            return client;
        }

        [HttpPost("create")]
        public async Task< IActionResult> Create(Client client)
        {
            var newClient = await service.Create(client);

            return CreatedAtAction(nameof(GetById), new {id = newClient.Id}, newClient); 
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(int id,Client client) { 
            
            if(id!= client.Id) { 
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el Id ({client.Id}) del cuerpo de la solicitud  " });
            }
            var clientToUpdate = service.GetById(id);

            if( clientToUpdate is not null) 
            {
               await service.Update(id, client);
                return NoContent();
            }
            else
            {
                return ClientNotFound(id);
            }

        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var clientToDelete = await service.GetById(id);

            if (clientToDelete is not null)
            {
                await service.Delete(id);
                return Ok();
            }
            else
            {
                return ClientNotFound(id);
            }

        }
        [NonAction]
        public NotFoundObjectResult ClientNotFound(int id)
        {

            return NotFound(new { message = $"El cliente con ID ={id} no existe" });
        }


    }

}
