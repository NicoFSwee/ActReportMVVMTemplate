using ActReport.Persistence;
using System;

namespace ActReport.ConsoleFillDb
{
  class Program
  {
    private static void Main()
    {
      using UnitOfWork uow = new UnitOfWork();

      uow.FillDb();
      var res = uow.EmployeeRepository.Get();
      foreach (var emp in res)
      {
        Console.WriteLine(emp.LastName);
      }

      Console.WriteLine("<Eingabe drücken>");
      Console.ReadLine();
    }
  }
}
