using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HallOfValhalla.Domain;

namespace HallOfValhalla.Services
{
    public interface IVenueService
    {
        Task<List<Venue>> GetVenuesAsync();
    }
}
