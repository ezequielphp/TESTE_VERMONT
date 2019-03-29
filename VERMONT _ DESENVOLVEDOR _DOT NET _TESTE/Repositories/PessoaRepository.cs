using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient; 
using VERMONT.Web.Models;

namespace VERMONT.Web.Infra
{
    public class PessoaRepository : Repositorybase<Pessoa, int>
    { 
        
        public override List<Pessoa> GetAll()
        {
            string sql = "Select * FROM Pessoa ORDER BY Nome";
            using (var conn = new SqlConnection(StringConnection))
            {
                var cmd = new SqlCommand(sql, conn);
                List<Pessoa> list = new List<Pessoa>();
                Pessoa p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            p = new Pessoa();
                            p.IdPessoa = (int)reader["IdPessoa"];
                            p.Nome = reader["Nome"].ToString();

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

 
        public override Pessoa GetById(int IdPessoa)
        {
            using (var conn = new SqlConnection(StringConnection))
            {
                string sql = "Select IdPessoa, Nome FROM Pessoa WHERE IdPessoa=@IdPessoa";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdPessoa", IdPessoa);
                Pessoa p = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                p = new Pessoa();
                                p.IdPessoa = (int)reader["IdPessoa"];
                                p.Nome = reader["Nome"].ToString(); 
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
         
        public override int Save(Pessoa entity)
        {
            using (var conn = new SqlConnection(StringConnection))
            {
                int newId;
                string sql = "INSERT INTO Pessoa (Nome) VALUES (@Nome) SELECT CAST(scope_identity() AS int)";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Nome", entity.Nome); 
                try
                {
                    conn.Open();
                    newId = (Int32)cmd.ExecuteScalar();

                    sql = @"
                                INSERT INTO Contato (idPessoa,nomeContato,infoContato, idTipoContato) 
                                VALUES (@IdPessoa, @NomeContato, @infoContato, @tipoContato)
                            ";

                    cmd = new SqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("@IdPessoa", newId);
                    cmd.Parameters.AddWithValue("@NomeContato", entity.NomeContato);
                    cmd.Parameters.AddWithValue("@infoContato", entity.InfoContato);
                    cmd.Parameters.AddWithValue("@tipoContato", entity.IdTipoContato);

                    cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    newId = 0;
                    throw e;
                     
                } 

                return newId;
            }
        }
         
        public override void Update(Pessoa entity)
        {
            using (var conn = new SqlConnection(StringConnection))
            {
                string sql = "UPDATE Pessoa SET Nome=@Nome Where IdPessoa=@IdPessoa";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdPessoa", entity.IdPessoa);
                cmd.Parameters.AddWithValue("@Nome", entity.Nome); 
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