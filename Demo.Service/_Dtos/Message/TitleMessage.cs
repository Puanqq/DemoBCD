using Demo.EntityFramework.Entities;
using MassTransit;
using System;

namespace Demo.Service._Dtos.Message
{
    public class TitleMessage : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public bool IsUpdate { get; set; }
        public Title Title { get; set; }
    }
}
