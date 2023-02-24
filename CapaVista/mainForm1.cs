using CapaModelo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;

namespace CapaVista
{
    public partial class mainForm1 : Form
    {
        alertForm alert;
        private List<Password> passList;
        private int userId = 0;
        public mainForm1(int id)
        {
            userId = id;
            passList = new List<Password>();
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            fillData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            addForm frm = new addForm(userId,0);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                fillData();
                alert = new alertForm("Contraseña guardada con éxito!", "success");
                alert.Show();
            }
        }

        public void removeColumns()
        {
            dgPass.Columns["userId"].Visible = false;
            dgPass.Columns["pass"].Visible = false;
            dgPass.Columns["id"].Visible = false;
            dgPass.Columns["position"].Visible = false;

            dgPass.Columns["descripcion"].DisplayIndex = 0;
            dgPass.Columns["descripcion"].HeaderText = "Descripción";
        }

        private void dgPass_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 0)
                {
                    string pass = dgPass.Rows[e.RowIndex].Cells[5].Value.ToString();
                    Clipboard.SetText(Encrypt.DesEncriptar(pass));
                    alertForm alert = new alertForm("Contraseña copiada al portapapeles!","success");
                    alert.ShowDialog();
                }

                if (e.ColumnIndex == 1)
                {
                    int id = Convert.ToInt32(dgPass.Rows[e.RowIndex].Cells[3].Value);

                    if (yesNo("Seguro que desea eliminar la contraseña?","Eliminar Contraseña"))
                    {
                        if (CapaDatos.CD_Password.removePassword(id,out string message))
                        {
                            fillData();
                            alert = new alertForm("Contraseña eliminada con éxito!", "delete");
                            alert.Show();
                        }
                        else
                        {
                            MessageBox.Show(message);
                        }
                    }
                }

                if (e.ColumnIndex == 2)
                {
                    int id = Convert.ToInt32(dgPass.Rows[e.RowIndex].Cells[3].Value);
                    addForm frm = new addForm(userId, id);
                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        fillData();
                        alert = new alertForm("Contraseña editada con éxito!", "success");
                        alert.Show();
                    }
                }
               
            }
        }

        public void fillData()
        {
            passList = CapaDatos.CD_Password.getAllPasswords(userId, out string message);
            if (passList != null && passList.Count() > 0)
            {
                dgPass.Visible = true;
                dgPass.DataSource = passList;
                removeColumns();
            }
            else if (message != null)
            {
                MessageBox.Show(message);
            }
            else
            {
                dgPass.DataSource = null;
                dgPass.Visible = false;
            }
        }

        public bool yesNo(string message,string title)
        {
            bool resutl = false;
            DialogResult dr = MessageBox.Show(message,
                      title, MessageBoxButtons.YesNo);
            switch (dr)
            {
                case DialogResult.Yes: resutl = true;
                    break;
            }

            return resutl;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string keyWord = txtSearch.Text.Replace(" ","").ToLower().Trim();
            if (!string.IsNullOrEmpty(keyWord))
            {
                var search = passList.Where(C => C.descripcion.Replace(" ", "").ToLower().Contains(keyWord)).ToList();
                fillDataGrid(search);
            }
            else
            {
                fillDataGrid(passList);
            }
        }

        private void dgPass_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (changeRow)
            {
                rowIndex = e.RowIndex;
                count++;

                if (count >= 10 && Cursor.Current != Cursors.SizeAll)
                {
                    Cursor.Current = Cursors.SizeAll;
                }

            }
            
        }

        private void dgPass_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 4  && e.RowIndex >= 0)
            {
                rowPress = e.RowIndex;
                changeRow = true;

            }
        }

        private void dgPass_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (changeRow)
            {
                changeRow = false;
                count = 0;
                swapItems();
                rowIndex = -1;
                rowPress = -1;
            }
        }

        public void swapItems()
        {
            if (rowPress != rowIndex && rowIndex >= 0)
            {
                int pass2 = Convert.ToInt32(dgPass.Rows[rowIndex].Cells[3].Value);
                
                int selectedPass = Convert.ToInt32(dgPass.Rows[rowPress].Cells[3].Value);

                bool result = CapaDatos.CD_Password.changePosition(passList.Where(c => c.id == selectedPass).FirstOrDefault(),
                    passList.Where(c => c.id == pass2).FirstOrDefault(), out string message
                    );
                if (result)
                {
                    fillData();
                    alert = new alertForm("Orden Actualizado!","success");
                    alert.Show();
                }
                else if (message != null)
                {
                    MessageBox.Show(message);
                }  

            }
            
        }

        private int rowIndex = 0;
        private int rowPress = 0;
        private bool changeRow = false;
        private int count = 0;

        private void dgPass_MouseLeave(object sender, EventArgs e)
        {
            changeRow = false;
            rowIndex = -1;
            rowPress = -1;
            count = 0;
        }

    }
}
