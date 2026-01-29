using Microsoft.Data.SqlClient;
using MvcCoreLinqToSql.Models;
using System.Data;

namespace MvcCoreLinqToSql.Repositories
{
    public class RepositoryEmpleados
    {
        private DataTable tablaEmpleados;

        public RepositoryEmpleados()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            string sql = "SELECT * FROM EMP";
            // AQUI ES DONDE CREAMOS EL ADAPTADOR PUENTE ENTRE EL SQL SERVER Y LINQ
            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaEmpleados = new DataTable();
            // TENEMOS LOS DATOS PARA EL LINQ
            ad.Fill(this.tablaEmpleados);
        }

        public List<Empleado> GetEmpleados()
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable() select datos;
            /* AHORA MISMO TENEMOS DENTRO DE CONSULTA INFORMACION DE LOS EMPLEADOS
             * LOS DATOS VIENEN EN FORMATO TABLA, CADA ELEMENTO DE UNA TABLA ES UNA FILA (DataRow)
             * DEBEMOS RECORRER LAS FILAS, EXTRAERLAS Y CONVERTIRLAS A NUESTRO MODEL Empleado */
            List<Empleado> empleados = new List<Empleado>();
            // RECORREMOS CADA FILA DE LA CONSULTA
            foreach(var fila in consulta)
            {
                Empleado emp = new Empleado();
                emp.IdEmpleado = fila.Field<int>("EMP_NO");
                emp.Apellido = fila.Field<string>("APELLIDO");
                emp.Oficio = fila.Field<string>("OFICIO");
                emp.Salario = fila.Field<int>("SALARIO");
                emp.IdDepartamento = fila.Field<int>("DEPT_NO");
                empleados.Add(emp);
            }

            return empleados;
        }

        public Empleado GetEmpleadoById(int idEmpleado)
        {
            // FILTRAMOS NUESTRA CONSULTA
            var consulta = from datos in this.tablaEmpleados.AsEnumerable() where datos.Field<int>("EMP_NO") == idEmpleado select datos;
            /* NOSOTROS SABEMOS QUE NUESTRA CONSULTA DEVUELVE UNA FILA PERO Linq NO,
             * DENTRO DE ESTE CONJUNTO TENEMOS METODOS LAMBDA PARA HACER COSITAS
             * POR EJEMPLO PODRIAMOS CONTAR, SABER EL MAXIMO O RECUPERARA EL PRIMER ELEMENTO */
            var fila = consulta.First();
            Empleado emp = new Empleado();
            emp.IdEmpleado = fila.Field<int>("EMP_NO");
            emp.Apellido = fila.Field<string>("APELLIDO");
            emp.Oficio = fila.Field<string>("OFICIO");
            emp.Salario = fila.Field<int>("SALARIO");
            emp.IdDepartamento = fila.Field<int>("DEPT_NO");
            return emp;
        }
    }
}
