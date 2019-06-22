using UnityEngine;
using System.Collections;
//============================================================
//@author	清歌与浊酒
//@create	7/13/2018
//
//@description:
//============================================================
namespace Com.Config
{
	public class ConfigInfo : MonoBehaviour {

        #region 
        //数据缓存路径
        /// <summary>
        /// 数据缓存路径
        /// </summary>
        public static string con_token =Application.dataPath+ "/../datadirectories/downCache";
        #endregion

        /// <summary>
        /// 企业信息
        /// </summary>
        public static string EnterpriseInfoUrl = "";
        /// <summary>
        /// 修改联系人信息
        /// </summary>
        public static string ModificationAboutUrl = "";

        /// <summary>
        /// 进程id
        /// </summary>
        /// <returns></returns>
        public static string CourseRuningWindowIdCacheFolderUrl = "";

        /// <summary>
        /// 解压后的资源路径
        /// </summary>
        /// <returns></returns>
        public static string UncompressOutFolderUrl()
        {
            return GetApplicationDataPathParentUrl() + "datadirectories/common/";
        }
       /// <summary>
       /// 压缩包的资源路径
       /// </summary>
       /// <returns></returns>
        public static string downCatchFolderUrl()
        {
            return GetApplicationDataPathParentUrl() + "datadirectories/downCache/";
        }
        public static string GetApplicationDataPathParentUrl()
        {
        
            return Application.dataPath+"/../";
        }




    }
}