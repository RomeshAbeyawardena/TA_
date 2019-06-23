using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TA.Domains.Exceptions
{
    public class InvalidModelStateException : Exception
    {
        public ModelStateDictionary ModelStateDictionary { get; }

        public InvalidModelStateException(ModelStateDictionary modelStateDictionary)
        {
            ModelStateDictionary = modelStateDictionary;
        }
    }
}