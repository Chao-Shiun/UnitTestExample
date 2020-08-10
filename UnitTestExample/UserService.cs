using System;

namespace UnitTestExample
{
    public class UserService
    {
        private readonly LiteDBTool _liteDbTool;
        public UserService(LiteDBTool liteDbTool)
        {
            _liteDbTool = liteDbTool;
            _liteDbTool.DeleteAll<Customer>();
        }

        public bool CheckUserExists(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("帳號或密碼不得為空白");
            }

            var isExists = _liteDbTool.Exists(x => x.Name == userName && x.Password == password);

            return isExists;
        }

        public Customer GetUserInfo(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException($"{nameof(id)}參數錯誤");
            }

            var result = _liteDbTool.GetData<Customer>(x => x.Id == id);
            return result;
        }

        public void RegisterUser(Customer customer)
        {
            _liteDbTool.Insert(customer);
        }

        public string DeleteUserInfo(int id)
        {
            var isDelete = _liteDbTool.Delete<Customer>(id);
            return isDelete ? "刪除成功" : "未刪除資料";
        }
    }
}