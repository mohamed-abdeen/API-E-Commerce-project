using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Reposatories;
using Talabat.Core.Specification;

namespace Talabat.APIs.Controllers
{

    public class EmployeeController : BaseApiController
    {
        private readonly IGenaricReposatiory<Employee> _employeesRepo;

        public EmployeeController(IGenaricReposatiory<Employee> employeesRepo)
        {
            _employeesRepo = employeesRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee()
        {
            var spec = new EmployeeWithDepartmentSpecification();
            var employees = await _employeesRepo.GetAllwithSpecificationAsync(spec);
            return Ok(employees);
        }
    }
}
