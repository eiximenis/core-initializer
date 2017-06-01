using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LoCrestia.AspNetCore.Initializer
{
    public class InitializerOptions
    {
        public int StatusCode { get; set; }
        public string ErrorText { get; set; }

        public string ResultPath { get; set; }

        public InitializerOptions()
        {
            StatusCode = 503;
            ErrorText = "Service is starting...";
            ResultPath = "/_init";
        }
    }
}