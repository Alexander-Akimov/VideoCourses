﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VOD.Domain.DTOModles.Admin;
using VOD.Domain.Entities;

namespace VOD.Database.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetUsersAsync();
        Task<UserDTO> GetUserAsync(string userId);
        Task<UserDTO> GetUserByEmailAsync(string email);
        Task<IdentityResult> AddUserAsync(RegisterUserDTO user);
        Task<bool> UpdateUserAsync(UserDTO user);
        Task<bool> DeleteUserAsync(string userId);
        Task<VODUser> GetUserAsync(LoginUserDTO loginUser, bool includeClaims);
    }
}
