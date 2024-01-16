﻿using Microsoft.AspNetCore.Http;

namespace PetCoffee.Application.Service;

public interface IAzureService
{
    public Task<bool> HasBadWords(string content);
    public Task<string> Translate(string content, string to);
    public Task<string> CreateBlob(string name, IFormFile file);
    public Task<string> GetBlob(string name);
}
