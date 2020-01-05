using EducationManual.Interfaces;
using EducationManual.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationManual.Services
{
    public class SchoolService : IGenericService<School>
    {
        private IUnitOfWork Database { get; set; }

        public SchoolService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Create(School item)
        {
            Database.Schools.Create(item);
            Database.Save();
        }

        public School FindById(int id)
        {
            return Database.Schools.FindById(id);
        }

        public IEnumerable<School> Get()
        {
            return Database.Schools.Get();
        }

        public IEnumerable<School> Get(Func<School, bool> predicate)
        {
            return Database.Schools.Get(predicate);
        }

        public void Remove(School item)
        {
            Database.Schools.Remove(item);
            Database.Save();
        }

        public void Update(School item)
        {
            Database.Schools.Update(item);
            Database.Save();
        }

        public IEnumerable<School> GetWithInclude(params Expression<Func<School, object>>[] includeProperties)
        {
            return Database.Schools.GetWithInclude(includeProperties);
        }

        public IEnumerable<School> GetWithInclude(Func<School, bool> predicate, params Expression<Func<School, object>>[] includeProperties)
        {
            return Database.Schools.GetWithInclude(predicate, includeProperties);
        }
    }
}