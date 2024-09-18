using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Interfaces;

using AutoMapper;
using Data;

using Domain.DTOs;
using Domain.Entities;

namespace Application.Services
{
    public class ModuleService : IModuleService
    {
        //UoW
        private readonly IDataCoordinator _dataCoordinator;
        //Mapper
        private readonly IMapper _mapper;

        public ModuleService(IDataCoordinator dataCoordinator, IMapper mapper)
        {
            _dataCoordinator = dataCoordinator;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ModuleDto?>> GetModulesOfCourseIdAsync(int id)
        {
            //var courseDto = _dataCoordinator.Courses.GetCourseByIdAsync(id);
            //var course = _mapper.Map<Course>(courseDto);
            //var modules = _dataCoordinator.Modules.GetModulesOfCourseAsync(course);

            var modules = await _dataCoordinator.Modules.GetModulesOfCourseAsync(id);
            var moduleDtos = _mapper.Map<IEnumerable<ModuleDto>>(modules);
            return moduleDtos;
        }
    }
}
