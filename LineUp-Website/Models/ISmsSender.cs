using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LineUp_Website.Models
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
