using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using TestTask.Models;
using TestTask.Services.Interfaces;
using TestTask.ViewModels;

namespace TestTask.Services
{
    public class MessageService : IMessageService
    {
        private static IWebHostEnvironment _environment;
        private readonly MessageContext _db;

        // These constants are needed to work with id of messages in the table
        private const int MinId = 0;
        private const int IdStep = 1;

        public MessageService(IWebHostEnvironment environment, MessageContext context)
        {
            _environment = environment;
            _db = context;
        }

        public async Task<string> UploadMessage(MessageViewModel model, string userId)
        {
            if (model.Message.Length > 0)
            {
                int id = MinId;
                try
                {
                    id = _db.messagesdb.Count();
                }
                catch (Exception e)
                {
                    id = MinId;
                }
                MessageModel messageAddedToDb = new MessageModel(id + IdStep, model.Message, userId, Guid.NewGuid().ToString(), model.AutoDelete);
                
                _db.messagesdb.Add(messageAddedToDb);
                await _db.SaveChangesAsync();
                return messageAddedToDb.url;
            }
            else
            {
                return "Empty message";
            }
        }

        public async Task<string> GetMessageByUrl(string url)
        {
            try
            {
                var message = await _db.messagesdb.FirstOrDefaultAsync(x => x.url == url);
                if (message == null)
                    return "Message not found!";
                var userId = message.userId;
                if (message.autoDelete)
                {
                    _db.messagesdb.Remove(message);
                    await _db.SaveChangesAsync();
                }
                return message.message;
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
        public async Task<List<MessageModel>> GetAllUserMessages(string userId)
        {
            try
                {
                    var result = await _db.messagesdb.Select(c => c).Where(c => c.userId == userId).ToListAsync();
                    if (result == null)
                        throw new Exception("No messages founded");
                    foreach (var message in result)
                    {
                        if (message.autoDelete)
                        {
                            _db.messagesdb.Remove(message);
                            await _db.SaveChangesAsync();
                        }
                    }
                    return result;
                }
                catch (Exception e)
                {
                    throw new Exception("Cant get messages");
                }
        }
        
        public async Task<string> DeleteMessageByUrl(string url)
        {
            try
            {
                var message = await _db.messagesdb.FirstOrDefaultAsync(x => x.url == url);
                if (message == null)
                    return "Message not found!";
                var userId = message.userId;
                
                _db.messagesdb.Remove(message);
                await _db.SaveChangesAsync();

                return "Message " + message.url + " has been deleted";
            }
            catch (Exception e)
            {
                return "Error: " + e.Message;
            }
        }
    }
}