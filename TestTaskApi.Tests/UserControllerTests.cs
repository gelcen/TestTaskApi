using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestTaskApi.Models;
using Xunit;

namespace TestTaskApi.Tests
{
    public class UserControllerTests
    {
        [Fact]
        public async Task GetUsers_ReturnsUsers()
        {
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(databaseName: "Test_GetUsers")
                .Options;
            var context = new UserContext(options);

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });
            context.Users.Add(new User
            {
                Name = "User2",
                Surname = "Sur2",
                BirthDate = new DateTime(2002, 02, 22)
            });
        }
    }
}
