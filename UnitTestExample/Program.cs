using System;

namespace UnitTestExample
{
    class Program
    {
        static void Main(string[] args)
        {
            LiteDBTool liteDbTool=new LiteDBTool();
            UserService userService = new UserService(liteDbTool);
            
            bool isExists = userService.CheckUserExists("Charles", "12345");
            userService.RegisterUser(new Customer
            {
                Id = 10,
                Name = "Charles",
                Password = "12345",
                Phones = new[] { "0912345678", "0911223344" },
                IsActive = true
            });
            isExists = userService.CheckUserExists("Charles", "12345");
            Customer userInfo = userService.GetUserInfo(10);
            string isDelete = userService.DeleteUserInfo(10);
        }
    }
}
