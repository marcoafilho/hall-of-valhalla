using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HallOfValhalla.Domain;

namespace HallOfValhalla.Services
{
    public interface ITopicService
    {
        Task<List<Topic>> GetTopicsAsync();
    }
}
