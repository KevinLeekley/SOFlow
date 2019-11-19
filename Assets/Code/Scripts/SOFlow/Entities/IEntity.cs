// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

namespace SOFlow.Entities
{
	/// <summary>
	///     The basic entity structure definition.
	/// </summary>
	public interface IEntity
    {
	    /// <summary>
	    ///     The name of the entity.
	    /// </summary>
	    string Name{ get; set; }
    }
}