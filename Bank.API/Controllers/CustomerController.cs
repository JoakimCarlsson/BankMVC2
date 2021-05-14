using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bank.API.Services.Customer;
using Bank.API.ViewModels.Customer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Bank.API.Controllers
{
    [ApiController]
    [Route("api/")]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ICustomerService _customerService;

        public CustomerController(IConfiguration config, ICustomerService customerService)
        {
            _config = config;
            _customerService = customerService;
        }
        
        [HttpPost]    
        [Route("GenerateJWTToken")]
        public async Task<ActionResult<string>> Get([FromBody]CustomerViewModel login)
        {
            if (!await _customerService.UserExists(login.Id)) return NotFound();
            
            var tokenString = GenerateJSONWebToken(login);    
            return  Ok(new { token = tokenString });
        }  

        [Route("me")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]    
        public async Task<ActionResult<CustomerDetailsViewModel>> Me()
        {
            var id = User.Claims.First(i => i.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var model = await _customerService.GetCustomerDetails(int.Parse(id));
            return Ok(model);
        }
        
        private string GenerateJSONWebToken(CustomerViewModel customerViewInfo)    
        {    
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));    
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, customerViewInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, customerViewInfo.EmailAddress), //
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            
            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], 
                claims,    
                expires: DateTime.Now.AddMinutes(120),    
                signingCredentials: credentials);    
    
            return new JwtSecurityTokenHandler().WriteToken(token);    
        }    
    }
}