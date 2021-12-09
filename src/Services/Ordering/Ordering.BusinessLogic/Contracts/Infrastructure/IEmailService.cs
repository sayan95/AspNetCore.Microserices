using Ordering.BusinessLogic.Models;
using System.Threading.Tasks;

namespace Ordering.BusinessLogic.Contracts.Infrastructure
{
    public interface IEmailService
    {
        Task<bool> SendEmail(Email email);
    }
}
