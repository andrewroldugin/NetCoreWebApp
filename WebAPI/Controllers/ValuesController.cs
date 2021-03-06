﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        PersonRepository repo = new PersonRepository();

        // GET api/values
        [HttpGet]
        public IEnumerable<Person> Get()
        {
            return repo.GetPersons();
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Person Get(int id)
        {
            return repo.Get(id);
        }

        // POST api/values
        [HttpPost]
        public Person Post([FromBody]Person value)
        {
            return repo.Create(value);
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public Person Put(int id, [FromBody]Person value)
        {
            value.id = id;
            repo.Update(value);
            return value;
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public Person Delete(int id)
        {
            repo.Delete(id);
            // server expects some result, let's return null
            return null;
        }
    }
}
