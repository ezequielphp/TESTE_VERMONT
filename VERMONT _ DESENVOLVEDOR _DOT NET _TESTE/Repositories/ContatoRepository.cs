using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using VERMONT.Web.Models;

namespace VERMONT.Web.Infra
{
    public class ContatoRepository : Repositorybase<Contato, int>
    {
        private PessoaRepository        _pessoaRepository;
        private TipoContatoRepository _tipoContatoRepository;

        public ContatoRepository()
        {
            _pessoaRepository = new PessoaRepository();
            _tipoContatoRepository = new TipoContatoRepository();
        }



        public List<Contato> FindByFiltro(ContatoFiltro filtro)
        {
            string sql = @" 
                            SELECT C.* 
                                FROM Contato C
                                INNER JOIN Pessoa P ON 
                                    C.IdPessoa = p.IdPessoa
                                WHERE  (@Email = '' OR (InfoContato = @Email AND IdTipoContato = 1)) AND
                                        (@Telefone = '' OR (InfoContato = @Telefone AND IdTipoContato = 1)) AND
                                        (@NomPessoa = '' OR P.Nome = @NomPessoa) 
                                        
                            ORDER BY P.Nome, C.NomeContato
                        ";


            if (string.IsNullOrEmpty(filtro.Email))
            {
                filtro.Email = "";
            }
            if (string.IsNullOrEmpty(filtro.Telefone))
            {
                filtro.Telefone = "";
            }

            if (string.IsNullOrEmpty(filtro.NomePessoa))
            {
                filtro.NomePessoa = "";
            }

            using (var conn = new SqlConnection(StringConnection))
            {
                var cmd = new SqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@Email", filtro.Email);
                cmd.Parameters.AddWithValue("@Telefone", filtro.Telefone);
                cmd.Parameters.AddWithValue("@NomPessoa", filtro.NomePessoa); 

                List<Contato> list = new List<Contato>();
                Contato c = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            c = new Contato();
                            c.IdContato = (int)reader["IdContato"];
                            c.IdPessoa = (int)reader["IdPessoa"];
                            c.IdTipoContato = (int)reader["IdTipoContato"];
                            c.NomeContato = reader["NomeContato"].ToString();
                            c.InfoContato = reader["InfoContato"].ToString();
                            c.Pessoa = _pessoaRepository.GetById(c.IdPessoa);
                            c.TipoContato = _tipoContatoRepository.GetById(c.IdTipoContato);

                            list.Add(c);
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

         


        public override List<Contato> GetAll()
        {
            string sql = "Select * FROM Contato ORDER BY 1";
            using (var conn = new SqlConnection(StringConnection))
            {
                var cmd = new SqlCommand(sql, conn);
                List<Contato> list = new List<Contato>();
                Contato c = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            c = new Contato();
                            c.IdContato = (int)reader["IdContato"];
                            c.IdPessoa = (int)reader["IdPessoa"];
                            c.IdTipoContato = (int)reader["IdTipoContato"];
                            c.NomeContato = reader["NomeContato"].ToString();
                            c.InfoContato = reader["InfoContato"].ToString();
                            c.Pessoa = _pessoaRepository.GetById(c.IdPessoa);
                            c.TipoContato = _tipoContatoRepository.GetById(c.IdTipoContato);

                            list.Add(c);
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

        internal List<Contato> GetContatosByIdPessoa(int idPessoa)
        {
            var retorno = new List<Contato>();

            string sql = "Select * FROM Contato WHERE IdPessoa = @IdPessoa ORDER BY 1";
            using (var conn = new SqlConnection(StringConnection))
            {
                var cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdPessoa", idPessoa);
                List<Contato> list = new List<Contato>();
                Contato c = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        while (reader.Read())
                        {
                            c = new Contato();
                            c.IdContato = (int)reader["IdContato"];
                            c.NomeContato = reader["NomeContato"].ToString();
                            c.InfoContato = reader["InfoContato"].ToString();
                            c.Pessoa = _pessoaRepository.GetById(c.IdPessoa);
                            c.TipoContato = _tipoContatoRepository.GetById(c.IdTipoContato);

                            list.Add(c);
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

        public override Contato GetById(int IdContato)
        {
            using (var conn = new SqlConnection(StringConnection))
            {
                string sql = "Select * FROM Contato WHERE IdContato=@IdContato";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdContato", IdContato);
                Contato c = null;
                try
                {
                    conn.Open();
                    using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                    {
                        if (reader.HasRows)
                        {
                            if (reader.Read())
                            {
                                c = new Contato();
                                c.IdContato = (int)reader["IdContato"];
                                c.IdPessoa = (int)reader["IdPessoa"];
                                c.IdTipoContato = (int)reader["IdTipoContato"];
                                c.NomeContato = reader["NomeContato"].ToString();
                                c.InfoContato = reader["InfoContato"].ToString();
                                c.Pessoa = _pessoaRepository.GetById(c.IdPessoa);
                                c.TipoContato = _tipoContatoRepository.GetById(c.IdTipoContato);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
                return c;
            }
        }

        public override int Save(Contato entity)
        { 
            using (var conn = new SqlConnection(StringConnection))
            {
                Int32 newId;
                string sql = @"     INSERT INTO Contato (idPessoa,nomeContato,infoContato, idTipoContato) 
                                    VALUES (@IdPessoa, @NomeContato, @infoContato, @tipoContato)

                                    SELECT CAST(scope_identity() AS int)
                             ";

                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdPessoa", entity.IdPessoa);
                cmd.Parameters.AddWithValue("@NomeContato", entity.NomeContato);
                cmd.Parameters.AddWithValue("@infoContato", entity.InfoContato);
                cmd.Parameters.AddWithValue("@tipoContato", entity.IdTipoContato); 
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

                return newId;
            }

            
        }

        public override void Update(Contato entity)
        {
            using (var conn = new SqlConnection(StringConnection))
            {
                string sql = @"UPDATE Contato SET 
                                NomeContato=@NomeContato,
                                InfoContato=@InfoContato,
                                IdTipoContato=@IdTipoContato,
                                IdPessoa=@IdPessoa
                                
                            Where IdContato=@IdContato";
                SqlCommand cmd = new SqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@IdContato", entity.IdContato);
                cmd.Parameters.AddWithValue("@NomeContato", entity.NomeContato);
                cmd.Parameters.AddWithValue("@InfoContato", entity.InfoContato);
                cmd.Parameters.AddWithValue("@IdPessoa", entity.IdPessoa);
                cmd.Parameters.AddWithValue("@IdTipoContato", entity.IdTipoContato);
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