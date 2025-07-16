using FmsAPI.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FmsAPI.Controllers
{
    public class IdLookupController : ApiController
    {
        private readonly IIdLookupService _idLookupService;

        public IdLookupController(IIdLookupService idLookupService)
        {
            _idLookupService = idLookupService;
        }

        [HttpGet]
        [Route("GetIdLookup")]
        public IHttpActionResult GetIdLookups()
        {
            var lookups = _idLookupService.GetIdLookups();
            return Ok(lookups);
        }

    [HttpGet]
    [Route("GetIdLookup/{id}")]
    public IHttpActionResult GetIdLookupById(int id)
    {
      var lookup = _idLookupService.GetIdLookupById(id);
      if (lookup == null)
      {
        return NotFound();
      }
      return Ok(lookup);
    }

  }
}
