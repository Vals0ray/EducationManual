using EducationManual.Interfaces;
using EducationManual.Models;
using EducationManual.Repositories;
using EducationManual.Services;
using System;
using Unity;

namespace EducationManual
{
    /// <summary>
    /// Specifies the Unity configuration for the main container.
    /// </summary>
    public static class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container =
          new Lazy<IUnityContainer>(() =>
          {
              var container = new UnityContainer();

              container.RegisterType<Interfaces.IUnitOfWork, UnitOfWork>();

              container.RegisterType<Interfaces.IGenericRepository<School>, GenericRepository<School>>();
              container.RegisterType<Interfaces.IGenericRepository<Classroom>, GenericRepository<Classroom>>();

              container.RegisterType<Interfaces.IGenericService<School>, SchoolService>();
              container.RegisterType<Interfaces.IGenericService<Classroom>, ClassroomService>();

              container.RegisterType<IUserRepository, UserRepository>();
              container.RegisterType<IUserService, UserService>();

              container.RegisterType<ITaskRepository, TaskRepository>();
              container.RegisterType<ITaskService, TaskService>();

              RegisterTypes(container);
              return container;
          });

        /// <summary>
        /// Configured Unity Container.
        /// </summary>
        public static IUnityContainer Container => container.Value;
        #endregion

        /// <summary>
        /// Registers the type mappings with the Unity container.
        /// </summary>
        /// <param name="container">The unity container to configure.</param>
        /// <remarks>
        /// There is no need to register concrete types such as controllers or
        /// API controllers (unless you want to change the defaults), as Unity
        /// allows resolving a concrete type even if it was not previously
        /// registered.
        /// </remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below.
            // Make sure to add a Unity.Configuration to the using statements.
            // container.LoadConfiguration();

            // TODO: Register your type's mappings here.
            // container.RegisterType<IProductRepository, ProductRepository>();
        }
    }
}