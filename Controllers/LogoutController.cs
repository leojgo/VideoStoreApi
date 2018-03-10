﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Logout")]
    public class LogoutController : Controller
    {
        private readonly EmployeeContext _context;

        public LogoutController(EmployeeContext context)
        {
            _context = context;
        }       

        //===========================================

        [HttpGet]
        public IEnumerable<Employee> GetAll()
        {
            return _context.Employees.ToList();
        }

        [HttpGet("{id}", Name = "LogOut Employee")]
        public IActionResult GetById(int id)
        {
            var item = _context.Employees.FirstOrDefault(t => t.EmployeeId == id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Logout credentials)
        {
            EmployeeUtils newEmpUtil = new EmployeeUtils();
            Employee toLogOut = newEmpUtil.ViewEmployeeAccount(credentials.username);

            if (toLogOut == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(toLogOut);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }

    public class Logout
    {
        public int username { get; set; }
    }
}