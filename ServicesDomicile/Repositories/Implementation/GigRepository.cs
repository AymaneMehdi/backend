using Microsoft.EntityFrameworkCore;
using ServicesDomicile.Db_Context;
using ServicesDomicile.Entities;
using ServicesDomicile.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicesDomicile.Repositories.Implementation
{
    public class GigRepository : IGigRepository
    {
        private readonly AppDbContext _context;

        public GigRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Gig>> GetAllGigsAsync()
        {
            return await _context.Gigs.ToListAsync();
        }

        public async Task<Gig> GetGigByIdAsync(Guid id)
        {
            return await _context.Gigs.FindAsync(id);
        }

        public async Task AddGigAsync(Gig gig)
        {
            _context.Gigs.Add(gig);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateGigAsync(Gig gig)
        {
            _context.Gigs.Update(gig);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGigAsync(Guid id)
        {
            var gig = await _context.Gigs.FindAsync(id);
            if (gig != null)
            {
                _context.Gigs.Remove(gig);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> GigExistsAsync(Guid id)
        {
            return await _context.Gigs.AnyAsync(e => e.Id == id);
        }
    }
}
