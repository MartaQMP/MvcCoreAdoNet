using Microsoft.Data.SqlClient;
using MvcPracticaFinalPlantilla.Models;
using System.Data;

#region STORED PROCEDURES
/*
---------------------------------------------
--INSERTO SI NO EXISTE, ACTUALIZO SI EXISTE--
---------------------------------------------

    CREATE PROCEDURE SP_PLANTILLA_UPSERT(@hospital_cod int, @sala_cod int, @empleado_no int, @apellido nvarchar(50), @funcion nvarchar(50), @salario int)
    AS
    BEGIN
        IF EXISTS (SELECT 1 FROM PLANTILLA WHERE EMPLEADO_NO = @empleado_no)
        BEGIN
            UPDATE PLANTILLA 
            SET HOSPITAL_COD = @hospital_cod, 
                SALA_COD = @sala_cod, 
                APELLIDO = @apellido, 
                FUNCION = @funcion, 
                SALARIO = @salario 
            WHERE EMPLEADO_NO = @empleado_no;
        END
        ELSE 
        BEGIN
            INSERT INTO PLANTILLA (HOSPITAL_COD, SALA_COD, EMPLEADO_NO, APELLIDO, FUNCION, SALARIO)
            VALUES (@hospital_cod, @sala_cod, @empleado_no, @apellido, @funcion, @salario);
        END
    END
    GO

*/
#endregion

namespace MvcPracticaFinalPlantilla.Repositories
{
    public class RepositoryPlantilla
    {
        SqlConnection cn;
        SqlCommand com;
        DataTable tablaPlantilla;

        public RepositoryPlantilla() 
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "SELECT * FROM PLANTILLA";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            this.tablaPlantilla = new DataTable();
            ad.Fill(tablaPlantilla);
        }

        public List<Plantilla> GetPlantilla()
        {
            var consulta = from datos in tablaPlantilla.AsEnumerable()
                           select datos;
            List<Plantilla> plantillas = new List<Plantilla>();
            foreach(var fila in consulta)
            {
                Plantilla pla = new Plantilla
                {
                    HospitalCod = fila.Field<int>("HOSPITAL_COD"),
                    SalaCod = fila.Field<int>("SALA_COD"),
                    EmpleadoNo = fila.Field<int>("EMPLEADO_NO"),
                    Apellido = fila.Field<string>("APELLIDO"),
                    Funcion = fila.Field<string>("FUNCION"),
                    Turno = fila.Field<string>("T"),
                    Salario = fila.Field<int>("SALARIO"),
                };
                plantillas.Add(pla);
            }
            return plantillas;
        }

        public List<string> GetFunciones()
        {
            var consulta = (from datos in tablaPlantilla.AsEnumerable()
                            select datos.Field<string>("FUNCION")).Distinct();
            return consulta.ToList();
        }

        public ResumenPlantilla GetPlantillaByFuncion(string funcion)
        {
            var consulta = from datos in tablaPlantilla.AsEnumerable()
                           where datos.Field<string>("FUNCION") == funcion
                           select datos;
            if (consulta.Count() == 0)
            {
                ResumenPlantilla resumen = new ResumenPlantilla
                {
                    Personas = 0,
                    MaximoSalario = 0,
                    MediaSalarial = 0,
                    Plantilla = null
                };
                return resumen;
            }
            else
            {
                int personas = consulta.Count();
                int maximo = consulta.Max(z => z.Field<int>("SALARIO"));
                double media = consulta.Average(x => x.Field<int>("SALARIO"));
                List<Plantilla> plantillas = new List<Plantilla>();
                foreach (var fila in consulta)
                {
                    Plantilla pla = new Plantilla
                    {
                        HospitalCod = fila.Field<int>("HOSPITAL_COD"),
                        SalaCod = fila.Field<int>("SALA_COD"),
                        EmpleadoNo = fila.Field<int>("EMPLEADO_NO"),
                        Apellido = fila.Field<string>("APELLIDO"),
                        Funcion = fila.Field<string>("FUNCION"),
                        Turno = fila.Field<string>("T"),
                        Salario = fila.Field<int>("SALARIO"),
                    };
                    plantillas.Add(pla);
                }
                ResumenPlantilla resumen = new ResumenPlantilla
                {
                    Personas = personas,
                    MaximoSalario = maximo,
                    MediaSalarial = media,
                    Plantilla = plantillas
                };

                return resumen;
            }
        }

        public Plantilla GetEmpleadoPlantillaById(int empleado_no)
        {
            // FILTRAMOS NUESTRA CONSULTA
            var consulta = from datos in this.tablaPlantilla.AsEnumerable() 
                           where datos.Field<int>("EMPLEADO_NO") == empleado_no
                           select datos;
            var fila = consulta.First();
            Plantilla pla = new Plantilla();
            pla.HospitalCod = fila.Field<int>("HOSPITAL_COD");
            pla.EmpleadoNo = fila.Field<int>("EMPLEADO_NO");
            pla.Apellido = fila.Field<string>("APELLIDO");
            pla.Funcion = fila.Field<string>("FUNCION");
            pla.Salario = fila.Field<int>("SALARIO");
            pla.SalaCod = fila.Field<int>("SALA_COD");
            return pla;
        }

        public async Task<int> DeletePlantillaAsync(int empleado_no)
        {
            string sql = "DELETE FROM PLANTILLA WHERE EMPLEADO_NO = @empleado_no";
            this.com.Parameters.AddWithValue("@empleado_no", empleado_no);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            int registros = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return registros;
        }

        public async Task<int> UpsertPlantillaAsync(Plantilla plantilla)
        {
            string sql = "SP_PLANTILLA_UPSERT";
            this.com.Parameters.AddWithValue("@hospital_cod", plantilla.HospitalCod);
            this.com.Parameters.AddWithValue("@sala_cod", plantilla.SalaCod);
            this.com.Parameters.AddWithValue("@empleado_no", plantilla.EmpleadoNo);
            this.com.Parameters.AddWithValue("@apellido", plantilla.Apellido);
            this.com.Parameters.AddWithValue("@funcion", plantilla.Funcion);
            this.com.Parameters.AddWithValue("@salario", plantilla.Salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            int registros = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return registros;
        }

    }
}
