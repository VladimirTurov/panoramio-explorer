using System;
using System.Threading.Tasks;

namespace PanoramioExplorer.Services
{
    public interface IFileSavingService
    {
        Task<SavingOperationResult> SaveImageAsync(Uri imageSource);
    }
}
