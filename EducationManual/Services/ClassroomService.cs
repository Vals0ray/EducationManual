using EducationManual.Interfaces;
using EducationManual.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EducationManual.Services
{
    public class ClassroomService : IGenericService<Classroom>
    {
        private IUnitOfWork Database { get; set; }

        public ClassroomService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public void Create(Classroom item)
        {
            Database.Classrooms.Create(item);
            Database.Save();
        }

        public Classroom FindById(int id)
        {
            return Database.Classrooms.FindById(id);
        }

        public IEnumerable<Classroom> Get()
        {
            return Database.Classrooms.Get();
        }

        public IEnumerable<Classroom> Get(Func<Classroom, bool> predicate)
        {
            return Database.Classrooms.Get(predicate);
        }

        public IEnumerable<Classroom> GetWithInclude(params Expression<Func<Classroom, object>>[] includeProperties)
        {
            return Database.Classrooms.GetWithInclude(includeProperties);
        }

        public IEnumerable<Classroom> GetWithInclude(Func<Classroom, bool> predicate, params Expression<Func<Classroom, object>>[] includeProperties)
        {
            return Database.Classrooms.GetWithInclude(predicate, includeProperties);
        }

        public void Remove(Classroom item)
        {
            Database.Classrooms.Remove(item);
            Database.Save();
        }

        public void Update(Classroom item)
        {
            Database.Classrooms.Update(item);
            Database.Save();
        }
    }
}