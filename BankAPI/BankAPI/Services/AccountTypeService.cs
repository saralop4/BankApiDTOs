using BankAPI.Data;
using BankAPI.Models;

namespace BankAPI.Services
{
    public class AccountTypeService
    {
        private readonly BankDbContext contexto;

        public AccountTypeService(BankDbContext contexto)
        {
            this.contexto = contexto;
        }

        public async Task<AccountType?> GetById(int id)
        {
            return await contexto.AccountTypes.FindAsync(id);
        }
    }

}
