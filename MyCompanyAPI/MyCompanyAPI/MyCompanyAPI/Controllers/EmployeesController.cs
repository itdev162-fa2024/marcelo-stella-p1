﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyCompanyAPI.Data;
using MyCompanyAPI.Models;

namespace MyCompanyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //might have to change it to employees
    public class EmployeesController : Controller
    {
        private readonly MyCompanyDbContext _myCompanyDbContext;

        public EmployeesController(MyCompanyDbContext myCompanyDbContext)
        {
            _myCompanyDbContext = myCompanyDbContext;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _myCompanyDbContext.Employees.ToListAsync();

            return Ok(employees);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employeeRequest)
        {
            employeeRequest.Id = Guid.NewGuid();

            await _myCompanyDbContext.Employees.AddAsync(employeeRequest);
            await _myCompanyDbContext.SaveChangesAsync();

            return Ok(employeeRequest);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetEmployee([FromRoute]Guid id)
        {
            var employee = await _myCompanyDbContext.Employees.FirstOrDefaultAsync(x => x.Id == id);

            if(employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateEmployee([FromRoute] Guid id, Employee updateEmployeeRequest)
        {
            var employee = await _myCompanyDbContext.Employees.FindAsync(id);

            if(employee == null)
            {
                return NotFound();
            }

            employee.Name = updateEmployeeRequest.Name;
            employee.Email = updateEmployeeRequest.Email;
            employee.Phone = updateEmployeeRequest.Phone;
            employee.Department = updateEmployeeRequest.Department;
            employee.Insurance = updateEmployeeRequest.Insurance;

            await _myCompanyDbContext.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteEmployee([FromRoute] Guid id)
        {
            var employee = await _myCompanyDbContext.Employees.FindAsync(id);

            if(employee == null)
            {
                return NotFound();
            }

            _myCompanyDbContext.Employees.Remove(employee);
            await _myCompanyDbContext.SaveChangesAsync();

            return Ok(employee);
        }
    }
}
