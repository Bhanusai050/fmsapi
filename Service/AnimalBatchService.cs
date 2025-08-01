using FmsAPI.Data;
using FmsAPI.Interface;
using System.Collections.Generic;
using System.Linq;

namespace FmsAPI.Service
{
    public class AnimalBatchService : IAnimalBatchService
    {
        private readonly FarmManagementSystemEntities _context;

        public AnimalBatchService(FarmManagementSystemEntities context)
        {
            _context = context;
        }

        public IEnumerable<AnimalBatch> GetAll()
        {
            return _context.AnimalBatches.ToList();
        }

        public AnimalBatch GetById(int id)
        {
            return _context.AnimalBatches.FirstOrDefault(b => b.BatchID == id);
        }

        public void Add(AnimalBatch batch)
        {
            bool exists = _context.AnimalBatches
              .Any(b => b.BatchName.ToLower() == batch.BatchName.ToLower());

            if (exists)
                throw new System.Exception("A batch with the same name already exists.");

            _context.AnimalBatches.Add(batch);
            _context.SaveChanges();
        }

        public void Update(int id, AnimalBatch batch)
        {
            var existing = _context.AnimalBatches.Find(id);
            if (existing != null)
            {
                existing.BatchName = batch.BatchName;
                existing.PurchasedDate = batch.PurchasedDate;
                existing.Purpose = batch.Purpose;
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var batch = _context.AnimalBatches.Find(id);
            if (batch != null)
            {
                _context.AnimalBatches.Remove(batch);
                _context.SaveChanges();
            }
        }
    }
}
