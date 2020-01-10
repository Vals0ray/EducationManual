using EducationManual.Models;
using EducationManual.Repositories;

namespace EducationManual.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<School> Schools { get; }

        IGenericRepository<Classroom> Classrooms { get; }

        IUserRepository UserManager { get; }

        void Save();
    }
}
