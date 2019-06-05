using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TestTaskApi.Controllers;
using TestTaskApi.Models;
using Xunit;

namespace TestTaskApi.Tests
{
    public class UserControllerTests
    {
        private ServiceProvider _provider;

        public UserControllerTests()
        {
            var services = new ServiceCollection();

            services.AddDbContext<UserContext>(
                options =>
                    options.UseInMemoryDatabase($"db-{Guid.NewGuid()}"),
                ServiceLifetime.Transient
                );

            _provider = services.BuildServiceProvider();
        }

        [Fact]
        public async Task GetUsers_DbHas2Users_Returns2Users()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

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

            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUsers();

            // Assert
            var users = Assert.IsType<List<User>>(result.Value);
            Assert.Equal(2, users.Count);

            context.Dispose();
        }

        [Fact]
        public async Task GetUser_GoodId_ReturnsUser()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });

            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUser(1);

            // Assert
            var user = Assert.IsType<User>(result.Value);
            Assert.Equal("User1", user.Name);
            Assert.Equal("Sur1", user.Surname);
            Assert.Equal(0, user.Balance);

            context.Dispose();
        }

        [Fact]
        public async Task GetUser_WrongId_ReturnsNotFound()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });

            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.GetUser(4);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);            

            context.Dispose();
        }

        [Fact]
        public async Task RegisterUser_GoodUser_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            var controller = new UsersController(context);

            var user = new User
            {
                Name = "AddedUser",
                Surname = "AddedSurname",
                BirthDate = new DateTime(1999, 04, 02)
            };



            // Act
            var result = await controller.RegisterUser(user);

            // Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);

            var addedUser = await controller.GetUsers();

            var users = Assert.IsType<List<User>>(addedUser.Value);
            Assert.Single(users);

            Assert.Equal("AddedUser", users[0].Name);
            Assert.Equal("AddedSurname", users[0].Surname);

            context.Dispose();
        }

        [Fact]
        public async Task RegisterUser_UserNotNullId_ReturnsBadRequest()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            var controller = new UsersController(context);

            var user = new User
            {
                Id = 1,
                Name = "AddedUser",
                Surname = "AddedSurname",
                BirthDate = new DateTime(1999, 04, 02)
            };



            // Act
            var result = await controller.RegisterUser(user);

            // Assert
            Assert.IsType<BadRequestResult>(result.Result);

            context.Dispose();
        }

        [Fact]
        public void RegisterUser_NotValidUser_ModelStateIsValidFalse()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            var controller = new UsersController(context);                   

            // Act            
            controller.ModelState.AddModelError("User is not valid", "User has null name");

            // Assert
            Assert.False(controller.ModelState.IsValid);

            context.Dispose();
        }

        [Fact]
        public async Task DeleteUser_GoodId_ReturnsNoContent()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

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
                BirthDate = new DateTime(2000, 02, 02)
            });

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });

            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.DeleteUser(2);

            // Assert
            Assert.IsType<NoContentResult>(result);

            context.Dispose();
        }

        [Fact]
        public async Task DeleteUser_GoodId_ReturnsNotFound()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

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
                BirthDate = new DateTime(2000, 02, 02)
            });

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });

            await context.SaveChangesAsync();

            var controller = new UsersController(context);

            // Act
            var result = await controller.DeleteUser(20);

            // Assert
            Assert.IsType<NotFoundResult>(result);

            context.Dispose();
        }

        [Fact]
        public async Task BalanceOperation_WrongId_ReturnsNotFound()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });


            await context.SaveChangesAsync();

            var operation = new Operation
            {
                OperationType = OperationType.Add,
                Sum = 50
            };

            var controller = new UsersController(context);

            // Act
            var result = await controller.BalanceOperation(2, operation);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);

            context.Dispose();
        }

        [Fact]
        public async Task BalanceOperation_GoodIdAdd_ReturnsUser()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });


            await context.SaveChangesAsync();

            var operation = new Operation
            {
                OperationType = OperationType.Add,
                Sum = 50
            };

            var controller = new UsersController(context);

            // Act
            var result = await controller.BalanceOperation(1, operation);

            // Assert
            var userWithNewBalance = Assert.IsType<User>(result.Value);
            Assert.Equal(50M, userWithNewBalance.Balance);

            context.Dispose();
        }

        [Fact]
        public async Task BalanceOperation_GoodWithdraw_ReturnsUser()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });


            await context.SaveChangesAsync();

            var operation = new Operation
            {
                OperationType = OperationType.Add,
                Sum = 50
            };

            var controller = new UsersController(context);

            await controller.BalanceOperation(1, operation);

            // Act
            var result = await controller.BalanceOperation(1,
                new Operation
                {
                    OperationType = OperationType.Withdraw,
                    Sum = 25
                });

            // Assert
            var userWithNewBalance = Assert.IsType<User>(result.Value);
            Assert.Equal(25M, userWithNewBalance.Balance);

            context.Dispose();
        }

        [Fact]
        public async Task BalanceOperation_BadWithdraw_ReturnsBadRequest()
        {
            // Arrange
            var context = _provider.GetService<UserContext>();

            context.Users.Add(new User
            {
                Name = "User1",
                Surname = "Sur1",
                BirthDate = new DateTime(2000, 02, 02)
            });


            await context.SaveChangesAsync();

            var operation = new Operation
            {
                OperationType = OperationType.Add,
                Sum = 50
            };

            var controller = new UsersController(context);

            await controller.BalanceOperation(1, operation);

            // Act
            var result = await controller.BalanceOperation(1,
                new Operation
                {
                    OperationType = OperationType.Withdraw,
                    Sum = 75
                });

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);

            context.Dispose();
        }
    }
}
