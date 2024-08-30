using ServicesDomicile.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesDomicile.Repositories.Interface
{
    public interface IGigRepository
    {
        Task<IEnumerable<Gig>> GetAllGigsAsync();
        Task<Gig> GetGigByIdAsync(Guid id);
        Task AddGigAsync(Gig gig);
        Task UpdateGigAsync(Gig gig);
        Task DeleteGigAsync(Guid id);
        Task<bool> GigExistsAsync(Guid id); // Add this method back
    }
}
