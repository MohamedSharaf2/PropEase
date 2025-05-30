﻿using System.Threading.Tasks;

namespace RealEstateAPI.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
    }
}
