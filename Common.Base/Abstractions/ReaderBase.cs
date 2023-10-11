using Common.Base.Interfaces;

namespace Common.Base.Abstractions
{
    public abstract class ReaderBase
    {
        #region Properties

        public abstract string FileName { get; set; }

        public abstract required string FileExtension { get; set; }

        #endregion

        #region Constructors

        protected ReaderBase() : base()
        {
        }

        #endregion

        #region Methods

        public abstract Task<IReaderSupport?> StartupAsync();

        public abstract string OpenTargetFile(string? fileExtension, out bool result);

        public abstract Task<IList<string>> ReadFileAsync();

        public abstract Task<IList<ICoordinatesCollection>> DataCollectionInitialization(
            IEnumerable<string> pointsCollection);

        #endregion
    }
}