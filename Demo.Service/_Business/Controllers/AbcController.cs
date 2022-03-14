using AutoMapper;
using Demo.EntityFramework.Entities;
using Demo.Service.Base;
using Demo.Service.Base.Dtos;
using Demo.Service.Base.Enums;
using Demo.Service.Base.Interfaces;
using Demo.Service.Dtos;
using Demo.Service.Enums;
using Demo.UnitOfWork.interfaces;
using LinqKit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo.Service.Business.Controllers
{

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AbcController : Controller
    {
        private readonly IRepository<Organization, Guid> _repository;
        private readonly IExcelManager _excelManager;
        private List<ExcelHeader>  products = new List<ExcelHeader>();
        private IFileManager _fileManager;
        private UserManager<User> _userManager;
        private IMapper _mapper;

        public AbcController(
            IRepository<Organization, Guid> repository,
            UserManager<User> userManager,
            IFileManager fileManager,
            IMapper mapper,
            IExcelManager excelManager)
        {
            _repository = repository;
            _excelManager = excelManager;
            _fileManager = fileManager;
            _userManager = userManager;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task A()
        {
            var id = "964E50FC-8F24-4AFD-3ACE-08DA03009A78";

            var user = await _userManager.FindByIdAsync(id);

            var a = new UpdateUserInfomationInputDto()
            {
                UserName = "abcd2",
                Name = "aa",
                Surname = "aaa"
            };

            user = _mapper.Map<UpdateUserInfomationInputDto, User>(a, user);
            //user.NormalizedUserName = _userManager.NormalizeName(user.UserName);
            await _userManager.UpdateAsync(user);
        }


        [HttpPost]
        public async Task Abc()
        {
            var query = _repository.Query;

            var list2 = await query.OrderBy(o => o.Name).ToListAsync();

            var searchPredicate = PredicateBuilder.New<Organization>();


            Expression<Func<Organization, bool>> expression = p => EF.Property<string>(p, "CreatedTime") == "3/11/2022 8:47:03 AM";

            query = query.OrderBy(o => EF.Property<string>(o, "Name"));
            //query = query.Where(expression);
            //query = query.Where(a => EF.Property<string>(a, "Name") == "Kế Toán");
            //query = query.Where(a => EF.Property<string>(a, "CodeValue") != "BV");
            //query = query.Where(a => EF.Property<string>(a, "CodeValue") != "NS");

            //searchPredicate = searchPredicate.Or(QueryableExtention.GetExpression<Organization>("Name", orValue.Operation, orValue.Value));

            //for (int i = 0; i < 2; i++)
            //{
            //    if(i == 0)
            //    {
            //        query = query.Where(w => w.Name.Contains("Kế Toán"));
            //    }
            //    else if(i == 1)
            //    {
            //        query = query.Or(w => w.CodeValue.Contains("KT"));
            //    }
            //}

            var list = await query.ToListAsync();
        }
       
    }
}
