using Microsoft.Data.SqlClient;
using MvcCoreLinqToSql.Models;
using System.Data;

namespace MvcCoreLinqToSql.Repositories
{
    public class RepositoryEnfermos
    {
        SqlConnection cn;
        SqlCommand com;
        DataTable tablaEnfermos;

        public RepositoryEnfermos()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "SELECT * FROM ENFERMO";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            this.tablaEnfermos = new DataTable();
            ad.Fill(this.tablaEnfermos);
        }

        public List<Enfermo> GetEnfermos()
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           select datos;
            if(consulta.Count() == 0)
            {
                return null;
            }
            List<Enfermo> enfermos = new List<Enfermo>();
            foreach(var fila in consulta)
            {
                Enfermo enfermo = new Enfermo
                {
                    Inscripcion = fila.Field<string>("INSCRIPCION"),
                    Apellido = fila.Field<string>("APELLIDO"),
                    Direccion = fila.Field<string>("DIRECCION"),
                    Fecha_Nac = fila.Field<DateTime>("FECHA_NAC"),
                    Sexo = fila.Field<string>("S"),
                    NSS = fila.Field<string>("NSS"),
                };
                enfermos.Add(enfermo);
            }
            return enfermos;
        }

        public Enfermo GetEnfermoDetails(string inscripcion)
        {
            var consulta = from datos in this.tablaEnfermos.AsEnumerable()
                           where datos.Field<string>("INSCRIPCION") == inscripcion
                           select datos;
            if(consulta.Count() == 0)
            {
                return null;
            }
            Enfermo enf = new Enfermo();
            foreach(var fila in consulta)
            {
                enf.Inscripcion = fila.Field<string>("INSCRIPCION");
                enf.Apellido = fila.Field<string>("APELLIDO");
                enf.Direccion = fila.Field<string>("DIRECCION");
                enf.Fecha_Nac = fila.Field<DateTime>("FECHA_NAC");
                enf.Sexo = fila.Field<string>("S");
                enf.NSS = fila.Field<string>("NSS");
            }
            return enf;
        }

        public int DeleteEnfermo(string inscripcion)
        {
            string sql = "DELETE FROM ENFERMO WHERE INSCRIPCION = @inscripcion";
            this.com.Parameters.AddWithValue("@inscripcion", inscripcion);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            this.cn.Open();
            int registros = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
            return registros;
        }
    }
}
