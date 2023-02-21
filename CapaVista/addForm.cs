using CapaDatos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaVista
{
    public partial class addForm : Form
    {
        alertForm alert;
        private int _id = 0;
        private int _idPass = 0;
        public addForm(int id,int idPass)
        {
            _id = id;
            _idPass = idPass;
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (validate())
            {
                if (_idPass == 0)
                {
                    CapaModelo.Password pass = new CapaModelo.Password();
                    pass.descripcion = txtDescripcion.Text;
                    pass.pass = Encrypt.Encriptar(txtPass.Text);
                    pass.userID = _id;

                    if (CD_Password.addNewPassword(pass,out string message))
                    {
                        DialogResult = DialogResult.OK;
                        this.Close();
                    }
                    else if(message != null)
                    {
                        MessageBox.Show(message);
                        DialogResult = DialogResult.Cancel;
                    }
                }
                else
                {
                    CapaModelo.Password pass = new CapaModelo.Password();
                    pass.descripcion = txtDescripcion.Text;
                    pass.pass = Encrypt.Encriptar(txtPass.Text);
                    pass.id = _idPass;
                    pass.userID = _id;

                    if (CD_Password.updatePassword(pass,out string message))
                    {
                            DialogResult = DialogResult.OK;
                            this.Close();
                    }
                    else if(message != null)
                    {
                        MessageBox.Show(message);
                        DialogResult = DialogResult.Cancel;
                    }
                }
            }
            else
            {
                alert = new alertForm("Debes llenar todos los campos!","warning");
                alert.Show();
            }
            
        }

        private void addForm_Load(object sender, EventArgs e)
        {
            if (_idPass != 0)
            {
                CapaModelo.Password paswords = new CapaModelo.Password();
                paswords = CD_Password.getPassword(_idPass,out string message);
                if (paswords != null)
                {
                    txtDescripcion.Text = paswords.descripcion;
                    txtPass.Text = Encrypt.DesEncriptar(paswords.pass);
                }
                else if(message != null)
                {
                    MessageBox.Show(message);
                    DialogResult = DialogResult.Cancel;
                }

                btnAdd.Text = "Guardar Cambios";
                lblTitle.Text = "Editar Contraseña:";
                this.Text = "Editar Contraseña";
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
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

        public bool validate()
        {
            string txtP = txtPass.Text.Trim();
            string txtD = txtDescripcion.Text.Trim();
            if (!string.IsNullOrEmpty(txtP.Trim()) && !string.IsNullOrEmpty(txtD.Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
