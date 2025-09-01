using Azure.Identity;
using DocsService.Data;
using DocsService.Interfaces;
using DocsService.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace DocsService.Repositories
{
    public class UsersRepository: IUsersRepository
    {
        private readonly AppDbContext _context;
        //private readonly IMapper _mapper;
        public UsersRepository(AppDbContext context) 
        {
            _context = context;
            //_mapper = mapper;
        }

        public async Task Add(User user)
        {
            var userEntity = new UserEntity()
            {
                Id = user.Id,
                //Username = user.UserName,
                PasswordHash = user.PasswordHash,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                MiddleName = user.MiddleName,
                Position = user.Position,
                DocumentNumber = user.DocumentNumber,
                ReminderDateOTseptember = new DateTime(2025, 8, 27),
                ReminderDatePBseptember = new DateTime(2025, 8, 27),
                ReminderDateOTmarch = new DateTime(2026, 3, 1)
            };

            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            

            // Проверим всех пользователей в базе
            var allUsers = await _context.Users.ToListAsync();
            Console.WriteLine($"Total users in DB: {allUsers.Count}");
            foreach (var u in allUsers)
            {
                Console.WriteLine($"User in DB: {u.Email}");
            }
        }

        public async Task<User> GetByEmail(string email)
        {
            var userEntity = await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email) ?? throw new Exception();
            return new User
            (
                userEntity.Id,
                //userEntity.Username,
                userEntity.PasswordHash,
                userEntity.Email,
                userEntity.FirstName,
                userEntity.LastName,
                userEntity.MiddleName,
                userEntity.Position,
                userEntity.DocumentNumber
            );
        }

        public async Task<UserEntity?> GetById(Guid id)
        {
            return await _context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);
        }
    }
}
