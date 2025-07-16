﻿using FmsAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FmsAPI.Interface
{
    public interface ICustomerService
    {
        List<Customer> GetCustomers();
        void AddCustomer(Customer customer);
        bool DeleteCustomer(int id);
    }
}
