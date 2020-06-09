using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ProjectViewer
{

    public class UnityFile
    {


        private string path;
        private string extension;




        public UnityFile(string path, UnityFolder parentFolder)
        {
            this.path = path;
            extension = FindExtension(path);





        }

        private string FindExtension(string path)
        {
            string[] splitPath = path.Split('.');
            string ext = splitPath[splitPath.Length - 1];
            return ext;
        }
        public string GetExtension()
        {
            return extension;

        }


    }

}