/*   
    Copyright (C) 2009 Galaktika Corporation ZAO

    This file is a part of Ranet.UILibrary.Olap
 
    Ranet.UILibrary.Olap is a free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.
      
    You should have received a copy of the GNU General Public License
    along with Ranet.UILibrary.Olap.  If not, see
  	<http://www.gnu.org/licenses/> 
  
    If GPL v.3 is not suitable for your products or company,
    Galaktika Corp provides Ranet.UILibrary.Olap under a flexible commercial license
    designed to meet your specific usage and distribution requirements.
    If you have already obtained a commercial license from Galaktika Corp,
    you can use this file under those license terms.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using System.IO;

namespace Ranet.Olap.Core.Storage
{
    public class FileStorageProvider : IStorageProvider
    {
        #region IStorageProvider Members

        public void Save(IPrincipal currentPrincipal, string name, string content)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            string folder = string.Empty;
            if (GetFolderForPrincipal(currentPrincipal, ref folder))
            {
                string path = Path.Combine(folder, name);

                using (var sw = new StreamWriter(File.OpenWrite(path)))
                {
                    sw.Write(content);
                    sw.Close();
                }
            }
        }

        public string Load(IPrincipal currentPrincipal, string name)
        {
            String result = String.Empty;

            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            string folder = string.Empty;
            if (GetFolderForPrincipal(currentPrincipal, ref folder))
            {
                string path = Path.Combine(folder, name);

                if (File.Exists(path))
                {
                    using (var sr = new StreamReader(File.OpenRead(path)))
                    {
                        result = sr.ReadToEnd();
                        sr.Close();
                    }
                }
            }

            return result;
        }

        #endregion

        public List<ObjectStorageFileDescription> GetList(IPrincipal currentPrincipal, string mask)
        {
            List<ObjectStorageFileDescription> result = new List<ObjectStorageFileDescription>();

            if (string.IsNullOrEmpty(mask))
                throw new ArgumentNullException("mask");

            string folder = string.Empty;
            if (GetFolderForPrincipal(currentPrincipal, ref folder))
            {
                String[] files = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly);
                if (files != null)
                {
                    for (int i = 0; i < files.Length; i++)
                    {
                        String file = files[i];
                        if (!file.Contains(".content."))
                        {
                            string path = Path.Combine(folder, file);
                            if (File.Exists(path))
                            {
                                using (var sr = new StreamReader(File.OpenRead(path)))
                                {
                                    string str = sr.ReadToEnd();
                                    sr.Close();

                                    if (!String.IsNullOrEmpty(str))
                                    {
                                        // Пытаемся преобразовать в описатель
                                        ObjectStorageFileDescription descr = XmlSerializationUtility.XmlStr2Obj<ObjectStorageFileDescription>(str);
                                        if (descr != null)
                                        {
                                            // Если файла с содержимым не существует, то имя файла скидываем
                                            if(!String.IsNullOrEmpty(descr.ContentFileName))
                                            {
                                                string content_path = Path.Combine(folder, descr.ContentFileName);
                                                if(!File.Exists(content_path))
                                                    descr.ContentFileName = String.Empty;
                                            }
                                            result.Add(descr);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        private bool GetFolderForPrincipal(IPrincipal principal, ref string folder)
        {
            bool res = false;
            if (principal == null || principal.Identity == null)
            {
                return res;
            }

            try
            {
                String name = "SYSTEM";
                if (!String.IsNullOrEmpty(principal.Identity.Name))
                    name = principal.Identity.Name;
                string path = Path.Combine(UserStorageBase, name);//.Replace(Path.PathSeparator, ','));
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                folder = path;
                res = true;
            }
            catch (IOException)
            {
            }
            catch (UnauthorizedAccessException)
            {
            }

            return res;
        }

        private string UserStorageBase
        {
            get
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "UserStorage");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            }
        }
        
        public static String GetFilteExtension(StorageContentTypes contentType)
        {
            switch (contentType)
            {
                case StorageContentTypes.MdxDesignerLayout:
                    return "mdl";
                case StorageContentTypes.ValueCopySettings:
                    return "vcs";
                default:
                    return "xml";
            }
        }
    }
}
