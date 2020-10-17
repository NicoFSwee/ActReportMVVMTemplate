using ActReport.Core.Entities;
using System;

namespace ActReport.Core.Contracts
{
  public interface IUnitOfWork: IDisposable
    {
      
       
        /// <summary>
        /// Standard Repositories 
        /// </summary>
        IGenericRepository<Activity> ActivityRepository { get; }
        IGenericRepository<Employee> EmployeeRepository { get; }
   


        /// <summary>
        /// Erweiterte Repositories
        /// </summary>
        //IAddressRepository AddressRepository { get; }

        void Save();

        void DeleteDatabase();

        void FillDb();
       
        void MigrateDatabase();

    }
}
