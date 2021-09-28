using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace BFF.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TableActionAttribute : HttpPostAttribute
    {
        private bool? _showInTable;
        private bool? _showInView;


        public TableActionAttribute()
        {
        }

        /// <summary>
        ///     Action label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        ///     Does support multiple documents
        /// </summary>
        public bool IsMulti { get; set; }

        /// <summary>
        ///     If true, don't allow to perform action on all queried documents,
        ///     But only on selected items
        /// </summary>
        public bool NoQuery { get; set; }

        /// <summary>
        ///     Custom documents filter enabled for this action
        /// </summary>
        public object Filter { get; set; } = null;

        /// <summary>
        ///     If true this action is for the entire collection, not for specific documents
        /// </summary>
        public bool IsGlobal { get; set; }

        public bool ShowInTable
        {
            get => _showInTable ?? true;
            set => _showInTable = value;
        }

        public bool ShowInView
        {
            get => !IsGlobal && (_showInView ?? !IsMulti);
            set => _showInView = value;
        }
        
    }
}