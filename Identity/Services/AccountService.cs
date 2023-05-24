using Application.DTOs.Users;
using Application.Enums;
using Application.Exceptions;
using Application.Interfaces;
using Application.Wrappers;
using Domain.Settings;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JWTSettings _jwtSettings;
        private readonly IDateTimeService _dateTimeService;

        public AccountService(
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager, 
            SignInManager<ApplicationUser> signInManager, 
            JWTSettings jwtSettings,
            IDateTimeService dateTimeService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _dateTimeService = dateTimeService;
        }


        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, string ipAddress)
        {
            var usuario = await _userManager.FindByEmailAsync(request.Email);
            if (usuario == null)
            {
                throw new ApiException($"No hay una cuenta registrada con el email {request.Email}.");
            }

            var result = await _signInManager.PasswordSignInAsync(
                usuario.UserName,
                request.Password,
                false,
                lockoutOnFailure: false);
            if (!result.Succeeded) 
            {
                throw new ApiException($"Las credenciales del usuario no son validas {request.Email}.");
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWTToken(usuario);
        }

        public async Task<Response<string>> RegisterAsync(RegisterRequest request, string origin)
        {
            var usuarioConElMismoUserName = await _userManager.FindByEmailAsync(request.UserName);
            if (usuarioConElMismoUserName != null)
            {
                throw new ApiException($"El nombre de usuario {request.UserName} ya fue registrado previamente.");
            }

            var usuario = new ApplicationUser
            {
                Email = request.Email,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                UserName = request.UserName,
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };

            var usuarioConElMismoCorreo = await _userManager.FindByEmailAsync(request.Email);
            if (usuarioConElMismoCorreo != null)
            {
                throw new ApiException($"El email {request.Email} ya fue registrado previamente.");
            }
            else
            {
                var result = await _userManager.CreateAsync(usuario, request.Passsword);
                if (result.Succeeded) 
                {
                    await _userManager.AddToRoleAsync(usuario, Roles.Basic.ToString());
                    return new Response<string>(usuario.Id, message: $"Usuario registrado exitosamente {request.UserName}");
                }
                else
                {
                    throw new ApiException($"{result.Errors}.");
                }
            }
        }

        private async Task<JwtSecurityToken> GenerateJWTToken(ApplicationUser usuario)
        {

        }
    }
}
