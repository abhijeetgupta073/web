using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Scaffold.App_Code;

namespace Scaffold.Controllers
{
    public class ValuesController1 : ApiController
    {
        // GET api/<controller>
        [Route("api/Test")]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
        public string GetDecryptClientID(string id)
        {
            return databaseCon.Decrypt(id);
        }

    }
}