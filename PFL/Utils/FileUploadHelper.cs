using System.Web;

namespace PFL.Utils
{
    public class FileUploadHelper
    {
        public static FileCheckResponseType CheckPDF(HttpPostedFileBase upload)
        {

            if (upload.ContentType != "application/pdf")
                return FileCheckResponseType.TypeError;
                                       
            if (upload.ContentLength > 25000000)
                return FileCheckResponseType.SizeError;

            return FileCheckResponseType.OK;
        }

        public static FileCheckResponseType CheckPDF(HttpPostedFileBase upload, int fileSize)
        {

            if (upload.ContentType != "application/pdf")
                return FileCheckResponseType.TypeError;

            if (upload.ContentLength > fileSize)
                return FileCheckResponseType.SizeError;

            return FileCheckResponseType.OK;
        }

        public static FileCheckResponseType CheckImage(HttpPostedFileBase upload)
        {
            if (upload.ContentType != "image/jpeg" && upload.ContentType != "image/png")
                return FileCheckResponseType.TypeError;

            if (upload.ContentLength > 5000000)
                return FileCheckResponseType.SizeError;

            return FileCheckResponseType.OK;
        }


        public static FileCheckResponseType CheckImage(HttpPostedFileBase upload, int fileSize)
        {
            if (upload.ContentType != "image/jpeg" && upload.ContentType != "image/png")
                return FileCheckResponseType.TypeError;

            if (upload.ContentLength > fileSize)
                return FileCheckResponseType.SizeError;

            return FileCheckResponseType.OK;
        }


    }
    public enum FileCheckResponseType
    {
        OK=1,
        TypeError=2,
        SizeError=3
    }
}