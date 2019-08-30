using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;
using VOD.Common.Extensions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VOD.Domain.Interfaces;
using VOD.Database;

namespace VOD.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly VODContext _dbContext;
        private readonly UserManager<VODUser> _userManager;
        public UserService(VODContext dbContext, UserManager<VODUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        public async Task<IEnumerable<UserDTO>> GetUsersAsync()
        {
            return await _dbContext.Users
                .OrderBy(u => u.Email)
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = _dbContext.UserRoles.Any(ur =>
                        ur.UserId.Equals(user.Id) &&
                        ur.RoleId.Equals(1.ToString()))
                })
                .ToListAsync();
        }

        public async Task<UserDTO> GetUserAsync(string userId)
        {
            return await _dbContext.Users
                .OrderBy(u => u.Email)
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = _dbContext.UserRoles.Any(ur =>
                        ur.UserId.Equals(user.Id) &&
                        ur.RoleId.Equals(1.ToString()))
                })
                .FirstOrDefaultAsync(u => u.Id.Equals(userId));
        }

        public async Task<UserDTO> GetUserByEmailAsync(string email)
        {
            return await _dbContext.Users
                .OrderBy(u => u.Email)
                .Select(user => new UserDTO
                {
                    Id = user.Id,
                    Email = user.Email,
                    IsAdmin = _dbContext.UserRoles.Any(ur =>
                        ur.UserId.Equals(user.Id) &&
                        ur.RoleId.Equals(1.ToString()))
                })
                .FirstOrDefaultAsync(u => u.Email.Equals(email));
        }

        public async Task<IdentityResult> AddUserAsync(RegisterUserDTO user)
        {
            var dbUser = new VODUser { UserName = user.Email, Email = user.Email, EmailConfirmed = true };

            var result = await _userManager.CreateAsync(dbUser, user.Password);

            return result;
        }

        public async Task<bool> UpdateUserAsync(UserDTO user)
        {
            //check the user on null before go on
            if (user == null) return false;

            var dbUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == user.Id);

            //TODO: validate user.Email
            if (dbUser == null) return false;
            if (String.IsNullOrEmpty(user.Email)) return false;

            dbUser.Email = user.Email;

            var admin = "Admin";
            var isAdmin = await _userManager.IsInRoleAsync(dbUser, admin);

            if (isAdmin && !user.IsAdmin)
                await _userManager.RemoveFromRoleAsync(dbUser, admin);
            else if (!isAdmin && user.IsAdmin)
                await _userManager.AddToRoleAsync(dbUser, admin);

            var result = await _dbContext.SaveChangesAsync();
            return result >= 0; // if result is 0 then anyway we return true
        }

        public async Task<bool> DeleteUserAsync(string userId)
        {
            try
            {
                var dbUser = await _userManager.FindByIdAsync(userId);
                if (dbUser == null) return false;

                //remove roles from user
                var userRoles = await _userManager.GetRolesAsync(dbUser);
                await _userManager.RemoveFromRolesAsync(dbUser, userRoles);

                //remove the user
                var deleted = await _userManager.DeleteAsync(dbUser);
                return deleted.Succeeded;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<VODUser> GetUserAsync(LoginUserDTO loginUser, bool includeClaims = false)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginUser.Email);
                if (user == null) return null;

                if (loginUser.Password.IsNullOrEmptyOrWhiteSpace() &&
                    loginUser.PasswordHash.IsNullOrEmptyOrWhiteSpace()) return null;

                if (loginUser.Password.Length > 0)
                {
                    var password = _userManager.PasswordHasher
                        .VerifyHashedPassword(user, user.PasswordHash, loginUser.Password);
                    if (password == PasswordVerificationResult.Failed)
                        return null;
                }
                else
                {
                    if (!user.PasswordHash.Equals(loginUser.PasswordHash))
                        return null;
                }

                if (includeClaims)
                    user.Claims = await _userManager.GetClaimsAsync(user);

                return user;
            }
            catch (Exception)
            {
                throw;//why do we need it???
            }
        }
    }
}
