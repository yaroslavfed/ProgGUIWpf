using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.GUIWpf.Infrastructures.Interfaces;
using Application.GUIWpf.Models;

namespace Application.GUIWpf.Infrastructures.Abstractions
{
    public abstract class Reader
    {
        #region Properties

        public abstract string FileName { get; set; }

        public abstract required string FileExtension { get; set; }

        #endregion

        #region Constructors

        protected Reader() : base()
        {
        }

        #endregion

        #region Methods

        public abstract Task<DataLocation?> StartupAsync();

        public abstract string OpenTargetFile(string? fileExtension, out bool result);

        public abstract Task<IList<string>> ReadFileAsync();

        public abstract Task<IList<ICoordinatesCollection>> DataCollectionInitialization(
            IEnumerable<string> pointsCollection);

        #endregion
    }
}