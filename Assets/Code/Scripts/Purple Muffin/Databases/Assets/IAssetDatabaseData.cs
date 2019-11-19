// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

namespace PurpleMuffin.Databases
{
    public interface IAssetDatabaseData
    {
        /// <summary>
        ///     Returns the asset ID for this data asset.
        /// </summary>
        /// <returns></returns>
        string GetAssetID();
    }
}