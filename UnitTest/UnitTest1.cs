using ExpectedObjects;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnitTestExample;

namespace UnitTest
{
    public class Tests
    {
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            LiteDBTool liteDbTool = new LiteDBTool();
            _userService = new UserService(liteDbTool);
        }

        [Test(Author = "Charles", Description = "帳號密碼正確的情況")]
        [TestCaseSource(typeof(CustomerDataSetting), nameof(CustomerDataSetting.CheckUserWhenExists))]
        public void CheckUserWhenExists(string userName, string password)
        {
            GivenCustomerInfo(new Customer
            {
                Name = "Charles",
                Password = "12345"
            }, new Customer
            {
                Name = "David",
                Password = "55667788"
            });
            var actual = _userService.CheckUserExists(userName, password);
            Assert.IsTrue(actual);
        }

        [Test(Author = "Charles", Description = "查無帳號的情況")]
        [TestCaseSource(typeof(CustomerDataSetting), nameof(CustomerDataSetting.CheckUserWhenNotExists))]
        public void CheckUserWhenNotExists(string userName, string password)
        {
            GivenCustomerInfo(new Customer
            {
                Name = "Charles",
                Password = "1234544"
            }, new Customer
            {
                Name = "David",
                Password = "5566778855"
            });
            var actual = _userService.CheckUserExists(userName, password);
            Assert.IsFalse(actual);
        }

        [Test(Author = "Charles", Description = "使用者名稱或密碼為空的情況")]
        [TestCaseSource(typeof(CustomerDataSetting), nameof(CustomerDataSetting.CheckUserFailedWhenUserOrPasswordIsEmpty))]
        public void CheckUserWhenUserNameOrPasswordIsEmpty(string userName, string password)
        {
            var actual = Assert.Throws<ArgumentException>(() => _userService.CheckUserExists(userName, password));
            Assert.IsInstanceOf(typeof(ArgumentException), actual);
            Assert.That(actual.Message, Is.EqualTo("帳號或密碼不得為空白"));
        }

        [Test(Author = "Charles", Description = "成功取得使用者資料")]
        [TestCaseSource(typeof(CustomerDataSetting), nameof(CustomerDataSetting.GetUserInfoWhenSuccess))]
        public void GetUserInfoWhenSuccess(int id, Customer expectedResult)
        {
            GivenCustomerInfo(new Customer
            {
                Id = 10,
                Name = "Charles",
                Password = "12345",
                Phones = new[] { "0912345678", "0911223344" },
                IsActive = true
            }, new Customer
            {
                Id = 20,
                Name = "David",
                Password = "55667788",
                Phones = new[] { "0922334455", "0988888888" },
                IsActive = true
            });
            var actual = _userService.GetUserInfo(id);
            expectedResult.ToExpectedObject().ShouldMatch(actual);
        }

        [Test(Author = "Charles", Description = "查詢的id不存在")]
        [TestCase(500)]
        public void GetUserInfoFailedWhenNotExists(int id)
        {
            var actual = _userService.GetUserInfo(id);
            Assert.IsNull(actual);
        }

        [Test(Author = "Charles", Description = "id小於等於0的情況")]
        [TestCase(0)]
        [TestCase(-10)]
        public void GetUserInfoFailedWhenIdLessThanOrEqualToZero(int id)
        {
            var actual = Assert.Throws<ArgumentException>(() => _userService.GetUserInfo(id));
            Assert.IsInstanceOf(typeof(ArgumentException), actual);
            Assert.That(actual.Message, Is.EqualTo("id參數錯誤"));
        }

        [Test(Author = "Charles", Description = "新增使用者資料成功的情況")]
        [TestCaseSource(typeof(CustomerDataSetting), nameof(CustomerDataSetting.InsertUserInfo))]
        public void InsertUserInfoWhenSuccess(Customer customer)
        {
            _userService.RegisterUser(customer);
        }

        [Test(Author = "Charles", Description = "成功刪除使用者資料的情況")]
        [TestCase(10, "刪除成功")]
        public void DeleteUserInfoWhenSuccess(int id, string expectedResult)
        {
            GivenCustomerInfo(new Customer
            {
                Id = 10
            });
            var actual = _userService.DeleteUserInfo(id);
            Assert.That(actual, Is.EqualTo(expectedResult));
        }

        [Test(Author = "Charles", Description = "未成功刪除使用者資料的情況")]
        [TestCase(990, "未刪除資料")]
        public void DeleteUserInfoWhenUnsuccessful(int id, string expectedResult)
        {
            GivenCustomerInfo(new Customer
            {
                Id = 500
            });
            var actual = _userService.DeleteUserInfo(id);
            Assert.That(actual, Is.EqualTo(expectedResult));
        }


        private void GivenCustomerInfo(params Customer[] customers)
        {

        }
    }

    public class CustomerDataSetting
    {
        public static IEnumerable CheckUserWhenExists
        {
            get
            {
                yield return new TestCaseData("Charles", "12345");
                yield return new TestCaseData("David", "55667788");
            }
        }

        public static IEnumerable CheckUserWhenNotExists
        {
            get
            {
                yield return new TestCaseData("Charles1", "12345");
            }
        }

        public static IEnumerable CheckUserFailedWhenUserOrPasswordIsEmpty
        {
            get
            {
                yield return new TestCaseData("", "");
                yield return new TestCaseData("", "12345");
                yield return new TestCaseData("Charles", "");
            }
        }

        public static object[] GetUserInfoWhenSuccess
        {
            get
            {
                var cases = new List<object>
                {
                    new object[]
                    {
                        10,
                        new Customer
                        {
                            Id = 10,
                            Name = "Charles",
                            Password = "12345",
                            Phones = new[] { "0912345678", "0911223344" },
                            IsActive = true
                        }
                    },
                    new object[]
                    {
                        20,
                        new Customer
                        {
                            Id = 20,
                            Name = "David",
                            Password = "55667788",
                            Phones = new[] { "0922334455", "0988888888" },
                            IsActive = true
                        }
                    }
                };
                return cases.ToArray();
            }
        }

        public static object[] InsertUserInfo
        {
            get
            {
                var cases = new List<object>
                {
                    new object[]
                    {
                        new object[]
                        {
                            new Customer
                            {
                                Id = 10,
                                Name = "Charles",
                                Password = "12345",
                                Phones = new[] {"0912345678", "0911223344"},
                                IsActive = true
                            }
                        },
                    }
                };
                return cases.ToArray();
            }
        }
    }
}