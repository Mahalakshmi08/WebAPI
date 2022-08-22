using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DemoWebApi.Models;
using System.Linq;
using DemoWebApi.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;

namespace DemoWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeptController : ControllerBase
    {
        db1045Context db = new db1045Context();

        [HttpGet]
        [Route("ListDept")]

        public IActionResult GetDept()
        {
            //var data = db.Depts.ToList();
            //var data = from dept in db.Depts select dept;
            var data = from dept in db.Depts select new {Id=dept.Id, Name=dept.Name,Location = dept.Location};
            return Ok(data);             //return code 200:ok

        }


        [HttpGet]
        [Route("ListDept/{id}")]

        public IActionResult GetDept(int?id)
        {
            if(id==null)
            {
                return BadRequest("Id Cannot be null");
            }
            //var data = db.Depts.Where(d => d.Id == id).Select(d => new {id = d.Id, Name=d.Name, Location=d.Location}).FirstOrDefault();
            var data = (from dept in db.Depts where dept.Id == id select new {Id=dept.Id,Name = dept.Name, Location=dept.Location}).FirstOrDefault();
            
            if(data==null)
            {
                return NotFound($"Department {id} not present");
            }
            return Ok(data);
        }

        //......./api/dept/listcity?city=mumbai
        [HttpGet]
        [Route("ListCity")]

        public IActionResult GetCity([FromQuery] string city)
        {
            var data = db.Depts.Where(d => d.Location == city);

            if (data == null)
            {
                return NotFound($"No Locations found in {city}");
            }           
            return Ok(data);
        }

        [HttpGet]
        [Route("ShowDept")]

        public IActionResult GetDeptInfo()
        {
            var data = db.DeptInfo_VMs.FromSqlInterpolated<DeptInfo_VM>($"DeptInfo");
            return Ok(data);
        }

        [HttpPost]
        [Route("AddDept")]

        public IActionResult PostDept(Dept dept)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    db.Depts.Add(dept);
                    db.SaveChanges();
                }

                catch(Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            return Created("Record Successfully Added", dept);
        }
    }
}
