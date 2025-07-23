using FmsAPI.Data;
using FmsAPI.Helpers;
using FmsAPI.Interface;
using FmsAPI.Models;
using FmsAPI.Service;
using System;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FmsAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api")]
    public class ContactMessagesController : ApiController

    {
        private readonly IContactMessageService _contactService;

        // Constructor for Dependency Injection
        public ContactMessagesController(IContactMessageService contactService)
        {
            _contactService = contactService;
        }

        // Parameterless constructor fallback
        public ContactMessagesController()
        {
            _contactService = new ContactMessageService();
        }

        [HttpGet]
        [Route("Getcontactmessages")]
        public async Task<IHttpActionResult> GetMessages()
        {
            var messages = await _contactService.GetAllMessagesAsync();
            return Ok(messages);
        }

        [HttpPost]
        [Route("Postcontactmessages")]
        public async Task<IHttpActionResult> SubmitContact(ContactMessage contact)
        {
            var savedMessage = await _contactService.AddMessageAsync(contact);

            // Send thank you email to user
            MailService.SendContactAcknowledgementEmail(
                savedMessage.Email, savedMessage.Name, savedMessage.Message
            );

            return Ok("Message received. A confirmation has been sent to your email.");
        }
    }
    
}
