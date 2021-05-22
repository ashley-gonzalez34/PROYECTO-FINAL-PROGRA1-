using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PROYECTO_FINAL_PROGRA1_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            LoadGrid();
        }

        SqlConnection conexion = new SqlConnection(@"Data Source=LAPTOP-PD7FSINE\\SQLEXPRESS;Initial Catalog=bd_proyecto;Integrated Security=True");
        public void clearData()
        {
            nombre_text.Clear();
            cantidad_text.Clear();
            Precio_text.Clear();
            fecha_text.Clear();
            Buscar_text.Clear();
        }
        public void LoadGrid()// esta parte es donde se muestran los datos que se han insetado para la base de datos 
        {
            SqlCommand cdm = new SqlCommand("select  * from tb_articulos", conexion);
            DataTable datos = new DataTable();
            conexion.Open();//abre la conexion
            SqlDataReader sdr = cdm.ExecuteReader();
            datos.Load(sdr);
            conexion.Close();//cierra la conexion 
            DataGrid.ItemsSource = datos.DefaultView;


        }
        private void Button_ClearData_Click(object sender, RoutedEventArgs e)//este boton limpiar los texbox 
        {
            clearData();
        }
        public bool Valido()//esta parte lo que hace es llenar los registros con los datos y si esa informacion no esta le da un aviso de que no hay info.
        {
            if (nombre_text.Text == string.Empty)

            {
                MessageBox.Show("Ingrese el nombre del articulos ", "No hay Informacion", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            if (cantidad_text.Text == string.Empty)

            {
                MessageBox.Show("Ingrese el registro que hay de los articulos ", "No hay Informacion", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            if (Precio_text.Text == string.Empty)

            {
                MessageBox.Show("Ingrese el precio del articulo", "No hay Informacion", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            if (fecha_text.Text == string.Empty)

            {
                MessageBox.Show("Ingrese la fecha  de cuando se vendieron los articulos  ", "No hay Informacion", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

            }
            return true;
        }

        private void Button_Create_Click(object sender, RoutedEventArgs e)//aca es cuando se insertan los datos
        {
            try
            {
                if (Valido())//este if lo que hace es cuando se inserta los datos a los textbox los reconoce y los envia al datagrid los muestra
                {
                    SqlCommand cdm = new SqlCommand("Insert into tb_articulos values(@Nombre,@Cantidad,@Precio,@Fecha )", conexion);//aca reconocera las filas de la base de datos de SQl Server
                    cdm.CommandType = CommandType.Text;
                    cdm.Parameters.AddWithValue("@Nombre", nombre_text.Text);
                    cdm.Parameters.AddWithValue("@Cantidad", cantidad_text.Text);
                    cdm.Parameters.AddWithValue("@Precio", Precio_text.Text);
                    cdm.Parameters.AddWithValue("@Fecha", fecha_text.Text);

                    conexion.Open();//conexion abierta de la base de datos 
                    cdm.ExecuteNonQuery();
                    conexion.Close();//programa se cierra
                    LoadGrid();
                    MessageBox.Show("Se ha registrado exitosamente", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                    clearData();


                }

            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)//este botn lo que va a hacer es borrar el dato que nosotros querramos 
        {
            conexion.Open();//abre conexion
            SqlCommand cmd = new SqlCommand("delete from tb_articulos where ID= " + Buscar_text.Text + "", conexion);//esta parte lo que hara es poder eliminar el dato que querramos especificando con el correlativo
            try
            {
                cmd.ExecuteNonQuery();
                MessageBox.Show("Este registro se ha eliminado ", "Eliminado", MessageBoxButton.OK, MessageBoxImage.Information);
                conexion.Close();
                clearData();//limpia
                LoadGrid();
                conexion.Close();//cierra conexion


            }
            catch (SqlException ex)//este catch avisa que no se ha eliminado el dato
            {
                MessageBox.Show("No ha sido eliminado" + ex.Message);

            }
            finally
            {
                conexion.Close();

            }
        }

        private void Button_Update_Click(object sender, RoutedEventArgs e)//el botn de actualizar restablece los datos enseñando todo lo que se ha agregado en la base de datos
        {
            conexion.Open();
            SqlCommand cdm = new SqlCommand("Update tb_articulos set nombre = '" + nombre_text.Text + "', cantidad = '" + cantidad_text.Text + "', precio = '" + Precio_text.Text + "', fecha = '" + fecha_text.Text + "'WHERE ID='" + Buscar_text.Text + "'", conexion);

            try
            {
                cdm.ExecuteNonQuery();
                MessageBox.Show("El registro se ha actualizado correctamente", "update", MessageBoxButton.OK, MessageBoxImage.Information);//da un aviso que se actualizaron los datos
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);

            }
            finally
            {
                conexion.Close();
                clearData();
                LoadGrid();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string codigo = idmysql_text.Text;
            string nombre = nombremysql_text.Text;
            string cantidad = cantidadmysql_text.Text;
            string preciop = Precio_text.Text;
            string fecha = fechamysq_text.Text;
            string centencia = $"INSERT INTO restuarante.ordenes (codigo, nombre, cantidad, precio,fecha) VALUES " + $" ('{codigo}','{nombre}','{cantidad}','{preciop}','{fecha}');\n ";
            ClsConexionMySQL cn = new ClsConexionMySQL();
            cn.EjecutaMySQLDirecto(centencia);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string id = idmysql_text.Text;


            ClsConexionMySQL cn = new ClsConexionMySQL();
            string query = $"select* from restuarante.ordenes where codigo ={id}";
            DataTable dt = cn.consultaTablaDirecta(query);
            if (dt.Rows.Count > 0)
            {
                string nombre = dt.Rows[0].Field<string>("nombre");
                int precio = dt.Rows[0].Field<Int32>("precio");
                int cantidad = dt.Rows[0].Field<Int32>("cantidad");

                nombremysql_text.Text = nombre;
                preciomysql_text.Text = precio + "";
                cantidadmysql_text.Text = cantidad + "";
            }
        } // Boton Buscar, se utiliza para buscar los datos ingresados en MYSQL

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string ID = idmysql_text.Text;
            string nombre = nombremysql_text.Text;
            String centencia = $"Update  restuarante.ordenes set Codigo ='{ID}' where codigo ='{nombre}'";
            ClsConexionMySQL cn = new ClsConexionMySQL();
            cn.EjecutaMySQLDirecto(centencia);
        } // Boton Actualizar, actualiza los datos llamados en MYSQL

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            string codigo = idmysql_text.Text;
            string centencia = $" delete from restaurante.ordenes where codigo ={codigo}";
            ClsConexionMySQL cn = new ClsConexionMySQL();
            cn.EjecutaMySQLDirecto(centencia);
        } //    Boton Eliminar, elimina los datos ingresados en MYSQL
    }
}
        
    
    

    






    




        

        
