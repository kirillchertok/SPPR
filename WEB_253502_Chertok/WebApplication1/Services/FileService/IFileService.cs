namespace WEB_253502_Chertok.Services.FileService
{
	public interface IFileService
	{
		Task<string> SaveFileAsync(IFormFile formFile);
		Task DeleteFileAsync(string fileName);
	}
}
