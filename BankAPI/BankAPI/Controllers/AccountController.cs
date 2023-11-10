using BankAPI.Data.DTOs;
using BankAPI.Models;
using BankAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BankAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly AccountService service;
        private readonly AccountTypeService accountTypeService;
        private readonly ClientService clientService;


        public AccountController(AccountService service, AccountTypeService accountTypeService, ClientService clientService)
        {
            this.service = service;
            this.accountTypeService = accountTypeService;
            this.clientService = clientService;

        }

        [HttpGet("getAll")]
        public async Task<IEnumerable<AccountDtoOut>> Get()
        {
            return await service.GetAll();
        }

        [HttpGet("getOne/{id}")]
        public async Task<ActionResult<AccountDtoOut>> GetById(int id)
        {
            var account = await service.GetDtoById(id);
            if (account is null)
            {
                return AccountNotFound(id);
            }

            return account;
        }

        [Authorize(Policy = "SuperAdmin")]
        [HttpPost("create")]
        public async Task<IActionResult> Create(AccountDtoIn account)
        {
            string validationResult = await ValidateAccount(account);
            if (!validationResult.Equals("Valid"))
            {
                return BadRequest(new { message = validationResult });
            }
            var newAccount = await service.Create(account);

            return CreatedAtAction(nameof(GetById), new { id = newAccount.Id }, newAccount);
        }


        [Authorize(Policy = "SuperAdmin")]
        [HttpPut("updateOne/{id}")]
        public async Task<IActionResult> Update(int id, AccountDtoIn account)
        {

            if (id != account.Id)
            {
                return BadRequest(new { message = $"El ID = {id} de la URL no coincide con el Id ({account.Id}) del cuerpo de la solicitud  " });
            }
            var accountToUpdate = service.GetById(id);

            if (accountToUpdate is not null)
            {
                string validationResult = await ValidateAccount(account);
                if (!validationResult.Equals("Valid"))
                {
                    return BadRequest(new { message = validationResult });
                }
                await service.Update(account);
                return NoContent();
            }
            else
            {
                return AccountNotFound(id);
            }

        }


        [Authorize(Policy = "SuperAdmin")]
        [HttpDelete("deleteOne/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var accountToDelete = await service.GetById(id);

            if (accountToDelete is not null)
            {
                await service.Delete(id);
                return Ok();
            }
            else
            {
                return AccountNotFound(id);
            }

        }
        [NonAction]
        public NotFoundObjectResult AccountNotFound(int id)
        {
            return NotFound(new { message = $"la cuenta con ID ={id} no existe" });
        }



        [NonAction]
        public async Task<string> ValidateAccount(AccountDtoIn account)
        {
            string result = "Valid";

            var accountType = await accountTypeService.GetById(account.AccountType);

            if (accountType is null)
            {
                result = $"El tipo de cuenta {account.AccountType} no existe. ";

            }
            var clientId = account.ClientId.GetValueOrDefault();
            var client = await clientService.GetById(clientId);

            if (client is null)
            {
                result = $"El Cliente {clientId} no existe. ";
            }

            return result;


        }
    }
}
