using System;
using System.Collections.Generic;
using System.Text;
using TestTaskApi.Controllers;
using TestTaskApi.Models;

namespace TestTaskApi.Tests
{
    public class TestingThread
    {
        private UsersController _controller;

        private int _a;

        private int _b;

        public TestingThread(UsersController controller,
                            int a,
                            int b)
        {
            _controller = controller;
            _a = a;
            _b = b;
        }

        public async void TestBalance()
        {
            for (int i = _a; i <= _b; i++)
            {
                await _controller.BalanceOperation(i, new Operation
                {
                    OperationType = OperationType.Add,
                    Sum = 50
                });
                await _controller.BalanceOperation(i, new Operation
                {
                    OperationType = OperationType.Withdraw,
                    Sum = 25
                });
            }
        }
    }
}
