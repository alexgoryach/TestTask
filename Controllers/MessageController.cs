using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TestTask.Models;
using TestTask.Services.Interfaces;
using TestTask.ViewModels;

namespace TestTask.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService, MessageContext context)
        {
            _messageService = messageService;
        }

        [HttpPost]
        [Authorize]
        public Task<IActionResult> Post(MessageViewModel model)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var uploadedUrl = _messageService.UploadMessage(model, userId);
                return Task.FromResult<IActionResult>(Ok("Uploaded message link: /" + uploadedUrl.Result));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IActionResult>(BadRequest(ex.Message.ToString()));
            }
        }

        [HttpGet("{messageUrl}")]
        public async Task<IActionResult> Get(string messageUrl)
        {
            try
            {
                var message = _messageService.GetMessageByUrl(messageUrl).Result;
                return Ok(message);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var messages = await _messageService.GetAllUserMessages(userId);
                return Ok(messages);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
        
        [HttpDelete("{messageUrl}")]
        [Authorize]
        public async Task<IActionResult> Delete(string messageUrl)
        {
            try
            {
                var deleteResult = await _messageService.DeleteMessageByUrl(messageUrl);
                return Ok(deleteResult);
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }
    }
}