using BaseLibrary.DTOs;
using BaseLibrary.Entities;
using BaseLibrary.Responses;
using Core.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;
using System.Threading.Tasks;

namespace Services
{
    public class UserAccount(IOptions<JwtSection> config, AppDbContext appDbContext) : IUserAccount
    {
        public async Task<GeneralResponse> CreateAsync(Register user)
        {
            if (user is null)
                return new GeneralResponse(false, "Model is empty");
            var CheckUser = await FindUserByEmail(user.Email);
            if (CheckUser != null)
                return new GeneralResponse(true, "User Register already");
            //save user
            var applicationUser = await AddToDatabase(new ApplicationUser()
            {
                FullName = user.FullName,
                Email = user.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(user.Password)

            });

            //check, create and assign role
            var checkAdminRole = await AddToDatabase(new SystemRole() { Name = Constants.Admin });
            if (checkAdminRole is null)
            {
                var createAdminRole = await AddToDatabase(new SystemRole() { Name = Constants.Admin });
                await AddToDatabase(new UserRole() { RoleId = createAdminRole.Id, UserId = applicationUser.Id });
                return new GeneralResponse(true, "Acount Created");
            }
            //////////////////////
            var checkUserRole = await AddToDatabase(new SystemRole() { Name = Constants.User });
            SystemRole response = new();
            if (checkUserRole is null)
            {
                response = await AddToDatabase(new SystemRole() { Name = Constants.User });
                await AddToDatabase(new UserRole() { RoleId = response.Id, UserId = applicationUser.Id });
            }
            else
            {
                await AddToDatabase(new UserRole() { RoleId = checkUserRole.Id, UserId = applicationUser.Id });
            }
            return new GeneralResponse(true, "Acount Created");

        }

        public async Task<LoginResponse> SignInAsync(Login user)
        {
            if (user == null) return new LoginResponse(false, "Model is empty");
            var applicationUser = await FindUserByEmail(user.Email);
            if (applicationUser == null) return new LoginResponse(false, "User not found");

            if (!BCrypt.Net.BCrypt.Verify(user.Password, applicationUser.Password))//if pass is wrong
                return new LoginResponse(false, "Email or Password is invalid");

            var getUserRole = await appDbContext.UserRoles.FirstOrDefaultAsync(a => a.UserId == applicationUser.Id);
            if (getUserRole == null) return new LoginResponse(false, "user role not found");

            var getUserName = await appDbContext.SystemRoles.FirstOrDefaultAsync(a => a.Id == getUserRole.RoleId);
            if (getUserName == null) return new LoginResponse(false, "user role not found");

            string jwtToken = GenerateToken(applicationUser, getUserName!.Name!);
            string refreshToken = GenerateRefreshToken();
            return new LoginResponse(true, "Login Successfully", jwtToken, refreshToken);

        }
        public string GenerateToken(ApplicationUser user, string role)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FullName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, role!),
            };
            var token = new JwtSecurityToken(
                issuer: config.Value.Issuer,
                audience: config.Value.Audience,
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private static string GenerateRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        private async Task<ApplicationUser> FindUserByEmail(string email) =>
            await appDbContext.ApplicationUsers.FirstOrDefaultAsync(o => o.Email!.ToLower()!.Equals(email!.ToLower()));


        private async Task<T> AddToDatabase<T>(T model)
        {
            var result = appDbContext.Add(model);
            await  appDbContext.SaveChangesAsync();
            return (T)result.Entity;
        }
    }
}
