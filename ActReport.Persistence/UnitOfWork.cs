using System;
using ActReport.Core.Contracts;
using ActReport.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActReport.Persistence
{
  public class UnitOfWork : IUnitOfWork
    {

        private readonly ApplicationDbContext _context = new ApplicationDbContext();
        private bool _disposed;


        /// <summary>
        ///     Konkrete Standard-Repositories. Keine Ableitung nötig
        /// </summary>
        public IGenericRepository<Activity> ActivityRepository { get; }
        public IGenericRepository<Employee> EmployeeRepository { get; }


        /// <summary>
        ///     Konkrete Repositories. Mit Ableitung nötig
        /// </summary>
        //  public IAddressRepository AddressRepository { get; }



        public UnitOfWork()
        {
            _context = new ApplicationDbContext();

            ActivityRepository = new GenericRepository<Activity>(_context);
            EmployeeRepository = new GenericRepository<Employee>(_context);

            //Bsp.: Konkretes Repository mit Überschreibung
            //AddressRepository = new AddressRepository(_context);
        }

        /// <summary>
        ///     Repository-übergreifendes Speichern der Änderungen
        /// </summary>
        public void Save()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void DeleteDatabase()
        {
            _context.Database.EnsureDeleted();
        }

        public void MigrateDatabase()
        {
            try
            {
                _context.Database.Migrate();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public void FillDb()
        {

            DeleteDatabase();
            MigrateDatabase();


            Employee emp = new Employee() { FirstName = "Max", LastName = "Mustermann" };
            _context.Employees.Add(emp);
            _context.Employees.Add(new Employee() { FirstName = "Sarah", LastName = "Aigner" });
            _context.Activities.Add(new Activity() { ActivityText = "Vorbereitung Schulung", Date = Convert.ToDateTime("10.10.2017"), StartTime = DateTime.Parse("01.01.1900 12:00:00"), EndTime = DateTime.Parse("01.01.1900 14:00:00"), Employee = emp });
            _context.Activities.Add(new Activity() { ActivityText = "Durchführung Schulung", Date = Convert.ToDateTime("10.10.2017"), StartTime = DateTime.Parse("01.01.1900 14:00:00"), EndTime = DateTime.Parse("01.01.1900 17:00:00"), Employee = emp });
            _context.SaveChanges();

        }

    }
}
