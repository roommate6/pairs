using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pairs.Classes.Normal
{
    internal class Decoration : ImageSource
    {
        // Public static members:
        public static readonly string PATH_TO_DECORATIONS = @"Images\Decorations";
        public static readonly string FULL_PATH_TO_DECORATIONS = FilePath.PATH_RESOURCES + PATH_TO_DECORATIONS;
        public static readonly string NULL_DECORATION_IMAGE_NAME = "null_decoration.png";

        // Constructors:
        public Decoration(string imageName) : base(PATH_TO_DECORATIONS, imageName, NULL_DECORATION_IMAGE_NAME)
        {
            /* Empty */
        }

        public Decoration() : base(PATH_TO_DECORATIONS, NULL_DECORATION_IMAGE_NAME)
        {
            /* Empty */
        }

        // Properties:
        public override bool Initialized
        {
            get
            {
                return ImageName != NULL_DECORATION_IMAGE_NAME;
            }
        }
    }
}
