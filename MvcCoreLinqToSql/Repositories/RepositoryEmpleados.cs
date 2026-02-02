using Microsoft.Data.SqlClient;
using MvcCoreLinqToSql.Models;
using System.Data;
using System.Security.Cryptography;

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

        public List<Empleado> GetEmpleadosOficioSalario(string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;
            if(consulta.Count() == 0)
            {
                return null;
            }
            List<Empleado> empleados = new List<Empleado>();
            foreach(var fila in consulta)
            {
                Empleado emp = new Empleado
                {
                    IdEmpleado = fila.Field<int>("EMP_NO"),
                    Apellido = fila.Field<string>("APELLIDO"),
                    Oficio = fila.Field<string>("OFICIO"),
                    Salario = fila.Field<int>("SALARIO"),
                    IdDepartamento = fila.Field<int>("DEPT_NO"),
                };
                empleados.Add(emp);
            }
            return empleados;
        }

        public ResumenEmpleados GetResumenEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;
            // SI NO HAY DATOS HAY Q CONTROLARLO
            if (consulta.Count() == 0)
            {
                // VALORES NEUTROS
                ResumenEmpleados resumen = new ResumenEmpleados
                {
                    Personas = 0,
                    MaximoSalario = 0,
                    MediaSalarial = 0,
                    Empleados = null
                };
                return resumen;
            }
            else
            {
                // QUIERO ORDENADO EMPLEADOS POR SU SALARIO
                consulta = consulta.OrderBy(z => z.Field<int>("SALARIO"));
                int personas = consulta.Count();

                int maximo = consulta.Max(z => z.Field<int>("SALARIO"));
                double media = consulta.Average(x => x.Field<int>("SALARIO"));
                List<Empleado> empleados = new List<Empleado>();
                foreach (var fila in consulta)
                {
                    Empleado emp = new Empleado
                    {
                        IdEmpleado = fila.Field<int>("EMP_NO"),
                        Apellido = fila.Field<string>("APELLIDO"),
                        Oficio = fila.Field<string>("OFICIO"),
                        Salario = fila.Field<int>("SALARIO"),
                        IdDepartamento = fila.Field<int>("DEPT_NO"),
                    };
                    empleados.Add(emp);
                }
                ResumenEmpleados resumen = new ResumenEmpleados
                {
                    Personas = personas,
                    MaximoSalario = maximo,
                    MediaSalarial = media,
                    Empleados = empleados
                };
                return resumen;
            }
        }

        public List<string> GetOficios()
        {
            var consulta = (from datos in this.tablaEmpleados.AsEnumerable()
                           select datos.Field<string>("OFICIO")).Distinct();
            // AHORA MISMO YA TENEMOS LO QUE NECSITAMOS, UN CONJUNTO DE STRING
            // LA NORMA SUELE SER DEVOLVER LA COLECCION GENERICA List<>
            return consulta.ToList();
        }
    }
}
