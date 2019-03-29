using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using VERMONT.Web.Models;

namespace VERMONT.Web.Infra
{
    public class TipoContatoRepository : Repositorybase<TipoContato, int>
    {
         
        public override List<TipoContato> GetAll()
        {
            string sql = "Select * FROM TipoContato ORDER BY 1";
            using (var conn = new SqlConnection(StringConnection))
            {
                var cmd = new SqlCommand(sql, conn);
                List<TipoContato> list = new List<TipoContato>();
                TipoContato p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            p = new TipoContato();
                            p.IdTipoContato = (int)reader["IdTipoContato"];
                            p.Descricao = reader["Descricao"].ToString();
                            list.Add(p);
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return list;
            }
        }


        public override TipoContato GetById(int IdTipoContato)
        {
            using (var conn = new SqlConnection(StringConnection))
            {
                string sql = "Select * FROM TipoContato WHERE IdTipoContato=@IdTipoContato";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdTipoContato", IdTipoContato);
                TipoContato p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                p = new TipoContato();
                                p.IdTipoContato = (int)reader["IdTipoContato"];
                                p.Descricao = reader["Descricao"].ToString();
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return p;
            }
        }

        public override int Save(TipoContato entity)
        {
            Int32 newId;
            using (var conn = new SqlConnection(StringConnection))
            {
                string sql = "INSERT INTO TipoContato (Descricao) VALUES (@Descricao) SELECT SCOPE_IDENTITY()";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Descricao", entity.Descricao);
                try
                {
                    conn.Open();
                    newId = (Int32)cmd.ExecuteScalar();
                }
                catch (Exception e)
                {
                    newId = 0;
                    throw e;
                }
            }

            return newId;
        }

        public override void Update(TipoContato entity)
        {
            using (var conn = new SqlConnection(StringConnection))
            {
                string sql = "UPDATE TipoContato SET Descricao=@Descricao Where IdTipoContato=@IdTipoContato";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdTipoContato", entity.IdTipoContato);
                cmd.Parameters.AddWithValue("@Descricao", entity.Descricao);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
        }
    }
}