﻿namespace HospitalManagement.Models.AdminModels
{
    public class VerifyOtpRequest
    {
        public string Mobile { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;

    }
}