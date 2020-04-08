using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get  {if (instance == null)instance = new AccountDAO(); return instance;}
            private set{ instance = value;}
        }
        private AccountDAO() { }
        public bool Login(string username, string password)
        {
            string query = "USP_Login @UserName , @PassWord";
            DataTable result = DataProvider.Instance.ExcuteQuery(query, new object[] {username,password});
            return result.Rows.Count > 0;
        }
        public AccountDTO GetAccountByUsername(string userName)
        {
            DataTable dataTable = DataProvider.Instance.ExcuteQuery("SELECT * FROM dbo.Account WHERE UserName = '"+userName+ "'");
            foreach (DataRow item in dataTable.Rows)
            {
                return new AccountDTO(item);

            }
            return null;
        }
        public bool UpdateAccountInfo(string username, string dispalyname,string pass,string newpass)
        {
            string query = "EXEC USP_UpdateAccount @username , @displayname , @password , @newpassword";

            int result = DataProvider.Instance.ExcuteNonQuery(query, new object[] { username, dispalyname, pass, newpass });

            return result > 0;
        }
        public List<AccountDTO> GetListAccount()
        {
            List<AccountDTO> accountDTOs = new List<AccountDTO>();
            string query = "SELECT * FROM dbo.Account";
            DataTable dataTable = DataProvider.Instance.ExcuteQuery(query);
            foreach (DataRow item in dataTable.Rows)
            {
                AccountDTO accountDTO = new AccountDTO(item);
                accountDTOs.Add(accountDTO);
            }
            return accountDTOs;
        }
        public bool InsertAccount(string username, string displayname, int type)
        {
            string query = string.Format("INSERT INTO dbo.Account ( UserName,DisplayName,PassWord,Type) VALUES(   N'{0}', N'{1}', N'{2}' ,{3})", username, displayname, 0, type);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool UpdateAccount(string username, string displayname, int type)
        {
            string query = string.Format("UPDATE dbo.Account SET DisplayName = N'{0}', Type = {1} WHERE UserName =N'{2}'", displayname, type, username);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool DeleteAccount(string username)
        {
            string query = string.Format("DELETE dbo.Account WHERE UserName =N'{0}'", username);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
        public bool ResetPass(string username)
        {
            string query = string.Format("UPDATE dbo.Account SET PassWord = N'{0}' WHERE UserName =N'{1}'", 0, username);
            int result = DataProvider.Instance.ExcuteNonQuery(query);
            return result > 0;
        }
    }
}
