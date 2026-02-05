using Microsoft.Data.SqlClient;
using MvcPracticaFinalPlantilla.Models;
using System.Data;

#region STORED PROCEDURES
/*
-----------------------------------------------------------
--MIRO LA INFORMACION DEL USUARIO CON SU ACTIVIDAD Y TODO--
-----------------------------------------------------------

    CREATE PROCEDURE SP_INFORMACION_USUARIO(@idusuario INT)
    AS
	    DECLARE @idEvAc INT
	    SELECT @idEvAc =  I.IdEventoActividad FROM inscripciones I INNER JOIN USUARIOSTAJAMAR U ON I.id_usuario = U.IDUSUARIO WHERE U.IDUSUARIO= @idusuario
	    DECLARE @nombreAc nvarchar(50)
	    SELECT @nombreAc = A.nombre FROM ACTIVIDADES A INNER JOIN evento_actividades E ON A.id_actividad = E.IdActividad WHERE E.IdEventoActividad = @idEvAc

	    SELECT U.NOMBRE, U.APELLIDOS, U.EMAIL, U.IMAGEN, C.NOMBRE AS CURSO, I.fecha_inscripcion, I.quiere_ser_capitan, @nombreAc AS ACTIVIDAD
	    FROM USUARIOSTAJAMAR U INNER JOIN CURSOSTAJAMAR C ON U.IDCURSO = C.IDCURSO 
	    INNER JOIN inscripciones I ON U.IDUSUARIO = I.id_usuario
	    WHERE U.IDUSUARIO = @idusuario
    GO
 */
#endregion 

namespace MvcPracticaFinalPlantilla.Repositories
{
    public class RepositoryUsuario
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;
        DataTable tablaUsuarios;

        public RepositoryUsuario()
        {
            string connection = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=RETO;Persist Security Info=True;User ID=SA;Trust Server Certificate=True";
            this.cn = new SqlConnection(connection);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "SELECT * FROM USUARIOSTAJAMAR";
            SqlDataAdapter ad = new SqlDataAdapter(sql, this.cn);
            this.tablaUsuarios = new DataTable();
            ad.Fill(tablaUsuarios);
        }

        public List<Usuario> GetUsuarios()
        {
            var consulta = from datos in tablaUsuarios.AsEnumerable()
                           select datos;
            List<Usuario> usuarios = new List<Usuario>();
            foreach (var fila in consulta)
            {
                Usuario usuario = new Usuario
                {
                    IdUsuario = fila.Field<int>("IDUSUARIO"),
                    Nombre = fila.Field<string>("NOMBRE"),
                    Apellidos = fila.Field<string>("APELLIDOS"),
                    Email = fila.Field<string>("EMAIL"),
                    Imagen = fila.Field<string>("IMAGEN"),
                    IdCurso = fila.Field<int>("IDCURSO"),
                };
                usuarios.Add(usuario);
            }
            return usuarios;
        }

        public async Task<InformacionUsuario> GetInformacionUsuario(int idUsuario)
        {
            string sql = "SP_INFORMACION_USUARIO";
            this.com.Parameters.AddWithValue("@idusuario", idUsuario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            InformacionUsuario info = new InformacionUsuario();
            this.reader = await this.com.ExecuteReaderAsync();
            while(await this.reader.ReadAsync())
            {
                info.Nombre = this.reader["NOMBRE"].ToString();
                info.Apellidos = this.reader["APELLIDOS"].ToString();
                info.Email = this.reader["EMAIL"].ToString();
                info.Imagen = this.reader["IMAGEN"].ToString();
                info.Curso = this.reader["CURSO"].ToString();
                info.Actividad = this.reader["ACTIVIDAD"].ToString();
                info.Fecha = DateTime.Parse(this.reader["fecha_inscripcion"].ToString());
                info.Capitan = Boolean.Parse(this.reader["quiere_ser_capitan"].ToString());
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return info;
        }
    }
}
