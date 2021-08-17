using System;
using System.Threading.Tasks;
using API.Data;
using API.DTO;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using API.Models.MainContext;
// using API.Models.MainContext;

namespace API.Repositories
{
    // TODO: Add IAuthRepository
    public class AuthRepository : IAuthRepository
    {
        private readonly MainContext _context;
        private DbSet<Usuario> _dbSet;
        private IConfiguration _configuration { get; }
        public AuthRepository(MainContext context)
        {
            _context = context;
            _dbSet = _context.Set<Usuario>();
        }

        public async Task<object> Login(LoginDTO model)
        {
            if(!(await UserExist(model.Username))) return new { message = "username is not valid", status = false };
            
            var user = await _context.Usuarios.SingleOrDefaultAsync(x => x.Usuario1 == model.Username); // TODO: change to username
            
            return  VerifyPasswordHash(model.Password, user.PasswordHash, user.PasswordSalt) // FIXME: get user attributes
                    ? new { token = CreateToken(user), validated = true, message = "User signin successfully" }
                    : new { validated = false, message = "User signin failed" };
        }

        
        public async Task<object> Register(RegisterDTO model)
        {
            if(await UserExist(model.Username)) return new { message = "user is already in use", status = false };

             CreatePasswordHash(model.Password, out byte[] passwordHash, out byte[] passwordSalt);

            // TODO: Create a new Usuario instance and Add to _dbSet
            // var user = new UserLogin 
            // {
            //     Username = model.Username,
            //     PasswordHash = passwordHash, 
            //     PasswordSalt = passwordSalt,
            //     CodMember = model.CodMember
            // };

            await _dbSet.AddAsync(user);
            
            return  await _context.SaveChangesAsync() > 0 
                    ? new { message = "User registered successfully", status = true } 
                    : new { message = "User registration failed", status = false }; 
        }

        public async Task<bool> UserExist(string username)
        {
            return await _context.Usuarios.AnyAsync(user => user.Usuario1.ToLower() == username.ToLower());
        }

        /* --------------- This method generate the Jwt Token based on User model ------------ */
        private string CreateToken(Usuario model) // TODO: change to Sipffaa Db-first generated model 
        {
            List<Claim> claims = new List<Claim>
            {
              new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
              new Claim(ClaimTypes.Name, model.Username),
              new Claim(ClaimTypes.Role, model.CodRole.ToString())
            };
        
            var SecretKey = _configuration.GetSection("SecretKey").Value; // TODO: add secretkey to appsetting.json
            var SimmetricKey = new SymmetricSecurityKey(System.Text.ASCIIEncoding.UTF8.GetBytes(SecretKey));
        
            SigningCredentials credentials  = new SigningCredentials(SimmetricKey, SecurityAlgorithms.HmacSha512Signature);

            /* This method sets the necessary parameters to generate JWT Bearer Token */

            SecurityTokenDescriptor tokenDecriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDecriptor);

            return tokenHandler.WriteToken(token);
        }

        // OUT keyword enables the passwordSalt and passwordHash on Register() without the need of using return 
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.ASCIIEncoding.UTF8.GetBytes(password));
            }
        }

        /* ----------- Checks all the coincidences and validate password --------------- */
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.ASCIIEncoding.UTF8.GetBytes(password));
                for (int i=0; i < computedHash.Length; i++)
                {
                    if(computedHash[i] != passwordHash[i]) return false;    
                }
            }   

            return true;
        }
        
    }
}