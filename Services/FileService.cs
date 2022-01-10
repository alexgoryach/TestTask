using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using TestTask.Models;
using TestTask.Services.Interfaces;
using TestTask.ViewModels;

namespace TestTask.Services
{
    public class FileService : IFileService
    {
        private static IWebHostEnvironment _environment;
        private readonly FileContext _db;

        // These constants are needed to work with id of files in the table
        private const int MinId = 0;
        private const int IdStep = 1;

        public FileService(IWebHostEnvironment environment, FileContext context)
        {
            _environment = environment;
            _db = context;
        }

        public async Task<string> UploadFile(FileViewModel model, string userId)
        {
            if (model.File.Length > 0)
            {
                if (!Directory.Exists(_environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\"))
                {
                    Directory.CreateDirectory(_environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\");
                }

                using (FileStream fileStream =
                       System.IO.File.Create(_environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\" + model.File.FileName))
                {
                    model.File.CopyTo(fileStream);
                    fileStream.Flush();
                }
                int id = MinId;
                try
                {
                    id = _db.filesdb.Count();
                }
                catch (Exception e)
                {
                    id = MinId;
                }
                FileModel fileAddedToDb = new FileModel(id + IdStep, model.File.FileName, userId, Path.GetRandomFileName(), model.AutoDelete);
                
                _db.filesdb.Add(fileAddedToDb);
                await _db.SaveChangesAsync();
                return fileAddedToDb.url;
            }
            else
            {
                return "Empty file";
            }
        }

        public async Task<string> GetFileByUrl(string userId, string url)
        {
            if (Directory.Exists(_environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\"))
            {
                try
                {
                    var file = await _db.filesdb.FirstOrDefaultAsync(x => x.url == url);
                    if (file == null)
                        return "File not found!";
                    if (file.autoDelete)
                    {
                        _db.filesdb.Remove(file);
                        await _db.SaveChangesAsync();
                    }

                    return _environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\" + file.name;
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }
            else
            {
                return "Path does not exist";
            }
        }
        public async Task<string> GetAllUserFiles(string userId)
        {
            if (Directory.Exists(_environment.WebRootPath + "\\UploadFiles\\" + $"{userId}\\"))
            {
                try
                {
                    var result = await _db.filesdb.Select(c => c).Where(c => c.userId == userId).ToListAsync();
                    if (result == null)
                        return "Files were not found!";
                    foreach (var file in result)
                    {
                        if (file.autoDelete)
                        {
                            _db.filesdb.Remove(file);
                            await _db.SaveChangesAsync();
                        }
                    }
                    return result.ToString();
                }
                catch (Exception e)
                {
                    return "Error: " + e.Message;
                }
            }
            else
            {
                return "Path does not exist";
            }
        }
    }
}