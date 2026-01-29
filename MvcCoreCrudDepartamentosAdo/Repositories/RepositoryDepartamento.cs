using Microsoft.Data.SqlClient;
using MvcCoreCrudDepartamentosAdo.Models;
using System.Data;

namespace MvcCoreCrudDepartamentosAdo.Repositories
{
    public class RepositoryDepartamento
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryDepartamento()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public async Task<List<Departamento>> GetDepartamentosAsync()
        {
            string sql = "SELECT * FROM DEPT";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            List<Departamento> departamentos = new List<Departamento>();
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            while (await this.reader.ReadAsync())
            {
                Departamento departamento = new Departamento();
                departamento.IdDepartamento = int.Parse(this.reader["DEPT_NO"].ToString());
                departamento.Nombre = this.reader["DNOMBRE"].ToString();
                departamento.Localidad = this.reader["LOC"].ToString();
                departamentos.Add(departamento);
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            return departamentos;
        }

        public async Task<Departamento> FindDepartamentoAsync(int dept_no)
        {
            string sql = "SELECT * FROM DEPT WHERE DEPT_NO = @dept_no";
            this.com.Parameters.AddWithValue("@dept_no", dept_no);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            Departamento dept = new Departamento();
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            while (await this.reader.ReadAsync())
            {
                dept.IdDepartamento = int.Parse(this.reader["DEPT_NO"].ToString());
                dept.Nombre = this.reader["DNOMBRE"].ToString();
                dept.Localidad = this.reader["LOC"].ToString();
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return dept;
        }

        public async Task DeleteDepartamentoAsync(int idDept)
        {
            string sql = "DELETE FROM DEPT WHERE DEPT_NO=@dept_no";
            this.com.Parameters.AddWithValue("@dept_no", idDept);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task InsertDepartamentoAsync(int dept_no, string nombre, string localidad)
        {
            string sql = "INSERT INTO DEPT VALUES (@dept_no, @nombre, @localidad)";
            this.com.Parameters.AddWithValue("@dept_no", dept_no);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@localidad", localidad);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task UpdateDepartamentoAsync(int dept_no, string nombre, string localidad)
        {
            string sql = "UPDATE DEPT SET DNOMBRE=@nombre, LOC=@localidad where DEPT_NO=@dept_no";
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@localidad", localidad);
            this.com.Parameters.AddWithValue("@dept_no", dept_no);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
    }
}
