using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Windows.Forms;

namespace MultiFaceRec
{
    class Conexiones
    {

        public SqlConnection conexion;

        //Definir conexión
        public void Inicializar()
        {
            string dircon = @"Data Source=DESKTOP-47369CL\SEMINARIO;Initial Catalog=master;Integrated Security=True";
            conexion = new SqlConnection(dircon);
        }

        //Abrir la conexión
        public bool AbrirCon()
        {
            try
            {
                conexion.Open();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
                return false;
            }
        }

        //Cerrar la conexión
        public bool CerrarCon()
        {
            try
            {
                conexion.Close();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
                return false;
            }
        }

        //Insertar
        public void Insertar(string tabla, string seters, string valores)
        {
            try
            {
                string query = "INSERT INTO " + tabla + " (" + seters + ") VALUES (" + valores + ")";
                AbrirCon();

                SqlCommand comando = new SqlCommand(query, conexion);

                //Control de query para testeo de errores
                MessageBox.Show(query);

                comando.ExecuteNonQuery();
                MessageBox.Show("Nuevo " + tabla + " guardado");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
            finally
            {
                this.CerrarCon();
            }
        }

        //Actualizar
        public void Update(string tabla, int id, string valores, string key)
        {
            try
            {
                string query = "UPDATE " + tabla + " SET " + valores + " WHERE " + key + " = " + id + "";
                AbrirCon();

                SqlCommand comando = new SqlCommand(query, conexion);

                //Control de query para testeo de errores
                //MessageBox.Show(query);

                comando.ExecuteNonQuery();
                MessageBox.Show(tabla + " actualizado");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
            finally
            {
                this.CerrarCon();
            }
        }

        //Eliminar
        public void Delete(string tabla, int valor, string campo)
        {
            try
            {
                string query = "DELETE * FROM " + tabla + " WHERE " + campo + " = " + valor + "";
                AbrirCon();

                SqlCommand comando = new SqlCommand(query, conexion);

                //Control de query para testeo de errores
                //MessageBox.Show(query);

                comando.ExecuteNonQuery();
                MessageBox.Show(tabla + " eliminado");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error " + ex);
            }
            finally
            {
                this.CerrarCon();
            }
        }


    }
}
