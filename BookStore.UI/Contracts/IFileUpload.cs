namespace BookStore.UI.Contracts
{
    using BlazorInputFile;
    using System.IO;
    using System.Threading.Tasks;
    public interface IFileUpload
    {
        public Task UploadFile(IFileListEntry file, string picName);
        public void UploadFile(IFileListEntry file, MemoryStream msFile, string picName);
        public void RemoveFile(string picName);
    }
}
