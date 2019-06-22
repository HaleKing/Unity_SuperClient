using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
//============================================================
//@author	清歌与浊酒
//@create	7/16/2018
//
//@description:
//============================================================
namespace Com.Datafile
{
	public class ZIP  {

        private static ZIP instance;
        public static ZIP Instance
        {
            get
            {
                if (instance == null)
                    instance = new ZIP();
                return instance;
            }
        }

        private event Action<bool> _eventCompressing = null;//压缩完成回调
        private event Action<string> _eventPrompt = null;//压缩提示
        public void CompressingBack(Action<bool> back)
        {
            if (back != null)
                _eventCompressing += back;

        }
        public void CompressiPrompt (Action<string> back)
        {
            if (back != null)
                _eventPrompt += back;
        }

        private int totalCount;
        private int doneCount;
        private int indicatorStep = 1;
        public IEnumerator UnzipWithPath(string path, string dirPath, Action<bool> callback = null)
        {

            //将codepage编码设置对应的字符编码
            ZipConstants.DefaultCodePage = Encoding.UTF8.CodePage;
            //这是根目录的路径  
            //ZipEntry：文件条目 就是该目录下所有的文件列表(也就是所有文件的路径)  
            ZipEntry zip = null;
            //输入的所有的文件流都是存储在这里面的  
            ZipInputStream zipInStream = null;
            //读取文件流到zipInputStream  
            zipInStream = new ZipInputStream(File.OpenRead(path));
            bool isError = false;
            while ((zip = zipInStream.GetNextEntry()) != null)
            {
        
                bool error = UnzipFile(zip, zipInStream, dirPath);
                if (error)
                {
              
                    if (_eventPrompt != null)
                        _eventPrompt.Invoke("error:错误！！！！");
                    if (callback != null)
                        callback(false);
                    isError = true;
                    break;
                }
                doneCount++;
                if (doneCount % indicatorStep == 0)
                    yield return new WaitForEndOfFrame();
            }
            try
            {
                zipInStream.Close();
            }
            catch (Exception ex)
            {
                if (_eventPrompt != null)
                    _eventPrompt.Invoke("UnZip Error");

                throw ex;
            }
            if (!isError)
            {
                if (callback != null)
                    callback(true);
                //解压完成后删除zip
                File.Delete(path);

                if (ZIP.Instance._eventPrompt != null)
                    ZIP.Instance._eventPrompt.Invoke("解压完成");
                if (ZIP.Instance._eventCompressing != null)
                    ZIP.Instance._eventCompressing.Invoke(true);
            }
        }
        static bool UnzipFile(ZipEntry zip, ZipInputStream zipInStream, string dirPath)
        {
            try
            {
                //文件名不为空  
                if (!string.IsNullOrEmpty(zip.Name))
                {
                    string filePath = dirPath;
                    filePath += ("/" + zip.Name);


            
                    //    UIUtils.LogError("删除更新包资源");
                    filePath = filePath.Replace("//", "/");
                    if (ZIP.Instance._eventPrompt != null)
                        ZIP.Instance._eventPrompt.Invoke(zip.Name);         
                    //如果是一个新的文件路径　这里需要创建这个文件路径  
                    if (IsDirectory(filePath))
                    {//是文件夹则不需要处理
                        if (!Directory.Exists(filePath)) //没有则夹创建文件夹
                        {

                    
                            Directory.CreateDirectory(filePath);
                        }
           
                    }
                    else
                    {
                        //找到文件夹
                        string directory = GetDirectory(filePath);
                        if (ZIP.Instance._eventPrompt != null)
                            ZIP.Instance._eventPrompt.Invoke("得到文件");
                        if (!string.IsNullOrEmpty(filePath))
                        {//检测是否有文件夹
                            if (!Directory.Exists(directory)) //没有则夹创建文件夹
                            {
                            
                                Directory.CreateDirectory(directory);
                            }
                        }
                        FileStream fs = null;
                        //当前文件夹下有该文件  删掉  重新创建  

                        if (ZIP.Instance._eventPrompt != null)
                            ZIP.Instance._eventPrompt.Invoke("检测文件");
                        if (File.Exists(filePath))
                        {
             
                            if (ZIP.Instance._eventPrompt != null)
                                ZIP.Instance._eventPrompt.Invoke("Delete Files:::");
                            File.Delete(filePath);
                        }
                      
                        if (ZIP.Instance._eventPrompt != null)
                         ZIP.Instance._eventPrompt.Invoke("创建文件");
                        fs = File.Create(filePath);
                        int size = 2048;
                        byte[] data = new byte[2048];
                        //每次读取2MB  直到把这个内容读完  
                        while (true)
                        {
                            size = zipInStream.Read(data, 0, data.Length);
                            //小于0， 也就读完了当前的流  
                            if (size > 0)
                            {
                                fs.Write(data, 0, size);
                            }
                            else
                            {
                                break;
                            }
                        }
                        fs.Close();
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                Debug.LogError(e);
                return true;
            }
        }


        /// <summary>  
        /// 目录文件  
        /// </summary>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        static string GetDirectory(string path)
        {
            for (int i = path.Length - 1; i >= 0; --i)
            {
                if (path[i] == '/')
                {
                    string stringPath = new string(path.ToCharArray(), 0, i);
                    return stringPath.ToString();
                }
            }
            return null;
        }
        /// <summary>  
        /// 是否是目录文件夹  
        /// </summary>  
        /// <param name="path"></param>  
        /// <returns></returns>  
        static bool IsDirectory(string path)
        {
            if (path[path.Length - 1] == '/')
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 压缩
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string CompressStringToString(string param)
        {
            byte[] data = Encoding.UTF8.GetBytes(param);
            return CompressByteToString(data);
        }
        public static string CompressByteToString(byte[] inputBytes)
        {
            return Convert.ToBase64String(CompressByteToByte(inputBytes));
        }
        public static byte[] CompressByteToByte(byte[] inputBytes)
        {
            MemoryStream ms = new MemoryStream();
            Stream stream = new ICSharpCode.SharpZipLib.BZip2.BZip2OutputStream(ms);
            try
            {
                stream.Write(inputBytes, 0, inputBytes.Length);
            }
            finally
            {
                stream.Close();
                ms.Close();
            }
            return ms.ToArray();
        }
        /// <summary>
        /// 解压
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string DecompressStringToString(string param)
        {
            byte[] buffer = Convert.FromBase64String(param);
            return DecompressByteToString(buffer);
        }
        public static string DecompressByteToString(byte[] inputBytes)
        {
            string commonString = "";
            MemoryStream ms = new MemoryStream(inputBytes);
            Stream sm = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(ms);
            //这里要指明要读入的格式，要不就有乱码
            StreamReader reader = new StreamReader(sm, Encoding.UTF8);
            try
            {
                commonString = reader.ReadToEnd();
            }
            finally
            {
                sm.Close();
                ms.Close();
            }
            return commonString;
        }
        public static byte[] DecompressByteToByte(byte[] inputBytes)
        {
            MemoryStream ms = new MemoryStream(inputBytes);
            Stream sm = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(ms);
            byte[] data = new byte[sm.Length];
            int count = 0;
            MemoryStream re = new MemoryStream();
            while ((count = sm.Read(data, 0, data.Length)) != 0)
            {
                re.Write(data, 0, count);
            }
            sm.Close();
            ms.Close();
            return re.ToArray();
        }
        public static byte[] DecompressStringToByte(string param)
        {
            byte[] buffer = Convert.FromBase64String(param);
            MemoryStream ms = new MemoryStream(buffer);
            Stream sm = new ICSharpCode.SharpZipLib.BZip2.BZip2InputStream(ms);
            byte[] data = new byte[sm.Length];
            int count = 0;
            MemoryStream re = new MemoryStream();
            //这里要指明要读入的格式，要不就有乱码
            StreamReader reader = new StreamReader(sm, Encoding.UTF8);
            try
            {
                while ((count = sm.Read(data, 0, data.Length)) != 0)
                {
                    re.Write(data, 0, count);
                }
            }
            finally
            {
                sm.Close();
                ms.Close();
            }
            return re.ToArray();
        }
    }
}