using Demo.EntityFramework.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Service.Interfaces
{
    public interface ITitleManager
    {
        Task<ActionResult> UpdateTitleOrganizationAsync(Title input);

        Task<ActionResult> DeleteTitleOrganizationAsync(Guid id);
    }
}
