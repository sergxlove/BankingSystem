using BankingSystemCore.Abstractions;
using BankingSystemCore.Services;

namespace BankingSystemCore.Models
{
    public class Users
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public static IPasswordHasherService PasswordHasherService { get; set; } = new PasswordHasherService();

        public static ResultModel<Users> Create(Guid id, string username, string password, 
            string role, IPasswordHasherService passwordHasherService)
        {
            if (id == Guid.Empty)
                return ResultModel<Users>.Failure("Поле Id не должно быть пустым");

            if (string.IsNullOrWhiteSpace(username))
                return ResultModel<Users>.Failure("Поле Имя не должно быть пустым");

            if (string.IsNullOrWhiteSpace(password))
                return ResultModel<Users>.Failure("Поле Пароль не должно быть пустым");

            if (string.IsNullOrWhiteSpace(role))
                return ResultModel<Users>.Failure("Поле Роль не должно быть пустым");
            return ResultModel<Users>.Success(new Users(id, username,
                passwordHasherService.Hash(password), role, passwordHasherService));
        }

        public static bool VerifyPassword(string password, string hashPassword)
        {
            return PasswordHasherService.Verify(password, hashPassword);
        }

        private Users(Guid id, string username, string hashPassword, string role, 
            IPasswordHasherService passwordHasherService)
        {
            Id = id;
            Username = username;
            HashPassword = hashPassword;
            Role = role;
            PasswordHasherService = passwordHasherService;
        }
    }
}
