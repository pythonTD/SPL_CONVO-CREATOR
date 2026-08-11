using System.Threading.Tasks;
using static ManagerAI.OPEN_AI;

public class ManagerAI_File
{
    public static async Task<string> SEND_PDF_FILE(byte[] pdfBytes, string fileName)
    {
        Request_File result = await UPLOAD_PDF(pdfBytes, fileName);
        if (result != null)
        {
            return result.id;
        }
        return null;
    }
    
    public static async Task DELETE_FILE_WITH_ID(string file_id)
    {
        if (string.IsNullOrEmpty(file_id))
        {
            await DELETE_PDF(file_id);
        }
    }
    
   
}