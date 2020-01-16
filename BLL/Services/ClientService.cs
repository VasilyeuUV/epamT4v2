using BLL.DTO;
using BLL.Interfaces;
using EFCF.DataModels;
using EFCF.Interfaces;
using System.Collections.Generic;

namespace BLL.Services
{
    public class ClientService : IEntityService<ClientDTO>
    {
        IUnitOfWork Database { get; set; }

        /// <summary>
        /// CTOR
        /// </summary>
        public ClientService(IUnitOfWork uow)
        {
            Database = uow;
        }


        private ClientDTO ConvertToClientDTO(Client client)
        {
            return new ClientDTO()
            { 
                Id = client.Id, 
                Name = client.Name
            };
        }

        private Client ConvertToClient(ClientDTO entity)
        {
            return new Client()
            {
                Id = entity.Id,
                Name = entity.Name
            };
        }

        #region ISALESERVICE
        //#################################################################################################################

        /// <summary>
        /// Get all entities
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ClientDTO> GetAllEntities()
        {
            var clients = Database.Clients.GetAll();
            if (clients == null) { return null; }

            List<ClientDTO> clientsDTO = new List<ClientDTO>();
            foreach (var client in clients)
            {
                clientsDTO.Add(ConvertToClientDTO(client));
            }
            return clientsDTO.Count < 1 ? null : clientsDTO;
        }



        /// <summary>
        /// Get entity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ClientDTO GetEntity(int id = 0)
        {
            if (id == 0) { return null; }

            var client = Database.Clients.Get(id);
            return client == null ? null : ConvertToClientDTO(client);
        }

        /// <summary>
        /// Get entity by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ClientDTO GetEntity(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) { return null; }

            var client = Database.Clients.Get<Client>(name);
            return client == null ? null : ConvertToClientDTO(client);
        }

        /// <summary>
        /// Save entity
        /// </summary>
        /// <param name="entity"></param>
        public void SaveEntity(ClientDTO entity)
        {
            if (entity == null) { return; }
            Client client = ConvertToClient(entity);

            Database.Clients.Insert(client);
            Database.Save();
        }



        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity"></param>
        public void UpdateEntity(ClientDTO entity)
        {
            if (entity == null) { return; }

            ClientDTO clientDTO = GetEntity(entity.Id);
            if (clientDTO == null)
            {
                SaveEntity(entity);
            }
            else
            {
                Client client = ConvertToClient(entity);
                Database.Clients.Update(client);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity by id
        /// </summary>
        /// <param name="id"></param>
        public void DeleteEntity(int id)
        {
            ClientDTO clientDTO = GetEntity(id);
            if (clientDTO != null)
            {
                Database.Clients.Delete(id);
                Database.Save();
            }
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity"></param>
        public void DeleteEntity(ClientDTO entity)
        {
            DeleteEntity(entity.Id);
        }


        /// <summary>
        /// Dispose UnitOfWork
        /// </summary>
        public void Dispose()
        {
            Database.Dispose();
        }


        #endregion // ISALESERVICE




    }
}
