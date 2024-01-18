using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace pairs.Classes
{
    [Serializable]
    internal class Avatar : ImageSource
    {
        // Public static members:
        public static readonly string PATH_TO_AVATARS = @"Images\Avatars";
        public static readonly string FULL_PATH_TO_AVATARS = FilePath.PATH_RESOURCES + PATH_TO_AVATARS;
        public static readonly string NULL_AVATAR_IMAGE_NAME = "question_mark.png";

        // Constructors:
        public Avatar(string imageName) : base(PATH_TO_AVATARS, imageName, NULL_AVATAR_IMAGE_NAME)
        {
            /* Empty */
        }
        public Avatar() : base(PATH_TO_AVATARS, NULL_AVATAR_IMAGE_NAME)
        {
            /* Empty */
        }

        // Properties:
        public override bool Initialized
        {
            get
            {
                return ImageName != NULL_AVATAR_IMAGE_NAME;
            }
        }

        // Methods:
        public void Randomize()
        {
            string newName = ImageName;
            Random random = new Random();
            List<string> names = SingletonNameLoader.Instance.Avatars;
            while (newName == ImageName)
            {
                newName = names[random.Next(names.Count)];
            }
            ImageName = newName;
        }
    }
}
