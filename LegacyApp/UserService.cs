using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserCreditService _userCreditService;

        public UserService()
        {
            
            _clientRepository = new ClientRepository();

            _userCreditService = new UserCreditService();
        }

        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || !email.Contains("@") || !email.Contains("."))
                return false;

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
                age--;

            if (age < 21)
                return false;

            var client = (Client)_clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            if (client.Type == ClientType.VeryImportantClient)
                user.HasCreditLimit = false;
            else
            {
                int creditLimit = GetCreditLimit(user.LastName, user.DateOfBirth);
                if (client.Type == ClientType.ImportantClient)
                    creditLimit *= 2;

                user.CreditLimit = creditLimit;
                user.HasCreditLimit = true;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
                return false;

            UserDataAccess.AddUser(user);
            return true;
        }

        private int GetCreditLimit(string lastName, DateTime dateOfBirth)
        {
            return _userCreditService.GetCreditLimit(lastName, dateOfBirth);
        }
    }
}
