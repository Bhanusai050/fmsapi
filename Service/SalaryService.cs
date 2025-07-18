using FmsAPI.Data;
using FmsAPI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FmsAPI.Service
{
    public class SalaryService: ISalaryService
    {
        FarmManagementSystemEntities context = new FarmManagementSystemEntities();

        public List<Salary> GetSalaries()
        {
            return context.Salaries.ToList();
        }
    }
}
