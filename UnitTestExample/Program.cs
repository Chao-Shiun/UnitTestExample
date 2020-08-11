using System;

namespace UnitTestExample
{
    class Program
    {
        static void Main(string[] args)
        {
            LiteDBTool liteDbTool = new LiteDBTool();
            UserService userService = new UserService(liteDbTool);

            bool isExists = userService.CheckUserExists("Charles", "12345");
            Console.WriteLine(isExists ? "使用者資訊存在" : "使用者資訊不存在");
            userService.RegisterUser(new Customer
            {
                Id = 10,
                Name = "Charles",
                Password = "12345",
                Phones = new[] { "0912345678", "0911223344" },
                IsActive = true
            });
            isExists = userService.CheckUserExists("Charles", "12345");
            Console.WriteLine(isExists ? "使用者資訊存在" : "使用者資訊不存在");
            Customer userInfo = userService.GetUserInfo(10);
            Console.WriteLine(nameof(userInfo.Name) + " = " + userInfo.Name);
            Console.WriteLine(nameof(userInfo.Password) + " = " + userInfo.Password);
            Console.WriteLine(nameof(userInfo.Phones) + " = " + string.Join(',', userInfo.Phones));
            string deleteResult = userService.DeleteUserInfo(10);
            Console.WriteLine(deleteResult);
            Console.ReadKey();
        }
    }
}
