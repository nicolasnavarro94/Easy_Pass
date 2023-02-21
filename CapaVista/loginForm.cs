using System;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class loginForm : Form
    {
        private bool createNew = false;
        public loginForm()
        {
            InitializeComponent();
        }

        private void loginForm_Load(object sender, EventArgs e)
        {
            if (CapaDatos.CD_User.userExist(out string message) == 0)
            {
                lblTitle.Text = "Crear Contraseña Maestra:";
                btnLogin.Text = "Crear";
                createNew = true;
            }else if (message != null)
            {
                MessageBox.Show(message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            login();
        }

        private void checkBoxPass_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPass.Checked)
            {
                txtPass.PasswordChar = '\0';
            }
            else
            {
                txtPass.PasswordChar = '*';
            }
        }

        public void login()
        {
            string pass = txtPass.Text.Trim();
            if (!string.IsNullOrEmpty(pass.Trim()))
            {
                int respuesta = 0;
                if (createNew)
                {
                    if (CapaDatos.CD_User.createUser(CapaDatos.Encrypt.GetSHA256(txtPass.Text),out string message))
                    {
                        Application.Restart();
                    }
                    else if(message != null)
                    {
                        MessageBox.Show(message);
                    }
                }
                else
                {
                    respuesta = CapaDatos.CD_User.userLogin(CapaDatos.Encrypt.GetSHA256(txtPass.Text),out string message);
                    if (respuesta != 0)
                    {
                        mainForm1 frm = new mainForm1(respuesta);
                        txtPass.Clear();
                        this.Hide();
                        if (frm.ShowDialog() == DialogResult.Cancel)
                        {
                            this.Close();
                        }
                    }
                    else if(message != null)
                    {
                        MessageBox.Show(message);
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.Text = "Contraseña incorrecta.";
                    }
                }
            }
            else
            {
                alertForm alert = new alertForm("Debes ingresar una contraseña!", "warning");
                alert.Show();
            }
        }

        private void txtPass_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                login();
            }
        }
    }
}
