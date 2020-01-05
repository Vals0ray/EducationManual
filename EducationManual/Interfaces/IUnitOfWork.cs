using EducationManual.Models;

namespace EducationManual.Interfaces
{
    public interface IUnitOfWork
    {
        IGenericRepository<School> Schools { get; }

        IGenericRepository<Classroom> Classrooms { get; }

        void Save();
    }
}
