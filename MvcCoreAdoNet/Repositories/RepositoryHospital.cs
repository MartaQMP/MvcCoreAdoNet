using Microsoft.Data.SqlClient;
using MvcCoreAdoNet.Models;
using System.Data;

namespace MvcCoreAdoNet.Repositories
{
    public class RepositoryHospital
    {
        SqlConnection cn;
        SqlCommand com;
        SqlDataReader reader;

        public RepositoryHospital()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
        }

        public async Task<List<Hospital>> GetHospitalesAsync()
        {
            string sql = "SELECT * FROM HOSPITAL";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            List<Hospital> hospitales = new List<Hospital>();
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            while(await this.reader.ReadAsync())
            {
                Hospital hospital = new Hospital();
                hospital.IdHospital = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                hospital.Nombre = this.reader["NOMBRE"].ToString();
                hospital.Direccion = this.reader["DIRECCION"].ToString();
                hospital.Telefono = this.reader["TELEFONO"].ToString();
                hospital.Camas = int.Parse(this.reader["NUM_CAMA"].ToString());
                hospitales.Add(hospital);
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            return hospitales;
        }

        public async Task<Hospital> FindHospitalAsync(int idHospital)
        {
            string sql = "SELECT * FROM HOSPITAL WHERE HOSPITAL_COD = @idHospital";
            this.com.Parameters.AddWithValue("@idHospital", idHospital);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            Hospital hospital = new Hospital();
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            while(await this.reader.ReadAsync())
            {
                hospital.IdHospital = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                hospital.Nombre = this.reader["NOMBRE"].ToString();
                hospital.Direccion = this.reader["DIRECCION"].ToString();
                hospital.Telefono = this.reader["TELEFONO"].ToString();
                hospital.Camas = int.Parse(this.reader["NUM_CAMA"].ToString());
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            return hospital;
        }

        public async Task DeleteHospitalAsync(int idHospital)
        {
            string sql = "delete from HOSPITAL where HOSPITAL_COD=@hospitalcod";
            this.com.Parameters.AddWithValue("@hospitalcod", idHospital);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task InsertHospitalAsync(int idHospital, string nombre, string direccion, string telefono, int camas)
        {
            string sql = "insert into HOSPITAL values (@hospitalcod, @nombre, @direccion, @telefono,@camas)";
            this.com.Parameters.AddWithValue("@hospitalcod", idHospital);
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@direccion", direccion);
            this.com.Parameters.AddWithValue("@telefono", telefono);
            this.com.Parameters.AddWithValue("@camas", camas);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task UpdateHospitalAsync(int idHospital, string nombre, string direccion, string telefono, int camas)
        {
            string sql = "update HOSPITAL set NOMBRE=@nombre, DIRECCION=@direccion, TELEFONO=@telefono, NUM_CAMA=@camas where HOSPITAL_COD=@hospitalcod";
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@direccion", direccion);
            this.com.Parameters.AddWithValue("@telefono", telefono);
            this.com.Parameters.AddWithValue("@camas", camas);
            this.com.Parameters.AddWithValue("@hospitalcod", idHospital);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }

        public async Task<List<Doctor>> GetDoctoresAsync()
        {
            string sql = "SELECT * FROM DOCTOR";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            List<Doctor> doctores = new List<Doctor>();
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            while (await this.reader.ReadAsync())
            {
                Doctor doctor = new Doctor();
                doctor.IdDoctor = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doctor.Apellido = this.reader["APELLIDO"].ToString();
                doctor.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doctor.Salario = int.Parse(this.reader["SALARIO"].ToString());
                doctor.idHospital = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doctores.Add(doctor);
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            return doctores;
        }
        public async Task<List<Doctor>> GetDoctoresEspecialidadAsync(string especialidad)
        {
            string sql = "SELECT * FROM DOCTOR WHERE ESPECIALIDAD = @especialidad";
            this.com.Parameters.AddWithValue("@especialidad", especialidad);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            List<Doctor> doctores = new List<Doctor>();
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            while (await this.reader.ReadAsync())
            {
                Doctor doctor = new Doctor();
                doctor.IdDoctor = int.Parse(this.reader["DOCTOR_NO"].ToString());
                doctor.Apellido = this.reader["APELLIDO"].ToString();
                doctor.Especialidad = this.reader["ESPECIALIDAD"].ToString();
                doctor.Salario = int.Parse(this.reader["SALARIO"].ToString());
                doctor.idHospital = int.Parse(this.reader["HOSPITAL_COD"].ToString());
                doctores.Add(doctor);
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return doctores;
        }

        public async Task<List<string>> GetEspecialidadesAsync()
        {
            string sql = "SELECT DISTINCT ESPECIALIDAD FROM DOCTOR";
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            List<string> especialidades = new List<string>();
            await this.cn.OpenAsync();
            this.reader = await this.com.ExecuteReaderAsync();
            while (await this.reader.ReadAsync())
            {
                especialidades.Add(this.reader["ESPECIALIDAD"].ToString());
            }
            await this.reader.CloseAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return especialidades;
        }
    }
}
