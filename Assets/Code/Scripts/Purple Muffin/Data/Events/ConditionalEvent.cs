// Created by Kearan Petersen : https://www.blumalice.wordpress.com | https://www.linkedin.com/in/kearan-petersen/

using System;

namespace PurpleMuffin.Data.Events
{
    [Serializable]
    public class ConditionalEvent : SerializableCallback<bool>
    {
    }
}