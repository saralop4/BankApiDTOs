using BankAPI.Data;
using BankAPI.Data.DTOs;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace BankAPI.Services
{
    public class AccountService
    {

        private readonly BankDbContext contexto;

        public AccountService(BankDbContext contexto)
        {
            this.contexto = contexto;
        }
        public async Task<IEnumerable<AccountDtoOut>> GetAll()
        {
            return await contexto.Accounts.Select(a => new AccountDtoOut
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation != null ? a.AccountTypeNavigation.Name : "",
                ClientName = a.Client != null ? a.Client.Name : "",
                Balance = a.Balance,
                RegDate = a.RegDate
            }).ToListAsync();

        }

        public async Task<AccountDtoOut?> GetDtoById(int id)
        {
            return await contexto.Accounts.Where(a => a.Id == id).Select(a => new AccountDtoOut
            {
                Id = a.Id,
                AccountName = a.AccountTypeNavigation != null ? a.AccountTypeNavigation.Name : "",
                ClientName = a.Client != null ? a.Client.Name : "",
                Balance = a.Balance,
                RegDate = a.RegDate
            }).SingleOrDefaultAsync();

        }

        public async Task<Account?> GetById(int id)
        {
            return await contexto.Accounts.FindAsync(id);
        }
        public async Task<Account> Create(AccountDtoIn accountDTO)
        {
            var newAccount = new Account();

            newAccount.AccountType = accountDTO.AccountType;
            newAccount.ClientId = accountDTO.ClientId;
            newAccount.Balance = accountDTO.Balance;

            contexto.Accounts.Add(newAccount);
            await contexto.SaveChangesAsync();
            return newAccount;
        }

        public async Task Update(AccountDtoIn account)
        {

            var existingAccount = await GetById(account.Id);

            if (existingAccount != null)
            {
                //proporcionamos los atributos que queremos que se actualicen
                existingAccount.Balance = account.Balance;
                existingAccount.AccountType = account.AccountType;
                existingAccount.ClientId = account.ClientId;
                await contexto.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var accountDelete = await GetById(id);

            if (accountDelete != null)
            {
                contexto.Accounts.Remove(accountDelete);
                await contexto.SaveChangesAsync();
            }

        }
    }
}
