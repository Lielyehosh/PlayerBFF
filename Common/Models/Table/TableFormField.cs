﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Common.Models.Table
{
    public class TableFormField
    {
        [JsonIgnore]
        public string ViewModelId { get; set; }

        [JsonIgnore]
        public string DbModelId { get; set; }

        [Required]
        public string Id { get; set; }

        [Required]
        public string Label { get; set; }
        
        [Required]
        public TableFieldType Type { get; set; }
        
        public bool Required { get; set; }
        
        
        public bool Hidden { get; set; }

        public bool ReadOnly { get; set; }
        
        public List<TableFieldChoice> Choices { get; set; }
        
        [JsonIgnore]
        public Type EnumType { get; set; }
    }
}