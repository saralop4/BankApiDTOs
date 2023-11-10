using BankAPI.Data;
using BankAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace BankAPI.Services


{
    public class ClientService
    {
        private readonly BankDbContext contexto;

        public ClientService(BankDbContext contexto)
        {
            this.contexto = contexto;
        }
        public async Task<IEnumerable<Client>> GetAll()
        {
            return await contexto.Clients.ToListAsync();

        }
        
        public async Task<Client?> GetById(int id)
        {
            return await contexto.Clients.FindAsync(id);
        }
        public async Task<Client> Create(Client client) 
        { 
             contexto.Clients.Add(client);
            await contexto.SaveChangesAsync();
            return client;
        }

        public async Task Update(int id, Client client)
        {

            var existingClient = await GetById(id);

            if (existingClient != null)
            {
                //proporcionamos los atributos que queremos que se actualicen
                existingClient.Name = client.Name;
                existingClient.PhoneNumber = client.PhoneNumber;
                existingClient.Email = client.Email;

               await  contexto.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            var clientToDelete = await GetById(id);

            if(clientToDelete != null)
            {
                contexto.Clients.Remove(clientToDelete);    
               await contexto.SaveChangesAsync();
            }
        
        }

    }  
}
