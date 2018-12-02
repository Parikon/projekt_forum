using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

namespace proj_forum
{
    class Base
    {
        // zmienna path nie jest potrzebna w zadnej z metod
        // można ją spokojnie przenieść do konstruktora jako zmienną prywatną
        // ale może się przyda więc zostaje
        private string path;
        // do połączenia wystarczy db_con ustawiony raz w konstruktorze 
        private SQLiteConnection db_con;


        /// <summary>
        /// Konstruktor klasy. Pobiera ścieżkę do bazy.
        /// </summary>
        /// <param name="_path"></param>

        public Base(string _path)
        {
            this.path = _path + "\\Bazy";
            string sciezkaBaza = this.path + "\\projekty.sqlite";
            this.db_con = new SQLiteConnection($"Data Source = {sciezkaBaza}; Version = 3");
        }

        public interface IBase
        {
            bool AddBaseIfNotExist();
            int GetBaseVersion();
        }
        /// <summary>
        /// Tworzy bazę jeśli nie istnieje i tabelę projekt_lista
        /// </summary>
        public void AddBaseIfNotExist()
        {

            if (!Directory.Exists(this.path))
            {
                Directory.CreateDirectory(this.path);
            }
            this.db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = System.Data.CommandType.Text;
            db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS projekt_lista ([Numer projektu]  INT, [Nazwa projektu]  TEXT, [Data dodania] TEXT, [Data zakończenia] TEXT, [Ścieżka] TEXT)";
            db_cmd.ExecuteNonQuery();
            db_cmd.CommandText = "CREATE TABLE IF NOT EXISTS wersja_bazy ([Numer wersji]  INT)";
            db_cmd.ExecuteNonQuery();
            this.db_con.Close();
        }

        public int GetBaseVersion()

        {
            int wersja = 1;

            this.db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = CommandType.Text;
            db_cmd.CommandText = "select * from wersja_bazy";
            db_cmd.ExecuteNonQuery();
            var dt = new DataTable();
            SQLiteDataAdapter da = new SQLiteDataAdapter(db_cmd);
            da.Fill(dt);
            int wierszy = dt.Rows.Count;
            if (wierszy == 0)
            {
                db_cmd.CommandText = "INSERT INTO wersja_bazy VALUES ('" + wersja + "')";
                db_cmd.ExecuteNonQuery();
                this.db_con.Close();
            }
            else
            {
                this.db_con.Close();
                DataRow dtr = dt.Rows[0];
                wersja = Convert.ToInt16(dtr[0]);
            }

            return wersja; // metoda zwróci wersję
        }

        public DataTable PobierzDaneTabeli(string nazwa_tabeli)
        {
            var dt = new DataTable();
            this.db_con.Open();
            SQLiteCommand db_cmd = db_con.CreateCommand();
            db_cmd.CommandType = CommandType.Text;
            db_cmd.CommandText = "select * from " + nazwa_tabeli + "";
            db_cmd.ExecuteNonQuery();
            SQLiteDataAdapter da = new SQLiteDataAdapter(db_cmd);
            da.Fill(dt);
            this.db_con.Close();
            return dt;
        }

    }
}
