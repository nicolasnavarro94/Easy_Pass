using CapaModelo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CapaDatos
{
    public static class CD_Password
    {

        public static bool addNewPassword(Password password, out string message)
        {
            message = null;
            using (var db = new easyPassEntities())
            {
                try
                {
                    if (db.Password.Max(c => c.position) == null)
                    {
                        password.position = 0;
                    }
                    else
                    {
                        password.position = db.Password.Max(c => c.position + 1);
                    }
                    db.Password.Add(password);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }
        }


        public static bool updatePassword(Password password, out string message)
        {
            message = null;
            using (var db = new easyPassEntities())
            {
                try
                {
                    var pass = db.Password.Where(c => c.id == password.id).FirstOrDefault();
                    pass.descripcion = password.descripcion;
                    pass.pass = password.pass;
                    db.Password.AddOrUpdate(pass);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }
        }


        public static List<Password> getAllPasswords(int id,out string message)
        {
            message = null;
            using (var db = new easyPassEntities())
            {
                return db.Password.Where(c => c.userID == id).OrderBy(c=>c.position).ToList();
            }
        }


        public static Password getPassword(int id, out string message)
        {
            message = null;
            using (var db = new easyPassEntities())
            {
                return db.Password.Where(c => c.id == id).FirstOrDefault();
            }
        }


        public static bool removePassword(int id,out string message)
        {
            message = null;
            using (var db = new easyPassEntities())
            {
                try
                {
                    var pass = db.Password.Where(c => c.id == id).FirstOrDefault();
                    db.Password.Remove(pass);
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }
        }


        public static bool changePosition(Password selectedPass, Password pass2, out string message)
        {
            message = null;
            int sum = 1;
            using (var db = new easyPassEntities())
            {
                try
                {
                    var pass = db.Password.Where(c => c.id == selectedPass.id).FirstOrDefault();
                    pass.position = pass2.position;
                    db.SaveChanges();
                    var passDecending = db.Password.Where(c => c.position > pass2.position).ToList();

                    foreach (var item in passDecending)
                    {
                        item.position = item.position +1;
                    }
                    if (selectedPass.position < pass2.position)
                    {
                        sum = -1;
                    }
                    var pass_2 = db.Password.Where(c => c.id == pass2.id).FirstOrDefault();
                    pass_2.position = pass2.position + sum;
                    db.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    return false;
                }
            }
        }
    }
}
