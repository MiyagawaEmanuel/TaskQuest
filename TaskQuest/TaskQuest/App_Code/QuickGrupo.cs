using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskQuest.App_Code
{
    public class QuickGrupo
    {

        public QuickGrupo()
        {
            DataCriacao = DateTime.Now;
        }

        public int Id
        {
            //Id está setado como AUTO_INCREMENT no banco de dados
            get { return Id; }
        }

        public string Nome
        {
            get { return Nome; }
            set
            {
                if(value.Length > 0 && value.Length <= 20)
                    Nome = value;
            }
        }

        public string Cor
        {
            get { return Cor; }
            set
            {
                if(value.Length == 7)
                Cor = value;
            }
        }

        public DateTime DataCriacao
        {
            get { return DataCriacao;  }
            private set { DataCriacao = value; }
        }

        public bool Plano
        {
            get { return Plano; }
            set { Plano = value; }
        }

        public void Create() { }
        public void Read(int Quantity) { }
        public void Update() { }
        public void Delete() { }

    }
}