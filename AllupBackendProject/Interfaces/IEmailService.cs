using System.Collections.Generic;
using System.Threading.Tasks;

namespace AllupBackendProject.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(List<string> email, string subject, string message);
    }
}
