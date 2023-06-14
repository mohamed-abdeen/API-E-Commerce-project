using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specification
{
    public class EmployeeWithDepartmentSpecification:BaseSpecification<Employee>
    {
        public EmployeeWithDepartmentSpecification()
        {
            AddInclude(e => e.Department);
        }
        public EmployeeWithDepartmentSpecification(int id):base(E=>E.Id==id)
        {
            AddInclude(e => e.Department);
        }
    }
}
