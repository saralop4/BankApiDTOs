using BankAPI.Data;
using BankAPI.Data.DTOs;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BankAPI.Services
{
    public class LoginService
    {

        private readonly BankDbContext contexto ;
        
        public LoginService(BankDbContext contexto)
        {
                 this.contexto = contexto ;
        }
        public async Task<Administrator?> GetAdmin(AdminDto admin)
        {
            return await contexto.Administrators.SingleOrDefaultAsync(x=> x.Email == admin.Email && x.Pwd == admin.Pwd);   
        }

    
    }
}
