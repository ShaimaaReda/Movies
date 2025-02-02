﻿using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Core.IServices;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("register")]
       public async Task<IActionResult> CreateAsync (Register user)
        {
            if (user == null)
            {
                return BadRequest("Model is empty");
            }
            var result = await userAccount.CreateAsync (user);
            return Ok(result);   
        }
        [HttpPost("login")]
        public async Task<IActionResult> signInAsync(Login user)
        {
            if (user == null)
            {
                return BadRequest("Model is empty");
            }
            var result = await userAccount.SignInAsync(user);
            return Ok(result);
        }

    }
}
