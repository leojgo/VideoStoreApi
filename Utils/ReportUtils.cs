using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using VideoStoreApi.Controllers;
using VideoStoreApi.Models;

namespace VideoStoreApi.Utils
{
    public class ReportUtils
    {
        public List<Movie> RunOverdueReport(int NumberofResults)
        {
            List<Movie> Overdue = new List<Movie>();

            return null;
        }

        public List<Movie> RunPopularReport(int NumberofResults)
        {
            List<Movie> Popular = new List<Movie>();

            return null;
        }

        public List<Customer> RunCustomerReport(int NumberofResults)
        {
            List<Customer> BestCustomers = new List<Customer>();

            return null;
        }
    }
}
