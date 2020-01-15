using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCF.Repositories
{
    public class ClientRepository : IRepository<Client>
    {
        private SalesContext _context = null;

        public ClientRepository(SalesContext context)
        {
            this._context = context;
        }



        public void Create(Client client)
        {
            this._context.Clients.Add(client);
        }

        public void Delete(int id)
        {
            Client client = this._context.Clients.Find(id);
            if (client != null)
                this._context.Clients.Remove(client);
        }

        public IEnumerable<Client> Find(Func<Client, bool> predicate)
        {
            return this._context.Clients.Where(predicate).ToList();
        }

        public Client Get(int id)
        {
            return this._context.Clients.Find(id);
        }

        public Client Get(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Client> GetAll()
        {
            return this._context.Clients;
        }

        public void Update(Client client)
        {
            this._context.Entry(client).State = EntityState.Modified;
        }
    }
}
