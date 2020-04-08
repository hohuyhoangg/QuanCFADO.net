using QuanLiQuanCafeHoang.DAO;
using QuanLiQuanCafeHoang.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLiQuanCafeHoang
{
    public partial class fAccountProfile : Form
    {
        private AccountDTO loginAccountDTO;
        public AccountDTO LoginAccountDTO
        {
            get => loginAccountDTO;
            set
            {
                this.loginAccountDTO = value;
                ChangeAccount(LoginAccountDTO);
            }
        }
        public fAccountProfile( AccountDTO accountDTO)
        {
            InitializeComponent();
            LoginAccountDTO = accountDTO;
        }
        #region Event

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void bUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
        /// <summary>
        /// Tạo 1 Event bạn mong muốn
        /// </summary>
        private event EventHandler<AccountEvent> updateAccount;
        public event EventHandler<AccountEvent> UpdateAccount { 
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }
        #endregion
        #region Methods
        void ChangeAccount(AccountDTO account)
        {
            txbUsername.Text = LoginAccountDTO.Username;
            tbDisplayname.Text = LoginAccountDTO.Displayname;
        }
        void UpdateAccountInfo()
        {
            string username = txbUsername.Text;
            string displayname = tbDisplayname.Text;
            string password = txbPassword.Text;
            string newpassword = txbNewPassword.Text;
            string renterpassword = txbRenterpassword.Text;
            if(!newpassword.Equals(renterpassword))
            {
                MessageBox.Show("Mật khẩu mới và nhập lại không trùng khớp");
            }
            else
            {
                if (AccountDAO.Instance.UpdateAccountInfo(username, displayname, password, newpassword))
                {
                    MessageBox.Show("Bạn đã thay đổi thành công");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUsername(username)));
                }
                else
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
            }
        }
        #endregion

       
    }
    public class AccountEvent: EventArgs
    {
        private AccountDTO accountDTO;

        public AccountDTO AccountDTO { get => accountDTO; set => accountDTO = value; }

        public AccountEvent(AccountDTO accountDTO)
        {
            AccountDTO = accountDTO;
        }
    }
}
