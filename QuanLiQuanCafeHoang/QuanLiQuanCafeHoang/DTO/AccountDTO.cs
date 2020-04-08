using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLiQuanCafeHoang.DTO
{
    public class AccountDTO
    {
        private string username;
        private string displayname;
        private string password;
        private int type;

        public string Username { get => username; set => username = value; }
        public string Displayname { get => displayname; set => displayname = value; }
        public string Password { get => password; set => password = value; }
        public int Type { get => type; set => type = value; }
        public AccountDTO()
        {

        }
        public AccountDTO(string username, string displayname, string password, int type)
        {
            Username = username;
            Displayname = displayname;
            Password = password;
            Type = type;
        }
        public AccountDTO(DataRow dataRow)
        {
            Username = dataRow["UserName"].ToString();
            Displayname = dataRow["displayname"].ToString(); 
            Password = dataRow["password"].ToString(); 
            Type = (int)dataRow["type"];
        }
    }
}
