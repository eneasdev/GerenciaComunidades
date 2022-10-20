﻿using System.Text.Json.Serialization;
using Novo.Models.Enums;

namespace Novo.Models
{
    public class Item
    {
        public Item(string descricao, Status status)
        {
            Descricao = descricao;
            Status = status;
        }

        public int IdItem { get; set; }
        public string Descricao { get; set; }
        public Status Status { get; set; }


        public int IdAmbiente { get; set; }
        [JsonIgnore]
        public virtual Ambiente Ambiente { get; set; }
    }
}