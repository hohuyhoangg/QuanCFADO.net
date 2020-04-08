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
    public partial class fLogin : Form
    {
        public fLogin()
        {
            InitializeComponent();
        }
        #region Ấn nút thoát
        private void fLogin_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình?", "Thông báo", MessageBoxButtons.OKCancel) != DialogResult.OK)
                e.Cancel = true;
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion

        #region Đăng nhập
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txbUsernameLogin.Text;
            string password = txbPassword.Text;
            if (Login(username,password))
            {
                AccountDTO accountDTO = AccountDAO.Instance.GetAccountByUsername(username);
                fTableManager f = new fTableManager(accountDTO);
                this.Hide();
                f.ShowDialog();
                this.Show();
                txbPassword.Text = "";
            }
            else
            {
                MessageBox.Show("Tên tài khoản hoặc mật khẩu sai!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
        bool Login(string username, string password)
        {
            return AccountDAO.Instance.Login(username, password);
        }
        #endregion
    }
}
