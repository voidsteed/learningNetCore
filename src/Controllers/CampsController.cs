﻿using AutoMapper;
using CoreCodeCamp.Data;
using CoreCodeCamp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreCodeCamp.Controllers
{
    [Route("api/[controller]")]
    public class CampsController : ControllerBase
    {
        private readonly ICampRepository _repository;
        private readonly IMapper _mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<ActionResult<CampModel[]>> Get(bool includeTalks = false)
        {
            try
            {
            
                var results = await _repository.GetAllCampsAsync(includeTalks);

                return Ok(_mapper.Map<CampModel[]>(results));
            }
            catch(Exception) { return this.StatusCode(StatusCodes.Status500InternalServerError, "DB failure"); }

        }

        [HttpGet("{moniker}")]
        public async Task<ActionResult<CampModel>> Get(string moniker)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker);
                if (result == null) return NotFound();
                return Ok(_mapper.Map<CampModel>(result));
            }
            catch (Exception) {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "DB failure");
            }

        }

        [HttpGet("search")]
        public async Task<ActionResult<CampModel[]>> SearchByDate(DateTime theDate, bool includeTalks = false)
        {
            try
            {
                var results = await _repository.GetAllCampsByEventDate(theDate, includeTalks);
                if (!results.Any()) return NotFound();
                return Ok(_mapper.Map<CampModel[]>(results));
            }
            catch (Exception)
            {

                return this.StatusCode(StatusCodes.Status500InternalServerError, "DB failure");
            }
        }
    }
}
