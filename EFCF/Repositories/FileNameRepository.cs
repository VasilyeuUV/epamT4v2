using EFCF.DataContexts;
using EFCF.DataModels;
using EFCF.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace EFCF.Repositories
{
    public class FileNameRepository : IRepository<FileName>
    {
        private SalesContext _context = null;

        public FileNameRepository(SalesContext context)
        {
            this._context = context;
        }



        public void Create(FileName fileName)
        {
            this._context.Files.Add(fileName);
        }

        public void Delete(int id)
        {
            FileName fileName = this._context.Files.Find(id);
            if (fileName != null)
                this._context.Files.Remove(fileName);
        }

        public IEnumerable<FileName> Find(Func<FileName, bool> predicate)
        {
            return this._context.Files.Where(predicate).ToList();
        }

        public FileName Get(int id)
        {
            return this._context.Files.Find(id);
        }

        public IEnumerable<FileName> GetAll()
        {
            return this._context.Files;
        }

        public void Update(FileName fileName)
        {
            this._context.Entry(fileName).State = EntityState.Modified;
        }
    }
}
