﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using VideoStoreApi.Models;
using VideoStoreApi.Utils;

namespace VideoStoreApi.Controllers
{
    [Route("api/Transactions")]
    public class TransactionController : Controller
    {
        [HttpPost]
        public IActionResult Create([FromBody] TransInput newTrans)
        {
            try
            {
                TransactionUtils newTransactionUtils = new TransactionUtils();
                if (newTransactionUtils.MakeTransaction(newTrans))
                {
                    return Ok();
                }
                return StatusCode(500, "Couldn't Complete the Transaction!");
            }
            catch (Exception e)
            {
                return StatusCode(500, "Couldn't Complete the Transaction!" + e);
            }
        }
    }

    public class TransInput
    {
        public int EmployeeId { get; set; }
        public List<MovieId> MovieList { get; set; }
        public int CustomerId { get; set; }
    }
}