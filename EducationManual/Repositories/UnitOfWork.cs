using EducationManual.Interfaces;
using EducationManual.Models;
using System;

namespace EducationManual.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private ApplicationContext db;
        private GenericRepository<School> schoolRepository;
        private GenericRepository<Classroom> classroomRepository;

        public UnitOfWork()
        {
            db = new ApplicationContext();
        }

        public IGenericRepository<School> Schools
        {
            get
            {
                if (schoolRepository == null)
                    schoolRepository = new GenericRepository<School>(db);
                return schoolRepository;
            }
        }

        public IGenericRepository<Classroom> Classrooms
        {
            get
            {
                if (classroomRepository == null)
                    classroomRepository = new GenericRepository<Classroom>(db);
                return classroomRepository;
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                    db.Dispose();

                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}